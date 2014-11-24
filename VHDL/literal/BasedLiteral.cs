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

namespace VHDL.literal
{
    using SubtypeIndication = VHDL.type.ISubtypeIndication;

    /// <summary>
    /// Based literal.
    /// </summary>
    [Serializable]
    public class BasedLiteral : AbstractLiteral
    {

        private string @value;

        /// <summary>
        /// Creates a based literal.
        /// </summary>
        /// <param name="value">the value</param>
        public BasedLiteral(string @value)
        {
            this.value = @value;
        }

        /// <summary>
        /// Returns/Sets value
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

        public override SubtypeIndication Type
        {
            get { throw new Exception("Not supported yet."); }
        }

        public override Choice copy()
        {
            return new BasedLiteral(@value);
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