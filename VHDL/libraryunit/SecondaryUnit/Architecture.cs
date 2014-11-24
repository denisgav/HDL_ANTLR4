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
    using NamedEntity = VHDL.INamedEntity;
    using Resolvable = VHDL.IResolvable;
    using Scope = VHDL.IScope;
    using Scopes = VHDL.Scopes;
    using ConcurrentStatement = VHDL.concurrent.ConcurrentStatement;
    using BlockDeclarativeItem = VHDL.declaration.IBlockDeclarativeItem;
    using VhdlCollections = VHDL.util.VhdlCollections;

    /// <summary>
    /// Architecture body.
    /// </summary>
    [Serializable]
    public class Architecture : SecondaryUnit, INamedEntity
    {
        private string identifier;
        private Entity entity;
        private readonly IResolvableList<BlockDeclarativeItem> declarations;
        private readonly IResolvableList<ConcurrentStatement> statements;
        private readonly IResolvable resolvable;
        private readonly IScope scope;

        /// <summary>
        /// Creates an architecture.
        /// </summary>
        /// <param name="identifier">the architectures identifier</param>
        /// <param name="entity">the associated entity</param>
        public Architecture(string identifier, Entity entity)
            : base(entity)
        {
            declarations = VhdlCollections.CreateDeclarationList<BlockDeclarativeItem>();
            statements = VhdlCollections.CreateLabeledElementList<ConcurrentStatement>(this);
            this.identifier = identifier;
            this.entity = entity;
            resolvable = new ResolvableImpl(this);
            scope = Scopes.createScope(this, declarations, statements, resolvable, new LibraryUnitResolvable(this));
            entity.AddArchitecture(this);
        }

        /// <summary>
        /// Returns/Sets the entity that belogs to this architecture.
        /// </summary>
        public virtual Entity Entity
        {
            get { return entity; }
            set { entity = value; }
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
        /// Returns the list of declarations in this architecture.
        /// </summary>
        public virtual IResolvableList<BlockDeclarativeItem> Declarations
        {
            get { return declarations; }
        }

        /// <summary>
        /// Returns the list of statements in this architecture.
        /// </summary>
        public virtual IResolvableList<ConcurrentStatement> Statements
        {
            get { return statements; }
        }

        public override IScope Scope
        {
            get { return scope; }
        }

        internal override void accept(LibraryUnitVisitor visitor)
        {
            visitor.visitArchitecture(this);
        }

        public override string ToString()
        {
            return identifier;
        }

        [Serializable]
        private class ResolvableImpl : IResolvable
        {
            private Architecture parent;
            public ResolvableImpl(Architecture parent)
            {
                this.parent = parent;
            }

            public virtual object Resolve(string identifier)
            {
                if (parent.Entity != null)
                {
                    return parent.Entity.Scope.resolveLocal(identifier);
                }
                return null;
            }


            public List<object> GetListOfObjects()
            {
                List<object> res = new List<object>();
                res.AddRange(parent.Entity.Scope.GetListOfObjects());
                return res;
            }

            public List<object> GetLocalListOfObjects()
            {
                return new List<object>();
            }
        }
    }
}