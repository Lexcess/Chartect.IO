namespace Ude.Core
{
    using System;

    /// <summary>
    /// Multi-byte charsets probers
    /// </summary>
    public class MBCSGroupProber : CharsetProber
    {
        private const int PROBERSNUM = 7;
        private static readonly string[] ProberName =
            { "UTF8", "SJIS", "EUCJP", "GB18030", "EUCKR", "Big5", "EUCTW" };

        private CharsetProber[] probers = new CharsetProber[PROBERSNUM];
        private bool[] isActive = new bool[PROBERSNUM];
        private int bestGuess;
        private int activeNum;

        public MBCSGroupProber()
        {
            this.probers[0] = new UTF8Prober();
            this.probers[1] = new SJISProber();
            this.probers[2] = new EUCJPProber();
            this.probers[3] = new GB18030Prober();
            this.probers[4] = new EUCKRProber();
            this.probers[5] = new Big5Prober();
            this.probers[6] = new EUCTWProber();
            this.Reset();
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

        public override ProbingState HandleData(byte[] buf, int offset, int len)
        {
            // do filtering to reduce load to probers
            byte[] highbyteBuf = new byte[len];
            int hptr = 0;

            // assume previous is not ascii, it will do no harm except add some noise
            bool keepNext = true;
            int max = offset + len;

            for (int i = offset; i < max; i++)
            {
                if ((buf[i] & 0x80) != 0)
                {
                    highbyteBuf[hptr++] = buf[i];
                    keepNext = true;
                }
                else
                {
                    // if previous is highbyte, keep this even it is a ASCII
                    if (keepNext)
                    {
                        highbyteBuf[hptr++] = buf[i];
                        keepNext = false;
                    }
                }
            }

            ProbingState st = ProbingState.NotMe;

            for (int i = 0; i < this.probers.Length; i++)
            {
                if (!this.isActive[i])
                {
                    continue;
                }

                st = this.probers[i].HandleData(highbyteBuf, 0, hptr);
                if (st == ProbingState.FoundIt)
                {
                    this.bestGuess = i;
                    this.State = ProbingState.FoundIt;
                    break;
                }
                else if (st == ProbingState.NotMe)
                {
                    this.isActive[i] = false;
                    this.activeNum--;
                    if (this.activeNum <= 0)
                    {
                        this.State = ProbingState.NotMe;
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

            if (this.State == ProbingState.FoundIt)
            {
                return 0.99f;
            }
            else if (this.State == ProbingState.NotMe)
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
                    Console.WriteLine(
                        "  MBCS inactive: {0} (confidence is too low).",
                         ProberName[i]);
                }
                else
                {
                    cf = this.probers[i].GetConfidence();
                    Console.WriteLine("  MBCS {0}: [{1}]", cf, ProberName[i]);
                }
            }
        }
    }
}
