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
    using Constant = VHDL.Object.Constant;
    /// <summary>
    /// Constant Declaration
    /// </summary>
    [Serializable]
    public class ConstantDeclaration : ObjectDeclaration<Constant>, IBlockDeclarativeItem, IEntityDeclarativeItem, IPackageBodyDeclarativeItem, IPackageDeclarativeItem, IProcessDeclarativeItem, ISubprogramDeclarativeItem
    {
        /// <summary>
        /// Creates a new constant declaration.
        /// </summary>
        /// <param name="constants">the declared constants</param>
        public ConstantDeclaration(params Constant[] constants)
            : this(new List<Constant>(constants))
        {
        }

        /// <summary>
        /// Creates a new constant declaration.
        /// </summary>
        /// <param name="constants">the declared constants</param>
        public ConstantDeclaration(List<Constant> constants)
            : base(constants)
        {
        }

        internal override void accept(DeclarationVisitor visitor)
        {
            visitor.visitConstantDeclaration(this);
        }
    }

}