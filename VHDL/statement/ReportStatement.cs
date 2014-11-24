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

    /// <summary>
    /// Report statement.
    ///
    ///  ReportStatement statement = new ReportStatement("reported error",
    ///   Standard.SEVERITY_LEVEL_ERROR);
    ///  ---
    ///  report "reported error" severity ERROR;
    /// </summary>
    [Serializable]
    public class ReportStatement : SequentialStatement
    {
        private Expression reportExpression;
        private Expression severity;

        /// <summary>
        /// Creates a report statement.
        /// </summary>
        /// <param name="reportExpression">the reported message</param>
        public ReportStatement(Expression reportExpression)
        {
            this.reportExpression = reportExpression;
        }

        /// <summary>
        /// Creates a report statement.
        /// </summary>
        /// <param name="reportExpression">the reported message</param>
        public ReportStatement(string reportExpression)
            : this(new StringLiteral(reportExpression))
        {
        }

        /// <summary>
        /// Creates a report statement with severity.
        /// </summary>
        /// <param name="reportExpression">the reported message</param>
        /// <param name="severity">the severity</param>
        public ReportStatement(Expression reportExpression, Expression severity)
        {
            this.reportExpression = reportExpression;
            this.severity = severity;
        }

        /// <summary>
        /// Creates a report statement with severity.
        /// </summary>
        /// <param name="reportExpression">the reported message</param>
        /// <param name="severity">the severity</param>
        public ReportStatement(string reportExpression, Expression severity)
            : this(new StringLiteral(reportExpression), severity)
        {
        }

        /// <summary>
        /// Returns/Sets the reported message.
        /// </summary>
        public virtual Expression ReportExpression
        {
            get { return reportExpression; }
            set { reportExpression = value; }
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
            visitor.visitReportStatement(this);
        }

        public override List<VhdlElement> GetAllStatements()
        {
            List<VhdlElement> res = new List<VhdlElement>();
            res.Add(severity);
            res.Add(reportExpression);
            return res;
        }
    }

}