namespace Ude.Core
{
    using System;

    public class EUCJPProber : CharsetProber
    {
        private CodingStateMachine codingSM;
        private EUCJPContextAnalyser contextAnalyser;
        private EUCJPDistributionAnalyser distributionAnalyser;
        private byte[] lastChar = new byte[2];

        public EUCJPProber()
        {
            this.codingSM = new CodingStateMachine(new EUCJPSMModel());
            this.distributionAnalyser = new EUCJPDistributionAnalyser();
            this.contextAnalyser = new EUCJPContextAnalyser();
            this.Reset();
        }

        public override string GetCharsetName()
        {
            return "EUC-JP";
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
                        this.contextAnalyser.HandleOneChar(this.lastChar, 0, charLen);
                        this.distributionAnalyser.HandleOneChar(this.lastChar, 0, charLen);
                    }
                    else
                    {
                        this.contextAnalyser.HandleOneChar(buf, i - 1, charLen);
                        this.distributionAnalyser.HandleOneChar(buf, i - 1, charLen);
                    }
                }
            }

            this.lastChar[0] = buf[max - 1];
            if (this.state == ProbingState.Detecting)
            {
                if (this.contextAnalyser.GotEnoughData() && this.GetConfidence() > ShortcutThreshold)
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
            this.contextAnalyser.Reset();
            this.distributionAnalyser.Reset();
        }

        public override float GetConfidence()
        {
            float contxtCf = this.contextAnalyser.GetConfidence();
            float distribCf = this.distributionAnalyser.GetConfidence();
            return contxtCf > distribCf ? contxtCf : distribCf;
        }
    }
}
