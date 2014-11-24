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
    using EnumerationType = VHDL.type.EnumerationType;
    using SubtypeIndication = VHDL.type.ISubtypeIndication;

    /// <summary>
    /// Enumeration literal meta class.
    /// </summary>
    [Serializable]
    public abstract class EnumerationLiteral : Literal
    {
        private readonly EnumerationType type;

        /// <summary>
        /// Creates an enumeration literal.
        /// </summary>
        /// <param name="type">the enumeration type</param>
        public EnumerationLiteral(EnumerationType type)
        {
            this.type = type;
        }

        /// <summary>
        /// Returns the type.
        /// </summary>
        public override SubtypeIndication Type
        {
            get { return type; }
        }

        public override Choice copy()
        {
            //EnumerationLiterals are inmutable
            return this;
        }

        public override void accept(ILiteralVisitor visitor)
        {
            visitor.visit(this);
        }
    }

}