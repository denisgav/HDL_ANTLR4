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
    public class FILE_OPEN_KIND_Enum : EnumBaseType<FILE_OPEN_KIND_Enum>
    {
        public static FILE_OPEN_KIND_Enum READ_MODE = new FILE_OPEN_KIND_Enum(0, "READ_MODE");
        public static FILE_OPEN_KIND_Enum WRITE_MODE = new FILE_OPEN_KIND_Enum(1, "WRITE_MODE");
        public static FILE_OPEN_KIND_Enum APPEND_MODE = new FILE_OPEN_KIND_Enum(2, "APPEND_MODE");

        static FILE_OPEN_KIND_Enum()
        {
            Init();
        }

        public FILE_OPEN_KIND_Enum(int idx, string value)
            : base(idx, value)
        {
        }

        public static void Init()
        {
            if (READ_MODE == null)
                READ_MODE = new FILE_OPEN_KIND_Enum(0, "READ_MODE");
            if (WRITE_MODE == null)
                WRITE_MODE = new FILE_OPEN_KIND_Enum(1, "WRITE_MODE");
            if (APPEND_MODE == null)
                APPEND_MODE = new FILE_OPEN_KIND_Enum(2, "APPEND_MODE");
        }
    }

    [Serializable]
    public class FILE_OPEN_KIND : VHDLEnumValue<FILE_OPEN_KIND_Enum>
    {
        public FILE_OPEN_KIND(FILE_OPEN_KIND_Enum Value)
            : base(Value)
        {
            FILE_OPEN_KIND_Enum.Init();
        }

        public FILE_OPEN_KIND()
            : this(FILE_OPEN_KIND_Enum.READ_MODE)
        {
        }

        public static implicit operator FILE_OPEN_KIND(FILE_OPEN_KIND_Enum Value)
        {
            return new FILE_OPEN_KIND(Value);
        }

    }
}
