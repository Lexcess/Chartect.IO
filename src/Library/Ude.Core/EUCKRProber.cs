using System;

namespace Ude.Core
{
    public class EUCKRProber : CharsetProber
    {
        private CodingStateMachine codingSM;
        private EUCKRDistributionAnalyser distributionAnalyser;
        private byte[] lastChar = new byte[2];

        public EUCKRProber()
        {
            codingSM = new CodingStateMachine(new EUCKRSMModel());
            distributionAnalyser = new EUCKRDistributionAnalyser(); 
            Reset();
        }
        
        public override string GetCharsetName()
        {
            return "EUC-KR";        
        }
        
        public override ProbingState HandleData(byte[] buf, int offset, int len)
        {
            int codingState;
            int max = offset + len;

            for (int i = offset; i < max; i++)
            {
                codingState = codingSM.NextState(buf[i]);
                if (codingState == SMModel.ERROR)
                {
                    state = ProbingState.NotMe;
                    break;
                }
                if (codingState == SMModel.ITSME)
                {
                    state = ProbingState.FoundIt;
                    break;
                }
                if (codingState == SMModel.START)
                {
                    int charLen = codingSM.CurrentCharLen;
                    if (i == offset)
                    {
                        lastChar[1] = buf[offset];
                        distributionAnalyser.HandleOneChar(lastChar, 0, charLen);
                    }
                    else
                    {
                         distributionAnalyser.HandleOneChar(buf, i-1, charLen);
                    }
                }
            }

            lastChar[0] = buf[max-1];

            if (state == ProbingState.Detecting)
            { 
                if (distributionAnalyser.GotEnoughData() && GetConfidence() > SHORTCUT_THRESHOLD)
                {
                    state = ProbingState.FoundIt;
                }
            }

            return state;
        }

        public override float GetConfidence()
        {
            return distributionAnalyser.GetConfidence();
        }

        public override void Reset()
        {
            codingSM.Reset(); 
            state = ProbingState.Detecting;
            distributionAnalyser.Reset();
            //mContextAnalyser.Reset();
        }
    }
}
