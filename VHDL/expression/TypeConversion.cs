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

using VHDL.type;
using System;

namespace VHDL.expression
{
    using SubtypeIndication = VHDL.type.ISubtypeIndication;

    /// <summary>
    /// Type conversion expression.
    /// </summary>
    [Serializable]
    public class TypeConversion : Primary
    {

        //TODO: replace SubtypeIndication by type mark
        private SubtypeIndication type;
        private Expression expression;

        /// <summary>
        /// Creates a type conversion.
        /// </summary>
        /// <param name="type">the type</param>
        /// <param name="expression">the converted expression</param>
        public TypeConversion(SubtypeIndication type, Expression expression)
        {
            this.type = type;
            this.expression = expression;
        }

        /// <summary>
        /// Returns/Sets the converted expression.
        /// </summary>
        public virtual Expression Expression
        {
            get { return expression; }
            set { expression = value; }
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
            //TODO: copy subtype indication
            return new TypeConversion(type, expression.copy() as Expression);
        }

        public override void accept(ExpressionVisitor visitor)
        {
            visitor.visitTypeConversion(this);
        }
    }
}