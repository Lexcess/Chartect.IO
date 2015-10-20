namespace Chartect.IO.Core
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    internal class EscCharsetProbeSet : CharsetProber, IProbeSet
    {
        private const int CharsetsNum = 4;
        private string detectedCharset;
        private CodingStateMachine[] stateMachines;
        private int activeSM;

        public EscCharsetProbeSet()
        {
            this.stateMachines = new CodingStateMachine[CharsetsNum];
            this.stateMachines[0] = new CodingStateMachine(new HzsmEscapedModel());
            this.stateMachines[1] = new CodingStateMachine(new Iso2022CnsmEscapedModel());
            this.stateMachines[2] = new CodingStateMachine(new Iso2022JpsmEscapedModel());
            this.stateMachines[3] = new CodingStateMachine(new Iso2022KrsmEscapedModel());
            this.Reset();
        }

        public override void Reset()
        {
            this.State = ProbingState.Detecting;
            for (int i = 0; i < CharsetsNum; i++)
            {
                this.stateMachines[i].Reset();
            }

            this.activeSM = CharsetsNum;
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
                    int codingState = this.stateMachines[j].NextState(buf[i]);
                    if (codingState == StateMachineModel.Error)
                    {
                        // got negative answer for this state machine, make it inactive
                        this.activeSM--;
                        if (this.activeSM == 0)
                        {
                            this.State = ProbingState.NegativeDetection;
                            return this.State;
                        }
                        else if (j != this.activeSM)
                        {
                            CodingStateMachine t = this.stateMachines[this.activeSM];
                            this.stateMachines[this.activeSM] = this.stateMachines[j];
                            this.stateMachines[j] = t;
                        }
                    }
                    else if (codingState == StateMachineModel.ItsMe)
                    {
                        this.State = ProbingState.Detected;
                        this.detectedCharset = this.stateMachines[j].ModelName;
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
