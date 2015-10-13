using System;

/// <summary>
/// Escaped charsets state machines
/// </summary>
namespace Ude.Core
{
    public class HZSMModel : SMModel
    {
        private readonly static int[] HZ_cls = {
            BitPackage.Pack4bits(1,0,0,0,0,0,0,0),  // 00 - 07 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 08 - 0f 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 10 - 17 
            BitPackage.Pack4bits(0,0,0,1,0,0,0,0),  // 18 - 1f 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 20 - 27 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 28 - 2f 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 30 - 37 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 38 - 3f 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 40 - 47 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 48 - 4f 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 50 - 57 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 58 - 5f 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 60 - 67 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 68 - 6f 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 70 - 77 
            BitPackage.Pack4bits(0,0,0,4,0,5,2,0),  // 78 - 7f 
            BitPackage.Pack4bits(1,1,1,1,1,1,1,1),  // 80 - 87 
            BitPackage.Pack4bits(1,1,1,1,1,1,1,1),  // 88 - 8f 
            BitPackage.Pack4bits(1,1,1,1,1,1,1,1),  // 90 - 97 
            BitPackage.Pack4bits(1,1,1,1,1,1,1,1),  // 98 - 9f 
            BitPackage.Pack4bits(1,1,1,1,1,1,1,1),  // a0 - a7 
            BitPackage.Pack4bits(1,1,1,1,1,1,1,1),  // a8 - af 
            BitPackage.Pack4bits(1,1,1,1,1,1,1,1),  // b0 - b7 
            BitPackage.Pack4bits(1,1,1,1,1,1,1,1),  // b8 - bf 
            BitPackage.Pack4bits(1,1,1,1,1,1,1,1),  // c0 - c7 
            BitPackage.Pack4bits(1,1,1,1,1,1,1,1),  // c8 - cf 
            BitPackage.Pack4bits(1,1,1,1,1,1,1,1),  // d0 - d7 
            BitPackage.Pack4bits(1,1,1,1,1,1,1,1),  // d8 - df 
            BitPackage.Pack4bits(1,1,1,1,1,1,1,1),  // e0 - e7 
            BitPackage.Pack4bits(1,1,1,1,1,1,1,1),  // e8 - ef 
            BitPackage.Pack4bits(1,1,1,1,1,1,1,1),  // f0 - f7 
            BitPackage.Pack4bits(1,1,1,1,1,1,1,1)   // f8 - ff 
        };

        private readonly static int[] HZ_st = {
            BitPackage.Pack4bits(START, ERROR,     3, START, START, START, ERROR, ERROR),//00-07 
            BitPackage.Pack4bits(ERROR, ERROR, ERROR, ERROR, ITSME, ITSME, ITSME, ITSME),//08-0f 
            BitPackage.Pack4bits(ITSME, ITSME, ERROR, ERROR, START, START,     4, ERROR),//10-17 
            BitPackage.Pack4bits(    5, ERROR,     6, ERROR,     5,     5,     4, ERROR),//18-1f 
            BitPackage.Pack4bits(    4, ERROR,     4,     4,     4, ERROR,     4, ERROR),//20-27 
            BitPackage.Pack4bits(    4, ITSME, START, START, START, START, START, START) //28-2f 
        };

        private readonly static int[] HZCharLenTable = {0, 0, 0, 0, 0, 0};
        
        public HZSMModel() : base(
              new BitPackage(BitPackage.INDEX_SHIFT_4BITS, 
                         BitPackage.SHIFT_MASK_4BITS, 
                         BitPackage.BIT_SHIFT_4BITS,
                         BitPackage.UNIT_MASK_4BITS, HZ_cls),
                         6,
              new BitPackage(BitPackage.INDEX_SHIFT_4BITS, 
                         BitPackage.SHIFT_MASK_4BITS, 
                         BitPackage.BIT_SHIFT_4BITS,
                         BitPackage.UNIT_MASK_4BITS, HZ_st),
              HZCharLenTable, "HZ-GB-2312")
        {

        }
    }
    
    public class ISO2022CNSMModel : SMModel
    {
        private readonly static int[] ISO2022CN_cls = {
            BitPackage.Pack4bits(2,0,0,0,0,0,0,0),  // 00 - 07 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 08 - 0f 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 10 - 17 
            BitPackage.Pack4bits(0,0,0,1,0,0,0,0),  // 18 - 1f 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 20 - 27 
            BitPackage.Pack4bits(0,3,0,0,0,0,0,0),  // 28 - 2f 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 30 - 37 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 38 - 3f 
            BitPackage.Pack4bits(0,0,0,4,0,0,0,0),  // 40 - 47 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 48 - 4f 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 50 - 57 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 58 - 5f 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 60 - 67 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 68 - 6f 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 70 - 77 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 78 - 7f 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // 80 - 87 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // 88 - 8f 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // 90 - 97 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // 98 - 9f 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // a0 - a7 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // a8 - af 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // b0 - b7 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // b8 - bf 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // c0 - c7 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // c8 - cf 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // d0 - d7 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // d8 - df 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // e0 - e7 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // e8 - ef 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // f0 - f7 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2)   // f8 - ff 
        };

        private readonly static int[] ISO2022CN_st = {
                BitPackage.Pack4bits(START,    3,ERROR,START,START,START,START,START),//00-07 
                BitPackage.Pack4bits(START,ERROR,ERROR,ERROR,ERROR,ERROR,ERROR,ERROR),//08-0f 
                BitPackage.Pack4bits(ERROR,ERROR,ITSME,ITSME,ITSME,ITSME,ITSME,ITSME),//10-17 
                BitPackage.Pack4bits(ITSME,ITSME,ITSME,ERROR,ERROR,ERROR,    4,ERROR),//18-1f 
                BitPackage.Pack4bits(ERROR,ERROR,ERROR,ITSME,ERROR,ERROR,ERROR,ERROR),//20-27 
                BitPackage.Pack4bits(     5,   6,ERROR,ERROR,ERROR,ERROR,ERROR,ERROR),//28-2f 
                BitPackage.Pack4bits(ERROR,ERROR,ERROR,ITSME,ERROR,ERROR,ERROR,ERROR),//30-37 
                BitPackage.Pack4bits(ERROR,ERROR,ERROR,ERROR,ERROR,ITSME,ERROR,START) //38-3f 
        };

        private readonly static int[] ISO2022CNCharLenTable = {0, 0, 0, 0, 0, 0, 0, 0, 0};

        public ISO2022CNSMModel() : base(
              new BitPackage(BitPackage.INDEX_SHIFT_4BITS, 
                         BitPackage.SHIFT_MASK_4BITS, 
                         BitPackage.BIT_SHIFT_4BITS,
                         BitPackage.UNIT_MASK_4BITS, ISO2022CN_cls),
                         9,
              new BitPackage(BitPackage.INDEX_SHIFT_4BITS, 
                         BitPackage.SHIFT_MASK_4BITS, 
                         BitPackage.BIT_SHIFT_4BITS,
                         BitPackage.UNIT_MASK_4BITS, ISO2022CN_st),
              ISO2022CNCharLenTable, "ISO-2022-CN")
        {

        }
    }
    
    public class ISO2022JPSMModel : SMModel
    {
        private readonly static int[] ISO2022JP_cls = {
            BitPackage.Pack4bits(2,0,0,0,0,0,0,0),  // 00 - 07 
            BitPackage.Pack4bits(0,0,0,0,0,0,2,2),  // 08 - 0f 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 10 - 17 
            BitPackage.Pack4bits(0,0,0,1,0,0,0,0),  // 18 - 1f 
            BitPackage.Pack4bits(0,0,0,0,7,0,0,0),  // 20 - 27 
            BitPackage.Pack4bits(3,0,0,0,0,0,0,0),  // 28 - 2f 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 30 - 37 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 38 - 3f 
            BitPackage.Pack4bits(6,0,4,0,8,0,0,0),  // 40 - 47 
            BitPackage.Pack4bits(0,9,5,0,0,0,0,0),  // 48 - 4f 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 50 - 57 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 58 - 5f 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 60 - 67 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 68 - 6f 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 70 - 77 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 78 - 7f 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // 80 - 87 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // 88 - 8f 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // 90 - 97 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // 98 - 9f 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // a0 - a7 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // a8 - af 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // b0 - b7 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // b8 - bf 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // c0 - c7 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // c8 - cf 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // d0 - d7 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // d8 - df 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // e0 - e7 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // e8 - ef 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // f0 - f7 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2)   // f8 - ff 
        };

        private readonly static int[] ISO2022JP_st = {
            BitPackage.Pack4bits(START,     3, ERROR,START,START,START,START,START),//00-07 
            BitPackage.Pack4bits(START, START, ERROR,ERROR,ERROR,ERROR,ERROR,ERROR),//08-0f 
            BitPackage.Pack4bits(ERROR, ERROR, ERROR,ERROR,ITSME,ITSME,ITSME,ITSME),//10-17 
            BitPackage.Pack4bits(ITSME, ITSME, ITSME,ITSME,ITSME,ITSME,ERROR,ERROR),//18-1f 
            BitPackage.Pack4bits(ERROR,     5, ERROR,ERROR,ERROR,    4,ERROR,ERROR),//20-27 
            BitPackage.Pack4bits(ERROR, ERROR, ERROR,    6,ITSME,ERROR,ITSME,ERROR),//28-2f 
            BitPackage.Pack4bits(ERROR, ERROR, ERROR,ERROR,ERROR,ERROR,ITSME,ITSME),//30-37 
            BitPackage.Pack4bits(ERROR, ERROR, ERROR,ITSME,ERROR,ERROR,ERROR,ERROR),//38-3f 
            BitPackage.Pack4bits(ERROR, ERROR, ERROR,ERROR,ITSME,ERROR,START,START) //40-47 
        };

        private readonly static int[] ISO2022JPCharLenTable = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

        public ISO2022JPSMModel() : base(
              new BitPackage(BitPackage.INDEX_SHIFT_4BITS, 
                         BitPackage.SHIFT_MASK_4BITS, 
                         BitPackage.BIT_SHIFT_4BITS,
                         BitPackage.UNIT_MASK_4BITS, ISO2022JP_cls),
                         10,
              new BitPackage(BitPackage.INDEX_SHIFT_4BITS, 
                         BitPackage.SHIFT_MASK_4BITS, 
                         BitPackage.BIT_SHIFT_4BITS,
                         BitPackage.UNIT_MASK_4BITS, ISO2022JP_st),
              ISO2022JPCharLenTable, "ISO-2022-JP")
        {

        }
    
    }
    
    public class ISO2022KRSMModel : SMModel
    {   
        private readonly static int[] ISO2022KR_cls = {
            BitPackage.Pack4bits(2,0,0,0,0,0,0,0),  // 00 - 07 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 08 - 0f 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 10 - 17 
            BitPackage.Pack4bits(0,0,0,1,0,0,0,0),  // 18 - 1f 
            BitPackage.Pack4bits(0,0,0,0,3,0,0,0),  // 20 - 27 
            BitPackage.Pack4bits(0,4,0,0,0,0,0,0),  // 28 - 2f 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 30 - 37 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 38 - 3f 
            BitPackage.Pack4bits(0,0,0,5,0,0,0,0),  // 40 - 47 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 48 - 4f 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 50 - 57 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 58 - 5f 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 60 - 67 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 68 - 6f 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 70 - 77 
            BitPackage.Pack4bits(0,0,0,0,0,0,0,0),  // 78 - 7f 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // 80 - 87 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // 88 - 8f 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // 90 - 97 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // 98 - 9f 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // a0 - a7 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // a8 - af 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // b0 - b7 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // b8 - bf 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // c0 - c7 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // c8 - cf 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // d0 - d7 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // d8 - df 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // e0 - e7 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // e8 - ef 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2),  // f0 - f7 
            BitPackage.Pack4bits(2,2,2,2,2,2,2,2)   // f8 - ff 
        };

        private readonly static int[] ISO2022KR_st = {
            BitPackage.Pack4bits(START,    3,ERROR,START,START,START,ERROR,ERROR),//00-07 
            BitPackage.Pack4bits(ERROR,ERROR,ERROR,ERROR,ITSME,ITSME,ITSME,ITSME),//08-0f 
            BitPackage.Pack4bits(ITSME,ITSME,ERROR,ERROR,ERROR,    4,ERROR,ERROR),//10-17 
            BitPackage.Pack4bits(ERROR,ERROR,ERROR,ERROR,    5,ERROR,ERROR,ERROR),//18-1f 
            BitPackage.Pack4bits(ERROR,ERROR,ERROR,ITSME,START,START,START,START) //20-27 
        };

        private readonly static int[] ISO2022KRCharLenTable = {0, 0, 0, 0, 0, 0};

        public ISO2022KRSMModel() : base(
              new BitPackage(BitPackage.INDEX_SHIFT_4BITS, 
                         BitPackage.SHIFT_MASK_4BITS, 
                         BitPackage.BIT_SHIFT_4BITS,
                         BitPackage.UNIT_MASK_4BITS, ISO2022KR_cls),
                         6,
              new BitPackage(BitPackage.INDEX_SHIFT_4BITS, 
                         BitPackage.SHIFT_MASK_4BITS, 
                         BitPackage.BIT_SHIFT_4BITS,
                         BitPackage.UNIT_MASK_4BITS, ISO2022KR_st),
              ISO2022KRCharLenTable, "ISO-2022-KR")
        {

        }
    }
}
