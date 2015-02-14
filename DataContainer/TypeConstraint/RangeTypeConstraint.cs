using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VHDL.type;
using VHDL;

namespace DataContainer.TypeConstraint
{
    /// <summary>
    /// Ограничения, накладываемые на тип определенными числовыми границами.
    /// </summary>
    [System.Serializable]
    public class RangeTypeConstraint : AbstractConstraint
    {
        /// <summary>
        /// Вычисленые границы
        /// </summary>
        private ResolvedDiscreteRange resolvedRange;
        public ResolvedDiscreteRange ResolvedRange
        {
            get { return resolvedRange; }
            set { resolvedRange = value; }
        }
        

        public override bool IsValid(Value.AbstractValue value)
        {
            return true;
        }

        public RangeTypeConstraint(RangeSubtypeIndication type)
        {
        }
    }
}
