﻿namespace Chartect.IO.Core
{
    internal class Latin7GreekModel : GreekModel
    {
        // 255: Control characters that usually does not exist in any text
        // 254: Carriage/Return
        // 253: symbol (punctuation) that does not belong to word
        // 252: 0 - 9
        // Character Mapping Table:
        private static readonly byte[] OrderMap =
        {
             255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 254, 255, 255, 254, 255, 255,  // 00
             255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255,  // 10
            +253, 253, 253, 253, 253, 253, 253, 253, 253, 253, 253, 253, 253, 253, 253, 253,  // 20
             252, 252, 252, 252, 252, 252, 252, 252, 252, 252, 253, 253, 253, 253, 253, 253,  // 30
             253,  82, 100, 104,  94,  98, 101, 116, 102, 111, 187, 117,  92,  88, 113,  85,  // 40
              79, 118, 105,  83,  67, 114, 119,  95,  99, 109, 188, 253, 253, 253, 253, 253,  // 50
             253,  72,  70,  80,  81,  60,  96,  93,  89,  68, 120,  97,  77,  86,  69,  55,  // 60
              78, 115,  65,  66,  58,  76, 106, 103,  87, 107, 112, 253, 253, 253, 253, 253,  // 70
             255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255,  // 80
             255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255,  // 90
            +253, 233,  90, 253, 253, 253, 253, 253, 253, 253, 253, 253, 253,  74, 253, 253,  // a0
             253, 253, 253, 253, 247, 248,  61,  36,  46,  71,  73, 253,  54, 253, 108, 123,  // b0
             110,  31,  51,  43,  41,  34,  91,  40,  52,  47,  44,  53,  38,  49,  59,  39,  // c0
              35,  48, 250,  37,  33,  45,  56,  50,  84,  57, 120, 121,  17,  18,  22,  15,  // d0
             124,   1,  29,  20,  21,   3,  32,  13,  25,   5,  11,  16,  10,   6,  30,   4,  // e0
               9,   8,  14,   7,   2,  12,  28,  23,  42,  24,  64,  75,  19,  26,  27, 253,  // f0
        };

        public Latin7GreekModel()
            : base(OrderMap, Charsets.Iso88597)
        {
        }
    }
}
