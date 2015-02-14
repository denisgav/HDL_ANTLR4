using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataContainer.Value
{
    /// <summary>
    /// Базовый абстрактный класс для хранения значения составного типа данных
    /// </summary>
    [System.Serializable]
    public abstract class CompositeValue:AbstractValue
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public CompositeValue(ModellingType type)
            : base(type)
        {
        }

        /// <summary>
        /// Получение дочерних єлементов
        /// </summary>
        /// <returns></returns>
        public abstract IList<AbstractValue> GetChilds();

        /// <summary>
        /// Создание объекта составного типа с набора его элементов
        /// </summary>
        /// <param name="type"></param>
        /// <param name="_values"></param>
        /// <returns></returns>
        public static CompositeValue CreateCompositeValue(ModellingType type, IList<AbstractValue> _values)
        {
            if (type.Type is VHDL.type.ArrayType)
            {
                if (type.Type == VHDL.builtin.StdLogic1164.STD_LOGIC_VECTOR)
                    return new STD_LOGIC_VECTOR_VALUE(_values);
                if (type.Type == VHDL.builtin.StdLogic1164.STD_ULOGIC_VECTOR)
                    return new STD_ULOGIC_VECTOR_VALUE(_values);
                if (type.Type == VHDL.builtin.Standard.BIT_VECTOR)
                    return new BIT_VECTOR_VALUE(_values);
                return new ArrayValue(type, _values);
            }
            if (type.Type is VHDL.type.RecordType)
                return new RecordValue(type, _values);
            throw new ArgumentException("Invalid Argument", "type");
        }

        public override DataRepresentation GetDefaultDataRepresentation()
        {
            return new VectorDataRepresentation();
        }

        ~CompositeValue()
        { }
    }
}
