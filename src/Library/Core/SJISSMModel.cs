namespace Chartect.IO.Core
{
    using System;

    public class SJISSMModel : StateMachineModel
    {
        private static readonly int[] SJISCls =
        {
            // BitPacket.Pack4bits(0,1,1,1,1,1,1,1),  // 00 - 07
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),  // 00 - 07
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 0, 0),  // 08 - 0f
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),  // 10 - 17
            BitPackage.Pack4bits(1, 1, 1, 0, 1, 1, 1, 1),  // 18 - 1f
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),  // 20 - 27
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),  // 28 - 2f
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),  // 30 - 37
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),  // 38 - 3f
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // 40 - 47
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // 48 - 4f
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // 50 - 57
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // 58 - 5f
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // 60 - 67
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // 68 - 6f
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // 70 - 77
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 1),  // 78 - 7f
            BitPackage.Pack4bits(3, 3, 3, 3, 3, 3, 3, 3),  // 80 - 87
            BitPackage.Pack4bits(3, 3, 3, 3, 3, 3, 3, 3),  // 88 - 8f
            BitPackage.Pack4bits(3, 3, 3, 3, 3, 3, 3, 3),  // 90 - 97
            BitPackage.Pack4bits(3, 3, 3, 3, 3, 3, 3, 3),  // 98 - 9f
            // 0xa0 is illegal in sjis encoding, but some pages does
            // contain such byte. We need to be more error forgiven.
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // a0 - a7
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // a8 - af
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // b0 - b7
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // b8 - bf
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // c0 - c7
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // c8 - cf
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // d0 - d7
            BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2),  // d8 - df
            BitPackage.Pack4bits(3, 3, 3, 3, 3, 3, 3, 3),  // e0 - e7
            BitPackage.Pack4bits(3, 3, 3, 3, 3, 4, 4, 4),  // e8 - ef
            BitPackage.Pack4bits(4, 4, 4, 4, 4, 4, 4, 4),  // f0 - f7
            BitPackage.Pack4bits(4, 4, 4, 4, 4, 0, 0, 0) // f8 - ff
        };

        private static readonly int[] SJISSt =
        {
            BitPackage.Pack4bits(Error, Start, Start,    3, Error, Error, Error, Error), // 00-07
            BitPackage.Pack4bits(Error, Error, Error, Error, ItsMe, ItsMe, ItsMe, ItsMe), // 08-0f
            BitPackage.Pack4bits(ItsMe, ItsMe, Error, Error, Start, Start, Start, Start) // 10-17
        };

        private static readonly int[] SJISCharLenTable = { 0, 1, 1, 2, 0, 0 };

        public SJISSMModel()
            : base(
              SJISCls.To4BitPackage(),
              6,
              SJISSt.To4BitPackage(),
              SJISCharLenTable,
              "Shift_JIS")
        {
        }
    }
}
