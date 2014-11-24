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

namespace VHDL.builtin
{
    using Attribute = VHDL.declaration.Attribute;

    //TODO: fix types
    /// <summary>
    /// Predefined signal attributes.
    /// </summary>
    public class SignalAttributes
    {

        /// DELAYED attribute. 
        public static readonly Attribute DELAYED = new Attribute("DELAYED", null);
        /// STABLE attribute. 
        public static readonly Attribute STABLE = new Attribute("STABLE", Standard.BOOLEAN);
        /// QUIET attribute. 
        public static readonly Attribute QUIET = new Attribute("QUIET", Standard.BOOLEAN);
        /// TRANSACTION attribute. 
        public static readonly Attribute TRANSACTION = new Attribute("TRANSACTION", Standard.BIT);
        /// EVENT attribute. 
        public static readonly Attribute EVENT = new Attribute("EVENT", Standard.BOOLEAN);
        /// ACTIVE attribute. 
        public static readonly Attribute ACTIVE = new Attribute("ACTIVE", Standard.BOOLEAN);
        /// LAST_EVENT attribute. 
        public static readonly Attribute LAST_EVENT = new Attribute("LAST_EVENT", null);
        /// LAST_ACTIVE attribute. 
        public static readonly Attribute LAST_ACTIVE = new Attribute("LAST_ACTIVE", null);
        /// LAST_VALUE attribute. 
        public static readonly Attribute LAST_VALUE = new Attribute("LAST_VALUE", null);
        /// DRIVING attribute. 
        public static readonly Attribute DRIVING = new Attribute("DRIVING", Standard.BOOLEAN);
        /// DRIVING_VALUE attribute. 
        public static readonly Attribute DRIVING_VALUE = new Attribute("DRIVING_VALUE", null);

        /// <summary>
        /// Prevent instantiation.
        /// </summary>
        private SignalAttributes()
        {
        }
    }

}