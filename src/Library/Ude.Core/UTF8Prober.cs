namespace Ude.Core
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class UTF8Prober : CharsetProber
    {
        private const float OneCharProb = 0.50f;
        private CodingStateMachine codingSM;
        private int numOfMBChar;

        public UTF8Prober()
        {
            this.numOfMBChar = 0;
            this.codingSM = new CodingStateMachine(new UTF8SMModel());
            this.Reset();
        }

        public override string GetCharsetName()
        {
            return "UTF-8";
        }

        public override void Reset()
        {
            this.codingSM.Reset();
            this.numOfMBChar = 0;
            this.State = ProbingState.Detecting;
        }

        public override ProbingState HandleData(byte[] buf, int offset, int len)
        {
            int codingState = StateMachineModel.Start;
            int max = offset + len;

            for (int i = offset; i < max; i++)
            {
                codingState = this.codingSM.NextState(buf[i]);

                if (codingState == StateMachineModel.Error)
                {
                    this.State = ProbingState.NotMe;
                    break;
                }

                if (codingState == StateMachineModel.ItsMe)
                {
                    this.State = ProbingState.FoundIt;
                    break;
                }

                if (codingState == StateMachineModel.Start)
                {
                    if (this.codingSM.CurrentCharLen >= 2)
                    {
                        this.numOfMBChar++;
                    }
                }
            }

            if (this.State == ProbingState.Detecting)
            {
                if (this.GetConfidence() > ShortcutThreshold)
                {
                    this.State = ProbingState.FoundIt;
                }
            }

            return this.State;
        }

        public override float GetConfidence()
        {
            float unlike = 0.99f;
            float confidence = 0.0f;

            if (this.numOfMBChar < 6)
            {
                for (int i = 0; i < this.numOfMBChar; i++)
                {
                    unlike *= OneCharProb;
                }

                confidence = 1.0f - unlike;
            }
            else
            {
                confidence = 0.99f;
            }

            return confidence;
        }
    }
}
