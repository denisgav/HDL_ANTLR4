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

namespace VHDLRuntime
{
    public enum TimeUnit
    {
        s,
        ms,
        us,
        ns,
        ps,
        fs
    }

    /// <summary>
    /// Структура, хранящая некоторый промежуток времени
    /// </summary>
    public class TimeInterval
    {
        /// <summary>
        /// Промежуток времени
        /// </summary>
        protected int time_number;
        public virtual int TimeNumber
        {
            get
            {
                return time_number;
            }
            set
            {
                time_number = value;
            }
        }

        /// <summary>
        /// Единица измерения времени
        /// </summary>
        protected TimeUnit time_unit;
        public TimeUnit Unit
        {
            get
            {
                return time_unit;
            }
            set
            {
                time_unit = value;
            }
        }

        public TimeInterval(UInt64 time)
        {
            if ((time % (ulong)1e15) == 0)
            {
                time_number = (int) (time / (ulong)1e15);
                time_unit = TimeUnit.s;
                return;
            }
            if ((time % (ulong)1e12) == 0)
            {
                time_number = (int)(time / (ulong)1e12);
                time_unit = TimeUnit.ms;
                return;
            }
            if ((time % (ulong)1e9) == 0)
            {
                time_number = (int)(time / (ulong)1e9);
                time_unit = TimeUnit.us;
                return;
            }
            if ((time % (ulong)1e6) == 0)
            {
                time_number = (int)(time / (ulong)1e6);
                time_unit = TimeUnit.ns;
                return;
            }
            if ((time % (ulong)1e3) == 0)
            {
                time_number = (int)(time / (ulong)1e3);
                time_unit = TimeUnit.ps;
                return;
            }
            time_number = (int)time;
            time_unit = TimeUnit.fs;
        }

        public TimeInterval(int TimeNumber, TimeUnit Unit)
        {
            this.time_unit = Unit;
            this.time_number = TimeNumber;
        }

        public TimeInterval() { }

        public override string ToString()
        {
            return string.Format("{0} {1}", time_number, time_unit);
        }

        public UInt64 GetTimeUnitInFS()
        {
            UInt64 mul = 0;
            switch (time_unit)
            {
                case TimeUnit.s: mul =  (ulong)1e15; break;
                case TimeUnit.ms: mul = (ulong)1e12; break;
                case TimeUnit.us: mul = (ulong)1e9; break;
                case TimeUnit.ns: mul = (ulong)1e6; break;
                case TimeUnit.ps: mul = (ulong)1e3; break;
                case TimeUnit.fs: mul = (ulong)1; break;
                default: mul = 0; break;
            }

            return mul * (ulong)time_number;
        }

        public static string ToString(UInt64 value)
        {
            if((value % (ulong)1e15) == 0)
                return string.Format("{0:n0} s", value/(ulong)1e15);
            if ((value % (ulong)1e12) == 0)
                return string.Format("{0:n0} ms", value / (ulong)1e12);
            if ((value % (ulong)1e9) == 0)
                return string.Format("{0:n0} us", value / (ulong)1e9);
            if ((value % (ulong)1e6) == 0)
                return string.Format("{0:n0} ns", value / (ulong)1e6);
            if ((value % (ulong)1e3) == 0)
                return string.Format("{0:n0} ps", value / (ulong)1e3);

            return string.Format("{0:n0} fs", value);
        }
    }

    /// <summary>
    /// Структура хранящая единицу модельного времени
    /// </summary>
    public  class Timescale: TimeInterval
    {
        public Timescale(int TimeNumber, TimeUnit Unit)
        {
            this.time_unit = Unit;
            this.TimeNumber = TimeNumber;
        }

        public override int TimeNumber
        {
            get
            {
                return base.TimeNumber;
            }
            set
            {
                if ((value == 1) || (value == 10) || (value == 100))
                    time_number = value;
                else
                    throw new Exception("Invalid time number");
            }
        }

        #region Parsing
        /// <summary>
        /// Функция для парсинга единицы модельного времени
        /// (может генерировать исключения)
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static Timescale Parse(string Text)
        {
            if (string.IsNullOrEmpty(Text) == true)
                throw new Exception("String is null or empty");
            string[] parts = Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
                throw new Exception("String is not correct");
            int time_number = int.Parse(parts[0]);
            TimeUnit time_unit = TimeUnit.fs;
            switch (parts[1])
            {
                case "s": time_unit = TimeUnit.s; break;
                case "ms": time_unit = TimeUnit.ms; break;
                case "us": time_unit = TimeUnit.us; break;
                case "ns": time_unit = TimeUnit.ns; break;
                case "ps": time_unit = TimeUnit.ps; break;
                case "fs": time_unit = TimeUnit.fs; break;
                default: throw new Exception("Unknown time unit"); break;
            }
            return new Timescale(time_number, time_unit);
        }

        /// <summary>
        /// Функция для парсинга единицы модельного времени
        /// (не выдает исключений)
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="timescale"></param>
        /// <returns></returns>
        public static bool TryParse(string Text, out Timescale timescale)
        {
            if (string.IsNullOrEmpty(Text) == true)
            {
                timescale = null;
                return false;
            }
            string[] parts = Text.Split(new char[] { ' ' });
            if (parts.Length != 2)
            {
                timescale = null;
                return false;
            }
            int time_number = int.Parse(parts[0]);
            TimeUnit time_unit = TimeUnit.fs;
            switch (parts[1])
            {
                case "s": time_unit = TimeUnit.s; break;
                case "ms": time_unit = TimeUnit.ms; break;
                case "us": time_unit = TimeUnit.us; break;
                case "ns": time_unit = TimeUnit.ns; break;
                case "ps": time_unit = TimeUnit.ps; break;
                case "fs": time_unit = TimeUnit.fs; break;
                default:
                    {
                        timescale = null;
                        return false;
                    }
                    break;
            }
            timescale = new Timescale(time_number, time_unit);
            return true;
        }
        #endregion
    }
}
