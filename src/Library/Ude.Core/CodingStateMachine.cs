namespace Ude.Core
{
    using System;

    /// <summary>
    /// Parallel state machine for the Coding Scheme Method
    /// </summary>
    public class CodingStateMachine
    {
        private int currentState;
        private StateMachineModel model;
        private int currentCharLen;
        private int currentBytePos;

        public CodingStateMachine(StateMachineModel model)
        {
            this.currentState = StateMachineModel.Start;
            this.model = model;
        }

        public int CurrentCharLen
        {
            get { return this.currentCharLen; }
        }

        public string ModelName
        {
            get { return this.model.Name; }
        }

        public int NextState(byte b)
        {
            // for each byte we get its class, if it is first byte,
            // we also get byte length
            int byteCls = this.model.GetClass(b);
            if (this.currentState == StateMachineModel.Start)
            {
                this.currentBytePos = 0;
                this.currentCharLen = this.model.CharLenTable[byteCls];
            }

            // from byte's class and stateTable, we get its next state
            this.currentState = this.model.StateTable.Unpack((this.currentState * this.model.ClassFactor) + byteCls);
            this.currentBytePos++;
            return this.currentState;
        }

        public void Reset()
        {
            this.currentState = StateMachineModel.Start;
        }
    }
}
