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
    using VhdlElement = VHDL.VhdlElement;

    /// <summary>
    /// VhdlObjectGroup of <code>VhdlElement</code>s.
    /// </summary>
    /// <typeparam name="E">the object type</typeparam>
    [Serializable]
    public abstract class VhdlObjectGroup<E> : VhdlElement, IVhdlObjectProvider where E : VhdlObject
    {
        /// <summary>
        /// Returns the elements in this group.
        /// </summary>
        public abstract IList<E> Elements { get; }

        #region VhdlObjectProvider Members

        public abstract IList<VhdlObject> VhdlObjects { get; }

        #endregion
    }

}