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
using System.Globalization;

namespace VHDL.literal
{
    using SubtypeIndication = VHDL.type.ISubtypeIndication;

    /// <summary>
    /// Decimal literal.
    /// </summary>
    [Serializable]
    public class DecBasedInteger : IntegerLiteral
	{		     
		public DecBasedInteger(string @value)
            :base(@value)
		{
		}

        /// <summary>
        /// Creates a decimal literal by converting a integer value.
        /// </summary>
        /// <param name="value">the integer value</param>
        public DecBasedInteger(Int64 @value)
            : base(@value)
		{
		}

        public override Int64 ConvertToInt()
        {
            return ConvertToIntDec(@value);
        }

        public override string ConvertToString()
        {
            return ConvertToStringDec(@integerValue);
        }

        public override Choice copy()
		{
			return new DecBasedInteger(@value);
		}

	}

}