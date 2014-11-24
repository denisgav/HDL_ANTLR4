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

namespace VHDL.Object
{
    //TODO: check for equal types
    /// <summary>
    /// Group of signals.
    /// </summary>
    [Serializable]
    public class SignalGroup : VhdlObjectGroup<Signal>
    {
        private readonly List<Signal> signals;

        /// <summary>
        /// Creates a group of signals.
        /// </summary>
        /// <param name="signals"></param>
        public SignalGroup(List<Signal> signals)
        {
            this.signals = new List<Signal>(signals);
        }

        /// <summary>
        /// Creates a group of signals.
        /// </summary>
        /// <param name="signals"></param>
        public SignalGroup(params Signal[] signals)
            : this(new List<Signal>(signals))
        {
        }

        /// <summary>
        /// Returns the signals in this group.
        /// </summary>
        public override IList<Signal> Elements
        {
            get
            {
                return signals;
            }
        }

        public override IList<VhdlObject> VhdlObjects
        {
            get
            {
                List<VhdlObject> res = new List<VhdlObject>();
                foreach (var s in signals)
                    res.Add(s);
                return res;
            }
        }
    }

}