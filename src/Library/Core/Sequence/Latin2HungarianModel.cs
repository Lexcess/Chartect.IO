﻿namespace Chartect.IO.Core
{
    using System;

    internal class Latin2HungarianModel : HungarianModel
    {
        private static readonly byte[] OrderMap =
        {
             255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 254, 255, 255, 254, 255, 255, // 00
             255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, // 10
            +253, 253, 253, 253, 253, 253, 253, 253, 253, 253, 253, 253, 253, 253, 253, 253, // 20
             252, 252, 252, 252, 252, 252, 252, 252, 252, 252, 253, 253, 253, 253, 253, 253, // 30
             253,  28,  40,  54,  45,  32,  50,  49,  38,  39,  53,  36,  41,  34,  35,  47,
              46,  71,  43,  33,  37,  57,  48,  64,  68,  55,  52, 253, 253, 253, 253, 253,
             253,   2,  18,  26,  17,   1,  27,  12,  20,   9,  22,   7,   6,  13,   4,   8,
              23,  67,  10,   5,   3,  21,  19,  65,  62,  16,  11, 253, 253, 253, 253, 253,
             159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174,
             175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189, 190,
             191, 192, 193, 194, 195, 196, 197,  75, 198, 199, 200, 201, 202, 203, 204, 205,
              79, 206, 207, 208, 209, 210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220,
             221,  51,  81, 222,  78, 223, 224, 225, 226,  44, 227, 228, 229,  61, 230, 231,
             232, 233, 234,  58, 235,  66,  59, 236, 237, 238,  60,  69,  63, 239, 240, 241,
              82,  14,  74, 242,  70,  80, 243,  72, 244,  15,  83,  77,  84,  30,  76,  85,
             245, 246, 247,  25,  73,  42,  24, 248, 249, 250,  31,  56,  29, 251, 252, 253,
        };

        public Latin2HungarianModel()
            : base(OrderMap, Charsets.Iso88592)
        {
        }
    }
}
