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
    using FileObject = VHDL.Object.FileObject;

    /// <summary>
    /// File declaration.
    /// </summary>
    [Serializable]
    public class FileDeclaration : ObjectDeclaration<FileObject>, IBlockDeclarativeItem, IEntityDeclarativeItem, IPackageBodyDeclarativeItem, IPackageDeclarativeItem, IProcessDeclarativeItem, ISubprogramDeclarativeItem
    {
        /// <summary>
        /// Creates a new file declaration.
        /// </summary>
        /// <param name="files">the declared files</param>
        public FileDeclaration(params FileObject[] files)
            : this(new List<FileObject>(files))
        {
        }

        /// <summary>
        /// Creates a new file declaration.
        /// </summary>
        /// <param name="files">the declared files</param>
        public FileDeclaration(List<FileObject> files)
            : base(files)
        {
        }

        internal override void accept(DeclarationVisitor visitor)
        {
            visitor.visitFileDeclaration(this);
        }
    }

}