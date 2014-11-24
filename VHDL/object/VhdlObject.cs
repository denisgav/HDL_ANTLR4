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

using VHDL.expression;
using System.Collections.Generic;
using System;

namespace VHDL.Object
{
    using NamedEntity = VHDL.INamedEntity;
    using SubtypeIndication = VHDL.type.ISubtypeIndication;

    /// <summary>
    /// VHDL object.
    /// </summary>
    [Serializable]
    public abstract class VhdlObject : Name, IVhdlObjectProvider, INamedEntity
    {

        /// <summary>
        /// Returns/Sets the identifier of this attribtue.
        /// </summary>
        public abstract string Identifier { get; set; }

        /// <summary>
        /// Returns/Sets the mode of this vhdl object.
        /// </summary>
        public abstract ModeEnum Mode { get; set; }

        /// <summary>
        /// Returns the type of this VhdlObject.
        /// </summary>
        public abstract ObjectClassEnum ObjectClass { get; }

        /// <summary>
        /// Object class describes the type of VhdlObject.
        /// </summary>
        public enum ObjectClassEnum
        {
            /// Constant. 
            CONSTANT,
            /// File. 
            FILE,
            /// Signal. 
            SIGNAL,
            /// Variable. 
            VARIABLE
        }
        public enum ModeEnum
        {
            /// None. 
            NONE,
            /// In. 
            IN,
            /// Out. 
            OUT,
            /// InOut. 
            INOUT,
            /// Buffer. 
            BUFFER,
            /// Linkage. 
            LINKAGE
        }

        #region VhdlObjectProvider Members

        public virtual IList<VhdlObject> VhdlObjects
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion
    }
}