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

namespace VHDLCompiler.CodeTemplates.Statements
{
    public partial class ForTemplate
    {
        private List<string> parameters;
        public List<string> Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }        

        private string rangeType;
        public string RangeType
        {
            get { return rangeType; }
            set { rangeType = value; }
        }

        private string indexName;
        public string IndexName
        {
            get { return indexName; }
            set { indexName = value; }
        }

        private List<string> statements;
        public List<string> Statements
        {
            get { return statements; }
            set { statements = value; }
        }


        public ForTemplate(string rangeType, List<string> parameters, string indexName, List<string> statements)
        {
            this.parameters = parameters;
            this.rangeType = rangeType;
            this.indexName = indexName;
            this.statements = statements;
        }

        public ForTemplate(string rangeType, string indexName, List<string> statements)
            : this(rangeType, new List<string>(), indexName, statements)
        {
        }
    }
}
