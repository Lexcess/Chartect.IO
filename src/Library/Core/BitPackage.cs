namespace Chartect.IO.Core
{
    using System;

    public class BitPackage
    {
        public const int IndexShift4Bits = 3;
        public const int IndexShift8Bits = 2;
        public const int IndexShift16Bits = 1;

        public const int ShiftMask4Bits = 7;
        public const int ShiftMask8Bits = 3;
        public const int ShiftMask16Bits = 1;

        public const int BitShift4Bits = 2;
        public const int BitShift8Bits = 3;
        public const int BitShift16Bits = 4;

        public const int UnitMask4Bits = 0x0000000F;
        public const int UnitMask8Bits = 0x000000FF;
        public const int UnitMask16Bits = 0x0000FFFF;

        private int indexShift;
        private int shiftMask;
        private int bitShift;
        private int unitMask;
        private int[] data;

        public BitPackage(int indexShift, int shiftMask, int bitShift, int unitMask, int[] data)
        {
            this.indexShift = indexShift;
            this.shiftMask = shiftMask;
            this.bitShift = bitShift;
            this.unitMask = unitMask;
            this.data = data;
        }

         public static int Pack16bits(int a, int b)
        {
            return (b << 16) | a;
        }

        public static int Pack8bits(int a, int b, int c, int d)
        {
            return Pack16bits((b << 8) | a, (d << 8) | c);
        }

        public static int Pack4bits(int a, int b, int c, int d, int e, int f, int g, int h)
        {
            return Pack8bits((b << 4) | a, (d << 4) | c, (f << 4) | e, (h << 4) | g);
        }

        public int Unpack(int i)
        {
            return (this.data[i >> this.indexShift] >> ((i & this.shiftMask) << this.bitShift)) & this.unitMask;
        }
   }
}
