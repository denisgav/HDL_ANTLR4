using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VHDL.type;
using VHDL;

namespace DataContainer.TypeConstraint
{
    /// <summary>
    /// Ограничения по индексам, накладываемые  в основном на массивы.
    /// </summary>
    [System.Serializable]
    public class IndexTypeConstraint : AbstractConstraint
    {
        /// <summary>
        /// Вычисленые границы
        /// </summary>
        private List<ResolvedDiscreteRange> resolvedRanges;
        public List<ResolvedDiscreteRange> ResolvedRanges
        {
            get { return resolvedRanges; }
            set { resolvedRanges = value; }
        }
        
        public override bool IsValid(Value.AbstractValue value)
        {
            return true;
        }

        public IndexTypeConstraint(IndexSubtypeIndication type)
        {
        }
    }
}
