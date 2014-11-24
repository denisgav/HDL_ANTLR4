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
using Libraries = VHDL.builtin.Libraries;
using Standard = VHDL.builtin.Standard;
using LibraryUnit = VHDL.libraryunit.LibraryUnit;
using UseClause = VHDL.libraryunit.UseClause;
using VhdlCollections = VHDL.util.VhdlCollections;
using VHDL.util;
using System;

namespace VHDL
{
    /// <summary>
    /// Root declarative region.
    /// The root declarative region is the base of the scope tree and can contain multiple libraries.
    /// </summary>
    [Serializable]
    public class RootDeclarativeRegion : VhdlElement, IDeclarativeRegion
    {
        private readonly IResolvableList<LibraryDeclarativeRegion> libraries;
        private readonly IScope scope;
        private readonly IScope internalScope;

        /// <summary>
        /// Creates an root declarative region containing the Standard library.
        /// </summary>
        public RootDeclarativeRegion()
        {
            libraries = VhdlCollections.CreateNamedEntityList<LibraryDeclarativeRegion>(this);
            //libraries.Add(Libraries.STD);
            //libraries.Add(Libraries.IEEE);
            scope = Scopes.createScope(this, libraries, new UseClauseResolvable(this));
            internalScope = Scopes.createScope(this, libraries);
        }


        /// <summary>
        /// Returns the libraries in this root declarative region.
        /// </summary>
        public virtual IResolvableList<LibraryDeclarativeRegion> Libraries
        {
            get { return libraries; }
        }

        public void AddLibrary(LibraryDeclarativeRegion library)
        {
            if (libraries.Contains(library) == false)
                libraries.Add(library);
        }

        public virtual IScope Scope
        {
            get { return scope; }
        }

        //TODO: move to VhdlFile. Problem:
        //File 1:
        //library ieee;
        //use ieee.std_logic_1164.all;
        //entity ent is
        //end;
        //----
        //File 2:
        //architecture beh of ent is
        //signal test : std_logic;
        //begin
        //end;
        [Serializable]
        private class UseClauseResolvable : IResolvable
        {
            private RootDeclarativeRegion parent;
            public UseClauseResolvable(RootDeclarativeRegion parent)
            {
                this.parent = parent;
            }

            private object descentHierarchy(string[] parts, string identifier)
            {
                IScope scope = parent.internalScope;

                for (int i = 0; i < parts.Length - 1; i++)
                {
                    IDeclarativeRegion region = scope.resolveLocal<IDeclarativeRegion>(parts[i]);
                    if (region == null)
                    {
                        return null;
                    }
                    else
                    {
                        scope = region.Scope;
                    }
                }
                return scope.resolveLocal(identifier);
            }

            private List<object> analyseHierarchy(string[] parts)
            {
                IScope scope = parent.internalScope;

                for (int i = 0; i < parts.Length - 1; i++)
                {
                    IDeclarativeRegion region = scope.resolveLocal<IDeclarativeRegion>(parts[i]);
                    if (region == null)
                    {
                        return null;
                    }
                    else
                    {
                        scope = region.Scope;
                    }
                }
                return scope.GetListOfObjects();
            }

            public virtual object Resolve(string identifier)
            {

                //implicit use std.standard.all:
                object standart = Standard.PACKAGE.Scope.resolveLocal(identifier);
                return standart;
            }


            public List<object> GetListOfObjects()
            {
                List<object> res = new List<object>();

                //implicit use std.standard.all:
                res.AddRange(Standard.PACKAGE.Scope.GetListOfObjects());

                return res;
            }

            public List<object> GetLocalListOfObjects()
            {
                return new List<object>();
            }
        }
    }

}