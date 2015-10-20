namespace Chartect.IO.Core
{
    using System;

    internal class EucJPProber : CharsetProber
    {
        private CodingStateMachine codingSM;
        private EucJPContextAnalyser contextAnalyser;
        private EucJPDistributionAnalyser distributionAnalyser;
        private byte[] lastChar = new byte[2];

        public EucJPProber()
        {
            this.codingSM = new CodingStateMachine(new EucJPModel());
            this.distributionAnalyser = new EucJPDistributionAnalyser();
            this.contextAnalyser = new EucJPContextAnalyser();
            this.Reset();
        }

        public override string GetCharsetName()
        {
            return Charsets.EucJP;
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
            if (this.State == ProbingState.Detecting)
            {
                if (this.contextAnalyser.GotEnoughData() && this.GetConfidence() > ShortcutThreshold)
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
