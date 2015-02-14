using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VHDL.literal;
using VHDL.builtin;

namespace DataContainer.Value
{
    /// <summary>
    /// Хранение значения перечислимого типа данных BIT
    /// </summary>
    [System.Serializable]
    public class BIT_VALUE: EnumerationValue
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public BIT_VALUE(EnumerationLiteral enumerationLiteral)
            : base(Standard.BIT, enumerationLiteral)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public BIT_VALUE()
            : base(Standard.BIT)
        {
        }

        public static BIT_VALUE Create_BIT_VALUE(bool value)
        {
            if (value == true)
                return new BIT_VALUE(Standard.BIT_1);
            else
                return new BIT_VALUE(Standard.BIT_0);
        }

        ~BIT_VALUE()
        { }
    }
}
