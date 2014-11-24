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
    /// Access type.
    /// </summary>
    [Serializable]
	public class AccessType : Type
	{
		private ISubtypeIndication designatedSubtype;

        /// <summary>
        /// Creates a access type.
        /// </summary>
        /// <param name="identifier">the identifier</param>
        /// <param name="designatedSubtype">the designated subtype</param>
		public AccessType(string identifier, ISubtypeIndication designatedSubtype) : base(identifier)
		{
			this.designatedSubtype = designatedSubtype;
		}

        /// <summary>
        /// Returns/Sets the designated subtype.
        /// </summary>
        public virtual ISubtypeIndication DesignatedSubtype
        {
            get { return designatedSubtype; }
            set { designatedSubtype = value; }
        }

		internal override void accept(TypeVisitor visitor)
		{
			visitor.visitAccessType(this);
		}
	}

}