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
    using Scope = VHDL.IScope;
    using Scopes = VHDL.Scopes;
    using PackageDeclarativeItem = VHDL.declaration.IPackageDeclarativeItem;
    using VhdlCollections = VHDL.util.VhdlCollections;

    /// <summary>
    /// PackageDeclaration declaration.
    /// </summary>
    [Serializable]
    public class PackageDeclaration : PrimaryUnit, INamedEntity
    {
        private string identifier;
        private readonly IResolvableList<PackageDeclarativeItem> declarations;
        //TODO: also resolve the package declaration?
        private readonly IScope scope;
        [NonSerialized]
        private PackageBody packageBody;

        /// <summary>
        /// Creates a package declaration.
        /// </summary>
        /// <param name="identifier">the package identifier</param>
        public PackageDeclaration(string identifier)
        {
            declarations = VhdlCollections.CreateDeclarationList<PackageDeclarativeItem>();
            scope = Scopes.createScope(this, declarations, new LibraryUnitResolvable(this));
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


        public PackageBody PackageBody
        {
            get { return packageBody; }
            set { packageBody = value; }
        }

        /// <summary>
        /// Returns the list of declarations in this package.
        /// </summary>
        public virtual IList<PackageDeclarativeItem> Declarations
        {
            get { return declarations; }
        }

        public override IScope Scope
        {
            get { return scope; }
        }

        internal override void accept(LibraryUnitVisitor visitor)
        {
            visitor.visitPackageDeclaration(this);
        }

        public override string ToString()
        {
            return identifier;
        }
    }
}