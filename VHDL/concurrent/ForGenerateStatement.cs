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
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace VHDL.concurrent
{
    using DiscreteRange = VHDL.DiscreteRange;
    using Resolvable = VHDL.IResolvable;
    using Scope = VHDL.IScope;
    using Scopes = VHDL.Scopes;
    using Standard = VHDL.builtin.Standard;
    using Constant = VHDL.Object.Constant;

    /// <summary>
    /// For generate statement.
    /// </summary>
    [Serializable]
    public class ForGenerateStatement : AbstractGenerateStatement
    {
        //TODO: make type of loop parameter unmutable
        private readonly Constant loopParameter;
        private DiscreteRange range;
        private readonly Resolvable resolvable;
        private readonly Scope scope;

        /// <summary>
        /// Creates a for generate statement.
        /// </summary>
        /// <param name="label">the label</param>
        /// <param name="loopParameter">the identifier of the for loop parameter</param>
        /// <param name="range">the loop range</param>
        public ForGenerateStatement(string label, string loopParameter, DiscreteRange range)
        {
            Label = label;
            resolvable = new ResolvableImpl(this);
            scope = Scopes.createScope(this, resolvable);
            this.loopParameter = new Constant(loopParameter, Standard.INTEGER);
            this.range = range;
        }

        /// <summary>
        /// Returns the loop parameter.
        /// </summary>
        public virtual Constant Parameter
        {
            get { return loopParameter; }
        }

        /// <summary>
        /// Returns/Sets the loop range.
        /// </summary>
        public virtual DiscreteRange Range
        {
            get { return range; }
            set { Range = value; }
        }


        internal override void accept(ConcurrentStatementVisitor visitor)
        {
            visitor.visitForGenerateStatement(this);
        }

        public override Scope Scope
        {
            get { return scope; }
        }

        [Serializable]
        private class ResolvableImpl : Resolvable
        {
            private ForGenerateStatement parent;
            public ResolvableImpl(ForGenerateStatement parent)
            {
                this.parent = parent;
            }
            public virtual object Resolve(string identifier)
            {
                if (identifier.VHDLIdentifierEquals(parent.loopParameter.Identifier))
                {
                    return parent.loopParameter;
                }

                return null;
            }

            public List<object> GetListOfObjects()
            {
                return new List<object>() { parent };
            }

            public List<object> GetLocalListOfObjects()
            {
                return new List<object>() { parent };
            }
        }
    }
}