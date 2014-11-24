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

using System.Collections.Generic;
using System;

namespace VHDL.Object
{
    //TODO: check for equal types
    /// <summary>
    /// Group of constants.
    /// </summary>
    [Serializable]
    public class ConstantGroup : VhdlObjectGroup<Constant>
    {
        private readonly List<Constant> constants;

        /// <summary>
        /// Creates a group of constants.
        /// </summary>
        /// <param name="constants">a list of constants</param>
        public ConstantGroup(List<Constant> constants)
        {
            this.constants = new List<Constant>(constants);
        }

        /// <summary>
        /// Creates a group of constants.
        /// </summary>
        /// <param name="constants">a list of constants</param>
        public ConstantGroup(params Constant[] constants)
            : this(new List<Constant>(constants))
        {
        }

        /// <summary>
        /// Returns the constants in this group.
        /// </summary>
        public override IList<Constant> Elements
        {
            get { return constants; }
        }

        public override IList<VhdlObject> VhdlObjects
        {
            get
            {
                List<VhdlObject> res = new List<VhdlObject>();
                foreach (var c in constants)
                    res.Add(c);
                return res;
            }
        }
    }

}