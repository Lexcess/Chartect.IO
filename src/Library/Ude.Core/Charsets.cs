using System;

namespace Ude
{
    public static class Charsets
    {
        public const string ASCII        = "ASCII";
        
        public const string UTF8         = "UTF-8";
        
        public const string UTF16_LE     = "UTF-16LE";
        
        public const string UTF16_BE     = "UTF-16BE";
        
        public const string UTF32_BE     = "UTF-32BE";
        
        public const string UTF32_LE     = "UTF-32LE";

        /// <summary> Unusual BOM (3412 order) </summary>
        public const string UCS4_3412    = "X-ISO-10646-UCS-4-3412";
        
        /// <summary> Unusual BOM (2413 order) </summary>
        public const string UCS4_2413    = "X-ISO-10646-UCS-4-2413";
       
        /// <summary> Cyrillic (based on bulgarian and russian data) </summary>
        public const string WIN1251      = "windows-1251";
        
        /// <summary> Latin-1, almost identical to ISO-8859-1 </summary>
        public const string WIN1252      = "windows-1252";
        
        /// <summary> Greek </summary>
        public const string WIN1253      = "windows-1253";
        
        /// <summary> Logical hebrew (includes ISO-8859-8-I and most of x-mac-hebrew) </summary>
        public const string WIN1255      = "windows-1255";
        
        /// <summary> Traditional chinese </summary>
        public const string BIG5         = "Big-5";

        public const string EUCKR        = "EUC-KR";

        public const string EUCJP        = "EUC-JP";
        
        public const string EUCTW        = "EUC-TW";

        /// <summary> Note: gb2312 is a subset of gb18030 </summary>
        public const string GB18030      = "gb18030";

        public const string ISO2022_JP   = "ISO-2022-JP";
        
        public const string ISO2022_CN   = "ISO-2022-CN";
        
        public const string ISO2022_KR   = "ISO-2022-KR";
        
        /// <summary> Simplified chinese </summary>
        public const string HZ_GB_2312   = "HZ-GB-2312";

        public const string SHIFT_JIS    = "Shift-JIS";

        public const string MAC_CYRILLIC = "x-mac-cyrillic";
        
        public const string KOI8R        = "KOI8-R";
        
        public const string IBM855       = "IBM855";
        
        public const string IBM866       = "IBM866";

        /// <summary>
        /// East-Europe. Disabled because too similar to windows-1252 
        /// (latin-1). Should use tri-grams models to discriminate between
        /// these two charsets.
        /// </summary>
        public const string ISO8859_2    = "ISO-8859-2";

        /// <summary> Cyrillic </summary>
        public const string ISO8859_5    = "ISO-8859-5";

        /// <summary> Greek </summary>
        public const string ISO_8859_7   = "ISO-8859-7";

        /// <summary> Visual Hebrew </summary>
        public const string ISO8859_8    = "ISO-8859-8";

        /// <summary> Thai. This recognizer is not enabled yet. </summary>
        public const string TIS620       = "TIS620";
        
    }
}
