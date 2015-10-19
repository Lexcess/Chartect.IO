namespace Chartect.IO.Core
{
    internal enum ProbingState
    {
        Detecting = 0,  // no sure answer yet, but caller can ask for confidence
        Detected = 1,   // positive answer
        NegativeDetection = 2 // negative answer
    }

    internal abstract class CharsetProber
    {
        protected const float ShortcutThreshold = 0.95F;

        private ProbingState state = ProbingState.Detecting;

        protected ProbingState State
        {
            get
            {
                return this.state;
            }

            set
            {
                this.state = value;
            }
        }

        /// <summary>
        /// Read data to the prober
        /// </summary>
        /// <param name="buffer">a buffer</param>
        /// <param name="offset">offset into buffer</param>
        /// <param name="len">number of bytes available into buffer</param>
        /// <returns>
        /// A <see cref="ProbingState"/>
        /// </returns>
        public abstract ProbingState HandleData(byte[] buffer, int offset, int len);

        /// <summary>
        /// Reset prober state
        /// </summary>
        public abstract void Reset();

        public abstract string GetCharsetName();

        public abstract float GetConfidence();

        public virtual ProbingState GetState()
        {
            return this.State;
        }

        public virtual void SetOption()
        {
        }

        public virtual void DumpStatus()
        {
        }
    }
}
