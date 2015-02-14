using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Schematix.Waveform.Value_Dump;
using DataContainer.MySortedDictionary;
using DataContainer;

namespace Schematix.Waveform.Iterators
{
    /// <summary>
    /// Итератор, который выдает данные, которые находятся
    /// в определенном временном интервале
    /// </summary>
    class TimeIterator : IEnumerable<List<string []>>
    {
        private WaveformCore core;
        private List<IValueIterator> iterators;

        /// <summary>
        /// Начальное время
        /// </summary>
        private UInt64 startTime;
        public UInt64 StartTime
        {
            get
            {
                return startTime;
            }
        }

        /// <summary>
        /// Конечное время
        /// </summary>
        private UInt64 endTime;
        public UInt64 EndTime
        {
            get
            {
                return endTime;
            }
        }

        public TimeIterator(WaveformCore core, UInt64 startTime, UInt64 endTime)
        {
            this.core = core;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public void PrepareIterators()
        {
            foreach (My_Variable var in core.CurrentDump)
                iterators.Add(var.Iterator);
        }


        #region IEnumerable<List<string>> Members

        public IEnumerator<List<string []>> GetEnumerator()
        {
            return new TimeDataEnumerator(iterators, startTime, endTime);
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new TimeDataEnumerator(iterators, startTime, endTime);
        }

        #endregion
    }
}
