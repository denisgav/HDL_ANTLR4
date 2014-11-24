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

    using Entity = VHDL.libraryunit.Entity;


    ///
    // * Entity instantiation.
    // 
    [Serializable]
    public class EntityInstantiation : AbstractComponentInstantiation
    {
        private Entity entity;

        /// <summary>
        /// Creates a entity instantiation.
        /// </summary>
        /// <param name="label">the label</param>
        /// <param name="entity">the instantiated entity</param>
        public EntityInstantiation(string label, Entity entity)
            : base(label)
        {
            this.entity = entity;
        }

        /// <summary>
        /// Returns/Sets the instantiated entity.
        /// </summary>
        public virtual Entity Entity
        {
            get { return entity; }
            set { entity = value; }
        }

        internal override void accept(ConcurrentStatementVisitor visitor)
        {
            visitor.visitEntityInstantiation(this);
        }
    }
}