//
//  Copyright (C) 2010-2014  Denis Gavrish
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Text;

namespace VHDLInputGenerators.Random.Discrete
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
