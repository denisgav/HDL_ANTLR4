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
using VHDL.expression;

namespace VHDL.literal
{
    using SubtypeIndication = VHDL.type.ISubtypeIndication;

    /// <summary>
    /// Literals.
    /// </summary>
    [Serializable]
    public class Literals
    {
        /// <summary>
        /// NULL literal.
        /// </summary>
        public static readonly Expression NULL = new NullLiteral();

        private Literals()
        {
        }        

        [Serializable]
        public class NullLiteral : Literal
        {

            public override SubtypeIndication Type
            {
                get { throw new Exception("Not supported yet."); }
            }

            //TODO: use output case setting
            public override string ToString()
            {
                return "null";
            }

            public override Choice copy()
            {
                //No need to copy becaue NULL is case insensitive.
                return this;
            }

            public override void accept(ILiteralVisitor visitor)
            {
                visitor.visit(this);
            }
        }
    }

}