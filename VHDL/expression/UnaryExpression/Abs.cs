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
namespace VHDL.expression
{
    /// <summary>
    /// Absolute value expression.
    /// </summary>
    [Serializable]
    public class Abs : UnaryExpression
    {
        /// <summary>
        /// Creates a new absolute value expression.
        /// </summary>
        /// <param name="expression">the parameter</param>
        public Abs(Expression expression)
            : base(expression, ExpressionKind.ABS, ExpressionPrecedences.MISCELLANEOUS_EXPRESSION)
        {
        }

        public override Choice copy()
        {
            return new Abs(Expression.copy() as Expression);
        }
    }

}