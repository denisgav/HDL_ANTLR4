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
using VHDLRuntime.Range;
using VHDLRuntime.Types;
using VHDLRuntime.Values;

namespace VHDLRuntime.Values.BuiltIn
{
    [Serializable]
    public class NATURAL : INTEGER
    {
        private static IntegerRange integerRange2;
        private static VHDLIntegerType integerType2;

        public override IntegerRange IntegerRange
        {
            get
            {
                if (integerRange2 == null)
                {
                    integerRange2 =
                      new IntegerRange(
                          new VHDLIntegerValue(0),
                          new VHDLIntegerValue(2147483647),
                          RangeDirection.To);
                }
                return integerRange2;
            }
        }

        protected override VHDLIntegerType IntegerType
        {
            get
            {
                if (integerType2 == null)
                    integerType2 = new VHDLIntegerType(this.IntegerRange);
                return integerType2;
            }
        }

        public NATURAL(long i)
            : base(i)
        {
        }

        public NATURAL(VHDLIntegerValue i)
            : base(i)
        {
        }

        public NATURAL()
        {
        }
    }
}