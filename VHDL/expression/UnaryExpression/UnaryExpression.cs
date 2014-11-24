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
using System.Runtime.Serialization;

namespace VHDL.expression
{
    using SubtypeIndication = VHDL.type.ISubtypeIndication;

    /// <summary>
    /// Abstract base class for unary expressions.
    /// </summary>
    [Serializable]
    public abstract class UnaryExpression : Expression
    {
        private Expression expression;
        private readonly int precedence;
        private readonly ExpressionKind kind;

        internal UnaryExpression(Expression expression, ExpressionKind kind, int precedence)
        {
            this.expression = expression;
            this.precedence = precedence;
            this.kind = kind;
        }

        /// <summary>
        /// Gets/Sets the parameter expression.
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

        public override int Precedence
        {
            get { return precedence; }
        }

        //TODO: make package private?
        /// <summary>
        /// Returns the kind of this expression.
        /// </summary>
        public virtual ExpressionKind ExpressionKind
        {
            get { return kind; }
        }

        public override void accept(ExpressionVisitor visitor)
        {
            visitor.visitUnaryExpression(this);
        }
    }

}