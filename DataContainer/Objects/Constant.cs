using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataContainer.Value;

namespace DataContainer.Objects
{
    [System.Serializable]
    public class Constant : IValueProvider
    {
        private readonly VHDL.Object.Constant parsedVariable;

        public Constant(VHDL.Object.Constant parsedVariable)
        {
            this.parsedVariable = parsedVariable;
            currentValue = ExpressionEvaluator.DefaultEvaluator.Evaluate(parsedVariable.DefaultValue);
            if(currentValue != null)
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

        public VHDL.ResolvedDiscreteRange[] Range
        {
            get { return type.ResolvedRange; }
        }
        #endregion
    }
}
