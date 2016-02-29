namespace Chartect.IO.Core
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    internal class EucJPDistributionAnalyser : SjisDistributionAnalyser
    {
        // first  byte range: 0xa0 -- 0xfe
        // second byte range: 0xa1 -- 0xfe
        // no validation needed here. State machine has done that
        public override int GetOrder(byte[] buf, int offset)
        {
            if (buf[offset] >= 0xA0)
            {
                return (94 * (buf[offset] - 0xA1)) + buf[offset + 1] - 0xA1;
            }
            else
            {
                return -1;
            }
        }
    }
}
