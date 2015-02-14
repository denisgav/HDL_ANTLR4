using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VHDL.type;
using VHDL.builtin;
using VHDL;
using DataContainer.ValueDump;

namespace DataContainer.Value
{
    /// <summary>
    /// Хранение значения перечислимого типа данных STD_LOGIC_VECTOR
    /// </summary>
    [System.Serializable]
    public class STD_LOGIC_VECTOR_VALUE : STD_ULOGIC_VECTOR_VALUE
    {
        public STD_LOGIC_VECTOR_VALUE(IList<AbstractValue> _value, ResolvedDiscreteRange range)
            : base(_value, range)
        { }

        public STD_LOGIC_VECTOR_VALUE(IList<AbstractValue> _value)
            : base( _value)
        { }

        /// <summary>
        /// Создание объекта STD_LOGIC_VECTOR_VALUE из числа
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static STD_LOGIC_VECTOR_VALUE CreateSTD_LOGIC_VECTOR_VALUE(Int64 value, int size)
        {
            List<AbstractValue> elements = new List<AbstractValue>();

            bool[] booleanData = DataConvertorUtils.ToBitArray(value, size);

            foreach (bool b in booleanData)
                elements.Add(new STD_LOGIC_VALUE(b ? VHDL.builtin.StdLogic1164.STD_LOGIC_1 : VHDL.builtin.StdLogic1164.STD_LOGIC_0));

            return new STD_LOGIC_VECTOR_VALUE(elements);
        }

        /// <summary>
        /// Создание объекта STD_LOGIC_VECTOR_VALUE из массива двоичных значений
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static STD_LOGIC_VECTOR_VALUE CreateSTD_LOGIC_VECTOR_VALUE(bool[] value)
        {
            List<AbstractValue> elements = new List<AbstractValue>();


            foreach (bool b in value)
                elements.Add(new STD_LOGIC_VALUE(b ? VHDL.builtin.StdLogic1164.STD_LOGIC_1 : VHDL.builtin.StdLogic1164.STD_LOGIC_0));

            return new STD_LOGIC_VECTOR_VALUE(elements);
        }

        ~STD_LOGIC_VECTOR_VALUE()
        { }
    }
}
