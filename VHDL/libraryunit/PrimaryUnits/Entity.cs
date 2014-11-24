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

namespace VHDL.libraryunit
{
    using Constant = VHDL.Object.Constant;
    using NamedEntity = VHDL.INamedEntity;
    using Scope = VHDL.IScope;
    using Scopes = VHDL.Scopes;
    using Signal = VHDL.Object.Signal;
    using EntityStatement = VHDL.concurrent.EntityStatement;
    using EntityDeclarativeItem = VHDL.declaration.IEntityDeclarativeItem;
    using VhdlObjectProvider = VHDL.Object.IVhdlObjectProvider;
    using VhdlCollections = VHDL.util.VhdlCollections;

    /// <summary>
    /// Entity declaration.
    /// </summary>
    [Serializable]
    public class Entity : PrimaryUnit, INamedEntity
    {
        private string identifier;
        private readonly IResolvableList<VhdlObjectProvider> port;
        private readonly IResolvableList<VhdlObjectProvider> generic;
        private readonly IResolvableList<EntityDeclarativeItem> declarations;
        private readonly List<EntityStatement> statements;
        [NonSerialized]
        private readonly List<Architecture> architectures;
        private readonly IScope scope;

        /// <summary>
        /// Creates a entity.
        /// </summary>
        /// <param name="identifier">the identifier</param>
        public Entity(string identifier)
        {
            this.identifier = identifier;

            port = VhdlCollections.CreateVhdlObjectList<VhdlObjectProvider>();
            generic = VhdlCollections.CreateVhdlObjectList<VhdlObjectProvider>();
            declarations = VhdlCollections.CreateDeclarationList<EntityDeclarativeItem>();
            architectures = new List<Architecture>();

            statements = new List<EntityStatement>();
            scope = Scopes.createScope(this, generic, port, declarations, new LibraryUnitResolvable(this));
        }

        public void AddArchitecture(Architecture arch)
        {
            if (architectures.Contains(arch) == false)
            {
                architectures.Add(arch);
            }
        }

        /// <summary>
        /// Returns/Sets the identifier of this attribtue.
        /// </summary>
        public virtual string Identifier
        {
            get { return identifier; }
            set { identifier = value; }
        }

        public IList<Architecture> Architectures
        {
            get { return architectures; }
        }

        /// <summary>
        /// Returns the list of declarations in this entity.
        /// </summary>
        public virtual IList<EntityDeclarativeItem> Declarations
        {
            get { return declarations; }
        }

        /// <summary>
        /// Returns the list of statements in this entity.
        /// </summary>
        public virtual IList<EntityStatement> Statements
        {
            get { return statements; }
        }

        /// <summary>
        /// Returns the generic.
        /// </summary>
        public virtual IList<VhdlObjectProvider> Generic
        {
            get { return generic; }
        }

        /// <summary>
        /// Returns the port.
        /// </summary>
        public virtual IList<VhdlObjectProvider> Port
        {
            get { return port; }
        }

        public override IScope Scope
        {
            get { return scope; }
        }

        internal override void accept(LibraryUnitVisitor visitor)
        {
            visitor.visitEntity(this);
        }

        public override string ToString()
        {
            return identifier;
        }
    }
}