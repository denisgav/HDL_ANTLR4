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
    
    /// <summary>
    /// Return statement.
    /// </summary>
    [Serializable]
    public class ReturnStatement : SequentialStatement
    {
        private Expression returnedExpression;

        /// <summary>
        /// Creates a new ReturnStatement without a returned expression.
        /// </summary>
        public ReturnStatement()
        {
        }

        /// <summary>
        /// Creates a new ReturnStatement with a returned expression.
        /// </summary>
        /// <param name="returnedExpression">the returned expression</param>
        public ReturnStatement(Expression returnedExpression)
        {
            this.returnedExpression = returnedExpression;
        }

        /// <summary>
        /// Returns/Sets the returned expression.
        /// </summary>
        public virtual Expression ReturnedExpression
        {
            get { return returnedExpression; }
            set { returnedExpression = value; }
        }

        internal override void accept(SequentialStatementVisitor visitor)
        {
            visitor.visitReturnStatement(this);
        }

        public override List<VhdlElement> GetAllStatements()
        {
            return new List<VhdlElement>() { returnedExpression };
        }
    }

}