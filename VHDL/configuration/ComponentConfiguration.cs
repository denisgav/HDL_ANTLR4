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


namespace VHDL.configuration
{
    using ComponentSpecification = VHDL.ComponentSpecification;
    using AssociationElement = VHDL.AssociationElement;
    using EntityAspect = VHDL.EntityAspect;

    /// <summary>
    /// Component configuration.
    /// </summary>
    [Serializable]
	public class ComponentConfiguration : ConfigurationItem
	{
		private ComponentSpecification componentSpecification;
		private AbstractBlockConfiguration blockConfiguration;
		private EntityAspect entityAspect;
		private readonly List<AssociationElement> portMap = new List<AssociationElement>();
		private readonly List<AssociationElement> genericMap = new List<AssociationElement>();

        /// <summary>
        /// Creates a component configuration.
        /// </summary>
        /// <param name="componentSpecification">specifies the configured components</param>
		public ComponentConfiguration(ComponentSpecification componentSpecification)
		{
			this.componentSpecification = componentSpecification;
		}

        /// <summary>
        /// Returns/Sets the block configuration.
        /// </summary>
		public virtual AbstractBlockConfiguration BlockConfiguration
		{
            get { return blockConfiguration; }
            set { blockConfiguration = value; }
		}

//    *
//     * Returns the component specification.
//     * @return the component specification
//     
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

		internal override void accept(ConfigurationVisitor visitor)
		{
			visitor.visitComponentConfiguration(this);
		}
	}

}