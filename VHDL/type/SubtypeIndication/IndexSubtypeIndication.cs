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


namespace VHDL.type
{
    using DiscreteRange = VHDL.DiscreteRange;

    /// <summary>
    /// Index constraint subtype indication.
    /// </summary>
    [Serializable]
	public class IndexSubtypeIndication : ISubtypeIndication
	{
		private ISubtypeIndication baseType;
		private readonly List<DiscreteRange> ranges;

        /// <summary>
        /// Creates a index subtype indication.
        /// </summary>
        /// <param name="baseType">the base type</param>
        /// <param name="ranges">the ranges</param>
		public IndexSubtypeIndication(ISubtypeIndication baseType, List<DiscreteRange> ranges)
		{
			this.baseType = baseType;
			this.ranges = new List<DiscreteRange>(ranges);
		}

        /// <summary>
        /// Creates a index subtype indication.
        /// </summary>
        /// <param name="baseType">the base type</param>
        /// <param name="ranges">the ranges</param>
        public IndexSubtypeIndication(ISubtypeIndication baseType, params DiscreteRange[] ranges)
            : this(baseType, new List<DiscreteRange>(ranges))
		{
		}

        /// <summary>
        /// Returns/Sets the base type.
        /// </summary>
        public virtual ISubtypeIndication BaseType
        {
            get { return baseType; }
            set { baseType = value; }
        }

        /// <summary>
        /// Returns the index ranges.
        /// </summary>
		public virtual List<DiscreteRange> Ranges
		{
            get { return ranges; }
		}

        public void accept(VHDL.type.ISubtypeIndicationVisitor visitor)
        {
            visitor.visit(this);
        }

    }

}