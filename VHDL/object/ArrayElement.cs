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
using VHDL.expression;
using System;

namespace VHDL.Object
{
    using DecimalLiteral = VHDL.literal.DecBasedInteger;
    using SubtypeIndication = VHDL.type.ISubtypeIndication;

    //TODO: check if array element is a valid signal assignment or variable assignment target
    /// <summary>
    /// Array element of a VhdlObject.
    /// </summary>
    [Serializable]
    public class ArrayElement : Name, ISignalAssignmentTarget, IVariableAssignmentTarget
    {
        private readonly Name prefix;
        private readonly List<Expression> indices;

        /// <summary>
        /// Creates an array element.
        /// </summary>
        /// <param name="prefix">the prefix of this array element</param>
        /// <param name="index">the array index</param>
        public ArrayElement(Name prefix, Expression index)
        {
            this.prefix = prefix;
            this.indices = new List<Expression>();
            this.indices.Add(index);
        }

        /// <summary>
        /// Creates an array element.
        /// </summary>
        /// <param name="prefix">the prefix of this array element</param>
        /// <param name="index">the array index</param>
        public ArrayElement(Name prefix, int index)
            : this(prefix, new DecimalLiteral(index))
        {
        }

        /// <summary>
        /// Creates an array element.
        /// </summary>
        /// <param name="prefix">the prefix of this array element</param>
        /// <param name="indices">the array indices</param>
        public ArrayElement(Name prefix, List<Expression> indices)
        {
            this.prefix = prefix;
            this.indices = new List<Expression>();
            this.indices.AddRange(indices);
        }

        /// <summary>
        /// Creates an array element.
        /// </summary>
        /// <param name="prefix">the prefix of this array element</param>
        /// <param name="indices">the array indices</param>
        public ArrayElement(Name prefix, params Expression[] indices)
            : this(prefix, new List<Expression>(indices))
        {
        }

        /// <summary>
        /// Returns the index.
        /// </summary>
        public virtual List<Expression> Indices
        {
            get { return indices; }
        }

        /// <summary>
        /// Returns the prefix of this array element.
        /// </summary>
        public virtual Name Prefix
        {
            get { return prefix; }
        }

        public override SubtypeIndication Type
        {
            get
            {
                //TODO: implement corrently
                return prefix.Type;
            }
        }

        public override void accept(INameVisitor visitor)
        {
            visitor.visit(this);
        }
    }
}