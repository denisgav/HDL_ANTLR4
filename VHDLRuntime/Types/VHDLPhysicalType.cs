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
    public class VHDLPhysicalType<T> : VHDLDiscreteType<VHDLPhysicalValue<T>> where T : PhysicalUnitBaseType<T>
    {
        private static VHDLPhysicalType<T> defPhysicalType;
        public static VHDLPhysicalType<T> DefPhysicalType
        {
            get
            {
                if (defPhysicalType == null)
                    defPhysicalType = new VHDLPhysicalType<T>();
                return defPhysicalType;
            }
        }

        private static PhysicalRange<T> mRange;
        public static PhysicalRange<T> Range
        {
            get { return mRange; }
            private set
            {
                mRange = value;
            }
        }

        public VHDLPhysicalType(PhysicalRange<T> range)
        {
            defPhysicalType = this;
            Range = range;            
        }

        public VHDLPhysicalType(Int64 left, Int64 right, RangeDirection direction)
        {
            defPhysicalType = this;
            Range = new PhysicalRange<T>(left, right, direction);            
        }

        public VHDLPhysicalType()
        {
            defPhysicalType = this;
            Range = new PhysicalRange<T>();            
        }

        public override DiscreteRange<VHDLPhysicalValue<T>> DiscreteRange
        {
            get { return mRange; }
        }

        public override ScalarRange<VHDLPhysicalValue<T>> ScalarRange
        {
            get { return mRange; }
        }

        public override VHDLPhysicalValue<T> VALUE(string X)
        {
            throw new NotImplementedException();
        }

        public override VHDLPhysicalValue<T> CorrectValue(VHDLPhysicalValue<T> value)
        {
            if (mRange == null)
                return value;
            else
                return mRange.CorrectValue(value);
        }
    }
}
