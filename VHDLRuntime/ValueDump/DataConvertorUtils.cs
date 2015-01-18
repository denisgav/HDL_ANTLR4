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
using System.Numerics;
using VHDLRuntime.MySortedDictionary;

namespace VHDLRuntime.ValueDump
{
    /// <summary>
    /// Класс, используемый для преобразования данных
    /// </summary>
    public abstract class DataConvertorUtils
    {
        /// <summary>
        /// Функция для отображения ASCII кодов
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ASCII_Encoder(byte data)
        {
            switch (data)
            {
                case 000: return " NUL "; 	//Null character
                case 001: return " SOH "; 	//Start of header
                case 002: return " STX "; 	//Start of text
                case 003: return " ETX "; 	//End of text
                case 004: return " EOT "; 	//End of transmission
                case 005: return " ENQ "; 	//Enquiry
                case 006: return " ACK "; 	//Acknowledgement
                case 007: return " BEL "; 	//Bell
                case 008: return " BS "; 	//Backspace
                case 009: return " HT "; 	//Horizontal tab
                case 010: return " LF "; 	//Line feed
                case 011: return " VT "; 	//Vertical tab
                case 012: return " FF "; 	//Form feed
                case 013: return " CR "; 	//Carriage return
                case 014: return " SO "; 	//Shift out
                case 015: return " SI "; 	//Shift in
                case 016: return " DLE "; 	//Data link escape
                case 017: return " DC1 "; 	//Device control 1/XON
                case 018: return " DC2 "; 	//Device control 2
                case 019: return " DC3 "; 	//Device control 3/XOFF
                case 020: return " DC4 "; 	//Device control 4
                case 021: return " NAK "; 	//Negative acknowledgement
                case 022: return " SYN "; 	//Synchronous idle
                case 023: return " ETB "; 	//End of transmission block
                case 024: return " CAN "; 	//Cancel
                case 025: return " EM "; 	//End of medium
                case 026: return " SUB "; 	//Substitute
                case 027: return " ESC "; 	//Escape
                case 028: return " FS "; 	//File separator
                case 029: return " GS "; 	//Group separator
                case 030: return " RS "; 	//Request to send/Record separator
                case 031: return " US "; 	//Unit separator
                case 032: return " SP "; 	//Space
                case 033: return " ! "; 	//Exclamation mark
                case 034: return " \" "; 	//Double quote
                case 035: return " # "; 	//Number sign
                case 036: return " $ "; 	//Dollar sign
                case 037: return " % "; 	//Percent sign
                case 038: return " & "; 	//Ampersand
                case 039: return " ' "; 	//Single quote
                case 040: return " ( "; 	//Left parenthesis
                case 041: return " ) "; 	//Right parenthesis
                case 042: return " * "; 	//Asterix
                case 043: return " + "; 	//Plus
                case 044: return " , "; 	//Comma
                case 045: return " - "; 	//Minus/Dash
                case 046: return " . "; 	//Dot/Period
                case 047: return " / "; 	//Forward slash
                case 048: return " 0 "; 	//Zero
                case 049: return " 1 "; 	//One
                case 050: return " 2 "; 	//Two
                case 051: return " 3 "; 	//Three
                case 052: return " 4 "; 	//Four
                case 053: return " 5 "; 	//Five
                case 054: return " 6 "; 	//Six
                case 055: return " 7 "; 	//Seven
                case 056: return " 8 "; 	//Eight
                case 057: return " 9 "; 	//Nine
                case 058: return " : "; 	//Colon
                case 059: return " ; "; 	//Semi-colon
                case 060: return " < "; 	//Less than
                case 061: return " = "; 	//Equals
                case 062: return " > "; 	//Greater than
                case 063: return " ? "; 	//Question mark
                case 064: return " @ "; 	//At symbol
                case 065: return " A ";
                case 066: return " B ";
                case 067: return " C ";
                case 068: return " D ";
                case 069: return " E ";
                case 070: return " F ";
                case 071: return " G ";
                case 072: return " H ";
                case 073: return " I ";
                case 074: return " J ";
                case 075: return " K ";
                case 076: return " L ";
                case 077: return " M ";
                case 078: return " N ";
                case 079: return " O ";
                case 080: return " P ";
                case 081: return " Q ";
                case 082: return " R ";
                case 083: return " S ";
                case 084: return " T ";
                case 085: return " U ";
                case 086: return " V ";
                case 087: return " W ";
                case 088: return " X ";
                case 089: return " Y ";
                case 090: return " Z ";
                case 091: return " [ "; 	//Left bracket
                case 092: return " \\ "; 	//Back slash
                case 093: return " ] "; 	//Right bracket
                case 094: return " ^ "; 	//Caret/Cirumflex
                case 095: return " _ "; 	//Underscore
                case 096: return " ` ";
                case 097: return " a ";
                case 098: return " b ";
                case 099: return " c ";
                case 100: return " d ";
                case 101: return " e ";
                case 102: return " f ";
                case 103: return " g ";
                case 104: return " h ";
                case 105: return " i ";
                case 106: return " j ";
                case 107: return " k ";
                case 108: return " l ";
                case 109: return " m ";
                case 110: return " n ";
                case 111: return " o ";
                case 112: return " p ";
                case 113: return " q ";
                case 114: return " r ";
                case 115: return " s ";
                case 116: return " t ";
                case 117: return " u ";
                case 118: return " v ";
                case 119: return " w ";
                case 120: return " x ";
                case 121: return " y ";
                case 122: return " z ";
                case 123: return " { "; 	//Left brace
                case 124: return " | "; 	//Vertical bar
                case 125: return " } "; 	//Right brace
                case 126: return " ~ "; 	//Tilde
                case 127: return " DEL "; 	//Delete

                //Extended ASCII Codes

                case 128: return " € ";	   //Euro sign
                case 129: return "   ";	   //
                case 130: return " ‚ ";	   //Single low-9 quotation mark
                case 131: return " ? ";	   //Latin small letter f with hook
                case 132: return " „ ";	   //Double low-9 quotation mark
                case 133: return " … ";	   //Horizontal ellipsis
                case 134: return " † ";	   //Dagger
                case 135: return " ‡ ";	   //Double dagger
                case 136: return " ? ";	   //Modifier letter circumflex accent
                case 137: return " ‰ ";	   //Per mille sign
                case 138: return " S ";	   //Latin capital letter S with caron
                case 139: return " ‹ ";	   //Single left-pointing angle quotation
                case 140: return " ? ";	   //Latin capital ligature OE
                case 141: return "   ";	   //
                case 142: return " Z ";	   //Latin captial letter Z with caron
                case 143: return "   ";	   //
                case 144: return "   ";	   //
                case 145: return " ‘ ";	   //Left single quotation mark
                case 146: return " ’ ";	   //Right single quotation mark
                case 147: return " “ ";	   //Left double quotation mark
                case 148: return " ” ";	   //Right double quotation mark
                case 149: return " • ";	   //Bullet
                case 150: return " – ";	   //En dash
                case 151: return " — ";	   //Em dash
                case 152: return " ? ";	   //Small tilde
                case 153: return " ™ ";	   //Trade mark sign
                case 154: return " s ";	   //Latin small letter S with caron
                case 155: return " › ";	   //Single right-pointing angle quotation mark
                case 156: return " ? ";	   //Latin small ligature oe
                case 157: return "   ";	   //
                case 158: return " z ";	   //Latin small letter z with caron
                case 159: return " Y ";	   //Latin capital letter Y with diaeresis
                case 160: return "   ";	   //Non-breaking space
                case 161: return " ? ";	   //Inverted exclamation mark
                case 162: return " ? ";	   //Cent sign
                case 163: return " ? ";	   //Pound sign
                case 164: return " ¤ ";	   //Currency sign
                case 165: return " ? ";	   //Yen sign
                case 166: return " ¦ ";	   //Pipe, Broken vertical bar
                case 167: return " § ";	   //Section sign
                case 168: return " ? ";	   //Spacing diaeresis - umlaut
                case 169: return " © ";	   //Copyright sign
                case 170: return " ? ";	   //Feminine ordinal indicator
                case 171: return " « ";	   //Left double angle quotes
                case 172: return " ¬ ";	   //Not sign
                case 173: return " ";	   //Soft hyphen
                case 174: return " ® ";	   //Registered trade mark sign
                case 175: return " ? ";	   //Spacing macron - overline
                case 176: return " ° ";	   //Degree sign
                case 177: return " ± ";	   //Plus-or-minus sign
                case 178: return " ? ";	   //Superscript two - squared
                case 179: return " ? ";	   //Superscript three - cubed
                case 180: return " ? ";	   //Acute accent - spacing acute
                case 181: return " µ ";	   //Micro sign
                case 182: return " ¶ ";	   //Pilcrow sign - paragraph sign
                case 183: return " · ";	   //Middle dot - Georgian comma
                case 184: return " ? ";	   //Spacing cedilla
                case 185: return " ? ";	   //Superscript one
                case 186: return " ? ";	   //Masculine ordinal indicator
                case 187: return " » ";	   //Right double angle quotes
                case 188: return " ? ";	   //Fraction one quarter
                case 189: return " ? ";	   //Fraction one half
                case 190: return " ? ";	   //Fraction three quarters
                case 191: return " ? ";	   //Inverted question mark
                case 192: return " A ";	   //Latin capital letter A with grave
                case 193: return " A ";	   //Latin capital letter A with acute
                case 194: return " A ";	   //Latin capital letter A with circumflex
                case 195: return " A ";	   //Latin capital letter A with tilde
                case 196: return " A ";	   //Latin capital letter A with diaeresis
                case 197: return " A ";	   //Latin capital letter A with ring above
                case 198: return " ? ";	   //Latin capital letter AE
                case 199: return " C ";	   //Latin capital letter C with cedilla
                case 200: return " E ";	   //Latin capital letter E with grave
                case 201: return " E ";	   //Latin capital letter E with acute
                case 202: return " E ";	   //Latin capital letter E with circumflex
                case 203: return " E ";	   //Latin capital letter E with diaeresis
                case 204: return " I ";	   //Latin capital letter I with grave
                case 205: return " I ";	   //Latin capital letter I with acute
                case 206: return " I ";	   //Latin capital letter I with circumflex
                case 207: return " I ";	   //Latin capital letter I with diaeresis
                case 208: return " ? ";	   //Latin capital letter ETH
                case 209: return " N ";	   //Latin capital letter N with tilde
                case 210: return " O ";	   //Latin capital letter O with grave
                case 211: return " O ";	   //Latin capital letter O with acute
                case 212: return " O ";	   //Latin capital letter O with circumflex
                case 213: return " O ";	   //Latin capital letter O with tilde
                case 214: return " O ";	   //Latin capital letter O with diaeresis
                case 215: return " ? ";	   //Multiplication sign
                case 216: return " O ";	   //Latin capital letter O with slash
                case 217: return " U ";	   //Latin capital letter U with grave
                case 218: return " U ";	   //Latin capital letter U with acute
                case 219: return " U ";	   //Latin capital letter U with circumflex
                case 220: return " U ";	   //Latin capital letter U with diaeresis
                case 221: return " Y ";	   //Latin capital letter Y with acute
                case 222: return " ? ";	   //Latin capital letter THORN
                case 223: return " ? ";	   //Latin small letter sharp s - ess-zed
                case 224: return " a ";	   //Latin small letter a with grave
                case 225: return " a ";	   //Latin small letter a with acute
                case 226: return " a ";	   //Latin small letter a with circumflex
                case 227: return " a ";	   //Latin small letter a with tilde
                case 228: return " a ";	   //Latin small letter a with diaeresis
                case 229: return " a ";	   //Latin small letter a with ring above
                case 230: return " ? ";	   //Latin small letter ae
                case 231: return " c ";	   //Latin small letter c with cedilla
                case 232: return " e ";	   //Latin small letter e with grave
                case 233: return " e ";	   //Latin small letter e with acute
                case 234: return " e ";	   //Latin small letter e with circumflex
                case 235: return " e ";	   //Latin small letter e with diaeresis
                case 236: return " i ";	   //Latin small letter i with grave
                case 237: return " i ";	   //Latin small letter i with acute
                case 238: return " i ";	   //Latin small letter i with circumflex
                case 239: return " i ";	   //Latin small letter i with diaeresis
                case 240: return " ? ";	   //Latin small letter eth
                case 241: return " n ";	   //Latin small letter n with tilde
                case 242: return " o ";	   //Latin small letter o with grave
                case 243: return " o ";	   //Latin small letter o with acute
                case 244: return " o ";	   //Latin small letter o with circumflex
                case 245: return " o ";	   //Latin small letter o with tilde
                case 246: return " o ";	   //Latin small letter o with diaeresis
                case 247: return " ? ";	   //Division sign
                case 248: return " o ";	   //Latin small letter o with slash
                case 249: return " u ";	   //Latin small letter u with grave
                case 250: return " u ";	   //Latin small letter u with acute
                case 251: return " u ";	   //Latin small letter u with circumflex
                case 252: return " u ";	   //Latin small letter u with diaeresis
                case 253: return " y ";	   //Latin small letter y with acute
                case 254: return " ? ";	   //Latin small letter thorn
                case 255: return " y ";	   //Latin small letter y with diaeresis

                default: return " null ";
            }
        }

        /// <summary>
        /// Преобразует данные в ASCII строку
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string BinaryToASCII(bool?[] data)
        {
            StringBuilder res = new StringBuilder();
            int index = 0;
            int count = data.Length % 8;
            bool?[] buf;
            int symb;

            if (count != 0)
            {
                buf = new bool?[count];

                Array.Copy(data, index, buf, 0, count);

                symb = ToInt(buf);
                if (symb == -1)
                    return "-";

                res.Append(ASCII_Encoder((byte)symb));

                index += count;
            }

            while (index < data.Length)
            {
                int nextindex = index + 8;
                if (nextindex >= data.Length)
                    nextindex = data.Length;

                count = nextindex - index;

                buf = new bool?[count];

                Array.Copy(data, index, buf, 0, count);

                symb = ToInt(buf);
                if (symb == -1)
                    return "-";

                res.Append(ASCII_Encoder((byte)symb));

                index += 8;
            }

            return res.ToString();
        }

        /// <summary>
        /// Конвертирует значения bool?[] в целое число
        /// если встречает значение null - возвращает -1
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BigInteger ToBigInteger(bool?[] value)
        {
            BigInteger res = 0;
            BigInteger pow = 1;
            for (int i = value.Length - 1; i >= 0; i--, pow *= 2)
            {
                if (value[i] == true)
                    res += pow;
                if (value[i] == null)
                    return -1;
            }
            return res;
        }

        /// <summary>
        /// Конвертирует значения bool?[] в целое число
        /// если встречает значение null - возвращает -1
        /// если разрядность превышает 32 - возвращает максимальное значение int
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(bool?[] value)
        {
            if (value.Length > 32)
                return int.MaxValue;
            int res = 0;
            int pow = 1;
            for (int i = value.Length - 1; i >= 0; i--, pow *= 2)
            {
                if (value[i] == true)
                    res += pow;
                if (value[i] == null)
                    return -1;
            }
            return res;
        }

        /// <summary>
        /// Конвертирует значения bool[] в целое число
        /// если разрядность превышает 32 - возвращает максимальное значение int
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(bool[] value)
        {
            if (value.Length > 32)
                return int.MaxValue;
            int res = 0;
            int pow = 1;
            for (int i = value.Length - 1; i >= 0; i--, pow *= 2)
            {
                if (value[i] == true)
                    res += pow;
            }
            return res;
        }

        /// <summary>
        /// Преобразование в Nullable<BigInteger>  с учетом формата представления данных
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Nullable<BigInteger> ToBigInteger(bool?[] value, VectorDataRepresentation dataRepresentation)
        {
            BigInteger res = 0;
            BigInteger pow = 1;

            //if(dataRepresentation.IsInverted)

            switch (dataRepresentation.DecimalDataPresentation)
            {
                case DecimalDataPresentation.Unsigned:
                    {
                        for (int i = value.Length - 1; i >= 0; i--, pow *= 2)
                        {
                            if (value[i] == true)
                                res += pow;

                            if (value[i] == null)
                                return null;
                        }
                    }
                    break;

                case DecimalDataPresentation.Complement:
                    {
                        if (value[0] == null)
                            return null;
                        if (value[0] == false)
                        {
                            for (int i = value.Length - 1; i >= 1; i--, pow *= 2)
                            {
                                if (value[i] == true)
                                    res += pow;

                                if (value[i] == null)
                                    return null;
                            }
                        }
                        else
                        {
                            for (int i = value.Length - 1; i >= 1; i--, pow *= 2)
                            {
                                if (value[i] != true)
                                    res += pow;

                                if (value[i] == null)
                                    return null;
                            }
                            res = -res;
                        }
                    }
                    break;

                case DecimalDataPresentation.Twos_Сomplement:
                    {
                        if (value[0] == null)
                            return null;

                        if (value[0] == false)
                        {
                            for (int i = value.Length - 1; i >= 1; i--, pow *= 2)
                            {
                                if (value[i] == true)
                                    res += pow;

                                if (value[i] == null)
                                    return null;
                            }
                        }
                        else
                        {
                            pow = -1; bool isFlag = false;
                            for (int i = value.Length - 1; i >= 1; i--, pow *= 2)
                            {
                                if (value[i] == null)
                                    return null;

                                if (value[i].Value ^ isFlag)
                                    res += pow;
                                isFlag |= value[i].Value;
                            }
                            if (!isFlag)
                                res = pow; // for values like 1000000000 .. 00
                        }
                    }
                    break;
            }

            return res;
        }

        /// <summary>
        /// Преобразование целого числа в битовый массив
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool[] ToBitArray(long value, Int64 size)
        {
            bool[] res = new bool[size];

            for (int i = 0; i < size; i++)
            {
                res[i] = (value % 2) == 1;
                value /= 2;
            }

            Array.Reverse(res);

            return res;
        }

        /// <summary>
        /// Перевод одной цифры в число
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

        public static int BoolToInt(bool value)
        {
            return (value) ? 1 : 0;
        }

        public static string BoolToString(bool value)
        {
            return (value) ? "1" : "0";
        }

        /// <summary>
        /// Преобразует целочисленное значение в двоичный вид и выводит в виде строки
        /// </summary>
        /// <param name="IntegerValue"></param>
        /// <param name="vectorSize"></param>
        /// <returns></returns>
        public static string ToBitArrayString(int IntegerValue, uint vectorSize)
        {
            StringBuilder res = new StringBuilder();
            bool[] tmp = DataConvertorUtils.ToBitArray((long)IntegerValue, (int)vectorSize);
            res.Append("\"");
            for (int i = tmp.Length - 1; i >= 0; i--)
            {
                res.Append(BoolToInt(tmp[i]));
            }
            res.Append("\"");
            return res.ToString();
        }

        #region Binary convertion to string

        /// <summary>
        /// Преобразование в строку данных в двоичном виде
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string BinaryToBin(bool?[] data)
        {
            StringBuilder res = new StringBuilder();
            foreach (bool? b in data)
            {
                if (b == null)
                    res.Append('X');
                else
                {
                    if (b == true)
                        res.Append('1');
                    else
                        res.Append('0');
                }
            }
            return res.ToString();
        }

        /// <summary>
        /// Преобразование в строку данных в весьмиричном виде
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string BinaryToOct(bool?[] data)
        {
            StringBuilder res = new StringBuilder();
            int index = 0;
            bool?[] buf;
            int num;
            int count = data.Length % 3;

            if (count != 0)
            {
                buf = new bool?[count];

                Array.Copy(data, index, buf, 0, count);

                num = ToInt(buf);
                if (num == -1)
                    res.Append('X');
                else
                    res.Append(num);

                index += count;
            }

            while (index < data.Length)
            {
                int nextindex = index + 3;
                if (nextindex >= data.Length)
                    nextindex = data.Length;

                count = nextindex - index;

                buf = new bool?[count];

                Array.Copy(data, index, buf, 0, count);

                num = ToInt(buf);
                if (num == -1)
                    res.Append('X');
                else
                    res.Append(num);
                index += 3;
            }
            return res.ToString();
        }

        /// <summary>
        /// Преобразование в строку данных в шестнадцатеричном виде
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string BinaryToHex(bool?[] data)
        {
            StringBuilder res = new StringBuilder();
            int index = 0;
            int count = data.Length % 4;
            bool?[] buf;
            int num;

            if (count != 0)
            {

                buf = new bool?[count];

                Array.Copy(data, index, buf, 0, count);

                num = ToInt(buf);
                switch (num)
                {
                    case -1:
                        res.Append('X');
                        break;
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                        res.Append(num);
                        break;
                    case 10:
                        res.Append('A');
                        break;
                    case 11:
                        res.Append('B');
                        break;
                    case 12:
                        res.Append('C');
                        break;
                    case 13:
                        res.Append('D');
                        break;
                    case 14:
                        res.Append('E');
                        break;
                    case 15:
                        res.Append('F');
                        break;
                }

                index += count;
            }

            while (index < data.Length)
            {
                int nextindex = index + 4;
                if (nextindex >= data.Length)
                    nextindex = data.Length;

                count = nextindex - index;

                buf = new bool?[count];

                Array.Copy(data, index, buf, 0, count);

                num = ToInt(buf);
                switch (num)
                {
                    case -1:
                        res.Append('X');
                        break;
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                        res.Append(num);
                        break;
                    case 10:
                        res.Append('A');
                        break;
                    case 11:
                        res.Append('B');
                        break;
                    case 12:
                        res.Append('C');
                        break;
                    case 13:
                        res.Append('D');
                        break;
                    case 14:
                        res.Append('E');
                        break;
                    case 15:
                        res.Append('F');
                        break;
                }
                index += 4;
            }
            return res.ToString();
        }

        public static string BinaryToDec(bool?[] value)
        {
            return ToBigInteger(value).ToString();
        }

        public static string BinaryToDec(bool?[] value, VectorDataRepresentation dataRepresentation)
        {
            BigInteger? res = ToBigInteger(value, dataRepresentation);
            return (res == null) ? "X" : res.Value.ToString("n0");
        }

        #endregion

        #region ToStringConversion


        /// <summary>
        /// Инвертирование входного массива
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static Nullable<bool>[] Invert(Nullable<bool>[] array)
        {
            bool?[] res = new bool?[array.Length];
            Array.Copy(array, res, array.Length);
            for (int i = 0; i < res.Length; i++)
                if (res[i].HasValue == true)
                    res[i] = !res[i];
            return res;
        }

        

        /// <summary>
        /// Преобразование в строку данных Nullable<bool>[]
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dataRepresentation"></param>
        /// <returns></returns>
        public static string ToString(Nullable<bool>[] data, VectorDataRepresentation dataRepresentation)
        {
            if (dataRepresentation.IsInverted)
                data = Invert(data);

            StringBuilder res = new StringBuilder();

            switch (dataRepresentation.EnumerationSystem)
            {
                case EnumerationSystem.Bin:
                {
                    foreach (Nullable<bool> b in data)
                    {
                        if (b == null)
                            res.Append('X');
                        else
                        {
                            if (b == true)
                                res.Append('1');
                            else
                                res.Append('0');
                        }
                    }
                }
                break;
                case EnumerationSystem.Oct:
                {
                    int index = 0;
                    int count = data.Length % 3;

                    Nullable<bool>[] buf = new bool?[count];

                    Array.Copy(data, index, buf, 0, count);

                    int num = ToInt(buf);
                    if (num == -1)
                        res.Append('X');
                    else
                        res.Append(num);

                    index += count;

                    while (index < data.Length)
                    {
                        int nextindex = index + 3;
                        if (nextindex >= data.Length)
                            nextindex = data.Length;

                        count = nextindex - index;

                        buf = new bool?[count];

                        Array.Copy(data, index, buf, 0, count);

                        num = ToInt(buf);
                        if (num == -1)
                            res.Append('X');
                        else
                            res.Append(num);
                        index += 3;
                    }
                }
                break;
                case EnumerationSystem.Hex:
                {
                    int index = 0;
                    int count = data.Length % 4;

                    Nullable<bool>[] buf = new bool?[count];

                    Array.Copy(data, index, buf, 0, count);

                    int num = ToInt(buf);
                    switch (num)
                    {
                        case -1:
                            res.Append('X');
                            break;
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                            res.Append(num);
                            break;
                        case 10:
                            res.Append('A');
                            break;
                        case 11:
                            res.Append('B');
                            break;
                        case 12:
                            res.Append('C');
                            break;
                        case 13:
                            res.Append('D');
                            break;
                        case 14:
                            res.Append('E');
                            break;
                        case 15:
                            res.Append('F');
                            break;
                    }

                    index += count;

                    while (index < data.Length)
                    {
                        int nextindex = index + 4;
                        if (nextindex >= data.Length)
                            nextindex = data.Length;

                        count = nextindex - index;

                        buf = new Nullable<bool>[count];

                        Array.Copy(data, index, buf, 0, count);

                        num = ToInt(buf);
                        switch (num)
                        {
                            case -1:
                                res.Append('X');
                                break;
                            case 0:
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                            case 5:
                            case 6:
                            case 7:
                            case 8:
                            case 9:
                                res.Append(num);
                                break;
                            case 10:
                                res.Append('A');
                                break;
                            case 11:
                                res.Append('B');
                                break;
                            case 12:
                                res.Append('C');
                                break;
                            case 13:
                                res.Append('D');
                                break;
                            case 14:
                                res.Append('E');
                                break;
                            case 15:
                                res.Append('F');
                                break;
                        }
                        index += 4;
                    }
                }
                break;
                case EnumerationSystem.Dec:
                {
                    int num = ToInt(data);
                    if (num == -1)
                        res.Append('X');
                    else
                        res.Append(num);
                }
                break;
            }

            return res.ToString();
        }

        #endregion

        

        

        /// <summary>
        /// Преобразует значение bool[] в long
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ToLong(bool[] value)
        {
            long res = 0;
            long pow = 1;
            for (int i = value.Length - 1; i >= 0; i--, pow *= 2)
            {
                if (value[i] == true)
                    res += pow;
            }
            return res;
        }
    }
}
