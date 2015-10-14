// Escaped charsets state machines
namespace Chartect.IO.Core
{
    using System;

    public class Iso2022JpsmEscapedModel : StateMachineModel
    {
        private static readonly int[] ISO2022JPCls =
        {
            BitPackage.Pack4bits(2, 0, 0, 0, 0, 0, 0, 0),  // 00 - 07
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 2, 2),  // 08 - 0f
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // 10 - 17
            BitPackage.Pack4bits(0, 0, 0, 1, 0, 0, 0, 0),  // 18 - 1f
            BitPackage.Pack4bits(0, 0, 0, 0, 7, 0, 0, 0),  // 20 - 27
            BitPackage.Pack4bits(3, 0, 0, 0, 0, 0, 0, 0),  // 28 - 2f
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // 30 - 37
            BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0),  // 38 - 3f
            BitPackage.Pack4bits(6, 0, 4, 0, 8, 0, 0, 0),  // 40 - 47
            BitPackage.Pack4bits(0, 9, 5, 0, 0, 0, 0, 0),  // 48 - 4f
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

        private static readonly int[] ISO2022JPSt =
        {
            BitPackage.Pack4bits(Start,     3, Error, Start, Start, Start, Start, Start), // 00-07
            BitPackage.Pack4bits(Start, Start, Error, Error, Error, Error, Error, Error), // 08-0f
            BitPackage.Pack4bits(Error, Error, Error, Error, ItsMe, ItsMe, ItsMe, ItsMe), // 10-17
            BitPackage.Pack4bits(ItsMe, ItsMe, ItsMe, ItsMe, ItsMe, ItsMe, Error, Error), // 18-1f
            BitPackage.Pack4bits(Error,     5, Error, Error, Error,    4, Error, Error), // 20-27
            BitPackage.Pack4bits(Error, Error, Error,    6, ItsMe, Error, ItsMe, Error), // 28-2f
            BitPackage.Pack4bits(Error, Error, Error, Error, Error, Error, ItsMe, ItsMe), // 30-37
            BitPackage.Pack4bits(Error, Error, Error, ItsMe, Error, Error, Error, Error), // 38-3f
            BitPackage.Pack4bits(Error, Error, Error, Error, ItsMe, Error, Start, Start) // 40-47
        };

        private static readonly int[] ISO2022JPCharLenTable = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public Iso2022JpsmEscapedModel()
            : base(
              ISO2022JPCls.To4BitPackage(),
              10,
              ISO2022JPSt.To4BitPackage(),
              ISO2022JPCharLenTable,
              "ISO-2022-JP")
        {
        }
    }
}
