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
    public class BIT_Enum : EnumBaseType<BIT_Enum>
    {
        public static BIT_Enum item_0 = new BIT_Enum(0, "0");
        public static BIT_Enum item_1 = new BIT_Enum(1, "1");

        static BIT_Enum()
        {
            Init();
        }

        public BIT_Enum(int idx, string value)
            : base(idx, value)
        {
        }

        public static void Init()
        {
            if (item_0 == null)
                item_0 = new BIT_Enum(0, "0");
            if (item_1 == null)
                item_1 = new BIT_Enum(1, "1");
        }
    }

    [Serializable]
    public class BIT : VHDLEnumValue<BIT_Enum>, IVHDLComparable<BIT>, IVHDLLogic<BIT>
    {
        public BIT(BIT_Enum Value)
            : base(Value)
        {
            BIT_Enum.Init();
        }

        public BIT(BIT Value)
            : this(Value.TypedValue)
        {
        }

        public BIT()
            : this(BIT_Enum.item_0)
        {
        }

        public static implicit operator BIT(BIT_Enum Value)
        {
            return new BIT(Value);
        }

        public bool LessThan(BIT value)
        {
            throw new NotImplementedException();
        }

        public bool LessEquals(BIT value)
        {
            throw new NotImplementedException();
        }

        public bool GreaterThan(BIT value)
        {
            throw new NotImplementedException();
        }

        public bool GreaterEquals(BIT value)
        {
            throw new NotImplementedException();
        }

        public bool NotEquals(BIT value)
        {
            throw new NotImplementedException();
        }

        public bool Equals(BIT value)
        {
            throw new NotImplementedException();
        }

        public BIT And(BIT value)
        {
            throw new NotImplementedException();
        }

        public BIT Or(BIT value)
        {
            throw new NotImplementedException();
        }

        public BIT Nand(BIT value)
        {
            throw new NotImplementedException();
        }

        public BIT Nor(BIT value)
        {
            throw new NotImplementedException();
        }

        public BIT Xor(BIT value)
        {
            throw new NotImplementedException();
        }

        public BIT Xnor(BIT value)
        {
            throw new NotImplementedException();
        }

        public BIT Not()
        {
            if (typedValue.Equals(BIT_Enum.item_0))
                return new BIT(BIT_Enum.item_1);
            else
                return new BIT(BIT_Enum.item_0);
        }
    }
}
