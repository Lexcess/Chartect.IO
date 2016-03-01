namespace Chartect.IO.Core
{
    /// <summary>
    /// Multi-byte charsets probers
    /// </summary>
    internal class MultiByteCharsetProbeSet : CharsetProber, IProbeSet
    {
        private const int PROBERSNUM = 7;
        private static readonly string[] ProberName =
            { "UTF8", "SJIS", "EUCJP", "GB18030", "EUCKR", "Big5", "EUCTW" };

        private readonly CharsetProber[] probers = new CharsetProber[PROBERSNUM];
        private readonly bool[] isActive = new bool[PROBERSNUM];
        private int bestGuess;
        private int activeNum;

        public MultiByteCharsetProbeSet()
        {
            this.probers[0] = new Utf8Prober();
            this.probers[1] = new SjisProber();
            this.probers[2] = new EucJPProber();
            this.probers[3] = new GB18030Prober();
            this.probers[4] = new EucKRProber();
            this.probers[5] = new Big5Prober();
            this.probers[6] = new EucTWProber();
            this.InitialiseProbes();
        }

        public override string GetCharsetName()
        {
            if (this.bestGuess == -1)
            {
                this.GetConfidence();
                if (this.bestGuess == -1)
                {
                    this.bestGuess = 0;
                }
            }

            return this.probers[this.bestGuess].GetCharsetName();
        }

        public override void Reset()
        {
            this.InitialiseProbes();
        }

        public override ProbingState HandleData(byte[] buffer, int offset, int length)
        {
            // do filtering to reduce load to probers
            byte[] highbyteBuf = new byte[length];
            int hptr = 0;

            // assume previous is not ASCII, it will do no harm except add some noise
            bool keepNext = true;
            int max = offset + length;

            for (int i = offset; i < max; i++)
            {
                if ((buffer[i] & 0x80) != 0)
                {
                    highbyteBuf[hptr++] = buffer[i];
                    keepNext = true;
                }
                else
                {
                    // if previous is highbyte, keep this even it is a ASCII
                    if (keepNext)
                    {
                        highbyteBuf[hptr++] = buffer[i];
                        keepNext = false;
                    }
                }
            }

            ProbingState st = ProbingState.NegativeDetection;

            for (int i = 0; i < this.probers.Length; i++)
            {
                if (!this.isActive[i])
                {
                    continue;
                }

                st = this.probers[i].HandleData(highbyteBuf, 0, hptr);
                if (st == ProbingState.Detected)
                {
                    this.bestGuess = i;
                    this.State = ProbingState.Detected;
                    break;
                }
                else if (st == ProbingState.NegativeDetection)
                {
                    this.isActive[i] = false;
                    this.activeNum--;
                    if (this.activeNum <= 0)
                    {
                        this.State = ProbingState.NegativeDetection;
                        break;
                    }
                }
            }

            return this.State;
        }

        public override float GetConfidence()
        {
            float bestConf = 0.0f;
            float cf = 0.0f;

            if (this.State == ProbingState.Detected)
            {
                return 0.99f;
            }
            else if (this.State == ProbingState.NegativeDetection)
            {
                return 0.01f;
            }
            else
            {
                for (int i = 0; i < PROBERSNUM; i++)
                {
                    if (!this.isActive[i])
                    {
                        continue;
                    }

                    cf = this.probers[i].GetConfidence();
                    if (bestConf < cf)
                    {
                        bestConf = cf;
                        this.bestGuess = i;
                    }
                }
            }

            return bestConf;
        }

        public override void DumpStatus()
        {
            float cf;
            this.GetConfidence();
            for (int i = 0; i < PROBERSNUM; i++)
            {
                if (!this.isActive[i])
                {
                    System.Diagnostics.Debug.WriteLine($"  MBCS inactive: {ProberName[i]} (confidence is too low).");
                }
                else
                {
                    cf = this.probers[i].GetConfidence();
                    System.Diagnostics.Debug.WriteLine($"  MBCS {cf}: [{ProberName[i]}]");
                }
            }
        }

        private void InitialiseProbes()
        {
            this.activeNum = 0;
            for (int i = 0; i < this.probers.Length; i++)
            {
                if (this.probers[i] != null)
                {
                   this.probers[i].Reset();
                   this.isActive[i] = true;
                   ++this.activeNum;
                }
                else
                {
                   this.isActive[i] = false;
                }
            }

            this.bestGuess = -1;
            this.State = ProbingState.Detecting;
        }
    }
}
