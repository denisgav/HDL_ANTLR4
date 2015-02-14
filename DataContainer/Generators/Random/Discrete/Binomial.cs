using System;
using System.Collections.Generic;
using System.Text;
using DataContainer;

namespace DataContainer.Generator.Random.Discrete
{
    /* ================================================================ 
    * Returns a binomial distributed integer between 0 and n inclusive. 
    * NOTE: use n > 0 and 0.0 < p < 1.0
    * ================================================================
    */
    public class Binomial : My_Random_Discrete_Base
    {
        private Bernoulli bernoulli;

        public static string Description =
            "Returns a binomial distributed integer between 0 and n inclusive. \n"
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
                    bernoulli.P = value;
                }
            }
        }

        public Binomial(long n, double p)
        {
            bernoulli = new Bernoulli(p);
            this.N = n;
            this.P = p;
        }

        public override long NextValue()
        {
            long i, x = 0;

            for (i = 0; i < n; i++)
                x += bernoulli.NextValue();
            return x;
        }
        public override string ToString()
        {
            return "Random_Discrete : Binomial";
        }
        public override StringBuilder StringVhdlRealization(KeyValuePair<string, TimeInterval> param)
        {
            throw new NotImplementedException();
        }
    }
}
