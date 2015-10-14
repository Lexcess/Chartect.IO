namespace Chartect.IO.Core
{
    using System;

    public class UTF8SMModel : StateMachineModel
    {
        private static readonly int[] UTF8Cls =
        {
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),  // 00 - 07
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 0, 0),  // 08 - 0f
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),  // 10 - 17
            BitPackage.Pack4bits(1, 1, 1, 0, 1, 1, 1, 1),  // 18 - 1f
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),  // 20 - 27
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),  // 28 - 2f
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),  // 30 - 37
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),  // 38 - 3f
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),  // 40 - 47
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),  // 48 - 4f
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),  // 50 - 57
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),  // 58 - 5f
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),  // 60 - 67
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),  // 68 - 6f
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),  // 70 - 77
            BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1),  // 78 - 7f
            BitPackage.Pack4bits(2, 2, 2, 2, 3, 3, 3, 3),  // 80 - 87
            BitPackage.Pack4bits(4, 4, 4, 4, 4, 4, 4, 4),  // 88 - 8f
            BitPackage.Pack4bits(4, 4, 4, 4, 4, 4, 4, 4),  // 90 - 97
            BitPackage.Pack4bits(4, 4, 4, 4, 4, 4, 4, 4),  // 98 - 9f
            BitPackage.Pack4bits(5, 5, 5, 5, 5, 5, 5, 5),  // a0 - a7
            BitPackage.Pack4bits(5, 5, 5, 5, 5, 5, 5, 5),  // a8 - af
            BitPackage.Pack4bits(5, 5, 5, 5, 5, 5, 5, 5),  // b0 - b7
            BitPackage.Pack4bits(5, 5, 5, 5, 5, 5, 5, 5),  // b8 - bf
            BitPackage.Pack4bits(0, 0, 6, 6, 6, 6, 6, 6),  // c0 - c7
            BitPackage.Pack4bits(6, 6, 6, 6, 6, 6, 6, 6),  // c8 - cf
            BitPackage.Pack4bits(6, 6, 6, 6, 6, 6, 6, 6),  // d0 - d7
            BitPackage.Pack4bits(6, 6, 6, 6, 6, 6, 6, 6),  // d8 - df
            BitPackage.Pack4bits(7, 8, 8, 8, 8, 8, 8, 8),  // e0 - e7
            BitPackage.Pack4bits(8, 8, 8, 8, 8, 9, 8, 8),  // e8 - ef
            BitPackage.Pack4bits(10, 11, 11, 11, 11, 11, 11, 11),  // f0 - f7
            BitPackage.Pack4bits(12, 13, 13, 13, 14, 15, 0, 0) // f8 - ff
        };

        private static readonly int[] UTF8St =
        {
            BitPackage.Pack4bits(Error, Start, Error, Error, Error, Error,   12,   10), // 00-07
            BitPackage.Pack4bits(9,   11,    8,    7,    6,    5,    4,    3), // 08-0f
            BitPackage.Pack4bits(Error, Error, Error, Error, Error, Error, Error, Error), // 10-17
            BitPackage.Pack4bits(Error, Error, Error, Error, Error, Error, Error, Error), // 18-1f
            BitPackage.Pack4bits(ItsMe, ItsMe, ItsMe, ItsMe, ItsMe, ItsMe, ItsMe, ItsMe), // 20-27
            BitPackage.Pack4bits(ItsMe, ItsMe, ItsMe, ItsMe, ItsMe, ItsMe, ItsMe, ItsMe), // 28-2f
            BitPackage.Pack4bits(Error, Error,    5,    5,    5,    5, Error, Error), // 30-37
            BitPackage.Pack4bits(Error, Error, Error, Error, Error, Error, Error, Error), // 38-3f
            BitPackage.Pack4bits(Error, Error, Error,    5,    5,    5, Error, Error), // 40-47
            BitPackage.Pack4bits(Error, Error, Error, Error, Error, Error, Error, Error), // 48-4f
            BitPackage.Pack4bits(Error, Error,    7,    7,    7,    7, Error, Error), // 50-57
            BitPackage.Pack4bits(Error, Error, Error, Error, Error, Error, Error, Error), // 58-5f
            BitPackage.Pack4bits(Error, Error, Error, Error,    7,    7, Error, Error), // 60-67
            BitPackage.Pack4bits(Error, Error, Error, Error, Error, Error, Error, Error), // 68-6f
            BitPackage.Pack4bits(Error, Error,    9,    9,    9,    9, Error, Error), // 70-77
            BitPackage.Pack4bits(Error, Error, Error, Error, Error, Error, Error, Error), // 78-7f
            BitPackage.Pack4bits(Error, Error, Error, Error, Error,    9, Error, Error), // 80-87
            BitPackage.Pack4bits(Error, Error, Error, Error, Error, Error, Error, Error), // 88-8f
            BitPackage.Pack4bits(Error, Error,   12,   12,   12,   12, Error, Error), // 90-97
            BitPackage.Pack4bits(Error, Error, Error, Error, Error, Error, Error, Error), // 98-9f
            BitPackage.Pack4bits(Error, Error, Error, Error, Error,   12, Error, Error), // a0-a7
            BitPackage.Pack4bits(Error, Error, Error, Error, Error, Error, Error, Error), // a8-af
            BitPackage.Pack4bits(Error, Error,   12,   12,   12, Error, Error, Error), // b0-b7
            BitPackage.Pack4bits(Error, Error, Error, Error, Error, Error, Error, Error), // b8-bf
            BitPackage.Pack4bits(Error, Error, Start, Start, Start, Start, Error, Error), // c0-c7
            BitPackage.Pack4bits(Error, Error, Error, Error, Error, Error, Error, Error) // c8-cf
        };

        private static readonly int[] UTF8CharLenTable =
            { 0, 1, 0, 0, 0, 0, 2, 3, 3, 3, 4, 4, 5, 5, 6, 6 };

        public UTF8SMModel()
            : base(
              UTF8Cls.To4BitPackage(),
              16,
              UTF8St.To4BitPackage(),
              UTF8CharLenTable,
              "UTF-8")
        {
        }
    }
}