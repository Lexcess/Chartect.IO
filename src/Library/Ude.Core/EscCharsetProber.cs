using System;
using System.Collections.Generic;
using System.Text;

namespace Ude.Core
{
    public class EscCharsetProber : CharsetProber
    {
        private const int CHARSETS_NUM = 4;
        private string detectedCharset;
        private CodingStateMachine[] codingSM; 
        int activeSM;

        public EscCharsetProber()
        {
            codingSM    = new CodingStateMachine[CHARSETS_NUM]; 
            codingSM[0] = new CodingStateMachine(new HZSMModel());
            codingSM[1] = new CodingStateMachine(new ISO2022CNSMModel());
            codingSM[2] = new CodingStateMachine(new ISO2022JPSMModel());
            codingSM[3] = new CodingStateMachine(new ISO2022KRSMModel());
            Reset();
        }
        
        public override void Reset()
        {
            state = ProbingState.Detecting;
            for (int i = 0; i < CHARSETS_NUM; i++)
            {
                codingSM[i].Reset();
            }
            activeSM        = CHARSETS_NUM;
            detectedCharset = null;
        }

        public override ProbingState HandleData(byte[] buf, int offset, int len)
        {
            int max = offset + len;
            
            for (int i = offset; i < max && state == ProbingState.Detecting; i++)
            {
                for (int j = activeSM - 1; j >= 0; j--)
                {
                    // byte is feed to all active state machine
                    int codingState = codingSM[j].NextState(buf[i]);
                    if (codingState == SMModel.ERROR)  {
                        // got negative answer for this state machine, make it inactive
                        activeSM--;
                        if (activeSM == 0)
                        {
                            state = ProbingState.NotMe;
                            return state;
                        } else if (j != activeSM)
                        {
                            CodingStateMachine t = codingSM[activeSM];
                            codingSM[activeSM]   = codingSM[j];
                            codingSM[j]          = t;
                        }
                    }
                    else if (codingState == SMModel.ITSME)
                    {
                        state = ProbingState.FoundIt;
                        detectedCharset = codingSM[j].ModelName;
                        return state;
                    }
                }
            }
            return state;
        }

        public override string GetCharsetName()
        {
            return detectedCharset;        
        }
        
        public override float GetConfidence()
        {
            return 0.99f;
        }           
    }
}
