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
