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

namespace VHDL.statement
{
    using Expression = VHDL.expression.Expression;
    
    /// <summary>
    /// * While loop.
    /// * Variable i = new Variable("i", Standard.INTEGER, new DecimalLiteral(0));
    /// * Expression condition = new LessThan(i, new DecimalLiteral(100));
    /// * WhileStatement loop = new WhileStatement(condition);
    /// * loop.getStatements().add(new ReportStatement("loop"));
    /// * Expression addExpression = new Add(i, new DecimalLiteral(1));
    /// * loop.getStatements().add(new VariableAssignment(i, addExpression));
    /// * ---
    /// * while i &lt; 100 loop
    /// *   report "loop";
    /// *  i := i + 1;
    /// * end loop;
    /// </summary>
    [Serializable]
    public class WhileStatement : LoopStatement
    {
        private Expression condition;

        /// <summary>
        /// Creates a while loop.
        /// </summary>
        /// <param name="condition">the while condition</param>
        public WhileStatement(Expression condition)
        {
            this.condition = condition;
        }

        /// <summary>
        /// Returns/Sets the condition.
        /// </summary>
        public virtual Expression Condition
        {
            get { return condition; }
            set { condition = value; }
        }


        internal override void accept(SequentialStatementVisitor visitor)
        {
            visitor.visitWhileStatement(this);
        }
    }

}