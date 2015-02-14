using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataContainer.MySortedDictionary;
using DataContainer;

namespace Schematix.Waveform.Iterators
{
    /// <summary>
    /// этот класс используется дла разделения всех данных на "страницы"
    /// при отображении
    /// </summary>
    struct DataPage
    {
        /// <summary>
        /// количество записей на странице
        /// </summary>
        private int count;
        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        /// <summary>
        /// Значения времени в итераторах
        /// </summary>
        private List<UInt64> iteratorTimes;
        public List<UInt64> IteratorTimes
        {
            get { return iteratorTimes; }
            set { iteratorTimes = value; }
        }

        private UInt64 previousTime;
        public UInt64 PreviousTime
        {
            get { return previousTime; }
            set { previousTime = value; }
        }

        /// <summary>
        /// Индекс начальной записи
        /// </summary>
        private int startIndex;
        public int StartIndex
        {
            get { return startIndex; }
            set { startIndex = value; }
        }

        public DataPage(List<IValueIterator> iterators, UInt64 previousTime, int count, int startIndex)
        {
            this.previousTime = previousTime;
            this.count = count;
            this.startIndex = startIndex;
            iteratorTimes = new List<UInt64>();
            foreach (var iter in iterators)
            {
                iteratorTimes.Add(iter.LastEvent);
            }
        }

        /// <summary>
        /// Установка значений итераторов
        /// </summary>
        /// <param name="iterators"></param>
        public void SetData(List<IValueIterator> iterators)
        {
            if (iteratorTimes.Count != iterators.Count)
                throw new Exception("Count of iterators is not correct");
            for (int i = 0; i < iterators.Count; i++)
            {
                iterators[i].SetCurrentIndexByKey(iteratorTimes[i]);
            }
        }

        /// <summary>
        /// Количество записей на одной странице
        /// </summary>
        public const int PageItemsCount = 1000;
    }    
}
