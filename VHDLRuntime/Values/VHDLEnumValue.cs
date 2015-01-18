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
    public class VHDLEnumValue<T> : VHDLDiscreteValue where T : EnumBaseType<T>
    {
        private static VHDLEnumType<T> enumType;
        public static VHDLEnumType<T> EnumType
        {
            get
            {
                if (enumType == null)
                    enumType = new VHDLEnumType<T>();
                return enumType;
            }
        }

        public override int CompareTo(VHDLScalarValue obj)
        {
            if (obj is VHDLEnumValue<T>)
                return typedValue.Key.CompareTo((obj as VHDLEnumValue<T>).typedValue.Key);

            return 0;
        }

        public override string ToString()
        {
            return TypedValue.Value;
        }

        protected T typedValue;
        public T TypedValue
        {
            get 
            {
                return typedValue; 
            }
            set 
            {
                typedValue = value;
            }
        }

        public VHDLEnumValue(VHDLEnumValue<T> mValue)
            :base(enumType)
        {
            typedValue = mValue.typedValue;
        }

        public VHDLEnumValue(T mValue)
            : base(enumType)
        {
            typedValue = mValue;
        }
    }
}
