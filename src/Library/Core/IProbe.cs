namespace Chartect.IO.Core
{
    internal interface IProbe
    {
        /// <summary>
        /// Read data to the probe
        /// </summary>
        /// <param name="buffer">a buffer</param>
        /// <param name="offset">offset into buffer</param>
        /// <param name="length">number of bytes available into buffer</param>
        /// <returns>
        /// A <see cref="ProbingState"/>
        /// </returns>
        ProbingState HandleData(byte[] buffer, int offset, int length);

        /// <summary>
        /// Reset prober state
        /// </summary>
        void Reset();

        string GetCharsetName();

        float GetConfidence();

        ProbingState GetState();

        void DumpStatus();
    }
}
