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
    public enum VHDLDirection
    {
        In,
        Out,
        InOut,
        Buffer,
        Linkeage
    }

    [Serializable]
    public class ValueProvider
    {
        protected VHDLDirection direction;
        public VHDLDirection Direction
        {
            get { return direction; }
        }

        protected string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        protected VHDLBaseValue value_;
        public virtual VHDLBaseValue Value
        {
            get { return value_; }
            set { value_ = value; }
        }

        public virtual VHDLBaseValue GetValue()
        {
            return value_;
        }
             

        private Type type;
        public Type Type
        {
            get { return type; }
            set { type = value; }
        }

        private static Type defaultVariableType= typeof(VHDLBaseValue);

        public ValueProvider(VHDLBaseValue value_ = null, string name = "", VHDLDirection direction = VHDLDirection.InOut)
            : this(defaultVariableType, value_, name, direction)
        { }


        public ValueProvider(Type type, VHDLBaseValue value_ = null, string name = "", VHDLDirection direction = VHDLDirection.InOut)
        {
            this.type = type;
            this.value_ = value_;
            this.name = name;
            this.direction = direction;
        }

        public virtual void Assign(VHDLBaseValue value_)
        {
            this.value_ = value_;
        }

        public override string ToString()
        {
            return value_.ToString();
        }
    }
}
