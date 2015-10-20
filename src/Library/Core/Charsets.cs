namespace Chartect
{
    using System;

    public static class Charsets
    {
        public const string Ascii = "ASCII";

        public const string Utf8 = "UTF-8";

        public const string Utf16LE = "UTF-16LE";

        public const string Utf16BE = "UTF-16BE";

        public const string Utf32BE = "UTF-32BE";

        public const string Utf32LE = "UTF-32LE";

        /// <summary> Unusual BOM (3412 order) </summary>
        public const string Ucs43412 = "X-ISO-10646-UCS-4-3412";

        /// <summary> Unusual BOM (2413 order) </summary>
        public const string Ucs42413 = "X-ISO-10646-UCS-4-2413";

        /// <summary> Hungarian </summary>
        public const string Win1250 = "windows-1250";

        /// <summary> Cyrillic (based on bulgarian and russian data) </summary>
        public const string Win1251 = "windows-1251";

        /// <summary> Latin-1, almost identical to ISO-8859-1 </summary>
        public const string Win1252 = "windows-1252";

        /// <summary> Greek </summary>
        public const string Win1253 = "windows-1253";

        /// <summary> Logical hebrew (includes ISO-8859-8-I and most of x-mac-hebrew) </summary>
        public const string Win1255 = "windows-1255";

        /// <summary> Traditional chinese </summary>
        public const string Big5 = "Big-5";

        public const string EucKR = "EUC-KR";

        public const string EucJP = "EUC-JP";

        public const string EucTW = "EUC-TW";

        /// <summary> Note: gb2312 is a subset of gb18030 </summary>
        public const string GB18030 = "gb18030";

        public const string Iso2022JP = "ISO-2022-JP";

        public const string Iso2022CN = "ISO-2022-CN";

        public const string Iso2022KR = "ISO-2022-KR";

        /// <summary> Simplified chinese </summary>
        public const string HZGB2312 = "HZ-GB-2312";

        public const string ShiftJis = "Shift-JIS";

        public const string MacCyrillic = "x-mac-cyrillic";

        public const string Koi8R = "KOI8-R";

        public const string IBM855 = "IBM855";

        public const string Ibm866 = "IBM866";

        /// <summary>
        /// East-Europe. Disabled because too similar to windows-1252
        /// (latin-1). Should use tri-grams models to discriminate between
        /// these two charsets.
        /// </summary>
        public const string Iso88592 = "ISO-8859-2";

        /// <summary> Cyrillic </summary>
        public const string Iso88595 = "ISO-8859-5";

        /// <summary> Greek </summary>
        public const string Iso88597 = "ISO-8859-7";

        /// <summary> Visual Hebrew </summary>
        public const string ISO88598 = "ISO-8859-8";

        /// <summary> Thai. This recognizer is not enabled yet. </summary>
        public const string TIS620 = "TIS620";
    }
}
