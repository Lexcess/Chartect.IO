using System.IO;

namespace Ude
{
    public interface ICharsetDetector
    {

        /// <summary>
        /// The detected charset. It can be null.
        /// </summary>
        string Charset { get; }
        
        /// <summary>
        /// The confidence of the detected charset, if any 
        /// </summary>
        float Confidence { get; }
        
        /// <summary>
        /// Feed a block of bytes to the detector. 
        /// </summary>
        /// <param name="buf">input buffer</param>
        /// <param name="offset">offset into buffer</param>
        /// <param name="len">number of available bytes</param>
        void Feed(byte[] buf, int offset, int len);
        
        /// <summary>
        /// Feed a bytes stream to the detector. 
        /// </summary>
        /// <param name="stream">an input stream</param>
        void Feed(Stream stream);

        /// <summary>
        /// Resets the state of the detector. 
        /// </summary>        
        void Reset();
        
        /// <summary>
        /// Returns true if the detector has found a result and it is sure about it.
        /// </summary>
        /// <returns>true if the detector has detected the encoding</returns>
        bool IsDone();

        /// <summary>
        /// Tell the detector that there is no more data and it must take its
        /// decision.
        /// </summary>
        void DataEnd();
        
    }
}
