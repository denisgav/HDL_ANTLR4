using System;
using System.Collections.Generic;
using System.Text;

namespace VHDLInputGenerators.Random.Continuous
{
    //генерирует случайные числа простым методом
    public class SimpleContinuous : My_Random_Continuous_Base
    {
        public static string Description =
            "генерирует случайные числа простым методом";

        private System.Random rnd;
        private double diapasone;
        public double Diapasone 
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

        public SimpleContinuous(double Diapasone)
        {
            this.Diapasone = Diapasone;
            rnd = new System.Random();
        }

        public override double NextValue()
        {
            return rnd.NextDouble() * Diapasone;
        }
        public override string ToString()
        {
            return "Random_Continuous : SimpleContinuous";
        }
    }
}
