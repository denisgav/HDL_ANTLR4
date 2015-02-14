using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VHDL;
using VHDL.type;
using VHDL.builtin;
using DataContainer.ValueDump;

namespace DataContainer.Value
{
    /// <summary>
    /// Хранение значения перечислимого типа данных BIT_VECTOR
    /// </summary>
    [System.Serializable]
    public class BIT_VECTOR_VALUE : ArrayValue
    {
        public BIT_VECTOR_VALUE(IList<AbstractValue> _value, ResolvedDiscreteRange range)
            : base(new ConstrainedArray("std_logic_vector", Standard.BIT, range.CreateRange()), _value, range)
        { }

        public BIT_VECTOR_VALUE(IList<AbstractValue> _value)
            : base(new ConstrainedArray("std_logic_vector", Standard.BIT, new Range(1, VHDL.Range.RangeDirection.TO, _value.Count - 1)), _value)
        { }

        /// <summary>
        /// Создание объекта BIT_VECTOR_VALUE из числа
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BIT_VECTOR_VALUE CreateBIT_VECTOR_VALUE(Int64 value, int size)
        {
            List<AbstractValue> elements = new List<AbstractValue>();

            bool[] booleanData = DataConvertorUtils.ToBitArray(value, size);

            foreach (bool b in booleanData)
                elements.Add(new BIT_VALUE(b ? VHDL.builtin.Standard.BIT_1 : VHDL.builtin.Standard.BIT_0));

            return new BIT_VECTOR_VALUE(elements);
        }

        /// <summary>
        /// Создание объекта BIT_VECTOR_VALUE из массива двоичных значений
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BIT_VECTOR_VALUE CreateBIT_VECTOR_VALUE(bool[] value)
        {
            List<AbstractValue> elements = new List<AbstractValue>();


            foreach (bool b in value)
                elements.Add(new BIT_VALUE(b ? VHDL.builtin.Standard.BIT_1 : VHDL.builtin.Standard.BIT_0));

            return new BIT_VECTOR_VALUE(elements);
        }

        ~BIT_VECTOR_VALUE()
        { }
    }
}
