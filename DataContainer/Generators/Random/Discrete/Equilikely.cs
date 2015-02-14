using System;
using System.Collections.Generic;
using System.Text;
using DataContainer;

namespace DataContainer.Generator.Random.Discrete
{
    /* ===================================================================
    * Returns an equilikely distributed integer between a and b inclusive. 
    * NOTE: use a < b
    * ===================================================================
    */
    public class Equilikely : My_Random_Discrete_Base
    {
        public static string Description =
            "Returns an equilikely distributed integer between a and b inclusive. \n"
            + "NOTE: use a < b";
        private long a;
        public long A
        {
            get
            {
                return a;
            }
            set
            {
                if (value < b)
                    throw new Exception("Argument exception: a > b");
                else
                    a = value;
            }
        }

        private long b;
        public long B
        {
            get
            {
                return b;
            }
            set
            {
                if (a > value)
                    throw new Exception("Argument exception: a > b");
                else
                    b = value;
            }
        }

        public Equilikely(long a, long b)
        {
            this.A = a;
            this.B = b;
        }

        public override long NextValue()
        {
            return (a + (long)((b - a + 1) * Random()));
        }
        public override string ToString()
        {
            return "Random_Discrete : Equilikely";
        }
        public override StringBuilder StringVhdlRealization(KeyValuePair<string, TimeInterval> param)
        {
            throw new NotImplementedException();
        }
    }
}
