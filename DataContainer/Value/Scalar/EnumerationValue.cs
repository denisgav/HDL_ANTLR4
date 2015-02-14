using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VHDL.literal;
using VHDL.expression;

namespace DataContainer.Value
{
    /// <summary>
    /// Хранение значения перечислимого типа данных
    /// </summary>
    [System.Serializable]
    public class EnumerationValue : ScalarValue
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public EnumerationValue(VHDL.type.EnumerationType type, EnumerationLiteral enumerationLiteral)
            : base(type)
        {
            this.Value = enumerationLiteral;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public EnumerationValue(VHDL.type.EnumerationType type)
            : base(type)
        {
            this.Value = type.Literals[0];
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is EnumerationValue)
            {
                EnumerationValue elseValue = obj as EnumerationValue;
                return elseValue.Value.Equals(Value);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Хранимое значение
        /// </summary>
        protected EnumerationLiteral _value;
        public EnumerationLiteral Value
        {
            get { return _value; }
            set 
            {
                if ((base.Type.Type as VHDL.type.EnumerationType).Literals.Contains(value) == false)
                {
                    throw new Exception("Undefined Literal");
                }
                else
                    _value = value;
            }
        }

        ~EnumerationValue()
        { }
    }
}
