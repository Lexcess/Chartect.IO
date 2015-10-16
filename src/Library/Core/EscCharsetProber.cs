namespace Chartect.IO.Core
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class EscCharsetProber : CharsetProber
    {
        private const int CHARSETSNUM = 4;
        private string detectedCharset;
        private CodingStateMachine[] codingSM;
        private int activeSM;

        public EscCharsetProber()
        {
            this.codingSM = new CodingStateMachine[CHARSETSNUM];
            this.codingSM[0] = new CodingStateMachine(new HzsmEscapedModel());
            this.codingSM[1] = new CodingStateMachine(new Iso2022CnsmEscapedModel());
            this.codingSM[2] = new CodingStateMachine(new Iso2022JpsmEscapedModel());
            this.codingSM[3] = new CodingStateMachine(new Iso2022KrsmEscapedModel());
            this.Reset();
        }

        public override void Reset()
        {
            this.State = ProbingState.Detecting;
            for (int i = 0; i < CHARSETSNUM; i++)
            {
                this.codingSM[i].Reset();
            }

            this.activeSM = CHARSETSNUM;
            this.detectedCharset = null;
        }

        public override ProbingState HandleData(byte[] buf, int offset, int len)
        {
            int max = offset + len;

            for (int i = offset; i < max && this.State == ProbingState.Detecting; i++)
            {
                for (int j = this.activeSM - 1; j >= 0; j--)
                {
                    // byte is fed to all active state machine
                    int codingState = this.codingSM[j].NextState(buf[i]);
                    if (codingState == StateMachineModel.Error)
                    {
                        // got negative answer for this state machine, make it inactive
                        this.activeSM--;
                        if (this.activeSM == 0)
                        {
                            this.State = ProbingState.NotMe;
                            return this.State;
                        }
                        else if (j != this.activeSM)
                        {
                            CodingStateMachine t = this.codingSM[this.activeSM];
                            this.codingSM[this.activeSM] = this.codingSM[j];
                            this.codingSM[j] = t;
                        }
                    }
                    else if (codingState == StateMachineModel.ItsMe)
                    {
                        this.State = ProbingState.FoundIt;
                        this.detectedCharset = this.codingSM[j].ModelName;
                        return this.State;
                    }
                }
            }

            return this.State;
        }

        public override string GetCharsetName()
        {
            return this.detectedCharset;
        }

        public override float GetConfidence()
        {
            return 0.99f;
        }
    }
}
