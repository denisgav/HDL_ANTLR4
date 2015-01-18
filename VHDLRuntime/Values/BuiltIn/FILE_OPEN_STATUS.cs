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
    public class FILE_OPEN_STATUS_Enum : EnumBaseType<FILE_OPEN_STATUS_Enum>
    {
        public static FILE_OPEN_STATUS_Enum OPEN_OK = new FILE_OPEN_STATUS_Enum(0, "OPEN_OK");
        public static FILE_OPEN_STATUS_Enum STATUS_ERROR = new FILE_OPEN_STATUS_Enum(1, "STATUS_ERROR");
        public static FILE_OPEN_STATUS_Enum NAME_ERROR = new FILE_OPEN_STATUS_Enum(2, "NAME_ERROR");
        public static FILE_OPEN_STATUS_Enum MODE_ERROR = new FILE_OPEN_STATUS_Enum(3, "MODE_ERROR");

        static FILE_OPEN_STATUS_Enum()
        {
            Init();
        }

        public FILE_OPEN_STATUS_Enum(int idx, string value)
            : base(idx, value)
        {
        }

        public static void Init()
        {
            if (OPEN_OK == null)
                OPEN_OK = new FILE_OPEN_STATUS_Enum(0, "OPEN_OK");
            if (STATUS_ERROR == null)
                STATUS_ERROR = new FILE_OPEN_STATUS_Enum(1, "STATUS_ERROR");
            if (NAME_ERROR == null)
                NAME_ERROR = new FILE_OPEN_STATUS_Enum(2, "NAME_ERROR");
            if (MODE_ERROR == null)
                MODE_ERROR = new FILE_OPEN_STATUS_Enum(3, "MODE_ERROR");
        }
    }

    [Serializable]
    public class FILE_OPEN_STATUS : VHDLEnumValue<FILE_OPEN_STATUS_Enum>
    {
        public FILE_OPEN_STATUS(FILE_OPEN_STATUS_Enum Value)
            : base(Value)
        {
            FILE_OPEN_STATUS_Enum.Init();
        }

        public FILE_OPEN_STATUS()
            : this(FILE_OPEN_STATUS_Enum.OPEN_OK)
        {
        }

        public static implicit operator FILE_OPEN_STATUS(FILE_OPEN_STATUS_Enum Value)
        {
            return new FILE_OPEN_STATUS(Value);
        }

    }
}
