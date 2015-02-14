using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataContainer.TypeConstraint;
using DataContainer.Objects;

namespace DataContainer.Value
{
    /// <summary>
    /// Промежуточное значение при вычислениях
    /// </summary>
    [System.Serializable]
    public abstract class AbstractValue : IDisposable
    {
        /// <summary>
        /// Тип значения
        /// </summary>
        private ModellingType type;
        public ModellingType Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="type"></param>
        public AbstractValue(ModellingType type)
        {
            this.type = type;
        }

        /// <summary>
        /// Получение представления данных по-умолчанию
        /// </summary>
        /// <returns></returns>
        public abstract DataRepresentation GetDefaultDataRepresentation();

        ~AbstractValue()
        {
            Dispose();
        }

        public virtual void Dispose()
        {
            type = null;
        }
    }
}
