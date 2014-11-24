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

using System;
using System.Runtime.Serialization;

namespace VHDL.concurrent
{
    using Scope = VHDL.IScope;
    using Scopes = VHDL.Scopes;
    using Expression = VHDL.expression.Expression;

    /// <summary>
    /// If generate statement.
    /// </summary>
    [Serializable]
    public class IfGenerateStatement : AbstractGenerateStatement
    {
        private Expression condition;
        private readonly Scope scope;

        /// <summary>
        /// Creates an if generate statement.
        /// </summary>
        /// <param name="label">the label</param>
        /// <param name="condition">the if condition</param>
        public IfGenerateStatement(string label, Expression condition)
        {
            scope = Scopes.createScope(this);
            Label = label;
            this.condition = condition;
        }

        /// <summary>
        /// Returns/Sets the if condition.
        /// </summary>
        public virtual Expression Condition
        {
            get { return condition; }
            set { condition = value; }
        }

        internal override void accept(ConcurrentStatementVisitor visitor)
        {
            visitor.visitIfGenerateStatement(this);
        }

        public override Scope Scope
        {
            get { return scope; }
        }
    }

}