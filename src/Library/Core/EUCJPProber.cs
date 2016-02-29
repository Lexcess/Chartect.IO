namespace Chartect.IO.Core
{
    using System;

    internal sealed class EucJPProber : CharsetProber
    {
        private readonly EucJPContextAnalyser contextAnalyser;
        private readonly EucJPDistributionAnalyser distributionAnalyser;
        private CodingStateMachine codingSM;
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

        public override ProbingState HandleData(byte[] buffer, int offset, int length)
        {
            int codingState;
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
                        this.contextAnalyser.HandleOneChar(this.lastChar, 0, charLen);
                        this.distributionAnalyser.HandleOneChar(this.lastChar, 0, charLen);
                    }
                    else
                    {
                        this.contextAnalyser.HandleOneChar(buffer, i - 1, charLen);
                        this.distributionAnalyser.HandleOneChar(buffer, i - 1, charLen);
                    }
                }
            }

            this.lastChar[0] = buffer[max - 1];
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
