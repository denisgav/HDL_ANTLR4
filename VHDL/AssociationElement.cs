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

using Expression = VHDL.expression.Expression;
using System;

namespace VHDL
{
    /// <summary>
    /// Association element.
    /// </summary>
    /// FIXME: dummy implementation
    [Serializable]
    public class AssociationElement : VhdlElement
    {
        private string formal;
        private Expression actual;

        /// <summary>
        /// Creates an association element without a formal part.
        /// </summary>
        /// <param name="actual">actual the actual part</param>
        public AssociationElement(Expression actual)
        {
            this.actual = actual;
        }

        /// <summary>
        /// Creates an association element with a formal and actual part.
        /// </summary>
        /// <param name="formal">the formal part</param>
        /// <param name="actual">the actual part</param>
        public AssociationElement(string formal, Expression actual)
        {
            this.formal = formal;
            this.actual = actual;
        }

        //    *
        //     * 
        //     * @return the actual part
        //     
        /// <summary>
        /// Return/Set the actual part of this association.
        /// </summary>
        public virtual Expression Actual
        {
            get { return actual; }
            set { this.actual = value; }
        }

        /// <summary>
        /// Returns/Sets the formal part of this association.
        /// </summary>
        public virtual string Formal
        {
            get { return formal; }
            set { this.formal = value; }
        }
    }
}