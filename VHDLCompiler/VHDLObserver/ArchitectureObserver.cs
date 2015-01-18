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
using VHDLRuntime;
using VHDLRuntime.Range;
using VHDLCompiler.CodeGenerator;
using VHDL.libraryunit;
using VHDL.declaration;
using VHDL.concurrent;
using VHDLCompiler.CodeTemplates;

namespace VHDLCompiler.VHDLObserver
{
    public class ArchitectureObserver : VHDLObserverBase
    {
        /// <summary>
        /// Моделируемая архитектура
        /// </summary>
        private readonly VHDL.libraryunit.Architecture architecture;
        public VHDL.libraryunit.Architecture Architecture
        {
            get { return architecture; }
        }

        private VHDL.parser.Logger logger;
        public VHDL.parser.Logger Logger
        {
            get { return logger; }
            set { logger = value; }
        }

        private List<ConcurrentStatementsObserver> statementObservers;
        public List<ConcurrentStatementsObserver> StatementObservers
        {
            get { return statementObservers; }
            set { statementObservers = value; }
        }

        private List<string> declarations;
        public List<string> Declarations
        {
            get { return declarations; }
            set { declarations = value; }
        }

        private List<string> signalNameList;
        public List<string> SignalNameList
        {
            get { return signalNameList; }
            set { signalNameList = value; }
        }

        private List<ProcesRoutineInfo> processList;
        public List<ProcesRoutineInfo> ProcessList
        {
            get { return processList; }
            set { processList = value; }
        }

        public ArchitectureObserver(VHDL.libraryunit.Architecture architecture, VHDL.parser.Logger logger)
        {
            this.logger = logger;
            this.architecture = architecture;
            statementObservers = new List<ConcurrentStatementsObserver>();
            declarations = new List<string>();
            signalNameList = new List<string>();
            processList = new List<ProcesRoutineInfo>();
        }

        private List<VHDL.INamedEntity> GetUseClauseList(VHDL.libraryunit.Architecture architecture)
        {
            List<VHDL.INamedEntity> res = new List<VHDL.INamedEntity>();
            res.Add(VHDL.builtin.Standard.PACKAGE);
            foreach (var libraryUnit in architecture.ContextItems)
            {
                if(libraryUnit is VHDL.libraryunit.UseClause)
                {
                    res.AddRange((libraryUnit as VHDL.libraryunit.UseClause).LinkedElements);
                }
            }
            foreach (var libraryUnit in architecture.Entity.ContextItems)
            {
                if (libraryUnit is VHDL.libraryunit.UseClause)
                {
                    res.AddRange((libraryUnit as VHDL.libraryunit.UseClause).LinkedElements);
                }
            }
            return res;
        }

        public override void Observe(VHDLCompilerInterface compiler)
        {
            List<VHDL.INamedEntity> architectureReferences = GetUseClauseList(architecture);

            foreach (VHDL.INamedEntity item in architectureReferences)
            {
                ObserveNamedEntity(compiler, item);
            }

            ObserveDeclaration(compiler, architecture.Declarations);
            ObserveConcurrentStatements(compiler, architecture.Statements);

            ArchitectureTemplate code = new ArchitectureTemplate(architecture.Identifier, "Work", declarations, signalNameList, processList);
            string text = code.TransformText();
            compiler.SaveCode(text, architecture.Identifier);
        }

        public void ObserveDeclaration(VHDLCompilerInterface compiler, IList<IBlockDeclarativeItem> items)
        {
            foreach (IBlockDeclarativeItem i in items)
            {
                BlockDeclarationObserver blockObserver = new BlockDeclarationObserver(i, logger);
                blockObserver.Observe(compiler);
                if (string.IsNullOrEmpty(blockObserver.DeclarationText) == false)
                    declarations.Add(blockObserver.DeclarationText);
                if (blockObserver.SignalNames != null)
                {
                    signalNameList.AddRange(blockObserver.SignalNames);
                }
            }
        }

        private void ObserveNamedEntity(VHDLCompilerInterface compiler, VHDL.INamedEntity item)
        {
            if (item is PackageDeclaration)
            {
                PackageDeclaration packageDeclaration = item as PackageDeclaration;

                PackageDeclarationObserver observer = new PackageDeclarationObserver(packageDeclaration, logger);
                observer.Observe(compiler);
            }
        }

        private void ObserveConcurrentStatements(VHDLCompilerInterface compiler, IList<ConcurrentStatement> statements)
        {
            foreach (ConcurrentStatement st in statements)
            {
                ConcurrentStatementsObserver stObserver = new ConcurrentStatementsObserver(st, logger);
                stObserver.Observe(compiler);
                statementObservers.Add(stObserver);
                processList.Add(stObserver.Method);
            }
        }
    }
}

