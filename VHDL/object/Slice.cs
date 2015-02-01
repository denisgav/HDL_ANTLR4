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

namespace VHDL.Object
{
    using VHDL.expression;
    using SubtypeIndication = VHDL.type.ISubtypeIndication;
    using System.Collections.Generic;

    //TODO: check if slice is a valid signal assignment or variable assignment target
    /// <summary>
    /// Slice of a VhdlObject.
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

        public override SubtypeIndication Type
        {
            get
            {
                //TODO: implement correctly
                return prefix.Type;
            }
        }

        public override void accept(INameVisitor visitor)
        {
            visitor.visit(this);
        }
    }

}