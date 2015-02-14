using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataContainer.Value;

namespace DataContainer.Objects
{
    [System.Serializable]
    public class Variable : IValueProvider
    {
        private readonly VHDL.Object.Variable parsedVariable;

        public Variable(VHDL.Object.Variable parsedVariable)
        {
            this.parsedVariable = parsedVariable;
            type = currentValue.Type;
        }

        #region IValueProvider Members

        public VHDL.Object.DefaultVhdlObject DefaultVhdlObject
        {
            get { return parsedVariable; }
        }

        public string Name
        {
            get { return parsedVariable.Identifier; }
        }

        private AbstractValue currentValue;
        public AbstractValue CurrentValue
        {
            get { return currentValue; }
            set { currentValue = value; }
        }

        private ModellingType type;
        public ModellingType Type
        {
            get
            {
                return type;
            }
        }

        #endregion


        public VHDL.ResolvedDiscreteRange[] Range
        {
            get { return type.ResolvedRange; }
        }

        ~Variable()
        {
        }
    }
}
