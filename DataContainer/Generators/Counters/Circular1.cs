using System;
using System.Collections.Generic;
using System.Text;
using DataContainer;

namespace DataContainer.Generator.Counters
{
    public class Circular1 : Counter
    {
        public override bool[] Next(bool[] value)
        {
            bool[] res = new bool[value.Length];
            value.CopyTo(res, 0);

            int index = StartIndexOf(value, true);
            if (index < 0)
                index = 0;

            res[index] = false;
            index = (index + value.Length - 1) % value.Length;
            res[index] = true;
            return res;
        }

        public override bool[] Next(bool[] value, uint step_count)
        {
            if (step_count == 1)
            {
                return this.Next(value);
            }
            else
            {
                step_count %= (uint)value.Length;
                bool[] res = new bool[value.Length];
                value.CopyTo(res, 0);

                int index = StartIndexOf(value, true);
                if (index == -1)
                    index = 0;

                res[index] = false;
                index = (index + value.Length - (int)step_count) % value.Length;
                res[index] = true;
                return res;
            }
        }

        public override bool[] Prev(bool[] value)
        {
            bool[] res = new bool[value.Length];
            value.CopyTo(res, 0);

            int index = StartIndexOf(value, true);
            if (index < 0)
                index = value.Length - 1;

            res[index] = false;
            index = (index + 1) % (value.Length);
            res[index] = true;
            return res;
        }

        public override bool[] Prev(bool[] value, uint step_count)
        {
            if (step_count == 1)
            {
                return this.Prev(value);
            }
            else
            {
                step_count %= (uint)value.Length;
                bool[] res = new bool[value.Length];
                value.CopyTo(res, 0);

                int index = StartIndexOf(value, true);
                if (index < 0)
                    index = value.Length - 1;
                res[index] = false;
                index = (index + (int)step_count) % (value.Length);
                res[index] = true;
                return res;
            }
        }

        public override bool CheckForCorrect(bool[] value)
        {
            int zerocount = 0;
            int onecount = 0;
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == false)
                    zerocount++;
                else
                    onecount++;
            }
            if (onecount == 1)
                return true;
            else
                return false;
        }
        public override string ToString()
        {
            return "Counter : Circular1";
        }

        public override StringBuilder StringVhdlRealization(KeyValuePair<string, TimeInterval> param)
        {
            StringBuilder res = new StringBuilder();

            res.Append("\n").Append(param.Key).Append(" <= ").Append(param.Key).Append("(").Append(param.Key).Append("\'right )&").Append(param.Key).Append("(").Append(param.Key).Append("\'left downto").Append(param.Key).Append("\'right - ").Append(stepCount).Append(")").Append(" after ").Append(timeStep.TimeNumber).Append(" ").Append(timeStep.Unit).Append(" when now < END_TIME ").Append(" ; ").Append("\n");
            //      s1<=s1(s1'right)&s1(s1'left downto s1'right-1) after 10 ns; 
            return res;
        }

    }
}
