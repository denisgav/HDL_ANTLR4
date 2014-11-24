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
    /// Abstract base class for binary expressions.
    /// </summary>
    [Serializable]
    public abstract class BinaryExpression : Expression
    {
        private Expression left;
        private Expression right;
        private readonly ExpressionKind kind;
        private readonly int precedence;

        /// <summary>
        /// Creates a new <code>BinaryExpression</code>.
        /// </summary>
        /// <param name="left">the left-hand side expression</param>
        /// <param name="kind">the expression kind</param>
        /// <param name="right">the right-hand side expression</param>
        /// <param name="precedence">the precedence of this operation</param>
        internal BinaryExpression(Expression left, ExpressionKind kind, Expression right, int precedence)
        {
            this.left = left;
            this.kind = kind;
            this.right = right;
            this.precedence = precedence;
        }

        /// <summary>
        /// Returns/Set the left-hand side expression of this binary exprssion.
        /// </summary>
        public virtual Expression Left
        {
            get { return left; }
            set { left = value; }
        }

        /// <summary>
        /// Returns/Sets the right-hand side expression of this binary exprssion.
        /// </summary>
        public virtual Expression Right
        {
            get { return right; }
            set { right = value; }
        }

        //TODO: make package private?
        /// <summary>
        /// Returns the expression kind
        /// </summary>
        public virtual ExpressionKind ExpressionKind
        {
            get { return kind; }
        }

        public override int Precedence
        {
            get { return precedence; }
        }

        public override void accept(ExpressionVisitor visitor)
        {
            visitor.visitBinaryExpression(this);
        }
    }

}