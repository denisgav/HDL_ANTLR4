//
//  Copyright (C) 2010-2014  Denis Gavrish
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace VHDLRuntime.MySortedDictionary
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
