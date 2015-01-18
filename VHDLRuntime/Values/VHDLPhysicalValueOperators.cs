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

namespace VHDLRuntime.Values
{
    public partial class VHDLPhysicalValue<T>
    {
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------

        /*
        public VHDLPhysicalValue()
            : base(VHDLPhysicalType<T>.StandardPhysicalType)
        {
            physicalType = VHDLPhysicalType<T>.StandardPhysicalType;
            mValue = 0;
            init();
        }

        public VHDLPhysicalValue(int value)
            : base(VHDLPhysicalType<T>.StandardPhysicalType)
        {
            physicalType = VHDLPhysicalType<T>.StandardPhysicalType;
            init(value);
        }

        public VHDLPhysicalValue(int value, string unitName)
            : base(VHDLPhysicalType<T>.StandardPhysicalType)
        {
            physicalType = VHDLPhysicalType<T>.StandardPhysicalType;
            init(value, unitName);
        }

        public VHDLPhysicalValue(VHDLIntegerValue value)
            : base(VHDLPhysicalType<T>.StandardPhysicalType)
        {
            physicalType = VHDLPhysicalType<T>.StandardPhysicalType;
            init(value.Value);
        }

        public VHDLPhysicalValue(VHDLIntegerValue value, string unitName)
            : base(VHDLPhysicalType<T>.StandardPhysicalType)
        {
            physicalType = VHDLPhysicalType<T>.StandardPhysicalType;
            init(value.Value, unitName);
        }
        */

        public VHDLPhysicalValue()
            : this(VHDLPhysicalType<T>.DefPhysicalType)
        {
        }

        public VHDLPhysicalValue(VHDLPhysicalType<T> physicalType)
            : base(physicalType)
        {
            this.physicalType = physicalType;
            init(0);
        }

        public VHDLPhysicalValue( VHDLPhysicalValue<T> value)
            : this(VHDLPhysicalType<T>.DefPhysicalType, value)
        {

        }

        public VHDLPhysicalValue(VHDLPhysicalType<T> physicalType, VHDLPhysicalValue<T> value)
            : base(physicalType)
        {
            this.physicalType = physicalType;
            init(value.mValue);
        }
       
        public VHDLPhysicalValue(long value)
            : this(VHDLPhysicalType<T>.DefPhysicalType, value)
        {
        }

        public VHDLPhysicalValue(long value, string unitName)
            : this(VHDLPhysicalType<T>.DefPhysicalType, value, unitName)
        {
        }
        
        public VHDLPhysicalValue(VHDLPhysicalType<T> physicalType, long value)
            : base(physicalType)
        {
            this.physicalType = physicalType;
            init(value);
        }

        public VHDLPhysicalValue(VHDLPhysicalType<T> physicalType, long value, string unitName)
            : base(physicalType)
        {
            this.physicalType = physicalType;
            init(value, unitName);
        }        
        //--------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------
    }
}
