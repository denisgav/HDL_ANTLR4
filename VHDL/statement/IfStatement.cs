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
using VHDL.util;
using System;

namespace VHDL.statement
{
    using Expression = VHDL.expression.Expression;
    ///
    // * If statement.
    // *
    // * @VHDL.example
    // * IfStatement statement = new IfStatement(Standard.BOOLEAN_FALSE);
    // * statement.getStatements().add(new ReportStatement("if part"));
    // * statement.createElsifPart(Standard.BOOLEAN_TRUE).
    // *  getStatements().add(new ReportStatement("elsif part"));
    // * statement.getElseStatements().add(new ReportStatement("else part"));
    // * ---
    // * if FALSE then
    // *  report "if part";
    // * elsif TRUE then
    // *  report "elsif part";
    // * else
    // *  report "else part";
    // * end if;
    // 
    [Serializable]
    public class IfStatement : SequentialStatement
    {
        private Expression condition;
        private readonly List<SequentialStatement> statements;
        private readonly List<ElsifPart> elsifParts;
        private readonly List<SequentialStatement> elseStatements;

        /// <summary>
        /// Creates an if statement.
        /// </summary>
        /// <param name="condition">the if condition</param>
        public IfStatement(Expression condition)
        {
            statements = ParentSetList<SequentialStatement>.CreateProxyList(this);
            elsifParts = new List<ElsifPart>();
            elseStatements = ParentSetList<SequentialStatement>.CreateProxyList(this);
            this.condition = condition;
        }

        /// <summary>
        /// Returns/Sets the condition for this if statement.
        /// </summary>
        public virtual Expression Condition
        {
            get { return condition; }
            set { condition = value; }
        }

        /// <summary>
        /// Returns the statement.
        /// </summary>
        public virtual List<SequentialStatement> Statements
        {
            get { return statements; }
        }

        /// <summary>
        /// Returns the statement in the else part of the if statement.
        /// If the list is empty no else part will be created.
        /// </summary>
        public virtual List<SequentialStatement> ElseStatements
        {
            get { return elseStatements; }
        }

        /// <summary>
        /// Creates a elsif part and adds it to this if statement.
        /// </summary>
        /// <param name="condition">the condition for the elsif part</param>
        /// <returns>the creates elsif part</returns>
        public virtual ElsifPart createElsifPart(Expression condition)
        {
            ElsifPart part = new ElsifPart(condition);
            elsifParts.Add(part);
            return part;
        }

        /// <summary>
        /// Returns the elsif parts.
        /// </summary>
        public virtual List<ElsifPart> ElsifParts
        {
            get { return elsifParts; }
        }

        internal override void accept(SequentialStatementVisitor visitor)
        {
            visitor.visitIfStatement(this);
        }

        //    *
        //     * Elsif part in an if statement.
        //     
        [Serializable]
        public class ElsifPart
        {
            private Expression condition;
            private readonly List<SequentialStatement> statements = new List<SequentialStatement>();

            public ElsifPart(Expression condition)
                : this(condition, new List<SequentialStatement>())
            {
            }

            public ElsifPart(Expression condition, List<SequentialStatement> statements)
            {
                this.condition = condition;
                this.statements = statements;
            }

            /// <summary>
            /// Returns/Sets the condition for this elsif statement.
            /// </summary>
            public virtual Expression Condition
            {
                get { return condition; }
                set { condition = value; }
            }

            /// <summary>
            /// Returns the statement.
            /// </summary>
            public virtual List<SequentialStatement> Statements
            {
                get { return statements; }
            }
        }

        public override List<VhdlElement> GetAllStatements()
        {
            List<VhdlElement> res = new List<VhdlElement>();
            res.AddRange(statements);
            res.Add(condition);
            foreach (ElsifPart el in elsifParts)
            {
                res.Add(el.Condition);
                res.AddRange(el.Statements);
            }
            res.AddRange(elseStatements);
            return res;
        }
    }

}