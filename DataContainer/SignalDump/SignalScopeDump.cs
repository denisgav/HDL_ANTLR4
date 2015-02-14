using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataContainer.MySortedDictionary;
using DataContainer.Value;
using VHDL;

namespace DataContainer.SignalDump
{
    /// <summary>
    /// Класс, хранящий дамп набора сигналов
    /// используется для многомерных массивов и записей
    /// </summary>
    [System.Serializable]
    public class SignalScopeDump : AbstractSignalDump
    {
        public SignalScopeDump(string name, ModellingType type, List<AbstractSignalDump> dumps)
            : base(name, type)
        {
            this.dumps = dumps;
        }

        /// <summary>
        /// Набор хранимых дампов
        /// </summary>
        private List<AbstractSignalDump> dumps;
        public List<AbstractSignalDump> Dumps
        {
            get { return dumps; }
        }

        /// <summary>
        /// Получение итератора
        /// </summary>
        public override MySortedDictionary.IValueIterator Iterator
        {
            get { return new NewSortedDictionaryScopeIterator(this); }
        }

        /// <summary>
        /// Количество свободных ячеек
        /// </summary>
        public override int FreeCells
        {
            get
            {
                int min = int.MaxValue;
                foreach (AbstractSignalDump d in dumps)
                {
                    if (d.FreeCells < min)
                        min = d.FreeCells;
                }
                return min;
            }
        }

        /// <summary>
        /// Добавление данных в конец дампа
        /// </summary>
        /// <param name="CurrentTime"></param>
        /// <param name="value"></param>
        public override void AppendValue(ulong CurrentTime, TimeStampInfo value)
        {
            List<TimeStampInfo> info = TimeStampInfo.SplitTimestamps(Type, value);
            for(int i=0; i<dumps.Count; i++)
                dumps[i].AppendValue(CurrentTime, info[i]);

            foreach (TimeStampInfo i in info)
            {
                i.Dispose();
            }
        }

        public override void AddEvent(ulong NOW, ulong after, AbstractValue value, DelayMechanism delayMechanism)
        {
            IList<AbstractValue> childs = (value as CompositeValue).GetChilds();
            for (int i = 0; i < dumps.Count; i++)
                dumps[i].AddEvent(NOW, after, childs[i], delayMechanism);

            foreach (AbstractValue i in childs)
            {
                i.Dispose();
            }
        }

        public override void InsertValues(SortedList<UInt64, TimeStampInfo> data, UInt64 StartTime, UInt64 EndTime)
        {
            Dictionary<AbstractSignalDump, SortedList<UInt64, TimeStampInfo>> insData = new Dictionary<AbstractSignalDump, SortedList<UInt64, TimeStampInfo>>();
            foreach (AbstractSignalDump d in dumps)
            {
                insData.Add(d, new SortedList<ulong, TimeStampInfo>());
            }

            foreach (var el in data)
            {
                List<TimeStampInfo> elements = TimeStampInfo.SplitTimestamps(base.Type, el.Value);
                for (int i = 0; i < elements.Count; i++)
                    insData.ElementAt(i).Value.Add(el.Key, elements[i]);
            }

            foreach (var el in insData)
                el.Key.InsertValues(el.Value, StartTime, EndTime);
        }

        /// <summary>
        /// Получение данных о моменте времени
        /// </summary>
        /// <param name="Time"></param>
        /// <returns></returns>
        public override TimeStampInfo GetValue(UInt64 Time)
        {
            List<TimeStampInfo> res = new List<TimeStampInfo>();
            foreach(AbstractSignalDump d in dumps)
                res.Add(d.GetValue(Time));
            return TimeStampInfo.CombineTimestamps(Type, res);
        }

        /// <summary>
        /// Начальное время
        /// </summary>
        public override UInt64 StartTime 
        {
            get
            {
                return 0; 
            } 
        }

        /// <summary>
        /// Конечное время
        /// </summary>
        public override UInt64 EndTime 
        { 
            get 
            {
                UInt64 max = 0;
                foreach (var value in dumps)
                {
                    UInt64 v = value.EndTime;
                    if (max < v)
                        max = v;
                }
                return max;
            }
        }
    }
}