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
    /// Hexadecimal literal.
    /// </summary>
    [Serializable]
    public class HexStringLiteral : StringLiteral
    {
        //TODO: check string for invalid chars
        /// <summary>
        /// Creates a hex literal.
        /// </summary>
        /// <param name="value">the value</param>
        public HexStringLiteral(string @value)
            : base(@value)
        {
        }

        /// <summary>
        /// Creates a hex literal by converting a integer value.
        /// </summary>
        /// <param name="value">the integer value</param>
        /// <param name="width"></param>
        public HexStringLiteral(int @value, int width)
            : base()
        {
            this.@string = toHex(@value, width);
        }

        public override SubtypeIndication Type
        {
            get { throw new Exception("Not supported yet."); }
        }

        public override Choice copy()
        {
            return new HexStringLiteral(@string);
        }

        public override string ToString()
        {
            return "x\"" + @string + '"';
        }
    }

}