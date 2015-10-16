namespace Chartect.IO.Core
{
    using System;

    internal enum InputState
    {
        PureASCII = 0,
        EscASCII = 1,
        Highbyte = 2,
    }

    public abstract class UniversalDetector
    {
        protected const int FilterChineseSimplified = 1;
        protected const int FilterChineseTraditional = 2;
        protected const int FilterJapanese = 4;
        protected const int FilterKorean = 8;
        protected const int FilterNonCJK = 16;
        protected const int FilterAll = 31;
        protected const int FilterChinese = FilterChineseSimplified | FilterChineseTraditional;
        protected const int FilterCJK = FilterJapanese | FilterKorean | FilterChineseSimplified | FilterChineseTraditional;

        protected const float SHORTCUTTHRESHOLD = 0.95f;
        protected const float MINIMUMTHRESHOLD = 0.20f;
        protected const int ProbersNum = 3;

        private InputState inputState;
        private bool start;
        private bool gotData;
        private bool done;
        private byte lastChar;
        private int bestGuess;
        private int languageFilter;
        private CharsetProber[] charsetProbers = new CharsetProber[ProbersNum];
        private CharsetProber escCharsetProber;
        private string detectedCharset;

        public UniversalDetector(int languageFilter)
        {
            this.Start = true;
            this.InputState = InputState.PureASCII;
            this.LastChar = 0x00;
            this.BestGuess = -1;
            this.LanguageFilter = languageFilter;
        }

        internal InputState InputState
        {
            get
            {
                return this.inputState;
            }

            set
            {
                this.inputState = value;
            }
        }

        protected bool Start
        {
            get
            {
                return this.start;
            }

            set
            {
                this.start = value;
            }
        }

        protected bool GotData
        {
            get
            {
                return this.gotData;
            }

            set
            {
                this.gotData = value;
            }
        }

        protected bool Done
        {
            get
            {
                return this.done;
            }

            set
            {
                this.done = value;
            }
        }

        protected byte LastChar
        {
            get
            {
                return this.lastChar;
            }

            set
            {
                this.lastChar = value;
            }
        }

        protected int LanguageFilter
        {
            get
            {
                return this.languageFilter;
            }

            set
            {
                this.languageFilter = value;
            }
        }

        protected CharsetProber EscCharsetProber
        {
            get
            {
                return this.escCharsetProber;
            }

            set
            {
                this.escCharsetProber = value;
            }
        }

        protected string DetectedCharset
        {
            get
            {
                return this.detectedCharset;
            }

            set
            {
                this.detectedCharset = value;
            }
        }

        protected CharsetProber[] CharsetProbers
        {
            get
            {
                return this.charsetProbers;
            }

            set
            {
                this.charsetProbers = value;
            }
        }

        protected int BestGuess
        {
            get
            {
                return this.bestGuess;
            }

            set
            {
                this.bestGuess = value;
            }
        }

        /// <summary>
        /// Read a block of bytes into the detector.
        /// </summary>
        /// <param name="input">input buffer</param>
        /// <param name="offset">offset into buffer</param>
        /// <param name="length">number of available bytes</param>
        public virtual void Read(byte[] input, int offset, int length)
        {
            if (this.Done)
            {
                return;
            }

            if (length > 0)
            {
                this.GotData = true;
            }

            // If the data starts with BOM, we know it is UTF
            if (this.Start)
            {
                this.Start = false;
                if (length > 3)
                {
                    switch (input[0])
                    {
                    case 0xEF:
                        if (0xBB == input[1] && 0xBF == input[2])
                            {
                                this.DetectedCharset = Charsets.Utf8;
                            }

                            break;
                    case 0xFE:
                        if (0xFF == input[1] && 0x00 == input[2] && 0x00 == input[3])
                            {
                                // FE FF 00 00  UCS-4, unusual octet order BOM (3412)
                                this.DetectedCharset = Charsets.UCS43412;
                            }
                            else if (0xFF == input[1])
                            {
                                this.DetectedCharset = Charsets.UTF16BE;
                            }

                            break;
                    case 0x00:
                        if (0x00 == input[1] && 0xFE == input[2] && 0xFF == input[3])
                            {
                                this.DetectedCharset = Charsets.UTF32BE;
                            }
                            else if (0x00 == input[1] && 0xFF == input[2] && 0xFE == input[3])
                            {
                                // 00 00 FF FE  UCS-4, unusual octet order BOM (2143)
                                this.DetectedCharset = Charsets.UCS42413;
                            }

                            break;
                    case 0xFF:
                        if (0xFE == input[1] && 0x00 == input[2] && 0x00 == input[3])
                            {
                                this.DetectedCharset = Charsets.UTF32LE;
                            }
                            else if (0xFE == input[1])
                            {
                                this.DetectedCharset = Charsets.Utf16LE;
                            }

                            break;
                    } // switch
                }

                if (this.DetectedCharset != null)
                {
                    this.Done = true;
                    return;
                }
            }

            for (int i = 0; i < length; i++)
            {
                // other than 0xa0, if every other character is ascii, the page is ascii
                if ((input[i] & 0x80) != 0 && input[i] != 0xA0)
                {
                    // we got a non-ascii byte (high-byte)
                    if (this.InputState != InputState.Highbyte)
                    {
                        this.InputState = InputState.Highbyte;

                        // kill EscCharsetProber if it is active
                        if (this.EscCharsetProber != null)
                        {
                            this.EscCharsetProber = null;
                        }

                        // start multibyte and singlebyte charset prober
                        if (this.CharsetProbers[0] == null)
                        {
                            this.CharsetProbers[0] = new MBCSGroupProber();
                        }

                        if (this.CharsetProbers[1] == null)
                        {
                            this.CharsetProbers[1] = new SBCSGroupProber();
                        }

                        if (this.CharsetProbers[2] == null)
                        {
                            this.CharsetProbers[2] = new Latin1Prober();
                        }
                    }
                }
                else
                {
                    if (this.InputState == InputState.PureASCII &&
                        (input[i] == 0x1B || (input[i] == 0x7B && this.LastChar == 0x7E)))
                        {
                        // found escape character or HZ "~{"
                        this.InputState = InputState.EscASCII;
                    }

                    this.LastChar = input[i];
                }
            }

            ProbingState st = ProbingState.NotMe;

            switch (this.InputState)
            {
                case InputState.EscASCII:
                    if (this.EscCharsetProber == null)
                    {
                        this.EscCharsetProber = new EscCharsetProber();
                    }

                    st = this.EscCharsetProber.HandleData(input, offset, length);
                    if (st == ProbingState.FoundIt)
                    {
                        this.Done = true;
                        this.DetectedCharset = this.EscCharsetProber.GetCharsetName();
                    }

                    break;
                case InputState.Highbyte:
                    for (int i = 0; i < ProbersNum; i++)
                    {
                        if (this.CharsetProbers[i] != null)
                        {
                            st = this.CharsetProbers[i].HandleData(input, offset, length);
                            #if DEBUG
                            this.CharsetProbers[i].DumpStatus();
                            #endif
                            if (st == ProbingState.FoundIt)
                            {
                                this.Done = true;
                                this.DetectedCharset = this.CharsetProbers[i].GetCharsetName();
                                return;
                            }
                        }
                    }

                    break;
                default:
                    // pure ascii
                    break;
            }

            return;
        }

        /// <summary>
        /// Tell the detector that there is no more data and it must make its
        /// decision.
        /// </summary>
        public virtual void DataEnd()
        {
            if (!this.GotData)
            {
                // we haven't got any data yet, return immediately
                // caller program sometimes call DataEnd before anything has
                // been sent to detector
                return;
            }

            if (this.DetectedCharset != null)
            {
                this.Done = true;
                this.Report(this.DetectedCharset, 1.0f);
                return;
            }

            if (this.InputState == InputState.Highbyte)
            {
                float proberConfidence = 0.0f;
                float maxProberConfidence = 0.0f;
                int maxProber = 0;
                for (int i = 0; i < ProbersNum; i++)
                {
                    if (this.CharsetProbers[i] != null)
                    {
                        proberConfidence = this.CharsetProbers[i].GetConfidence();
                        if (proberConfidence > maxProberConfidence)
                        {
                            maxProberConfidence = proberConfidence;
                            maxProber = i;
                        }
                    }
                }

                if (maxProberConfidence > MINIMUMTHRESHOLD)
                {
                    this.Report(this.CharsetProbers[maxProber].GetCharsetName(), maxProberConfidence);
                }
            }
            else if (this.InputState == InputState.PureASCII)
            {
                this.Report(Charsets.Ascii, 1.0f);
            }
        }

        /// <summary>
        /// Clear internal state of charset detector.
        /// In the original interface this method is protected.
        /// </summary>
        public virtual void Reset()
        {
            this.Done = false;
            this.Start = true;
            this.DetectedCharset = null;
            this.GotData = false;
            this.BestGuess = -1;
            this.InputState = InputState.PureASCII;
            this.LastChar = 0x00;
            if (this.EscCharsetProber != null)
            {
                this.EscCharsetProber.Reset();
            }

            for (int i = 0; i < ProbersNum; i++)
            {
                if (this.CharsetProbers[i] != null)
                {
                    this.CharsetProbers[i].Reset();
                }
            }
        }

        protected abstract void Report(string charset, float confidence);
    }
}
