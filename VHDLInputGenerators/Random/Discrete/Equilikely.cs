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
