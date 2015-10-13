using System;
using System.Collections.Generic;
using System.Text;

namespace Ude.Core
{
    public class UTF8Prober : CharsetProber
    {
        private static float ONE_CHAR_PROB = 0.50f;
        private CodingStateMachine codingSM;
        private int numOfMBChar;

        public UTF8Prober()
        {
            numOfMBChar = 0; 
            codingSM = new CodingStateMachine(new UTF8SMModel());
            Reset();
        }
        
        public override string GetCharsetName() {
            return "UTF-8";
        }

        public override void Reset()
        {
            codingSM.Reset();
            numOfMBChar = 0;
            state = ProbingState.Detecting;
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
                    if (codingSM.CurrentCharLen >= 2)
                        numOfMBChar++;
                }
            }

            if (state == ProbingState.Detecting)
                if (GetConfidence() > SHORTCUT_THRESHOLD)
                    state = ProbingState.FoundIt;
            return state;
        }

        public override float GetConfidence()
        {
            float unlike = 0.99f;
            float confidence = 0.0f;
            
            if (numOfMBChar < 6) {
                for (int i = 0; i < numOfMBChar; i++)
                    unlike *= ONE_CHAR_PROB;
                confidence = 1.0f - unlike;
            } else {
                confidence = 0.99f;
            }
            return confidence;

        }
    }
}
