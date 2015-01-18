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
using VHDL.parser;
using VHDL.concurrent;
using VHDLCompiler.CodeTemplates;

namespace VHDLCompiler.VHDLObserver
{
    public class ConcurrentStatementsObserver : VHDLObserverBase
    {
        private readonly ConcurrentStatement statement;
        public ConcurrentStatement Statement
        {
            get { return statement; }
        }

        private Logger logger;
        public Logger Logger
        {
            get { return logger; }
            set { logger = value; }
        }

        private ProcesRoutineInfo method;
        public ProcesRoutineInfo Method
        {
            get { return method; }
            set { method = value; }
        }

        public ConcurrentStatementsObserver(ConcurrentStatement statement, Logger logger)
        {
            this.logger = logger;
            this.statement = statement;
        }

        public override void Observe(VHDLCompilerInterface compiler)
        {
            if (statement is ProcessStatement)
            {
                ObserveProcess(compiler, statement as ProcessStatement);
            }
        }

        public void ObserveProcess(VHDLCompilerInterface compiler, ProcessStatement process)
        {
            ProcessObserver po = new ProcessObserver(process, logger);
            po.Observe(compiler);
            method = new ProcesRoutineInfo(po.MethodName, po.Method);
        }
    }
}
