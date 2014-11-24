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

using DecimalLiteral = VHDL.literal.DecBasedInteger;
using RealLiteral = VHDL.literal.RealLiteral;
using Expression = VHDL.expression.Expression;
using System;

namespace VHDL
{
    /// <summary>
    /// Range.
    /// </summary>
    [Serializable]
    public class Range : RangeProvider
    {
        private Expression from;
        private RangeDirection direction;
        private Expression to;

        /// <summary>
        /// Creates a range.
        /// </summary>
        /// <param name="from">the from expression</param>
        /// <param name="direction">the direction</param>
        /// <param name="to">the to expression</param>
        public Range(Expression from, RangeDirection direction, Expression to)
        {
            this.from = from;
            this.direction = direction;
            this.to = to;
        }

        /// <summary>
        /// Creates a range with integer bounds.
        /// </summary>
        /// <param name="from">the from value</param>
        /// <param name="direction">the direction</param>
        /// <param name="to">the to value</param>
        public Range(int from, RangeDirection direction, int to)
            : this(new DecimalLiteral(from), direction, new DecimalLiteral(to))
        {
        }

        /// <summary>
        /// Creates a range with integer bounds.
        /// </summary>
        /// <param name="from">the from value</param>
        /// <param name="direction">the direction</param>
        /// <param name="to">the to value</param>
        public Range(double from, RangeDirection direction, double to)
            : this(new RealLiteral(from), direction, new RealLiteral(to))
        {
        }

        /// <summary>
        /// Gets/Sets the direction of this range.
        /// </summary>
        public virtual RangeDirection Direction
        {
            get { return direction; }
            set { this.direction = value; }
        }

        /// <summary>
        /// Gets/Sets the from expression.
        /// </summary>
        public virtual Expression From
        {
            get { return from; }
            set { this.from = value; }
        }

        /// <summary>
        /// Gets/Sets the to expression.
        /// </summary>
        public virtual Expression To
        {
            get { return to; }
            set { this.to = value; }
        }

        /// <summary>
        /// Sets the from expression.
        /// </summary>
        /// <param name="from">the value of the from expression</param>
        public virtual void setFrom(int from)
        {
            this.from = new DecimalLiteral(from);
        }

        /// <summary>
        /// Sets the to expression.
        /// </summary>
        /// <param name="to">the value of the to expression</param>
        public virtual void setTo(int to)
        {
            this.to = new DecimalLiteral(to);
        }

        public override Choice copy()
        {
            return new Range(from.copy() as Expression, direction, to.copy() as Expression);
        }

        //    *
        //     * Range direction.
        //     
        public enum RangeDirection
        {
            /// TO direction 
            TO,
            /// DOWNTO direction 
            DOWNTO
        }
    }

}