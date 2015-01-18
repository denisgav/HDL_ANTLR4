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
using VHDLCompiler.CodeGenerator;

namespace VHDLCompiler.CodeTemplates.Types
{
    public partial class EnumTypeTemplate 
    {
        private List<EnumBaseTypeInfo> dict;
        public List<EnumBaseTypeInfo> Dictionary
        {
            get { return dict; }
            set { dict = value; }
        }
        
        private string nameSpaceName;
        public string NameSpaceName
        {
            get { return nameSpaceName; }
            set { nameSpaceName = value; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public EnumTypeTemplate(string nameSpaceName, string name, List<EnumBaseTypeInfo> dict)
        {
            this.nameSpaceName = nameSpaceName;
            this.name = name;
            this.dict = dict;
        }

        public EnumTypeTemplate(string name, List<EnumBaseTypeInfo> dict)
            : this("Work", name, dict)
        {
        }
    }
}
