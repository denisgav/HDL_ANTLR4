using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using DataContainer.MySortedDictionary;
using DataContainer;

namespace Schematix.Waveform.Iterators
{
    class SimpleDataEnumerator:DataEnumerator
    {
        public SimpleDataEnumerator(List<IValueIterator> iterators)
            :base(iterators)
        {}
    }
}
