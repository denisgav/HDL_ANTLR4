using System;
using System.Collections.Generic;
using System.Text;
using DataContainer;

namespace DataContainer.Generator.Counters
{
    public class BinaryCounter : Counter
    {
        public override bool[] Next(bool[] value)
        {
            bool[] res = new bool[value.Length];
            value.CopyTo(res, 0);
            int n = value.Length - 1;
            do
            {
                res[n] = !res[n];
            }
            while ((n > 0) && (res[n--] != true));
            return res;
        }

        public override bool[] Next(bool[] value, uint step_count)
        {
            if (step_count == 1)
            {
                return Next(value);
            }
            else
            {
                bool[] res = new bool[value.Length];
                value.CopyTo(res, 0);
                bool[] step = ConvertUIntegerToBoolArray(step_count);


                int len = res.Length - 1;
                int j = step.Length - 1;
                bool carry = false;
                for (int i = len; i >= 0; i--)
                {
                    if (j >= 0)
                    {
                        if ((step[j] == true) && (!carry) || ((step[j] == false) && (carry)))  // carry in
                        {
                            res[i] = !res[i];   // invert bit of result
                        }
                        if ((value[i] == false) && (step[j] == false))
                            carry = false;
                        if ((value[i] == true) && (step[j] == true))
                            carry = true;
                        j--;
                    }
                    else
                    {
                        if (carry)
                        {
                            res[i] = !res[i];   // invert bit of result
                            if (res[i] == true)
                                break;
                        }
                    }
                }
                
                return res;
            }
        }
        public override bool[] Prev(bool[] value)
        {
            bool[] res = new bool[value.Length];
            value.CopyTo(res, 0);
            
            int n = value.Length - 1;
            do
            {
                res[n] = !res[n];
            }
            while ((n > 0) && (res[n--] != false));

            return res;
        }
        public override bool[] Prev(bool[] value, uint step_count)
        {
            if (step_count == 1)
            {
                return Prev(value);
            }
            else
            {
                bool[] res = new bool[value.Length];
                value.CopyTo(res, 0);
                bool[] step = ConvertUIntegerToBoolArray(step_count);

                
                int len = res.Length - 1;
                int j = step.Length - 1;
                bool carry = false;
                for (int i = len; i >= 0; i--)
                {
                    if (j >= 0)
                    {
                        if ((step[j] == true) && (!carry) || ((step[j] == false) && (carry)))  // carry in
                        {
                            res[i] = ! res[i];   // invert bit of result
                        }
                        if ((value[i] == true) && (step[j] == false))
                            carry = false;
                        if ((value[i] == false) && (step[j] == true))
                            carry = true;
                        j--;
                    }
                    else
                    {
                        if (carry)
                        {
                            res[i] = !res[i];   // invert bit of result
                            if (res[i] == false)
                                break;
                        }
                    }
                }
                
                return res;
            }
        }

        public override bool CheckForCorrect(bool[] value)
        {
            return true;
        }

        public override string ToString()
        {
            return "Counter : BinaryCounter";
        }

        public override StringBuilder StringVhdlRealization(KeyValuePair<string, TimeInterval> param)
        {
            StringBuilder res = new StringBuilder();

            res.Append("\n").Append(param.Key).Append(" <= ").Append(param.Key).Append(" + ").Append(stepCount).Append(" after ").Append(timeStep.TimeNumber).Append(" ").Append(timeStep.Unit).Append(" when now < END_TIME ").Append(" ; ").Append("\n");
            //s1<=s1+1 after 10 ns; 
            return res;
        }
    }
}
