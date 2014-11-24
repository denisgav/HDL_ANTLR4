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
    using Signal = VHDL.Object.Signal;
    using Expression = VHDL.expression.Expression;

    /// <summary>
    ///  Wait statement.
    /// 
    ///  @VHDL.example
    ///  Signal clk = new Signal("CLK", StdLogic1164.STD_LOGIC);
    ///  WaitStatement statement = new WaitStatement(Expressions.risingEdge(clk));
    ///  ---
    ///  wait for CLK'EVENT and CLK = '1';
    /// </summary>
    [Serializable]
    public class WaitStatement : SequentialStatement
    {
        private readonly List<Signal> sensitivityList = new List<Signal>();
        private Expression condition;
        private Expression timeout;

        /// <summary>
        /// Creates a wait statement.
        /// </summary>
        public WaitStatement()
        {
        }

        /// <summary>
        /// Create a wait statement with a sensitivity list.
        /// </summary>
        /// <param name="sensitivityList">the sensitivity list</param>
        public WaitStatement(params Signal[] sensitivityList)
        {
            this.sensitivityList.AddRange(new List<Signal>(sensitivityList));
        }

        /// <summary>
        /// Create a wait statement with a sensitivity list.
        /// </summary>
        /// <param name="sensitivityList">the sensitivity list</param>
        public WaitStatement(List<Signal> sensitivityList)
        {
            this.sensitivityList.AddRange(sensitivityList);
        }

        /// <summary>
        /// Creates a wait statement with timeout expression.
        /// </summary>
        /// <param name="timeout">the timeout expression</param>
        public WaitStatement(Expression timeout)
        {
            this.timeout = timeout;
        }

        /// <summary>
        /// Creates a wait statement with condition and timeout expression.
        /// </summary>
        /// <param name="condition">the condtion</param>
        /// <param name="timeout">the timeout expression</param>
        public WaitStatement(Expression condition, Expression timeout)
        {
            this.condition = condition;
            this.timeout = timeout;
        }

        /// <summary>
        /// Returns the sensitivity list.
        /// </summary>
        public virtual List<Signal> SensitivityList
        {
            get { return sensitivityList; }
        }

        /// <summary>
        /// Returns/Sets the condition.
        /// </summary>
        public virtual Expression Condition
        {
            get { return condition; }
            set { condition = value; }
        }

        /// <summary>
        /// Returns/Sets the timeout expression.
        /// </summary>
        public virtual Expression Timeout
        {
            get { return timeout; }
            set { timeout = value; }
        }


        internal override void accept(SequentialStatementVisitor visitor)
        {
            visitor.visitWaitStatement(this);
        }

        public override List<VhdlElement> GetAllStatements()
        {
            List<VhdlElement> res = new List<VhdlElement>();
            res.Add(timeout);
            res.Add(condition);
            return res;
        }
    }

}