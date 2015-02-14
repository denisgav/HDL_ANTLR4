using System.Collections.Generic;
using System.Collections;
using System;
using System.Text;
using DataContainer.MySortedDictionary;
using DataContainer;
using DataContainer.SignalDump;

namespace Schematix.Waveform.Iterators
{
    class PageDataEnumerator : DataEnumerator
    {
        private DataPage page;
        private int count;

        public PageDataEnumerator(List<IValueIterator> iterators, DataPage page)
            :base(iterators)
        {
            this.page = page;
            CurrentIndex = page.StartIndex;
            page.SetData(iterators);
            PreviousTime = page.PreviousTime;

            FormCurrentValue(PreviousTime);
        }

        public override bool  IsEndOfIteration()
        {
 	        //Проверяем, есть ли ещо данные для выборки
            if ((count > page.Count) || (base.IsEndOfIteration() == true))
            {
                return true;
            }
            else
                return false;
        }

        public override bool MoveNext()
        {
            count++;
            return base.MoveNext();
        }
    }
}
