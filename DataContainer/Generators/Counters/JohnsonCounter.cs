using System;
using System.Collections.Generic;
using System.Text;
using DataContainer;

namespace DataContainer.Generator.Counters
{
    public class JohnsonCounter : Counter
    {
        private int IndexOfStartElem(bool[] value)
        {
            bool start = value[0];
            int index = StartIndexOf(value, !start);
            if (index == -1)
                return value.Length;
            else
                return index;
        }

        private int IndexOfLastElem(bool[] value)
        {
            bool start = value[value.Length-1];
            
            int index = LastIndexOf(value, !start);
            if (index == -1)
                return value.Length-1;
            else
                return index;
        }

        public override bool[] Next(bool[] value)
        {
            bool[] res = new bool[value.Length];
            value.CopyTo(res, 0);

            int length = value.Length;
            int index = IndexOfStartElem(value);
            index = (index + length - 1) % length;
            res[index] = !value[0];
            return res;
        }

        public override bool[] Prev(bool[] value)
        {
            bool[] res = new bool[value.Length];
            value.CopyTo(res, 0);

            int length = value.Length;
            int index = IndexOfLastElem(value);
            index = (index + 1) % length;
            res[index] = !res[res.Length - 1];
            return res;
        }

        public override bool CheckForCorrect(bool[] value)
        {
            int index = 0;
            bool cur_value = value[0];
            for (index = 0; index < value.Length; index++)
                if (value[index] != cur_value)
                    break;
            if (index == value.Length)
                return true;

            cur_value = value[index];
            for (; index < value.Length; index++)
                if (value[index] != cur_value)
                    return false;
            return true;
        }
        public override string ToString()
        {
            return "Counter : JohnsonCounter";
        }
        public override StringBuilder StringVhdlRealization(KeyValuePair<string, TimeInterval> param)
        {
            StringBuilder res = new StringBuilder();

            res.Append("\n").Append(param.Key).Append(" <= ").Append(param.Key).Append("(").Append(param.Key).Append("\'left - ").Append(stepCount).Append(" downto ").Append(param.Key).Append("\'right)&(not ").Append(param.Key).Append("(").Append(param.Key).Append("\'left ))").Append(" after ").Append(timeStep.TimeNumber).Append(" ").Append(timeStep.Unit).Append(" when now < END_TIME ").Append(" ; ").Append("\n");
            //s1<=s1(s1'left-1 downto s1'right)&(not s1(s1'left)) after 10 ns;  
            return res;
        }
    }
}
