namespace Chartect.IO.Tests
{
    using System.Text;
    using Xunit;

    internal class ArrayDetectorTest
    {
        [Theory]
        public void AsciiShouldBeDetected()
        {
            var s = "The Documentation of the libraries is not complete and your contributions would be greatly appreciated " +
                "the documentation you want to contribute to and click on the [Edit] link to start writing";
            var detector = new ArrayDetector();
            byte[] array = Encoding.UTF8.GetBytes(s);
            detector.Read(array, 0, array.Length);
            detector.DataEnd();
            Assert.Equal(Charsets.Ascii, detector.Charset);
            Assert.Equal(1.0f, detector.Confidence);
        }

        [Fact]
        public void TestUtf81()
        {
            var detector = new ArrayDetector();
            var s = "ウィキペディアはオープンコンテントの百科事典です。基本方針に賛同し" +
                           "ていただけるなら、誰でも記事を編集したり新しく作成したりできます。" +
                           "ガイドブックを読んでから、サンドボックスで練習してみましょう。質問は" +
                           "利用案内でどうぞ。";
            byte[] input = Encoding.UTF8.GetBytes(s);
            detector.Read(input, 0, input.Length);
            detector.DataEnd();
            Assert.Equal(Charsets.Utf8, detector.Charset);
            Assert.Equal(1.0f, detector.Confidence);
        }

        [Fact]
        public void TestBomUtf8()
        {
            var detector = new ArrayDetector();
            byte[] input = { 0xEF, 0xBB, 0xBF, 0x68, 0x65, 0x6C, 0x6C, 0x6F, 0x21 };
            detector.Read(input, 0, input.Length);
            detector.DataEnd();
            Assert.Equal(Charsets.Utf8, detector.Charset);
            Assert.Equal(1.0f, detector.Confidence);
        }

        [Fact]
        public void TestBomUtf16BE()
        {
            var detector = new ArrayDetector();
            byte[] input = { 0xFE, 0xFF, 0x00, 0x68, 0x00, 0x65 };
            detector.Read(input, 0, input.Length);
            detector.DataEnd();
            Assert.Equal(Charsets.Utf16BE, detector.Charset);
            Assert.Equal(1.0f, detector.Confidence);
        }

        [Fact]
        public void TestBomUtf16LE()
        {
            var detector = new ArrayDetector();
            byte[] input = { 0xFF, 0xFE, 0x68, 0x00, 0x65, 0x00 };
            detector.Read(input, 0, input.Length);
            detector.DataEnd();
            Assert.Equal(Charsets.Utf16LE, detector.Charset);
            Assert.Equal(1.0f, detector.Confidence);
        }

        [Fact]
        public void TestBomUTF32_BE()
        {
            var detector = new ArrayDetector();
            byte[] input = { 0x00, 0x00, 0xFE, 0xFF, 0x00, 0x00, 0x00, 0x68 };
            detector.Read(input, 0, input.Length);
            detector.DataEnd();
            Assert.Equal(Charsets.Utf32BE, detector.Charset);
            Assert.Equal(1.0f, detector.Confidence);
        }

        [Fact]
        public void TestBomUTF32_LE()
        {
            var detector = new ArrayDetector();
            byte[] input = { 0xFF, 0xFE, 0x00, 0x00, 0x68, 0x00, 0x00, 0x00 };
            detector.Read(input, 0, input.Length);
            detector.DataEnd();
            Assert.Equal(Charsets.Utf32LE, detector.Charset);
            Assert.Equal(1.0f, detector.Confidence);
        }

        [Fact]
        public void TestIssue3()
        {
            var detector = new ArrayDetector();
            byte[] buf = Encoding.UTF8.GetBytes("3");
            detector.Read(buf, 0, buf.Length);
            detector.DataEnd();
            Assert.Equal(Charsets.Ascii, detector.Charset);
            Assert.Equal(1.0f, detector.Confidence);
        }
    }
}
