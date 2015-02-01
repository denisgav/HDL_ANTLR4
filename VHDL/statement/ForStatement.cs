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

namespace VHDL.statement
{
    using DiscreteRange = VHDL.DiscreteRange;
    using Resolvable = VHDL.IResolvable;
    using Scope = VHDL.IScope;
    using Scopes = VHDL.Scopes;
    using Standard = VHDL.builtin.Standard;
    using Constant = VHDL.Object.Constant;

    /// <summary>
    /// For loop.
    /// 
    /// ForStatement loop = new ForStatement("I",
    ///  new Range(0, Range.Direction.TO, 127));
    /// loop.getStatements().add(new NullStatement());
    /// ---
    /// for I in 0 to 127 loop
    ///  null;
    /// end loop;
    /// </summary>
    [Serializable]
    public class ForStatement : LoopStatement
    {
        //TODO: make type of loop parameter unmutable
        private readonly Constant loopParameter;
        private DiscreteRange range;
        private readonly IScope scope;

        /// <summary>
        /// Creates a for loop.
        /// </summary>
        /// <param name="loopParameter">the identifier of the loop parameter</param>
        /// <param name="range">the loop range</param>
        public ForStatement(string loopParameter, DiscreteRange range)
        {
            scope = Scopes.createScope(this, new ResolvableImpl(this));
            this.loopParameter = new Constant(loopParameter, Standard.INTEGER);
            this.range = range;
        }

        /// <summary>
        /// Returns loop parameter.
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
            set { range = value; }
        }        

        internal override void accept(SequentialStatementVisitor visitor)
        {
            visitor.visitForStatement(this);
        }

        public override IScope Scope
        {
            get { return scope; }
        }

        [Serializable]
        private class ResolvableImpl : IResolvable
        {
            private ForStatement parent;

            public ResolvableImpl(ForStatement parent)
            {
                this.parent = parent;
            }

            public virtual object Resolve(string identifier)
            {
                if (identifier.EqualsIdentifier(parent.Label))
                {
                    return parent;
                }

                if (identifier.EqualsIdentifier(parent.Parameter.Identifier))
                {
                    return parent.Parameter;
                }

                return null;
            }

            public List<object> GetListOfObjects()
            {
                return new List<object>() { parent, parent.Parameter };
            }

            public List<object> GetLocalListOfObjects()
            {
                return GetListOfObjects();
            }
        }
    }

}