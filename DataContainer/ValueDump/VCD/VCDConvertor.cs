using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataContainer.Value;
using DataContainer.Objects;
using DataContainer.SignalDump;

namespace DataContainer.ValueDump
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
        /// Распарсивание переменной STD_ULOGIC в VCD формате
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static STD_ULOGIC_VALUE ToSTD_ULOGIC_VALUE(string text)
        {
            if (string.IsNullOrEmpty(text) == true)
                throw new Exception("String is null or empty");
            if (text.Length != 1)
                throw new Exception("Invalid length of string");
            switch (text[0])
            {
                case 'U':
                    return new STD_ULOGIC_VALUE(VHDL.builtin.StdLogic1164.STD_ULOGIC_U);
                case 'X':
                    return new STD_ULOGIC_VALUE(VHDL.builtin.StdLogic1164.STD_ULOGIC_X);
                case '0':
                    return new STD_ULOGIC_VALUE(VHDL.builtin.StdLogic1164.STD_ULOGIC_0);
                case '1':
                    return new STD_ULOGIC_VALUE(VHDL.builtin.StdLogic1164.STD_ULOGIC_1);
                case 'Z':
                    return new STD_ULOGIC_VALUE(VHDL.builtin.StdLogic1164.STD_ULOGIC_Z);
                case 'W':
                    return new STD_ULOGIC_VALUE(VHDL.builtin.StdLogic1164.STD_ULOGIC_W);
                case 'L':
                    return new STD_ULOGIC_VALUE(VHDL.builtin.StdLogic1164.STD_ULOGIC_L);
                case 'H':
                    return new STD_ULOGIC_VALUE(VHDL.builtin.StdLogic1164.STD_ULOGIC_H);
                default:
                    throw new Exception("Cant parse string");
            }
        }

        public static void Append_STD_ULOGIC_VALUE(string text, Signal Variable, UInt64 CurrentTime)
        {
            AbstractSimpleSignalDump<VHDL.literal.EnumerationLiteral> dump = Variable.Dump as AbstractSimpleSignalDump<VHDL.literal.EnumerationLiteral>;

            if (string.IsNullOrEmpty(text) == true)
                throw new Exception("String is null or empty");
            if (text.Length != 1)
                throw new Exception("Invalid length of string");
            switch (text[0])
            {
                case 'U':
                    dump.AppendValue(CurrentTime, VHDL.builtin.StdLogic1164.STD_ULOGIC_U);
                    break;
                case 'X':
                    dump.AppendValue(CurrentTime, VHDL.builtin.StdLogic1164.STD_ULOGIC_X);
                    break;
                case '0':
                    dump.AppendValue(CurrentTime, VHDL.builtin.StdLogic1164.STD_ULOGIC_0);
                    break;
                case '1':
                    dump.AppendValue(CurrentTime, VHDL.builtin.StdLogic1164.STD_ULOGIC_1);
                    break;
                case 'Z':
                    dump.AppendValue(CurrentTime, VHDL.builtin.StdLogic1164.STD_ULOGIC_Z);
                    break;
                case 'W':
                    dump.AppendValue(CurrentTime, VHDL.builtin.StdLogic1164.STD_ULOGIC_W);
                    break;
                case 'L':
                    dump.AppendValue(CurrentTime, VHDL.builtin.StdLogic1164.STD_ULOGIC_L);
                    break;
                case 'H':
                    dump.AppendValue(CurrentTime, VHDL.builtin.StdLogic1164.STD_ULOGIC_H);
                    break;
                default:
                    throw new Exception("Cant parse string");
            }
        }

        /// <summary>
        /// Распарсивание переменной STD_ULOGIC_VECTOR в VCD формате
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static STD_ULOGIC_VECTOR_VALUE ToSTD_ULOGIC_VECTOR_VALUE(string text)
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

            List<AbstractValue> res = new List<AbstractValue>();
            string value = text.Substring(1);
            switch (bus)
            {
                case EnumerationSystem.Bin:
                    {
                        foreach (char ch in value)
                        {
                            res.Add(ToSTD_ULOGIC_VALUE(new string(new char[] { ch })));
                        }
                    }
                    break;
                case EnumerationSystem.Dec:
                case EnumerationSystem.Oct:
                case EnumerationSystem.Hex:
                    {
                        int pow = 1;
                        int int_res = 0;
                        for (int i = text.Length - 1; i >= 1; i--)
                        {
                            int num = CharToInt(text[i], bus);
                            int_res += num * pow;
                            pow *= bus_int;
                        }
                        while (int_res != 0)
                        {
                            if ((int_res % 2) == 1)
                                res.Add(new STD_ULOGIC_VALUE(VHDL.builtin.StdLogic1164.STD_ULOGIC_1));
                            else
                                res.Add(new STD_ULOGIC_VALUE(VHDL.builtin.StdLogic1164.STD_ULOGIC_0));
                            int_res /= 2;
                        }
                        res.Reverse();
                    }
                    break;
            }

            return new STD_ULOGIC_VECTOR_VALUE(res);
        }

        public static void Append_STD_ULOGIC_VECTOR_VALUE(string text, Signal Variable, UInt64 CurrentTime)
        {
            SignalScopeDump dump = Variable.Dump as SignalScopeDump;

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

            string value = text.Substring(1);
            switch (bus)
            {
                case EnumerationSystem.Bin:
                    {
                        for (int i=0; i<value.Length; i++)
                        {
                            char ch = value[i];
                            AbstractSimpleSignalDump<VHDL.literal.EnumerationLiteral> cur_dump = (dump.Dumps[i] as AbstractSimpleSignalDump<VHDL.literal.EnumerationLiteral>);

                            switch (ch)
                            {
                                case 'U':
                                    cur_dump.AppendValue(CurrentTime, VHDL.builtin.StdLogic1164.STD_ULOGIC_U);
                                    break;
                                case 'X':
                                    cur_dump.AppendValue(CurrentTime, VHDL.builtin.StdLogic1164.STD_ULOGIC_X);
                                    break;
                                case '0':
                                    cur_dump.AppendValue(CurrentTime, VHDL.builtin.StdLogic1164.STD_ULOGIC_0);
                                    break;
                                case '1':
                                    cur_dump.AppendValue(CurrentTime, VHDL.builtin.StdLogic1164.STD_ULOGIC_1);
                                    break;
                                case 'Z':
                                    cur_dump.AppendValue(CurrentTime, VHDL.builtin.StdLogic1164.STD_ULOGIC_Z);
                                    break;
                                case 'W':
                                    cur_dump.AppendValue(CurrentTime, VHDL.builtin.StdLogic1164.STD_ULOGIC_W);
                                    break;
                                case 'L':
                                    cur_dump.AppendValue(CurrentTime, VHDL.builtin.StdLogic1164.STD_ULOGIC_L);
                                    break;
                                case 'H':
                                    cur_dump.AppendValue(CurrentTime, VHDL.builtin.StdLogic1164.STD_ULOGIC_H);
                                    break;
                                default:
                                    throw new Exception("Cant parse string");
                            }
                        }
                    }
                    break;
                case EnumerationSystem.Dec:
                case EnumerationSystem.Oct:
                case EnumerationSystem.Hex:
                    {
                        int pow = 1;
                        int int_res = 0;
                        for (int i = text.Length - 1; i >= 1; i--)
                        {
                            int num = CharToInt(text[i], bus);
                            int_res += num * pow;
                            pow *= bus_int;
                        }
                        bool[] res = new bool[dump.Dumps.Count];
                        int index = 0;
                        while (int_res != 0)
                        {
                            res[index] = (int_res % 2) == 1;
                            index++;
                            int_res /= 2;
                        }
                        res.Reverse();
                        for (int i = 0; i < res.Length; i++)
                        {
                            using(AbstractValue val = STD_ULOGIC_VALUE.Create_STD_ULOGIC_VALUE(res[i]))
                            {
                                (dump.Dumps[i] as AbstractSimpleSignalDump<VHDL.literal.EnumerationLiteral>).AppendValue(CurrentTime, res[i] ? VHDL.builtin.StdLogic1164.STD_ULOGIC_1 : VHDL.builtin.StdLogic1164.STD_ULOGIC_0);
                            }
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Распарсивание переменной BIT_VECTOR в VCD формате
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static BIT_VECTOR_VALUE ToBIT_VECTOR_VALUE(string text)
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

            List<AbstractValue> res = new List<AbstractValue>();
            string value = text.Substring(1);
            switch (bus)
            {
                case EnumerationSystem.Bin:
                    {
                        foreach (char ch in value)
                        {
                            res.Add(ToBIT_VALUE(new string(new char[] { ch })));
                        }
                    }
                    break;
                case EnumerationSystem.Dec:
                case EnumerationSystem.Oct:
                case EnumerationSystem.Hex:
                    {
                        int pow = 1;
                        int int_res = 0;
                        for (int i = text.Length - 1; i >= 1; i--)
                        {
                            int num = CharToInt(text[i], bus);
                            int_res += num * pow;
                            pow *= bus_int;
                        }
                        while (int_res != 0)
                        {
                            if ((int_res % 2) == 1)
                                res.Add(new BIT_VALUE(VHDL.builtin.Standard.BIT_1));
                            else
                                res.Add(new BIT_VALUE(VHDL.builtin.Standard.BIT_0));
                            int_res /= 2;
                        }
                        res.Reverse();
                    }
                    break;
            }

            return new BIT_VECTOR_VALUE(res);
        }

        public static void Append_BIT_VECTOR_VALUE(string text, Signal Variable, UInt64 CurrentTime)
        {
            SignalScopeDump dump = Variable.Dump as SignalScopeDump;

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

            string value = text.Substring(1);
            switch (bus)
            {
                case EnumerationSystem.Bin:
                    {
                        for (int i = 0; i < value.Length; i++)
                        {
                            char ch = value[i];
                            dump.Dumps[i].AppendValue(CurrentTime, ToBIT_VALUE(new string(new char[] { ch })));
                        }
                    }
                    break;
                case EnumerationSystem.Dec:
                case EnumerationSystem.Oct:
                case EnumerationSystem.Hex:
                    {
                        int pow = 1;
                        int int_res = 0;
                        for (int i = text.Length - 1; i >= 1; i--)
                        {
                            int num = CharToInt(text[i], bus);
                            int_res += num * pow;
                            pow *= bus_int;
                        }
                        bool[] res = new bool[dump.Dumps.Count];
                        int index = 0;
                        while (int_res != 0)
                        {
                            res[index] = (int_res % 2) == 1;
                            index++;
                            int_res /= 2;
                        }
                        res.Reverse();
                        for (int i = 0; i < res.Length; i++)
                        {
                            dump.Dumps[i].AppendValue(CurrentTime, BIT_VALUE.Create_BIT_VALUE(res[i]));
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Распарсивание переменной BIT в VCD формате
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static BIT_VALUE ToBIT_VALUE(string text)
        {
            if (string.IsNullOrEmpty(text) == true)
                throw new Exception("String is null or empty");
            switch (text)
            {
                case "0":
                    return new BIT_VALUE(VHDL.builtin.Standard.BIT_0);
                case "1":
                    return new BIT_VALUE(VHDL.builtin.Standard.BIT_1);
                default:
                    throw new Exception("Cant parse string");
            }
        }

        public static void Append_BIT_VALUE(string text, Signal Variable, UInt64 CurrentTime)
        {
            AbstractSimpleSignalDump<VHDL.literal.EnumerationLiteral> dump = Variable.Dump as AbstractSimpleSignalDump<VHDL.literal.EnumerationLiteral>;
            if (string.IsNullOrEmpty(text) == true)
                throw new Exception("String is null or empty");
            switch (text)
            {
                case "0":
                    dump.AppendValue(CurrentTime, VHDL.builtin.Standard.BIT_0);
                    break;
                case "1":
                    dump.AppendValue(CurrentTime, VHDL.builtin.Standard.BIT_1);
                    break;
                default:
                    throw new Exception("Cant parse string");
            }
        }

        /// <summary>
        /// Распарсивание переменной BOOLEAN в VCD формате
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static BOOLEAN_VALUE ToBOOLEAN_VALUE(string text)
        {
            if (string.IsNullOrEmpty(text) == true)
                throw new Exception("String is null or empty");
            switch (text)
            {
                case "0":
                    return new BOOLEAN_VALUE(VHDL.builtin.Standard.BOOLEAN_FALSE);
                case "1":
                    return new BOOLEAN_VALUE(VHDL.builtin.Standard.BOOLEAN_TRUE);
                default:
                    throw new Exception("Cant parse string");
            }
        }

        public static void Append_BOOLEAN_VALUE(string text, Signal Variable, UInt64 CurrentTime)
        {
            using (AbstractValue val = ToBOOLEAN_VALUE(text))
            {
                Variable.AppendValue(CurrentTime, val);
            }
        }

        /// <summary>
        /// Распарсивание переменной CHARACTER в VCD формате
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static CHARACTER_VALUE ToCHARACTER_VALUE(string text)
        {
            if (string.IsNullOrEmpty(text) == true)
                throw new Exception("String is null or empty");
            if (text.Length != 1)
                throw new Exception("Invalid length of string");
            foreach (var v in VHDL.builtin.Standard.CHARACTER.Literals)
                if (v is VHDL.type.EnumerationType.CharacterEnumerationLiteral)
                    if (((VHDL.type.EnumerationType.CharacterEnumerationLiteral)v).getLiteral() == text[0])
                        return new CHARACTER_VALUE(v as VHDL.type.EnumerationType.CharacterEnumerationLiteral);
            throw new Exception("Parse Error");
        }

        public static void Append_CHARACTER_VALUE(string text, Signal Variable, UInt64 CurrentTime)
        {
            using(AbstractValue val = ToCHARACTER_VALUE(text))
            {
                Variable.AppendValue(CurrentTime, val);
            }
        }

        /// <summary>
        /// Распарсивание переменной Integer в VCD формате
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static IntegerValue ToInteger_VALUE(string text)
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
            return new IntegerValue(VHDL.builtin.Standard.INTEGER, res);
        }

        public static void Append_Integer_VALUE(string text, Signal Variable, UInt64 CurrentTime)
        {
            AbstractSimpleSignalDump<int> dump = Variable.Dump as AbstractSimpleSignalDump<int>;
            if (string.IsNullOrEmpty(text) == true)
                throw new Exception("String is null or empty");

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

            dump.AppendValue(CurrentTime, res);
        }

        /// <summary>
        /// Распарсивание переменной Real в VCD формате
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static RealValue ToReal_VALUE(string text)
        {
            if (string.IsNullOrEmpty(text) == true)
                throw new Exception("String is null or empty");
            if (text[0] == 'r')
                throw new Exception("Invalid format");
            return new RealValue(
                VHDL.builtin.Standard.REAL, 
                Double.Parse(
                    text.Substring(1), 
                       System.Globalization.NumberStyles.Float, 
                       System.Globalization.CultureInfo.InvariantCulture));
        }

        public static void Append_Real_VALUE(string text, Signal Variable, UInt64 CurrentTime)
        {
            using (AbstractValue val = ToReal_VALUE(text))
            {
                Variable.AppendValue(CurrentTime, val);
            }
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
        /// Преобразование STD_ULOGIC_VALUE в строку
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToVCDString(STD_ULOGIC_VALUE value, string identifier)
        {
            return string.Format("{0}{1}", (value.Value as VHDL.type.EnumerationType.CharacterEnumerationLiteral).getLiteral(), identifier);
        }

        /// <summary>
        /// Преобразование BIT_VALUE в строку
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToVCDString(BIT_VALUE value, string identifier)
        {
            return string.Format("{0}{1}", (value.Value as VHDL.type.EnumerationType.CharacterEnumerationLiteral).getLiteral(), identifier);
        }

        /// <summary>
        /// Преобразование BOOLEAN_VALUE в строку
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToVCDString(BOOLEAN_VALUE value, string identifier)
        {
            return string.Format("{0}{1}", (value.Value == VHDL.builtin.Standard.BOOLEAN_TRUE)?'1':'0', identifier);
        }

        /// <summary>
        /// Преобразование IntegerValue в строку
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToVCDString(IntegerValue value, string identifier)
        {
            return string.Format("b{0} {1}", Convert.ToString(value.Value, 2), identifier);
        }

        /// <summary>
        /// Преобразование RealValue в строку
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToVCDString(RealValue value, string identifier)
        {
            return string.Format("{0}{1}", value.Value, identifier);
        }

        /// <summary>
        /// Преобразование STD_ULOGIC_VECTOR_VALUE в строку
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToVCDString(STD_ULOGIC_VECTOR_VALUE value, string identifier)
        {
            StringBuilder res = new StringBuilder();
            foreach (var v in value)
            {
                res.Append(((v.Value as STD_ULOGIC_VALUE).Value as VHDL.type.EnumerationType.CharacterEnumerationLiteral).getLiteral());
            }
            return string.Format("b{0} {1}", res, identifier);
        }

        /// <summary>
        /// Преобразование BIT_VECTOR_VALUE в строку
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToVCDString(BIT_VECTOR_VALUE value, string identifier)
        {
            StringBuilder res = new StringBuilder();
            foreach (var v in value)
            {
                res.Append(((v.Value as BIT_VALUE).Value as VHDL.type.EnumerationType.CharacterEnumerationLiteral).getLiteral());
            }
            return string.Format("b{0} {1}", res, identifier);
        }

        /// <summary>
        /// Преобразование AbstractValue в строку
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToVCDString(AbstractValue value, string identifier)
        {
            if (value is STD_ULOGIC_VALUE)
                return ToVCDString(value as STD_ULOGIC_VALUE, identifier);
            if (value is BIT_VALUE)
                return ToVCDString(value as BIT_VALUE, identifier);
            if (value is BOOLEAN_VALUE)
                return ToVCDString(value as BOOLEAN_VALUE, identifier);
            if (value is IntegerValue)
                return ToVCDString(value as IntegerValue, identifier);
            if (value is RealValue)
                return ToVCDString(value as RealValue, identifier);
            if (value is STD_ULOGIC_VECTOR_VALUE)
                return ToVCDString(value as STD_ULOGIC_VECTOR_VALUE, identifier);
            if (value is BIT_VECTOR_VALUE)
                return ToVCDString(value as BIT_VECTOR_VALUE, identifier);
            return string.Empty;
        }
    }
}
