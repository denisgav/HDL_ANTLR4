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
    public class SignalDeclarationObserver : VHDLObserverBase
    {
        private VHDL.Object.Signal mSignal;
        public VHDL.Object.Signal Signal
        {
            get { return mSignal; }
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


        public SignalDeclarationObserver(VHDL.Object.Signal mSignal, Logger logger)
        {
            this.logger = logger;
            this.mSignal = mSignal;
        }

        public override void Observe(VHDLCompilerInterface compiler)
        {
            string typeName = compiler.TypeDictionary[mSignal.Type];
            string variableName = mSignal.Identifier;
            SignalDeclarationTemplate template = null;
            if (mSignal.DefaultValue != null)
            {
                string defValue = VHDLOperandGenerator.GetOperand(mSignal.DefaultValue, compiler);
                template = new SignalDeclarationTemplate(typeName, variableName, defValue);
            }
            else
            {
                template = new SignalDeclarationTemplate(typeName, variableName);
            }

            compiler.ObjectDictionary.AddItem(mSignal, variableName);

            declarationText = template.TransformText();
        }
    }
}