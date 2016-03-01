namespace Chartect.IO
{
    using System;
    using System.IO;

    using Chartect.Properties;

    public static class Application
    {
        /// <summary>
        /// Command line example: detects the encoding of the given file.
        /// </summary>
        /// <param name="args">a filename</param>
        public static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                Console.WriteLine(Resources.UsageHelp);
                return;
            }

            string filename = args[0];
            using (FileStream stream = File.OpenRead(filename))
            {
                var detector = new StreamDetector();
                detector.Read(stream);
                detector.DataEnd();
                if (detector.Charset != null)
                {
                    Console.WriteLine(Resources.DetectorSuccessFormat, detector.Charset, detector.Confidence);
                }
                else
                {
                    Console.WriteLine(Resources.DetectorFail);
                }
            }
        }
    }
}
