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
using System.Linq;
using System.Text;

namespace VHDLParser
{
    /// <summary>
    /// Информация об архитектуре
    /// </summary>
    public class ArchitectureInfo
    {
        /// <summary>
        /// Имя архитектуры
        /// </summary>
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Имя Entity
        /// </summary>
        private string entityName;
        public string EntityName
        {
            get { return entityName; }
            set { entityName = value; }
        }

        public ArchitectureInfo() { }

        public ArchitectureInfo(string name, string entityName)
        {
            this.name = name;
            this.entityName = entityName;
        }
    }
}
