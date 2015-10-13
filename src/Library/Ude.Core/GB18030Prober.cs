namespace Ude.Core
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    // We use gb18030 to replace gb2312, because 18030 is a superset.
    public class GB18030Prober : CharsetProber
    {
        private CodingStateMachine codingSM;
        private GB18030DistributionAnalyser analyser;
        private byte[] lastChar;

        public GB18030Prober()
        {
            this.lastChar = new byte[2];
            this.codingSM = new CodingStateMachine(new GB18030SMModel());
            this.analyser = new GB18030DistributionAnalyser();
            this.Reset();
        }

        public override string GetCharsetName()
        {
            return "gb18030";
        }

        public override ProbingState HandleData(byte[] buf, int offset, int len)
        {
            int codingState = StateMachineModel.Start;
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
                        this.analyser.HandleOneChar(this.lastChar, 0, charLen);
                    }
                    else
                    {
                        this.analyser.HandleOneChar(buf, i - 1, charLen);
                    }
                }
            }

            this.lastChar[0] = buf[max - 1];

            if (this.state == ProbingState.Detecting)
            {
                if (this.analyser.GotEnoughData() && this.GetConfidence() > ShortcutThreshold)
                {
                    this.state = ProbingState.FoundIt;
                }
            }

            return this.state;
        }

        public override float GetConfidence()
        {
            return this.analyser.GetConfidence();
        }

        public override void Reset()
        {
            this.codingSM.Reset();
            this.state = ProbingState.Detecting;
            this.analyser.Reset();
        }
    }
}
