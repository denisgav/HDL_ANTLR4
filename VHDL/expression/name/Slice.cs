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
using VHDL.expression;
using VHDL.Object;
using VHDL.type;

namespace VHDL.expression.name
{
    //TODO: check if slice is a valid signal assignment or variable assignment target
    /// <summary>
    /// A slice name denotes a one-dimensional array composed of a sequence of
    /// consecutive elements of another one-dimensional array.
    /// </summary>
    [Serializable]
    public class Slice : Name, ISignalAssignmentTarget, IVariableAssignmentTarget
    {
        private readonly Name prefix;
        private readonly List<DiscreteRange> ranges;

        /// <summary>
        /// Creates a slice.
        /// </summary>
        /// <param name="prefix">the slice prefix</param>
        /// <param name="range">the range</param>
        public Slice(Name prefix, List<DiscreteRange> ranges)
        {
            this.prefix = prefix;
            this.ranges = ranges;
        }

        /// <summary>
        /// Returns the prefix of this slice.
        /// </summary>
        public virtual Name Prefix
        {
            get { return prefix; }
        }

        /// <summary>
        /// Returns the range of this slice.
        /// </summary>
        public virtual List<DiscreteRange> Ranges
        {
            get { return ranges; }
        }

        public override ISubtypeIndication Type
        {
            get
            {
                //TODO: implement correctly
                return prefix.Type;
            }
        }

        public override INamedEntity Referenced
        {
            get { return prefix.Referenced; }
        }

        public override void accept(INameVisitor visitor)
        {
            visitor.visit(this);
        }
    }

}