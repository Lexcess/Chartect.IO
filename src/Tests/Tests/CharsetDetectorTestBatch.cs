namespace Chartect.IO.Tests
{
    using System.Diagnostics;
    using System.IO;
    using Xunit;

    public class CharsetDetectorTestBatch
    {
        private const string DataRoot = "../../Data";

        [Fact]
        public void Latin1Test()
        {
            this.Process(Charsets.Win1252, "latin1");
        }

        [Fact]
        public void GB18030CjkTest()
        {
            this.Process(Charsets.GB18030, "gb18030");
        }

        [Fact]
        public void Big5CjkTest()
        {
            this.Process(Charsets.Big5, "big5");
        }

        [Fact]
        public void ShiftJisCjkTest()
        {
            this.Process(Charsets.ShiftJis, "shiftjis");
        }

        [Fact]
        public void EucjpCjkTest()
        {
            this.Process(Charsets.EucJP, "eucjp");
        }

        [Fact]
        public void EuckrCjkTest()
        {
            this.Process(Charsets.EucKR, "euckr");
        }

        [Fact]
        public void EuctwCjkTest()
        {
            this.Process(Charsets.EucTW, "euctw");
        }

        [Fact]
        public void Iso2022JPCjkTest()
        {
            this.Process(Charsets.Iso2022JP, "iso2022jp");
        }

        [Fact]
        public void Iso2022KRCjkTest()
        {
            this.Process(Charsets.Iso2022KR, "iso2022kr");
        }

        [Fact]
        public void HebrewTest()
        {
            this.Process(Charsets.Win1255, "windows1255");
        }

        [Fact]
        public void GreekTest()
        {
            this.Process(Charsets.Iso88597, "iso88597");
        }

        public void Win1253GreekTest()
        {
            // broken detection
            this.Process(Charsets.Win1253, "windows1253");
        }

        [Fact]
        public void Win1251CyrillicTest()
        {
            this.Process(Charsets.Win1251, "windows1251");
        }

        [Fact]
        public void Koi8RCyrillicTest()
        {
            this.Process(Charsets.Koi8R, "koi8r");
        }

        [Fact]
        public void Ibm855CyrillicTest()
        {
            this.Process(Charsets.Ibm855, "ibm855");
        }

        [Fact]
        public void Ibm866CyrillicTest()
        {
            this.Process(Charsets.Ibm866, "ibm866");
        }

        [Fact]
        public void MacCyrillicTest()
        {
            this.Process(Charsets.MacCyrillic, "maccyrillic");
        }

        [Fact]
        public void Utf8Test()
        {
            this.Process(Charsets.Utf8, "utf8");
        }

        private void Process(string expected, string dirname)
        {
            var detector = new StreamDetector();
            var path = Path.Combine(DataRoot, dirname);

            Assert.True(Directory.Exists(path), $"File path not found: {path}");

            string[] files = Directory.GetFiles(path);

            foreach (string file in files)
            {
                using (FileStream fs = new FileStream(file, FileMode.Open))
                {
                    Debug.WriteLine($"Analyzing {file}");
                    detector.Read(fs);
                    detector.DataEnd();
                    Debug.WriteLine($"{file} : {detector.Charset} {detector.Confidence}");
                    Assert.Equal(expected, detector.Charset);
                    detector.Reset();
                }
            }

            detector = null;
        }
    }
}
