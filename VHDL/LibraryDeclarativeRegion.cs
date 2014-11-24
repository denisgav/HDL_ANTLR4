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
using LibraryUnit = VHDL.libraryunit.LibraryUnit;
using VHDL.util;
using System;
using VHDL.libraryunit;
using VHDL.declaration;
using VHDL.type;
using VHDL.literal;
using VHDL.Object;

namespace VHDL
{

    //TODO: rename class to Library

    /// <summary>
    /// Library declarative region.
    /// </summary>
    [Serializable]
    public class LibraryDeclarativeRegion : VhdlElement, IDeclarativeRegion, INamedEntity
    {
        private readonly ParentSetList<VhdlFile> files;
        private string identifier;
        private readonly IResolvable resolvable;
        private readonly IScope scope;

        /// <summary>
        /// Creates a library declarative region.
        /// </summary>
        /// <param name="identifier">the identifier of the library</param>
        public LibraryDeclarativeRegion(string identifier)
        {
            files = ParentSetList<VhdlFile>.Create(this);
            resolvable = new ResolvableImpl(this);
            scope = Scopes.createScope(this, resolvable);
            this.identifier = identifier;
        }

        /// <summary>
        /// Returns/Sets the identifier of this library declarative region.
        /// </summary>
        public virtual string Identifier
        {
            get { return identifier; }
            set { identifier = value; }
        }

        /// <summary>
        /// Returns a list of files included in this libray.
        /// </summary>
        /// <returns></returns>
        public virtual ParentSetList<VhdlFile> Files
        {
            get { return files; }
        }

        public virtual IScope Scope
        {
            get { return scope; }
        }

        public override string ToString()
        {
            return identifier;
        }

        [Serializable]
        private class ResolvableImpl : IResolvable
        {
            private LibraryDeclarativeRegion parent;
            public ResolvableImpl(LibraryDeclarativeRegion parent)
            {
                this.parent = parent;
            }

            public virtual object Resolve(string identifier)
            {
                foreach (VhdlFile file in parent.files)
                {
                    foreach (LibraryUnit libraryUnit in file.Elements)
                    {
                        if (libraryUnit is PackageDeclaration)
                        {
                            foreach (IPackageDeclarativeItem o in (libraryUnit as PackageDeclaration).Declarations)
                            {
                                if (o is EnumerationType)
                                {
                                    EnumerationType type = o as EnumerationType;
                                    if (identifier.Equals(type.Identifier, StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        return type;
                                    }

                                    //TODO: support overloading
                                    foreach (EnumerationLiteral literal in type.Literals)
                                    {
                                        if (identifier.Equals(literal.ToString(), StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            return literal;
                                        }
                                    }
                                }
                                else if (o is PhysicalType)
                                {
                                    PhysicalType type = o as PhysicalType;
                                    if (identifier.Equals(type.Identifier, StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        return type;
                                    }

                                    //TODO: don't use strings for the physical literals
                                    if (identifier.Equals(type.PrimaryUnit, StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        return new PhysicalLiteral(type.PrimaryUnit);
                                    }

                                    foreach (PhysicalType.Unit unit in type.Units)
                                    {
                                        if (identifier.Equals(unit.Identifier, StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            return new PhysicalLiteral(unit.Identifier);
                                        }
                                    }
                                }
                            }
                        }
                        if (libraryUnit is INamedEntity)
                        {
                            INamedEntity ie = (INamedEntity)libraryUnit;
                            if (ie.Identifier.Equals(identifier, System.StringComparison.InvariantCultureIgnoreCase))
                            {
                                return ie;
                            }
                        }
                    }
                }

                return null;
            }


            public List<object> GetListOfObjects()
            {
                List<object> res = new List<object>();
                foreach (VhdlFile file in parent.files)
                {
                    foreach (LibraryUnit libraryUnit in file.Elements)
                    {
                        if (libraryUnit is PackageDeclaration)
                        {
                            foreach (IPackageDeclarativeItem o in (libraryUnit as PackageDeclaration).Declarations)
                            {
                                if (o is EnumerationType)
                                {
                                    EnumerationType type = o as EnumerationType;
                                    res.Add(type);

                                    //TODO: support overloading
                                    foreach (EnumerationLiteral literal in type.Literals)
                                    {
                                        res.Add(literal);
                                    }
                                }
                                else if (o is PhysicalType)
                                {
                                    PhysicalType type = o as PhysicalType;
                                    res.Add(type);
                                }
                            }
                        }
                        if (libraryUnit is INamedEntity)
                        {
                            INamedEntity ie = (INamedEntity)libraryUnit;
                            res.Add(ie);
                        }
                    }
                }
                return res;
            }

            public List<object> GetLocalListOfObjects()
            {
                return new List<object>();
            }
        }
    }

}