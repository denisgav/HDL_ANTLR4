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
    [Serializable]
    public partial class VHDLIntegerValue : VHDLDiscreteValue, IVHDLIntegerValue        
    {
        private VHDLIntegerType integerType;
        protected virtual VHDLIntegerType IntegerType
        {
            get 
            {
                if (integerType == null)
                    integerType = VHDLIntegerType.StandardIntegerType;
                return integerType; 
            }
        }

        public static IntegerRange IntegerRangeRange
        {
            get
            {
                return VHDLIntegerType.StandardIntegerType.Range;
            }
        }

        protected virtual void init()
        {
            mValue = IntegerType.CorrectValue(this).mValue;
        }

        protected virtual void init(Int64 value)
        {
            mValue = value;
            mValue = IntegerType.CorrectValue(this).mValue;
        }

        private Int64 mValue;
        public Int64 Value
        {
            get { return mValue; }
        }

        

        public override int CompareTo(VHDLScalarValue obj)
        {
            if (obj is VHDLIntegerValue)
            {
                VHDLIntegerValue els = obj as VHDLIntegerValue;
                return mValue.CompareTo(els.mValue);
            }
            throw new ArgumentException("Could not cast {0} to VHDLIntegerValue", obj.GetType().Name);
        }

        public override string ToString()
        {
            return mValue.ToString();
        }

        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
        //5 mod 3 = 2
        //(-5) mod 3 = 1
        //5 mod (-3) = -1
        //(-5) mod (-3) = -2

        //res = a mod n;
        //res = a - (n * int(a/n))

        public VHDLIntegerValue Mod(Int64 value)
        {
            return new VHDLIntegerValue(mValue % value);
        }

        public VHDLIntegerValue Mod(VHDLIntegerValue value)
        {
            return new VHDLIntegerValue(mValue % value.mValue);
        }

        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------

        public VHDLIntegerValue Rem(Int64 value)
        {
            long rem = 0;
            long div = Math.DivRem(mValue, value, out rem);
            return new VHDLIntegerValue(rem);
        }

        public VHDLIntegerValue Rem(VHDLIntegerValue value)
        {
            long rem = 0;
            long div = Math.DivRem(mValue, value.mValue, out rem);
            return new VHDLIntegerValue(rem);
        }

        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------

        public bool LessThan(Int64 value)
        {
            return mValue < value;
        }

        public bool LessEquals(Int64 value)
        {
            return mValue <= value;
        }

        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------

        public bool GreaterThan(Int64 value)
        {
            return mValue > value;
        }

        public bool GreaterEquals(Int64 value)
        {
            return mValue >= value;
        }

        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------

        public bool NotEquals(Int64 value)
        {
            return mValue != value;
        }

        public bool Equals(Int64 value)
        {
            return mValue == value;
        }

        public bool LessThan(VHDLIntegerValue value)
        {
            return mValue < value.mValue;
        }

        public bool LessEquals(VHDLIntegerValue value)
        {
            return mValue <= value.mValue;
        }

        public bool GreaterThan(VHDLIntegerValue value)
        {
            return mValue > value.mValue;
        }

        public bool GreaterEquals(VHDLIntegerValue value)
        {
            return mValue >= value.mValue;
        }

        public bool NotEquals(VHDLIntegerValue value)
        {
            return mValue != value.mValue;
        }

        public bool Equals(VHDLIntegerValue value)
        {
            return mValue == value.mValue;
        }

        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------

        public Int64 Abs()
        {
            return Math.Abs(mValue);
        }

        public Int64 Exp(Int64 value)
        {
            return (Int64)Math.Pow(mValue, value);
        }

        public Int64 Exp(VHDLIntegerValue value)
        {
            return (Int64)Math.Pow(mValue, value.mValue);
        }

        public Int64 Divide(Int64 value)
        {
            return mValue / value;
        }

        public Int64 Multiply(Int64 value)
        {
            return mValue * value;
        }

        public Int64 UnaryMinus()
        {
            return -mValue;
        }

        public Int64 UnaryPlus()
        {
            return mValue;
        }

        public Int64 Plus(Int64 value)
        {
            return mValue + value;
        }

        public Int64 Minus(Int64 value)
        {
            return mValue - value;
        }

        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------

        VHDLIntegerValue IVHDLArithmetic<VHDLIntegerValue>.Abs()
        {
            return new VHDLIntegerValue(Math.Abs(mValue));
        }

        VHDLIntegerValue IVHDLArithmetic<VHDLIntegerValue>.Exp(Int64 value)
        {
            return new VHDLIntegerValue((Int64)Math.Pow(mValue, value));
        }

        VHDLIntegerValue IVHDLArithmetic<VHDLIntegerValue>.Exp(VHDLIntegerValue value)
        {
            return new VHDLIntegerValue((Int64)Math.Pow(mValue, value.mValue));
        }

        public VHDLIntegerValue Divide(VHDLIntegerValue value)
        {
            return new VHDLIntegerValue(mValue / value.mValue);
        }

        public VHDLIntegerValue Multiply(VHDLIntegerValue value)
        {
            return new VHDLIntegerValue(mValue * value.mValue);
        }

        VHDLIntegerValue IVHDLArithmetic<VHDLIntegerValue>.UnaryMinus()
        {
            return new VHDLIntegerValue(-mValue);
        }

        VHDLIntegerValue IVHDLArithmetic<VHDLIntegerValue>.UnaryPlus()
        {
            return new VHDLIntegerValue(mValue);
        }

        public VHDLIntegerValue Plus(VHDLIntegerValue value)
        {
            return new VHDLIntegerValue(mValue + value.mValue);
        }

        public VHDLIntegerValue Minus(VHDLIntegerValue value)
        {
            return new VHDLIntegerValue(mValue - value.mValue);
        }
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
    }
}
