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
using System.Runtime.Serialization;

namespace VHDL.literal
{
    using SubtypeIndication = VHDL.type.ISubtypeIndication;

    /// <summary>
    /// Binary literal.
    /// </summary>
    [Serializable]
    public class BinaryStringLiteral : StringLiteral
    {
        /// <summary>
        /// Creates a binary literal.
        /// </summary>
        /// <param name="value">the value</param>
        public BinaryStringLiteral(string @value)
            : base(value)
        {
        }

        /// <summary>
        /// Creates a binary literal by converting a integer.
        /// </summary>
        /// <param name="value">the integer value</param>
        /// <param name="width">the width of the binary literal in bits</param>
        public BinaryStringLiteral(int @value, int width)
            : base()
        {
            this.@string = toBinary(@value, width);
        }



        public override SubtypeIndication Type
        {
            get { throw new Exception("Not supported yet."); }
        }

        public override Choice copy()
        {
            return new BinaryStringLiteral(@string);
        }

        public override string ToString()
        {
            return "b\"" + @string + '"';
        }
    }

}