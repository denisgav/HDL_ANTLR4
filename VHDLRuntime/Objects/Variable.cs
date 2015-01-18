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

namespace VHDLRuntime.Objects
{
    [Serializable]
    public class Variable : ValueProvider
    {
        public Variable(VHDLBaseValue value_ = null, string name = "", VHDLDirection direction = VHDLDirection.InOut)
            : base(value_, name, direction)
        {
        }
    }

    [Serializable]
    public class Variable<T> : Variable where T : VHDLBaseValue
    {
        public Variable(T value_ = null, string name = "", VHDLDirection direction = VHDLDirection.InOut)
            : base(value_, name, direction)
        {
        }

        public new T GetValue()
        {
            return base.GetValue() as T;
        }

        public new T Value
        {
            get
            {
                return base.Value as T;
            }
            set
            {
                base.Value = value;
            }
        }

    }
}
