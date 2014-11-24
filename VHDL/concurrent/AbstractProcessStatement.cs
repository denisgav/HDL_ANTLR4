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

using VHDL.util;
using System;
using System.Collections.Generic;

namespace VHDL.concurrent
{
    using DeclarativeRegion = VHDL.IDeclarativeRegion;
    using Resolvable = VHDL.IResolvable;
    using Scope = VHDL.IScope;
    using Scopes = VHDL.Scopes;
    using Signal = VHDL.Object.Signal;
    using ProcessDeclarativeItem = VHDL.declaration.IProcessDeclarativeItem;
    using SequentialStatement = VHDL.statement.SequentialStatement;
    using VhdlCollections = VHDL.util.VhdlCollections;
    
    /// <summary>
    /// Abstract base class for process statements.
    /// </summary>
    [Serializable]
    public abstract class AbstractProcessStatement : EntityStatement, IDeclarativeRegion
    {
        private readonly ResolvableImpl resolvable;
        private readonly IScope scope;

        /// <summary>
        /// Creates an abstract process statement without a label.
        /// </summary>
        public AbstractProcessStatement()
        {
            resolvable = new ResolvableImpl(this);
            scope = Scopes.createScope(this, resolvable);
        }

        /// <summary>
        /// Creates an abstract process statement with the given label.
        /// </summary>
        /// <param name="label">the label</param>
        public AbstractProcessStatement(string label)
        {
            resolvable = new ResolvableImpl(this);
            scope = Scopes.createScope(this, resolvable);
            Label = label;
        }

        /// <summary>
        /// Returns the sensitivity list.
        /// </summary>
        public abstract List<Signal> SensitivityList { get; }

        /// <summary>
        /// Returns the declarations.
        /// </summary>
        public abstract List<ProcessDeclarativeItem> Declarations { get; }

        /// <summary>
        /// Returns the statements.
        /// </summary>
        public abstract ParentSetList<SequentialStatement> Statements { get; }

        public virtual IScope Scope
        {
            get { return scope; }
        }

        internal override void accept(ConcurrentStatementVisitor visitor)
        {
            visitor.visitProcessStatement(this);
        }

        [Serializable]
        private class ResolvableImpl : IResolvable
        {
            private AbstractProcessStatement parent;
            public ResolvableImpl(AbstractProcessStatement parent)
            {
                this.parent = parent;
            }

            public virtual object Resolve(string identifier)
            {
                IResolvableList<SequentialStatement> stmts = VhdlCollections.CreateLabeledElementList<SequentialStatement>(parent, parent.Statements);
                object result = stmts.Resolve(identifier);
                if (result != null)
                {
                    return result;
                }

                IResolvableList<ProcessDeclarativeItem> decls = VhdlCollections.CreateDeclarationList<ProcessDeclarativeItem>(parent.Declarations);
                return decls.Resolve(identifier);
            }


            public List<object> GetListOfObjects()
            {
                List<object> res = new List<object>();
                IResolvableList<ProcessDeclarativeItem> decls = VhdlCollections.CreateDeclarationList<ProcessDeclarativeItem>(parent.Declarations);
                res.AddRange(decls.GetListOfObjects());
                IResolvableList<SequentialStatement> stmts = VhdlCollections.CreateLabeledElementList<SequentialStatement>(parent, parent.Statements);
                res.AddRange(stmts.GetListOfObjects());
                return res;
            }

            public List<object> GetLocalListOfObjects()
            {
                return GetListOfObjects();
            }
        }
    }

}