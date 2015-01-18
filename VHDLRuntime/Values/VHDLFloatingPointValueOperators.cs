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
using System.Linq;
using System.Text;
using VHDLRuntime.Types;

namespace VHDLRuntime.Values
{
    public partial class VHDLFloatingPointValue
    {
        public VHDLFloatingPointValue()
            : base(VHDLFloatingPointType.StandardFloatType)
        {
            floatType = VHDLFloatingPointType.StandardFloatType;
            mValue = 0;
            init();
        }

        public VHDLFloatingPointValue(double value)
            : base(VHDLFloatingPointType.StandardFloatType)
        {
            floatType = VHDLFloatingPointType.StandardFloatType;
            mValue = value;
            init();
        }

        public VHDLFloatingPointValue(VHDLFloatingPointType floatType, double value)
            : base(floatType)
        {
            this.floatType = floatType;
            mValue = (Int64)value;
            init();
        }

        public VHDLFloatingPointValue(VHDLFloatingPointType floatType, int value)
            : base(floatType)
        {
            this.floatType = floatType;
            mValue = value;
            init();
        }


        public VHDLFloatingPointValue(uint value)
            : base(VHDLIntegerType.StandardIntegerType)
        {
            floatType = VHDLFloatingPointType.StandardFloatType;
            mValue = value;
            init();
        }

        public VHDLFloatingPointValue(VHDLFloatingPointType floatType, uint value)
            : base(floatType)
        {
            this.floatType = floatType;
            mValue = value;
            init();
        }

        public VHDLFloatingPointValue(long value)
            : base(VHDLIntegerType.StandardIntegerType)
        {
            floatType = VHDLFloatingPointType.StandardFloatType;
            mValue = value;
            init();
        }

        public VHDLFloatingPointValue(VHDLFloatingPointType floatType, long value)
            : base(floatType)
        {
            this.floatType = floatType;
            mValue = value;
            init();
        }

        public VHDLFloatingPointValue(ulong value)
            : base(VHDLIntegerType.StandardIntegerType)
        {
            floatType = VHDLFloatingPointType.StandardFloatType;
            mValue = value;
            init();
        }

        public VHDLFloatingPointValue(VHDLFloatingPointType floatType, ulong value)
            : base(floatType)
        {
            this.floatType = floatType;
            mValue = value;
            init();
        }

        public VHDLFloatingPointValue(float value)
            : base(VHDLIntegerType.StandardIntegerType)
        {
            floatType = VHDLFloatingPointType.StandardFloatType;
            mValue = value;
            init();
        }

        public VHDLFloatingPointValue(VHDLFloatingPointType floatType, float value)
            : base(floatType)
        {
            this.floatType = floatType;
            mValue = value;
            init();
        }




        public VHDLFloatingPointValue(Decimal value)
            : base(VHDLIntegerType.StandardIntegerType)
        {
            floatType = VHDLFloatingPointType.StandardFloatType;
            mValue = (double)value;
            init();
        }

        public VHDLFloatingPointValue(VHDLFloatingPointType floatType, Decimal value)
            : base(floatType)
        {
            this.floatType = floatType;
            mValue = (double)value;
            init();
        }

        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------


        public static explicit operator VHDLFloatingPointValue(int value)
        {
            return new VHDLFloatingPointValue(value);
        }

        public static explicit operator VHDLFloatingPointValue(uint value)
        {
            return new VHDLFloatingPointValue(value);
        }

        public static explicit operator VHDLFloatingPointValue(long value)
        {
            return new VHDLFloatingPointValue(value);
        }

        public static explicit operator VHDLFloatingPointValue(ulong value)
        {
            return new VHDLFloatingPointValue(value);
        }

        public static explicit operator VHDLFloatingPointValue(float value)
        {
            return new VHDLFloatingPointValue(value);
        }

        public static explicit operator VHDLFloatingPointValue(double value)
        {
            return new VHDLFloatingPointValue(value);
        }

        public static explicit operator VHDLFloatingPointValue(Decimal value)
        {
            return new VHDLFloatingPointValue(value);
        }

        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------

        public static VHDLFloatingPointValue operator -(VHDLFloatingPointValue value)
        {
            return new VHDLFloatingPointValue(-value.mValue);
        }

        public static VHDLFloatingPointValue operator +(VHDLFloatingPointValue value)
        {
            return value;
        }

        public static VHDLFloatingPointValue operator ++(VHDLFloatingPointValue value)
        {
            return new VHDLFloatingPointValue((value.mValue + 1));
        }

        public static VHDLFloatingPointValue operator --(VHDLFloatingPointValue value)
        {
            return new VHDLFloatingPointValue((value.mValue - 1));
        }

        public static VHDLFloatingPointValue operator +(VHDLFloatingPointValue left, VHDLFloatingPointValue right)
        {
            return new VHDLFloatingPointValue((left.mValue + right.mValue));
        }

        public static VHDLFloatingPointValue operator -(VHDLFloatingPointValue left, VHDLFloatingPointValue right)
        {
            return new VHDLFloatingPointValue((left.mValue - right.mValue));
        }

        public static VHDLFloatingPointValue operator *(VHDLFloatingPointValue left, VHDLFloatingPointValue right)
        {
            return new VHDLFloatingPointValue((left.mValue * right.mValue));
        }

        public static VHDLFloatingPointValue operator /(VHDLFloatingPointValue dividend, VHDLFloatingPointValue divisor)
        {
            return new VHDLFloatingPointValue((dividend.mValue / divisor.mValue));
        }

        public static VHDLFloatingPointValue operator %(VHDLFloatingPointValue dividend, VHDLFloatingPointValue divisor)
        {
            return new VHDLFloatingPointValue((dividend.mValue % divisor.mValue));
        }

        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
    }
}
