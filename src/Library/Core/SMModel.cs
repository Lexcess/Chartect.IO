namespace Chartect.IO.Core
{
    using System;

    /// <summary>
    /// State machine model
    /// </summary>
    public abstract class StateMachineModel
    {
        public const int Start = 0;
        public const int Error = 1;
        public const int ItsMe = 2;

        private BitPackage classTable;
        private BitPackage stateTable;
        private int[] charLenTable;

        private string name;

        private int classFactor;

        public StateMachineModel(BitPackage classTable, int classFactor, BitPackage stateTable, int[] charLenTable, string name)
        {
            this.ClassTable = classTable;
            this.classFactor = classFactor;
            this.StateTable = stateTable;
            this.CharLenTable = charLenTable;
            this.name = name;
        }

        public string Name
        {
            get { return this.name;  }
        }

        public int ClassFactor
        {
            get { return this.classFactor;  }
        }

        public BitPackage ClassTable
        {
            get
            {
                return this.classTable;
            }

            set
            {
                this.classTable = value;
            }
        }

        public BitPackage StateTable
        {
            get
            {
                return this.stateTable;
            }

            set
            {
                this.stateTable = value;
            }
        }

        public int[] CharLenTable
        {
            get
            {
                return this.charLenTable;
            }

            set
            {
                this.charLenTable = value;
            }
        }

        public int GetClass(byte b)
        {
            return this.ClassTable.Unpack((int)b);
        }
    }
}
