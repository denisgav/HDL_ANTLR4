using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Schematix.Waveform.Value_Dump;
using DataContainer;
using DataContainer.MySortedDictionary;

namespace Schematix.Waveform.Iterators
{
    class SimpleIterator : IEnumerable<List<string []>>
    {
        private WaveformCore core;
        private List<IValueIterator> iterators;

        public SimpleIterator(WaveformCore core)
        {
            this.core = core;
            iterators = new List<IValueIterator>();
            PrepareIterators();
        }

        public void PrepareIterators()
        {
            foreach (My_Variable var in core.CurrentDump)
                iterators.Add(var.Iterator);
        }
        #region IEnumerable<List<string>> Members

        public IEnumerator<List<string []>> GetEnumerator()
        {
            return new SimpleDataEnumerator(iterators);
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new SimpleDataEnumerator(iterators);
        }

        #endregion
    }
}
