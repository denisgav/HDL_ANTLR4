using System;
using System.Collections.Generic;
using System.Text;

namespace DataContainer.Generator.Random.Discrete
{
    /* ================================================= 
     * Returns a Pascal distributed non-negative integer. 
     * NOTE: use n > 0 and 0.0 < p < 1.0
     * =================================================
     */
    public class Pascal : My_Random_Discrete_Base
    {
        private Geometric geometric;

        public static string Description =
            "Returns a Pascal distributed non-negative integer.\n"
            + "NOTE: use n > 0 and 0.0 < p < 1.0";

        private long n;
        public long N
        {
            get
            {
                return n;
            }
            set
            {
                if (n < 0)
                    throw new Exception("Argument n must be  n > 0");
                else
                    n = value;
            }
        }
        private double p;
        public double P
        {
            get
            {
                return p;
            }
            set
            {
                if ((value < 0.0) || (value > 1.0))
                    throw new Exception("Argument p must be  0.0 < p < 1.0");
                else
                {
                    p = value;
                    geometric.P = value;
                }
            }
        }

        public Pascal(long n, double p)
        {
            geometric = new Geometric(p);
            this.N = n;
            this.P = p;
        }

        public override long NextValue()
        {
            long i, x = 0;

            for (i = 0; i < n; i++)
                x += geometric.NextValue();
            return x;
        }
        public override string ToString()
        {
            return "Random_Discrete : Pascal";
        }
    }
}
