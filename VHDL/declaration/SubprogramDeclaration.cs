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
    using VhdlObject = VHDL.Object.VhdlObject;
    using VhdlObjectProvider = VHDL.Object.IVhdlObjectProvider;

    /// <summary>
    /// Abstract base class for subprogram declarations.
    /// </summary>
    [Serializable]
    public abstract class SubprogramDeclaration : DeclarativeItem, IBlockDeclarativeItem, IEntityDeclarativeItem, IPackageBodyDeclarativeItem, IPackageDeclarativeItem, IProcessDeclarativeItem, ISubprogramDeclarativeItem, ISubprogram
    {
        private readonly IResolvableList<VhdlObjectProvider> parameters;
        private string identifier;

        /// <summary>
        /// Creates a subprogram declaration.
        /// </summary>
        /// <param name="identifier">the identifier of this subprogram declaration</param>
        /// <param name="parameters">the parameters</param>
        public SubprogramDeclaration(string identifier, params VhdlObjectProvider[] parameters)
            : this(identifier, new List<VhdlObjectProvider>(parameters))
        {
            //this.parameters =  VhdlCollections.createVhdlObjectList<VhdlObjectProvider>();
        }

        /// <summary>
        /// Creates a subprogram declaration.
        /// </summary>
        /// <param name="identifier">the identifier of this subprogram declaration</param>
        /// <param name="parameters">the parameters</param>     
        public SubprogramDeclaration(string identifier, List<VhdlObjectProvider> parameters)
        {
            this.parameters = VhdlCollections.CreateVhdlObjectList<VhdlObjectProvider>();
            this.identifier = identifier;
            foreach (VhdlObjectProvider provider in parameters)
            {
                VhdlObjectProvider p = (VhdlObjectProvider)provider;
                this.parameters.Add(p);
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

        public IResolvableList<VhdlObjectProvider> Parameters
        {
            get { return parameters; }
        }

        public override string ToString()
        {
            return identifier;
        }
    }
}