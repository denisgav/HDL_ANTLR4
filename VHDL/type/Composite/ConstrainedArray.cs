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
using DiscreteRange = VHDL.DiscreteRange;
using System;

namespace VHDL.type
{

    /// <summary>
    /// Constrained array.
    /// </summary>
    [Serializable]
    public class ConstrainedArray : ArrayType
    {
        private readonly List<DiscreteRange> indexRanges;

        /// <summary>
        /// Creates a constrained array.
        /// </summary>
        /// <param name="identifier">the identifier</param>
        /// <param name="elementType">the type of the array elements</param>
        /// <param name="indexRanges">the index ranges</param>
        public ConstrainedArray(string identifier, ISubtypeIndication elementType, params DiscreteRange[] indexRanges)
            : this(identifier, elementType, new List<DiscreteRange>(indexRanges))
        {
        }

        /// <summary>
        /// Creates a constrained array.
        /// </summary>
        /// <param name="identifier">the identifier</param>
        /// <param name="elementType">the type of the array elements</param>
        /// <param name="indexRanges">the index ranges</param>     
        public ConstrainedArray(string identifier, ISubtypeIndication elementType, List<DiscreteRange> indexRanges)
            : base(identifier, elementType)
        {
            this.indexRanges = new List<DiscreteRange>(indexRanges);
        }

        /// <summary>
        /// Returns the index ranges.
        /// </summary>
        public virtual List<DiscreteRange> IndexRanges
        {
            get { return indexRanges; }
        }

        internal override void accept(TypeVisitor visitor)
        {
            visitor.visitConstrainedArray(this);
        }
    }

}