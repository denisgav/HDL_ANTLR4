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

namespace VHDL.declaration
{
    using AssociationElement = VHDL.AssociationElement;
    using ComponentSpecification = VHDL.ComponentSpecification;
    using EntityAspect = VHDL.EntityAspect;

    //TODO: replace dummy implementation
    /// <summary>
    /// Configuration specification.
    /// </summary>
    [Serializable]
    public class ConfigurationSpecification : DeclarativeItem, IBlockDeclarativeItem
    {
        private ComponentSpecification componentSpecification;
        private EntityAspect entityAspect;
        private readonly List<AssociationElement> portMap = new List<AssociationElement>();
        private readonly List<AssociationElement> genericMap = new List<AssociationElement>();

        /// <summary>
        /// Creates a configuration specification.
        /// </summary>
        /// <param name="componentSpecification"></param>
        public ConfigurationSpecification(ComponentSpecification componentSpecification)
        {
            this.componentSpecification = componentSpecification;
        }

        /// <summary>
        /// Returns/Sets the component specification.
        /// </summary>
        public virtual ComponentSpecification ComponentSpecification
        {
            get { return componentSpecification; }
            set { componentSpecification = value; }
        }

        /// <summary>
        /// Returns/Sets the entity aspect.
        /// </summary>
        public virtual EntityAspect EntityAspect
        {
            get { return entityAspect; }
            set { entityAspect = value; }
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

        internal override void accept(DeclarationVisitor visitor)
        {
            visitor.visitConfigurationSpecification(this);
        }
    }

}