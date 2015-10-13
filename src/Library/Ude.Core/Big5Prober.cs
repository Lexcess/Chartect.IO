using System;

namespace Ude.Core
{
    public class Big5Prober : CharsetProber
    {
        //void GetDistribution(PRUint32 aCharLen, const char* aStr);
        private CodingStateMachine codingSM;
        private BIG5DistributionAnalyser distributionAnalyser;
        private byte[] lastChar = new byte[2];
        
        public Big5Prober()
        {
            this.codingSM = new CodingStateMachine(new BIG5SMModel());
            this.distributionAnalyser = new BIG5DistributionAnalyser();
            this.Reset();        
        }

        public override ProbingState HandleData(byte[] buf, int offset, int len)
        {
            int codingState = 0;
            int max = offset + len;

            for (int i = offset; i < max; i++) {
                codingState = codingSM.NextState(buf[i]);
                if (codingState == SMModel.ERROR) {
                    state = ProbingState.NotMe;
                    break;
                }
                if (codingState == SMModel.ITSME) {
                    state = ProbingState.FoundIt;
                    break;
                }
                if (codingState == SMModel.START) {
                    int charLen = codingSM.CurrentCharLen;
                    if (i == offset) {
                        lastChar[1] = buf[offset];
                        distributionAnalyser.HandleOneChar(lastChar, 0, charLen);
                    } else {
                        distributionAnalyser.HandleOneChar(buf, i-1, charLen);        
                    }
                }
            }
            lastChar[0] = buf[max-1];

            if (state == ProbingState.Detecting)
                if (distributionAnalyser.GotEnoughData() && GetConfidence() > SHORTCUT_THRESHOLD)
                    state = ProbingState.FoundIt;
            return state;
        }
        
        public override void Reset()
        {
            codingSM.Reset(); 
            state = ProbingState.Detecting;
            distributionAnalyser.Reset();
        }
            
        public override string GetCharsetName()
        {
            return "Big-5";        
        }
        
        public override float GetConfidence()
        {
            return distributionAnalyser.GetConfidence();
        }
        
    }
}
