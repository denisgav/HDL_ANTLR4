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

namespace VHDL.type
{
    using NamedEntity = VHDL.INamedEntity;

    //TODO: remove class?
    /// <summary>
    /// Unresolved type.
    /// </summary>
    [Serializable]
    public class UnresolvedType : ISubtypeIndication, INamedEntity
    {

        /// Unresolved type with unknown name. 
        public static readonly UnresolvedType NO_NAME = new UnresolvedType("no_name");
        private string identifier;

        /// <summary>
        /// Creates an unresolved type.
        /// </summary>
        /// <param name="identifier">the identifier</param>
        public UnresolvedType(string identifier)
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

        public void accept(ISubtypeIndicationVisitor visitor)
        {
            visitor.visit(this);
        }

        public ISubtypeIndication BaseType
        {
            get { return null; }
        }
    }

}