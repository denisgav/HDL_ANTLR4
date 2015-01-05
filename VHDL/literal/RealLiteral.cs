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
using System.Globalization;

namespace VHDL.literal
{
    using SubtypeIndication = VHDL.type.ISubtypeIndication;
    
    /// <summary>
    /// Decimal literal.
    /// </summary>
    [Serializable]
    public class RealLiteral : AbstractLiteral
    {
        private string @value;

        /// <summary>
        /// Creates a decimal literal.
        /// </summary>
        /// <param name="value">the value</param>
        public RealLiteral(string @value)
        {
            this.value = @value;
        }

        /// <summary>
        /// Creates a decimal literal by converting a integer value.
        /// </summary>
        /// <param name="value">the integer value</param>
        public RealLiteral(double @value)
        {
            this.value = Convert.ToString(@value);
        }

        /// <summary>
        /// Returns/Sets the value.
        /// </summary>
        public virtual string Value
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

        /// <summary>
        /// Целочисленное значение
        /// </summary>
        public double RealValue
        {
            get
            {
                return convertToDouble(value);
            }
        }

        private double convertToDouble(string str)
        {
            double dbl;

            if (double.TryParse(str, NumberStyles.Any,  CultureInfo.InvariantCulture, out dbl))
                return dbl;

            if (str.Equals(double.MaxValue.ToString()))
                return double.MaxValue;

            return double.MinValue;
        }

        public override SubtypeIndication Type
        {
            get { return VHDL.builtin.Standard.REAL; }
        }

        public override Choice copy()
        {
            return new RealLiteral(@value);
        }

        public override string ToString()
        {
            return @value;
        }

        public override void accept(ILiteralVisitor visitor)
        {
            visitor.visit(this);
        }
    }

}