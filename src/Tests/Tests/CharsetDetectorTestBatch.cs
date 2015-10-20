namespace Chartect.IO.Tests
{
    using System;
    using System.IO;
    using Chartect;
    using NUnit.Framework;

    [TestFixture]
    public class CharsetDetectorTestBatch
    {
        private const string DataRoot = "../../Data";

        private StreamDetector detector;

        [SetUpAttribute]
        public void SetUp()
        {
            this.detector = new StreamDetector();
        }

        [TearDownAttribute]
        public void TearDown()
        {
            this.detector = null;
        }

        [Test]
        public void Latin1Test()
        {
            this.Process(Charsets.Win1252, "latin1");
        }

        [Test]
        public void GB18030CjkTest()
        {
            this.Process(Charsets.GB18030, "gb18030");
        }

        [Test]
        public void Big5CjkTest()
        {
            this.Process(Charsets.Big5, "big5");
        }

        [Test]
        public void ShiftJisCjkTest()
        {
            this.Process(Charsets.ShiftJis, "shiftjis");
        }

        [Test]
        public void EucjpCjkTest()
        {
            this.Process(Charsets.EucJP, "eucjp");
        }

        [Test]
        public void EuckrCjKTest()
        {
            this.Process(Charsets.EucKR, "euckr");
        }

        [Test]
        public void EuctwCjkTest()
        {
            this.Process(Charsets.EucTW, "euctw");
        }

        [Test]
        public void Iso2022JPCjkTest()
        {
            this.Process(Charsets.Iso2022JP, "iso2022jp");
        }

        [Test]
        public void Iso2022KRCjkTest()
        {
            this.Process(Charsets.Iso2022KR, "iso2022kr");
        }

        [Test]
        public void HebrewTest()
        {
            this.Process(Charsets.Win1255, "windows1255");
        }

        [Test]
        public void GreekTest()
        {
            this.Process(Charsets.Iso88597, "iso88597");
        }

        public void Win1253GreekTest()
        {
            // broken detection
            this.Process(Charsets.Win1253, "windows1253");
        }

        [Test]
        public void Win1251CyrillicTest()
        {
            this.Process(Charsets.Win1251, "windows1251");
        }

        [Test]
        public void Koi8rCyrillicTest()
        {
            this.Process(Charsets.Koi8R, "koi8r");
        }

        [Test]
        public void Ibm855CyrillicTest()
        {
            this.Process(Charsets.IBM855, "ibm855");
        }

        [Test]
        public void Ibm866CyrillicTest()
        {
            this.Process(Charsets.Ibm866, "ibm866");
        }

        [Test]
        public void MacCyrillicTest()
        {
            this.Process(Charsets.MacCyrillic, "maccyrillic");
        }

        [Test]
        public void UTF8Test()
        {
            this.Process(Charsets.Utf8, "utf8");
        }

        private void Process(string charset, string dirname)
        {
            string path = Path.Combine(DataRoot, dirname);
            if (!Directory.Exists(path))
            {
                Assert.Fail($"File path not found: {path}");
            }

            string[] files = Directory.GetFiles(path);

            foreach (string file in files)
            {
                using (FileStream fs = new FileStream(file, FileMode.Open))
                {
                    Console.WriteLine($"Analysing {file}");
                    this.detector.Read(fs);
                    this.detector.DataEnd();
                    Console.WriteLine($"{file} : {this.detector.Charset} {this.detector.Confidence}");
                    Assert.AreEqual(charset, this.detector.Charset);
                    this.detector.Reset();
                }
            }
        }
    }
}
