namespace Chartect.IO
{
    using System;
    using System.IO;

    using Chartect.IO.Core;

    /// <summary>
    /// Default implementation of charset detection interface.
    /// The detector can be fed by a System.IO.Stream:
    /// <example>
    /// <code>
    /// using (FileStream fs = File.OpenRead(filename)) {
    ///    CharsetDetector cdet = new CharsetDetector();
    ///    cdet.Read(fs);
    ///    cdet.DataEnd();
    ///    Console.WriteLine("{0}, {1}", cdet.Charset, cdet.Confidence);
    /// </code>
    /// </example>
    /// </summary>
    public sealed class StreamDetector
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
        /// Read a bytes stream to the detector.
        /// </summary>
        /// <param name="stream">an input stream</param>
        public void Read(Stream stream)
        {
            byte[] buffer = new byte[1024];
            int read;
            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0 && this.universalDetector.DetectorState != DetectorState.Done)
            {
                this.universalDetector.Read(buffer, 0, read);
            }
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

    // public delegate void DetectorFinished(string charset, float confidence);
}