using System;
using System.Collections.Generic;
using System.Text;

namespace Ude.Core
{
    // We use gb18030 to replace gb2312, because 18030 is a superset. 
    public class GB18030Prober : CharsetProber
    {
        private CodingStateMachine codingSM;
        private GB18030DistributionAnalyser analyser;
        private byte[] lastChar;

        public GB18030Prober()
        {
            lastChar = new byte[2];
            codingSM = new CodingStateMachine(new GB18030SMModel());
            analyser = new GB18030DistributionAnalyser();
            Reset();
        }
        
        public override string GetCharsetName()
        {
            return "gb18030";        
        }
        

        public override ProbingState HandleData(byte[] buf, int offset, int len)
        {
            int codingState = SMModel.START;
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
                        analyser.HandleOneChar(lastChar, 0, charLen);
                    } else {
                        analyser.HandleOneChar(buf, i-1, charLen);
                    }
                }
            }

            lastChar[0] = buf[max-1];

            if (state == ProbingState.Detecting) {
                if (analyser.GotEnoughData() && GetConfidence() > SHORTCUT_THRESHOLD)
                    state = ProbingState.FoundIt;
            }
            
            return state;
        }
        
        public override float GetConfidence()
        {
            return analyser.GetConfidence();
        }
        
        public override void Reset()
        {
            codingSM.Reset(); 
            state = ProbingState.Detecting;
            analyser.Reset();
        }

    }
}
