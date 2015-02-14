using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using DataContainer.MySortedDictionary;
using DataContainer;

namespace Schematix.Waveform.Iterators
{
    class TimeDataEnumerator : DataEnumerator
    {
        private UInt64 startTime;
        private UInt64 endTime;

        public TimeDataEnumerator(List<IValueIterator> iterators, UInt64 startTime, UInt64 endTime)
            : base(iterators)
        {
            this.startTime = startTime;
            this.endTime = endTime;
            CurrentValue = new List<string []>();
            foreach (var iter in iterators)
                iter.SetCurrentIndexByKey(startTime);
        }

        public override bool IsEndOfIteration()
        {
            //Проверяем, есть ли ещо данные для выборки
            bool IsEndOfData = true;
            foreach (var iter in iterators)
                if (iter.LastEvent < endTime)
                {
                    IsEndOfData = false;
                    break;
                }
            if (IsEndOfData == true)
                return false;
            else
                return true;
        }
    }
}
