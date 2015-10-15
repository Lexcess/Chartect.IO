// Escaped charsets state machines
namespace Chartect.IO.Core
{
    using System;

    public class Iso2022CnsmEscapedModel : StateMachineModel
    {
        private static readonly int[] ModelClassTable =
        {
            BitPackage.Pack4bits(2, 0, 0, 0, 0, 0, 0, 0),  // 00 - 07
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // 08 - 0f
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // 10 - 17
            BitPackage.Pack4bits(0, 0, 0, 1, 0, 0, 0, 0),  // 18 - 1f
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // 20 - 27
            BitPackage.Pack4bits(0, 3, 0, 0, 0, 0, 0, 0),  // 28 - 2f
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // 30 - 37
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // 38 - 3f
            BitPackage.Pack4bits(0, 0, 0, 4, 0, 0, 0, 0),  // 40 - 47
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // 48 - 4f
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // 50 - 57
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // 58 - 5f
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // 60 - 67
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // 68 - 6f
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // 70 - 77
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // 78 - 7f
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // 80 - 87
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // 88 - 8f
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // 90 - 97
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // 98 - 9f
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // a0 - a7
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // a8 - af
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // b0 - b7
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // b8 - bf
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // c0 - c7
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // c8 - cf
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // d0 - d7
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // d8 - df
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // e0 - e7
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // e8 - ef
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // f0 - f7
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2) // f8 - ff
        };

        private static readonly int[] ModelStateTable =
        {
                BitPackage.Pack4bits(Start,    3, Error, Start, Start, Start, Start, Start), // 00-07
                BitPackage.Pack4bits(Start, Error, Error, Error, Error, Error, Error, Error), // 08-0f
                BitPackage.Pack4bits(Error, Error, ItsMe, ItsMe, ItsMe, ItsMe, ItsMe, ItsMe), // 10-17
                BitPackage.Pack4bits(ItsMe, ItsMe, ItsMe, Error, Error, Error,    4, Error), // 18-1f
                BitPackage.Pack4bits(Error, Error, Error, ItsMe, Error, Error, Error, Error), // 20-27
                BitPackage.Pack4bits(5,   6, Error, Error, Error, Error, Error, Error), // 28-2f
                BitPackage.Pack4bits(Error, Error, Error, ItsMe, Error, Error, Error, Error), // 30-37
                BitPackage.Pack4bits(Error, Error, Error, Error, Error, ItsMe, Error, Start) // 38-3f
        };

        private static readonly int[] CharacterLengthTable = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public Iso2022CnsmEscapedModel()
            : base(
                    ModelClassTable.To4BitPackage(),
                    9,
                    ModelStateTable.To4BitPackage(),
                    CharacterLengthTable,
                    Charsets.ISO2022CN)
        {
        }
    }
}
