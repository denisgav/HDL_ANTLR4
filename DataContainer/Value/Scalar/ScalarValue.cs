using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataContainer.Value
{
    /// <summary>
    /// Абстрактный класс для хранения скалярного типа данных
    /// </summary>
    [System.Serializable]
    public abstract class ScalarValue:AbstractValue
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public ScalarValue(VHDL.type.Type type)
            :base(ModellingType.CreateModellingType(type))
        {
        }

        public override DataRepresentation GetDefaultDataRepresentation()
        {
            return new DataRepresentation();
        }

        ~ScalarValue()
        { }
    }
}
