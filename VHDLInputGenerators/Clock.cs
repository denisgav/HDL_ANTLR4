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
    public class Clock : BaseGenerator, IGeneratorDataFill<bool>
    {
        /// <summary>
        /// Начальное значение
        /// </summary>
        private bool startValue;
        public bool StartValue
        {
            get { return startValue; }
            set { startValue = value; }
        }

        private double dutyCycle;
        public double DutyCycle
        {
            get { return dutyCycle; }
            set 
            {
                if ((value >= 0) && (value <= 100))
                    dutyCycle = value;
                else
                    throw new Exception("DutyCycle must be in range between 0 and 100");
            }
        }

        public override string ToString()
        {
            return "Clock";
        }
        public override string GetStringStartValue()
        {
            return "\'" + DataConvertorUtils.BoolToInt(startValue) + "\'";
        }
        public override StringBuilder StringVhdlRealization(KeyValuePair<string, TimeInterval> param)
        {
            StringBuilder res = new StringBuilder();
            if (dutyCycle == 50)
            {
                res.Append(param.Key).Append("<=not ").Append(param.Key).Append(" after ").Append((ulong)timeStep.GetTimeUnitInFS()/2).Append(" ").Append(TimeUnit.fs).Append(" when now < END_TIME;");
            }
            else
            {
                res.Append("process is").Append("\n").Append("begin").Append("\n").Append(" if now<END_TIME then").Append("\n").Append(param.Key).Append("<=").Append("\'").Append(DataConvertorUtils.BoolToInt(startValue)).Append("\';").Append("\n");
                res.Append(" wait for ").Append((ulong)(timeStep.GetTimeUnitInFS() * (dutyCycle / 100))).Append(" ").Append(TimeUnit.fs).Append(" ;").Append("\n");
                res.Append(param.Key).Append("<=").Append("\'").Append(DataConvertorUtils.BoolToInt(!startValue)).Append("\';").Append("\n");
                res.Append(" wait for ").Append((ulong)(timeStep.GetTimeUnitInFS() * (1 - dutyCycle / 100))).Append(" ").Append(TimeUnit.fs).Append(" ;").Append("\n");
                res.Append(" else wait;").Append("\n").Append(" end if;").Append("\n").Append("end process; ").Append("\n"); 

            }
            return res;
        }
        public override void StreamVhdlRealization(KeyValuePair<string, TimeInterval> param, System.IO.StreamWriter sw)
        {
            sw.Write(StringVhdlRealization(param).ToString());
        }

        public IEnumerable<bool> GetBitEnumerator()
        {
            bool value = startValue;
            while (true)
            {
                yield return value;
                value = !value;
            }
        }

        SortedList<UInt64, bool> IGeneratorDataFill<bool>.InsertValues(UInt64 StartTime, UInt64 EndTime)
        {
            SortedList<UInt64, bool> data = new SortedList<UInt64, bool>();
            UInt64 time = StartTime;

            int counter = 0;
            foreach (var b in GetBitEnumerator())
            {
                if (counter % 2 == 0)
                {
                    data.Add(time, b);
                    time += (UInt64)((dutyCycle / 100.0) * (double)timeStep.GetTimeUnitInFS());
                }
                else
                {
                    data.Add(time, b);
                    time += (UInt64)((1.0 - dutyCycle / 100.0) * (double)timeStep.GetTimeUnitInFS());
                }

                counter++;

                if (time > EndTime)
                    break;
            }
            return data;
        }
    }
}
