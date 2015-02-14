using System;
using System.Collections.Generic;
using System.Text;

namespace DataContainer.Generator.Random.Discrete
{
    //генерирует случайные числа простым методом
    public class SimpleDiscrete : My_Random_Discrete_Base
    {
        public static string Description =
            "Generates random numbers.";
        private System.Random rnd;
        private int diapasone;
        public int Diapasone 
        {
            get
            {
                return diapasone;
            }
            set
            {
                diapasone = value;
            }
        }

        public SimpleDiscrete(int Diapasone)
        {
            rnd = new System.Random();
            this.Diapasone = Diapasone;
        }

        public override long NextValue()
        {
            return rnd.Next(Diapasone);
        }
        public override string ToString()
        {
            return "Random_Discrete : SimpleDiscrete";
        }
    }
}
