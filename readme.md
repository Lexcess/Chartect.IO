## Description

Chartect.IO can recognize the following charsets:

* UTF-8
* UTF-16 (BE and LE)
* UTF-32 (BE and LE)
* windows-1252 (mostly equivalent to iso8859-1)
* windows-1251 and ISO-8859-5 (cyrillic)
* windows-1253 and ISO-8859-7 (greek)
* windows-1255 (logical hebrew. Includes ISO-8859-8-I and most of x-mac-hebrew)
* ISO-8859-8 (visual hebrew)
* Big-5
* gb18030 (superset of gb2312)
* HZ-GB-2312
* Shift-JIS
* EUC-KR, EUC-JP, EUC-TW
* ISO-2022-JP, ISO-2022-KR, ISO-2022-CN
* KOI8-R
* x-mac-cyrillic
* IBM855 and IBM866
* X-ISO-10646-UCS-4-3412 and X-ISO-10646-UCS-4-2413 (unusual BOM)
* ASCII

## Build Status

![Build Status](https://lexm.visualstudio.com/_apis/public/build/definitions/e6a58d77-f73b-4b58-b2d3-e1a08fc5b23d/4/badge)

## Platform
Portable .Net Framework 4.0+,
Windows Phone 8+,
Windows 8+,
Universal Windows Apps,
.Net Core (dotnet, dnx)

## Usage

Import the library:

using Chartect.IO;

you can feed a StreamDetector to the detector:  
```c#
using System.IO;
using Chartect.IO;

public class program
{
    public static void Main(String[] args)
    {
        var filename = args[0];
        var detector = new StreamDetector();
        using (FileStream stream = File.OpenRead(filename)) 
        {
            detector.Read(stream);
            detector.DataEnd();
            if (detector.Charset != null) 
            {
                Console.WriteLine("Charset: {0}, confidence: {1}", detector.Charset, detector.Confidence);
            }  
            else  
            {  
                Console.WriteLine("Detection failed.");  
            } 
        }
    }
}
```

or use StringDetector. StringDetector assumes that there is only one string (so you don't have to call DataEnd):
```c#         
    var detector = new StringDetector();
    var input = "ðÏÓÌÅ ÏËÏÎÞÁÔÅÌØÎÏÇÏ ÒÁÚÏÒÅÎÉÑ ÏÔÃÁ ÓÅÍÅÊÓÔ×Á";

    detector.Read(input);

    if (detector.Charset != null) 
    {
        Console.WriteLine("Charset: {0}, confidence: {1}", detector.Charset, detector.Confidence);
    }  
    else  
    {  
        Console.WriteLine("Detection failed.");  
    }  
```
You can also use ArrayDetector to take in an array of bytes.

## History and other ports


Chartect.IO is a fork of the UDE C# port of Mozilla Universal Charset Detector by Rudi Pettazzi from https://code.google.com/p/ude/.

This work was based on the original source code from Mozilla available at: 

http://lxr.mozilla.org/mozilla/source/intl/chardet/src/  

The article "A composite approach to language/encoding detection" describes the algorithms of Universal Charset Detector and is available at: 

http://www-archive.mozilla.org/projects/intl/chardet.html

Some data-structures used into this port have been adapted from the Java port "juniversalchardet", available at:
     
http://code.google.com/p/juniversalchardet/

Also there is "chardet" (in Python) available at: 
       
http://chardet.feedparser.org/


## License

    This library is subject to the Mozilla Public License Version 1.1 (the "License"). An initial check of this work is available under the LGPL but subsequent versions use MPL as a sole alternative as allowed under the original terms.
