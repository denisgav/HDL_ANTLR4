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
using VHDLRuntime.Objects;
using VHDLRuntime.MySortedDictionary;
using VHDLRuntime.Values;


namespace VHDLRuntime.ValueDump
{
    /// <summary>
    /// Система сичисления
    /// </summary>
    public enum EnumerationSystem
    {
        Bin,
        Oct,
        Dec,
        Hex,
        ASCII,
        Real
    }
    
    /// <summary>
    /// Класс, используемый для распарсивания VCD формата
    /// </summary>
    public abstract class VCDConvertor
    {
        /// <summary>
        /// Распарсивание переменной BIT в VCD формате
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static VHDLRuntime.Values.BuiltIn.BIT ToBIT_VALUE(string text)
        {
            if (string.IsNullOrEmpty(text) == true)
                throw new Exception("String is null or empty");
            switch (text)
            {
                case "0":
                    return new VHDLRuntime.Values.BuiltIn.BIT(Values.BuiltIn.BIT_Enum.item_0);
                case "1":
                    return new VHDLRuntime.Values.BuiltIn.BIT(Values.BuiltIn.BIT_Enum.item_1);
                default:
                    throw new Exception("Cant parse string");
            }
        }

        public static void Append_BIT_VALUE(string text, Signal Variable, UInt64 CurrentTime)
        {
            Variable.Dump.Append(CurrentTime, new TimeStampInfo<VHDLBaseValue>(ToBIT_VALUE(text)));
        }

        
        /// <summary>
        /// Распарсивание переменной Integer в VCD формате
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static VHDLIntegerValue ToInteger_VALUE(string text)
        {
            if (string.IsNullOrEmpty(text) == true)
                throw new Exception("String is null or empty");
            EnumerationSystem bus = EnumerationSystem.Bin;
            int bus_int = 0;
            switch (text[0])
            {
                case 'b':
                    bus = EnumerationSystem.Bin;
                    bus_int = 2;
                    break;
                case 'o':
                    bus = EnumerationSystem.Oct;
                    bus_int = 8;
                    break;
                case 'd':
                    bus = EnumerationSystem.Dec;
                    bus_int = 10;
                    break;
                case 'h':
                    bus = EnumerationSystem.Hex;
                    bus_int = 16;
                    break;
                default:
                    throw new Exception("Invalid enumeration system");
            }

            int pow = 1;
            int res = 0;
            for (int i = text.Length - 1; i >= 1; i--)
            {
                int num = CharToInt(text[i], bus);
                res += num * pow;
                pow *= bus_int;
            }
            return new VHDLIntegerValue(res);
        }

        public static void Append_Integer_VALUE(string text, Signal Variable, UInt64 CurrentTime)
        {
            Variable.Dump.Append(CurrentTime, new TimeStampInfo<VHDLBaseValue>(ToInteger_VALUE(text)));
        }

        /// <summary>
        /// Распарсивание переменной Real в VCD формате
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static VHDLFloatingPointValue ToReal_VALUE(string text)
        {
            if (string.IsNullOrEmpty(text) == true)
                throw new Exception("String is null or empty");
            if (text[0] == 'r')
                throw new Exception("Invalid format");
            return new VHDLFloatingPointValue(
                Double.Parse(
                    text.Substring(1), 
                       System.Globalization.NumberStyles.Float, 
                       System.Globalization.CultureInfo.InvariantCulture));
        }

        public static void Append_Real_VALUE(string text, Signal Variable, UInt64 CurrentTime)
        {
            Variable.Dump.Append(CurrentTime, new TimeStampInfo<VHDLBaseValue>(ToReal_VALUE(text)));
        }

        /// <summary>
        /// Перевод одного символа в число
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="bus"></param>
        /// <returns></returns>
        public static Int32 CharToInt(char symbol, EnumerationSystem bus)
        {
            Int32 res = 0;
            switch (symbol)
            {
                case '0':
                    res = 0;
                    break;
                case '1':
                    res = 1;
                    break;
                case '2':
                    res = 2;
                    break;
                case '3':
                    res = 3;
                    break;
                case '4':
                    res = 4;
                    break;
                case '5':
                    res = 5;
                    break;
                case '6':
                    res = 6;
                    break;
                case '7':
                    res = 7;
                    break;
                case '8':
                    res = 8;
                    break;
                case '9':
                    res = 9;
                    break;
                case 'A':
                    res = 10;
                    break;
                case 'B':
                    res = 11;
                    break;
                case 'C':
                    res = 12;
                    break;
                case 'D':
                    res = 13;
                    break;
                case 'E':
                    res = 14;
                    break;
                case 'F':
                    res = 15;
                    break;
                default:
                    throw new Exception("Invalid character");
            }

            switch (bus)
            {
                case EnumerationSystem.Bin:
                    if ((res == 0) || (res == 1))
                        return res;
                    break;
                case EnumerationSystem.Oct:
                    if ((res >= 0) && (res <= 7))
                        return res;
                    break;
                case EnumerationSystem.Dec:
                    if ((res >= 0) && (res <= 9))
                        return res;
                    break;
                case EnumerationSystem.Hex:
                    if ((res >= 0) && (res <= 15))
                        return res;
                    break;
            }

            throw new Exception("Parsing failed");
        }

        

        /// <summary>
        /// Преобразование BIT_VALUE в строку
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToVCDString(VHDLRuntime.Values.BuiltIn.BIT value, string identifier)
        {
            return string.Format("{0}{1}", value, identifier);
        }

        /// <summary>
        /// Преобразование IntegerValue в строку
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToVCDString(VHDLIntegerValue value, string identifier)
        {
            return string.Format("b{0} {1}", Convert.ToString(value.Value, 2), identifier);
        }

        /// <summary>
        /// Преобразование RealValue в строку
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToVCDString(VHDLFloatingPointValue value, string identifier)
        {
            return string.Format("{0}{1}", value.Value, identifier);
        }

        

        /// <summary>
        /// Преобразование AbstractValue в строку
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToVCDString(VHDLBaseValue value, string identifier)
        {
            if (value is VHDLRuntime.Values.BuiltIn.BIT)
                return ToVCDString(value as VHDLRuntime.Values.BuiltIn.BIT, identifier);
            if (value is VHDLIntegerValue)
                return ToVCDString(value as VHDLIntegerValue, identifier);
            if (value is VHDLFloatingPointValue)
                return ToVCDString(value as VHDLFloatingPointValue, identifier);
            return string.Empty;
        }
    }
}
