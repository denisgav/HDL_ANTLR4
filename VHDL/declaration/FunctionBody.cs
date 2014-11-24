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
using VHDL.util;
using System;


namespace VHDL.declaration
{
    using VhdlObjectProvider = VHDL.Object.IVhdlObjectProvider;
    using SubtypeIndication = VHDL.type.ISubtypeIndication;
    
    /// <summary>
    /// Function body.
    /// </summary>
    [Serializable]
    public class FunctionBody : SubprogramBody, IFunction
    {
        private SubtypeIndication returnType;
        private bool impure;

        /// <summary>
        /// Creates a function body.
        /// </summary>
        /// <param name="identifier">the identifier</param>
        /// <param name="returnType">the return type</param>
        /// <param name="parameters">the parameters</param>
        public FunctionBody(string identifier, SubtypeIndication returnType, List<VhdlObjectProvider> parameters)
            : base(identifier, parameters)
        {
            this.returnType = returnType;
        }

        /// <summary>
        /// Creates a function body.
        /// </summary>
        /// <param name="identifier">the identifier</param>
        /// <param name="returnType">the return type</param>
        /// <param name="parameters">the parameters</param>
        public FunctionBody(string identifier, SubtypeIndication returnType, params VhdlObjectProvider[] parameters)
            : base(identifier, parameters)
        {
            this.returnType = returnType;
        }

        //TODO: link function body to declaration
        /// <summary>
        /// Creates a function body based on a function declaration.
        /// </summary>
        /// <param name="declaration">the base function declaration</param>
        public FunctionBody(FunctionDeclaration declaration)
            : base(declaration)
        {
            this.returnType = declaration.ReturnType;
        }

        internal override void accept(DeclarationVisitor visitor)
        {
            visitor.visitFunctionBody(this);
        }

        #region Function Members

        public bool Impure
        {
            get { return impure; }
            set { impure = value; }
        }

        public SubtypeIndication ReturnType
        {
            get { return returnType; }
            set { returnType = value; }
        }
        #endregion
    }
}