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
using VHDLRuntime.Values;

namespace VHDLRuntime.Types
{
    [Serializable]
    public class VHDLIntegerType : VHDLDiscreteType<VHDLIntegerValue>
    {
        private static VHDLIntegerType standardIntegerType = new VHDLIntegerType();
        public static VHDLIntegerType StandardIntegerType
        {
            get 
            {
                if (standardIntegerType == null)
                    standardIntegerType = new VHDLIntegerType();

                return standardIntegerType; 
            }
        }

        private static IntegerRange defIntegerRange = new IntegerRange(new VHDLIntegerValue(Int32.MinValue), new VHDLIntegerValue(Int32.MaxValue), RangeDirection.To);
        public static  IntegerRange DefIntegerRange
        {
            get
            {
                if (defIntegerRange == null)
                    defIntegerRange = new IntegerRange(new VHDLIntegerValue(Int32.MinValue), new VHDLIntegerValue(Int32.MaxValue), RangeDirection.To);
                return defIntegerRange; 
            }
        }


        private IntegerRange mRange;
        public IntegerRange Range
        {
            get { return mRange; }
            private set
            {
                mRange = value;
                discreteRange = mRange;
                scalarRange = mRange;
            }
        }

        public VHDLIntegerType(IntegerRange range)
        {
            Range = range;
        }

        public VHDLIntegerType()
        {
            Range = defIntegerRange;
        }


        private DiscreteRange<VHDLIntegerValue> discreteRange;
        public override DiscreteRange<VHDLIntegerValue> DiscreteRange
        {
            get { return discreteRange; }
        }

        private ScalarRange<VHDLIntegerValue> scalarRange;
        public override ScalarRange<VHDLIntegerValue> ScalarRange
        {
            get { return scalarRange; }
        }

        public override VHDLIntegerValue VALUE(string X)
        {
            Int32 data = Int32.Parse(X);
            return new VHDLIntegerValue(data);
        }

        public override VHDLIntegerValue CorrectValue(VHDLIntegerValue value)
        {
            if (mRange == null)
                return value;
            else
                return mRange.CorrectValue(value);
        }
    }
}
