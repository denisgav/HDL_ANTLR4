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
using System.Linq;
using System.Text;
using System.Globalization;
using VHDL.type;

namespace VHDL.literal
{
    [Serializable]
    public abstract class IntegerLiteral : AbstractLiteral
    {
        protected string @value;
        protected Int64 @integerValue;

        /// <summary>
        /// Creates a decimal literal.
        /// </summary>
        /// <param name="value">the value</param>
        public IntegerLiteral(string @value)
        {
            this.value = @value;
            @integerValue = ConvertToInt();
        }

        /// <summary>
        /// Creates a decimal literal by converting a integer value.
        /// </summary>
        /// <param name="value">the integer value</param>
        public IntegerLiteral(Int64 @value)
        {
            this.@value = ConvertToString();
            @integerValue = @value;
        }

        /// <summary>
        /// Returns the value.
        /// </summary>
        public string Value
        {
            get
            {
                return @value;
            }
        }

        /// <summary>
        /// Целочисленное значение
        /// </summary>
        public Int64 IntegerValue
        {
            get
            {
                return @integerValue;
            }
        }

        public abstract Int64 ConvertToInt();


        public abstract string ConvertToString();


        public override ISubtypeIndication Type
        {
            get { return VHDL.builtin.Standard.INTEGER; }
        }

        public override string ToString()
        {
            return @value;
        }

        public static Int64 ConvertToIntDec(string value)
        {
            return Int64.Parse(value, CultureInfo.InvariantCulture);
        }

        public static string ConvertToStringDec(Int64 value)
        {
            return Convert.ToString(value, CultureInfo.InvariantCulture); ;
        }

        public static Int64 ConvertToIntBased(string value, int num_base)
        {
            Int64 res = 0;

            string num_base_s = string.Empty;
            string num_value_s = string.Empty;
            string num_exp_s = string.Empty;

            int num_base_i = 0;
            Int64 num_value_i = 0;
            int num_exp_i = 0;
            Int64 pow = 1;

            string[] tokens = value.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);

            num_base_s = (tokens.Length >= 1) ? tokens[0] : string.Empty;
            num_value_s = (tokens.Length >= 2) ? tokens[1] : string.Empty;
            num_exp_s = (tokens.Length >= 3) ? tokens[2] : string.Empty;

            if (num_base_s == string.Empty)
                throw new MissingFieldException(string.Format("Could not find base of the integer based number {0}", value));

            if (num_value_s == string.Empty)
                throw new MissingFieldException(string.Format("Could not find value of the integer based number {0}", value));

            num_base_i = int.Parse(num_base_s, CultureInfo.InvariantCulture);

            if (num_base_i != num_base)
                throw new MissingFieldException(string.Format("Base of the value is {0}, input base parameter is {1}. Please check convert and parse method. Input value is {2}", num_base_i, num_base, value));

            num_value_i = Convert.ToInt32(num_value_s, num_base_i);

            if (string.IsNullOrEmpty(num_exp_s) == false)
            {
                num_exp_i = int.Parse(num_exp_s, CultureInfo.InvariantCulture);
                for (int i = 0; i < num_exp_i; i++)
                    pow *= num_base_i;
            }

            res = num_value_i * pow;

            return res;
        }

        public static string ConvertToStringBased(Int64 value, int num_base)
        {
            return ConvertToStringBased(value, num_base, "{0}#{1}#{2}");
        }

        public static string ConvertToStringBased(Int64 value, int num_base, string format)
        {
            string num_base_s = num_base.ToString();
            string num_value_s = Convert.ToString(value, num_base);
            string num_exp_s = string.Empty;

            return string.Format(format, num_base_s, num_value_s, num_exp_s);
        }

        public override void accept(ILiteralVisitor visitor)
        {
            visitor.visit(this);
        }
    }
}
