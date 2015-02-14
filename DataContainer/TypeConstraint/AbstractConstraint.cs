using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataContainer.Value;

namespace DataContainer.TypeConstraint
{
    /// <summary>
    /// Класс, описывающий огданичения, накладаваемые на определенные значения.
    /// </summary>
    [System.Serializable]
    public abstract class AbstractConstraint
    {
        /// <summary>
        /// Выполнить проверку
        /// </summary>
        /// <param name="value"></param>
        public abstract bool IsValid(AbstractValue value);
    }
}
