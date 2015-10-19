namespace Chartect.IO.Core
{
    using System;

    internal class EUCKRSMModel : StateMachineModel
    {
        private static readonly int[] ModelClassTable =
        {
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1), // 00 - 07
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 0, 0), // 08 - 0f
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1), // 10 - 17
            BitPackage.Pack4bits(1, 1, 1, 0, 1, 1, 1, 1), // 18 - 1f
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1), // 20 - 27
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1), // 28 - 2f
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1), // 30 - 37
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1), // 38 - 3f
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1), // 40 - 47
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1), // 48 - 4f
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1), // 50 - 57
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1), // 58 - 5f
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1), // 60 - 67
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1), // 68 - 6f
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1), // 70 - 77
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1), // 78 - 7f
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // 80 - 87
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // 88 - 8f
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // 90 - 97
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), // 98 - 9f
            BitPackage.Pack4bits(0, 2, 2, 2, 2, 2, 2, 2), // a0 - a7
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 3, 3, 3), // a8 - af
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2), // b0 - b7
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2), // b8 - bf
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2), // c0 - c7
            BitPackage.Pack4bits(2, 3, 2, 2, 2, 2, 2, 2), // c8 - cf
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2), // d0 - d7
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2), // d8 - df
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2), // e0 - e7
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2), // e8 - ef
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2), // f0 - f7
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 0), // f8 - ff
        };

        private static readonly int[] ModelStateTable =
        {
            BitPackage.Pack4bits(Error, Start,     3, Error, Error, Error, Error, Error), // 00-07
            BitPackage.Pack4bits(ItsMe, ItsMe, ItsMe, ItsMe, Error, Error, Start, Start), // 08-0f
        };

        private static readonly int[] CharacterLengthTable = { 0, 1, 2, 0 };

        public EUCKRSMModel()
            : base(
              ModelClassTable.To4BitPackage(),
              4,
              ModelStateTable.To4BitPackage(),
              CharacterLengthTable,
              Charsets.EUCKR)
        {
        }
    }
}
