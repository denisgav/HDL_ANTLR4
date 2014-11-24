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
    /// Unary minus sign expression.
    /// </summary>
    [Serializable]
    public class Minus : UnaryExpression
    {
        /// <summary>
        /// Creates a unary minus sign expression.
        /// </summary>
        /// <param name="expression">the parameter expression</param>
        public Minus(Expression expression)
            : base(expression, ExpressionKind.MINUS, ExpressionPrecedences.ADDING_EXPRESSION)
        {
        }

        public override Choice copy()
        {
            return new Minus(Expression.copy() as Expression);
        }
    }

}