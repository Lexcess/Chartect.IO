namespace Chartect.IO.Core
{
    internal class Utf8Prober : CharsetProber
    {
        private const float OneCharProb = 0.50f;
        private readonly CodingStateMachine stateMachine;
        private int numOfMultiByteChar;

        public Utf8Prober()
        {
            this.numOfMultiByteChar = 0;
            this.stateMachine = new CodingStateMachine(new Utf8Model());
            this.InitialiseProbes();
        }

        public override string GetCharsetName()
        {
            return Charsets.Utf8;
        }

        public override void Reset()
        {
            this.InitialiseProbes();
        }

        public override ProbingState HandleData(byte[] buffer, int offset, int length)
        {
            int codingState = StateMachineModel.Start;
            int max = offset + length;

            for (int i = offset; i < max; i++)
            {
                codingState = this.stateMachine.NextState(buffer[i]);

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

        private void InitialiseProbes()
        {
            this.stateMachine.Reset();
            this.numOfMultiByteChar = 0;
            this.State = ProbingState.Detecting;
        }
    }
}
