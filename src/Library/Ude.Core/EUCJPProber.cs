using System;

namespace Ude.Core
{
    public class EUCJPProber : CharsetProber
    {
        private CodingStateMachine codingSM;
        private EUCJPContextAnalyser contextAnalyser;
        private EUCJPDistributionAnalyser distributionAnalyser;
        private byte[] lastChar = new byte[2];
        
        public EUCJPProber()
        {
            codingSM = new CodingStateMachine(new EUCJPSMModel());
            distributionAnalyser = new EUCJPDistributionAnalyser();
            contextAnalyser = new EUCJPContextAnalyser(); 
            Reset();
        }

        public override string GetCharsetName() 
        {
            return "EUC-JP";
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
                        contextAnalyser.HandleOneChar(lastChar, 0, charLen);
                        distributionAnalyser.HandleOneChar(lastChar, 0, charLen);
                    } else
                    {
                        contextAnalyser.HandleOneChar(buf, i-1, charLen);
                        distributionAnalyser.HandleOneChar(buf, i-1, charLen);
                    }
                }
            }
             
            lastChar[0] = buf[max-1];
            if (state == ProbingState.Detecting)
            {
                if (contextAnalyser.GotEnoughData() && GetConfidence() > SHORTCUT_THRESHOLD)
                { 
                    state = ProbingState.FoundIt;
                }
            }
            return state;
        }

        public override void Reset()
        {
            codingSM.Reset(); 
            state = ProbingState.Detecting;
            contextAnalyser.Reset();
            distributionAnalyser.Reset();
        }
        
        public override float GetConfidence()
        {
            float contxtCf  = contextAnalyser.GetConfidence();
            float distribCf = distributionAnalyser.GetConfidence();
            return (contxtCf > distribCf ? contxtCf : distribCf);
        }
        
    }
}
