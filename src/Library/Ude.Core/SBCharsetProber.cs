namespace Ude.Core
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public sealed class SingleByteCharSetProber : CharsetProber
    {
        private const int SAMPLESIZE = 64;
        private const int SBENOUGHRELTHRESHOLD = 1024;
        private const float POSITIVESHORTCUTTHRESHOLD = 0.95f;
        private const float NEGATIVESHORTCUTTHRESHOLD = 0.05f;
        private const int SYMBOLCATORDER = 250;
        private const int NUMBEROFSEQCAT = 4;
        private const int POSITIVECAT = NUMBEROFSEQCAT - 1;
        private const int NEGATIVECAT = 0;

        private SequenceModel model;

        // true if we need to reverse every pair in the model lookup
        private bool reversed;

        // char order of last character
        private byte lastOrder;

        private int totalSeqs;
        private int totalChar;
        private int[] seqCounters = new int[NUMBEROFSEQCAT];

        // characters that fall in our sampling range
        private int freqChar;

        // Optional auxiliary prober for name decision. created and destroyed by the GroupProber
        private CharsetProber nameProber;

        public SingleByteCharSetProber(SequenceModel model)
            : this(model, false, null)
        {
        }

        public SingleByteCharSetProber(SequenceModel model, bool reversed, CharsetProber nameProber)
        {
            this.model = model;
            this.reversed = reversed;
            this.nameProber = nameProber;
            this.Reset();
        }

        public override ProbingState HandleData(byte[] buf, int offset, int len)
        {
            int max = offset + len;

            for (int i = offset; i < max; i++)
            {
                byte order = this.model.GetOrder(buf[i]);

                if (order < SYMBOLCATORDER)
                {
                    this.totalChar++;
                }

                if (order < SAMPLESIZE)
                {
                    this.freqChar++;

                    if (this.lastOrder < SAMPLESIZE)
                    {
                        this.totalSeqs++;
                        if (!this.reversed)
                        {
                            ++this.seqCounters[this.model.GetPrecedence((this.lastOrder * SAMPLESIZE) + order)];
                        }
                        else
                        {
                            // reverse the order of the letters in the lookup
                            ++this.seqCounters[this.model.GetPrecedence((order * SAMPLESIZE) + this.lastOrder)];
                        }
                    }
                }

                this.lastOrder = order;
            }

            if (this.State == ProbingState.Detecting)
            {
                if (this.totalSeqs > SBENOUGHRELTHRESHOLD)
                {
                    float cf = this.GetConfidence();
                    if (cf > POSITIVESHORTCUTTHRESHOLD)
                    {
                        this.State = ProbingState.FoundIt;
                    }
                    else if (cf < NEGATIVESHORTCUTTHRESHOLD)
                    {
                        this.State = ProbingState.NotMe;
                    }
                }
            }

            return this.State;
        }

        public override void DumpStatus()
        {
            Console.WriteLine(
                "  SBCS: {0} [{1}]",
                this.GetConfidence(),
                this.GetCharsetName());
        }

        public override float GetConfidence()
        {
            /*
            NEGATIVE_APPROACH
            if (totalSeqs > 0) {
                if (totalSeqs > seqCounters[NEGATIVE_CAT] * 10)
                    return (totalSeqs - seqCounters[NEGATIVE_CAT] * 10)/totalSeqs * freqChar / mTotalChar;
            }
            return 0.01f;
            */
            // POSITIVE_APPROACH
            float r = 0.0f;

            if (this.totalSeqs > 0)
            {
                r = 1.0f * this.seqCounters[POSITIVECAT] / this.totalSeqs / this.model.TypicalPositiveRatio;
                r = r * this.freqChar / this.totalChar;
                if (r >= 1.0f)
                {
                    r = 0.99f;
                }

                return r;
            }

            return 0.01f;
        }

        public override void Reset()
        {
            this.State = ProbingState.Detecting;
            this.lastOrder = 255;
            for (int i = 0; i < NUMBEROFSEQCAT; i++)
            {
                this.seqCounters[i] = 0;
            }

            this.totalSeqs = 0;
            this.totalChar = 0;
            this.freqChar = 0;
        }

        public override string GetCharsetName()
        {
            return (this.nameProber == null) ? this.model.CharsetName : this.nameProber.GetCharsetName();
        }
    }
}
