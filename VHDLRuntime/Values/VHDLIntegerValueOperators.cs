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
using VHDLRuntime.Range;

namespace VHDLRuntime.Values
{
    public partial class VHDLIntegerValue
    {
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------

        public static VHDLIntegerValue operator &(VHDLIntegerValue left, VHDLIntegerValue right)
        {
            return new VHDLIntegerValue((left.mValue & right.mValue));
        }

        public static VHDLIntegerValue operator |(VHDLIntegerValue left, VHDLIntegerValue right)
        {
            return new VHDLIntegerValue((left.mValue | right.mValue));
        }

        public static VHDLIntegerValue operator ^(VHDLIntegerValue left, VHDLIntegerValue right)
        {
            return new VHDLIntegerValue((left.mValue ^ right.mValue));
        }

        public static VHDLIntegerValue operator <<(VHDLIntegerValue value, int shift)
        {
            return new VHDLIntegerValue((value.mValue << shift));
        }

        public static VHDLIntegerValue operator >>(VHDLIntegerValue value, int shift)
        {
            return new VHDLIntegerValue((value.mValue >> shift));
        }

        public static VHDLIntegerValue operator ~(VHDLIntegerValue value)
        {
            return new VHDLIntegerValue(~value.mValue);
        }

        public static VHDLIntegerValue operator -(VHDLIntegerValue value)
        {
            return new VHDLIntegerValue(-value.mValue);
        }

        public static VHDLIntegerValue operator +(VHDLIntegerValue value)
        {
            return value;
        }

        public static VHDLIntegerValue operator ++(VHDLIntegerValue value)
        {
            return new VHDLIntegerValue((value.mValue + 1));
        }

        public static VHDLIntegerValue operator --(VHDLIntegerValue value)
        {
            return new VHDLIntegerValue((value.mValue - 1));
        }

        public static VHDLIntegerValue operator +(VHDLIntegerValue left, VHDLIntegerValue right)
        {
            return new VHDLIntegerValue((left.mValue + right.mValue));
        }

        public static VHDLIntegerValue operator -(VHDLIntegerValue left, VHDLIntegerValue right)
        {
            return new VHDLIntegerValue((left.mValue - right.mValue));
        }

        public static VHDLIntegerValue operator *(VHDLIntegerValue left, VHDLIntegerValue right)
        {
            return new VHDLIntegerValue((left.mValue * right.mValue));
        }

        public static VHDLIntegerValue operator /(VHDLIntegerValue dividend, VHDLIntegerValue divisor)
        {
            return new VHDLIntegerValue((dividend.mValue / divisor.mValue));
        }

        public static VHDLIntegerValue operator %(VHDLIntegerValue dividend, VHDLIntegerValue divisor)
        {
            return new VHDLIntegerValue((dividend.mValue % divisor.mValue));
        }

        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------

        public static explicit operator VHDLIntegerValue(int value)
        {
            return new VHDLIntegerValue(value);
        }

        public static explicit operator VHDLIntegerValue(uint value)
        {
            return new VHDLIntegerValue(value);
        }

        public static explicit operator VHDLIntegerValue(long value)
        {
            return new VHDLIntegerValue(value);
        }

        public static explicit operator VHDLIntegerValue(ulong value)
        {
            return new VHDLIntegerValue(value);
        }

        public static explicit operator VHDLIntegerValue(float value)
        {
            return new VHDLIntegerValue(value);
        }

        public static explicit operator VHDLIntegerValue(double value)
        {
            return new VHDLIntegerValue(value);
        }

        public static explicit operator VHDLIntegerValue(Decimal value)
        {
            return new VHDLIntegerValue(value);
        }

        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------

        public VHDLIntegerValue()
            : base(VHDLIntegerType.StandardIntegerType)
        {
            integerType = VHDLIntegerType.StandardIntegerType;
            mValue = 0;
            init();
        }

        public VHDLIntegerValue(int value)
            : base(VHDLIntegerType.StandardIntegerType)
        {
            integerType = VHDLIntegerType.StandardIntegerType;
            mValue = value;
            init();
        }

        public VHDLIntegerValue(VHDLIntegerValue value)
            : base(VHDLIntegerType.StandardIntegerType)
        {
            integerType = VHDLIntegerType.StandardIntegerType;
            mValue = value.mValue;
            init();
        }

        public VHDLIntegerValue(VHDLIntegerType integerType, int value)
            : base(integerType)
        {
            this.integerType = integerType;
            mValue = value;
            init();
        }


        public VHDLIntegerValue(uint value)
            : base(VHDLIntegerType.StandardIntegerType)
        {
            integerType = VHDLIntegerType.StandardIntegerType;
            mValue = value;
            init();
        }

        public VHDLIntegerValue(VHDLIntegerType integerType, uint value)
            : base(integerType)
        {
            this.integerType = integerType;
            mValue = value;
            init();
        }

        public VHDLIntegerValue(long value)
            : base(VHDLIntegerType.StandardIntegerType)
        {
            integerType = VHDLIntegerType.StandardIntegerType;
            mValue = value;
            init();
        }

        public VHDLIntegerValue(VHDLIntegerType integerType, long value)
            : base(integerType)
        {
            this.integerType = integerType;
            mValue = value;
            init();
        }

        public VHDLIntegerValue(ulong value)
            : base(VHDLIntegerType.StandardIntegerType)
        {
            integerType = VHDLIntegerType.StandardIntegerType;
            mValue = (Int64)value;
            init();
        }

        public VHDLIntegerValue(VHDLIntegerType integerType, ulong value)
            : base(integerType)
        {
            this.integerType = integerType;
            mValue = (Int64)value;
            init();
        }

        public VHDLIntegerValue(float value)
            : base(VHDLIntegerType.StandardIntegerType)
        {
            integerType = VHDLIntegerType.StandardIntegerType;
            mValue = (Int64)value;
            init();
        }

        public VHDLIntegerValue(VHDLIntegerType integerType, float value)
            : base(integerType)
        {
            this.integerType = integerType;
            mValue = (Int64)value;
            init();
        }

        public VHDLIntegerValue(double value)
            : base(VHDLIntegerType.StandardIntegerType)
        {
            integerType = VHDLIntegerType.StandardIntegerType;
            mValue = (Int64)value;
            init();
        }

        public VHDLIntegerValue(VHDLIntegerType integerType, double value)
            : base(integerType)
        {
            this.integerType = integerType;
            mValue = (Int64)value;
            init();
        }

        public VHDLIntegerValue(Decimal value)
            : base(VHDLIntegerType.StandardIntegerType)
        {
            integerType = VHDLIntegerType.StandardIntegerType;
            mValue = (Int64)value;
            init();
        }

        public VHDLIntegerValue(VHDLIntegerType integerType, Decimal value)
            : base(integerType)
        {
            this.integerType = integerType;
            mValue = (Int64)value;
            init();
        }

        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
        
    }
}
