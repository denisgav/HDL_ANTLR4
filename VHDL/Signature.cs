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
using SubtypeIndication = VHDL.type.ISubtypeIndication;
using System;

namespace VHDL
{
    /// <summary>
    /// Signature.
    /// </summary>
    [Serializable]
    public class Signature : VhdlElement
    {

        //TODO: use type_mark
        private readonly List<SubtypeIndication> parameterTypes;
        private SubtypeIndication returnType;

        /// <summary>
        /// Creates an empty signature.
        /// </summary>
        public Signature()
        {
            this.returnType = null;
            this.parameterTypes = new List<SubtypeIndication>();
        }

        /// <summary>
        /// Creates a signature with a return type and a variable number of parameter types.
        /// </summary>
        /// <param name="returnType">the return type</param>
        /// <param name="parameterTypes">the parameter types</param>
        public Signature(SubtypeIndication returnType, params SubtypeIndication[] parameterTypes)
            : this(returnType, new List<SubtypeIndication>(parameterTypes))
        {
        }

        /// <summary>
        /// Creates a signature with a return type and a list of parameter types.
        /// </summary>
        /// <param name="returnType">the return type</param>
        /// <param name="parameterTypes">the list of parameter types</param>
        public Signature(SubtypeIndication returnType, List<SubtypeIndication> parameterTypes)
        {
            this.returnType = returnType;
            this.parameterTypes = new List<SubtypeIndication>(parameterTypes);
        }

        /// <summary>
        /// Creates a signature with a return type.
        /// </summary>
        /// <param name="returnType">the return type.</param>
        public Signature(SubtypeIndication returnType)
        {
            this.returnType = returnType;
            this.parameterTypes = new List<SubtypeIndication>();
        }

        /// <summary>
        /// Creates a signature with a list of parameter types.
        /// </summary>
        /// <param name="parameterTypes">the list of parameter types</param>
        public Signature(List<SubtypeIndication> parameterTypes)
        {
            this.returnType = null;
            this.parameterTypes = new List<SubtypeIndication>(parameterTypes);
        }

        /// <summary>
        /// Returns/Sets the return type of this signature.
        /// </summary>
        public virtual SubtypeIndication ReturnType
        {
            get { return returnType; }
            set { returnType = value; }
        }

        /// <summary>
        /// Returns a list of parameter types.
        /// </summary>
        public virtual List<SubtypeIndication> ParameterTypes
        {
            get { return parameterTypes; }
        }
    }

}