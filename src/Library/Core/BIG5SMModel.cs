namespace Chartect.IO.Core
{
    using System;

    public class Big5SMModel : StateMachineModel
    {
        private static readonly int[] Big5Cls =
        {
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
            BitPackage.Pack4bits(4, 4, 4, 4, 4, 4, 4, 4),  // 80 - 87
            BitPackage.Pack4bits(4, 4, 4, 4, 4, 4, 4, 4),  // 88 - 8f
            BitPackage.Pack4bits(4, 4, 4, 4, 4, 4, 4, 4),  // 90 - 97
            BitPackage.Pack4bits(4, 4, 4, 4, 4, 4, 4, 4),  // 98 - 9f
            BitPackage.Pack4bits(4, 3, 3, 3, 3, 3, 3, 3),  // a0 - a7
            BitPackage.Pack4bits(3, 3, 3, 3, 3, 3, 3, 3),  // a8 - af
            BitPackage.Pack4bits(3, 3, 3, 3, 3, 3, 3, 3),  // b0 - b7
            BitPackage.Pack4bits(3, 3, 3, 3, 3, 3, 3, 3),  // b8 - bf
            BitPackage.Pack4bits(3, 3, 3, 3, 3, 3, 3, 3),  // c0 - c7
            BitPackage.Pack4bits(3, 3, 3, 3, 3, 3, 3, 3),  // c8 - cf
            BitPackage.Pack4bits(3, 3, 3, 3, 3, 3, 3, 3),  // d0 - d7
            BitPackage.Pack4bits(3, 3, 3, 3, 3, 3, 3, 3),  // d8 - df
            BitPackage.Pack4bits(3, 3, 3, 3, 3, 3, 3, 3),  // e0 - e7
            BitPackage.Pack4bits(3, 3, 3, 3, 3, 3, 3, 3),  // e8 - ef
            BitPackage.Pack4bits(3, 3, 3, 3, 3, 3, 3, 3),  // f0 - f7
            BitPackage.Pack4bits(3, 3, 3, 3, 3, 3, 3, 0) // f8 - ff
        };

        private static readonly int[] BIG5St =
        {
            BitPackage.Pack4bits(Error, Start, Start,    3, Error, Error, Error, Error), // 00-07
            BitPackage.Pack4bits(Error, Error, ItsMe, ItsMe, ItsMe, ItsMe, ItsMe, Error), // 08-0f
            BitPackage.Pack4bits(Error, Start, Start, Start, Start, Start, Start, Start) // 10-17
        };

        private static readonly int[] Big5CharLenTable = { 0, 1, 1, 2, 0 };

        public Big5SMModel()
            : base(
              Big5Cls.To4BitPackage(),
              5,
              BIG5St.To4BitPackage(),
              Big5CharLenTable,
              "Big5")
        {
        }
    }
}
