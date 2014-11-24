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
using System;
using System.Runtime.Serialization;

namespace VHDL.type
{
///
// * Unconstrained array.
// 
    [Serializable]
	public class UnconstrainedArray : ArrayType
	{
		private readonly List<ISubtypeIndication> indexSubtypes;

        /// <summary>
        /// Creates an unconstrained array.
        /// </summary>
        /// <param name="identifier">the identifier</param>
        /// <param name="elementType">the element type</param>
        /// <param name="indexSubtypes">the index subtypes</param>
		public UnconstrainedArray(string identifier, ISubtypeIndication elementType, params ISubtypeIndication[] indexSubtypes) : this(identifier, elementType, new List<ISubtypeIndication>(indexSubtypes))
		{
		}

        /// <summary>
        /// Creates an unconstrained array.
        /// </summary>
        /// <param name="identifier">the identifier</param>
        /// <param name="elementType">the element type</param>
        /// <param name="indexSubtypes">the index subtypes</param>
		public UnconstrainedArray(string identifier, ISubtypeIndication elementType, List<ISubtypeIndication> indexSubtypes) : base(identifier, elementType)
		{
			this.indexSubtypes = new List<ISubtypeIndication>(indexSubtypes);
		}

        /// <summary>
        /// Returns the index subtypes.
        /// </summary>
		public virtual List<ISubtypeIndication> IndexSubtypes
		{
            get { return indexSubtypes; }
		}

		internal override void accept(TypeVisitor visitor)
		{
			visitor.visitUnconstrainedArray(this);
		}
	}

}