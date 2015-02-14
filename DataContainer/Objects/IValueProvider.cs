using VHDL.Object;
using VHDL.type;
using DataContainer.Value;
using VHDL;

namespace DataContainer.Objects
{
    public interface IValueProvider
    {
        /// <summary>
        /// Распарсеное значение
        /// </summary>
        DefaultVhdlObject DefaultVhdlObject { get; }

        /// <summary>
        /// Предоставление текущего значения
        /// </summary>
        AbstractValue CurrentValue { get; set; }

        /// <summary>
        /// Базовый тип данных
        /// </summary>
        ModellingType Type { get; }

        /// <summary>
        /// Имя объекта
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Размерность
        /// </summary>
        ResolvedDiscreteRange[] Range { get; }
    }
}
