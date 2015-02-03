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

namespace VHDL.statement
{
    using DeclarativeRegion = VHDL.IDeclarativeRegion;
    using Resolvable = VHDL.IResolvable;
    using Scope = VHDL.IScope;
    using Scopes = VHDL.Scopes;

    /// <summary>
    /// * Loop.
    /// *
    /// * @VHDL.example
    /// * LoopStatement loop = new LoopStatement();
    /// * loop.getStatements().add(new NullStatement());
    /// * ---
    /// * loop
    /// *  null;
    /// * end loop;
    /// </summary>
    [Serializable]
    public class LoopStatement : SequentialStatement, IDeclarativeRegion
    {
        private readonly IResolvable resolvable;
        private readonly List<SequentialStatement> statements;
        private readonly IScope scope;

        /// <summary>
        /// Creates a loop statement.
        /// </summary>
        public LoopStatement()
        {
            resolvable = new ResolvableImpl(this);
            statements = ParentSetList<SequentialStatement>.Create(this);
            scope = Scopes.createScope(this, resolvable);
        }

        /// <summary>
        /// Returns the statements.
        /// </summary>
        public virtual List<SequentialStatement> Statements
        {
            get { return statements; }
        }

        internal override void accept(SequentialStatementVisitor visitor)
        {
            visitor.visitLoopStatement(this);
        }

        public virtual IScope Scope
        {
            get { return scope; }
        }

        [Serializable]
        private class ResolvableImpl : IResolvable
        {
            private LoopStatement parent;

            public ResolvableImpl(LoopStatement parent)
            {
                this.parent = parent;
            }

            public object Resolve(string identifier)
            {
                if (identifier.EqualsIgnoreCase(parent.Label))
                {
                    return parent;
                }

                //TODO: move to ForStatement
                if (parent is ForStatement)
                {
                    ForStatement forStatement = parent as ForStatement;
                    if (identifier.EqualsIgnoreCase(forStatement.Parameter.Identifier))
                    {
                        return forStatement.Parameter;
                    }
                }

                return null;
            }


            public List<object> GetListOfObjects()
            {
                List<object> res = new List<object>() { parent };

                if (parent is ForStatement)
                {
                    ForStatement forStatement = parent as ForStatement;
                    res.Add(forStatement.Parameter);
                }

                return res;
            }

            public List<object> GetLocalListOfObjects()
            {
                return GetListOfObjects();
            }
        }

        public override List<VhdlElement> GetAllStatements()
        {
            List<VhdlElement> res = new List<VhdlElement>();
            res.AddRange(statements);
            return res;
        }
    }
}