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

using VHDL.Object;
using System;
using System.Collections.Generic;

namespace VHDL.expression
{
    using Attribute = VHDL.declaration.Attribute;
    using SubtypeIndication = VHDL.type.ISubtypeIndication;

    /// <summary>
    /// A name of a VHDL element or function call.
    /// </summary>
    [Serializable]
    public abstract class Name : Primary
    {
        /// <summary>
        /// Returns the type of this object.
        /// </summary>
        public override abstract SubtypeIndication Type { get; }

        /// <summary>
        /// Returns a slice of this object.
        /// </summary>
        /// <param name="range">the slice range.</param>
        /// <returns>the slice</returns>
        public Slice getSlice(List<DiscreteRange> ranges)
        {
            //safe if T extends VhdlObject<T>
            return new Slice(this, ranges);
        }

        /// <summary>
        /// Returns an array element of this object.
        /// </summary>
        /// <param name="index">the index of the array element</param>
        /// <returns>the array element</returns>
        public virtual ArrayElement getArrayElement(Expression index)
        {
            //safe if T extends VhdlObject<T>
            return new ArrayElement(this, index);
        }

        /// <summary>
        /// Returns an array element of this object.
        /// </summary>
        /// <param name="index">the index of the array element</param>
        /// <returns>the array element</returns>
        public virtual ArrayElement getArrayElement(int index)
        {
            //safe if T extends VhdlObject<T>
            return new ArrayElement(this, index);
        }

        /// <summary>
        /// Returns an array element of this object.
        /// </summary>
        /// <param name="indices">the indices of the array element</param>
        /// <returns>the array element</returns>
        public virtual ArrayElement getArrayElement(List<Expression> indices)
        {
            //safe if T extends VhdlObject<T>
            return new ArrayElement(this, indices);
        }

        /// <summary>
        /// Returns an array element of this object.
        /// </summary>
        /// <param name="indices">the indices of the array element</param>
        /// <returns>array element</returns>
        public virtual ArrayElement getArrayElement(params Expression[] indices)
        {
            //safe if T extends VhdlObject<T>
            return new ArrayElement(this, indices);
        }

        /// <summary>
        /// Returns a record element of this object.
        /// </summary>
        /// <param name="element">the identifier of the record element</param>
        /// <returns>record element</returns>
        public virtual RecordElement getRecordElement(string element)
        {
            //safe if T extends VhdlObject<T>			
            return new RecordElement(this, element);
        }

        /// <summary>
        /// Returns a attribute expression of this object.
        /// </summary>
        /// <param name="attribute">the attribute</param>
        /// <returns>the record element</returns>
        public virtual AttributeExpression getAttributeExpression(Attribute attribute)
        {
            //safe if T extends VhdlObject<T>
            return new AttributeExpression(this, attribute);
        }

        /// <summary>
        /// Returns a attribute expression of this object.
        /// </summary>
        /// <param name="attribute">the attribute</param>
        /// <param name="parameter">the parameter</param>
        /// <returns>the record element</returns>
        public virtual AttributeExpression getAttributeExpression(Attribute attribute, List<Expression> parameters)
        {
            //safe if T extends VhdlObject<T>
            return new AttributeExpression(this, attribute, parameters);
        }

        public override void accept(ExpressionVisitor visitor)
        {
            visitor.visitName(this);
        }

        public override Choice copy()
        {
            return this;
        }
    }

}