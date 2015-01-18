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
using VHDLRuntime;

namespace VHDLInputGenerators.Random.Discrete
{
    /* ========================================================
    * Returns 1 with probability p or 0 with probability 1 - p. 
    * NOTE: use 0.0 < p < 1.0                                   
    * ========================================================
    */
    public class Bernoulli : My_Random_Discrete_Base
    {
        public static string Description =
            "Returns 1 with probability p or 0 with probability 1 - p.  \n"
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

        public Bernoulli(double p)
        {
            this.P = p;
        }

        public override long NextValue()
        {
            return ((Random() < (1.0 - p)) ? 0 : 1);
        }
        public override string ToString()
        {
            return "Random_Discrete : Bernoulli";
        }
        public override StringBuilder StringVhdlRealization(KeyValuePair<string, TimeInterval> param)
        {
            throw new NotImplementedException();
        }
    }
}
