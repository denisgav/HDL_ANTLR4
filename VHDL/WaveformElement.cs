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


using Expression = VHDL.expression.Expression;
using System;

namespace VHDL
{
    /// <summary>
    /// Waveform element.
    /// </summary>
    [Serializable]
    public class WaveformElement
    {
        private Expression @value;
        private Expression after;

        /// <summary>
        /// Creates a waveform element.
        /// </summary>
        /// <param name="value">the value</param>
        public WaveformElement(Expression @value)
        {
            this.value = @value;
        }

        /// <summary>
        /// Creates a waveform element with a delay.
        /// </summary>
        /// <param name="value">value the value</param>
        /// <param name="after">after the delay</param>
        public WaveformElement(Expression @value, Expression after)
        {
            this.value = @value;
            this.after = after;
        }

        /// <summary>
        /// Gets/Sets the delay of this waveform element.
        /// </summary>
        public virtual Expression After
        {
            get { return after; }
            set { after = value; }
        }

        /// <summary>
        /// Gets/Sets the value of this waveform element.
        /// </summary>
        public virtual Expression Value
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
    }

}