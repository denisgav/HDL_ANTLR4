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


using VHDL.expression;
using System;

namespace VHDL.literal
{
    using SubtypeIndication = VHDL.type.ISubtypeIndication;

    /// <summary>
    /// Character literal.
    /// </summary>
    [Serializable]
    public class CharacterLiteral : Literal
    {
        private readonly char character;

        /// <summary>
        /// Creates a character literal.
        /// </summary>
        /// <param name="character">the character</param>
        public CharacterLiteral(char character)
        {
            this.character = character;
        }

        /// <summary>
        /// Returns the character
        /// </summary>
        public virtual char Character
        {
            get { return character; }
        }

        public override SubtypeIndication Type
        {
            get { throw new Exception("Not supported yet."); }
        }

        public override Choice copy()
        {
            return new CharacterLiteral(character);
        }

        public override string ToString()
        {
            return "'" + character + "'";
        }

        public override void accept(ILiteralVisitor visitor)
        {
            visitor.visit(this);
        }
    }

}