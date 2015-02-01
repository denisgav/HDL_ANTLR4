using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VHDL.util;

namespace VHDL
{
    //Extension methods must be defined in a static class
    public static class StringExtensions
    {
        /// <summary>
        /// Determines whether two VHDL identifiers are equal.
        /// </summary>
        /// <param name="id1"></param>
        /// <param name="id2"></param>
        /// <returns></returns>
        public static bool EqualsIdentifier(this string id1, string id2)
        {
            if (string.IsNullOrEmpty(id1))
                throw new ArgumentException("Identifier1 is empty");
            if (string.IsNullOrEmpty(id2))
                throw new ArgumentException("Identifier2 is empty");

            if (id1[0] == '\\')
                return id1.Equals(id2);
            else
                return id1.EqualsIgnoreCase(id2);
        }

        /// <summary>
        /// Determines whether opening and closing labels of the begin/end block are equals.
        /// </summary>
        /// <param name="idBegin">opening label</param>
        /// <param name="idEnd">closing label, can be optional</param>
        /// <returns></returns>
        public static bool EqualsLabel(this string idBegin, string idEnd)
        {
            if (!string.IsNullOrEmpty(idEnd))
                return EqualsIdentifier(idBegin, idEnd);
            return true;
        }

        public static bool EqualsIgnoreCase(this String str1, String str2)
        {
            return str1.Equals(str2, StringComparison.InvariantCultureIgnoreCase);
        }

        public static int CompareToIgnoreCase(this String str1, String str2)
        {
            return String.Compare(str2, str1, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
