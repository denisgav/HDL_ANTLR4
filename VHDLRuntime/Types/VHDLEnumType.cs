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
    public class VHDLEnumType<T> : VHDLDiscreteType<VHDLEnumValue<T>> where T : EnumBaseType<T>
    {
        private EnumRange<T> enumRange;
        public EnumRange<T> EnumRange
        {
            get 
            {
                if (enumRange == null)
                    enumRange = new EnumRange<T>();
                return enumRange;
            }
        }

        public VHDLEnumType()
        {
        }


        public override VHDLEnumValue<T> VALUE(string X)
        {
            T data = EnumBaseType<T>.Parse(X);
            return new VHDLEnumValue<T>(data);
        }

        public override VHDLEnumValue<T> CorrectValue(VHDLEnumValue<T> value)
        {
            return value;
        }

        public override DiscreteRange<VHDLEnumValue<T>> DiscreteRange
        {
            get { return enumRange; }
        }

        public override ScalarRange<VHDLEnumValue<T>> ScalarRange
        {
            get { return enumRange; }
        }
    }
}
