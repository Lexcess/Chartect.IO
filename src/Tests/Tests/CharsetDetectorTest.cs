namespace Chartect.IO.Tests
{
    using System.IO;
    using System.Text;
    using Chartect;
    using NUnit.Framework;

    [TestFixture]
    public class CharsetDetectorTest
    {
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
        public void TestAsciiStream()
        {
            string s =
                "The Documentation of the libraries is not complete " +
                "and your contributions would be greatly appreciated " +
                "the documentation you want to contribute to and " +
                "click on the [Edit] link to start writing";
            using (MemoryStream ms = new MemoryStream(Encoding.ASCII.GetBytes(s)))
            {
                this.detector.Read(ms);
                this.detector.DataEnd();
                Assert.AreEqual(Charsets.Ascii, this.detector.Charset);
                Assert.AreEqual(1.0f, this.detector.Confidence);
            }
        }
    }
}
