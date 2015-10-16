namespace Chartect.IO.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    public enum ProbingState
    {
        Detecting = 0, // no sure answer yet, but caller can ask for confidence
        FoundIt = 1, // positive answer
        NotMe = 2 // negative answer
    }

    public abstract class CharsetProber
    {
        protected const float ShortcutThreshold = 0.95F;

        // ASCII codes
        private const byte Space = 0x20;
        private const byte UpperA = 0x41;
        private const byte UpperZ = 0x5A;
        private const byte LowerA = 0x61;
        private const byte LowerZ = 0x7A;
        private const byte LessThan = 0x3C;
        private const byte GreaterThan = 0x3E;

        private ProbingState state;

        protected ProbingState State
        {
            get
            {
                return this.state;
            }

            set
            {
                this.state = value;
            }
        }

        /// <summary>
        /// Read data to the prober
        /// </summary>
        /// <param name="buffer">a buffer</param>
        /// <param name="offset">offset into buffer</param>
        /// <param name="len">number of bytes available into buffer</param>
        /// <returns>
        /// A <see cref="ProbingState"/>
        /// </returns>
        public abstract ProbingState HandleData(byte[] buffer, int offset, int len);

        /// <summary>
        /// Reset prober state
        /// </summary>
        public abstract void Reset();

        public abstract string GetCharsetName();

        public abstract float GetConfidence();

        public virtual ProbingState GetState()
        {
            return this.State;
        }

        public virtual void SetOption()
        {
        }

        public virtual void DumpStatus()
        {
        }

        // Helper functions used in the Latin1 and Group probers
        protected static byte[] FilterWithoutEnglishLetters(byte[] input, int offset, int length)
        {
            byte[] result = null;

            using (MemoryStream stream = new MemoryStream(input.Length))
            {
                bool meetMSB = false;
                int max = offset + length;
                int prev = offset;
                int cur = offset;

                while (cur < max)
                {
                    byte b = input[cur];

                    if ((b & 0x80) != 0)
                    {
                        meetMSB = true;
                    }
                    else if (b < UpperA || (b > UpperZ && b < LowerA) || b > LowerZ)
                    {
                        if (meetMSB && cur > prev)
                        {
                            stream.Write(input, prev, cur - prev);
                            stream.WriteByte(Space);
                            meetMSB = false;
                        }

                        prev = cur + 1;
                    }

                    cur++;
                }

                if (meetMSB && cur > prev)
                {
                    stream.Write(input, prev, cur - prev);
                }

                stream.SetLength(stream.Position);
                result = stream.ToArray();
            }

            return result;
        }

        /// <summary>
        /// Do filtering to reduce load to probers (Remove ASCII symbols,
        /// collapse spaces). This filter applies to all scripts which contain
        /// both English characters and upper ASCII characters.
        /// </summary>
        /// <param name="input"> A byte array buffer</param>
        /// <param name="offset"> The offset to use.</param>
        /// <param name="length"> The length of bytes to filter.</param>
        /// <returns> A filtered copy of the input buffer.</returns>
        protected static byte[] FilterWithEnglishLetters(byte[] input, int offset, int length)
        {
            byte[] result = null;

            using (MemoryStream stream = new MemoryStream(input.Length))
            {
                bool inTag = false;
                int max = offset + length;
                int prev = offset;
                int cur = offset;

                while (cur < max)
                {
                    byte b = input[cur];

                    if (b == GreaterThan)
                    {
                        inTag = false;
                    }
                    else if (b == LessThan)
                    {
                        inTag = true;
                    }

                    // it's ascii, but it's not a letter
                    if ((b & 0x80) == 0 && (b < UpperA || b > LowerZ || (b > UpperZ && b < LowerA)))
                    {
                        if (cur > prev && !inTag)
                        {
                            stream.Write(input, prev, cur - prev);
                            stream.WriteByte(Space);
                        }

                        prev = cur + 1;
                    }

                    cur++;
                }

                // If the current segment contains more than just a symbol
                // and it is not inside a tag then keep it.
                if (!inTag && cur > prev)
                {
                    stream.Write(input, prev, cur - prev);
                }

                stream.SetLength(stream.Position);
                result = stream.ToArray();
            }

            return result;
        }
    }
}
