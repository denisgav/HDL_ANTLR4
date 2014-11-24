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
using LibraryUnit = VHDL.libraryunit.LibraryUnit;
using VhdlCollections = VHDL.util.VhdlCollections;
using VHDL.util;
using System;
using VHDL.libraryunit;

namespace VHDL
{
    /// <summary>
    /// VHDL file.
    /// </summary>
    [Serializable]
    public class VhdlFile : VhdlElement, IDeclarativeRegion
    {
        private readonly IResolvableList<LibraryUnit> elements;
        private readonly IScope scope;

        /// <summary>
        /// Путь к текущему файлу
        /// </summary>
        private string filePath;
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        public VhdlFile()
        {
            elements = VhdlCollections.CreateNamedEntityList<LibraryUnit>(this);
            scope = Scopes.createScope(this, elements);
        }

        public VhdlFile(string filePath)
            : this()
        {
            this.filePath = filePath;
        }

        /// <summary>
        /// Returns the list of elements in this VHDL file.
        /// </summary>
        public virtual IResolvableList<LibraryUnit> Elements
        {
            get { return elements; }
        }

        public virtual IScope Scope
        {
            get { return scope; }
        }
    }
}