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
    public class CHARACTER_Enum : EnumBaseType<CHARACTER_Enum>
    {
        public static CHARACTER_Enum NUL = new CHARACTER_Enum(0, "NUL");
        public static CHARACTER_Enum SOH = new CHARACTER_Enum(1, "SOH");
        public static CHARACTER_Enum STX = new CHARACTER_Enum(2, "STX");
        public static CHARACTER_Enum ETX = new CHARACTER_Enum(3, "ETX");
        public static CHARACTER_Enum EOT = new CHARACTER_Enum(4, "EOT");
        public static CHARACTER_Enum ENQ = new CHARACTER_Enum(5, "ENQ");
        public static CHARACTER_Enum ACK = new CHARACTER_Enum(6, "ACK");
        public static CHARACTER_Enum BEL = new CHARACTER_Enum(7, "BEL");
        public static CHARACTER_Enum BS = new CHARACTER_Enum(8, "BS");
        public static CHARACTER_Enum HT = new CHARACTER_Enum(9, "HT");
        public static CHARACTER_Enum LF = new CHARACTER_Enum(10, "LF");
        public static CHARACTER_Enum VT = new CHARACTER_Enum(11, "VT");
        public static CHARACTER_Enum FF = new CHARACTER_Enum(12, "FF");
        public static CHARACTER_Enum CR = new CHARACTER_Enum(13, "CR");
        public static CHARACTER_Enum SO = new CHARACTER_Enum(14, "SO");
        public static CHARACTER_Enum SI = new CHARACTER_Enum(15, "SI");
        public static CHARACTER_Enum DLE = new CHARACTER_Enum(16, "DLE");
        public static CHARACTER_Enum DC1 = new CHARACTER_Enum(17, "DC1");
        public static CHARACTER_Enum DC2 = new CHARACTER_Enum(18, "DC2");
        public static CHARACTER_Enum DC3 = new CHARACTER_Enum(19, "DC3");
        public static CHARACTER_Enum DC4 = new CHARACTER_Enum(20, "DC4");
        public static CHARACTER_Enum NAK = new CHARACTER_Enum(21, "NAK");
        public static CHARACTER_Enum SYN = new CHARACTER_Enum(22, "SYN");
        public static CHARACTER_Enum ETB = new CHARACTER_Enum(23, "ETB");
        public static CHARACTER_Enum CAN = new CHARACTER_Enum(24, "CAN");
        public static CHARACTER_Enum EM = new CHARACTER_Enum(25, "EM");
        public static CHARACTER_Enum SUB = new CHARACTER_Enum(26, "SUB");
        public static CHARACTER_Enum ESC = new CHARACTER_Enum(27, "ESC");
        public static CHARACTER_Enum FSP = new CHARACTER_Enum(28, "FSP");
        public static CHARACTER_Enum GSP = new CHARACTER_Enum(29, "GSP");
        public static CHARACTER_Enum RSP = new CHARACTER_Enum(30, "RSP");
        public static CHARACTER_Enum USP = new CHARACTER_Enum(31, "USP");
        public static CHARACTER_Enum DEL = new CHARACTER_Enum(32, "DEL");
        public static CHARACTER_Enum C128 = new CHARACTER_Enum(33, "C128");
        public static CHARACTER_Enum C129 = new CHARACTER_Enum(34, "C129");
        public static CHARACTER_Enum C130 = new CHARACTER_Enum(35, "C130");
        public static CHARACTER_Enum C131 = new CHARACTER_Enum(36, "C131");
        public static CHARACTER_Enum C132 = new CHARACTER_Enum(37, "C132");
        public static CHARACTER_Enum C133 = new CHARACTER_Enum(38, "C133");
        public static CHARACTER_Enum C134 = new CHARACTER_Enum(39, "C134");
        public static CHARACTER_Enum C135 = new CHARACTER_Enum(40, "C135");
        public static CHARACTER_Enum C136 = new CHARACTER_Enum(41, "C136");
        public static CHARACTER_Enum C137 = new CHARACTER_Enum(42, "C137");
        public static CHARACTER_Enum C138 = new CHARACTER_Enum(43, "C138");
        public static CHARACTER_Enum C139 = new CHARACTER_Enum(44, "C139");
        public static CHARACTER_Enum C140 = new CHARACTER_Enum(45, "C140");
        public static CHARACTER_Enum C141 = new CHARACTER_Enum(46, "C141");
        public static CHARACTER_Enum C142 = new CHARACTER_Enum(47, "C142");
        public static CHARACTER_Enum C143 = new CHARACTER_Enum(48, "C143");
        public static CHARACTER_Enum C144 = new CHARACTER_Enum(49, "C144");
        public static CHARACTER_Enum C145 = new CHARACTER_Enum(50, "C145");
        public static CHARACTER_Enum C146 = new CHARACTER_Enum(51, "C146");
        public static CHARACTER_Enum C147 = new CHARACTER_Enum(52, "C147");
        public static CHARACTER_Enum C148 = new CHARACTER_Enum(53, "C148");
        public static CHARACTER_Enum C149 = new CHARACTER_Enum(54, "C149");
        public static CHARACTER_Enum C150 = new CHARACTER_Enum(55, "C150");
        public static CHARACTER_Enum C151 = new CHARACTER_Enum(56, "C151");
        public static CHARACTER_Enum C152 = new CHARACTER_Enum(57, "C152");
        public static CHARACTER_Enum C153 = new CHARACTER_Enum(58, "C153");
        public static CHARACTER_Enum C154 = new CHARACTER_Enum(59, "C154");
        public static CHARACTER_Enum C155 = new CHARACTER_Enum(60, "C155");
        public static CHARACTER_Enum C156 = new CHARACTER_Enum(61, "C156");
        public static CHARACTER_Enum C157 = new CHARACTER_Enum(62, "C157");
        public static CHARACTER_Enum C158 = new CHARACTER_Enum(63, "C158");
        public static CHARACTER_Enum C159 = new CHARACTER_Enum(64, "C159");

        static CHARACTER_Enum()
        {
            Init();
        }

        public CHARACTER_Enum(int idx, string value)
            : base(idx, value)
        {
        }

        public static void Init()
        {
            if (NUL == null)
                NUL = new CHARACTER_Enum(0, "NUL");
            if (SOH == null)
                SOH = new CHARACTER_Enum(1, "SOH");
            if (STX == null)
                STX = new CHARACTER_Enum(2, "STX");
            if (ETX == null)
                ETX = new CHARACTER_Enum(3, "ETX");
            if (EOT == null)
                EOT = new CHARACTER_Enum(4, "EOT");
            if (ENQ == null)
                ENQ = new CHARACTER_Enum(5, "ENQ");
            if (ACK == null)
                ACK = new CHARACTER_Enum(6, "ACK");
            if (BEL == null)
                BEL = new CHARACTER_Enum(7, "BEL");
            if (BS == null)
                BS = new CHARACTER_Enum(8, "BS");
            if (HT == null)
                HT = new CHARACTER_Enum(9, "HT");
            if (LF == null)
                LF = new CHARACTER_Enum(10, "LF");
            if (VT == null)
                VT = new CHARACTER_Enum(11, "VT");
            if (FF == null)
                FF = new CHARACTER_Enum(12, "FF");
            if (CR == null)
                CR = new CHARACTER_Enum(13, "CR");
            if (SO == null)
                SO = new CHARACTER_Enum(14, "SO");
            if (SI == null)
                SI = new CHARACTER_Enum(15, "SI");
            if (DLE == null)
                DLE = new CHARACTER_Enum(16, "DLE");
            if (DC1 == null)
                DC1 = new CHARACTER_Enum(17, "DC1");
            if (DC2 == null)
                DC2 = new CHARACTER_Enum(18, "DC2");
            if (DC3 == null)
                DC3 = new CHARACTER_Enum(19, "DC3");
            if (DC4 == null)
                DC4 = new CHARACTER_Enum(20, "DC4");
            if (NAK == null)
                NAK = new CHARACTER_Enum(21, "NAK");
            if (SYN == null)
                SYN = new CHARACTER_Enum(22, "SYN");
            if (ETB == null)
                ETB = new CHARACTER_Enum(23, "ETB");
            if (CAN == null)
                CAN = new CHARACTER_Enum(24, "CAN");
            if (EM == null)
                EM = new CHARACTER_Enum(25, "EM");
            if (SUB == null)
                SUB = new CHARACTER_Enum(26, "SUB");
            if (ESC == null)
                ESC = new CHARACTER_Enum(27, "ESC");
            if (FSP == null)
                FSP = new CHARACTER_Enum(28, "FSP");
            if (GSP == null)
                GSP = new CHARACTER_Enum(29, "GSP");
            if (RSP == null)
                RSP = new CHARACTER_Enum(30, "RSP");
            if (USP == null)
                USP = new CHARACTER_Enum(31, "USP");
            if (DEL == null)
                DEL = new CHARACTER_Enum(32, "DEL");
            if (C128 == null)
                C128 = new CHARACTER_Enum(33, "C128");
            if (C129 == null)
                C129 = new CHARACTER_Enum(34, "C129");
            if (C130 == null)
                C130 = new CHARACTER_Enum(35, "C130");
            if (C131 == null)
                C131 = new CHARACTER_Enum(36, "C131");
            if (C132 == null)
                C132 = new CHARACTER_Enum(37, "C132");
            if (C133 == null)
                C133 = new CHARACTER_Enum(38, "C133");
            if (C134 == null)
                C134 = new CHARACTER_Enum(39, "C134");
            if (C135 == null)
                C135 = new CHARACTER_Enum(40, "C135");
            if (C136 == null)
                C136 = new CHARACTER_Enum(41, "C136");
            if (C137 == null)
                C137 = new CHARACTER_Enum(42, "C137");
            if (C138 == null)
                C138 = new CHARACTER_Enum(43, "C138");
            if (C139 == null)
                C139 = new CHARACTER_Enum(44, "C139");
            if (C140 == null)
                C140 = new CHARACTER_Enum(45, "C140");
            if (C141 == null)
                C141 = new CHARACTER_Enum(46, "C141");
            if (C142 == null)
                C142 = new CHARACTER_Enum(47, "C142");
            if (C143 == null)
                C143 = new CHARACTER_Enum(48, "C143");
            if (C144 == null)
                C144 = new CHARACTER_Enum(49, "C144");
            if (C145 == null)
                C145 = new CHARACTER_Enum(50, "C145");
            if (C146 == null)
                C146 = new CHARACTER_Enum(51, "C146");
            if (C147 == null)
                C147 = new CHARACTER_Enum(52, "C147");
            if (C148 == null)
                C148 = new CHARACTER_Enum(53, "C148");
            if (C149 == null)
                C149 = new CHARACTER_Enum(54, "C149");
            if (C150 == null)
                C150 = new CHARACTER_Enum(55, "C150");
            if (C151 == null)
                C151 = new CHARACTER_Enum(56, "C151");
            if (C152 == null)
                C152 = new CHARACTER_Enum(57, "C152");
            if (C153 == null)
                C153 = new CHARACTER_Enum(58, "C153");
            if (C154 == null)
                C154 = new CHARACTER_Enum(59, "C154");
            if (C155 == null)
                C155 = new CHARACTER_Enum(60, "C155");
            if (C156 == null)
                C156 = new CHARACTER_Enum(61, "C156");
            if (C157 == null)
                C157 = new CHARACTER_Enum(62, "C157");
            if (C158 == null)
                C158 = new CHARACTER_Enum(63, "C158");
            if (C159 == null)
                C159 = new CHARACTER_Enum(64, "C159");
        }
    }

    [Serializable]
    public class CHARACTER : VHDLEnumValue<CHARACTER_Enum>
    {
        public CHARACTER(CHARACTER_Enum Value)
            : base(Value)
        {
            CHARACTER_Enum.Init();
        }

        public CHARACTER()
            : this(CHARACTER_Enum.NUL)
        {
        }

        public static implicit operator CHARACTER(CHARACTER_Enum Value)
        {
            return new CHARACTER(Value);
        }

    }
}
