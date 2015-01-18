using System;
using System.Collections.Generic;
using System.Text;

namespace VHDLInputGenerators.Random.Continuous
{
    /* ================================================== 
    * Returns an Erlang distributed positive real number.
    * NOTE: use n > 0 and b > 0.0
    * ==================================================
    */
    public class Erlang : My_Random_Continuous_Base
    {
        private Exponential exponential;

        public static string Description =
            "Returns an Erlang distributed positive real number. \n"
            + "NOTE: use n > 0 and b > 0.0";

        private long n;
        public long N
        {
            get
            {
                return n;
            }
            set
            {
                if (value < 0)
                    throw new Exception("Argument exception: use n > 0");
                else
                    n = value;
            }
        }

        private double b;
        public double B
        {
            get
            {
                return b;
            }
            set
            {
                if (value < 0.0)
                    throw new Exception("Argument exception: use b > 0.0");
                else
                {
                    b = value;
                    exponential.M = value;
                }
            }
        }

        public Erlang(long n, double b)
        {
            exponential = new Exponential(b);
            this.N = n;
            this.B = b;
        }

        public override double NextValue()
        {
            long i;
            double x = 0.0;

            for (i = 0; i < n; i++)
                x += exponential.NextValue();

            return x;
        }
        public override string ToString()
        {
            return "Random_Continuous : Erlang";
        }
    }
}
