namespace Chartect.IO.Core
{
    /// <summary>
    /// for S-JIS encoding, observe characteristic:
    /// 1, kana character (or hankaku?) often have hight frequency of appereance
    /// 2, kana character often exist in group
    /// 3, certain combination of kana is never used in japanese language
    /// </summary>
    internal sealed class SjisProber : CharsetProber
    {
        private readonly SjisContextAnalyser contextAnalyser;
        private readonly SjisDistributionAnalyser distributionAnalyser;
        private CodingStateMachine codingSM;
        private byte[] lastChar = new byte[2];

        public SjisProber()
        {
            this.codingSM = new CodingStateMachine(new SjisModel());
            this.distributionAnalyser = new SjisDistributionAnalyser();
            this.contextAnalyser = new SjisContextAnalyser();
            this.Reset();
        }

        public override string GetCharsetName()
        {
            return Charsets.ShiftJis;
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
                        this.contextAnalyser.HandleOneChar(this.lastChar, 2 - charLen, charLen);
                        this.distributionAnalyser.HandleOneChar(this.lastChar, 0, charLen);
                    }
                    else
                    {
                        this.contextAnalyser.HandleOneChar(buffer, i + 1 - charLen, charLen);
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
