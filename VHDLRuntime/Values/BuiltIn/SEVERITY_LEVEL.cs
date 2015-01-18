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
using System.Text;
using VHDLRuntime.Range;
using VHDLRuntime.Values;

namespace VHDLRuntime.Values.BuiltIn
{
    [Serializable]
    public class SEVERITY_LEVEL_Enum : EnumBaseType<SEVERITY_LEVEL_Enum>
    {
        public static SEVERITY_LEVEL_Enum NOTE = new SEVERITY_LEVEL_Enum(0, "NOTE");
        public static SEVERITY_LEVEL_Enum WARNING = new SEVERITY_LEVEL_Enum(1, "WARNING");
        public static SEVERITY_LEVEL_Enum ERROR = new SEVERITY_LEVEL_Enum(2, "ERROR");
        public static SEVERITY_LEVEL_Enum FAILURE = new SEVERITY_LEVEL_Enum(3, "FAILURE");

        static SEVERITY_LEVEL_Enum()
        {
            Init();
        }

        public SEVERITY_LEVEL_Enum(int idx, string value)
            : base(idx, value)
        {
        }

        public static void Init()
        {
            if (NOTE == null)
                NOTE = new SEVERITY_LEVEL_Enum(0, "NOTE");
            if (WARNING == null)
                WARNING = new SEVERITY_LEVEL_Enum(1, "WARNING");
            if (ERROR == null)
                ERROR = new SEVERITY_LEVEL_Enum(2, "ERROR");
            if (FAILURE == null)
                FAILURE = new SEVERITY_LEVEL_Enum(3, "FAILURE");
        }
    }

    [Serializable]
    public class SEVERITY_LEVEL : VHDLEnumValue<SEVERITY_LEVEL_Enum>
    {
        public SEVERITY_LEVEL(SEVERITY_LEVEL_Enum Value)
            : base(Value)
        {
            SEVERITY_LEVEL_Enum.Init();
        }

        public SEVERITY_LEVEL()
            : this(SEVERITY_LEVEL_Enum.NOTE)
        {
        }

        public static implicit operator SEVERITY_LEVEL(SEVERITY_LEVEL_Enum Value)
        {
            return new SEVERITY_LEVEL(Value);
        }

    }
}
