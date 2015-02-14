using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VHDL.builtin;
using VHDL.literal;

namespace DataContainer.Value
{
    /// <summary>
    /// Хранение значения Физического типа данных TIME
    /// </summary>
    [System.Serializable]
    public class TIME_VALUE :PhysicalValue
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public TIME_VALUE()
            : base(Standard.TIME)
        {
        }

        public TIME_VALUE(PhysicalLiteral literal)
            : base(Standard.TIME)
        {
            //if (literal.GetPhysicalType() == VHDL.builtin.Standard.TIME)
            //{
                UInt64 multiplyer = 0;
                switch (literal.Unit)
                {
                    case "s": multiplyer = (ulong)1e15; break;
                    case "ms": multiplyer = (ulong)1e12; break;
                    case "us": multiplyer = (ulong)1e9; break;
                    case "ns": multiplyer = (ulong)1e6; break;
                    case "ps": multiplyer = (ulong)1e3; break;
                    case "fs": multiplyer = (ulong)1; break;
                }
                UInt64 value = UInt64.Parse(literal.Value.ToString());
                DoubleValue = value * multiplyer;
            //}
            //else
            //{
            //    throw new NotImplementedException();
            //}
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public TIME_VALUE(double doubleValue)
            : base(Standard.TIME, doubleValue)
        {
        }

        ~TIME_VALUE()
        { }

        public static BOOLEAN_VALUE EQUALS(TIME_VALUE val1, TIME_VALUE val2)
        {
            return val1.DoubleValue.Equals(val2.DoubleValue) ? new BOOLEAN_VALUE(VHDL.builtin.Standard.BOOLEAN_TRUE) : new BOOLEAN_VALUE(VHDL.builtin.Standard.BOOLEAN_FALSE);
        }

        public static AbstractValue EQUALS(TIME_VALUE val1, PhysicalValue physicalValue)
        {
            TIME_VALUE val2 = new TIME_VALUE(physicalValue.DoubleValue);
            return val1.DoubleValue.Equals(val2.DoubleValue) ? new BOOLEAN_VALUE(VHDL.builtin.Standard.BOOLEAN_TRUE) : new BOOLEAN_VALUE(VHDL.builtin.Standard.BOOLEAN_FALSE);
        }

        public static BOOLEAN_VALUE LESS_THAN(TIME_VALUE val1, TIME_VALUE val2)
        {
            return val1.DoubleValue < val2.DoubleValue ? new BOOLEAN_VALUE(VHDL.builtin.Standard.BOOLEAN_TRUE) : new BOOLEAN_VALUE(VHDL.builtin.Standard.BOOLEAN_FALSE);
        }

        public static BOOLEAN_VALUE LESS_EQUALS_THAN(TIME_VALUE val1, TIME_VALUE val2)
        {
            return val1.DoubleValue <= val2.DoubleValue ? new BOOLEAN_VALUE(VHDL.builtin.Standard.BOOLEAN_TRUE) : new BOOLEAN_VALUE(VHDL.builtin.Standard.BOOLEAN_FALSE);
        }

        public static BOOLEAN_VALUE GREATE_THAN(TIME_VALUE val1, TIME_VALUE val2)
        {
            return val1.DoubleValue > val2.DoubleValue ? new BOOLEAN_VALUE(VHDL.builtin.Standard.BOOLEAN_TRUE) : new BOOLEAN_VALUE(VHDL.builtin.Standard.BOOLEAN_FALSE);
        }
    }
}
