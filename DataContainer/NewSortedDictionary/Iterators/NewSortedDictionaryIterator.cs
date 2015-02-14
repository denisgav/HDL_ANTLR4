using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DataContainer.Value;
using DataContainer.ValueDump;

namespace DataContainer.MySortedDictionary
{
    [System.Serializable]
    public class NewSortedDictionaryIterator<T> : IValueIterator
    {
        /// <summary>
        /// Обрабатываемые данные
        /// </summary>
        private NewSortedDictionary<AbstractTimeStampInfo<T>> data;
        public NewSortedDictionary<AbstractTimeStampInfo<T>> Data
        {
            get
            {
                return data;
            }
        }
        /// <summary>
        /// Используется для преобразования типов данных
        /// </summary>
        private AbstractValueConvertor<T> valueCovertor;
        public AbstractValueConvertor<T> ValueConvertor
        {
            get { return valueCovertor; }
            set { valueCovertor = value; }
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

        public NewSortedDictionaryIterator(NewSortedDictionary<AbstractTimeStampInfo<T>> data, DataRepresentation dataRepresentation, AbstractValueConvertor<T> valueCovertor)
        {
            this.valueCovertor = valueCovertor;
            this.data = data;
            this.dataRepresentation = dataRepresentation;
            Reset();
        }

        public NewSortedDictionaryIterator(NewSortedDictionary<AbstractTimeStampInfo<T>> data, AbstractValueConvertor<T> valueCovertor)
        {
            this.valueCovertor = valueCovertor;
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
        private AbstractTimeStampInfo<T> value;
        public TimeStampInfo CurrentValue
        {
            get { return valueCovertor.GetTimeStampInfo(value); }
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
