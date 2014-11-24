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
    using NamedEntity = VHDL.INamedEntity;
    using VhdlElement = VHDL.VhdlElement;
    using BlockDeclarativeItem = VHDL.declaration.IBlockDeclarativeItem;
    using EntityDeclarativeItem = VHDL.declaration.IEntityDeclarativeItem;
    using PackageBodyDeclarativeItem = VHDL.declaration.IPackageBodyDeclarativeItem;
    using PackageDeclarativeItem = VHDL.declaration.IPackageDeclarativeItem;
    using ProcessDeclarativeItem = VHDL.declaration.IProcessDeclarativeItem;
    using SubprogramDeclarativeItem = VHDL.declaration.ISubprogramDeclarativeItem;

    /// <summary>
    /// Type.
    /// </summary>
    [Serializable]
	public abstract class Type : VhdlElement, BlockDeclarativeItem, EntityDeclarativeItem, PackageBodyDeclarativeItem, PackageDeclarativeItem, ProcessDeclarativeItem, SubprogramDeclarativeItem, ISubtypeIndication, INamedEntity
	{
		private string identifier;

        /// <summary>
        /// Creates a type.
        /// </summary>
        /// <param name="identifier">the type's identifier</param>
		public Type(string identifier)
		{
			this.identifier = identifier;
		}

        /// <summary>
        /// Returns/Sets the identifier of this attribtue.
        /// </summary>
        public virtual string Identifier
        {
            get { return identifier; }
            set { identifier = value; }
        }

		internal abstract void accept(TypeVisitor visitor);

        public void accept(ISubtypeIndicationVisitor visitor)
        {
            visitor.visit(this);
        }

        public ISubtypeIndication BaseType
        {
            get { return null; }
        }
    }

}