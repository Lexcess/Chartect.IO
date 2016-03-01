namespace Chartect.IO.Tests
{
    using System.IO;
    using System.Text;
    using Xunit;

    public class StreamDetectorTest
    {
        [Fact]
        public void TestAsciiStream()
        {
            string s =
                "The Documentation of the libraries is not complete " +
                "and your contributions would be greatly appreciated " +
                "the documentation you want to contribute to and " +
                "click on the [Edit] link to start writing";
            using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(s)))
            {
                var detector = new StreamDetector();
                var expectedCharset = Charsets.Ascii;
                var expectedConfidence = 1.0f;

                detector.Read(stream);
                detector.DataEnd();
                Assert.Equal(expectedCharset, detector.Charset);
                Assert.Equal(expectedConfidence, detector.Confidence);
            }
        }
    }
}
