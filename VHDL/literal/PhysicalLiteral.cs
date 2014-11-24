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
using VHDL.type;

namespace VHDL.literal
{
    using Literal = VHDL.expression.Literal;
    using SubtypeIndication = VHDL.type.ISubtypeIndication;

    /// <summary>
    /// Physical literal.
    /// </summary>
    [Serializable]
    public class PhysicalLiteral : Literal
    {
        private AbstractLiteral @value;
        private string unit;
        private PhysicalType parent;


        public PhysicalType GetPhysicalType()
        {
            return parent;
        }

        /// <summary>
        /// Creates a physical literal containing only a unit.
        /// </summary>
        /// <param name="unit">the unit</param>
        public PhysicalLiteral(string unit)
        {
            this.unit = unit;
            foreach (var u in VHDL.builtin.Standard.TIME.Units)
            {
                if (u.Identifier.Equals(unit))
                {
                    parent = VHDL.builtin.Standard.TIME;
                }
            }
        }

        /// <summary>
        /// Creates a physical literal.
        /// </summary>
        /// <param name="value">the value</param>
        /// <param name="unit">the unit</param>
        public PhysicalLiteral(AbstractLiteral @value, string unit)
        {
            this.value = @value;
            this.unit = unit;
            foreach (var u in VHDL.builtin.Standard.TIME.Units)
            {
                if (u.Identifier.Equals(unit))
                {
                    parent = VHDL.builtin.Standard.TIME;
                }
            }
        }

        /// <summary>
        /// Returns/Sets the unit.
        /// </summary>
        public virtual string Unit
        {
            get { return unit; }
            set { unit = value; }
        }

        /// <summary>
        /// Returns/Sets the value.
        /// </summary>
        public virtual AbstractLiteral Value
        {
            get
            {
                return @value;
            }
            set
            {
                this.value = value;
            }
        }

        public override SubtypeIndication Type
        {
            get { return parent; }
        }

        public override Choice copy()
        {
            return new PhysicalLiteral(@value, unit);
        }

        public override string ToString()
        {
            if (@value != null)
            {
                return (@value.ToString()) + ' ' + unit;
            }
            else
            {
                return unit;
            }
        }

        public override void accept(ILiteralVisitor visitor)
        {
            visitor.visit(this);
        }
    }

}