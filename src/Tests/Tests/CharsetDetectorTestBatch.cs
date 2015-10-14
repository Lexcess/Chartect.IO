namespace Chartect.IO.Tests
{
    using System;
    using System.IO;
    using Chartect;
    using NUnit.Framework;

    [TestFixture]
    public class CharsetDetectorTestBatch
    {
        // Path.GetDirectoryName (Assembly.GetExecutingAssembly ().Location)
        private const string DataRoot = "../../Data";

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
        public void TestLatin1()
        {
            this.Process(Charsets.WIN1252, "latin1");
        }

        [Test]
        public void TestCJK()
        {
            this.Process(Charsets.GB18030, "gb18030");
            this.Process(Charsets.BIG5, "big5");
            this.Process(Charsets.SHIFTJIS, "shiftjis");
            this.Process(Charsets.EUCJP, "eucjp");
            this.Process(Charsets.EUCKR, "euckr");
            this.Process(Charsets.EUCTW, "euctw");
            this.Process(Charsets.ISO2022JP, "iso2022jp");
            this.Process(Charsets.ISO2022KR, "iso2022kr");
        }

        [Test]
        public void TestHebrew()
        {
            this.Process(Charsets.WIN1255, "windows1255");
        }

        [Test]
        public void TestGreek()
        {
            this.Process(Charsets.ISO88597, "iso88597");

            // Process(Charsets.WIN1253, "windows1253");
        }

        [Test]
        public void TestCyrillic()
        {
            this.Process(Charsets.WIN1251, "windows1251");
            this.Process(Charsets.KOI8R, "koi8r");
            this.Process(Charsets.IBM855, "ibm855");
            this.Process(Charsets.IBM866, "ibm866");
            this.Process(Charsets.MACCYRILLIC, "maccyrillic");
        }

        [Test]
        public void TestBulgarian()
        {
        }

        [Test]
        public void TestUTF8()
        {
            this.Process(Charsets.UTF8, "utf8");
        }

        private void Process(string charset, string dirname)
        {
            string path = Path.Combine(DataRoot, dirname);
            if (!Directory.Exists(path))
            {
                return;
            }

            string[] files = Directory.GetFiles(path);

            foreach (string file in files)
            {
                using (FileStream fs = new FileStream(file, FileMode.Open))
                {
                    Console.WriteLine("Analysing {0}", file);
                    this.detector.Feed(fs);
                    this.detector.DataEnd();
                    Console.WriteLine(
                            "{0} : {1} {2}",
                            file,
                            this.detector.Charset,
                            this.detector.Confidence);
                    Assert.AreEqual(charset, this.detector.Charset);
                    this.detector.Reset();
                }
            }
        }
    }
}
