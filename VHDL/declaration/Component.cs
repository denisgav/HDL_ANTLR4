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

using VHDL.util;
using System;

namespace VHDL.declaration
{
    using DeclarativeRegion = VHDL.IDeclarativeRegion;
    using Scope = VHDL.IScope;
    using Constant = VHDL.Object.Constant;
    using Entity = VHDL.libraryunit.Entity;
    using NamedEntity = VHDL.INamedEntity;
    using Scopes = VHDL.Scopes;
    using Signal = VHDL.Object.Signal;
    using VhdlObjectProvider = VHDL.Object.IVhdlObjectProvider;
    using VhdlCollections = VHDL.util.VhdlCollections;

    /// <summary>
    /// Component.
    /// </summary>
    [Serializable]
	public class Component : DeclarativeItem, IBlockDeclarativeItem, IPackageDeclarativeItem, INamedEntity, IDeclarativeRegion
	{
		private string identifier;
		private readonly IResolvableList<VhdlObjectProvider> generic;
        private readonly IResolvableList<VhdlObjectProvider> port;
		private readonly IScope scope;

        /// <summary>
        /// Creates a component.
        /// </summary>
        /// <param name="identifier">the component identifier</param>
		public Component(string identifier)
		{
            generic = VhdlCollections.CreateVhdlObjectList<VhdlObjectProvider>();
            port = VhdlCollections.CreateVhdlObjectList<VhdlObjectProvider>();
            scope = Scopes.createScope(this, generic, port);
			this.identifier = identifier;
		}

        /// <summary>
        /// Creates a component based on an entity.
        /// The identifier, port and generic of the entity is used to initialize the component.
        /// </summary>
        /// <param name="entity">the entity</param>
		public Component(Entity entity)
		{
            generic = VhdlCollections.CreateVhdlObjectList<VhdlObjectProvider>();
            port = VhdlCollections.CreateVhdlObjectList<VhdlObjectProvider>();
            scope = Scopes.createScope(this, generic, port);
			this.identifier = entity.Identifier;
            foreach(var o in entity.Generic)
                generic.Add(o);
            foreach (var o in entity.Port)
			    port.Add(o);
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
        /// Returns the generic.
        /// return a list of constants
        /// </summary>
        public virtual IResolvableList<VhdlObjectProvider> Generic
		{
            get { return generic; }
		}

        /// <summary>
        /// Returns the port.
        /// </summary>
        public virtual IResolvableList<VhdlObjectProvider> Port
		{
            get { return port; }
		}

		public virtual IScope Scope
		{
            get { return scope; }
		}

		internal override void accept(DeclarationVisitor visitor)
		{
			visitor.visitComponentDeclaration(this);
		}
	}

}