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
using System.Collections.ObjectModel;

namespace VHDLRuntime.Range
{
    
    [Serializable]
    public class EnumRange<T> : DiscreteRange<VHDLEnumValue<T>> where T : EnumBaseType<T>
    {
        public EnumRange()
            : base(new VHDLEnumValue<T>(EnumBaseType<T>.GetFirstValue()), new VHDLEnumValue<T>(EnumBaseType<T>.GetLastValue()), RangeDirection.To)
        {
        }

        public EnumRange(T left, T right, RangeDirection direction)
            : base(new VHDLEnumValue<T>(left), new VHDLEnumValue<T>(right), direction)
        {
        }

        public EnumRange(VHDLEnumValue<T> left, VHDLEnumValue<T> right, RangeDirection direction)
            : base(left, right, direction)
        {
        }

        public override IEnumerator<VHDLEnumValue<T>> GetEnumerator()
        {
            foreach (T i in EnumBaseType<T>.GetBaseValuesInRange(left.TypedValue, right.TypedValue, direction))
            {
                yield return new VHDLEnumValue<T>(i);
            }
        }
        
        public override VHDLIntegerValue POS(VHDLEnumValue<T> X)
        {
            return new VHDLIntegerValue(EnumBaseType<T>.IndexOf(X.TypedValue));
        }

        public override VHDLEnumValue<T> SUCC(VHDLEnumValue<T> X)
        {
            return new VHDLEnumValue<T>(EnumBaseType<T>.SUCC(X.TypedValue));
        }

        public override VHDLEnumValue<T> PRED(VHDLEnumValue<T> X)
        {
            return new VHDLEnumValue<T>(EnumBaseType<T>.PRED(X.TypedValue));
        }

        public override VHDLEnumValue<T> LEFTOF(VHDLEnumValue<T> X)
        {
            return new VHDLEnumValue<T>(EnumBaseType<T>.PRED(X.TypedValue));
        }

        public override VHDLEnumValue<T> RIGHTOF(VHDLEnumValue<T> X)
        {
            return new VHDLEnumValue<T>(EnumBaseType<T>.SUCC(X.TypedValue));
        }
    }
}
