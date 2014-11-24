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
    using SubtypeIndication = VHDL.type.ISubtypeIndication;
    /// <summary>
    /// Parentheses expression.
    /// </summary>
    [Serializable]
    public class Parentheses : Primary
    {
        private Expression expression;

        /// <summary>
        /// Creates a parentheses expression.
        /// </summary>
        /// <param name="expression">the expression inside the parentheses</param>
        public Parentheses(Expression expression)
        {
            this.expression = expression;
        }

        /// <summary>
        /// Returns/Sets the expression inside the parentheses.
        /// </summary>
        public virtual Expression Expression
        {
            get { return expression; }
            set { expression = value; }
        }

        public override SubtypeIndication Type
        {
            get { return expression.Type; }
        }

        public override void accept(ExpressionVisitor visitor)
        {
            visitor.visitParentheses(this);
        }

        public override Choice copy()
        {
            return new Parentheses(expression.copy() as Expression);
        }
    }
}