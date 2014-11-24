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
    using Resolvable = VHDL.IResolvable;
    using Scope = VHDL.IScope;
    using Scopes = VHDL.Scopes;
    using PackageBodyDeclarativeItem = VHDL.declaration.IPackageBodyDeclarativeItem;
    using VhdlCollections = VHDL.util.VhdlCollections;

    /// <summary>
    /// PackageDeclaration body.
    /// </summary>
    [Serializable]
    public class PackageBody : SecondaryUnit
    {
        private PackageDeclaration pack;
        private readonly IResolvableList<PackageBodyDeclarativeItem> declarations;
        private readonly IResolvable resolvable;
        private readonly IScope scope;

        /// <summary>
        /// Creates a package body.
        /// </summary>
        /// <param name="pack">the associated package</param>
        public PackageBody(PackageDeclaration pack)
            : base(pack)
        {
            declarations = VhdlCollections.CreateDeclarationList<PackageBodyDeclarativeItem>();
            resolvable = new ResolvableImpl(this);
            scope = Scopes.createScope(this, declarations, resolvable, new LibraryUnitResolvable(this));

            this.pack = pack;
            pack.PackageBody = this;
        }

        /// <summary>
        /// Returns/Sets the associated package.
        /// </summary>
        public virtual PackageDeclaration Package
        {
            get { return pack; }
            set { pack = value; }
        }

        /// <summary>
        /// Returns the list of declarations in this package body.
        /// </summary>
        public virtual IList<PackageBodyDeclarativeItem> Declarations
        {
            get { return declarations; }
        }

        public override IScope Scope
        {
            get { return scope; }
        }

        internal override void accept(LibraryUnitVisitor visitor)
        {
            visitor.visitPackageBody(this);
        }

        public override string ToString()
        {
            return pack.Identifier;
        }

        [Serializable]
        private class ResolvableImpl : IResolvable
        {

            private PackageBody parent;
            public ResolvableImpl(PackageBody parent)
            {
                this.parent = parent;
            }

            public virtual object Resolve(string identifier)
            {
                if (parent.pack != null)
                {
                    return parent.pack.Scope.resolveLocal(identifier);
                }

                return null;
            }



            public List<object> GetListOfObjects()
            {
                List<object> res = new List<object>();
                res.AddRange(parent.pack.Scope.GetListOfObjects());
                return res;
            }

            public List<object> GetLocalListOfObjects()
            {
                List<object> res = new List<object>();
                res.AddRange(parent.pack.Scope.GetLocalListOfObjects());
                return res;
            }
        }
    }

}