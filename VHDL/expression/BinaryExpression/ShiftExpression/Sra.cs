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
    /// <summary>
    /// Shift right arithmetic expression.
    /// </summary>
    [Serializable]
	public class Sra : ShiftExpression
	{
        /// <summary>
        /// Creates a shift right arithmetic expression.
        /// </summary>
        /// <param name="left">the left-hand side expression</param>
        /// <param name="right">the right-hand side expression</param>
		public Sra(Expression left, Expression right) : base(left, ExpressionKind.SRA, right)
		{
		}

        public override Choice copy()
		{
            return new Sra(Left.copy() as Expression, Right.copy() as Expression);
		}
	}

}