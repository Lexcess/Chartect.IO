namespace Ude.Core
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Base class for the Character Distribution Method, used for
    /// the CJK encodings
    /// </summary>
    public abstract class CharDistributionAnalyser
    {
        protected const float SUREYES = 0.99f;
        protected const float SURENO = 0.01f;
        protected const int MINIMUMDATATHRESHOLD = 4;
        protected const int ENOUGHDATATHRESHOLD = 1024;

        // If this flag is set to true, detection is done and conclusion has been made
        protected bool done;

        // The number of characters whose frequency order is less than 512
        protected int freqChars;

        // Total character encounted.
        protected int totalChars;

        // Mapping table to get frequency order from char order (get from GetOrder())
        protected int[] charToFreqOrder;

        // This constant value varies from language to language. It is used in calculating confidence.
        protected float typicalDistributionRatio;

        public CharDistributionAnalyser()
        {
            this.Reset();
        }

        // Feed a block of data and do distribution analysis
        // public abstract void HandleData(byte[] buf, int offset, int len);

        /// <summary>
        /// we do not handle character base on its original encoding string, but
        /// convert this encoding string to a number, here called order.
        /// This allow multiple encoding of a language to share one frequency table
        /// </summary>
        /// <param name="buf"> A byte array buffer. <see cref="byte"/></param>
        /// <param name="offset"> The offset in the buffer.</param>
        /// <returns>Returns the Order as an int.</returns>
        public abstract int GetOrder(byte[] buf, int offset);

        /// <summary>
        /// Feed a character with known length
        /// </summary>
        /// <param name="buf">A <see cref="byte"/></param>
        /// <param name="offset">buf offset</param>
        /// <param name="charLen">The character width.</param>
        public void HandleOneChar(byte[] buf, int offset, int charLen)
        {
            // we only care about 2-bytes character in our distribution analysis
            int order = (charLen == 2) ? this.GetOrder(buf, offset) : -1;
            if (order >= 0)
            {
                this.totalChars++;
                if (order < this.charToFreqOrder.Length)
                { // order is valid
                    if (512 > this.charToFreqOrder[order])
                    {
                        this.freqChars++;
                    }
                }
            }
        }

        public virtual void Reset()
        {
            this.done = false;
            this.totalChars = 0;
            this.freqChars = 0;
        }

        // return confidence base on received data
        public virtual float GetConfidence()
        {
            // if we didn't receive any character in our consideration range, or the
            // number of frequent characters is below the minimum threshold, return
            // negative answer
            if (this.totalChars <= 0 || this.freqChars <= MINIMUMDATATHRESHOLD)
            {
                return SURENO;
            }

            if (this.totalChars != this.freqChars)
            {
                float r = this.freqChars / ((this.totalChars - this.freqChars) * this.typicalDistributionRatio);
                if (r < SUREYES)
                {
                    return r;
                }
            }

            // normalize confidence, (we don't want to be 100% sure)
            return SUREYES;
        }

        // It is not necessary to receive all data to draw conclusion. For charset detection,
        // certain amount of data is enough
        public bool GotEnoughData()
        {
            return this.totalChars > ENOUGHDATATHRESHOLD;
        }
    }
}
