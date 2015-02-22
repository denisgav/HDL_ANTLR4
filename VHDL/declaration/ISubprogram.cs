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

namespace VHDL.declaration
{

    using NamedEntity = VHDL.INamedEntity;
    using VhdlObject = VHDL.Object.VhdlObject;
    using VhdlObjectProvider = VHDL.Object.IVhdlObjectProvider;
    using VHDL.Object;
    using VHDL.util;

    /// <summary>
    /// Subprogram.
    /// </summary>
    public interface ISubprogram : NamedEntity
    {

        /// <summary>
        /// Returns/Sets the identifier of this attribtue.
        /// </summary>
        new string Identifier { get; set; }

        /// <summary>
        /// Returns the parameters of this subprogram.
        /// </summary>
        IResolvableList<VhdlObjectProvider> Parameters { get; }
        // TODO: possibility to get objects directly by idx
    }
}