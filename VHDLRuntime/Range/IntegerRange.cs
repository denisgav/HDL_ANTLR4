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

namespace VHDLRuntime.Range
{
    [Serializable]
    public class IntegerRange : DiscreteRange<VHDLIntegerValue>
    {
        public IntegerRange(VHDLIntegerValue left, VHDLIntegerValue right, RangeDirection direction)
            : base(left, right, direction)
        {
        }

        public IntegerRange(Int64 left, Int64 right, RangeDirection direction)
            : base(new VHDLIntegerValue(left), new VHDLIntegerValue(right), direction)
        {
        }

        public IntegerRange()
            : base(new VHDLIntegerValue(Int64.MinValue), new VHDLIntegerValue(Int64.MaxValue), RangeDirection.To)
        {
        }

        public override IEnumerator<VHDLIntegerValue> GetEnumerator()
        {
            if (direction == RangeDirection.To)
            {
                for (Int64 i = left.Value; i <= right.Value; i++)
                {
                    yield return new VHDLIntegerValue(i);
                }
            }
            else
            {
                for (Int64 i = right.Value; i >= left.Value; i--)
                {
                    yield return new VHDLIntegerValue(i);
                }
            }
        }

        public override VHDLIntegerValue POS(VHDLIntegerValue X)
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

        public override VHDLIntegerValue SUCC(VHDLIntegerValue X)
        {
            return new VHDLIntegerValue(X.Value + 1);
        }

        public override VHDLIntegerValue PRED(VHDLIntegerValue X)
        {
            return new VHDLIntegerValue(X.Value - 1);
        }

        public override VHDLIntegerValue LEFTOF(VHDLIntegerValue X)
        {
            if (direction == RangeDirection.To)
            {
                return new VHDLIntegerValue(X.Value - 1);
            }
            else
            {
                return new VHDLIntegerValue(X.Value + 1);
            }
        }

        public override VHDLIntegerValue RIGHTOF(VHDLIntegerValue X)
        {
            if (direction == RangeDirection.To)
            {
                return new VHDLIntegerValue(X.Value + 1);
            }
            else
            {
                return new VHDLIntegerValue(X.Value - 1);
            }
        }

        
    }
}
