using System;
using System.Collections.Generic;
using System.Text;

namespace DataContainer.Generator.Random.Discrete
{
    /* ================================================== 
    * Returns a Poisson distributed non-negative integer. 
    * NOTE: use m > 0
    * ==================================================
    */
    public class Poisson : My_Random_Discrete_Base
    {
        public static string Description =
            "Returns a Poisson distributed non-negative integer.\n"
            + "NOTE: use m > 0";
        private double m;
        public double M
        {
            get
            {
                return m;
            }
            set
            {
                if (value < 0)
                    throw new Exception("Argument exception: m < 0");
                else
                    m = value;
            }
        }

        public Poisson(double m)
        {
            this.M = m;
        }

        public override long NextValue()
        {
            double t = 0.0;
            long x = 0;

            while (t < m)
            {
                t += (-1.0 * Math.Log(1.0 - Random()));
                x++;
            }
            return (x - 1);
        }
        public override string ToString()
        {
            return "Random_Discrete : Poisson";
        }
    }
}
