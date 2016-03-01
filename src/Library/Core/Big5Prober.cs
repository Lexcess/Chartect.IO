namespace Chartect.IO.Core
{
    internal sealed class Big5Prober : CharsetProber
    {
        private readonly Big5DistributionAnalyser distributionAnalyser;

        // void GetDistribution(PRUint32 aCharLen, const char* aStr);
        private CodingStateMachine codingSM;
        private byte[] lastChar = new byte[2];

        public Big5Prober()
        {
            this.codingSM = new CodingStateMachine(new Big5Model());
            this.distributionAnalyser = new Big5DistributionAnalyser();
            this.Reset();
        }

        public override ProbingState HandleData(byte[] buffer, int offset, int length)
        {
            int codingState = 0;
            int max = offset + length;

            for (int i = offset; i < max; i++)
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

        public override void Reset()
        {
            this.codingSM.Reset();
            this.State = ProbingState.Detecting;
            this.distributionAnalyser.Reset();
        }

        public override string GetCharsetName()
        {
            return Charsets.Big5;
        }

        public override float GetConfidence()
        {
            return this.distributionAnalyser.GetConfidence();
        }
    }
}
