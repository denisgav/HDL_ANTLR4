using System;
using System.Collections.Generic;
using System.Text;

namespace VHDLInputGenerators.Random.Continuous
{
    /* =========================================== 
    * Returns a student-t distributed real number.
    * NOTE: use n > 0
    * ===========================================
    */
    public class Student : My_Random_Continuous_Base
    {
        public static string Description =
            "Returns a student-t distributed real number. \n"
            + "NOTE: use n > 0";
        private Normal normal;
        private Chisquare chisquare;

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
                {
                    n = value;
                    chisquare.N = value;
                }
            }
        }

        public Student(long n)
        {
            normal = new Normal(0.0, 1.0);
            chisquare = new Chisquare(n);
            this.N = n;
        }

        public override double NextValue()
        {
            return (normal.NextValue() / Math.Sqrt(chisquare.NextValue() / n));
        }
        public override string ToString()
        {
            return "Random_Continuous : Student";
        }
    }
}
