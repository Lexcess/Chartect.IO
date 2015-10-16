namespace Chartect
{
    using System;
    using System.IO;

    public class Chartect
    {
        /// <summary>
        /// Command line example: detects the encoding of the given file.
        /// </summary>
        /// <param name="args">a filename</param>
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: chartect <filename>");
                return;
            }

            string filename = args[0];
            using (FileStream stream = File.OpenRead(filename))
            {
                var detector = new CharsetDetector();
                detector.Read(stream);
                detector.DataEnd();
                if (detector.Charset != null)
                {
                    Console.WriteLine($"Charset: {detector.Charset}, confidence: {detector.Confidence}");
                }
                else
                {
                    Console.WriteLine("Detection failed.");
                }
            }
        }
    }
}
