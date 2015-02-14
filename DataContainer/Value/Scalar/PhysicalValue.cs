using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VHDL.literal;

namespace DataContainer.Value
{
    /// <summary>
    /// Хранение значения физического типа данных
    /// </summary>
    [System.Serializable]
    public class PhysicalValue : ScalarValue
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public PhysicalValue(VHDL.type.PhysicalType type, PhysicalLiteral physicalValue)
            : base(type)
        {
            this._value = physicalValue;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public PhysicalValue(VHDL.type.PhysicalType type, double doubleValue)
            : base(type)
        {
            this.doubleValue = doubleValue;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public PhysicalValue(VHDL.type.PhysicalType type)
            : base(type)
        {
            this.doubleValue = 0;
        }

        /// <summary>
        /// Хранимое значение
        /// </summary>
        private PhysicalLiteral _value;
        public PhysicalLiteral Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        /// <summary>
        /// Представление данных в виде числа с плавающей запятой
        /// </summary>
        private double doubleValue;
        public double DoubleValue
        {
            get { return doubleValue; }
            set { doubleValue = value; }
        }

        ~PhysicalValue()
        { }
    }
}