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
    public class INTEGER : VHDLIntegerValue
    {
        protected static IntegerRange integerRange;
        protected static VHDLIntegerType integerType;

        public virtual IntegerRange IntegerRange
        {
            get
            {
                if (integerRange == null)
                {
                    integerRange =
                      new IntegerRange(
                          new VHDLIntegerValue(-2147483648),
                          new VHDLIntegerValue(2147483647),
                          RangeDirection.To);
                }
                return integerRange;
            }
        }


        protected override VHDLIntegerType IntegerType
        {
            get
            {
                if (integerType == null)
                    integerType = new VHDLIntegerType(this.IntegerRange);
                return integerType;
            }
        }

        public INTEGER(long i)
        {
            this.init(i);
        }

        public INTEGER(VHDLIntegerValue i)
        {
            this.init(i.Value);
        }

        public INTEGER()
        {
            this.init(0L);
        }
    }
}