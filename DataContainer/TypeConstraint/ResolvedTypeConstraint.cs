using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VHDL.type;

namespace DataContainer.TypeConstraint
{
    /// <summary>
    /// Ограничения, накладываемые на тип при помощи определенной функции
    /// </summary>
    [System.Serializable]
    public class ResolvedTypeConstraint : AbstractConstraint
    {
        /// <summary>
        /// Имя функции, накладывающей ограничения
        /// </summary>
        private string resolutionFunction;
        public string ReslutionFunction
        {
            get { return resolutionFunction; }
            set { resolutionFunction = value; }
        }
        
        /// <summary>
        /// Не забыть дописать :(
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(Value.AbstractValue value)
        {
            return true;
        }

        public ResolvedTypeConstraint(ResolvedSubtypeIndication type)
        {
            this.resolutionFunction = type.ResolutionFunction;
        }
    }
}
