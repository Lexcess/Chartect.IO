namespace Chartect.IO.Core
{
    using System;

    internal class EUCTWProber : CharsetProber
    {
        private CodingStateMachine codingSM;
        private EUCTWDistributionAnalyser distributionAnalyser;
        private byte[] lastChar = new byte[2];

        public EUCTWProber()
        {
            this.codingSM = new CodingStateMachine(new EUCTWSMModel());
            this.distributionAnalyser = new EUCTWDistributionAnalyser();
            this.Reset();
        }

        public override ProbingState HandleData(byte[] buf, int offset, int len)
        {
            int codingState;
            int max = offset + len;

            for (int i = 0; i < max; i++)
            {
                codingState = this.codingSM.NextState(buf[i]);
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
            return "EUC-TW";
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
