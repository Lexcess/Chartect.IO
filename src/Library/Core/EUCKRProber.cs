namespace Chartect.IO.Core
{
    using System;

    internal class EucKRProber : CharsetProber
    {
        private CodingStateMachine codingSM;
        private EucKRDistributionAnalyser distributionAnalyser;
        private byte[] lastChar = new byte[2];

        public EucKRProber()
        {
            this.codingSM = new CodingStateMachine(new EucKRModel());
            this.distributionAnalyser = new EucKRDistributionAnalyser();
            this.Reset();
        }

        public override string GetCharsetName()
        {
            return "EUC-KR";
        }

        public override ProbingState HandleData(byte[] input, int offset, int length)
        {
            int codingState;
            int max = offset + length;

            for (int i = offset; i < max; i++)
            {
                codingState = this.codingSM.NextState(input[i]);
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
                        this.lastChar[1] = input[offset];
                        this.distributionAnalyser.HandleOneChar(this.lastChar, 0, charLen);
                    }
                    else
                    {
                         this.distributionAnalyser.HandleOneChar(input, i - 1, charLen);
                    }
                }
            }

            this.lastChar[0] = input[max - 1];

            if (this.State == ProbingState.Detecting)
            {
                if (this.distributionAnalyser.GotEnoughData() && this.GetConfidence() > ShortcutThreshold)
                {
                    this.State = ProbingState.Detected;
                }
            }

            return this.State;
        }

        public override float GetConfidence()
        {
            return this.distributionAnalyser.GetConfidence();
        }

        public override void Reset()
        {
            this.codingSM.Reset();
            this.State = ProbingState.Detecting;
            this.distributionAnalyser.Reset();

            // mContextAnalyser.Reset();
        }
    }
}
