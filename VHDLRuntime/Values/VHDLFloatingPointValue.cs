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
    [Serializable]
    public partial class VHDLFloatingPointValue : VHDLScalarValue, IVHDLFloatingPointValue
    {
        private VHDLFloatingPointType floatType;
        protected virtual VHDLFloatingPointType FloatType
        {
            get 
            {
                if (floatType == null)
                    floatType = VHDLFloatingPointType.StandardFloatType;
                return floatType; 
            }
        }


        protected virtual void init()
        {
            mValue = FloatType.CorrectValue(this).mValue;
        }

        protected virtual void init(double value)
        {
            mValue = value;
            mValue = FloatType.CorrectValue(this).mValue;
        }

        private double mValue;
        public double Value
        {
            get { return mValue; }
        }

        public override int CompareTo(VHDLScalarValue obj)
        {
            if (obj is VHDLFloatingPointValue)
            {
                VHDLFloatingPointValue els = obj as VHDLFloatingPointValue;
                return mValue.CompareTo(els.mValue);
            }
            throw new ArgumentException("Could not cast {0} to VHDLFloatingPointValue", obj.GetType().Name);
        }

        public override string ToString()
        {
            return mValue.ToString();
        }

        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------



        public bool LessThan(double value)
        {
            return mValue < value;
        }

        public bool LessEquals(double value)
        {
            return mValue <= value;
        }

        public bool GreaterThan(double value)
        {
            throw new NotImplementedException();
        }

        public bool GreaterEquals(double value)
        {
            return mValue > value;
        }

        public bool NotEquals(double value)
        {
            return mValue != value;
        }

        public bool Equals(double value)
        {
            return mValue == value;
        }

        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------

        public bool LessThan(VHDLFloatingPointValue value)
        {
            return mValue < value.mValue;
        }

        public bool LessEquals(VHDLFloatingPointValue value)
        {
            return mValue <= value.mValue;
        }

        public bool GreaterThan(VHDLFloatingPointValue value)
        {
            return mValue > value.mValue;
        }

        public bool GreaterEquals(VHDLFloatingPointValue value)
        {
            return mValue >= value.mValue;
        }

        public bool NotEquals(VHDLFloatingPointValue value)
        {
            return mValue != value.mValue;
        }

        public bool Equals(VHDLFloatingPointValue value)
        {
            return mValue == value.mValue;
        }

        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------

        public double Abs()
        {
            return Math.Abs(mValue);
        }

        public double Exp(Int64 value)
        {
            return Math.Pow(mValue, value);
        }

        public double Exp(VHDLIntegerValue value)
        {
            return Math.Pow(mValue, value.Value);
        }

        public double Divide(double value)
        {
            return mValue / value;
        }

        public double Multiply(double value)
        {
            return mValue * value;
        }

        public double UnaryMinus()
        {
            return -mValue;
        }

        public double UnaryPlus()
        {
            return mValue;
        }

        public double Plus(double value)
        {
            return mValue + value;
        }

        public double Minus(double value)
        {
            return mValue - value;
        }

        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------

        VHDLFloatingPointValue IVHDLArithmetic<VHDLFloatingPointValue>.Abs()
        {
            return new VHDLFloatingPointValue(Math.Abs(mValue));
        }

        VHDLFloatingPointValue IVHDLArithmetic<VHDLFloatingPointValue>.Exp(Int64 value)
        {
            return new VHDLFloatingPointValue(Math.Pow(mValue, value));
        }

        VHDLFloatingPointValue IVHDLArithmetic<VHDLFloatingPointValue>.Exp(VHDLIntegerValue value)
        {
            return new VHDLFloatingPointValue(Math.Pow(mValue, value.Value));
        }

        public VHDLFloatingPointValue Divide(VHDLFloatingPointValue value)
        {
            return new VHDLFloatingPointValue (mValue / value.mValue);
        }

        public VHDLFloatingPointValue Multiply(VHDLFloatingPointValue value)
        {
            return new VHDLFloatingPointValue(mValue * value.mValue);
        }

        VHDLFloatingPointValue IVHDLArithmetic<VHDLFloatingPointValue>.UnaryMinus()
        {
            return new VHDLFloatingPointValue(-mValue);
        }

        VHDLFloatingPointValue IVHDLArithmetic<VHDLFloatingPointValue>.UnaryPlus()
        {
            return new VHDLFloatingPointValue(mValue);
        }

        public VHDLFloatingPointValue Plus(VHDLFloatingPointValue value)
        {
            return new VHDLFloatingPointValue(mValue + value.mValue);
        }

        public VHDLFloatingPointValue Minus(VHDLFloatingPointValue value)
        {
            return new VHDLFloatingPointValue(mValue - value.mValue);
        }
    }
}
