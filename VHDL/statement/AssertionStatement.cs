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
using System.Collections.Generic;

namespace VHDL.statement
{
    using Expression = VHDL.expression.Expression;
    using StringLiteral = VHDL.literal.StringLiteral;
    ///
    // * Assertion statement.
    // *
    // * @VHDL.example
    // * AssertionStatement statement = new AssertionStatement(
    // *  Standard.BOOLEAN_TRUE, "true is not true", Standard.SEVERITY_LEVEL_NOTE);
    // * ---
    // * assert TRUE report "true is true" severity NOTE;
    // 
    [Serializable]
    public class AssertionStatement : SequentialStatement
    {
        private Expression condition;
        private Expression reportedExpression;
        private Expression severity;

        /// <summary>
        /// Creates an assertion statement.
        /// </summary>
        /// <param name="condition">the assertion condition</param>
        public AssertionStatement(Expression condition)
        {
            this.condition = condition;
        }

        /// <summary>
        /// Creates an assertion statement with a reported message.
        /// </summary>
        /// <param name="condition">the assertion condtion</param>
        /// <param name="reportedExpression">the reported message</param>
        public AssertionStatement(Expression condition, Expression reportedExpression)
        {
            this.condition = condition;
            this.reportedExpression = reportedExpression;
        }

        /// <summary>
        /// Creates an assertion statement with a reported expression.
        /// </summary>
        /// <param name="condition">the assertion condition</param>
        /// <param name="reportedExpression">the reported message</param>
        public AssertionStatement(Expression condition, string reportedExpression)
            : this(condition, new StringLiteral(reportedExpression))
        {
        }

        //TODO: find other solution
        //    public AssertionStatement(Expression condition, Expression severity) {
        //    this.condition = condition;
        //    this.severity = severity;
        //    }

        /// <summary>
        /// Creates an assertion statement with reported message and severity.
        /// </summary>
        /// <param name="condition">the assertion condition</param>
        /// <param name="reportedExpression">the reported message</param>
        /// <param name="severity">the severity</param>
        public AssertionStatement(Expression condition, Expression reportedExpression, Expression severity)
        {
            this.condition = condition;
            this.reportedExpression = reportedExpression;
            this.severity = severity;
        }

        /// <summary>
        /// Creates an assertion statement with reported message and severity.
        /// </summary>
        /// <param name="condition">the assertion condition</param>
        /// <param name="reportedExpression">the reported message</param>
        /// <param name="severity">the severity</param>
        public AssertionStatement(Expression condition, string reportedExpression, Expression severity)
            : this(condition, new StringLiteral(reportedExpression), severity)
        {
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
        /// Returns/Sets the reported message.
        /// </summary>
        public virtual Expression ReportedExpression
        {
            get { return reportedExpression; }
            set { reportedExpression = value; }
        }
        
        /// <summary>
        /// Returns/Sets the severity.
        /// </summary>
        public virtual Expression Severity
        {
            get { return severity; }
            set { severity = value; }
        }

        internal override void accept(SequentialStatementVisitor visitor)
        {
            visitor.visitAssertionStatement(this);
        }

        public override List<VhdlElement> GetAllStatements()
        {
            List<VhdlElement> res = new List<VhdlElement>();
            res.Add(severity);
            res.Add(reportedExpression);
            res.Add(condition);
            return res;
        }
    }
}