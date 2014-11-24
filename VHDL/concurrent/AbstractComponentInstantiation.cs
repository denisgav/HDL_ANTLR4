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

using AssociationElement = VHDL.AssociationElement;
using System;
using System.Collections.Generic;

namespace VHDL.concurrent
{
    /// <summary>
    /// Abstract base class for component, entity and configuration instantiations.
    /// </summary>
    [Serializable]
    public abstract class AbstractComponentInstantiation : ConcurrentStatement
    {
        private List<AssociationElement> portMap;
        private List<AssociationElement> genericMap;

        /// <summary>
        /// Creates an abstract component instantiation with the given label.
        /// </summary>
        /// <param name="label">the label of the instantiated componet</param>
        public AbstractComponentInstantiation(string label)
        {
            portMap = new List<AssociationElement>();
            genericMap = new List<AssociationElement>();
            Label = label;
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
    }
}