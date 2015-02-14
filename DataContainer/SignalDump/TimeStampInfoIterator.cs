using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataContainer.Value;

namespace DataContainer
{
    /// <summary>
    /// Класс, который используется для перечисления элементов объекта класса TimeStampInfo
    /// </summary>
    [System.Serializable]
    public class TimeStampInfoIterator:IEnumerator<KeyValuePair<int, AbstractValue>>
    {
        private int currentIindex;
        private TimeStampInfo timeStampInfo;

        public TimeStampInfoIterator(TimeStampInfo timeStampInfo)
        {
            this.timeStampInfo = timeStampInfo;
            currentIindex = 0;
            current = timeStampInfo.ElementAt(0);
        }

        private KeyValuePair<int, AbstractValue> current;
        public KeyValuePair<int, AbstractValue> Current
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
