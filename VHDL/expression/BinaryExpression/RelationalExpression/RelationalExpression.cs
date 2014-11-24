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
    using Standard = VHDL.builtin.Standard;
    using SubtypeIndication = VHDL.type.ISubtypeIndication;

    /// <summary>
    /// Abstract base class for relational expressions.
    /// </summary>
    [Serializable]
    public abstract class RelationalExpression : BinaryExpression
    {
        /// <summary>
        /// Creates a <code>RelationalExpression</code>.
        /// </summary>
        /// <param name="left">the left-hand side expression</param>
        /// <param name="kind">the expression kind</param>
        /// <param name="right">the right-hand side expression</param>
        internal RelationalExpression(Expression left, ExpressionKind kind, Expression right)
            : base(left, kind, right, ExpressionPrecedences.RELATIONAL_EXPRESSION)
        {
        }

        public override SubtypeIndication Type
        {
            get { return Standard.BOOLEAN; }
        }
    }

}