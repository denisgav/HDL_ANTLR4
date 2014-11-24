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

namespace VHDL.concurrent
{
    using Component = VHDL.declaration.Component;
    
    /// <summary>
    /// Component instantitation.
    /// </summary>
    [Serializable]
    public class ComponentInstantiation : AbstractComponentInstantiation
    {
        private Component component;

        /// <summary>
        /// Creates a component instantiation.
        /// </summary>
        /// <param name="label">the label</param>
        /// <param name="component">the instantiated component</param>
        public ComponentInstantiation(string label, Component component)
            : base(label)
        {
            this.component = component;
            Parent = component.Parent;
        }

        /// <summary>
        /// Returns/Sets the instantiated component.
        /// </summary>
        public virtual Component Component
        {
            get { return component; }
            set { component = value; }
        }

        internal override void accept(ConcurrentStatementVisitor visitor)
        {
            visitor.visitComponentInstantiation(this);
        }
    }
}