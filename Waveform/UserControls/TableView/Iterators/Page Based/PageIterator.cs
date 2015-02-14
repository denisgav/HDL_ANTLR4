using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Schematix.Waveform.Value_Dump;
using DataContainer.MySortedDictionary;
using DataContainer;

namespace Schematix.Waveform.Iterators
{
    class PageIterator:IEnumerable<List<string []>>
    {
        private TableView form;

        public PageIterator(TableView form)
        {
            this.form = form;
            iterators = new List<IValueIterator>();
        }

        /// <summary>
        /// Итераторы
        /// </summary>
        private List<IValueIterator> iterators;
        /// <summary>
        /// разделение данных на страницы
        /// </summary>
        private List<DataPage> pages;
        public List<DataPage> Pages
        {
            get { return pages; }
            set { pages = value; }
        }
        /// <summary>
        /// Индекс текущей страницы
        /// </summary>
        private int pageIndex;
        public int PageIndex
        {
            get
            {
                return pageIndex;
            }
            set
            {
                pageIndex = value-1;
            }
        }

        public int TotalPageCount
        {
            get
            {
                if (pages == null)
                    return 0;
                else
                    return pages.Count;
            }
        }

        public void PrepareIterators()
        {
            pageIndex = 0;

            UpdateIterators();

            form.NumericUpDownPage.Maximum = pages.Count;
        }

        public void UpdateIterators()
        {
            iterators.Clear();
            foreach (My_Variable var in form.CurrentDump)
                iterators.Add(var.Iterator);
        }

        #region IEnumerable<List<string>> Members

        public IEnumerator<List<string []>> GetEnumerator()
        {
            if (pages.Count == 0)
                return null;
            return new PageDataEnumerator(iterators, pages[pageIndex]);
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new PageDataEnumerator(iterators, pages[pageIndex]);
        }

        #endregion
    }
}
