using System;
using System.Collections.Generic;
using System.Text;
using DataContainer;
using DataContainer.ValueDump;

namespace DataContainer.Generator.Random.Continuous
{
    public abstract class My_Random_Continuous_Base : My_Random_Base
    {
        public abstract double NextValue();

        public override IEnumerable<double> GetDoubleEnumerator()
        {
            while (true)
                yield return NextValue();
        }

        public override IEnumerable<long> GetIntegerEnumerator()
        {
            while (true)
                yield return (long)NextValue();
        }

        public override IEnumerable<bool[]> GetBitArrayEnumerator()
        {
            while (true)
                yield return DataConvertorUtils.ToBitArray((int)NextValue(), SizeInBits);
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
