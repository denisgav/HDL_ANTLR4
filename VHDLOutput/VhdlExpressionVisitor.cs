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

namespace VHDL.output
{

    using Choice = VHDL.Choice;
    using Aggregate = VHDL.expression.Aggregate;
    using BinaryExpression = VHDL.expression.BinaryExpression;
    using Expression = VHDL.expression.Expression;
    using ExpressionKind = VHDL.expression.ExpressionKind;
    using ExpressionVisitor = VHDL.expression.ExpressionVisitor;
    using FunctionCall = VHDL.expression.FunctionCall;
    using Literal = VHDL.expression.Literal;
    using Name = VHDL.expression.Name;
    using Parentheses = VHDL.expression.Parentheses;
    using QualifiedExpression = VHDL.expression.QualifiedExpression;
    using QualifiedExpressionAllocator = VHDL.expression.QualifiedExpressionAllocator;
    using SubtypeIndicationAllocator = VHDL.expression.SubtypeIndicationAllocator;
    using TypeConversion = VHDL.expression.TypeConversion;
    using UnaryExpression = VHDL.expression.UnaryExpression;

    /// <summary>
    /// Expression output visitor.
    /// </summary>
    internal class VhdlExpressionVisitor : ExpressionVisitor
    {

        private readonly VhdlWriter writer;
        private readonly OutputModule output;

        public VhdlExpressionVisitor(VhdlWriter writer, OutputModule output)
        {
            this.writer = writer;
            this.output = output;
        }

        public override void visit(Expression expression)
        {
            VhdlOutputHelper.handleAnnotationsBefore(expression, writer);
            base.visit(expression);
            VhdlOutputHelper.handleAnnotationsAfter(expression, writer);
        }

        private void appendExpression(Expression expression, int precedence)
        {
            if (expression == null)
            {
                writer.Append("null");
                return;
            }

            bool writeParenthesis = expression.Precedence < precedence;

            if (writeParenthesis)
            {
                writer.Append("(");
            }
            visit(expression);
            if (writeParenthesis)
            {
                writer.Append(")");
            }
        }

        protected override void visitAggregate(Aggregate expression)
        {
            writer.Append('(');

            bool first = true;
            foreach (Aggregate.ElementAssociation association in expression.Associations)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    writer.Append(", ");
                }

                if (association.Choices.Count != 0)
                {
                    bool first2 = true;
                    foreach (Choice choice in association.Choices)
                    {
                        if (first2)
                        {
                            first2 = false;
                        }
                        else
                        {
                            writer.Append(" | ");
                        }
                        output.writeChoice(choice);
                    }
                    writer.Append(" => ");
                }
                visit(association.Expression);
            }

            writer.Append(')');
        }

        protected override void visitBinaryExpression(BinaryExpression expression)
        {
            appendExpression(expression.Left, expression.Precedence);
            writer.Append(' ');
            writer.Append(expression.ExpressionKind.ToString());
            writer.Append(' ');
            appendExpression(expression.Right, expression.Precedence);
        }

        protected override void visitFunctionCall(FunctionCall expression)
        {
            writer.AppendIdentifier(expression.Function);
            if (expression.Parameters.Count != 0)
            {
                writer.Append('(');
                output.getMiscellaneousElementOutput().functionCallParameters(expression.Parameters);
                writer.Append(')');
            }
        }

        protected override void visitLiteral(Literal expression)
        {
            writer.Append(expression.ToString());
        }

        protected override void visitParentheses(Parentheses expression)
        {
            writer.Append("(");
            visit(expression.Expression);
            writer.Append(")");
        }

        protected override void visitQualifiedExpression(QualifiedExpression expression)
        {
            output.writeSubtypeIndication(expression.Type);
            writer.Append('\'');
            visit(expression.Operand);
        }

        protected override void visitQualifiedExpressionAllocator(QualifiedExpressionAllocator expression)
        {
            writer.Append(KeywordEnum.NEW.ToString()).Append(' ');
            visit(expression.Expression);
        }

        protected override void visitSubtypeIndicationAllocator(SubtypeIndicationAllocator expression)
        {
            writer.Append(KeywordEnum.NEW.ToString()).Append(' ');
            output.writeSubtypeIndication(expression.Type);
        }

        protected override void visitTypeConversion(TypeConversion expression)
        {
            output.writeSubtypeIndication(expression.Type);
            writer.Append('(');
            visit(expression.Expression);
            writer.Append(')');
        }

        protected override void visitUnaryExpression(UnaryExpression expression)
        {
            writer.Append(expression.ExpressionKind.ToString());
            if (expression.ExpressionKind != ExpressionKind.MINUS && expression.ExpressionKind != ExpressionKind.PLUS)
            {
                writer.Append(' ');
            }
            appendExpression(expression.Expression, expression.Precedence);
        }

        protected override void visitName(Name name)
        {
            name.accept(output.getNameVisitor());
        }
    }

}