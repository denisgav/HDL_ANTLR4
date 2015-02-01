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

namespace VHDL.type
{
    using NamedEntity = VHDL.INamedEntity;
    using RangeProvider = VHDL.RangeProvider;
    using AbstractLiteral = VHDL.literal.AbstractLiteral;
    using DecimalLiteral = VHDL.literal.DecBasedInteger;

    /// <summary>
    /// Physical type.
    /// </summary>
    [Serializable]
    public class PhysicalType : Type
    {
        private RangeProvider range;
        private string primaryUnit;
        private readonly List<Unit> units = new List<Unit>();

        /// <summary>
        /// Creates a physical type.
        /// </summary>
        /// <param name="identifier">the identifier of this type</param>
        /// <param name="range">the range</param>
        /// <param name="primaryUnit">the primary unit identifier</param>
        public PhysicalType(string identifier, RangeProvider range, string primaryUnit)
            : base(identifier)
        {
            this.range = range;
            this.primaryUnit = primaryUnit;
        }

        /// <summary>
        /// Returns/Sets the identifier of the primary unit.
        /// </summary>
        public virtual string PrimaryUnit
        {
            get { return primaryUnit; }
            set { primaryUnit = value; }
        }

        // <summary>
        /// Returns/Sets the range of this type
        /// </summary>
        public virtual RangeProvider Range
        {
            get { return range; }
            set { range = value; }
        }

        /// <summary>
        /// Returns the list of units.
        /// </summary>
        public virtual List<Unit> Units
        {
            get { return units; }
        }

        /// <summary>
        /// Creates a unit and adds it to this physical type.
        /// </summary>
        /// <param name="identifier">the unit's identifier</param>
        /// <param name="factor">the factor</param>
        /// <param name="baseUnit">the base unit</param>
        /// <returns>the created unit</returns>
        public virtual Unit createUnit(string identifier, AbstractLiteral factor, string baseUnit)
        {
            Int64 baseUnits = GetBaseUnits(baseUnit);
            Int64 int_factor = (factor as VHDL.literal.IntegerLiteral).IntegerValue;
            Unit unit = new Unit(identifier, int_factor, baseUnits * int_factor, baseUnit);
            units.Add(unit);

            return unit;
        }

        /// <summary>
        /// Creates a unit with a integer factor and adds it to this physical type.
        /// </summary>
        /// <param name="identifier">the unit's identifier</param>
        /// <param name="factor">the factor</param>
        /// <param name="baseUnit">the base unit</param>
        /// <returns>the created unit</returns>
        public virtual Unit createUnit(string identifier, Int64 factor, string baseUnit)
        {
            Int64 baseUnits = GetBaseUnits(baseUnit);
            Unit unit = new Unit(identifier, factor, baseUnits * factor, baseUnit);
            units.Add(unit);

            return unit;
        }

        /// <summary>
        /// Creates a unit without a factor and adds it to this physical type.
        /// Units without a factor implicitly use 1 as a factor.
        /// </summary>
        /// <param name="identifier">the unit's identifier</param>
        /// <returns></returns>
        public virtual Unit createUnit(string identifier)
        {
            Unit unit = new Unit(identifier, 1, 1, identifier);
            units.Add(unit);

            return unit;
        }

        public Int64 GetBaseUnits(string identifier)
        {
            foreach (Unit u in units)
            {
                if (u.Identifier.EqualsIdentifier(identifier))
                    return u.getInBaseUnits();
            }
            throw new Exception(string.Format("Could not find unit {0} in type {1}", identifier, GetType().FullName));
        }



        internal override void accept(TypeVisitor visitor)
        {
            visitor.visitPhysicalType(this);
        }

        /// <summary>
        /// A unit in a physical type.
        /// </summary>
        [Serializable]
        public class Unit : VhdlElement, INamedEntity
        {
            private string identifier;
            private Int64 factor;
            private Int64 inBaseUnits;
            private string baseUnit;

            public Unit(string identifier, Int64 factor, Int64 inBaseUnits, string baseUnit)
            {
                this.inBaseUnits = inBaseUnits;
                this.identifier = identifier;
                this.factor = factor;
                this.baseUnit = baseUnit;
            }

            /// <summary>
            /// Returns/Sets the base unit of this unit.
            /// </summary>
            public virtual string BaseUnit
            {
                get { return baseUnit; }
                set { baseUnit = value; }
            }

            public Int64 getInBaseUnits()
            {
                return inBaseUnits;
            }

            /// <summary>
            /// Returns/Sets the factor.
            /// </summary>
            public virtual Int64 Factor
            {
                get { return factor; }
                set { factor = value; }
            }
            
            /// <summary>
            /// Returns/Sets the identifier of this unit.
            /// </summary>
            public virtual string Identifier
            {
                get { return identifier; }
                set { this.identifier = value; }
            }
        }
    }

}