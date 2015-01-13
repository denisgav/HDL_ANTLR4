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
    using EnumerationLiteral = VHDL.literal.EnumerationLiteral;

    /// <summary>
    /// Enumeration type.
    /// </summary>
    [Serializable]
    public class EnumerationType : Type
    {
        private readonly List<EnumerationLiteral> literals = new List<EnumerationLiteral>();

        /// <summary>
        /// Creates an empty enumeration type.
        /// </summary>
        /// <param name="identifier">the identifier</param>
        public EnumerationType(string identifier)
            : base(identifier)
        {
        }

        /// <summary>
        /// Creates a enumeration type with the given identifier literals.
        /// </summary>
        /// <param name="identifier">the identifier of this enumeration type</param>
        /// <param name="literals">the identifier literals</param>
        public EnumerationType(string identifier, params string[] literals)
            : base(identifier)
        {

            foreach (string literal in literals)
            {
                this.literals.Add(new IdentifierEnumerationLiteral(literal, this));
            }
        }

        /// <summary>
        /// Creates a enumeration type with the given character literals.
        /// </summary>
        /// <param name="identifier"> the identifier of this enumeration type</param>
        /// <param name="literals">the character literals</param>
        public EnumerationType(string identifier, params char[] literals)
            : base(identifier)
        {
            AddLiterals(literals);
            
        }


        public void AddLiterals(params char[] literals)
        {
            foreach (char literal in literals)
            {
                this.literals.Add(new CharacterEnumerationLiteral(literal, this));
            }
        }

        public void AddLiterals(params string[] literals)
        {
            foreach (string literal in literals)
            {
                this.literals.Add(new IdentifierEnumerationLiteral(literal, this));
            }
        }

        /// <summary>
        /// Returns the literals.
        /// </summary>
        public virtual List<EnumerationLiteral> Literals
        {
            get { return literals; }
        }

        /// <summary>
        /// Creates a character enumeration literal and adds it to this literal.
        /// </summary>
        /// <param name="literal">the literal value</param>
        /// <returns>the created enumeration literal</returns>
        public virtual EnumerationLiteral createLiteral(char literal)
        {
            EnumerationLiteral l = new CharacterEnumerationLiteral(literal, this);
            literals.Add(l);
            return l;
        }

        /// <summary>
        /// Creates a character enumeration literal and adds it to this literal.
        /// </summary>
        /// <param name="literal">the literal value</param>
        /// <returns>the created enumeration literal</returns>     
        public virtual EnumerationLiteral createLiteral(string literal)
        {
            EnumerationLiteral l = new IdentifierEnumerationLiteral(literal, this);
            literals.Add(l);
            return l;
        }

        internal override void accept(TypeVisitor visitor)
        {
            visitor.visitEnumerationType(this);
        }

        [Serializable]
        public class IdentifierEnumerationLiteral : EnumerationLiteral
        {
            private readonly string literal;

            public IdentifierEnumerationLiteral(string literal, EnumerationType type)
                : base(type)
            {
                this.literal = literal;
            }

            public virtual string getLiteral()
            {
                return literal;
            }

            public override string ToString()
            {
                return literal;
            }
        }

        [Serializable]
        public class CharacterEnumerationLiteral : EnumerationLiteral
        {
            private readonly char literal;

            public CharacterEnumerationLiteral(char literal, EnumerationType type)
                : base(type)
            {
                this.literal = literal;
            }

            public virtual char getLiteral()
            {
                return literal;
            }

            public override string ToString()
            {
                return "'" + literal + "'";
            }
        }
    }
}