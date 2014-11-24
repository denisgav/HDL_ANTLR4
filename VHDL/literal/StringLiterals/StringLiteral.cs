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


using VHDL.expression;
using System;

namespace VHDL.literal
{
    using SubtypeIndication = VHDL.type.ISubtypeIndication;

    /// <summary>
    /// String literal.
    /// </summary>
    [Serializable]
    public class StringLiteral : Literal
    {
        protected string @string;

        /// <summary>
        /// Creates a string literal.
        /// </summary>
        /// <param name="string">the string</param>
        public StringLiteral(string @string)
        {
            this.@string = @string;
        }

        public StringLiteral()
        {
            this.@string = string.Empty;
        }

        public override SubtypeIndication Type
        {
            get { throw new Exception("Not supported yet."); }
        }

        public override Choice copy()
        {
            return new StringLiteral(@string);
        }

        public virtual string String
        {
            get { return @string; }
        }

        public override string ToString()
        {
            return '"' + @string + '"';
        }

        /// <summary>
        /// Converts an integer to binary.
        /// If the value is to big to be expressed with the given width only the lower bits are used.
        /// </summary>
        /// <param name="value">the numeric value</param>
        /// <param name="width">the width in bits</param>
        /// <returns>a string containing the binary</returns>
        protected string toBinary(int @value, int width)
        {
            string binString = Convert.ToString(@value, 2);

            int length = binString.Length;

            if (length > width)
            {
                binString = binString.Substring(length - width, length);
            }

            return binString;
        }

        /// <summary>
        /// Converts an integer to octal.
        /// If the value is to big to be expressed with the given width only the lower bits are used.
        /// </summary> 
        /// <param name="value">the numeric value</param>
        /// <param name="width">the width in bits</param>
        /// <returns>a string containing the binary</returns>
        protected string toOctal(int @value, int width)
        {
            string binString = Convert.ToString(@value, 8);

            int length = binString.Length;

            if (length > width)
            {
                binString = binString.Substring(length - width, length);
            }

            return binString;
        }

        /// <summary>
        /// Converts an integer to hexa.
        /// If the value is to big to be expressed with the given width only the lower bits are used.
        /// </summary>
        /// <param name="value">the numeric value</param>
        /// <param name="width">the width in bits</param>
        /// <returns>a string containing the binary</returns>
        protected string toHex(int @value, int width)
        {
            string binString = Convert.ToString(@value, 16);

            int length = binString.Length;

            if (length > width)
            {
                binString = binString.Substring(length - width, length);
            }

            return binString;
        }

        public override void accept(ILiteralVisitor visitor)
        {
            visitor.visit(this);
        }

    }

}