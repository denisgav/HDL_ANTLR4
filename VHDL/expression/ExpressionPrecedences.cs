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

namespace VHDL.expression
{
    /// <summary>
    /// Expression precedence constants.
    /// </summary>
    internal class ExpressionPrecedences
    {
        /// <summary>
        /// Logical expression precedence.
        /// </summary>
        public const int LOGICAL_EXPRESSION = 1;

        /// <summary>
        /// Relational expression precedence.
        /// </summary>
        public const int RELATIONAL_EXPRESSION = 2;

        /// <summary>
        /// Shift expression precedence.
        /// </summary>
        public const int SHIFT_EXPRESSION = 3;

        /// <summary>
        /// Adding expression precedence.
        /// </summary>
        public const int ADDING_EXPRESSION = 4;

        /// <summary>
        /// Multiplying expression precedence.
        /// </summary>
        public const int MULTIPLYING_EXPRESSION = 5;

        /// <summary>
        /// Miscellaneous expression precedence.
        /// </summary>
        public const int MISCELLANEOUS_EXPRESSION = 6;

        /// <summary>
        /// Primary precedence.
        /// </summary>
        public const int PRIMARY = 7;

        /// <summary>
        /// Prevent instantiation.
        /// </summary>
        private ExpressionPrecedences()
        {
        }
    }
}