namespace Chartect.IO.Core
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Base class for the Character Distribution Method, used for
    /// the CJK encodings
    /// </summary>
    internal abstract class CharDistributionAnalyser
    {
        protected const float SUREYES = 0.99f;
        protected const float SURENO = 0.01f;
        protected const int MINIMUMDATATHRESHOLD = 4;
        protected const int ENOUGHDATATHRESHOLD = 1024;

        // If this flag is set to true, detection is done and conclusion has been made
        private bool done;

        // The number of characters whose frequency order is less than 512
        private int freqChars;

        // Total character encounted.
        private int totalChars;

        // Mapping table to get frequency order from char order (get from GetOrder())
        private int[] charToFreqOrder;

        // This constant value varies from language to language. It is used in calculating confidence.
        private float typicalDistributionRatio;

        public CharDistributionAnalyser()
        {
            this.Reset();
        }

        protected bool Done
        {
            get
            {
                return this.done;
            }

            set
            {
                this.done = value;
            }
        }

        protected int FreqChars
        {
            get
            {
                return this.freqChars;
            }

            set
            {
                this.freqChars = value;
            }
        }

        protected int TotalChars
        {
            get
            {
                return this.totalChars;
            }

            set
            {
                this.totalChars = value;
            }
        }

        protected int[] CharToFreqOrder
        {
            get
            {
                return this.charToFreqOrder;
            }

            set
            {
                this.charToFreqOrder = value;
            }
        }

        protected float TypicalDistributionRatio
        {
            get
            {
                return this.typicalDistributionRatio;
            }

            set
            {
                this.typicalDistributionRatio = value;
            }
        }

        // Read a block of data and do distribution analysis
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
        /// Read a character with known length
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
                this.TotalChars++;
                if (order < this.CharToFreqOrder.Length)
                { // order is valid
                    if (512 > this.CharToFreqOrder[order])
                    {
                        this.FreqChars++;
                    }
                }
            }
        }

        public virtual void Reset()
        {
            this.Done = false;
            this.TotalChars = 0;
            this.FreqChars = 0;
        }

        // return confidence base on received data
        public virtual float GetConfidence()
        {
            // if we didn't receive any character in our consideration range, or the
            // number of frequent characters is below the minimum threshold, return
            // negative answer
            if (this.TotalChars <= 0 || this.FreqChars <= MINIMUMDATATHRESHOLD)
            {
                return SURENO;
            }

            if (this.TotalChars != this.FreqChars)
            {
                float r = this.FreqChars / ((this.TotalChars - this.FreqChars) * this.TypicalDistributionRatio);
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
            return this.TotalChars > ENOUGHDATATHRESHOLD;
        }
    }
}
