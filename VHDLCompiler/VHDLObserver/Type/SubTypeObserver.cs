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
using VHDL.declaration;
using VHDL.parser;
using VHDL.type;
using VHDLCompiler.CodeGenerator;

namespace VHDLCompiler.VHDLObserver
{
    public class SubTypeObserver: VHDLObserverBase
    {
        private Subtype type;
        public Subtype Type
        {
            get { return type; }
            set { type = value; }
        }

        private Logger logger;
        public Logger Logger
        {
            get { return logger; }
            set { logger = value; }
        }

        public SubTypeObserver(Subtype type, Logger logger)
        {
            this.logger = logger;
            this.type = type;
        }


        public override void Observe(VHDLCompilerInterface compiler)
        {
            compiler.TypeDictionary.AddItem(type, type.Identifier);

            if (BuiltInTypesDictionary.ContainsBuiltInSubType(type))
                return;

            ISubtypeIndication si = type.SubtypeIndication;
            string typeName = type.Identifier;
            if (si is RangeSubtypeIndication)
            {
                RangeSubtypeIndication rangeSI = si as RangeSubtypeIndication;
                ISubtypeIndication baseSI = rangeSI.BaseType;
                if (baseSI is VHDL.type.IntegerType)
                {
                    ObserveIntegerSubType(compiler, rangeSI, typeName);
                }

                if (baseSI is VHDL.type.RealType)
                {
                    ObserveRealSubType(compiler, rangeSI, typeName);
                }
            }
        }

        private void ObserveIntegerSubType(VHDLCompilerInterface compiler, RangeSubtypeIndication rangeSI, string typeName)
        {
            string code = IntegerSubTypeGeneratorHelper.GenerateIntegerSubType(compiler, typeName, rangeSI);
            compiler.SaveCode(code, typeName);
        }

        private void ObserveRealSubType(VHDLCompilerInterface compiler, RangeSubtypeIndication rangeSI, string typeName)
        {
            string code = FloatingPointSubTypeGeneratorHelper.GenerateFloatingPointSubType(compiler, typeName, rangeSI);
            compiler.SaveCode(code, typeName);
        }
    }
}
