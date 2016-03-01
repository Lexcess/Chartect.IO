namespace Chartect.IO.Tests
{
    using Xunit;

    public class StringDetectorTest
    {
        [Fact]
        public void TestAsciiStream()
        {
            string s =
            "The Documentation of the libraries is not complete " +
            "and your contributions would be greatly appreciated " +
            "the documentation you want to contribute to and " +
            "click on the [Edit] link to start writing";

            var detector = new StringDetector();
            var expectedCharset = Charsets.Ascii;
            var expectedConfidence = 1.0f;

            detector.Read(s);
            Assert.Equal(expectedCharset, detector.Charset);
            Assert.Equal(expectedConfidence, detector.Confidence);
        }
    }
}
