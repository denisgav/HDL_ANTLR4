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
    /// Concatenation expression.
    /// </summary>
    [Serializable]
    public class Concatenate : AddingExpression
    {
        /// <summary>
        /// Creates a concatenation epxression
        /// </summary>
        /// <param name="left">the left-hand side expression</param>
        /// <param name="right">the right-hand side expression</param>
        public Concatenate(Expression left, Expression right)
            : base(left, ExpressionKind.CONCATENATE, right)
        {
        }

        //TODO: implement using subtype indication
        public override SubtypeIndication Type
        {
            get
            {
                throw new Exception();
                //        SubtypeIndication leftType = getLeft().getType();
                //        SubtypeIndication rightType = getLeft().getType();
                //
                //        boolean isLeftArray = leftType instanceof ArrayTypeDefinition;
                //        boolean isRightArray = rightType instanceof ArrayTypeDefinition;
                //
                //        if (!isLeftArray && !isRightArray) {
                //        return new ConstrainedArrayDefinition(null, new Range(1, Range.Direction.TO, 0), leftType);
                //        } else {
                //        return null;
                //        }
            }
        }

        public override Choice copy()
        {
            return new Concatenate(Left.copy() as Expression, Right.copy() as Expression);
        }
    }

}