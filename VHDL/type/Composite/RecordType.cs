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

namespace VHDL.type
{
    /// <summary>
    /// Record type.
    /// </summary>
    [Serializable]
    public class RecordType : Type
    {
        private readonly List<ElementDeclaration> elements = new List<ElementDeclaration>();

        /// <summary>
        /// Creates a record type.
        /// </summary>
        /// <param name="identifier">the identifier of this type</param>
        public RecordType(string identifier)
            : base(identifier)
        {
        }

        /// <summary>
        /// Creates a new element declaration and adds it to this record.
        /// </summary>
        /// <param name="type">the type</param>
        /// <param name="identifiers">list of identifiers</param>
        /// <returns>the created element</returns>
        public virtual ElementDeclaration createElement(ISubtypeIndication type, List<string> identifiers)
        {
            ElementDeclaration element = new ElementDeclaration(type, identifiers);
            elements.Add(element);
            return element;
        }

        /// <summary>
        /// Creates a new element declaration and adds it to this record.
        /// </summary>
        /// <param name="type">the type</param>
        /// <param name="identifiers">a variable number of identifiers</param>
        /// <returns>the created element</returns>
        public virtual ElementDeclaration createElement(ISubtypeIndication type, params string[] identifiers)
        {
            return createElement(type, new List<string>(identifiers));
        }

        /// <summary>
        /// Returns the elements.
        /// </summary>
        public virtual List<ElementDeclaration> Elements
        {
            get { return elements; }
        }

        internal override void accept(TypeVisitor visitor)
        {
            visitor.visitRecordType(this);
        }

        /// <summary>
        /// Element declaration in a record type.
        /// </summary>
        [Serializable]
        public class ElementDeclaration : VhdlElement
        {
            private readonly List<string> identifiers;
            private ISubtypeIndication type;

            public ElementDeclaration(ISubtypeIndication type, List<string> identifiers)
            {
                this.type = type;
                this.identifiers = new List<string>(identifiers);
            }

            /// <summary>
            /// Returns the declared identifiers in this element declaration.
            /// </summary>
            public virtual List<string> Identifiers
            {
                get { return identifiers; }
            }

            /// <summary>
            /// Returns/Sets the type of this elements.
            /// </summary>
            public virtual ISubtypeIndication Type
            {
                get { return type; }
                set { type = value; }
            }
        }
    }

}