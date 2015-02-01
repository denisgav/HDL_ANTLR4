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
    using AssociationElement = VHDL.AssociationElement;
    using DeclarativeRegion = VHDL.IDeclarativeRegion;
    using Resolvable = VHDL.IResolvable;
    using Scope = VHDL.IScope;
    using Scopes = VHDL.Scopes;
    using Standard = VHDL.builtin.Standard;
    using Constant = VHDL.Object.Constant;
    using Signal = VHDL.Object.Signal;
    using BlockDeclarativeItem = VHDL.declaration.IBlockDeclarativeItem;
    using Expression = VHDL.expression.Expression;
    using VhdlObjectProvider = VHDL.Object.IVhdlObjectProvider;
    using VhdlCollections = VHDL.util.VhdlCollections;

    /// <summary>
    /// Block statement.
    /// </summary>
    [Serializable]
    public class BlockStatement : ConcurrentStatement, IDeclarativeRegion
    {
        private Expression guardExpression;
        private readonly IResolvableList<VhdlObjectProvider> port;
        private readonly IResolvableList<VhdlObjectProvider> generic;
        private readonly IResolvableList<BlockDeclarativeItem> declarations;
        private readonly IList<ConcurrentStatement> statements;
        private List<AssociationElement> portMap;
        private List<AssociationElement> genericMap;
        private readonly IScope scope;

        /// <summary>
        /// Creates a block statement.
        /// </summary>
        /// <param name="label"></param>
        public BlockStatement(string label)
        {
            port = VhdlCollections.CreateVhdlObjectList<VhdlObjectProvider>();
            generic = VhdlCollections.CreateVhdlObjectList<VhdlObjectProvider>();
            declarations = VhdlCollections.CreateDeclarationList<BlockDeclarativeItem>();
            statements = VhdlCollections.CreateLabeledElementList<ConcurrentStatement>(this);
            portMap = new List<AssociationElement>();
            genericMap = new List<AssociationElement>();
            scope = Scopes.createScope(this, port, generic, declarations, new GuardSignalResolvable(this));
            Label = label;
        }

        /// <summary>
        /// Creates a block statement with a guard epxression.
        /// </summary>
        /// <param name="label">the label</param>
        /// <param name="guardExpression">the guard expression</param>
        public BlockStatement(string label, Expression guardExpression)
        {
            Label = label;
            this.guardExpression = guardExpression;
        }

        /// <summary>
        /// Returns/Sets the guard expression.
        /// </summary>
        public virtual Expression GuardExpression
        {
            get { return guardExpression; }
            set { guardExpression = value; }
        }

        /// <summary>
        /// Returns the generic clause.
        /// </summary>
        public virtual IList<VhdlObjectProvider> Generic
        {
            get { return generic; }
        }

        /// <summary>
        /// Returns the port clause.
        /// </summary>
        public virtual IList<VhdlObjectProvider> Port
        {
            get { return port; }
        }

        /// <summary>
        /// Returns the generic map.
        /// </summary>
        public virtual List<AssociationElement> GenericMap
        {
            get { return genericMap; }
        }

        /// <summary>
        /// Returns the port map.
        /// </summary>
        public virtual List<AssociationElement> PortMap
        {
            get { return portMap; }
        }

        /// <summary>
        /// Returns the declarations.
        /// </summary>
        public virtual IList<BlockDeclarativeItem> Declarations
        {
            get { return declarations; }
        }

        /// <summary>
        /// Returns the statements.
        /// </summary>
        public virtual IList<ConcurrentStatement> Statements
        {
            get { return statements; }
        }

        public virtual IScope Scope
        {
            get { return scope; }
        }

        internal override void accept(ConcurrentStatementVisitor visitor)
        {
            visitor.visitBlockStatement(this);
        }

        [Serializable]
        private class GuardSignalResolvable : IResolvable
        {
            private BlockStatement parent;
            public GuardSignalResolvable(BlockStatement parent)
            {
                this.parent = parent;
            }

            public virtual object Resolve(string identifier)
            {
                if (parent.guardExpression != null && identifier.EqualsIdentifier("GUARD"))
                {
                    return new Signal("guard", Standard.BOOLEAN);
                }
                else
                {
                    return null;
                }
            }


            public List<object> GetListOfObjects()
            {
                return new List<object>() { parent };
            }

            public List<object> GetLocalListOfObjects()
            {
                return GetListOfObjects();
            }
        }
    }

}