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
    using VariableAssignmentTarget = VHDL.Object.IVariableAssignmentTarget;

    ///
    /// <summary>
    /// * Variable assignment.
    /// *
    /// * Variable target = new Variable("TARGET", Standard.BIT);
    /// * VariableAssignment assignment = new VariableAssignment(target, Standard.BIT_1);
    /// * ---
    /// * TARGET := '1';
    /// </summary>
    [Serializable]
    public class VariableAssignment : SequentialStatement
    {
        private VariableAssignmentTarget target;
        private Expression @value;

        /// <summary>
        /// Creates a variable assignment.
        /// </summary>
        /// <param name="target">the variable assignment target</param>
        /// <param name="value">the assigned value</param>
        public VariableAssignment(VariableAssignmentTarget target, Expression @value)
        {
            this.target = target;
            this.value = @value;
        }

        /// <summary>
        /// Returns/Sets the variable assignment target.
        /// </summary>
        public virtual VariableAssignmentTarget Target
        {
            get { return target; }
            set { target = value; }
        }

        //    *
        //     * Returns the assigned value.
        //     * @return the value
        //     
        public virtual Expression Value
        {
            get
            {
                return @value;
            }
            set
            {
                this.value = value;
            }
        }

        //    *
        //     * Sets the assigned value.
        //     * @param value the value
        //     

        internal override void accept(SequentialStatementVisitor visitor)
        {
            visitor.visitVariableAssignment(this);
        }

        public override List<VhdlElement> GetAllStatements()
        {
            List<VhdlElement> res = new List<VhdlElement>();
            res.Add(value);
            return res;
        }
    }

}