namespace Chartect.IO.Core
{
    using System;

    internal abstract class SequenceModel
    {
        // [256] table use to find a char's order
        private byte[] charToOrderMap;

        // [SAMPLE_SIZE][SAMPLE_SIZE] table to find a 2-char sequence's
        // frequency
        private byte[] precedenceMatrix;

        // freqSeqs / totalSeqs
        private float typicalPositiveRatio;

        // not used
        private bool keepEnglishLetter;

        private string charsetName;

        protected SequenceModel(
                byte[] charToOrderMap,
                byte[] precedenceMatrix,
                float typicalPositiveRatio,
                bool keepEnglishLetter,
                string charsetName)
        {
            this.CharToOrderMap = charToOrderMap;
            this.PrecedenceMatrix = precedenceMatrix;
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

        protected byte[] CharToOrderMap
        {
            get
            {
                return this.charToOrderMap;
            }

            set
            {
                this.charToOrderMap = value;
            }
        }

        protected byte[] PrecedenceMatrix
        {
            get
            {
                return this.precedenceMatrix;
            }

            set
            {
                this.precedenceMatrix = value;
            }
        }

        public byte GetOrder(byte b)
        {
            return this.CharToOrderMap[b];
        }

        public byte GetPrecedence(int pos)
        {
            return this.PrecedenceMatrix[pos];
        }
    }
}