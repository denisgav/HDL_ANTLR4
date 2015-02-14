using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VHDL.type;
using VHDL;
using VHDL.builtin;
using DataContainer.ValueDump;

namespace DataContainer.Value
{
    /// <summary>
    /// Хранение значения перечислимого типа данных STD_ULOGIC_VECTOR
    /// </summary>
    [System.Serializable]
    public class STD_ULOGIC_VECTOR_VALUE : ArrayValue
    {
        public STD_ULOGIC_VECTOR_VALUE(IList<AbstractValue> _value, ResolvedDiscreteRange range)
            : base(VHDL.builtin.StdLogic1164.STD_ULOGIC_VECTOR, _value, range)
        { }

        public STD_ULOGIC_VECTOR_VALUE(IList<AbstractValue> _value)
            : base(VHDL.builtin.StdLogic1164.STD_ULOGIC_VECTOR, _value)
        { }

        ~STD_ULOGIC_VECTOR_VALUE()
        { }

        /// <summary>
        /// Создание объекта STD_ULOGIC_VECTOR_VALUE из числа
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static STD_ULOGIC_VECTOR_VALUE CreateSTD_ULOGIC_VECTOR_VALUE(Int64 value, int size)
        {
            List<AbstractValue> elements = new List<AbstractValue>();

            bool[] booleanData = DataConvertorUtils.ToBitArray(value, size);

            foreach (bool b in booleanData)
                elements.Add(new STD_ULOGIC_VALUE(b ? VHDL.builtin.StdLogic1164.STD_ULOGIC_1 : VHDL.builtin.StdLogic1164.STD_ULOGIC_0));

            return new STD_ULOGIC_VECTOR_VALUE(elements);
        }

        /// <summary>
        /// Создание объекта STD_ULOGIC_VECTOR_VALUE из массива двоичных значений
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static STD_ULOGIC_VECTOR_VALUE CreateSTD_ULOGIC_VECTOR_VALUE(bool[] value)
        {
            List<AbstractValue> elements = new List<AbstractValue>();


            foreach (bool b in value)
                elements.Add(new STD_ULOGIC_VALUE(b ? VHDL.builtin.StdLogic1164.STD_ULOGIC_1 : VHDL.builtin.StdLogic1164.STD_ULOGIC_0));

            return new STD_ULOGIC_VECTOR_VALUE(elements);
        }
    }
}
