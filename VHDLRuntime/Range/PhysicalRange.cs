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
using VHDLRuntime.Types;

namespace VHDLRuntime.Range
{
    
    [Serializable]
    public class PhysicalRange<T> : DiscreteRange<VHDLPhysicalValue<T>> where T:PhysicalUnitBaseType<T>
    {
        public PhysicalRange(VHDLPhysicalValue<T> left, VHDLPhysicalValue<T> right, RangeDirection direction)
            : base(left, right, direction)
        {
        }

        public PhysicalRange(Int64 left, Int64 right, RangeDirection direction)
            : base(new VHDLPhysicalValue<T>(left), new VHDLPhysicalValue<T>(right), direction)
        {
        }

        public PhysicalRange()
            : base(new VHDLPhysicalValue<T>(Int32.MinValue), new VHDLPhysicalValue<T>(Int32.MaxValue), RangeDirection.To)
        {
        }       

        public override IEnumerator<VHDLPhysicalValue<T>> GetEnumerator()
        {
            if (direction == RangeDirection.To)
            {
                for (Int64 i = left.Value; i <= right.Value; i++)
                {
                    yield return new VHDLPhysicalValue<T>(i);
                }
            }
            else
            {
                for (Int64 i = right.Value; i >= left.Value; i--)
                {
                    yield return new VHDLPhysicalValue<T>(i);
                }
            }
        }

        public override VHDLIntegerValue POS(VHDLPhysicalValue<T> X)
        {
            if (direction == RangeDirection.To)
            {
                return new VHDLIntegerValue(X.Value - left.Value);
            }
            else
            {
                return new VHDLIntegerValue(left.Value - X.Value);
            }
        }

        public override VHDLPhysicalValue<T> SUCC(VHDLPhysicalValue<T> X)
        {
            return new VHDLPhysicalValue<T>(X.Value + 1);
        }

        public override VHDLPhysicalValue<T> PRED(VHDLPhysicalValue<T> X)
        {
            return new VHDLPhysicalValue<T>(X.Value - 1);
        }

        public override VHDLPhysicalValue<T> LEFTOF(VHDLPhysicalValue<T> X)
        {
            if (direction == RangeDirection.To)
            {
                return new VHDLPhysicalValue<T>(X.Value - 1);
            }
            else
            {
                return new VHDLPhysicalValue<T>(X.Value + 1);
            }
        }

        public override VHDLPhysicalValue<T> RIGHTOF(VHDLPhysicalValue<T> X)
        {
            if (direction == RangeDirection.To)
            {
                return new VHDLPhysicalValue<T>(X.Value + 1);
            }
            else
            {
                return new VHDLPhysicalValue<T>(X.Value - 1);
            }
        }        
    }
}
