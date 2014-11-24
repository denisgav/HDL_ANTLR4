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

namespace VHDL.expression
{
    using SubtypeIndication = VHDL.type.ISubtypeIndication;

    /// <summary>
    /// Qualified expression.
    /// </summary>
    [Serializable]
	public class QualifiedExpression : Primary
	{

	//TODO: use TypeMark instead of SubtypeIndication
		private SubtypeIndication type;
		private Aggregate operand;

        /// <summary>
        /// Creates a qualified expression.
        /// </summary>
        /// <param name="type">the type</param>
        /// <param name="operand">the operand</param>
		public QualifiedExpression(SubtypeIndication type, Aggregate operand)
		{
			this.type = type;
			this.operand = operand;
		}

        /// <summary>
        /// Creates a qualified expression.
        /// </summary>
        /// <param name="type">the type</param>
        /// <param name="operand">the operand</param>
		public QualifiedExpression(SubtypeIndication type, Expression operand) : this(type, new Aggregate(operand))
		{
		}

        /// <summary>
        /// Returns/Sets the operand.
        /// </summary>
		public virtual Aggregate Operand
		{
            get { return operand; }
            set { operand = value; }
		}

        /// <summary>
        /// Returns the type.
        /// </summary>
		public override SubtypeIndication Type
		{
            get { return type; }
		}
        
        public override Choice copy()
		{
			return new QualifiedExpression(type, operand.copy() as Expression);
		}

        public override void accept(ExpressionVisitor visitor)
		{
			visitor.visitQualifiedExpression(this);
		}
	}

}