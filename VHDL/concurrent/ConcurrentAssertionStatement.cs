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

namespace VHDL.concurrent
{
    using Expression = VHDL.expression.Expression;

    /// <summary>
    /// Concurrent assertion statement.
    /// </summary>
    [Serializable]
    public class ConcurrentAssertionStatement : EntityStatement
    {
        private Expression condition;
        private Expression reportedExpression;
        private Expression severity;

        /// <summary>
        /// Creates a concurrent assertion statement.
        /// </summary>
        /// <param name="condition">the asertion condition</param>
        public ConcurrentAssertionStatement(Expression condition)
        {
            this.condition = condition;
        }

        /// <summary>
        /// Creates a concurrent assertion statement with a reported message.
        /// </summary>
        /// <param name="condition">the condition</param>
        /// <param name="reportedExpression">the reported message</param>
        public ConcurrentAssertionStatement(Expression condition, Expression reportedExpression)
        {
            this.condition = condition;
            this.reportedExpression = reportedExpression;
        }

        //TODO: find other solution
        //    
        //    public ConcurrentAssertionStatement(Expression condition, Expression severity) {
        //    this.condition = condition;
        //    this.severity = severity;
        //    }
        //    *
        //     * Creates a concurrent assertion statement with reported message and severity.
        //     * @param condition the condition
        //     * @param reportedExpression the reported message
        //     * @param severity the severity
        //     
        public ConcurrentAssertionStatement(Expression condition, Expression reportedExpression, Expression severity)
        {
            this.condition = condition;
            this.reportedExpression = reportedExpression;
            this.severity = severity;
        }

        public ConcurrentAssertionStatement(VHDL.statement.AssertionStatement assertionStatement)
        {
            this.condition = assertionStatement.Condition;
            this.reportedExpression = assertionStatement.ReportedExpression;
            this.severity = assertionStatement.Severity;
            this.Label = assertionStatement.Label;
        }

        /// <summary>
        /// Returns/Sets the assertion condition.
        /// </summary>
        public virtual Expression Condition
        {
            get { return condition; }
            set { condition = value; }
        }

        /// <summary>
        /// Returns/Sets the reported message expression.
        /// </summary>
        public virtual Expression ReportedExpression
        {
            get { return reportedExpression; }
            set { reportedExpression = value; }
        }

        /// <summary>
        /// Returns/Sets the severity of this assertion.
        /// </summary>
        public virtual Expression Severity
        {
            get { return severity; }
            set { severity = value; }
        }

        internal override void accept(ConcurrentStatementVisitor visitor)
        {
            visitor.visitConcurrentAssertionStatement(this);
        }
    }
}