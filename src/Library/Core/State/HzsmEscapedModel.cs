// Escaped charsets state machines
namespace Chartect.IO.Core
{
    internal class HzsmEscapedModel : StateMachineModel
    {
        private static readonly int[] ModelClassTable =
        {
            BitPackage.Pack4bits(1, 0, 0, 0, 0, 0, 0, 0), // 00 - 07
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // 08 - 0f
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // 10 - 17
            BitPackage.Pack4bits(0, 0, 0, 1, 0, 0, 0, 0), // 18 - 1f
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // 20 - 27
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // 28 - 2f
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // 30 - 37
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // 38 - 3f
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // 40 - 47
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // 48 - 4f
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // 50 - 57
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // 58 - 5f
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // 60 - 67
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // 68 - 6f
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // 70 - 77
            BitPackage.Pack4bits(0, 0, 0, 4, 0, 5, 2, 0), // 78 - 7f
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1), // 80 - 87
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1), // 88 - 8f
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1), // 90 - 97
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1), // 98 - 9f
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1), // a0 - a7
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1), // a8 - af
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1), // b0 - b7
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1), // b8 - bf
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1), // c0 - c7
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1), // c8 - cf
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1), // d0 - d7
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1), // d8 - df
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1), // e0 - e7
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1), // e8 - ef
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1), // f0 - f7
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1), // f8 - ff
        };

        private static readonly int[] ModelStateTable =
        {
            BitPackage.Pack4bits(Start, Error,     3, Start, Start, Start, Error, Error), // 00-07
            BitPackage.Pack4bits(Error, Error, Error, Error, ItsMe, ItsMe, ItsMe, ItsMe), // 08-0f
            BitPackage.Pack4bits(ItsMe, ItsMe, Error, Error, Start, Start,     4, Error), // 10-17
            BitPackage.Pack4bits(5,     Error,     6, Error,     5,     5,     4, Error), // 18-1f
            BitPackage.Pack4bits(4,     Error,     4,     4,     4, Error,     4, Error), // 20-27
            BitPackage.Pack4bits(4,     ItsMe, Start, Start, Start, Start, Start, Start), // 28-2f
        };

        private static readonly int[] CharacterLengthTable = { 0, 0, 0, 0, 0, 0 };

        public HzsmEscapedModel()
            : base(
              ModelClassTable.To4BitPackage(),
              6,
              ModelStateTable.To4BitPackage(),
              CharacterLengthTable,
              Charsets.Hzgb2312)
        {
        }
    }
}
