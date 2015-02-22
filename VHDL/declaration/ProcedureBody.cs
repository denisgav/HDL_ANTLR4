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

namespace VHDL.declaration
{
    using VhdlObjectProvider = VHDL.Object.IVhdlObjectProvider;

    /// <summary>
    /// Procedure body.
    /// </summary>
    [Serializable]
    public class ProcedureBody : SubprogramBody, IProcedure
    {
        /// <summary>
        /// Creates a procedure body.
        /// </summary>
        /// <param name="identifier">the procedure body identifier</param>
        /// <param name="parameters">the parameters</param>
        public ProcedureBody(string identifier, List<VhdlObjectProvider> parameters)
            : base(identifier, parameters)
        {
        }

        /// <summary>
        /// Creates a procedure body.
        /// </summary>
        /// <param name="identifier">the procedure body identifier</param>
        /// <param name="parameters">the parameters</param>
        public ProcedureBody(string identifier, params VhdlObjectProvider[] parameters)
            : base(identifier, parameters)
        {
        }

        /// <summary>
        /// Creates a procedure body based on a procedure declaration.
        /// </summary>
        /// <param name="declaration">the procedure declaration</param>
        public ProcedureBody(ProcedureDeclaration declaration)
            : base(declaration)
        {
        }

        internal override void accept(DeclarationVisitor visitor)
        {
            visitor.visitProcedureBody(this);
        }
    }
}