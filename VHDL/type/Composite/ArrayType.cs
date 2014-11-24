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

namespace VHDL.type
{
    /// <summary>
    /// Abstract base class for array types.
    /// </summary>
    [Serializable]
    public abstract class ArrayType : Type
    {
        private ISubtypeIndication elementType;

        /// <summary>
        /// Creates array type.
        /// </summary>
        /// <param name="identifier">identifier the identifier</param>
        /// <param name="elementType">the type of the array elements</param>
        public ArrayType(string identifier, ISubtypeIndication elementType)
            : base(identifier)
        {
            this.elementType = elementType;
        }

        /// <summary>
        /// Returns/Sets the type of the array elements.
        /// </summary>
        public virtual ISubtypeIndication ElementType
        {
            get { return elementType; }
            set { elementType = value; }
        }

    }

}