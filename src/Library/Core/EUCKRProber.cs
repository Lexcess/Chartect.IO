namespace Chartect.IO.Core
{
    using System;

    public class EUCKRProber : CharsetProber
    {
        private CodingStateMachine codingSM;
        private EUCKRDistributionAnalyser distributionAnalyser;
        private byte[] lastChar = new byte[2];

        public EUCKRProber()
        {
            this.codingSM = new CodingStateMachine(new EUCKRSMModel());
            this.distributionAnalyser = new EUCKRDistributionAnalyser();
            this.Reset();
        }

        public override string GetCharsetName()
        {
            return "EUC-KR";
        }

        public override ProbingState HandleData(byte[] buf, int offset, int len)
        {
            int codingState;
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
                    this.State = ProbingState.FoundIt;
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
