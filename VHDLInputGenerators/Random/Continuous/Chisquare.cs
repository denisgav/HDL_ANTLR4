using System;
using System.Collections.Generic;
using System.Text;

namespace VHDLInputGenerators.Random.Continuous
{
    /* =====================================================
    * Returns a chi-square distributed positive real number. 
    * NOTE: use n > 0
    * =====================================================
    */
    public class Chisquare : My_Random_Continuous_Base
    {
        private Normal normal;

        public static string Description = 
            "Returns a chi-square distributed positive real number. \n"
            + "NOTE: use n > 0";

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
                    throw new Exception("Argument exception: n < 0");
                else
                    n = value;
            }
        }

        public Chisquare(long n)
        {
            this.N = n;
            normal = new Normal(0.0, 1.0);
        }

        public override double NextValue()
        {
            long i;
            double z, x = 0.0;

            for (i = 0; i < n; i++)
            {
                z = normal.NextValue();
                x += z * z;
            }
            return x;
        }
        public override string ToString()
        {
            return "Random_Continuous : Chisquare";
        }
    }
}
