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
using VHDLRuntime.Values;
using VHDLRuntime.Range;

namespace VHDLRuntime.Types
{
    [Serializable]
    public class VHDLFloatingPointType : VHDLScalarType<VHDLFloatingPointValue>
    {
        private static VHDLFloatingPointType standardFloatType = new VHDLFloatingPointType();
        public static VHDLFloatingPointType StandardFloatType
        {
            get
            {
                if (standardFloatType == null)
                    standardFloatType = new VHDLFloatingPointType();

                return standardFloatType;
            }
        }

        private static FloatingPointRange defFloatRange = new FloatingPointRange(new VHDLFloatingPointValue(double.MaxValue), new VHDLFloatingPointValue(double.MaxValue), RangeDirection.To);
        public static FloatingPointRange DefFloatRange
        {
            get
            {
                if (defFloatRange == null)
                    defFloatRange = new FloatingPointRange(new VHDLFloatingPointValue(double.MaxValue), new VHDLFloatingPointValue(double.MaxValue), RangeDirection.To);
                return defFloatRange;
            }
        }

        private FloatingPointRange mRange;
        public FloatingPointRange Range
        {
            get { return mRange; }
            private set
            {
                mRange = value;
                scalarRange = mRange;
            }
        }

        public VHDLFloatingPointType(FloatingPointRange range)
        {
            Range = range;
        }

        public VHDLFloatingPointType()
        {
            Range = defFloatRange;
        }

        

        public override VHDLFloatingPointValue VALUE(string X)
        {
            double data = double.Parse(X);
            return new VHDLFloatingPointValue(data);
        }

        private ScalarRange<VHDLFloatingPointValue> scalarRange;
        public override ScalarRange<VHDLFloatingPointValue> ScalarRange
        {
            get { return scalarRange; }
        }

        public override VHDLFloatingPointValue CorrectValue(VHDLFloatingPointValue value)
        {
            if (mRange == null)
                return value;
            else
                return mRange.CorrectValue(value);
        }
    }
}
