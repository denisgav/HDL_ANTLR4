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
using VHDLCompiler.CodeGenerator;
using VHDLCompiler.CodeTemplates.Declarations;

namespace VHDLCompiler.VHDLObserver
{
    public class VariableDeclarationObserver : VHDLObserverBase
    {
        private VHDL.Object.Variable mVariable;
        public VHDL.Object.Variable Variable
        {
            get { return mVariable; }
        }

        private Logger logger;
        public Logger Logger
        {
            get { return logger; }
            set { logger = value; }
        }

        private string declarationText;
        public string DeclarationText
        {
            get { return declarationText; }
            set { declarationText = value; }
        }


        public VariableDeclarationObserver(VHDL.Object.Variable mVariable, Logger logger)
        {
            this.logger = logger;
            this.mVariable = mVariable;
        }

        public override void Observe(VHDLCompilerInterface compiler)
        {
            string typeName = compiler.TypeDictionary[mVariable.Type];
            string variableName = mVariable.Identifier;
            VariableDeclarationTemplate template = null;
            if (mVariable.DefaultValue != null)
            {
                string defValue = VHDLOperandGenerator.GetOperand(mVariable.DefaultValue, compiler);
                template = new VariableDeclarationTemplate(typeName, variableName, defValue);
            }
            else
            {
                template = new VariableDeclarationTemplate(typeName, variableName);
            }

            compiler.ObjectDictionary.AddItem(mVariable, variableName);

            declarationText = template.TransformText();
        }
    }
}
