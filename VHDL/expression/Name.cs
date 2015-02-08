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
using VHDL.Object;
using VHDL.expression.name;

namespace VHDL.expression
{
    using Attribute = VHDL.declaration.Attribute;
    using SubtypeIndication = VHDL.type.ISubtypeIndication;

    /// <summary>
    /// Names can denote declared entities, whether declared explicitly or implicitly.
    /// Names can also denote the following:
    ///   — Objects denoted by access values
    ///   — Methods (see 5.6.2) of protected types
    ///   — Subelements of composite objects
    ///   — Subelements of composite values
    ///   — Slices of composite objects
    ///   — Slices of composite values
    ///   — Attributes of any named entity
    /// </summary>
    [Serializable]
    public abstract class Name : Primary
    {
        /// <summary>
        /// Returns the type of this object.
        /// </summary>
        public override abstract SubtypeIndication Type { get; }

        public abstract INamedEntity Referenced { get; }

        public static SimpleName reference(VhdlObject obj)
        {
            return new SimpleName(obj);
        }

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
        public virtual IndexedName getArrayElement(Expression index)
        {
            //safe if T extends VhdlObject<T>
            return new IndexedName(this, index);
        }

        /// <summary>
        /// Returns an array element of this object.
        /// </summary>
        /// <param name="index">the index of the array element</param>
        /// <returns>the array element</returns>
        public virtual IndexedName getArrayElement(int index)
        {
            //safe if T extends VhdlObject<T>
            return new IndexedName(this, index);
        }

        /// <summary>
        /// Returns an array element of this object.
        /// </summary>
        /// <param name="indices">the indices of the array element</param>
        /// <returns>the array element</returns>
        public virtual IndexedName getArrayElement(List<Expression> indices)
        {
            //safe if T extends VhdlObject<T>
            return new IndexedName(this, indices);
        }

        /// <summary>
        /// Returns an array element of this object.
        /// </summary>
        /// <param name="indices">the indices of the array element</param>
        /// <returns>array element</returns>
        public virtual IndexedName getArrayElement(params Expression[] indices)
        {
            //safe if T extends VhdlObject<T>
            return new IndexedName(this, indices);
        }

        /// <summary>
        /// Returns a record element of this object.
        /// </summary>
        /// <param name="element">the identifier of the record element</param>
        /// <returns>record element</returns>
        public virtual SelectedName getRecordElement(string element)
        {
            //safe if T extends VhdlObject<T>			
            return new SelectedName(this, element);
        }

        /// <summary>
        /// Returns a attribute expression of this object.
        /// </summary>
        /// <param name="attribute">the attribute</param>
        /// <returns>the record element</returns>
        public virtual AttributeName getAttributeExpression(Attribute attribute)
        {
            //safe if T extends VhdlObject<T>
            return new AttributeName(this, attribute);
        }

        /// <summary>
        /// Returns a attribute expression of this object.
        /// </summary>
        /// <param name="attribute">the attribute</param>
        /// <param name="parameter">the parameter</param>
        /// <returns>the record element</returns>
        public virtual AttributeName getAttributeExpression(Attribute attribute, List<Expression> parameters)
        {
            //safe if T extends VhdlObject<T>
            return new AttributeName(this, attribute, parameters);
        }

        public override void accept(ExpressionVisitor visitor)
        {
            visitor.visitName(this);
        }

        public abstract void accept(INameVisitor visitor);

        public override Choice copy()
        {
            return this;
        }
    }

}