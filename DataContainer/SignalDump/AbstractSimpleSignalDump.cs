using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataContainer.MySortedDictionary;
using DataContainer.Value;

namespace DataContainer.SignalDump
{
    /// <summary>
    /// Класс, хранящий дамп для сигнала
    /// </summary>
    [System.Serializable]
    public class AbstractSimpleSignalDump<T> : AbstractSignalDump
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name"></param>
        public AbstractSimpleSignalDump(string name, ModellingType type)
            :base (name, type)
        {
            dump = new NewSortedDictionary<AbstractTimeStampInfo<T>>();
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name"></param>
        public AbstractSimpleSignalDump(string name, ModellingType type, AbstractValueConvertor<T> valueCovertor)
            : base(name, type)
        {
            this.valueCovertor = valueCovertor;
            dump = new NewSortedDictionary<AbstractTimeStampInfo<T>>();
        }

        /// <summary>
        /// Используется для преобразования типов данных
        /// </summary>
        private AbstractValueConvertor<T> valueCovertor;
        public  AbstractValueConvertor<T> ValueConvertor
        {
            get { return valueCovertor; }
            set { valueCovertor = value; }
        }
        

        /// <summary>
        /// Дамп данных
        /// </summary>
        protected NewSortedDictionary<AbstractTimeStampInfo<T>> dump;
        public NewSortedDictionary<AbstractTimeStampInfo<T>> Dump
        {
            get { return dump; }
        }

        /// <summary>
        /// Получение итератора
        /// </summary>
        public override IValueIterator Iterator 
        {
            get { return new NewSortedDictionaryIterator<T>(dump, valueCovertor); }
        }

        /// <summary>
        /// Количество свободных ячеек
        /// </summary>
        public override int FreeCells
        {
            get { return dump.Capacity - dump.Count; }
        }

        /// <summary>
        /// Добавление данных в конец дампа
        /// </summary>
        /// <param name="CurrentTime"></param>
        /// <param name="value"></param>
        public override void AppendValue(UInt64 CurrentTime, TimeStampInfo value)
        {
            if (value.Count == 0)
                return;

            AbstractTimeStampInfo<T> newValue = valueCovertor.GetAbstractTimeStampInfo(value);

            if (dump.Count >= 1)
            {
                UInt64 lastTime = dump.Keys[dump.Count - 1];
                AbstractTimeStampInfo<T> lastInfo = dump.Values[dump.Count - 1];
                if (lastTime < CurrentTime)
                {
                    using (AbstractValue first_value = value.FirstValue)
                    {
                        if (first_value.Equals(lastInfo.LastValue))
                        {
                            value.Info.Remove(0);
                        }
                        if (value.Count != 0)
                            dump.Add(CurrentTime, newValue);
                    }
                }
                else
                    dump.Append(CurrentTime, newValue);
            }
            else
                dump.Append(CurrentTime, newValue);
            
        }

        /// <summary>
        /// Добавление данных в конец дампа
        /// </summary>
        /// <param name="CurrentTime"></param>
        /// <param name="value"></param>
        public void AppendValue(UInt64 CurrentTime, T value)
        {
            AbstractTimeStampInfo<T> newValue = new AbstractTimeStampInfo<T>(value);

            if (dump.Count >= 1)
            {
                UInt64 lastTime = dump.Keys[dump.Count - 1];
                AbstractTimeStampInfo<T> lastInfo = dump.Values[dump.Count - 1];
                if (lastTime < CurrentTime)
                {
                    if (value.Equals(lastInfo.LastValue))
                    {
                        return;
                    }
                    dump.Add(CurrentTime, newValue);
                }
                else
                    dump.Append(CurrentTime, newValue);
            }
            else
                dump.Append(CurrentTime, newValue);
        }

        public override void InsertValues(SortedList<UInt64, TimeStampInfo> data, UInt64 StartTime, UInt64 EndTime)
        {
            SortedList<UInt64, AbstractTimeStampInfo<T>> items = new SortedList<ulong, AbstractTimeStampInfo<T>>();
            foreach (KeyValuePair<UInt64, TimeStampInfo> i in data)
            {
                items.Add(i.Key, valueCovertor.GetAbstractTimeStampInfo(i.Value));
            }
            dump.InsertValues(items, StartTime, EndTime);
        }

        public override void AddEvent(ulong NOW, ulong after, AbstractValue value, VHDL.DelayMechanism delayMechanism)
        {
            AbstractTimeStampInfo<T> newValue = new AbstractTimeStampInfo<T>(valueCovertor.GetValue(value));
            if (delayMechanism is VHDL.TRANSPORTDelayMechanism)
            {
                dump.AddTransportEvent(NOW + after, newValue);
                return;
            }

            if (delayMechanism.PulseRejectionLimit != null)
            {
                TIME_VALUE reject = ExpressionEvaluator.DefaultEvaluator.Evaluate(delayMechanism.PulseRejectionLimit) as TIME_VALUE;
                dump.AddInertialEvent(NOW, NOW + after, newValue, (UInt64)reject.DoubleValue);
                return;
            }

            if (delayMechanism is VHDL.INERTIALDelayMechanism)
            {
                dump.AddInertialEvent(NOW, NOW + after, newValue);
                return;
            }
        }

        /// <summary>
        /// Получение данных о моменте времени
        /// </summary>
        /// <param name="Time"></param>
        /// <returns></returns>
        public override TimeStampInfo GetValue(UInt64 Time)
        {
            return this.valueCovertor.GetTimeStampInfo(dump.GetValue(Time));
        }

        /// <summary>
        /// Начальное время
        /// </summary>
        public override UInt64 StartTime { get { return dump.StartTime; } }

        /// <summary>
        /// Конечное время
        /// </summary>
        public override UInt64 EndTime { get { return dump.EndTime; } }
    }
}
