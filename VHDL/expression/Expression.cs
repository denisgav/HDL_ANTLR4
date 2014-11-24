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

namespace VHDL.expression
{
    using SubtypeIndication = VHDL.type.ISubtypeIndication;

    /// <summary>
    /// Abstract expression.
    /// </summary>
    [Serializable]
    public abstract class Expression : Choice
    {
        /// <summary>
        /// Returns the type of this expression.
        /// </summary>
        public abstract SubtypeIndication Type { get; }

        //TODO: make package private?
        /// <summary>
        /// Returns the precedence of this expression.
        /// </summary>
        public abstract int Precedence { get; }

        /// <summary>
        /// Used to implement the visitor pattern.
        /// </summary>
        /// <param name="visitor">the visitor</param>
        public abstract void accept(ExpressionVisitor visitor);
    }
}