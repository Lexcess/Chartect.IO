namespace Chartect.IO
{
    using System;
    using System.Collections.Generic;
    using Chartect.IO.Core;

    public class StringDetector
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
        public void Read(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var chars = input.ToCharArray();
            var bytes = new List<byte>(chars.Length * 2);
            foreach (char c in chars)
            {
                bytes.AddRange(BitConverter.GetBytes(c));
            }

            var array = bytes.ToArray();

            this.universalDetector.Read(array, 0, array.Length);
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
