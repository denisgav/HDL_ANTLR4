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

namespace VHDL.libraryunit
{
    using NamedEntity = VHDL.INamedEntity;
    using Scope = VHDL.IScope;
    using AbstractBlockConfiguration = VHDL.configuration.AbstractBlockConfiguration;
    using ConfigurationDeclarativeItem = VHDL.declaration.IConfigurationDeclarativeItem;

    /// <summary>
    /// Configuration.
    /// </summary>
    [Serializable]
    public class Configuration : PrimaryUnit, INamedEntity
    {
        private string identifier;
        private Entity entity;
        private readonly List<ConfigurationDeclarativeItem> declarations = new List<ConfigurationDeclarativeItem>();
        private AbstractBlockConfiguration blockConfiguration;

        /// <summary>
        /// Creates a configuration.
        /// </summary>
        /// <param name="identifier">the identifier of this configuration</param>
        /// <param name="entity">the entity</param>
        /// <param name="blockConfiguration">the block configuration</param>
        public Configuration(string identifier, Entity entity, AbstractBlockConfiguration blockConfiguration)
        {
            this.identifier = identifier;
            this.entity = entity;
            this.blockConfiguration = blockConfiguration;
        }

        /// <summary>
        /// Returns/Sets the configured entity.
        /// </summary>
        public virtual Entity Entity
        {
            get { return entity; }
            set { entity = value; }
        }

        /// <summary>
        /// Returns/Sets the identifier of this attribtue.
        /// </summary>
        public virtual string Identifier
        {
            get { return identifier; }
            set { identifier = value; }
        }

        /// <summary>
        /// Returns the list of declarations in this configuration.
        /// </summary>
        public virtual List<ConfigurationDeclarativeItem> Declarations
        {
            get { return declarations; }
        }

        /// <summary>
        /// Returns/Sets the block configuration.
        /// </summary>
        public virtual AbstractBlockConfiguration BlockConfiguration
        {
            get { return blockConfiguration; }
            set { blockConfiguration = value; }
        }

        public override IScope Scope
        {
            get { throw new NotImplementedException("Not supported yet."); }
        }

        internal override void accept(LibraryUnitVisitor visitor)
        {
            visitor.visitConfiguration(this);
        }
    }

}