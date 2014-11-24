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
    using RangeProvider = VHDL.RangeProvider;

    /// <summary>
    /// Integer type.
    /// </summary>
    [Serializable]
    public class IntegerType : Type
    {
        private RangeProvider range;

        /// <summary>
        /// Creates a integer type.
        /// </summary>
        /// <param name="identifier">the identifier of this type</param>
        /// <param name="range">the range of this integer type</param>
        public IntegerType(string identifier, RangeProvider range)
            : base(identifier)
        {
            this.range = range;
        }

        // <summary>
        /// Returns/Sets the range of this type
        /// </summary>
        public virtual RangeProvider Range
        {
            get { return range; }
            set { range = value; }
        }

        internal override void accept(TypeVisitor visitor)
        {
            visitor.visitIntegerType(this);
        }
    }

}