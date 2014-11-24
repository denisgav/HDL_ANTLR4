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
    using Scopes = VHDL.Scopes;
    using VhdlObject = VHDL.Object.VhdlObject;
    using VhdlObjectProvider = VHDL.Object.IVhdlObjectProvider;
    using SequentialStatement = VHDL.statement.SequentialStatement;
    using VhdlCollections = VHDL.util.VhdlCollections;

    /// <summary>
    /// Abstract base class for subprogram bodies.
    /// </summary>
    [Serializable]
    public abstract class SubprogramBody : DeclarativeItem, IBlockDeclarativeItem, IEntityDeclarativeItem, IPackageBodyDeclarativeItem, IProcessDeclarativeItem, ISubprogramDeclarativeItem, IDeclarativeRegion, ISubprogram
    {
        private readonly IResolvableList<VhdlObjectProvider> parameters;
        private string identifier;
        private readonly IResolvableList<ISubprogramDeclarativeItem> declarations;
        private readonly List<SequentialStatement> statements;
        private readonly IScope scope;

        /// <summary>
        /// Creates a subprogram body.
        /// </summary>
        /// <param name="identifier">the identifier of this subprogram body</param>
        /// <param name="parameters">the parameters</param>
        public SubprogramBody(string identifier, params VhdlObjectProvider[] parameters)
            : this(identifier, new List<VhdlObjectProvider>(parameters))
        {
        }

        /// <summary>
        /// Creates a subprogram body.
        /// </summary>
        /// <param name="identifier">the identifier of this subprogram body</param>
        /// <param name="parameters">the parameters</param>
        public SubprogramBody(string identifier, List<VhdlObjectProvider> parameters)
        {
            this.parameters = VhdlCollections.CreateVhdlObjectList<VhdlObjectProvider>();
            this.declarations = VhdlCollections.CreateDeclarationList<ISubprogramDeclarativeItem>();
            this.statements = ParentSetList<SequentialStatement>.Create(this);
            this.scope = Scopes.createScope(this, this.parameters, this.declarations);

            this.identifier = identifier;
            foreach (VhdlObjectProvider provider in parameters)
            {
                this.parameters.Add(provider);
            }
        }

        //TODO: link subprogram body to declaration
        /// <summary>
        /// Creates a subprogram body based on a subprogram declaration.
        /// </summary>
        /// <param name="declaration">the subprogam declaration</param>
        public SubprogramBody(SubprogramDeclaration declaration)
        {
            this.parameters = VhdlCollections.CreateVhdlObjectList<VhdlObjectProvider>();
            this.declarations = VhdlCollections.CreateDeclarationList<ISubprogramDeclarativeItem>();
            this.statements = ParentSetList<SequentialStatement>.Create(this);
            this.scope = Scopes.createScope(this, this.parameters, this.declarations);

            this.identifier = declaration.Identifier;
            foreach (var o in declaration.Parameters)
                this.parameters.Add(o);
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

        /// <summary>
        /// Returns the declarations.
        /// </summary>
        public virtual IResolvableList<ISubprogramDeclarativeItem> Declarations
        {
            get { return declarations; }
        }

        /// <summary>
        /// Returns the statements.
        /// </summary>
        public virtual List<SequentialStatement> Statements
        {
            get { return statements; }
        }

        public IScope Scope
        {
            get { return scope; }
        }
    }
}