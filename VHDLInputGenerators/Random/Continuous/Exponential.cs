using System;
using System.Collections.Generic;
using System.Text;

namespace VHDLInputGenerators.Random.Continuous
{
    /* =========================================================
    * Returns an exponentially distributed positive real number. 
    * NOTE: use m > 0.0
    * =========================================================
    */
    public class Exponential : My_Random_Continuous_Base
    {
        public static string Description =
            "Returns an exponentially distributed positive real number. \n"
            + "NOTE: use m > 0.0";

        private double m;
        public double M
        {
            get
            {
                return m;
            }
            set
            {
                if (value < 0.0)
                    throw new Exception("Argument exception: m < 0.0");
                else
                    m = value;
            }
        }

        public Exponential(double m)
        {
            this.M = m;
        }

        public override double NextValue()
        {
            return (-m * Math.Log(1.0 - Random()));
        }
        public override string ToString()
        {
            return "Random_Continuous : Exponential";
        }
    }
}
