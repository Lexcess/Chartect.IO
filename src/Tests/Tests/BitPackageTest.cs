namespace Chartect.IO.Tests
{
    using Chartect.IO.Core;
    using Xunit;

    public class BitPackageTest
    {
        [Fact]
        public void ShouldPackCorrectly()
        {
            Assert.Equal(BitPackage.Pack4bits(0, 0, 0, 0, 0, 0, 0, 0), 0);
            Assert.Equal(BitPackage.Pack4bits(1, 1, 1, 1, 1, 1, 1, 1), 286331153);
            Assert.Equal(BitPackage.Pack4bits(2, 2, 2, 2, 2, 2, 2, 2), 572662306);
            Assert.Equal(BitPackage.Pack4bits(15, 15, 15, 15, 15, 15, 15, 15), -1);
        }

        [Fact]
        public void ShouldUnpackCorrectly()
        {
            var data = new int[]
            {
                BitPackage.Pack4bits(0, 1,  2,  3,  4,  5,  6,  7),
                BitPackage.Pack4bits(8, 9, 10, 11, 12, 13, 14, 15)
            };

            var package = new BitPackage(
                    BitPackage.IndexShift4Bits,
                    BitPackage.ShiftMask4Bits,
                    BitPackage.BitShift4Bits,
                    BitPackage.UnitMask4Bits,
                    data);

            for (int i = 0; i < 16; i++)
            {
                int actual = package.Unpack(i);
                Assert.Equal(actual, i);
            }
        }
    }
}
