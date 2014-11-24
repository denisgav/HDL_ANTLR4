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
    ///
    
    // 
    /// <summary>
    /// Next statement.
    /// 
    /// LoopStatement loop = new LoopStatement();
    ///  loop.setLabel("INF_LOOP");
    ///  loop.getStatements().add(new NextStatement(loop));
    ///  loop.getStatements().add(new ReportStatement("not reached"));
    ///  ---
    ///  INF_LOOP : loop
    ///   next INF_LOOP;
    ///   report "not reached";
    ///  end loop;
    /// </summary>
    [Serializable]
    public class NextStatement : SequentialStatement
    {
        private LoopStatement loop;
        private Expression condition;

        /// <summary>
        /// Creates a next statement.
        /// </summary>
        public NextStatement()
        {
        }

        /// <summary>
        /// Creates a next statement for the given loop.
        /// </summary>
        /// <param name="loop">the loop</param>
        public NextStatement(LoopStatement loop)
        {
            this.loop = loop;
        }

        /// <summary>
        /// Creates a next statement with the given condition.
        /// </summary>
        /// <param name="condition">the condition</param>
        public NextStatement(Expression condition)
        {
            this.condition = condition;
        }

        /// <summary>
        /// Creates a next statement for the given loop with a condition.
        /// </summary>
        /// <param name="loop">the loop</param>
        /// <param name="condition">the condition</param>
        public NextStatement(LoopStatement loop, Expression condition)
        {
            this.loop = loop;
            this.condition = condition;
        }

        /// <summary>
        /// Returns/Sets the condition for this next statement.
        /// </summary>
        public virtual Expression Condition
        {
            get { return condition; }
            set { condition = value; }
        }

        /// <summary>
        /// Returns/Sets the associated loop statement.
        /// </summary>
        public virtual LoopStatement Loop
        {
            get { return loop; }
            set { loop = value; }
        }

        internal override void accept(SequentialStatementVisitor visitor)
        {
            visitor.visitNextStatement(this);
        }

        public override List<VhdlElement> GetAllStatements()
        {
            List<VhdlElement> res = new List<VhdlElement>();
            res.Add(loop);
            res.Add(condition);
            return res;
        }
    }

}