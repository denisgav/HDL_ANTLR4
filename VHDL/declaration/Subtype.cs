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

namespace VHDL.declaration
{
    using NamedEntity = VHDL.INamedEntity;
    using SubtypeIndication = VHDL.type.ISubtypeIndication;

    /// <summary>
    /// Subtype declaration.
    /// </summary>
    [Serializable]
    public class Subtype : DeclarativeItem, IBlockDeclarativeItem, IEntityDeclarativeItem, IPackageBodyDeclarativeItem, IPackageDeclarativeItem, IProcessDeclarativeItem, ISubprogramDeclarativeItem, SubtypeIndication, INamedEntity
    {
        private string identifier;
        private SubtypeIndication subtypeIndication;

        /// <summary>
        /// Creates a subtype declaration.
        /// </summary>
        /// <param name="identifier">the identifier of this subtype declaration</param>
        /// <param name="subtypeIndication">the subtype indication</param>
        public Subtype(string identifier, SubtypeIndication subtypeIndication)
        {
            this.identifier = identifier;
            this.subtypeIndication = subtypeIndication;
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
        /// Return/Set the subtype indication.
        /// </summary>
        public virtual SubtypeIndication SubtypeIndication
        {
            get { return subtypeIndication; }
            set { subtypeIndication = value; }
        }

        internal override void accept(DeclarationVisitor visitor)
        {
            visitor.visitSubtypeDeclaration(this);
        }

        public void accept(VHDL.type.ISubtypeIndicationVisitor visitor)
        {
            visitor.visit(this);
        }

        public SubtypeIndication BaseType
        {
            get { return subtypeIndication; }
        }
    }
}