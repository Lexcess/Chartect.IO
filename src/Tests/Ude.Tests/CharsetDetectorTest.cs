namespace Ude.Tests
{
    using System;
    using System.IO;
    using System.Text;
    using NUnit.Framework;
    using Ude;

    [TestFixture]
    public class CharsetDetectorTest
    {
        private ICharsetDetector detector;

        [SetUpAttribute]
        public void SetUp()
        {
            this.detector = new CharsetDetector();
        }

        [TearDownAttribute]
        public void TearDown()
        {
            this.detector = null;
        }

        [Test]
        public void TestASCII()
        {
            string s =
                "The Documentation of the libraries is not complete " +
                "and your contributions would be greatly appreciated " +
                "the documentation you want to contribute to and " +
                "click on the [Edit] link to start writing";
            using (MemoryStream ms = new MemoryStream(Encoding.ASCII.GetBytes(s)))
            {
                this.detector.Feed(ms);
                this.detector.DataEnd();
                Assert.AreEqual(Charsets.ASCII, this.detector.Charset);
                Assert.AreEqual(1.0f, this.detector.Confidence);
            }
        }

        [Test]
        public void TestUTF8_1()
        {
            string s = "ウィキペディアはオープンコンテントの百科事典です。基本方針に賛同し" +
                       "ていただけるなら、誰でも記事を編集したり新しく作成したりできます。" +
                       "ガイドブックを読んでから、サンドボックスで練習してみましょう。質問は" +
                       "利用案内でどうぞ。";
            byte[] buf = Encoding.UTF8.GetBytes(s);
            this.detector.Feed(buf, 0, buf.Length);
            this.detector.DataEnd();
            Assert.AreEqual(Charsets.UTF8, this.detector.Charset);
            Assert.AreEqual(1.0f, this.detector.Confidence);
        }

        [Test]
        public void TestBomUTF8()
        {
            byte[] buf = { 0xEF, 0xBB, 0xBF, 0x68, 0x65, 0x6C, 0x6C, 0x6F, 0x21 };
            this.detector.Feed(buf, 0, buf.Length);
            this.detector.DataEnd();
            Assert.AreEqual(Charsets.UTF8, this.detector.Charset);
            Assert.AreEqual(1.0f, this.detector.Confidence);
        }

        [Test]
        public void TestBomUTF16_BE()
        {
            byte[] buf = { 0xFE, 0xFF, 0x00, 0x68, 0x00, 0x65 };
            this.detector = new CharsetDetector();
            this.detector.Feed(buf, 0, buf.Length);
            this.detector.DataEnd();
            Assert.AreEqual(Charsets.UTF16BE, this.detector.Charset);
            Assert.AreEqual(1.0f, this.detector.Confidence);
        }

        [Test]
        public void TestBomUTF16_LE()
        {
            byte[] buf = { 0xFF, 0xFE, 0x68, 0x00, 0x65, 0x00 };
            this.detector.Feed(buf, 0, buf.Length);
            this.detector.DataEnd();
            Assert.AreEqual(Charsets.UTF16LE, this.detector.Charset);
            Assert.AreEqual(1.0f, this.detector.Confidence);
        }

        [Test]
        public void TestBomUTF32_BE()
        {
            byte[] buf = { 0x00, 0x00, 0xFE, 0xFF, 0x00, 0x00, 0x00, 0x68 };
            this.detector.Feed(buf, 0, buf.Length);
            this.detector.DataEnd();
            Assert.AreEqual(Charsets.UTF32BE, this.detector.Charset);
            Assert.AreEqual(1.0f, this.detector.Confidence);
        }

        [Test]
        public void TestBomUTF32_LE()
        {
            byte[] buf = { 0xFF, 0xFE, 0x00, 0x00, 0x68, 0x00, 0x00, 0x00 };
            this.detector.Feed(buf, 0, buf.Length);
            this.detector.DataEnd();
            Assert.AreEqual(Charsets.UTF32LE, this.detector.Charset);
            Assert.AreEqual(1.0f, this.detector.Confidence);
        }

        [Test]
        public void TestIssue3()
        {
            byte[] buf = Encoding.UTF8.GetBytes("3");
            this.detector.Feed(buf, 0, buf.Length);
            this.detector.DataEnd();
            Assert.AreEqual(Charsets.ASCII, this.detector.Charset);
            Assert.AreEqual(1.0f, this.detector.Confidence);
        }
    }
}
