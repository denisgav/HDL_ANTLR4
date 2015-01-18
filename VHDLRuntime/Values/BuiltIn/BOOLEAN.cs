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
    public class BOOLEAN_Enum : EnumBaseType<BOOLEAN_Enum>
    {
        public static BOOLEAN_Enum FALSE = new BOOLEAN_Enum(0, "FALSE");
        public static BOOLEAN_Enum TRUE = new BOOLEAN_Enum(1, "TRUE");

        static BOOLEAN_Enum()
        {
            Init();
        }

        public BOOLEAN_Enum(int idx, string value)
            : base(idx, value)
        {
        }

        public static void Init()
        {
            if (FALSE == null)
                FALSE = new BOOLEAN_Enum(0, "FALSE");
            if (TRUE == null)
                TRUE = new BOOLEAN_Enum(1, "TRUE");
        }
    }

    [Serializable]
    public class BOOLEAN : VHDLEnumValue<BOOLEAN_Enum>
    {
        public BOOLEAN(BOOLEAN_Enum Value)
            : base(Value)
        {
            BOOLEAN_Enum.Init();
        }

        public BOOLEAN()
            : this(BOOLEAN_Enum.FALSE)
        {
        }

        public static implicit operator BOOLEAN(BOOLEAN_Enum Value)
        {
            return new BOOLEAN(Value);
        }

    }
}
