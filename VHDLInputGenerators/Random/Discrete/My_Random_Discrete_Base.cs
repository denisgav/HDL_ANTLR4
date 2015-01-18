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

namespace VHDLInputGenerators.Random.Discrete
{
    public abstract class My_Random_Discrete_Base : My_Random_Base
    {
        public abstract long NextValue();

        public override IEnumerable<double> GetDoubleEnumerator()
        {
            while (true)
                yield return NextValue();
        }

        public override IEnumerable<long> GetIntegerEnumerator()
        {
            while (true)
                yield return NextValue();
        }

        public override IEnumerable<bool[]> GetBitArrayEnumerator()
        {
            while (true)
                yield return DataConvertorUtils.ToBitArray(NextValue(), SizeInBits);
        }
       
        public override StringBuilder StringVhdlRealization(KeyValuePair<string, TimeInterval> param)
        {
            int numbRepeats = (int)(param.Value.GetTimeUnitInFS() / this.timeStep.GetTimeUnitInFS()); //число повторений
            StringBuilder res = new StringBuilder();
            res.Append("--random generator\n");
            res.Append("process is \n begin\n");
            res.Append(param.Key).Append(" <= ").Append(DataConvertorUtils.ToBitArrayString((int)this.NextValue(), SizeVector)).Append(";\n");
            for (int i = 0; i < numbRepeats; ++i)
            {
                res.Append("wait for ").Append(timeStep.TimeNumber).Append(" ").Append(timeStep.Unit).Append(";\n");
                res.Append(param.Key).Append(" <= ").Append(DataConvertorUtils.ToBitArrayString((int)this.NextValue(), SizeVector)).Append(";\n");

            }
            res.Append("wait;\n");
            res.Append("end process;\n");
            return res;
        }

        public override void StreamVhdlRealization(KeyValuePair<string, TimeInterval> param, System.IO.StreamWriter sw)
        {
            int numbRepeats = (int)(param.Value.GetTimeUnitInFS() / this.timeStep.GetTimeUnitInFS()); //число повторений
            sw.Write("--random generator\n");
            sw.Write("process is \n begin\n");
            sw.Write(param.Key);
            sw.Write(" <= ");
            sw.Write(DataConvertorUtils.ToBitArrayString((int)this.NextValue(), SizeVector));
            sw.Write(";\n");
            StringBuilder temp = new StringBuilder();
            for (int i = 0; i < numbRepeats; )
            {
                temp = new StringBuilder();
                for (int j = i % 1000; (j < 1000 - 1) && (j < numbRepeats); ++j, ++i)
                {
                    temp.Append("wait for ");
                    temp.Append(timeStep.TimeNumber);
                    temp.Append(" ");
                    temp.Append(timeStep.Unit);
                    temp.Append(";\n");
                    temp.Append(param.Key);
                    temp.Append(" <= ");
                    temp.Append(DataConvertorUtils.ToBitArrayString((int)this.NextValue(), SizeVector));
                    temp.Append(";\n");
                }
                sw.Write(temp);
                ++i;
            }
            sw.Write("wait;\n");
            sw.Write("end process;\n");
        }
    }
}
