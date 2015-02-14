using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataContainer.Value
{
    /// <summary>
    /// Хранение значения вещественного типа данных
    /// </summary>
    [System.Serializable]
    public class RealValue : ScalarValue
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public RealValue(VHDL.type.RealType type, double value)
            : base(type)
        {
            this._value = value;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public RealValue(VHDL.type.RealType type)
            : base(type)
        {
            this._value = 0;
        }

        /// <summary>
        /// Хранимое значение
        /// </summary>
        private double _value;
        public double Value
        {
            get { return _value; }
            set { _value = value; }
        }

        ~RealValue()
        { }
    }
}
