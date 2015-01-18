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
using System.IO;

namespace VHDLRuntime.MySortedDictionary
{
    [System.Serializable]
    public class NewSortedDictionaryIterator<T> : IValueIterator<T>
    {
        /// <summary>
        /// Обрабатываемые данные
        /// </summary>
        private NewSortedDictionary<TimeStampInfo<T>> data;
        public NewSortedDictionary<TimeStampInfo<T>> Data
        {
            get
            {
                return data;
            }
        }


        /// <summary>
        /// Индекс текущего елемента в timeChanges, который обрабатывается
        /// </summary>
        private int currentIndex;
        public int CurrentIndex 
        {
            get
            {
                return currentIndex;
            }
        }

        /// <summary>
        /// Установка значения CurrentIndex по времени
        /// (используется для итератора TimeIterator)
        /// </summary>
        /// <param name="time"></param>
        public void SetCurrentIndexByKey(UInt64 Time)
        {
            if (data.Count != 0)
            {
                currentIndex = data.BinarySearchKey(Time);
                if (currentIndex < data.Count)
                    lastEvent = data.Keys[currentIndex];
                else
                    lastEvent = data.Keys[data.Count - 1];
                value = data.GetValue(Time);
            }
        }

        public NewSortedDictionaryIterator(NewSortedDictionary<TimeStampInfo<T>> data, DataRepresentation dataRepresentation)
        {
            this.data = data;
            this.dataRepresentation = dataRepresentation;
            Reset();
        }

        public NewSortedDictionaryIterator(NewSortedDictionary<TimeStampInfo<T>> data)
        {
            this.data = data;
            dataRepresentation = DataRepresentation.DefaultDataRepresentation;
            Reset();
        }

        public void Reset()
        {
            if (data.Count != 0)
            {
                currentIndex = 0;
                lastEvent = data.Keys[0];
                value = data.GetValue(lastEvent);
            }
        }

        /// <summary>
        /// время последнего изменения значения (относительно текущего)
        /// </summary>
        private UInt64 lastEvent;
        public UInt64 LastEvent
        {
            get { return lastEvent; }
        }

        /// <summary>
        /// Текущее значение сигнала
        /// </summary>
        private TimeStampInfo<T> value;
        public TimeStampInfo<T> CurrentValue
        {
            get { return value; }
        }

        public void MoveNext()
        {
            if (currentIndex >= data.Count)
            {
                return;
            }
            else
            {
                value = data.GetValue(lastEvent);
                currentIndex++;
                if (currentIndex < data.Count)
                {
                    lastEvent = data.Keys[currentIndex];
                }
            }
        }

        public void MoveNextWithoutRecalcValue()
        {
            if (currentIndex >= data.Count)
            {
                return;
            }
            else
            {
                currentIndex++;
                if (currentIndex < data.Count)
                {
                    lastEvent = data.Keys[currentIndex];
                }
            }
        }

        /// <summary>
        /// Завершена ли выборка данных из дампа
        /// </summary>
        public bool IsEndOfIteration
        {
            get
            {
                return currentIndex >= data.Count;
            }
        }

        public void UpdateLastEvent()
        {
            if (currentIndex >= data.Count)
            {
                lastEvent = data.Keys[data.Count - 1];
            }
            else
            {
                lastEvent = data.Keys[currentIndex];
            }
        }

        /// <summary>
        /// Представление данных
        /// </summary>
        private DataRepresentation dataRepresentation;
        public DataRepresentation DataRepresentation
        {
            get { return dataRepresentation; }
            set { dataRepresentation = value; }
        }
    }
}
