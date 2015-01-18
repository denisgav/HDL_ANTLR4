//
//  Copyright (C) 2010-2014  Denis Gavrish
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VHDLRuntime.MySortedDictionary
{
    /// <summary>
    /// Итератор, используемый для перебора данных в дампе сигнала
    /// </summary>
    public interface IValueIterator<T>
    {
        /// <summary>
        /// Текущее значение
        /// </summary>
        TimeStampInfo<T> CurrentValue
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
