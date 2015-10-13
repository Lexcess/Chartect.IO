namespace Ude.Core
{
    using System;

    public class Big5Prober : CharsetProber
    {
        // void GetDistribution(PRUint32 aCharLen, const char* aStr);
        private CodingStateMachine codingSM;
        private BIG5DistributionAnalyser distributionAnalyser;
        private byte[] lastChar = new byte[2];

        public Big5Prober()
        {
            this.codingSM = new CodingStateMachine(new BIG5SMModel());
            this.distributionAnalyser = new BIG5DistributionAnalyser();
            this.Reset();
        }

        public override ProbingState HandleData(byte[] buf, int offset, int len)
        {
            int codingState = 0;
            int max = offset + len;

            for (int i = offset; i < max; i++)
            {
                codingState = this.codingSM.NextState(buf[i]);
                if (codingState == StateMachineModel.Error)
                {
                    this.state = ProbingState.NotMe;
                    break;
                }

                if (codingState == StateMachineModel.ItsMe)
                {
                    this.state = ProbingState.FoundIt;
                    break;
                }

                if (codingState == StateMachineModel.Start)
                {
                    int charLen = this.codingSM.CurrentCharLen;
                    if (i == offset)
                    {
                        this.lastChar[1] = buf[offset];
                        this.distributionAnalyser.HandleOneChar(this.lastChar, 0, charLen);
                    }
                    else
                    {
                        this.distributionAnalyser.HandleOneChar(buf, i - 1, charLen);
                    }
                }
            }

            this.lastChar[0] = buf[max - 1];

            if (this.state == ProbingState.Detecting)
            {
                if (this.distributionAnalyser.GotEnoughData() && this.GetConfidence() > ShortcutThreshold)
                {
                    this.state = ProbingState.FoundIt;
                }
            }

            return this.state;
        }

        public override void Reset()
        {
            this.codingSM.Reset();
            this.state = ProbingState.Detecting;
            this.distributionAnalyser.Reset();
        }

        public override string GetCharsetName()
        {
            return "Big-5";
        }

        public override float GetConfidence()
        {
            return this.distributionAnalyser.GetConfidence();
        }
    }
}
