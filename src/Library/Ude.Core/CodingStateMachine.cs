using System;

namespace Ude.Core
{
    /// <summary>
    /// Parallel state machine for the Coding Scheme Method
    /// </summary>
    public class CodingStateMachine
    {
        private int currentState;
        private SMModel model;
        private int currentCharLen;
        private int currentBytePos;
        
        public CodingStateMachine(SMModel model) 
        {
            this.currentState = SMModel.START;
            this.model = model;
        }

        public int NextState(byte b)
        {
            // for each byte we get its class, if it is first byte, 
            // we also get byte length
            int byteCls = model.GetClass(b);
            if (currentState == SMModel.START)
            { 
                currentBytePos = 0;
                currentCharLen = model.charLenTable[byteCls];
            }
            
            // from byte's class and stateTable, we get its next state            
            currentState = model.stateTable.Unpack(currentState * model.ClassFactor + byteCls);
            currentBytePos++;
            return currentState;
        }
  
        public void Reset() 
        { 
            currentState = SMModel.START; 
        }

        public int CurrentCharLen 
        { 
            get { return currentCharLen; } 
        }

        public string ModelName 
        { 
            get { return model.Name; } 
        }
    }
}
