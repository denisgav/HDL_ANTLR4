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

namespace VHDL
{

    using Architecture = VHDL.libraryunit.Architecture;
    using Configuration = VHDL.libraryunit.Configuration;
    using Entity = VHDL.libraryunit.Entity;
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// An entity aspect using the OPEN keyword.
    /// </summary>
    [Serializable]
    public class OPENEntityAspect : EntityAspect
    {
        public override string ToString()
        {
            return "open";
        }
    }

    /// <summary>
    /// Entity aspect.
    /// </summary>
    [Serializable]
    public abstract class EntityAspect : VhdlElement
    {
        public static OPENEntityAspect OPEN = new OPENEntityAspect();

        /// <summary>
        /// Creates an entity aspect based on an entity.
        /// </summary>
        /// <param name="entity">the entity</param>
        /// <returns>the created entity aspect</returns>
        public static EntityAspect entity(Entity entity)
        {
            return new EntityEntityAspect(entity);
        }

        /// <summary>
        /// Creates an entity aspect based on an architecture.
        /// </summary>
        /// <param name="architecute">the architecture</param>
        /// <returns>the created entity aspect</returns>
        public static EntityAspect architecture(Architecture architecute)
        {
            return new ArchitectureEntityAspect(architecute);
        }

        /// <summary>
        /// Creates an entity aspect based on a configuration.
        /// </summary>
        /// <param name="configuration">the configuration</param>
        /// <returns>the created entity aspect</returns>
        public static EntityAspect configuration(Configuration configuration)
        {
            return new ConfigurationEntityAspect(configuration);
        }

        [Serializable]
        public class EntityEntityAspect : EntityAspect
        {
            private Entity entity;

            public EntityEntityAspect(Entity entity)
            {
                this.entity = entity;
            }

            public override string ToString()
            {
                if (entity != null)
                {
                    return "entity " + entity.Identifier;
                }
                else
                {
                    return "entity null";
                }
            }
        }

        [Serializable]
        private class ArchitectureEntityAspect : EntityAspect
        {
            private Architecture architecture;

            public ArchitectureEntityAspect(Architecture architecture)
            {
                this.architecture = architecture;
            }

            //TODO: use output keyword case setting
            public override string ToString()
            {
                if (architecture == null)
                {
                    return "entity null(null)";
                }
                else
                {
                    if (architecture.Entity != null)
                    {
                        return "entity " + architecture.Entity.Identifier + '(' + architecture.Identifier + ')';
                    }
                    else
                    {
                        return "entity null(" + architecture.Identifier + ")";
                    }
                }
            }
        }

        [Serializable]
        private class ConfigurationEntityAspect : EntityAspect
        {
            private Configuration configuration;

            public ConfigurationEntityAspect(Configuration configuration)
            {
                this.configuration = configuration;
            }

            public override string ToString()
            {
                if (configuration != null)
                {
                    return "configuration " + configuration.Identifier;
                }
                else
                {
                    return "configuration null";
                }
            }
        }
    }
}