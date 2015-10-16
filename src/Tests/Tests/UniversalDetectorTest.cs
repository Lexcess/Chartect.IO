namespace Chartect.IO.Tests
{
    using System;
    using System.IO;
    using System.Text;
    using Chartect;
    using Core;
    using NUnit.Framework;

    [TestFixture]
    public class UniversalDetectorTest
    {
        private UniversalDetector detector;

        [SetUpAttribute]
        public void SetUp()
        {
            this.detector = new UniversalDetector();
        }

        [TearDownAttribute]
        public void TearDown()
        {
            this.detector = null;
        }

        [Test]
        public void TestAscii()
        {
            string s =
                "The Documentation of the libraries is not complete " +
                "and your contributions would be greatly appreciated " +
                "the documentation you want to contribute to and " +
                "click on the [Edit] link to start writing";
            byte[] input = Encoding.UTF8.GetBytes(s);
            this.detector.Read(input, 0, input.Length);
            this.detector.DataEnd();
            Assert.AreEqual(Charsets.Ascii, this.detector.Charset);
            Assert.AreEqual(1.0f, this.detector.Confidence);
        }

        [Test]
        public void TestUtf81()
        {
            string s = "ウィキペディアはオープンコンテントの百科事典です。基本方針に賛同し" +
                       "ていただけるなら、誰でも記事を編集したり新しく作成したりできます。" +
                       "ガイドブックを読んでから、サンドボックスで練習してみましょう。質問は" +
                       "利用案内でどうぞ。";
            byte[] input = Encoding.UTF8.GetBytes(s);
            this.detector.Read(input, 0, input.Length);
            this.detector.DataEnd();
            Assert.AreEqual(Charsets.Utf8, this.detector.Charset);
            Assert.AreEqual(1.0f, this.detector.Confidence);
        }

        [Test]
        public void TestBomUtf8()
        {
            byte[] input = { 0xEF, 0xBB, 0xBF, 0x68, 0x65, 0x6C, 0x6C, 0x6F, 0x21 };
            this.detector.Read(input, 0, input.Length);
            this.detector.DataEnd();
            Assert.AreEqual(Charsets.Utf8, this.detector.Charset);
            Assert.AreEqual(1.0f, this.detector.Confidence);
        }

        [Test]
        public void TestBomUtf16BE()
        {
            byte[] input = { 0xFE, 0xFF, 0x00, 0x68, 0x00, 0x65 };
            this.detector.Read(input, 0, input.Length);
            this.detector.DataEnd();
            Assert.AreEqual(Charsets.UTF16BE, this.detector.Charset);
            Assert.AreEqual(1.0f, this.detector.Confidence);
        }

        [Test]
        public void TestBomUtf16LE()
        {
            byte[] input = { 0xFF, 0xFE, 0x68, 0x00, 0x65, 0x00 };
            this.detector.Read(input, 0, input.Length);
            this.detector.DataEnd();
            Assert.AreEqual(Charsets.Utf16LE, this.detector.Charset);
            Assert.AreEqual(1.0f, this.detector.Confidence);
        }

        [Test]
        public void TestBomUTF32_BE()
        {
            byte[] buf = { 0x00, 0x00, 0xFE, 0xFF, 0x00, 0x00, 0x00, 0x68 };
            this.detector.Read(buf, 0, buf.Length);
            this.detector.DataEnd();
            Assert.AreEqual(Charsets.UTF32BE, this.detector.Charset);
            Assert.AreEqual(1.0f, this.detector.Confidence);
        }

        [Test]
        public void TestBomUTF32_LE()
        {
            byte[] buf = { 0xFF, 0xFE, 0x00, 0x00, 0x68, 0x00, 0x00, 0x00 };
            this.detector.Read(buf, 0, buf.Length);
            this.detector.DataEnd();
            Assert.AreEqual(Charsets.UTF32LE, this.detector.Charset);
            Assert.AreEqual(1.0f, this.detector.Confidence);
        }

        [Test]
        public void TestIssue3()
        {
            byte[] buf = Encoding.UTF8.GetBytes("3");
            this.detector.Read(buf, 0, buf.Length);
            this.detector.DataEnd();
            Assert.AreEqual(Charsets.Ascii, this.detector.Charset);
            Assert.AreEqual(1.0f, this.detector.Confidence);
        }
    }
}
