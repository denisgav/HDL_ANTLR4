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
using System;
using VHDL.util;
using System.Runtime.Serialization;

namespace VHDL.concurrent
{
    using DeclarativeRegion = VHDL.IDeclarativeRegion;
    using BlockDeclarativeItem = VHDL.declaration.IBlockDeclarativeItem;
    using VhdlCollections = VHDL.util.VhdlCollections;

    /// <summary>
    /// Abstract base class for generate statements.
    /// </summary>
    [Serializable]
    public abstract class AbstractGenerateStatement : ConcurrentStatement, DeclarativeRegion
    {
        private readonly IResolvableList<BlockDeclarativeItem> declarations;
        private readonly IResolvableList<ConcurrentStatement> statements;
        private readonly IScope scope;

        public AbstractGenerateStatement()
        {
            declarations = VhdlCollections.CreateDeclarationList<BlockDeclarativeItem>();
            statements = VhdlCollections.CreateLabeledElementList<ConcurrentStatement>(this);
            scope = Scopes.createScope(this, declarations, statements);
        }

        /// <summary>
        /// Returns the declarations.
        /// </summary>
        public virtual IResolvableList<BlockDeclarativeItem> Declarations
        {
            get { return declarations; }
        }

        /// <summary>
        /// Returns the statements.
        /// </summary>
        public virtual IResolvableList<ConcurrentStatement> Statements
        {
            get { return statements; }
        }

        #region DeclarativeRegion Members

        public virtual IScope Scope
        {
            get { return scope; }
        }

        #endregion
    }
}