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
using Choice = VHDL.Choice;
using Expression = VHDL.expression.Expression;
using System;

namespace VHDL.statement
{
///
// * Case statement.
// * 
// *
// * @VHDL.example
// * EnumerationType stateType = new EnumerationType("STATE_TYPE", "IDLE", "RUN");
// * Signal state = new Signal("STATE", stateType);
// * CaseStatement statement = new CaseStatement(state);
// * statement.createAlternative(stateType.getLiterals().get(0)).
// *  getStatements().add(new ReportStatement("state is idle"));
// * statement.createAlternative(Choices.OTHERS).
// *  getStatements().add(new ReportStatement("state is not idle"));
// * ---
// * case STATE is
// *  when IDLE =>
// *   report "state is idle";
// *
// *  when others =>
// *   report "state is not idle";
// * end case;
// 
    [Serializable]
	public class CaseStatement : SequentialStatement
	{
		private Expression expression;
		private readonly List<Alternative> alternatives = new List<Alternative>();

        /// <summary>
        /// Creates a case statement.
        /// </summary>
        /// <param name="expression">the expression</param>
		public CaseStatement(Expression expression)
		{
			this.expression = expression;
		}

        /// <summary>
        /// Returns/Sets the expression.
        /// </summary>
		public virtual Expression Expression
		{
            get { return expression; }
            set { expression = value; }
		}

        /// <summary>
        /// Creates a new alternative and adds it to this case statement.
        /// </summary>
        /// <param name="choices">one or more choices that select this alternative</param>
        /// <returns>created alternative</returns>
		public virtual Alternative createAlternative(params Choice[] choices)
		{
			Alternative alternative = new Alternative(choices);
			alternatives.Add(alternative);
			return alternative;
		}

        /// <summary>
        /// Creates a new alternative and adds it to this case statement.
        /// </summary>
        /// <param name="choices">a list of choices that select this alternative</param>
        /// <returns>the created alternative</returns>
		public virtual Alternative createAlternative(List<Choice> choices)
		{
			Alternative alternative = new Alternative(choices);
			alternatives.Add(alternative);
			return alternative;
		}

        /// <summary>
        /// Returns the alternatives.
        /// </summary>
		public virtual List<Alternative> Alternatives
		{
            get { return alternatives; }
		}

		internal override void accept(SequentialStatementVisitor visitor)
		{
			visitor.visitCaseStatement(this);
		}

//    *
//     * Case statement alternative.
//     
        [Serializable]
		public sealed class Alternative
		{
			private readonly List<Choice> choices;
			private readonly List<SequentialStatement> statements = new List<SequentialStatement>();

			public Alternative(params Choice[] choices) : this(new List<Choice>(choices))
			{
			}

            public Alternative(List<Choice> choices)
			{
				this.choices = new List<Choice>(choices);
			}

            /// <summary>
            /// Returns the choices.
            /// </summary>
			public List<Choice> Choices
			{
                get { return choices; }
			}

            /// <summary>
            /// Returns the statements.
            /// </summary>
			public List<SequentialStatement> Statements
			{
                get { return statements; }
			}
		}

        public override List<VhdlElement> GetAllStatements()
        {
            List<VhdlElement> res = new List<VhdlElement>();
            res.Add(expression);
            foreach (Alternative el in alternatives)
            {
                res.AddRange(el.Statements);
                res.AddRange(el.Choices);
            }
            return res;
        }
    }

}