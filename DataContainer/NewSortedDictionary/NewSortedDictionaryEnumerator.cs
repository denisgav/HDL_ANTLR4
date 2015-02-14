using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace DataContainer.MySortedDictionary
{
    [System.Serializable]
    class NewSortedDictionaryEnumerator<TValue> : IDictionaryEnumerator
    {
        private NewSortedDictionary<TValue> dictionary;
        private int currentIndex = -1;

        public NewSortedDictionaryEnumerator(NewSortedDictionary<TValue> dictionary)
        {
            this.dictionary = dictionary;
            key = dictionary.Keys[0];
            value = dictionary.Values[0];
        }


        #region IDictionaryEnumerator Members

        public DictionaryEntry Entry
        {
            get
            {
                return new DictionaryEntry(key, value);
            }
        }

        private object key;
        public object Key
        {
            get { return key; }
        }

        private object value;
        public object Value
        {
            get { return value; }
        }

        #endregion

        #region IEnumerator Members

        public object Current
        {
            get { return new DictionaryEntry(key, value); }
        }

        public bool MoveNext()
        {
            if (currentIndex < dictionary.Count - 1)
            {
                currentIndex++;
                key = dictionary.Keys[currentIndex];
                value = dictionary.Values[currentIndex];
                return true;
            }
            else
                return false;
        }

        public void Reset()
        {
            currentIndex = -1;
        }

        #endregion
    }
}
