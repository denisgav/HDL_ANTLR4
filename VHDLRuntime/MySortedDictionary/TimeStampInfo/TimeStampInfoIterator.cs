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
using System.Linq;
using System.Text;

namespace VHDLRuntime.MySortedDictionary
{
    /// <summary>
    /// Класс, который используется для перечисления элементов объекта класса TimeStampInfo
    /// </summary>
    [System.Serializable]
    public class TimeStampInfoIterator<T>:IEnumerator<KeyValuePair<int, T>>
    {
        private int currentIindex;
        private TimeStampInfo<T> timeStampInfo;

        public TimeStampInfoIterator(TimeStampInfo<T> timeStampInfo)
        {
            this.timeStampInfo = timeStampInfo;
            currentIindex = 0;
            current = timeStampInfo.ElementAt(0);
        }

        private KeyValuePair<int, T> current;
        public KeyValuePair<int, T> Current
        {
            get { return current; }
        }

        object System.Collections.IEnumerator.Current
        {
            get { return current; }
        }
        
        public bool MoveNext()
        {
            currentIindex ++;
            if (currentIindex < timeStampInfo.Count)
            {
                current = timeStampInfo.ElementAt(currentIindex);
                return true;
            }
            else
                return false;
        }

        public bool IsDone
        {
            get { return currentIindex >= timeStampInfo.Count; }
        }

        public void Reset()
        {
            currentIindex = 0;
            current = timeStampInfo.ElementAt(0);
        }
        
        public void Dispose()
        {
            
        }
    }
}
