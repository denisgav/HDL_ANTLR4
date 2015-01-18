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
using VHDLRuntime.ValueDump;
using VHDLRuntime;


namespace VHDLInputGenerators
{
    public class Constant : BaseGenerator, IGeneratorDataFill<object>, IGeneratorDataFill<Int64>, IGeneratorDataFill<bool[]>, IGeneratorDataFill<Double>
    {
        public Constant()
        {
            timeStep = new TimeInterval(UInt64.MaxValue);
        }

        /// <summary>
        /// Тип генерируемого значения
        /// </summary>
        private GeneratorSettings.GeneratedValue genValue;
        public GeneratorSettings.GeneratedValue GenValue
        {
            get { return genValue; }
            set 
            {
                if (
                    (value == GeneratorSettings.GeneratedValue.DoubleValue) ||
                    (value == GeneratorSettings.GeneratedValue.IntegerValue) ||
                    (value == GeneratorSettings.GeneratedValue.EnumerableValue)||
                    (value == GeneratorSettings.GeneratedValue.BoolArray))
                    genValue = value;
                else
                    throw new Exception("generable data type is not supported");
            }
        }

        /// <summary>
        /// Константный нулевой генератор
        /// </summary>
        public static Constant Zero
        {
            get
            {
                Constant zero = new Constant();

                zero.EnumerableValue = "0";

                return zero;
            }
        }

        /// <summary>
        /// Константный единичный генератор
        /// </summary>
        public static Constant One
        {
            get
            {
                Constant one = new Constant();

                one.EnumerableValue = "1";

                return one;
            }
        }

        public uint SizeVector { get; set; }

        /// <summary>
        /// Целочисленное значение
        /// </summary>
        public int IntegerValue { get; set; }

        /// <summary>
        /// Вещественное значение
        /// </summary>
        public double DoubleValue { get; set; }


        /// <summary>
        /// Перечисляемое значение
        /// </summary>
        public object EnumerableValue { get; set; }

        public IEnumerable<long> GetIntegerEnumerator()
        {
            if (genValue == GeneratorSettings.GeneratedValue.IntegerValue)
                yield return IntegerValue;
            else
                throw new Exception("Type does not support this enumeration");
        }

        public IEnumerable<double> GetDoubleEnumerator()
        {
            if (genValue == GeneratorSettings.GeneratedValue.DoubleValue)
                yield return DoubleValue;
            else
                throw new Exception("Type does not support this enumeration");
        }

        public IEnumerable<bool[]> GetBitArrayEnumerator()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetEnumearbleValueEnumerator()
        {
            if (genValue == GeneratorSettings.GeneratedValue.EnumerableValue)
                yield return EnumerableValue;
            else
                throw new Exception("Type does not support this enumeration");
        }

        public override string GetStringStartValue()
        {
            switch (genValue)
            {
                case GeneratorSettings.GeneratedValue.EnumerableValue:
                    return '\'' + EnumerableValue.ToString() + '\'';
                case GeneratorSettings.GeneratedValue.IntegerValue:
                    return IntegerValue.ToString();
                case GeneratorSettings.GeneratedValue.DoubleValue:
                    return DoubleValue.ToString();
                default:
                    return string.Empty;
            }
        }

        public override StringBuilder StringVhdlRealization(KeyValuePair<string, TimeInterval> param)
        {
            StringBuilder res = new StringBuilder();

            res.Append("\n").Append(param.Key).Append(" <= ");
            switch (genValue)
            {
                case GeneratorSettings.GeneratedValue.EnumerableValue:
                    res.Append('\'' + EnumerableValue.ToString() + '\'');
                    break;

                case GeneratorSettings.GeneratedValue.IntegerValue:
                    res.Append(IntegerValue.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    break;

                case GeneratorSettings.GeneratedValue.DoubleValue:
                    res.Append(DoubleValue.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    break;
                default:
                    break;
            }
            res.Append(" ; ").Append("\n");
            return res;
        }

        public override void StreamVhdlRealization(KeyValuePair<string, TimeInterval> param, System.IO.StreamWriter sw)
        {
            sw.Write(StringVhdlRealization(param).ToString());
        }

        public override string ToString()
        {
            switch (genValue)
            {
                case GeneratorSettings.GeneratedValue.EnumerableValue:
                    return "Constant" + '\'' + EnumerableValue.ToString() + '\'';
                case GeneratorSettings.GeneratedValue.IntegerValue:
                    return "Constant" + IntegerValue.ToString();
                case GeneratorSettings.GeneratedValue.DoubleValue:
                    return "Constant" + DoubleValue.ToString();
                default:
                    return string.Empty;
            }
        }

        SortedList<ulong, object> IGeneratorDataFill<object>.InsertValues(ulong StartTime, ulong EndTime)
        {
            SortedList<UInt64, object> data = new SortedList<ulong, object>();
            data.Add(StartTime, EnumerableValue);
            return data;
        }

        SortedList<ulong, long> IGeneratorDataFill<long>.InsertValues(ulong StartTime, ulong EndTime)
        {
            SortedList<UInt64, Int64> data = new SortedList<ulong, Int64>();
            data.Add(StartTime, IntegerValue);
            return data;
        }

        SortedList<ulong, bool[]> IGeneratorDataFill<bool[]>.InsertValues(ulong StartTime, ulong EndTime)
        {
            SortedList<UInt64, bool[]> data = new SortedList<ulong, bool[]>();
            data.Add(StartTime, DataConvertorUtils.ToBitArray(IntegerValue, 64));
            return data;
        }

        SortedList<ulong, double> IGeneratorDataFill<double>.InsertValues(ulong StartTime, ulong EndTime)
        {
            SortedList<UInt64, Double> data = new SortedList<ulong, Double>();
            data.Add(StartTime, IntegerValue);
            return data;
        }
    }
}