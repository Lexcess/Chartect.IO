using System;

namespace Ude.Core
{
    /// <summary>
    /// State machine model
    /// </summary>
    public abstract class SMModel
    {
        public const int START = 0;
        public const int ERROR = 1;
        public const int ITSME = 2;

        public BitPackage classTable;
        public BitPackage stateTable;
        public int[] charLenTable;
        
        private string name;
        
        public string Name {
            get { return name;  }
        }

        private int classFactor;

        public int ClassFactor {
            get { return classFactor;  }
        }

        public SMModel(BitPackage classTable, int classFactor,
            BitPackage stateTable, int[] charLenTable, String name)
        {
            this.classTable = classTable;
            this.classFactor = classFactor;
            this.stateTable = stateTable;
            this.charLenTable = charLenTable;
            this.name = name;
        }
        
        public int GetClass(byte b)
        {        
            return classTable.Unpack((int)b);            
        }
    }    
}
