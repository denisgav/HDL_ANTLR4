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
using Component = VHDL.declaration.Component;
using System;

namespace VHDL
{
    /// <summary>
    /// Component specification.
    /// A component specification is used in component configurations and configuration specifications
    /// to specify a list of component instances.
    /// </summary>
    [Serializable]
    public abstract class ComponentSpecification : VhdlElement
    {
        private readonly Component component;

        private ComponentSpecification(Component component)
        {
            this.component = component;
        }

        /// <summary>
        /// Creates a component specification.
        /// </summary>
        /// <param name="component">the component</param>
        /// <param name="labels">a list of component labels</param>
        /// <returns>created component sepecification</returns>
        public static ComponentSpecification create(Component component, List<string> labels)
        {
            return new ListComponentSpecification(component, labels);
        }

        /// <summary>
        /// Creates a component specification.
        /// </summary>
        /// <param name="component">the component</param>
        /// <param name="labels">a list of component labels</param>
        /// <returns>the created component specification</returns>
        public static ComponentSpecification create(Component component, params string[] labels)
        {
            return new ListComponentSpecification(component, new List<string>(labels));
        }

        /// <summary>
        /// Creates a component sepecification for all instances.
        /// </summary>
        /// <param name="component">the component</param>
        /// <returns>the created component specification</returns>
        public static ComponentSpecification createAll(Component component)
        {
            return new AllComponentSpecification(component);
        }

        /// <summary>
        /// Creates a component specification for the other instances.
        /// </summary>
        /// <param name="component">the component</param>
        /// <returns>the created component specification</returns>
        public static ComponentSpecification createOthers(Component component)
        {
            return new OthersComponentSpecification(component);
        }

        /// <summary>
        /// Returns the type of this component specification.
        /// </summary>
        public abstract ComponentType Type { get; }

        /// <summary>
        /// Returns the list of component labels.
        /// </summary>
        public abstract List<string> Labels { get; }

        /// <summary>
        /// Returns the component.
        /// </summary>
        public virtual Component Component
        {
            get { return component; }
        }

        private class ListComponentSpecification : ComponentSpecification
        {

            private readonly List<string> labels;

            public ListComponentSpecification(Component component, List<string> labels)
                : base(component)
            {
                this.labels = new List<string>(labels);
            }

            public override List<string> Labels
            {
                get { return labels; }
            }

            public override ComponentType Type
            {
                get { return ComponentType.INSTANTIATION_LIST; }
            }
        }

        private class AllComponentSpecification : ComponentSpecification
        {

            public AllComponentSpecification(Component component)
                : base(component)
            {
            }

            public override ComponentType Type
            {
                get { return ComponentType.ALL; }
            }

            public override List<string> Labels
            {
                get { return null; }
            }
        }

        private class OthersComponentSpecification : ComponentSpecification
        {

            public OthersComponentSpecification(Component component)
                : base(component)
            {
            }

            public override ComponentType Type
            {
                get { return ComponentType.OTHERS; }
            }

            public override List<string> Labels
            {
                get { return null; }
            }
        }

        //    *
        //     * Component specification type.
        //     
        public enum ComponentType
        {

            //        *
            //         * Component specification with list of labels.
            //         
            INSTANTIATION_LIST,
            //        *
            //         * Component specification that uses the OTHERS keyword.
            //         
            OTHERS,
            //        *
            //         * Component specification that uses the ALL keyword.
            //         
            ALL
        }
    }

}