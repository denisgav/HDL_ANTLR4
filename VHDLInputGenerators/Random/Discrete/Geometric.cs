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
    /* ====================================================
     * Returns a geometric distributed non-negative integer.
     * NOTE: use 0.0 < p < 1.0
     * ====================================================
     */
    public class Geometric : My_Random_Discrete_Base
    {
        public static string Description =
            "Returns a geometric distributed non-negative integer.\n"
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

        public Geometric(double p)
        {
            this.P = p;
        }

        public override long NextValue()
        {
            return ((long)(Math.Log(1.0 - Random()) / Math.Log(p)));
        }
        public override string ToString()
        {
            return "Random_Discrete : Chisquare";
        }
        public override StringBuilder StringVhdlRealization(KeyValuePair<string, TimeInterval> param)
        {
            throw new NotImplementedException();
        }
    }
}
