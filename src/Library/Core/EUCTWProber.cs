namespace Chartect.IO.Core
{
    using System;

    internal sealed class EucTWProber : CharsetProber
    {
        private readonly EucTWDistributionAnalyser distributionAnalyser;
        private CodingStateMachine codingSM;
        private byte[] lastChar = new byte[2];

        public EucTWProber()
        {
            this.codingSM = new CodingStateMachine(new EucTWModel());
            this.distributionAnalyser = new EucTWDistributionAnalyser();
            this.Reset();
        }

        public override ProbingState HandleData(byte[] buffer, int offset, int length)
        {
            int codingState;
            int max = offset + length;

            for (int i = 0; i < max; i++)
            {
                codingState = this.codingSM.NextState(buffer[i]);
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
                    int charLen = this.codingSM.CurrentCharLen;
                    if (i == offset)
                    {
                        this.lastChar[1] = buffer[offset];
                        this.distributionAnalyser.HandleOneChar(this.lastChar, 0, charLen);
                    }
                    else
                    {
                        this.distributionAnalyser.HandleOneChar(buffer, i - 1, charLen);
                    }
                }
            }

            this.lastChar[0] = buffer[max - 1];

            if (this.State == ProbingState.Detecting)
            {
                if (this.distributionAnalyser.GotEnoughData() && this.GetConfidence() > ShortcutThreshold)
                {
                    this.State = ProbingState.Detected;
                }
            }

            return this.State;
        }

        public override string GetCharsetName()
        {
            return Charsets.EucTW;
        }

        public override void Reset()
        {
            this.codingSM.Reset();
            this.State = ProbingState.Detecting;
            this.distributionAnalyser.Reset();
        }

        public override float GetConfidence()
        {
            return this.distributionAnalyser.GetConfidence();
        }
    }
}
