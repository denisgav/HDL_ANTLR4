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
    using NamedEntity = VHDL.INamedEntity;

    //TODO: use names and character literals as constituents.
    /// <summary>
    /// Group declaration.
    /// </summary>
    [Serializable]
    public class Group : DeclarativeItem, IBlockDeclarativeItem, IConfigurationDeclarativeItem, IEntityDeclarativeItem, IPackageBodyDeclarativeItem, IPackageDeclarativeItem, IProcessDeclarativeItem, ISubprogramDeclarativeItem, INamedEntity
    {
        private string identifier;
        private GroupTemplate template;
        private readonly List<string> constituents = new List<string>();

        /// <summary>
        /// Creates a group declaration.
        /// </summary>
        /// <param name="identifier">the identifier</param>
        /// <param name="template">the group template</param>
        public Group(string identifier, GroupTemplate template)
        {
            this.identifier = identifier;
            this.template = template;
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
        /// Returns/Sets the group template.
        /// </summary>
        public virtual GroupTemplate Template
        {
            get { return template; }
            set { template = value; }
        }

        /// <summary>
        /// Returns the constituents.
        /// </summary>
        public virtual List<string> Constituents
        {
            get { return constituents; }
        }

        internal override void accept(DeclarationVisitor visitor)
        {
            visitor.visitGroupDeclaration(this);
        }
    }

}