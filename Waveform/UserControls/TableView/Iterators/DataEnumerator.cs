using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using DataContainer;
using DataContainer.MySortedDictionary;
using DataContainer.Value;
using DataContainer.SignalDump;

namespace Schematix.Waveform.Iterators
{
    abstract class DataEnumerator : IEnumerator<List<string []>>
    {
        protected List<IValueIterator> iterators;

        public DataEnumerator(List<IValueIterator> iterators)
        {
            this.iterators = iterators;
            CurrentValue = new List<string []>();
            PreviousTime = 0;
        }

        /// <summary>
        /// Текущее заначение
        /// </summary>
        protected List<string []> CurrentValue;

        public UInt64 PreviousTime {get; set;}

        /// <summary>
        /// Индекс текущей выборки
        /// </summary>
        protected int CurrentIndex;

        #region IEnumerator<List<string []>> Members

        public List<string []> Current
        {
            get
            {
                return CurrentValue;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            CurrentValue.Clear();
        }

        #endregion

        #region IEnumerator Members

        object IEnumerator.Current
        {
            get
            {
                return CurrentValue;
            }
        }

        public virtual bool IsEndOfIteration()
        {
            //Проверяем, есть ли ещо данные для выборки
            bool IsEndOfData = true;
            foreach (var iter in iterators)
                if (iter.IsEndOfIteration == false)
                {
                    IsEndOfData = false;
                    break;
                }
            return IsEndOfData;
        }

        public virtual bool MoveNext()
        {
            if (IsEndOfIteration() == true)
                return false;

            //выбираем первое событие (точнее минимальное время)
            ulong CurrentTime = ulong.MaxValue;
            foreach (var iter in iterators)
            {
                if ((iter.IsEndOfIteration == false) && (iter.LastEvent < CurrentTime))
                    CurrentTime = iter.LastEvent;
            }

            FormCurrentValue(CurrentTime);

            PreviousTime = CurrentTime;

            foreach (var iter in iterators)
            {
                if (iter.LastEvent == CurrentTime)
                    iter.MoveNext();
            }

            CurrentIndex++;
            
            return true;
        }

        /// <summary>
        /// Сформировать текущее значение, исходя из значений итераторов
        /// </summary>
        protected void FormCurrentValue(ulong CurrentTime)
        {
            SortedDictionary<int, List<AbstractValue>> data = TimeStampInfo.CombineTimestamps(iterators);

            //Заполнение данных
            CurrentValue.Clear();

            foreach (KeyValuePair<int, List<AbstractValue>> v in data)
            {
                string[] values = new string[iterators.Count + 3];
                values[0] = (CurrentIndex + 1).ToString();
                values[1] = TimeInterval.ToString(CurrentTime);
                values[2] = v.Key.ToString();

                for (int i = 0; i < v.Value.Count; i++)
                    values[i + 3] = DataContainer.ValueDump.DataConvertorUtils.ToString(v.Value[i], iterators[i].DataRepresentation);

                CurrentValue.Add(values);
            }
        }



        public void Reset()
        {
            CurrentIndex = 0;
            foreach (var iter in iterators)
                iter.Reset();
        }

        #endregion
    }
}
