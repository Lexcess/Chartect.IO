namespace Ude.Core
{
    using System;

    public abstract class SequenceModel
    {
        // [256] table use to find a char's order
        protected byte[] charToOrderMap;

        // [SAMPLE_SIZE][SAMPLE_SIZE] table to find a 2-char sequence's
        // frequency
        protected byte[] precedenceMatrix;

        // freqSeqs / totalSeqs
        private float typicalPositiveRatio;

        // not used
        private bool keepEnglishLetter;

        private string charsetName;

        public SequenceModel(
                byte[] charToOrderMap,
                byte[] precedenceMatrix,
                float typicalPositiveRatio,
                bool keepEnglishLetter,
                string charsetName)
        {
            this.charToOrderMap = charToOrderMap;
            this.precedenceMatrix = precedenceMatrix;
            this.typicalPositiveRatio = typicalPositiveRatio;
            this.keepEnglishLetter = keepEnglishLetter;
            this.charsetName = charsetName;
        }

        public float TypicalPositiveRatio
        {
            get { return this.typicalPositiveRatio; }
            protected set { this.typicalPositiveRatio = value; }
        }

        public bool KeepEnglishLetter
        {
            get { return this.keepEnglishLetter; }
            protected set { this.keepEnglishLetter = value; }
        }

        public string CharsetName
        {
            get { return this.charsetName; }
            protected set { this.charsetName = value; }
        }

        public byte GetOrder(byte b)
        {
            return this.charToOrderMap[b];
        }

        public byte GetPrecedence(int pos)
        {
            return this.precedenceMatrix[pos];
        }
    }
}