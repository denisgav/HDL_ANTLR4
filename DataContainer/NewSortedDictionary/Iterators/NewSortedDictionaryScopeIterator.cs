using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataContainer.SignalDump;
using DataContainer.Value;

namespace DataContainer.MySortedDictionary
{
    [System.Serializable]
    public class NewSortedDictionaryScopeIterator : IValueIterator
    {
        /// <summary>
        /// Перечень внутренних итераторов
        /// </summary>
        private List<IValueIterator> iterators;
        public List<IValueIterator> Iterators
        {
            get { return iterators; }
        }

        /// <summary>
        /// Обрабатываемый дамп данных
        /// </summary>
        private SignalScopeDump Dump;

        /// <summary>
        /// Моделируемый тип данных
        /// </summary>
        private ModellingType Type;

        public NewSortedDictionaryScopeIterator(SignalScopeDump dump)
        {
            this.Dump = dump;
            this.Type = dump.Type;
            iterators = new List<IValueIterator>();
            foreach (AbstractSignalDump d in dump.Dumps)
            {
                IValueIterator i = d.Iterator;
                i.Reset();
                iterators.Add(i);
            }
        }


        #region IValueIterator interface implementation
        private TimeStampInfo currentValue;
        public TimeStampInfo CurrentValue
        {
            get { return currentValue; }
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

        private int currentIndex;
        public int CurrentIndex
        {
            get
            {
                return currentIndex;
            }
        }

        public void SetCurrentIndexByKey(ulong Time)
        {
            foreach (var i in iterators)
            {
                i.SetCurrentIndexByKey(Time);
                i.MoveNext();
            }

            lastEvent = Time;

            currentValue = CreateTimeStampInfo();
        }

        public void Reset()
        {
            foreach (var i in iterators)
                i.Reset();

            UInt64 Current = default(UInt64);
            bool IsSet = false;

            foreach (var iter in iterators)
            {
                if (
                        (iter.IsEndOfIteration == false) && (((Current> iter.LastEvent) || (IsSet == false)))
                    )
                {
                    IsSet = true;
                    Current = iter.LastEvent;
                }
            }

            //Заполнение данных
            lastEvent = Current;

            currentValue = CreateTimeStampInfo();
        }

        /// <summary>
        /// время последнего изменения значения (относительно текущего)
        /// </summary>
        private UInt64 lastEvent;
        public UInt64 LastEvent
        {
            get { return lastEvent; }
        }

        public void MoveNext()
        {
            foreach (var iter in iterators)
            {
                if (iter.LastEvent == lastEvent)
                    iter.MoveNext();
            }

            currentValue = CreateTimeStampInfo();
            //Проверяем, есть ли ещо данные для выборки
            if (IsEndOfIteration == true)
                return;

            //выбираем первое событие (точнее минимальное время)
            UInt64 Current = default(UInt64);
            bool IsSet = false;

            foreach (var iter in iterators)
            {
                if (
                        (iter.IsEndOfIteration == false) && (((Current > iter.LastEvent) || (IsSet == false)))
                    )
                {
                    IsSet = true;
                    Current = iter.LastEvent;
                }
            }

            //Заполнение данных
            lastEvent = Current;
            currentIndex++;
        }

        public void MoveNextWithoutRecalcValue()
        {
            foreach (var iter in iterators)
            {
                if (iter.LastEvent == lastEvent)
                    iter.MoveNext();
            }
            //Проверяем, есть ли ещо данные для выборки
            if (IsEndOfIteration == true)
                return;

            //выбираем первое событие (точнее минимальное время)
            UInt64 Current = default(UInt64);
            bool IsSet = false;

            foreach (var iter in iterators)
            {
                if (
                        (iter.IsEndOfIteration == false) && (((Current > iter.LastEvent) || (IsSet == false)))
                    )
                {
                    IsSet = true;
                    Current = iter.LastEvent;
                }
            }

            //Заполнение данных
            lastEvent = Current;
            currentIndex++;
        }

        public bool IsEndOfIteration
        {
            get
            {
                bool IsEndOfData = true;
                foreach (var iter in iterators)
                    if (iter.IsEndOfIteration == false)
                    {
                        IsEndOfData = false;
                        break;
                    }
                return IsEndOfData == true;
            }
        }

        public void UpdateLastEvent()
        {
            //throw new NotImplementedException();
        }
        #endregion

        /// <summary>
        /// Формирование информации о текущем моменте времени 
        /// </summary>
        /// <returns></returns>
        private TimeStampInfo CreateTimeStampInfo()
        {
            List<TimeStampInfo> components = new List<TimeStampInfo>();
            foreach(var i in iterators)
                components.Add(i.CurrentValue);
            return TimeStampInfo.CombineTimestamps(Type, components);
        }
    }
}
