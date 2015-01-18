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
using VHDL.concurrent;
using VHDL.parser;
using VHDL.declaration;
using VHDL.statement;
using VHDLCompiler.CodeTemplates;

namespace VHDLCompiler.VHDLObserver
{
    public class ProcessObserver : VHDLObserverBase
    {
        private readonly ProcessStatement process;
        public ProcessStatement Process
        {
            get { return process; }
        }

        private Logger logger;
        public Logger Logger
        {
            get { return logger; }
            set { logger = value; }
        }

        private List<string> statements;
        public List<string> Statements
        {
            get { return statements; }
            set { statements = value; }
        }

        private string methodName;
        public string MethodName
        {
            get { return methodName; }
            set { methodName = value; }
        }

        private string method;
        public string Method
        {
            get { return method; }
            set { method = value; }
        }

        private List<string> declarations;
        public List<string> Declarations
        {
            get { return declarations; }
            set { declarations = value; }
        }
        

        public ProcessObserver(ProcessStatement process, Logger logger)
        {
            this.logger = logger;
            this.process = process;
            declarations = new List<string>();
            this.statements = new List<string>();
        }

        public override void Observe(VHDLCompilerInterface compiler)
        {
            ObserveDeclaration(compiler, process.Declarations);
            ObserveSequentialStatements(compiler, process.Statements);

            ProcessTemplate template = new ProcessTemplate(methodName, declarations, statements);
            method = template.TransformText();
        }

        public void ObserveDeclaration(VHDLCompilerInterface compiler, IList<IProcessDeclarativeItem> items)
        {
            foreach (IProcessDeclarativeItem i in items)
            {
                ProcessDeclarationObserver blockObserver = new ProcessDeclarationObserver(i, logger);
                blockObserver.Observe(compiler);
                if (string.IsNullOrEmpty(blockObserver.DeclarationText) == false)
                    declarations.Add(blockObserver.DeclarationText);
            }
        }

        public void ObserveSequentialStatements(VHDLCompilerInterface compiler, IList<SequentialStatement> statements)
        {
            methodName = process.Label;
            foreach (SequentialStatement statement in statements)
            {
                SequentialStatementObserver stObserver = new SequentialStatementObserver(statement, logger);
                stObserver.Observe(compiler);
                if (string.IsNullOrEmpty(stObserver.Code) == false)
                    this.statements.Add(stObserver.Code);
            }
        }
    }
}
