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
    /// Allocator with qualified expression parameter.
    /// </summary>
    [Serializable]
    public class QualifiedExpressionAllocator : Primary
    {
        private QualifiedExpression expression;

        /// <summary>
        /// Creates a qualified expression allocator.
        /// </summary>
        /// <param name="expression">the qualified expression</param>
        public QualifiedExpressionAllocator(QualifiedExpression expression)
        {
            this.expression = expression;
        }

        /// <summary>
        /// Returns/Sets the qualified expression.
        /// </summary>
        public virtual QualifiedExpression Expression
        {
            get { return expression; }
            set { expression = value; }
        }

        public override SubtypeIndication Type
        {
            get { throw new Exception("Not supported yet."); }
        }

        public override Choice copy()
        {
            return new QualifiedExpressionAllocator(expression.copy() as QualifiedExpression);
        }

        public override void accept(ExpressionVisitor visitor)
        {
            visitor.visitQualifiedExpressionAllocator(this);
        }
    }
}