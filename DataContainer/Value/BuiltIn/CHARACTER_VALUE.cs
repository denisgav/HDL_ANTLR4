using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VHDL.literal;

namespace DataContainer.Value
{
    /// <summary>
    /// Хранение значения перечислимого типа данных CHARACTER
    /// </summary>
    [System.Serializable]
    public class CHARACTER_VALUE : EnumerationValue
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public CHARACTER_VALUE(VHDL.type.EnumerationType.CharacterEnumerationLiteral enumerationLiteral)
            : base(VHDL.builtin.Standard.CHARACTER, enumerationLiteral)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public CHARACTER_VALUE()
            : base(VHDL.builtin.Standard.CHARACTER)
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public CHARACTER_VALUE(char value)
            : base(VHDL.builtin.Standard.CHARACTER)
        {
            base._value = new VHDL.type.EnumerationType.CharacterEnumerationLiteral(value, VHDL.builtin.Standard.CHARACTER);
        }

        ~CHARACTER_VALUE()
        { }
    }
}
