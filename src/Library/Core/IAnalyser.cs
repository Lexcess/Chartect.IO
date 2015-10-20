namespace Chartect.IO.Core
{
    internal interface IAnalyser
    {
        void HandleOneChar(byte[] input, int offset, int characterLength);

        void HandleData(byte[] input, int offset, int length);

        int GetOrder(byte[] input, int offset, out int characterLength);

        int GetOrder(byte[] input, int offset);
    }
}
