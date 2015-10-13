using System;

namespace Ude.Core
{
    /// <summary>
    /// Multi-byte charsets probers
    /// </summary>
    public class MBCSGroupProber : CharsetProber
    {
        private const int PROBERS_NUM = 7;
        private readonly static string[] ProberName = 
            { "UTF8", "SJIS", "EUCJP", "GB18030", "EUCKR", "Big5", "EUCTW" };
        private CharsetProber[] probers = new CharsetProber[PROBERS_NUM];
        private bool[] isActive = new bool[PROBERS_NUM];
        private int bestGuess;
        private int activeNum;
            
        public MBCSGroupProber()
        {
            probers[0] = new UTF8Prober();
            probers[1] = new SJISProber();
            probers[2] = new EUCJPProber();
            probers[3] = new GB18030Prober();
            probers[4] = new EUCKRProber();
            probers[5] = new Big5Prober();
            probers[6] = new EUCTWProber();
            Reset();        
        }

        public override string GetCharsetName()
        {
            if (bestGuess == -1) {
                GetConfidence();
                if (bestGuess == -1)
                    bestGuess = 0;
            }
            return probers[bestGuess].GetCharsetName();
        }

        public override void Reset()
        {
            activeNum = 0;
            for (int i = 0; i < probers.Length; i++) {
                if (probers[i] != null) {
                   probers[i].Reset();
                   isActive[i] = true;
                   ++activeNum;
                } else {
                   isActive[i] = false;
                }
            }
            bestGuess = -1;
            state = ProbingState.Detecting;
        }

        public override ProbingState HandleData(byte[] buf, int offset, int len)
        {
            // do filtering to reduce load to probers
            byte[] highbyteBuf = new byte[len];
            int hptr = 0;
            //assume previous is not ascii, it will do no harm except add some noise
            bool keepNext = true;
            int max = offset + len;
            
            for (int i = offset; i < max; i++) {
                if ((buf[i] & 0x80) != 0) {
                    highbyteBuf[hptr++] = buf[i];
                    keepNext = true;
                } else {
                    //if previous is highbyte, keep this even it is a ASCII
                    if (keepNext) {
                        highbyteBuf[hptr++] = buf[i];
                        keepNext = false;
                    }
                }
            }
            
            ProbingState st = ProbingState.NotMe;
            
            for (int i = 0; i < probers.Length; i++) {
                if (!isActive[i])
                    continue;
                st = probers[i].HandleData(highbyteBuf, 0, hptr);
                if (st == ProbingState.FoundIt) {
                    bestGuess = i;
                    state = ProbingState.FoundIt;
                    break;
                } else if (st == ProbingState.NotMe) {
                    isActive[i] = false;
                    activeNum--;
                    if (activeNum <= 0) {
                        state = ProbingState.NotMe;
                        break;
                    }
                }
            }
            return state;
        }

        public override float GetConfidence()
        {
            float bestConf = 0.0f;
            float cf = 0.0f;
            
            if (state == ProbingState.FoundIt) {
                return 0.99f;
            } else if (state == ProbingState.NotMe) {
                return 0.01f;
            } else {
                for (int i = 0; i < PROBERS_NUM; i++) {
                    if (!isActive[i])
                        continue;
                    cf = probers[i].GetConfidence();
                    if (bestConf < cf) {
                        bestConf = cf;
                        bestGuess = i;
                    }
                }
            }
            return bestConf;
        }

        public override void DumpStatus()
        {
            float cf;
            GetConfidence();
            for (int i = 0; i < PROBERS_NUM; i++) {
                if (!isActive[i]) {
                    Console.WriteLine("  MBCS inactive: {0} (confidence is too low).", 
                         ProberName[i]);
                } else {
                    cf = probers[i].GetConfidence();
                    Console.WriteLine("  MBCS {0}: [{1}]", cf, ProberName[i]);
                }
            }
        }
    }
}
