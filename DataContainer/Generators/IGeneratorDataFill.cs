using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataContainer.Generator
{
    /// <summary>
    /// Интерфейс, который используется для заполнения данных, полученых от генератора
    /// </summary>
    interface IGeneratorDataFill <TValue>
    {
        /// <summary>
        /// Функция, используемая для получения результатов работы генератора на заданном промежутке времени
        /// </summary>
        /// <returns></returns>
        SortedList<UInt64, TValue> InsertValues(UInt64 StartTime, UInt64 EndTime);
    }
}
