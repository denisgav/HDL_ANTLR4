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
    /// <summary>
    /// Procedure declaration.
    /// </summary>
    [Serializable]
	public class ProcedureDeclaration : SubprogramDeclaration
	{
        /// <summary>
        /// Creates a procedure declaration.
        /// </summary>
        /// <param name="identifier">the procedure identifier</param>
        /// <param name="parameters">the parameters</param>
		public ProcedureDeclaration(string identifier, List<VhdlObjectProvider> parameters) : base(identifier, parameters)
		{
		}

        /// <summary>
        /// Creates a procedure declaration.
        /// </summary>
        /// <param name="identifier">the procedure identifier</param>
        /// <param name="parameters">the parameters</param>
		public ProcedureDeclaration(string identifier, params VhdlObjectProvider[] parameters) : base(identifier, parameters)
		{
		}

		internal override void accept(DeclarationVisitor visitor)
		{
			visitor.visitProcedureDeclaration(this);
		}
	}
}