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
using VHDLRuntime;
using VHDLRuntime.Types;
using VHDLRuntime.Range;
using VHDLRuntime.Values;

namespace VHDLRuntime.Values.BuiltIn
{
    [Serializable]
    public class TIME_PhysicalValue : PhysicalUnitBaseType<TIME_PhysicalValue>
    {
        public static TIME_PhysicalValue fs;
        public static TIME_PhysicalValue ps;
        public static TIME_PhysicalValue ns;
        public static TIME_PhysicalValue us;
        public static TIME_PhysicalValue ms;
        public static TIME_PhysicalValue sec;
        public static TIME_PhysicalValue min;
        public static TIME_PhysicalValue hr;


        static TIME_PhysicalValue()
        {
            TIME_PhysicalValue.Init();
        }

        public TIME_PhysicalValue(long multiplier, string unitName)
            : base(multiplier, unitName)
        {
        }

        public TIME_PhysicalValue(long multiplier, string unitName, string baseUnitName)
            : base(multiplier, unitName, baseUnitName)
        {
        }

        public static void Init()
        {
            if (fs == null)
            {
                fs = new TIME_PhysicalValue(1, "fs");
            }
            if (ps == null)
            {
                ps = new TIME_PhysicalValue(1000, "ps", "fs");
            }
            if (ns == null)
            {
                ns = new TIME_PhysicalValue(1000, "ns", "ps");
            }
            if (us == null)
            {
                us = new TIME_PhysicalValue(1000, "us", "ns");
            }
            if (ms == null)
            {
                ms = new TIME_PhysicalValue(1000, "ms", "us");
            }
            if (sec == null)
            {
                sec = new TIME_PhysicalValue(1000, "sec", "ms");
            }
            if (min == null)
            {
                min = new TIME_PhysicalValue(60, "min", "sec");
            }
            if (hr == null)
            {
                hr = new TIME_PhysicalValue(60, "hr", "min");
            }

        }
    }



    public class TIME : VHDLPhysicalValue<TIME_PhysicalValue>
    {
        private static VHDLPhysicalType<TIME_PhysicalValue> physType;

        public static VHDLPhysicalType<TIME_PhysicalValue> PhysType
        {
            get
            {
                if (physType == null)
                {
                    physType = new VHDLPhysicalType<TIME_PhysicalValue>(-2147483648, 2147483647, RangeDirection.To);
                }
                return physType;
            }
        }

        static TIME()
        {
            TIME_PhysicalValue.Init();
        }

        public TIME(long value, string unitName)
            : base(PhysType, value, unitName)
        {
        }

        public TIME()
            : base(PhysType)
        {
        }

        public TIME(long value)
            : base(PhysType, value)
        {
        }

        public TIME(VHDLPhysicalValue<TIME_PhysicalValue> value)
            : base(PhysType, value)
        {
        }
    }
}