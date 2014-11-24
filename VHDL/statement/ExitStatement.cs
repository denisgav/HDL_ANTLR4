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
    // * Exit statement.
    // *
    // * @VHDL.example
    // * ForStatement loop = new ForStatement("I",
    // *  new Range(0, Range.Direction.TO, 127));
    // * loop.setLabel("I_LOOP");
    // *
    // * loop.getStatements().add(new ExitStatement(loop,
    // *  new Equals(loop.getParameter(), new DecimalLiteral(100))));
    // * ---
    // * I_LOOP : for I in 0 to 127 loop
    // *  exit I_LOOP when I = 100;
    // * end loop;
    // 
    [Serializable]
    public class ExitStatement : SequentialStatement
    {
        private LoopStatement loop;
        private Expression condition;

        /// <summary>
        /// Creates an exit statement
        /// </summary>
        public ExitStatement()
        {
        }

        /// <summary>
        /// Creates an exit statement for the given loop.
        /// </summary>
        /// <param name="loop">the loop</param>
        public ExitStatement(LoopStatement loop)
        {
            this.loop = loop;
        }

        /// <summary>
        /// Creates an exit statement with the given condition.
        /// </summary>
        /// <param name="condition">the condition</param>
        public ExitStatement(Expression condition)
        {
            this.condition = condition;
        }

        /// <summary>
        /// Creates an exit statement for the given loop with a condition.
        /// </summary>
        /// <param name="loop">the loop</param>
        /// <param name="condition">the condition</param>
        public ExitStatement(LoopStatement loop, Expression condition)
        {
            this.loop = loop;
            this.condition = condition;
        }

        /// <summary>
        /// Returns/Sets the condition for this exit statement.
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
            visitor.visitExitStatement(this);
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