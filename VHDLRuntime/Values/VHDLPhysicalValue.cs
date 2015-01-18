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
using VHDLRuntime.Range;
using VHDLRuntime.Types;

namespace VHDLRuntime.Values
{    
    [Serializable]
    public partial class VHDLPhysicalValue<T> : VHDLDiscreteValue, IVHDLPhysicalValue<VHDLPhysicalValue<T>> where T : PhysicalUnitBaseType<T>
    {
        private VHDLPhysicalType<T> physicalType;
        public virtual VHDLPhysicalType<T> PhysicalType
        {
            get { return physicalType; }
        }

        protected virtual void init()
        {
            mValue = PhysicalType.CorrectValue(this).mValue;
            mStringValue = PhysicalUnitBaseType<T>.IntToString(mValue);
        }

        protected virtual void init(Int64 value)
        {
            mValue = value;
            mValue = PhysicalType.CorrectValue(this).mValue;
            mStringValue = PhysicalUnitBaseType<T>.IntToString(mValue);
        }

        protected virtual void init(Int64 value, string unitName)
        {
            Int64 resValue = PhysicalUnitBaseType<T>.ConvertToInt(unitName, value);
            mValue = resValue;
            mValue = PhysicalType.CorrectValue(this).mValue;
            mStringValue = PhysicalUnitBaseType<T>.IntToString(mValue);
        }

        private string mStringValue;

        protected Int64 mValue;
        public Int64 Value
        {
            get { return mValue; }
        }

        public override int CompareTo(VHDLScalarValue obj)
        {
            if (obj is VHDLPhysicalValue<T>)
            {
                VHDLPhysicalValue<T> els = obj as VHDLPhysicalValue<T>;
                return mValue.CompareTo(els.mValue);
            }
            throw new ArgumentException(string.Format("Could not cast {0} to VHDLPhysicalValue<{1}>", obj.GetType().Name, GetType().Name));
        }

        public override string ToString()
        {
            return mStringValue;
        }

        public VHDLPhysicalValue<T> Abs()
        {
            return new VHDLPhysicalValue<T>(physicalType, Math.Abs(mValue));
        }

        public VHDLPhysicalValue<T> Divide(long value)
        {
            return new VHDLPhysicalValue<T>(physicalType, mValue / value);
        }

        public VHDLPhysicalValue<T> Divide(VHDLIntegerValue value)
        {
            return new VHDLPhysicalValue<T>(physicalType, mValue / value.Value);
        }

        public VHDLPhysicalValue<T> Multiply(long value)
        {
            return new VHDLPhysicalValue<T>(physicalType, mValue * value);
        }

        public VHDLPhysicalValue<T> Multiply(VHDLIntegerValue value)
        {
            return new VHDLPhysicalValue<T>(physicalType, mValue * value.Value);
        }

        public VHDLPhysicalValue<T> UnaryMinus()
        {
            return new VHDLPhysicalValue<T>(physicalType , - mValue);
        }

        public VHDLPhysicalValue<T> UnaryPlus()
        {
            return new VHDLPhysicalValue<T>(physicalType, mValue);
        }

        public VHDLPhysicalValue<T> Plus(VHDLPhysicalValue<T> value)
        {
            return new VHDLPhysicalValue<T>(physicalType, mValue + value.Value);
        }

        public VHDLPhysicalValue<T> Minus(VHDLPhysicalValue<T> value)
        {
            return new VHDLPhysicalValue<T>(physicalType, mValue - value.Value);
        }

        public bool LessThan(VHDLPhysicalValue<T> value)
        {
            return mValue < value.Value;
        }

        public bool LessEquals(VHDLPhysicalValue<T> value)
        {
            return mValue <= value.Value;
        }

        public bool GreaterThan(VHDLPhysicalValue<T> value)
        {
            return mValue > value.Value;
        }

        public bool GreaterEquals(VHDLPhysicalValue<T> value)
        {
            return mValue >= value.Value;
        }

        public bool NotEquals(VHDLPhysicalValue<T> value)
        {
            return mValue != value.Value;
        }

        public bool Equals(VHDLPhysicalValue<T> value)
        {
            return mValue == value.Value;
        }
    }
}
