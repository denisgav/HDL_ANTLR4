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
using VHDL.declaration;

namespace VHDLCompiler.VHDLObserver
{
    public class ProcessDeclarationObserver : VHDLObserverBase
    {
        private readonly IProcessDeclarativeItem item;
        public IProcessDeclarativeItem Item
        {
            get { return item; }
        }

        private string declarationText;
        public string DeclarationText
        {
            get { return declarationText; }
            set { declarationText = value; }
        }
        

        private Logger logger;
        public Logger Logger
        {
            get { return logger; }
            set { logger = value; }
        }

        public ProcessDeclarationObserver(IProcessDeclarativeItem item, Logger logger)
        {
            this.logger = logger;
            this.item = item;
        }

        public override void Observe(VHDLCompilerInterface compiler)
        {
            ObserveProcessDeclarativeItem(compiler, item);
        }


        public void ObserveProcessDeclarativeItem(VHDLCompilerInterface compiler, IProcessDeclarativeItem item)
        {
            if (item is VHDL.type.Type)
            {
                ObserveTypeDeclaration(compiler, item as VHDL.type.Type);
            }

            if (item is VariableDeclaration)
            {
                ObserveVariableDeclaration(compiler, item as VariableDeclaration);
            }

            if (item is SignalDeclaration)
            {
                ObserveSignalDeclaration(compiler, item as SignalDeclaration);
            }
        }

        private void ObserveTypeDeclaration(VHDLCompilerInterface compiler, VHDL.type.Type item)
        {
            TypeObserver typeObserver = new TypeObserver(item, logger);
            typeObserver.Observe(compiler);
        }

        private void ObserveVariableDeclaration(VHDLCompilerInterface compiler, VariableDeclaration item)
        {
            foreach(VHDL.Object.Variable v in item.Objects)
            {
                VariableDeclarationObserver varObserver = new VariableDeclarationObserver(v, logger);
                varObserver.Observe(compiler);
                declarationText += varObserver.DeclarationText;
            }
        }

        private void ObserveSignalDeclaration(VHDLCompilerInterface compiler, SignalDeclaration item)
        {
            foreach (VHDL.Object.Signal v in item.Objects)
            {
                SignalDeclarationObserver varObserver = new SignalDeclarationObserver(v, logger);
                varObserver.Observe(compiler);
                declarationText += varObserver.DeclarationText;
            }
        }
    }
}
