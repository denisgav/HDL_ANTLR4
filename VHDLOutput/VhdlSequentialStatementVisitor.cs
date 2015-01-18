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

using System.Collections.Generic;

namespace VHDL.output
{

    using Choice = VHDL.Choice;
    using Expression = VHDL.expression.Expression;
    using AssertionStatement = VHDL.statement.AssertionStatement;
    using CaseStatement = VHDL.statement.CaseStatement;
    using Alternative = VHDL.statement.CaseStatement.Alternative;
    using ExitStatement = VHDL.statement.ExitStatement;
    using ForStatement = VHDL.statement.ForStatement;
    using IfStatement = VHDL.statement.IfStatement;
    using ElsifPart = VHDL.statement.IfStatement.ElsifPart;
    using LoopStatement = VHDL.statement.LoopStatement;
    using NextStatement = VHDL.statement.NextStatement;
    using NullStatement = VHDL.statement.NullStatement;
    using ProcedureCall = VHDL.statement.ProcedureCall;
    using ReportStatement = VHDL.statement.ReportStatement;
    using ReturnStatement = VHDL.statement.ReturnStatement;
    using SequentialStatement = VHDL.statement.SequentialStatement;
    using SequentialStatementVisitor = VHDL.statement.SequentialStatementVisitor;
    using SignalAssignment = VHDL.statement.SignalAssignment;
    using VariableAssignment = VHDL.statement.VariableAssignment;
    using WaitStatement = VHDL.statement.WaitStatement;
    using WhileStatement = VHDL.statement.WhileStatement;
    using System;

    /// <summary>
    /// Sequential statement output visitor.
    /// </summary>
    internal class VhdlSequentialStatementVisitor : SequentialStatementVisitor
    {

        private readonly VhdlWriter writer;
        private readonly OutputModule output;

        public VhdlSequentialStatementVisitor(VhdlWriter writer, OutputModule output)
        {
            this.writer = writer;
            this.output = output;
        }

        public override void visit(SequentialStatement statement)
        {
            VhdlOutputHelper.handleAnnotationsBefore(statement, writer);
            base.visit(statement);
            VhdlOutputHelper.handleAnnotationsAfter(statement, writer);
        }

        public override void visit<T1>(IList<T1> statements)
        {
            foreach (SequentialStatement statement in statements)
            {
                visit(statement);
            }
        }

        private void appendLabel(SequentialStatement statement)
        {
            if (statement.Label != null)
            {
                writer.Append(statement.Label).Append(" : ");
            }
        }

        private void appendLoopPart(LoopStatement statement)
        {
            writer.Append(KeywordEnum.LOOP.ToString()).NewLine().Indent();
            visit(statement.Statements);
            writer.Dedent().Append(KeywordEnum.END.ToString()).Append(KeywordEnum.LOOP.ToString()).Append(';').NewLine();
        }

        private void appendExitOrNextStatement(SequentialStatement statement, KeywordEnum keyword, LoopStatement loop, Expression condition)
        {
            appendLabel(statement);
            writer.Append(keyword.ToString());
            if (loop != null)
            {
                string label = loop.Label;

                if (label == null)
                {
                    //FIXME: unify handling of null values
                    throw new ArgumentNullException("Loop label is null");
                }

                writer.Append(' ').Append(label);
            }
            if (condition != null)
            {
                writer.Append(' ').Append(KeywordEnum.WHEN.ToString()).Append(' ');
                output.writeExpression(condition);
            }
            writer.Append(';').NewLine();
        }

        protected override void visitAssertionStatement(AssertionStatement statement)
        {
            appendLabel(statement);
            writer.Append(KeywordEnum.ASSERT.ToString()).Append(' ');
            output.writeExpression(statement.Condition);
            if (statement.ReportedExpression != null)
            {
                writer.Append(' ').Append(KeywordEnum.REPORT.ToString()).Append(' ');
                output.writeExpression(statement.ReportedExpression);
            }
            if (statement.Severity != null)
            {
                writer.Append(' ').Append(KeywordEnum.SEVERITY.ToString()).Append(' ');
                output.writeExpression(statement.Severity);
            }
            writer.Append(';').NewLine();
        }

        protected override void visitCaseStatement(CaseStatement statement)
        {
            appendLabel(statement);
            writer.Append(KeywordEnum.CASE.ToString()).Append(' ');
            output.writeExpression(statement.Expression);
            writer.Append(' ').Append(KeywordEnum.IS.ToString()).NewLine();

            writer.Indent();
            foreach (CaseStatement.Alternative alternative in statement.Alternatives)
            {
                visitCaseStatementAlternative(alternative);
            }
            writer.Dedent();

            writer.Append(KeywordEnum.END.ToString()).Append(KeywordEnum.CASE.ToString());

            if (writer.Format.RepeatLabels && statement.Label != null)
            {
                writer.Append(' ').Append(statement.Label);
            }

            writer.Append(';').NewLine();
        }

        protected override void visitCaseStatementAlternative(Alternative alternative)
        {
            writer.Append(KeywordEnum.WHEN.ToString()).Append(' ');

            bool first = true;
            foreach (Choice choice in alternative.Choices)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    writer.Append(" | ");
                }
                output.writeChoice(choice);
            }
            writer.Append(" =>").NewLine();

            writer.Indent();
            output.writeSequentialStatements(alternative.Statements);
            writer.Dedent();
            writer.NewLine();
        }

        protected override void visitExitStatement(ExitStatement statement)
        {
            appendExitOrNextStatement(statement, KeywordEnum.EXIT, statement.Loop, statement.Condition);
        }

        protected override void visitForStatement(ForStatement statement)
        {
            appendLabel(statement);
            writer.Append(KeywordEnum.FOR.ToString()).Append(' ');
            output.writeExpression(statement.Parameter);
            writer.Append(' ').Append(KeywordEnum.IN.ToString()).Append(' ');
            output.writeDiscreteRange(statement.Range);
            writer.Append(' ');
            appendLoopPart(statement);
        }

        protected override void visitIfStatement(IfStatement statement)
        {
            appendLabel(statement);
            writer.Append(KeywordEnum.IF.ToString()).Append(' ');
            output.writeExpression(statement.Condition);
            writer.Append(' ').Append(KeywordEnum.THEN.ToString()).NewLine();
            writer.Indent();
            visit(statement.Statements);
            writer.Dedent();
            foreach (IfStatement.ElsifPart elsifPart in statement.ElsifParts)
            {
                visitIfStatementElsifPart(elsifPart);
            }
            if (statement.ElseStatements.Count != 0)
            {
                writer.Append(KeywordEnum.ELSE.ToString()).NewLine();
                writer.Indent();
                visit(statement.ElseStatements);
                writer.Dedent();
            }
            writer.Append(KeywordEnum.END.ToString()).Append(KeywordEnum.IF.ToString());
            if (statement.Label != null && writer.Format.RepeatLabels)
            {
                writer.Append(' ').Append(statement.Label);
            }
            writer.Append(';').NewLine();
        }

        protected override void visitIfStatementElsifPart(ElsifPart part)
        {
            writer.Append(KeywordEnum.ELSIF.ToString()).Append(' ');
            output.writeExpression(part.Condition);
            writer.Append(' ').Append(KeywordEnum.THEN.ToString()).NewLine();
            writer.Indent();
            visit(part.Statements);
            writer.Dedent();
        }

        protected override void visitLoopStatement(LoopStatement statement)
        {
            appendLabel(statement);
            appendLoopPart(statement);
        }

        protected override void visitNextStatement(NextStatement statement)
        {
            appendExitOrNextStatement(statement, KeywordEnum.NEXT, statement.Loop, statement.Condition);
        }

        protected override void visitNullStatement(NullStatement statement)
        {
            appendLabel(statement);
            writer.Append(KeywordEnum.NULL.ToString()).Append(';').NewLine();
        }

        protected override void visitProcedureCall(ProcedureCall statement)
        {
            appendLabel(statement);
            writer.Append(statement.Procedure.Identifier);
            if (statement.Parameters.Count != 0)
            {
                writer.Append('(');
                output.getMiscellaneousElementOutput().procedureCallParameters(statement.Parameters);
                writer.Append(')');
            }
            writer.Append(';').NewLine();
        }

        protected override void visitReportStatement(ReportStatement statement)
        {
            appendLabel(statement);
            writer.Append(KeywordEnum.REPORT.ToString()).Append(' ');
            output.writeExpression(statement.ReportExpression);
            if (statement.Severity != null)
            {
                writer.Append(' ').Append(KeywordEnum.SEVERITY.ToString()).Append(' ');
                output.writeExpression(statement.Severity);
            }
            writer.Append(';').NewLine();
        }

        protected override void visitReturnStatement(ReturnStatement statement)
        {
            appendLabel(statement);
            writer.Append(KeywordEnum.RETURN.ToString());
            if (statement.ReturnedExpression != null)
            {
                writer.Append(' ');
                output.writeExpression(statement.ReturnedExpression);
            }
            writer.Append(';').NewLine();
        }

        protected override void visitSignalAssignment(SignalAssignment statement)
        {
            appendLabel(statement);
            output.writeSignalAssignmentTarget(statement.Target);
            writer.Append(" <= ");

            if (statement.DelayMechanism != null)
            {
                output.getMiscellaneousElementOutput().delayMechanism(statement.DelayMechanism);
                writer.Append(' ');
            }

            output.getMiscellaneousElementOutput().waveform(statement.Waveform);
            writer.Append(";").NewLine();
        }

        protected override void visitVariableAssignment(VariableAssignment statement)
        {
            appendLabel(statement);
            output.writeVariableAssignmentTarget(statement.Target);
            writer.Append(" := ");
            output.writeExpression(statement.Value);
            writer.Append(";").NewLine();
        }

        protected override void visitWaitStatement(WaitStatement statement)
        {
            appendLabel(statement);
            writer.Append(KeywordEnum.WAIT.ToString());
            if (statement.SensitivityList.Count != 0)
            {
                writer.Append(' ').Append(KeywordEnum.ON.ToString()).Append(' ');
                writer.AppendIdentifiers(statement.SensitivityList, ", ");
            }
            if (statement.Condition != null)
            {
                writer.Append(' ').Append(KeywordEnum.UNTIL.ToString()).Append(' ');
                output.writeExpression(statement.Condition);
            }
            if (statement.Timeout != null)
            {
                writer.Append(' ').Append(KeywordEnum.FOR.ToString()).Append(' ');
                output.writeExpression(statement.Timeout);
            }
            writer.Append(';').NewLine();
        }

        protected override void visitWhileStatement(WhileStatement statement)
        {
            appendLabel(statement);
            writer.Append(KeywordEnum.WHILE.ToString()).Append(' ');
            output.writeExpression(statement.Condition);
            writer.Append(' ');
            appendLoopPart(statement);
        }
    }
}