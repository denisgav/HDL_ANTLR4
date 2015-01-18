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

namespace VHDLCompiler.CodeTemplates
{
    public partial class ProcessTemplate
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private List<string> declarationList;
        public List<string> DeclarationList
        {
            get { return declarationList; }
            set { declarationList = value; }
        }

        private List<string> statementList;
        public List<string> StatementList
        {
            get { return statementList; }
            set { statementList = value; }
        }

        public ProcessTemplate(string name, List<string> declarationList, List<string> statementList)
        {
            this.name = name;
            this.declarationList = declarationList;
            this.statementList = statementList;
        }

        public ProcessTemplate(string name)
            : this(name, new List<string>(), new List<string>())
        { }

    }
}
