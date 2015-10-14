namespace Chartect.IO.Core
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    internal static class UdeExtensions
    {
        public static BitPackage To4BitPackage(this int[] data)
        {
            return new BitPackage(
                                    BitPackage.IndexShift4Bits,
                                    BitPackage.ShiftMask4Bits,
                                    BitPackage.BitShift4Bits,
                                    BitPackage.UnitMask4Bits,
                                    data);
        }
    }
}
