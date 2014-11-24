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

using SubtypeIndication = VHDL.type.ISubtypeIndication;
using System;


namespace VHDL
{
    /// <summary>
    /// Wrapper to use a subtype indication as discrete range.
    /// </summary>
    [Serializable]
    public class SubtypeDiscreteRange : DiscreteRange
    {
        private SubtypeIndication subtypeIndication;

        /// <summary>
        /// Creates a discrete range subtype indication wrapper.
        /// </summary>
        /// <param name="subtypeIndication">the wrapped subtype indication</param>
        public SubtypeDiscreteRange(SubtypeIndication subtypeIndication)
        {
            this.subtypeIndication = subtypeIndication;
        }

        /// <summary>
        /// Returns/Sets the wrapped subtype indication.
        /// </summary>
        public virtual SubtypeIndication SubtypeIndication
        {
            get { return subtypeIndication; }
            set { this.subtypeIndication = value; }
        }

        public override Choice copy()
        {
            return new SubtypeDiscreteRange(subtypeIndication);
        }
    }
}