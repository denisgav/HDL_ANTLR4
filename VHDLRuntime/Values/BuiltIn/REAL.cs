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
    public class REAL : VHDLFloatingPointValue
    {
        private static FloatingPointRange floatingPointRange;
        private static VHDLFloatingPointType floatingPointType;

        public static FloatingPointRange RangeBase
        {
            get
            {
                if (floatingPointRange == null)
                {
                    floatingPointRange =
                        new FloatingPointRange(
                            new VHDLFloatingPointValue(-1.7976931348623157000000000000000000000000E+308),
                            new VHDLFloatingPointValue(1.7976931348623157000000000000000000000000E+308),
                            RangeDirection.To);
                }
                return floatingPointRange;
            }
        }

        protected override VHDLFloatingPointType FloatType
        {
            get
            {
                if (floatingPointType == null)
                {
                    floatingPointType = new VHDLFloatingPointType(RangeBase);
                }
                return floatingPointType;
            }
        }

        public REAL(double i)
        {
            this.init(i);
        }

        public REAL(VHDLFloatingPointValue i)
        {
            this.init(i.Value);
        }

        public REAL()
        {
            this.init(0.0);
        }
    }
}
