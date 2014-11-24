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
    /// Expression visitor.
    /// The expression visitor visits all parts of an expression.
    /// To use this class you need to subclass it and override the <code>visit...()</code> methods
    /// you want to handle. 
    /// </summary>
    [Serializable]
    public class ExpressionVisitor
    {
        /// <summary>
        /// Visits an expression.
        /// No visit method is called when the parameter is <code>null</code>.
        /// </summary>
        /// <param name="expression">the expression</param>
        public virtual void visit(Expression expression)
        {
            if (expression != null)
            {
                expression.accept(this);
            }
        }

        /// <summary>
        /// Visits an aggregate.
        /// </summary>
        /// <param name="expression">the aggregate</param>
        protected internal virtual void visitAggregate(Aggregate expression)
        {
        }

        /// <summary>
        /// Visits a binary expression.
        /// </summary>
        /// <param name="expression"> the expression</param>
        protected internal virtual void visitBinaryExpression(BinaryExpression expression)
        {
            visit(expression.Left);
            visit(expression.Right);
        }

        /// <summary>
        /// Visits a function call.
        /// </summary>
        /// <param name="expression">the function call</param>
        protected internal virtual void visitFunctionCall(FunctionCall expression)
        {
        }

        /// <summary>
        /// Visits a literal.
        /// </summary>
        /// <param name="expression">the literal</param>
        protected internal virtual void visitLiteral(Literal expression)
        {
        }

        /// <summary>
        /// Visits a parentheses expression.
        /// </summary>
        /// <param name="expression">the parentheses expression</param>
        protected internal virtual void visitParentheses(Parentheses expression)
        {
            visit(expression.Expression);
        }

        /// <summary>
        /// Visits a qualified expression.
        /// </summary>
        /// <param name="expression">the qualified expression</param>
        protected internal virtual void visitQualifiedExpression(QualifiedExpression expression)
        {
        }

        /// <summary>
        /// Vistis a qualified expression allocator.
        /// </summary>
        /// <param name="expression">the qualified expression allocator</param>
        protected internal virtual void visitQualifiedExpressionAllocator(QualifiedExpressionAllocator expression)
        {
        }

        /// <summary>
        /// Visits a subtype indication allocator.
        /// </summary>
        /// <param name="expression">the subtype indication allocator</param>
        protected internal virtual void visitSubtypeIndicationAllocator(SubtypeIndicationAllocator expression)
        {
        }

        /// <summary>
        /// Visits a type conversion.
        /// </summary>
        /// <param name="expression">the type conversion</param>
        protected internal virtual void visitTypeConversion(TypeConversion expression)
        {
            visit(expression.Expression);
        }

        /// <summary>
        /// Visits a unary expression.
        /// </summary>
        /// <param name="expression">the expression</param>
        protected internal virtual void visitUnaryExpression(UnaryExpression expression)
        {
            visit(expression.Expression);
        }

        /// <summary>
        /// Visits a name.
        /// </summary>
        /// <param name="object">the name</param>
        protected internal virtual void visitName(Name @object)
        {
        }
    }

}