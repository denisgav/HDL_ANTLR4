using System;
using System.Collections.Generic;
using System.Text;
using DataContainer;

namespace DataContainer.Generator.Random.Discrete
{
    /* ====================================================
     * Returns a geometric distributed non-negative integer.
     * NOTE: use 0.0 < p < 1.0
     * ====================================================
     */
    public class Geometric : My_Random_Discrete_Base
    {
        public static string Description =
            "Returns a geometric distributed non-negative integer.\n"
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

        public Geometric(double p)
        {
            this.P = p;
        }

        public override long NextValue()
        {
            return ((long)(Math.Log(1.0 - Random()) / Math.Log(p)));
        }
        public override string ToString()
        {
            return "Random_Discrete : Chisquare";
        }
        public override StringBuilder StringVhdlRealization(KeyValuePair<string, TimeInterval> param)
        {
            throw new NotImplementedException();
        }
    }
}
