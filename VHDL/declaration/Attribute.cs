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

namespace VHDL.declaration
{
    using NamedEntity = VHDL.INamedEntity;
    using SubtypeIndication = VHDL.type.ISubtypeIndication;
///
// * Attribute declaration.
// 
    [Serializable]
	public class Attribute : DeclarativeItem, IBlockDeclarativeItem, IEntityDeclarativeItem, IPackageDeclarativeItem, IProcessDeclarativeItem, ISubprogramDeclarativeItem, INamedEntity
	{
		private string identifier;
	//FIXME: use type mark instead of subtype indication
		private SubtypeIndication type;

        /// <summary>
        /// Creates a attribute declartion.
        /// </summary>
        /// <param name="identifier">the identifer</param>
        /// <param name="type">the type of this attribtue</param>
		public Attribute(string identifier, SubtypeIndication type)
		{
			this.identifier = identifier;
			this.type = type;
		}

        /// <summary>
        /// Returns/Sets the identifier of this attribtue.
        /// </summary>
		public virtual string Identifier
		{
            get { return identifier; }
            set { identifier = value; }
		}

        /// <summary>
        /// Returns/Sets the type of this attribtue.
        /// </summary>
		public virtual SubtypeIndication Type
		{
            get { return type; }
            set { type = value; }
		}

		internal override void accept(DeclarationVisitor visitor)
		{
			visitor.visitAttributeDeclaration(this);
		}
	}
}