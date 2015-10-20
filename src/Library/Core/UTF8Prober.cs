namespace Chartect.IO.Core
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    internal class UTF8Prober : CharsetProber
    {
        private const float OneCharProb = 0.50f;
        private CodingStateMachine stateMachine;
        private int numOfMultiByteChar;

        public UTF8Prober()
        {
            this.numOfMultiByteChar = 0;
            this.stateMachine = new CodingStateMachine(new Utf8Model());
            this.Reset();
        }

        public override string GetCharsetName()
        {
            return Charsets.Utf8;
        }

        public override void Reset()
        {
            this.stateMachine.Reset();
            this.numOfMultiByteChar = 0;
            this.State = ProbingState.Detecting;
        }

        public override ProbingState HandleData(byte[] buf, int offset, int len)
        {
            int codingState = StateMachineModel.Start;
            int max = offset + len;

            for (int i = offset; i < max; i++)
            {
                codingState = this.stateMachine.NextState(buf[i]);

                if (codingState == StateMachineModel.Error)
                {
                    this.State = ProbingState.NegativeDetection;
                    break;
                }

                if (codingState == StateMachineModel.ItsMe)
                {
                    this.State = ProbingState.Detected;
                    break;
                }

                if (codingState == StateMachineModel.Start)
                {
                    if (this.stateMachine.CurrentCharLen >= 2)
                    {
                        this.numOfMultiByteChar++;
                    }
                }
            }

            if (this.State == ProbingState.Detecting)
            {
                if (this.GetConfidence() > ShortcutThreshold)
                {
                    this.State = ProbingState.Detected;
                }
            }

            return this.State;
        }

        public override float GetConfidence()
        {
            float unlike = 0.99f;
            float confidence = 0.0f;

            if (this.numOfMultiByteChar < 6)
            {
                for (int i = 0; i < this.numOfMultiByteChar; i++)
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
