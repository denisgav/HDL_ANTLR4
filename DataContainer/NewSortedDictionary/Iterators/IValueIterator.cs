using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataContainer.Value;

namespace DataContainer.MySortedDictionary
{
    /// <summary>
    /// Итератор, используемый для перебора данных в дампе сигнала
    /// </summary>
    public interface IValueIterator
    {
        /// <summary>
        /// Текущее значение
        /// </summary>
        TimeStampInfo CurrentValue
        { get; }        

        /// <summary>
        /// система счисления
        /// </summary>
        DataRepresentation DataRepresentation
        { get; set; }

        /// <summary>
        /// текущий индекс
        /// </summary>
        int CurrentIndex
        { get; }

        /// <summary>
        /// установка текущего индекса по ключу
        /// </summary>
        /// <param name="Time"></param>
        void SetCurrentIndexByKey(UInt64 Time);

        /// <summary>
        /// сброс итератора
        /// </summary>
        void Reset();

        /// <summary>
        /// Последнее значение ключа
        /// </summary>
        UInt64 LastEvent
        { get; }

        /// <summary>
        /// переход к следующему значению
        /// </summary>
        void MoveNext();

        /// <summary>
        /// переход к следующему значению без вычисления данных
        /// </summary>
        void MoveNextWithoutRecalcValue();

        /// <summary>
        /// Проверка, дошел ли итератор к концу списка
        /// </summary>
        bool IsEndOfIteration
        { get; }

        /// <summary>
        /// Обновление значения последнего события (необходимо для моделирования)
        /// </summary>
        void UpdateLastEvent();
    }
}
