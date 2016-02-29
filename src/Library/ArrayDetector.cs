namespace Chartect.IO
{
    using Chartect.IO.Core;

    public class ArrayDetector
    {
        private readonly CharsetDetector universalDetector = new CharsetDetector();

        /// <summary>
        /// Gets the detected charset. It can be null.
        /// </summary>
        public string Charset
        {
            get { return this.universalDetector.Charset; }
        }

        /// <summary>
        /// Gets the confidence of the detected charset, if any
        /// </summary>
        public float Confidence
        {
            get { return this.universalDetector.Confidence; }
        }

        /// <summary>
        /// Read a byte array to the detector.
        /// </summary>
        /// <param name="input"> An array of bytes</param>
        public void Read(byte[] input)
        {
            this.Read(input, 0, input.Length);
        }

        /// <summary>
        /// Read a byte array to the detector.
        /// </summary>
        /// <param name="input"> An array of bytes</param>
        /// <param name="offset"> The array offset</param>
        /// <param name="length"> The length of bytes to select from the array.</param>
        public void Read(byte[] input, int offset, int length)
        {
            this.universalDetector.Read(input, 0, length);
        }

        /// <summary>
        /// Tells the detector that no more input is coming and it has to make a decision.
        /// </summary>
        public void DataEnd()
        {
            this.universalDetector.DataEnd();
        }

        /// <summary>
        /// Returns true if the detector has found a result and it is sure about it.
        /// </summary>
        /// <returns>true if the detector has detected the encoding</returns>
        public bool IsDone()
        {
            return this.universalDetector.DetectorState == DetectorState.Done;
        }

        /// <summary>
        /// Resets the state of the detector.
        /// </summary>
        public void Reset()
        {
            this.universalDetector.Reset();
        }
    }
}
