using System;

namespace Ude.Core
{   
    public abstract class SequenceModel
    {
        // [256] table use to find a char's order
        protected byte[] charToOrderMap;
        
        // [SAMPLE_SIZE][SAMPLE_SIZE] table to find a 2-char sequence's 
        // frequency        
        protected byte[] precedenceMatrix;
        
        // freqSeqs / totalSeqs
        protected float typicalPositiveRatio;
        
        public float TypicalPositiveRatio {
            get { return typicalPositiveRatio; }
        }
        
        // not used            
        protected bool keepEnglishLetter;
        
        public bool KeepEnglishLetter {
            get { return keepEnglishLetter; }
        }
        
        protected String charsetName;

        public string CharsetName {
            get { return charsetName; }
        }
        
        public SequenceModel(
                byte[] charToOrderMap,
                byte[] precedenceMatrix,
                float typicalPositiveRatio,
                bool keepEnglishLetter,
                String charsetName)
        {
            this.charToOrderMap = charToOrderMap;
            this.precedenceMatrix = precedenceMatrix;
            this.typicalPositiveRatio = typicalPositiveRatio;
            this.keepEnglishLetter = keepEnglishLetter;
            this.charsetName = charsetName;
        }
        
        public byte GetOrder(byte b)
        {
            return charToOrderMap[b];
        }
        
        public byte GetPrecedence(int pos)
        {
            return precedenceMatrix[pos];
        }
        
    }
}
