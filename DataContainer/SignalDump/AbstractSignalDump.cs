using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataContainer.MySortedDictionary;
using DataContainer.Value;
using DataContainer.Objects;

namespace DataContainer.SignalDump
{
    /// <summary>
    /// Абстрактный класс, используемый для хранения дампа сигнала
    /// </summary>
    [System.Serializable]
    public abstract class AbstractSignalDump
    {
        /// <summary>
        /// Имя сигнала
        /// </summary>
        private string name;
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Тип значения
        /// </summary>
        private readonly ModellingType type;
        public ModellingType Type
        {
            get { return type; }
        }

        /// <summary>
        /// Получение итератора
        /// </summary>
        public abstract IValueIterator Iterator { get; }

        /// <summary>
        /// Количество свободных ячеек
        /// </summary>
        public abstract int FreeCells { get; }

        public AbstractSignalDump(string name, ModellingType type)
        {
            this.type = type;
            this.name = name;
        }

        /// <summary>
        /// Добавлене данных в конец дампа
        /// </summary>
        /// <param name="CurrentTime"></param>
        /// <param name="value"></param>
        public void AppendValue(UInt64 CurrentTime, AbstractValue value)
        {
            using (TimeStampInfo ts = new TimeStampInfo(value))
            {
                AppendValue(CurrentTime, ts);
            }
        }

        /// <summary>
        /// Добавлене данных в конец дампа
        /// </summary>
        /// <param name="CurrentTime"></param>
        /// <param name="value"></param>
        public abstract void AppendValue(UInt64 CurrentTime, TimeStampInfo value);


        /// <summary>
        /// Добавление в дамп сгенерированных данных
        /// </summary>
        /// <param name="data"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        public abstract void InsertValues(SortedList<UInt64, TimeStampInfo> data, UInt64 StartTime, UInt64 EndTime);

        /// <summary>
        /// Получение данных о моменте времени
        /// </summary>
        /// <param name="Time"></param>
        /// <returns></returns>
        public abstract TimeStampInfo GetValue(UInt64 Time);

        /// <summary>
        /// Получение значения текущего итератора в виде строки
        /// </summary>
        /// <returns></returns>
        public static string GetStringValue(IValueIterator iterator)
        {
            if (iterator.CurrentValue == null)
                return "<NULL>";
            string result = string.Empty;

            AbstractValue val = iterator.CurrentValue.LastValue;
            result = DataContainer.ValueDump.DataConvertorUtils.ToString(val, iterator.DataRepresentation);
            
            return result;
        }

        /// <summary>
        /// Начальное время
        /// </summary>
        public abstract UInt64 StartTime { get; }

        /// <summary>
        /// Конечное время
        /// </summary>
        public abstract UInt64 EndTime { get; }

        /// <summary>
        /// Создание сигнала из имеющегося дампа
        /// </summary>
        /// <returns></returns>
        public Signal CreateSignal()
        {
            return new Signal(this);
        }

        /// <summary>
        /// Добавление события в очередь назначения сигнала
        /// </summary>
        /// <param name="NOW"></param>
        /// <param name="after"></param>
        /// <param name="value"></param>
        /// <param name="delayMechanism"></param>
        public abstract void AddEvent(ulong NOW, ulong after, AbstractValue value, VHDL.DelayMechanism delayMechanism);
    }
}
