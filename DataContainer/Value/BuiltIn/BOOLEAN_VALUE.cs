using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VHDL.literal;
using VHDL.builtin;

namespace DataContainer.Value
{
    /// <summary>
    /// Хранение значения перечислимого типа данных BOOLEAN
    /// </summary>
    [System.Serializable]
    public class BOOLEAN_VALUE: EnumerationValue
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public BOOLEAN_VALUE(EnumerationLiteral enumerationLiteral)
            : base(Standard.BOOLEAN, enumerationLiteral)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public BOOLEAN_VALUE()
            : base(Standard.BOOLEAN)
        {
        }

        ~BOOLEAN_VALUE()
        { }
    }
}
