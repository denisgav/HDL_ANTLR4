using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataContainer.Value;

namespace DataContainer.ValueDump
{
    /// <summary>
    /// Класс, который используется при генерации TestBench для языка VHDL
    /// </summary>
    public abstract class VHDLTestBenchGeneratorConvertorsion
    {
        /// <summary>
        /// Преобразование STD_ULOGIC_VALUE в строку
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToTestBenchString(STD_ULOGIC_VALUE value)
        {
            return new string(new char[] { (value.Value as VHDL.type.EnumerationType.CharacterEnumerationLiteral).getLiteral() });
        }

        /// <summary>
        /// Преобразование BIT_VALUE в строку
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToTestBenchString(BIT_VALUE value)
        {
            return new string (new char [] {(value.Value as VHDL.type.EnumerationType.CharacterEnumerationLiteral).getLiteral()});
        }

        /// <summary>
        /// Преобразование BOOLEAN_VALUE в строку
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToTestBenchString(BOOLEAN_VALUE value)
        {
            return new string(new char[] { (value.Value as VHDL.type.EnumerationType.CharacterEnumerationLiteral).getLiteral() });
        }

        /// <summary>
        /// Преобразование IntegerValue в строку
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToTestBenchString(IntegerValue value)
        {
            return Convert.ToString(value.Value, 2);
        }

        /// <summary>
        /// Преобразование RealValue в строку
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToTestBenchString(RealValue value)
        {
            return value.Value.ToString();
        }

        /// <summary>
        /// Преобразование STD_ULOGIC_VECTOR_VALUE в строку
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToTestBenchString(STD_ULOGIC_VECTOR_VALUE value)
        {
            StringBuilder res = new StringBuilder();
            foreach (var v in value)
            {
                res.Append(((v.Value as STD_ULOGIC_VALUE).Value as VHDL.type.EnumerationType.CharacterEnumerationLiteral).getLiteral());
            }
            return res.ToString();
        }

        /// <summary>
        /// Преобразование BIT_VECTOR_VALUE в строку
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToTestBenchString(BIT_VECTOR_VALUE value)
        {
            StringBuilder res = new StringBuilder();
            foreach (var v in value)
            {
                res.Append(((v.Value as BIT_VALUE).Value as VHDL.type.EnumerationType.CharacterEnumerationLiteral).getLiteral());
            }
            return res.ToString();
        }

        /// <summary>
        /// Преобразование AbstractValue в строку
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToTestBenchString(AbstractValue value)
        {
            if (value is STD_ULOGIC_VALUE)
                return ToTestBenchString(value as STD_ULOGIC_VALUE);
            if (value is BIT_VALUE)
                return ToTestBenchString(value as BIT_VALUE);
            if (value is BOOLEAN_VALUE)
                return ToTestBenchString(value as BOOLEAN_VALUE);
            if (value is IntegerValue)
                return ToTestBenchString(value as IntegerValue);
            if (value is RealValue)
                return ToTestBenchString(value as RealValue);
            if (value is STD_ULOGIC_VECTOR_VALUE)
                return ToTestBenchString(value as STD_ULOGIC_VECTOR_VALUE);
            if (value is BIT_VECTOR_VALUE)
                return ToTestBenchString(value as BIT_VECTOR_VALUE);
            return string.Empty;
        }
    }
}