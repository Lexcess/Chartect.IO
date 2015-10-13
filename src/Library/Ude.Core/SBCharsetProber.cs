using System;
using System.Collections.Generic;
using System.Text;

namespace Ude.Core
{

    public class SingleByteCharSetProber : CharsetProber
    {
        private const int SAMPLE_SIZE = 64;
        private const int SB_ENOUGH_REL_THRESHOLD = 1024;
        private const float POSITIVE_SHORTCUT_THRESHOLD = 0.95f;
        private const float NEGATIVE_SHORTCUT_THRESHOLD = 0.05f;
        private const int SYMBOL_CAT_ORDER = 250;
        private const int NUMBER_OF_SEQ_CAT = 4;
        private const int POSITIVE_CAT = NUMBER_OF_SEQ_CAT-1;
        private const int NEGATIVE_CAT = 0;
        
        protected SequenceModel model;
        
        // true if we need to reverse every pair in the model lookup        
        bool reversed; 

        // char order of last character
        byte lastOrder;

        int totalSeqs;
        int totalChar;
        int[] seqCounters = new int[NUMBER_OF_SEQ_CAT];
        
        // characters that fall in our sampling range
        int freqChar;
  
        // Optional auxiliary prober for name decision. created and destroyed by the GroupProber
        CharsetProber nameProber; 
                    
        public SingleByteCharSetProber(SequenceModel model) 
            : this(model, false, null)
        {
            
        }
    
        public SingleByteCharSetProber(SequenceModel model, bool reversed, 
                                       CharsetProber nameProber)
        {
            this.model = model;
            this.reversed = reversed;
            this.nameProber = nameProber;
            Reset();            
        }

        public override ProbingState HandleData(byte[] buf, int offset, int len)
        {
            int max = offset + len;
            
            for (int i = offset; i < max; i++) {
                byte order = model.GetOrder(buf[i]);

                if (order < SYMBOL_CAT_ORDER)
                    totalChar++;
                    
                if (order < SAMPLE_SIZE) {
                    freqChar++;

                    if (lastOrder < SAMPLE_SIZE) {
                        totalSeqs++;
                        if (!reversed)
                            ++(seqCounters[model.GetPrecedence(lastOrder*SAMPLE_SIZE+order)]);
                        else // reverse the order of the letters in the lookup
                            ++(seqCounters[model.GetPrecedence(order*SAMPLE_SIZE+lastOrder)]);
                    }
                }
                lastOrder = order;
            }

            if (state == ProbingState.Detecting) {
                if (totalSeqs > SB_ENOUGH_REL_THRESHOLD) {
                    float cf = GetConfidence();
                    if (cf > POSITIVE_SHORTCUT_THRESHOLD)
                        state = ProbingState.FoundIt;
                    else if (cf < NEGATIVE_SHORTCUT_THRESHOLD)
                        state = ProbingState.NotMe;
                }
            }
            return state;
        }
                
        public override void DumpStatus()
        {
            Console.WriteLine("  SBCS: {0} [{1}]", GetConfidence(), 
                GetCharsetName());
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

            if (totalSeqs > 0) {
                r = 1.0f * seqCounters[POSITIVE_CAT] / totalSeqs / model.TypicalPositiveRatio;
                r = r * freqChar / totalChar;
                if (r >= 1.0f)
                    r = 0.99f;
                return r;
            }
            return 0.01f;            
        }
        
        public override void Reset()
        {
            state = ProbingState.Detecting;
            lastOrder = 255;
            for (int i = 0; i < NUMBER_OF_SEQ_CAT; i++)
                seqCounters[i] = 0;
            totalSeqs = 0;
            totalChar = 0;
            freqChar = 0;
        }
        
        public override string GetCharsetName() 
        {
            return (nameProber == null) ? model.CharsetName
                                        : nameProber.GetCharsetName();
        }
        
    }
}
