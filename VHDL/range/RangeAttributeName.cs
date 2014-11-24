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

namespace VHDL
{
    using Expression = VHDL.expression.Expression;

    //TODO: replace class by a better solution
    /// <summary>
    /// Range attribute name.
    /// </summary>
    [Serializable]
    public class RangeAttributeName : RangeProvider
    {
        private string prefix;
        private RangeAttributeNameType type;
        private Expression index;

        /// <summary>
        /// Creates a range attribute name.
        /// </summary>
        /// <param name="prefix">the prefix</param>
        /// <param name="type">the type</param>
        public RangeAttributeName(string prefix, RangeAttributeNameType type)
        {
            this.prefix = prefix;
            this.type = type;
        }

        /// <summary>
        /// Creates a range attribute name.
        /// </summary>
        /// <param name="prefix">the prefix</param>
        /// <param name="type">the type</param>
        /// <param name="index">the index</param>
        public RangeAttributeName(string prefix, RangeAttributeNameType type, Expression index)
        {
            this.prefix = prefix;
            this.type = type;
            this.index = index;
        }

        /// <summary>
        /// Get/Set index
        /// </summary>
        public virtual Expression Index
        {
            get { return index; }
            set { this.index = value; }
        }

        /// <summary>
        /// Get/Set prefix
        /// </summary>
        public virtual string Prefix
        {
            get { return prefix; }
            set { this.prefix = value; }
        }

        /// <summary>
        /// Get/Set type
        /// </summary>
        public virtual RangeAttributeNameType Type
        {
            get { return type; }
            set { this.type = value; }
        }

        public override Choice copy()
        {
            return new RangeAttributeName(prefix, type, index.copy() as Expression);
        }

        //    *
        //     * Type of a range attribute name.
        //     
        public enum RangeAttributeNameType
        {
            /// Name with 'RANGE suffix. 
            RANGE,
            /// Name with 'REVERSE_RANGE suffix. 
            REVERSE_RANGE
        }
    }

}