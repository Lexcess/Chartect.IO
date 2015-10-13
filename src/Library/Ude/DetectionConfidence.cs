using System;

namespace Ude
{
    /// <summary>
    /// Indicate how confident the detection module about the return result.
    ///     
    ///  NoAnswerYet: the detector have not find out a answer yet based on 
    ///  the data it received.
    /// 
    ///  BestAnswer: the answer the detector returned is the best one within 
    ///     the knowledge of the detector. In other words, the test to all 
    ///     other candidates fail.
    ///     For example, the (Shift_JIS/EUC-JP/ISO-2022-JP) detection
    ///     module may return this with answer "Shift_JIS " if it receive 
    ///     bytes > 0x80 (which make ISO-2022-JP test failed) and byte 
    ///     0x82 (which may EUC-JP test failed)
    ///
    ///  SureAnswer: the detector is 100% sure about the  answer.
    ///  
    ///  Example 1: the Shift_JIS/ISO-2022-JP/EUC-JP detector return
    ///    this w/ ISO-2022-JP when it hit one of the following ESC seq
    ///     ESC ( J
    ///     ESC $ @
    ///     ESC $ B
    /// 
    ///  Example 2: the detector which can detect UCS2 return w/ UCS2
    ///     when the first 2 byte are BOM mark.
    ///  Example 3: the Korean detector return ISO-2022-KR when it
    ///     hit ESC $ ) C
    /// </summary>
    public enum DetectionConfidence
    {
        NoAnswerYet = 0,
        BestAnswer,
        SureAnswer,
        NoAnswerMatch
    }
}
