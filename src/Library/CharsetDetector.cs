namespace Chartect
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
    ///
    ///  or by a byte a array:
    ///
    /// <example>
    /// <code>
    /// byte[] buff = new byte[1024];
    /// int read;
    /// while ((read = stream.Read(buff, 0, buff.Length)) > 0 && !done)
    ///     Read(buff, 0, read);
    /// cdet.DataEnd();
    /// Console.WriteLine("{0}, {1}", cdet.Charset, cdet.Confidence);
    /// </code>
    /// </example>
    /// </summary>
    public class CharsetDetector : UniversalDetector
    {
        private string charset;

        private float confidence;

        // public event DetectorFinished Finished;
        public CharsetDetector()
            : base(FilterAll)
        {
        }

        /// <summary>
        /// The detected charset. It can be null.
        /// </summary>
        public string Charset
        {
            get { return this.charset; }
        }

        /// <summary>
        /// The confidence of the detected charset, if any
        /// </summary>
        public float Confidence
        {
            get { return this.confidence; }
        }

        /// <summary>
        /// Read a bytes stream to the detector.
        /// </summary>
        /// <param name="stream">an input stream</param>
        public void Read(Stream stream)
        {
            byte[] buffer = new byte[1024];
            int read;
            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0 && !this.Done)
            {
                this.Read(buffer, 0, read);
            }
        }

        /// <summary>
        /// Returns true if the detector has found a result and it is sure about it.
        /// </summary>
        /// <returns>true if the detector has detected the encoding</returns>
        public bool IsDone()
        {
            return this.Done;
        }

        /// <summary>
        /// Resets the state of the detector.
        /// </summary>
        public override void Reset()
        {
            this.charset = null;
            this.confidence = 0.0f;
            base.Reset();
        }

        protected override void Report(string charset, float confidence)
        {
            this.charset = charset;
            this.confidence = confidence;

            // if (Finished != null)
            // {
            // Finished(charset, confidence);
            // }
        }
    }

    // public delegate void DetectorFinished(string charset, float confidence);
}