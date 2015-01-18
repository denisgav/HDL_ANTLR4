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
using VHDLRuntime.ValueDump;
using VHDLRuntime;

namespace VHDLInputGenerators.Counters
{
    internal enum CounterRype
    {
        GrayCounter,
        JohnsonCounter,
        BinaryCounter,
        Circular0,
        Circular1
    }

    public abstract class Counter : BaseGenerator, IGeneratorDataFill<bool[]>, IGeneratorDataFill<Int64>, IGeneratorDataFill<Double>
    {
        /// <summary>
        /// следующее число после текущего
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract bool[] Next(bool[] value);

        /// <summary>
        /// число перед текущим
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract bool[] Prev(bool[] value);

        /// <summary>
        /// число через step_count после текущего
        /// </summary>
        /// <param name="value"></param>
        /// <param name="step_count"></param>
        /// <returns></returns>
        public virtual bool[] Next(bool[] value, uint step_count)
        {
            //периодичность с которой повторяется последовательность кратна 2^n
            //все лишние итерации отбрасываем
            step_count %= (uint)Math.Pow(2, value.Length);

            bool [] res = new bool[value.Length];
            value.CopyTo(res, 0);

            for (int i = 0; i < step_count; i++)
                res = Next(res);

            return res;
        }

        /// <summary>
        /// число через step_count перед текущего
        /// </summary>
        /// <param name="value"></param>
        /// <param name="step_count"></param>
        /// <returns></returns>
        public virtual bool[] Prev(bool[] value, uint step_count)
        {
            //периодичность с которой повторяется последовательность кратна 2^n
            //все лишние итерации отбрасываем
            step_count %= (uint)Math.Pow(2, value.Length);

            bool[] res = new bool[value.Length];
            value.CopyTo(res, 0);

            for (int i = 0; i < step_count; i++)
                res = Prev(res);

            return res;
        }

        /// <summary>
        /// Является ли текущая входная последовательность корректной для данного щотчика
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract bool CheckForCorrect(bool[] value);

        /// <summary>
        /// Поиск первого элемента value в массиве mas
        /// </summary>
        /// <param name="mas"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected int StartIndexOf(bool[] mas, bool value)
        {
            for (int i = 0; i < mas.Length; i++)
                if (mas[i] == value)
                    return i;
            return -1;
        }

        /// <summary>
        /// Поиск последнего элемента value в массиве mas
        /// </summary>
        /// <param name="mas"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected int LastIndexOf(bool[] mas, bool value)
        {
            for (int i = mas.Length - 1; i >= 0 ; i--)
                if (mas[i] == value)
                    return i;
            return -1;
        }

        /// <summary>
        /// Конвертирование числа value в массив bool[]
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool[] ConvertUIntegerToBoolArray(uint value)
        {
            List<bool> res = new List<bool>();
            while (value > 0)
            {
                res.Add(value % 2 == 1);
                value /= 2;
            }
            res.Reverse();
            return res.ToArray();
        }

        /// <summary>
        /// Конвертирование массив bool[] в число value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static uint ConvertBoolArrayToUInteger(bool[] value)
        {
            uint res = 0;
            uint pow = 1;
            for (int i = value.Length-1; i >=0 ; i--, pow *= 2)
            {
                if (value[i] == true)
                    res += pow;
            }
            return res;
        }

        /// <summary>
        /// Текущее значение массива
        /// </summary>
        protected bool[] currentVale;
        public bool[] CurrentValue
        {
            get
            {
                return currentVale;
            }
            set
            {
                if (CheckForCorrect(value) == true)
                    currentVale = value;
            }
        }

        /// <summary>
        /// единичный шаг
        /// </summary>
        protected uint stepCount;
        public uint StepCount
        {
            get { return stepCount; }
            set { stepCount = value; }
        }

        /// <summary>
        /// Направление генератора
        /// true - Next
        /// false - Prev
        /// </summary>
        protected bool isUpDirection;
        public bool IsUpDirection
        {
            get { return isUpDirection;  }
            set { isUpDirection = value; }
        }

        /// <summary>
        /// Следующее значение щетчика после currentVale через stepCount шагов
        /// </summary>
        /// <returns></returns>
        public virtual bool[] Next()
        {
            bool[] res = Next(currentVale, stepCount);
            currentVale = res;
            return res;
        }

        /// <summary>
        /// Следующее значение щетчика после currentVale через step_count итераций
        /// </summary>
        /// <returns></returns>
        public virtual bool[] Next(uint step_count)
        {
            bool[] res = Next(currentVale, step_count);
            currentVale = res;
            return res;
        }

        /// <summary>
        /// значение щетчика перед currentVale через stepCount шагов
        /// </summary>
        /// <returns></returns>
        public virtual bool[] Prev()
        {
            bool[] res = Prev(currentVale, stepCount);
            currentVale = res;
            return res;
        }

        /// <summary>
        /// значение щетчика за step_count итераций перед currentVale
        /// </summary>
        /// <param name="step_count"></param>
        /// <returns></returns>
        public virtual bool[] Prev(uint step_count)
        {
            bool[] res = Prev(currentVale, step_count);
            currentVale = res;
            return res;
        }

        public IEnumerable<bool[]> GetBitArrayEnumerator()
        {
            yield return CurrentValue;
            while(true)
            {
                if (isUpDirection == true)
                    yield return Next();
                else
                    yield return Prev();
            }
        }

        public IEnumerable<long> GetIntegerEnumerator()
        {
            yield return DataConvertorUtils.ToLong(CurrentValue);
            while (true)
            {
                if (isUpDirection == true)
                    yield return DataConvertorUtils.ToLong(Next());
                else
                    yield return DataConvertorUtils.ToLong(Prev());
            }
        }

        public IEnumerable<double> GetDoubleEnumerator()
        {
            yield return DataConvertorUtils.ToLong(CurrentValue);
            while (true)
            {
                if (isUpDirection == true)
                    yield return DataConvertorUtils.ToLong(Next());
                else
                    yield return DataConvertorUtils.ToLong(Prev());
            }
        }

        public override int SizeInBits
        {
            get
            {
                return CurrentValue.Length;
            }
        }

        public override string GetStringStartValue()
        {
            StringBuilder res = new StringBuilder();
            res.Append("\"");
            for (int i = CurrentValue.Length-1; i >= 0; i--)
            {
                res.Append(DataConvertorUtils.BoolToInt(CurrentValue[i]));
            }
            res.Append("\"");
            return res.ToString();
        }

        public override void StreamVhdlRealization(KeyValuePair<string, TimeInterval> param, System.IO.StreamWriter sw)
        {
            sw.Write(StringVhdlRealization(param).ToString());
        }

        SortedList<ulong, bool[]> IGeneratorDataFill<bool[]>.InsertValues(ulong StartTime, ulong EndTime)
        {
            SortedList<UInt64, bool[]> data = new SortedList<ulong, bool[]>();
            UInt64 time = StartTime;

            foreach (var b in GetBitArrayEnumerator())
            {
                data.Add(time, b);
                time += timeStep.GetTimeUnitInFS();

                if (time > EndTime)
                    break;
            }

            return data;
        }

        SortedList<ulong, long> IGeneratorDataFill<long>.InsertValues(ulong StartTime, ulong EndTime)
        {
            SortedList<UInt64, Int64> data = new SortedList<ulong, Int64>();
            UInt64 time = StartTime;

            foreach (var b in GetIntegerEnumerator())
            {
                data.Add(time, b);
                time += timeStep.GetTimeUnitInFS();

                if (time > EndTime)
                    break;
            }

            return data;
        }

        SortedList<ulong, double> IGeneratorDataFill<double>.InsertValues(ulong StartTime, ulong EndTime)
        {
            SortedList<UInt64, Double> data = new SortedList<ulong, Double>();
            UInt64 time = StartTime;

            foreach (var b in GetDoubleEnumerator())
            {
                data.Add(time, b);
                time += timeStep.GetTimeUnitInFS();

                if (time > EndTime)
                    break;
            }

            return data;
        }
    }
}
