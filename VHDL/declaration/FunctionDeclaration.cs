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

namespace VHDL.declaration
{
    using VhdlObjectProvider = VHDL.Object.IVhdlObjectProvider;
    using SubtypeIndication = VHDL.type.ISubtypeIndication;

    /// <summary>
    /// Function declaration.
    /// </summary>
    [Serializable]
    public class FunctionDeclaration : SubprogramDeclaration, IFunction
    {
        private SubtypeIndication returnType;
        private bool impure;

        /// <summary>
        /// Creates a function declaration.
        /// </summary>
        /// <param name="identifier">the identifier</param>
        /// <param name="returnType">the return type</param>
        /// <param name="parameters">the parameters</param>
        public FunctionDeclaration(string identifier, SubtypeIndication returnType, params VhdlObjectProvider[] parameters)
            : base(identifier, parameters)
        {
            this.returnType = returnType;
        }

        /// <summary>
        /// Creates a function declaration.
        /// </summary>
        /// <param name="identifier">the identifier</param>
        /// <param name="returnType">the return type</param>
        /// <param name="parameters">the parameters</param>     
        public FunctionDeclaration(string identifier, SubtypeIndication returnType, List<VhdlObjectProvider> parameters)
            : base(identifier, parameters)
        {
            this.returnType = returnType;
        }

        public virtual bool Impure
        {
            get { return impure; }
            set { impure = value; }
        }


        public virtual SubtypeIndication ReturnType
        {
            get { return returnType; }
            set { returnType = value; }
        }

        internal override void accept(DeclarationVisitor visitor)
        {
            visitor.visitFunctionDeclaration(this);
        }
    }

}