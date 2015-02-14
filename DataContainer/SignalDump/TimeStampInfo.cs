using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataContainer.Value;
using DataContainer.MySortedDictionary;

namespace DataContainer
{
    /// <summary>
    /// Информация о моменте времени
    /// </summary>
    [System.Serializable]
    public class TimeStampInfo : AbstractTimeStampInfo<AbstractValue>
    {
        public TimeStampInfo(Dictionary<int, AbstractValue> info)
            : base(info)
        {
        }

        public TimeStampInfo(AbstractValue value)
            : base (value)
        {
        }

        public TimeStampInfo()
            : base()
        {
        }

        /// <summary>
        /// Используется для объединения дельтациклов в один объект TimeStampInfo
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        public static TimeStampInfo CombineTimestamps(ModellingType groupModellingType, params TimeStampInfo[] elements)
        {
            return CombineTimestamps(groupModellingType, new List<TimeStampInfo>(elements));
        }

        /// <summary>
        /// Используется для объединения дельтациклов в один объект TimeStampInfo
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        public static TimeStampInfo CombineTimestamps(ModellingType groupModellingType, IList<TimeStampInfo> elements)
        {
            TimeStampInfo res = new TimeStampInfo();

            List<AbstractValue> values = new List<AbstractValue>();
            List<TimeStampInfoIterator> iterators = new List<TimeStampInfoIterator>();
            int currentDeltaCycle = int.MaxValue;
            foreach (TimeStampInfo inf in elements)
            {
                if (inf != null)
                {
                    values.Add(inf[0]);
                    iterators.Add(new TimeStampInfoIterator(inf));
                    if (inf.ElementAt(0).Key < currentDeltaCycle)
                        currentDeltaCycle = inf.ElementAt(0).Key;
                }
            }

            bool IsDone = false;
            while (IsDone == false)
            {
                IsDone = true;
                foreach (TimeStampInfoIterator i in iterators)
                    if (i.IsDone == false)
                    {
                        IsDone = false;
                        break;
                    }

                if (IsDone == true)
                    break;

                currentDeltaCycle = int.MaxValue;

                foreach (TimeStampInfoIterator i in iterators)
                    if ((i.IsDone == false) && (i.Current.Key < currentDeltaCycle))
                        currentDeltaCycle = i.Current.Key;

                CompositeValue compValue = CompositeValue.CreateCompositeValue(groupModellingType, values);
                res.info.Add(currentDeltaCycle, compValue);

                for (int i = 0; i < iterators.Count; i++)
                {
                    TimeStampInfoIterator iter = iterators[i];
                    if (iter.Current.Key == currentDeltaCycle)
                    {
                        iter.MoveNext();
                        values[i] = iter.Current.Value;
                    }
                }
            }

            return res;
        }


        /// <summary>
        /// Используется при выводе информации в табличном виде
        /// из набора объектов IValueIterator формируется словарь
        /// ключ - номер дельта цикла
        /// значение - список значений элементов
        /// </summary>
        /// <param name="iterators"></param>
        /// <returns></returns>
        public static SortedDictionary<int, List<AbstractValue>> CombineTimestamps(IList<IValueIterator> iterators)
        {
            List<TimeStampInfo> elements = new List<TimeStampInfo>();
            foreach (IValueIterator i in iterators)
                elements.Add(i.CurrentValue);

            return CombineTimestamps(elements);
        }

        /// <summary>
        /// Используется при выводе информации в табличном виде
        /// из набора объектов TimeStampInfo формируется словарь
        /// ключ - номер дельта цикла
        /// значение - список значений элементов
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        public static SortedDictionary<int, List<AbstractValue>> CombineTimestamps(IList<TimeStampInfo> elements)
        {
            SortedDictionary<int, List<AbstractValue>> res = new SortedDictionary<int, List<AbstractValue>>();

            List<AbstractValue> values = new List<AbstractValue>();
            List<TimeStampInfoIterator> iterators = new List<TimeStampInfoIterator>();
            int currentDeltaCycle = int.MaxValue;
            foreach (TimeStampInfo inf in elements)
            {
                values.Add(inf[0]);
                iterators.Add(new TimeStampInfoIterator(inf));
                if (inf.ElementAt(0).Key < currentDeltaCycle)
                    currentDeltaCycle = inf.ElementAt(0).Key;
            }

            bool IsDone = false;
            while (IsDone == false)
            {
                IsDone = true;
                foreach (TimeStampInfoIterator i in iterators)
                    if (i.IsDone == false)
                    {
                        IsDone = false;
                        break;
                    }

                if (IsDone == true)
                    break;

                currentDeltaCycle = int.MaxValue;

                List<AbstractValue> compValue = new List<AbstractValue>();
                compValue.AddRange(values);
                foreach (TimeStampInfoIterator i in iterators)
                {
                    if ((i.IsDone == false) && (i.Current.Key < currentDeltaCycle))
                        currentDeltaCycle = i.Current.Key;
                }

                
                res.Add(currentDeltaCycle, compValue);

                for (int i = 0; i< iterators.Count; i++)
                {
                    TimeStampInfoIterator iter = iterators[i];
                    if (iter.Current.Key == currentDeltaCycle)
                    {
                        iter.MoveNext();
                        values[i] = iter.Current.Value;
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// Используется для разделения информации о моменте времени составного типа 
        /// на набор информаций об момоенте времени его составляющих
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static List<TimeStampInfo> SplitTimestamps(ModellingType groupModellingType,TimeStampInfo info)
        {
            if (groupModellingType.Type is VHDL.type.RecordType)
            {
                List<TimeStampInfo> res = new List<TimeStampInfo>();
                foreach(var el in (groupModellingType.Type as VHDL.type.RecordType).Elements)
                    foreach(string s in el.Identifiers)
                        res.Add(new TimeStampInfo());

                foreach (var val in info)
                {
                    if (val.Value is CompositeValue)
                    {
                        int index = 0;
                        foreach (AbstractValue v in (val.Value as CompositeValue).GetChilds())
                        {
                            if ((res[index].Count == 0) || (res[index].LastValue != v))
                                res[index].info.Add(val.Key, v);
                            index++;
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Invalid type", "info");
                    }
                }

                return res;
            }

            if (groupModellingType.Type is VHDL.type.ArrayType)
            {
                List<TimeStampInfo> res = new List<TimeStampInfo>();
                for (int i = 0; i < groupModellingType.SizeOf; i++)
                    res.Add(new TimeStampInfo());

                foreach (var val in info)
                {
                    if (val.Value is CompositeValue)
                    {
                        int index= 0;
                        foreach (AbstractValue v in (val.Value as CompositeValue).GetChilds())
                        {
                            if ((res[index].Count == 0) || (res[index].LastValue != v))
                                res[index].info.Add(val.Key, v);
                            index++;
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Invalid type", "info");
                    }
                }

                return res;
            }
            
            throw new ArgumentException("Invalid type", "groupModellingType");
        }


        protected new void Dispose(bool disposing)
        {            
            if (disposing == true)
            {
                //someone want the deterministic release of all resources
                //Let us release all the managed resources
            }
            else
            {
                // Do nothing, no one asked a dispose, the object went out of
                // scope and finalized is called so lets next round of GC 
                // release these resources
            }

            // Release the unmanaged resource in any case as they will not be 
            // released by GC
            if (info != null)
            {
                foreach (var v in info)
                    v.Value.Dispose();
                info.Clear();
            }
            info = null;
        }

        ~TimeStampInfo()
        {
            // The object went out of scope and finalized is called
            // Lets call dispose in to release unmanaged resources 
            // the managed resources will anyways be released when GC 
            // runs the next time.
            Dispose(false);
        }
    }
}
