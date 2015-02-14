using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schematix.Waveform.Value_Dump
{
    /// <summary>
    /// Класс для хранения данных для измерения временного интервала
    /// </summary>
    public class TimeMeasureData
    {
        /// <summary>
        /// Начальное время
        /// </summary>
        private UInt64 timeStart;
        public UInt64 TimeStart
        {
            get { return timeStart; }
            set { timeStart = value; }
        }

        /// <summary>
        /// Конечное время
        /// </summary>
        private UInt64 timeEnd;
        public UInt64 TimeEnd
        {
            get { return timeEnd; }
            set { timeEnd = value; }
        }

        /// <summary>
        /// Первая переменная
        /// </summary>
        private My_Variable variable1;
        public My_Variable Variable1
        {
            get { return variable1; }
            set { variable1 = value; }
        }

        /// <summary>
        /// Вторая переменная
        /// </summary>
        private My_Variable variable2;
        public My_Variable Variable2
        {
            get { return variable2; }
            set { variable2 = value; }
        }

        public TimeMeasureData(UInt64 timeStart, UInt64 timeEnd, My_Variable variable1, My_Variable variable2)
        {
            this.timeStart = Math.Min(timeStart, timeEnd);
            this.timeEnd = Math.Max(timeStart, timeEnd);
            this.variable1 = (timeStart < timeEnd) ? variable1 : variable2;
            this.variable2 = (timeStart < timeEnd) ? variable2 : variable1;
        }

        public TimeMeasureData(TimeMarker timeStart, TimeMarker timeEnd, My_Variable variable1, My_Variable variable2)
            :this(timeStart.Time, timeEnd.Time, variable1, variable2)
        { }
    }

    /// <summary>
    /// Маркер, используемый для хранения точек изменения сигнала
    /// </summary>
    public class TimeMarker
    {
        /// <summary>
        /// Момент времени
        /// </summary>
        private UInt64 time;
        public UInt64 Time
        {
            get { return time; }
            set { time = value; }
        }

        /// <summary>
        /// Смещение
        /// </summary>
        private double offset;
        public double Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        public TimeMarker(UInt64 time, double offset)
        {
            this.time = time;
            this.offset = offset;
        }        
    }
}
