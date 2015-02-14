using System;
using System.Collections.Generic;
using System.Text;

namespace DataContainer.Generator.Random.Continuous
{
    /* ========================================================================
    * Returns a normal (Gaussian) distributed real number.
    * NOTE: use s > 0.0
    *
    * Uses a very accurate approximation of the normal idf due to Odeh & Evans, 
    * J. Applied Statistics, 1974, vol 23, pp 96-97.
    * ========================================================================
    */
    public class Normal : My_Random_Continuous_Base
    {
        public static string Description =
            "Returns a normal (Gaussian) distributed real number. \n"
            + "NOTE: use s > 0.0 \n"
            + "Uses a very accurate approximation of the normal idf due to Odeh & Evans, \n"
            + "J. Applied Statistics, 1974, vol 23, pp 96-97.";

        private double m;
        public double M
        {
            get
            {
                return m;
            }
            set
            {
                m = value;
            }
        }

        private double s;
        public double S
        {
            get
            {
                return s;
            }
            set
            {
                if (value < 0.0)
                    throw new Exception("Argument exception: s < 0.0");
                else
                    s = value;
            }
        }

        public Normal(double m, double s)
        {
            this.M = m;
            this.S = s;
        }

        public override double NextValue()
        {
            const double p0 = 0.322232431088; const double q0 = 0.099348462606;
            const double p1 = 1.0; const double q1 = 0.588581570495;
            const double p2 = 0.342242088547; const double q2 = 0.531103462366;
            const double p3 = 0.204231210245e-1; const double q3 = 0.103537752850;
            const double p4 = 0.453642210148e-4; const double q4 = 0.385607006340e-2;
            double u, t, p, q, z;

            u = Random();
            if (u < 0.5)
                t = Math.Sqrt(-2.0 * Math.Log(u));
            else
                t = Math.Sqrt(-2.0 * Math.Log(1.0 - u));
            p = p0 + t * (p1 + t * (p2 + t * (p3 + t * p4)));
            q = q0 + t * (q1 + t * (q2 + t * (q3 + t * q4)));
            if (u < 0.5)
                z = (p / q) - t;
            else
                z = t - (p / q);
            return (m + s * z);
        }
        public override string ToString()
        {
            return "Random_Continuous : Normal";
        }
    }
}
