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
    public partial class IfTemplate
    {
        private string condition;
        public string Condition
        {
            get { return condition; }
            set { condition = value; }
        }

        private List<string> statements;
        public List<string> Statements
        {
            get { return statements; }
            set { statements = value; }
        }

        private List<string> elseStatements;
        public List<string> ElseStatements
        {
            get { return elseStatements; }
            set { elseStatements = value; }
        }

        private List<IfTemplateElsifStatement> elsifParts;
        public List<IfTemplateElsifStatement> ElsifParts
        {
            get { return elsifParts; }
            set { elsifParts = value; }
        }

        public IfTemplate(string condition, List<string> statements, List<IfTemplateElsifStatement> elsifParts, List<string> elseStatements)
        {
            this.condition = condition;
            this.statements = statements;
            this.elsifParts = elsifParts;
            this.elseStatements = elseStatements;
        }

        public IfTemplate(string condition, List<string> statements, List<string> elseStatements)
            : this(condition, statements, new List<IfTemplateElsifStatement>(), elseStatements)
        { }

        public IfTemplate(string condition, List<string> statements)
            : this(condition, statements, new List<IfTemplateElsifStatement>(), new List<string>())
        { }

        public IfTemplate(string condition, string statements)
            : this(condition, new List<string>() { statements }, new List<IfTemplateElsifStatement>(), new List<string>())
        { }
    }

    public class IfTemplateElsifStatement
    {
        private string condition;
        public string Condition
        {
            get { return condition; }
            set { condition = value; }
        }

        private List<string> statements;
        public List<string> Statements
        {
            get { return statements; }
            set { statements = value; }
        }

        public IfTemplateElsifStatement(string condition)
            : this(condition, new List<string>())
        {
        }

        public IfTemplateElsifStatement(string condition, string statement)
            : this(condition, new List<string>() { statement })
        {
        }

        public IfTemplateElsifStatement(string condition, List<string> statements)
        {
            this.condition = condition;
            this.statements = statements;
        }
    }
}
