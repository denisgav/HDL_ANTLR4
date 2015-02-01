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
using Antlr4.Runtime;

namespace VHDL.ParseError
{
    public class vhdlSemanticException : Exception
    {
        private ParserRuleContext context;

        public ParserRuleContext Context
        {
            get { return context; }
        }

        private string fileName;

        public string FileName
        {
            get { return fileName; }
        }

        public vhdlSemanticException(ParserRuleContext context, string fileName, string msg)
            :base(msg)
        {
            this.context = context;
            this.fileName = fileName;
        }

        public string GetConsoleMessageTest()
        {
            return string.Format("Semantic error: {0} {1}:{2} {3} \n {4}", fileName, context.Start.Line, context.Start.Column, Message, context.GetText());
        }
    }
}
