using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataContainer.Value
{
    /// <summary>
    /// Хранение значения целочисленного типа данных
    /// </summary>
    [System.Serializable]
    public class IntegerValue : ScalarValue
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public IntegerValue(VHDL.type.IntegerType type, int value)
            : base(type)
        {
            this._value = value;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public IntegerValue(VHDL.type.IntegerType type)
            : base(type)
        {
            this._value = 0;
        }

        /// <summary>
        /// Хранимое значение
        /// </summary>
        private int _value;
        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }

        ~IntegerValue()
        { }
    }
}
