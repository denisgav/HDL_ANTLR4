using System;
using System.Collections.Generic;
using System.Text;
using DataContainer;

namespace DataContainer.Generator.Random.Discrete
{
    /* ========================================================
    * Returns 1 with probability p or 0 with probability 1 - p. 
    * NOTE: use 0.0 < p < 1.0                                   
    * ========================================================
    */
    public class Bernoulli : My_Random_Discrete_Base
    {
        public static string Description =
            "Returns 1 with probability p or 0 with probability 1 - p.  \n"
            + "NOTE: use 0.0 < p < 1.0";

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
                    p = value;
            }
        }

        public Bernoulli(double p)
        {
            this.P = p;
        }

        public override long NextValue()
        {
            return ((Random() < (1.0 - p)) ? 0 : 1);
        }
        public override string ToString()
        {
            return "Random_Discrete : Bernoulli";
        }
        public override StringBuilder StringVhdlRealization(KeyValuePair<string, TimeInterval> param)
        {
            throw new NotImplementedException();
        }
    }
}
