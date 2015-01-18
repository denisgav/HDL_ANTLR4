using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VHDL
{
    //Extension methods must be defined in a static class
    public static class StringExtension
    {
        /// <summary>
        /// Check that 2 VHDL identifiers are equals
        /// </summary>
        /// <param name="identifier1"></param>
        /// <param name="identifier2"></param>
        /// <returns></returns>
        public static bool VHDLIdentifierEquals(this string identifier1, string identifier2)
        {
            if (string.IsNullOrEmpty(identifier1))
                throw new ArgumentException("Identifier1 is empty");
            if (string.IsNullOrEmpty(identifier2))
                throw new ArgumentException("Identifier2 is empty");

            if (identifier1[0] == '\\')
            {
                return identifier1.Equals(identifier2);
            }
            else
            {
                return identifier1.Equals(identifier2, StringComparison.InvariantCultureIgnoreCase);
            }
        }

        public static bool VHDLCheckBeginEndIdentifierForEquals(this string identifier_begin, string identifier_end)
        {
            if (string.IsNullOrEmpty(identifier_end) == false)
            {
                return VHDLIdentifierEquals(identifier_begin, identifier_end);
            }
            return true;
        }
    }
}
