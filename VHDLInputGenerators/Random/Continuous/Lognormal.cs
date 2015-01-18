using System;
using System.Collections.Generic;
using System.Text;

namespace VHDLInputGenerators.Random.Continuous
{
    /* ==================================================== 
    * Returns a lognormal distributed positive real number. 
    * NOTE: use b > 0.0
    * ====================================================
    */
    public class Lognormal : My_Random_Continuous_Base
    {
        public static string Description =
            "Returns a lognormal distributed positive real number. \n"
            + "NOTE: use b > 0.0";
        private Normal normal;

        private double a;
        public double A
        {
            get
            {
                return a;
            }
            set
            {
                a = value;
            }
        }

        private double b;
        public  double B
        {
            get
            {
                return b;
            }
            set
            {
                if (value < 0.0)
                    throw new Exception("Argument exception: b < 0.0");
                else
                    b = value;
            }
        }

        public Lognormal(double a, double b)
        {
            this.A = a;
            this.B = b;
            normal = new Normal(0.0, 1.0);
        }

        public override double NextValue()
        {
            return (Math.Exp(a + b * normal.NextValue()));
        }
        public override string ToString()
        {
            return "Random_Continuous : Logonormal";
        }
    }
}
