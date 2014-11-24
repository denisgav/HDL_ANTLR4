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
using System;

namespace VHDL.statement
{
    /// <summary>
    ///  Sequential statement visitor.
    ///  The sequential statement visitor visits all statements in a hierarchy of statements.
    ///  To use this class you need to subclass it and override the <code>visit...()</code> methods
    ///  you want to handle. If you override the vist methods for loops, case- or if-statments you need
    ///  to call <code>super.visit...(statement)</code> to visit the child statements.
    /// </summary>
    [Serializable]
    public class SequentialStatementVisitor
    {
        /// <summary>
        /// Visits a sequential statement.
        /// </summary>
        /// <param name="statement">the statement</param>
        public virtual void visit(SequentialStatement statement)
        {
            if (statement != null)
            {
                statement.accept(this);
            }
        }

        /// <summary>
        /// Visits a list of sequential statements.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="statements">the list of statements</param>
        public virtual void visit<T1>(IList<T1> statements) where T1 : SequentialStatement
        {
            foreach (SequentialStatement statement in statements)
            {
                if (statement != null)
                {
                    visit(statement);
                }
            }
        }

        /// <summary>
        /// Visits a assertion statement.
        /// </summary>
        /// <param name="statement">the statement</param>
        protected internal virtual void visitAssertionStatement(AssertionStatement statement)
        {
        }

        //    *
        //     * Visits a case statement.
        //     * @param statement the statement
        //     
        protected internal virtual void visitCaseStatement(CaseStatement statement)
        {
            foreach (CaseStatement.Alternative alternative in statement.Alternatives)
            {
                visitCaseStatementAlternative(alternative);
            }
        }

        /// <summary>
        /// Visits an alternative of a case statement.
        /// </summary>
        /// <param name="alternative">the alternative</param>
        protected internal virtual void visitCaseStatementAlternative(CaseStatement.Alternative alternative)
        {
            visit(alternative.Statements);
        }

        /// <summary>
        /// Visits an exit statment.
        /// </summary>
        /// <param name="statement">the statement</param>
        protected internal virtual void visitExitStatement(ExitStatement statement)
        {
        }

        /// <summary>
        /// for statement.
        /// </summary>
        /// <param name="statement">the statement</param>
        protected internal virtual void visitForStatement(ForStatement statement)
        {
            foreach (SequentialStatement s in statement.Statements)
            {
                visit(s);
            }
        }

        /// <summary>
        /// Visits a if statement.
        /// </summary>
        /// <param name="statement">the statement</param>
        protected internal virtual void visitIfStatement(IfStatement statement)
        {
            visit(statement.Statements);
            foreach (IfStatement.ElsifPart elsifPart in statement.ElsifParts)
            {
                visitIfStatementElsifPart(elsifPart);
            }
            visit(statement.ElseStatements);
        }

        /// <summary>
        /// Visits the elsif part of a if statement.
        /// </summary>
        /// <param name="part">the elsif part</param>
        protected internal virtual void visitIfStatementElsifPart(IfStatement.ElsifPart part)
        {
            visit(part.Statements);
        }

        /// <summary>
        /// Visits a loop statement.
        /// </summary>
        /// <param name="statement">the statement</param>
        protected internal virtual void visitLoopStatement(LoopStatement statement)
        {
            foreach (SequentialStatement s in statement.Statements)
            {
                visit(s);
            }
        }

        /// <summary>
        /// Visits a next statement.
        /// </summary>
        /// <param name="statement">the statement</param>
        protected internal virtual void visitNextStatement(NextStatement statement)
        {
        }

        /// <summary>
        /// Visits a null statement.
        /// </summary>
        /// <param name="statement">the statement</param>
        protected internal virtual void visitNullStatement(NullStatement statement)
        {
        }

        /// <summary>
        /// Visits a procedure call statement.
        /// </summary>
        /// <param name="statement">the statement</param>
        protected internal virtual void visitProcedureCall(ProcedureCall statement)
        {
        }

        /// <summary>
        /// Visits a report statement.
        /// </summary>
        /// <param name="statement">statement the statement</param>
        protected internal virtual void visitReportStatement(ReportStatement statement)
        {
        }

        /// <summary>
        /// Visits a return statement.
        /// </summary>
        /// <param name="statement">the statement</param>
        protected internal virtual void visitReturnStatement(ReturnStatement statement)
        {
        }

        /// <summary>
        /// Visits a signal assignment statement.
        /// </summary>
        /// <param name="statement">the statement</param>
        protected internal virtual void visitSignalAssignment(SignalAssignment statement)
        {
        }

        /// <summary>
        /// Visits a variable assignment statement.
        /// </summary>
        /// <param name="statement">the statement</param>
        protected internal virtual void visitVariableAssignment(VariableAssignment statement)
        {
        }

        /// <summary>
        /// Visits a wait statement.
        /// </summary>
        /// <param name="statement">the statement</param>
        protected internal virtual void visitWaitStatement(WaitStatement statement)
        {
        }

        /// <summary>
        /// Visits a while statement.
        /// </summary>
        /// <param name="statement">the statement</param>
        protected internal virtual void visitWhileStatement(WhileStatement statement)
        {
            foreach (SequentialStatement s in statement.Statements)
            {
                visit(s);
            }
        }
    }

}