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

    /// <summary>
    /// Group template.
    /// </summary>
    [Serializable]
    public class GroupTemplate : DeclarativeItem, IBlockDeclarativeItem, IEntityDeclarativeItem, IPackageBodyDeclarativeItem, IPackageDeclarativeItem, IProcessDeclarativeItem, ISubprogramDeclarativeItem, INamedEntity
    {
        private string identifier;
        private readonly List<EntityClass> entityClasses = new List<EntityClass>();
        private bool repeatLast;

        /// <summary>
        /// Creates a group template.
        /// </summary>
        /// <param name="identifier">the identifier of this group template</param>
        public GroupTemplate(string identifier)
        {
            this.identifier = identifier;
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
        /// Returns the entity classes.
        /// </summary>
        public virtual List<EntityClass> EntityClasses
        {
            get { return entityClasses; }
        }

        /// <summary>
        /// Returns/Sets if the last entity class is repeated.
        /// </summary>
        public virtual bool RepeatLast
        {
            get { return repeatLast; }
            set { repeatLast = value; }
        }

        internal override void accept(DeclarationVisitor visitor)
        {
            visitor.visitGroupTemplateDeclaration(this);
        }
    }
}