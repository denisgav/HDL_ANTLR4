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
using VHDLRuntime.Range;

namespace VHDLCompiler.VHDLObserver
{
    public class TypeObserver : VHDLObserverBase
    {
        private VHDL.type.Type type;
        public VHDL.type.Type Type
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

        public TypeObserver(VHDL.type.Type type, Logger logger)
        {
            this.logger = logger;
            this.type = type;
        }

        public override void Observe(VHDLCompilerInterface compiler)
        {
            compiler.TypeDictionary.AddItem(type, type.Identifier);

            if (BuiltInTypesDictionary.ContainsBuiltInType(type))
            {
                return;
            }

            if (type is VHDL.type.IntegerType)
            {
                VHDL.type.IntegerType intType = type as VHDL.type.IntegerType;
                ObserveIntegerType(compiler, intType);
                return;
            }

            if (type is VHDL.type.RealType)
            {
                VHDL.type.RealType realType = type as VHDL.type.RealType;
                ObserveFloatingPointType(compiler, realType);
                return;
            }

            if (type is VHDL.type.EnumerationType)
            {
                VHDL.type.EnumerationType enumType = type as VHDL.type.EnumerationType;
                ObserveEnumerationType(compiler, enumType);
                return;
            }

            if (type is VHDL.type.PhysicalType)
            {
                VHDL.type.PhysicalType physType = type as VHDL.type.PhysicalType;
                ObserveEnumerationType(compiler, physType);
                return;
            }
        }

        public void ObserveIntegerType(VHDLCompilerInterface compiler, VHDL.type.IntegerType intType)
        {
            string code = IntegerTypeGeneratorHelper.GenerateIntegerType(compiler, intType);
            compiler.SaveCode(code, intType.Identifier);
        }

        public void ObserveFloatingPointType(VHDLCompilerInterface compiler, VHDL.type.RealType realType)
        {
            string code = FloatingPointTypeGeneratorHelper.GenerateFloatingPointType(realType);
            compiler.SaveCode(code, realType.Identifier);
        }

        public void ObserveEnumerationType(VHDLCompilerInterface compiler, VHDL.type.EnumerationType enumType)
        {
            string code = EnumTypeGeneratorHelper.GenerateEnumerationType(compiler, enumType);
            compiler.SaveCode(code, enumType.Identifier);
        }

        public void ObserveEnumerationType(VHDLCompilerInterface compiler, VHDL.type.PhysicalType physType)
        {
            string code = PhysicalTypeGeneratorHelper.GeneratePhysicalType(compiler, physType);
            compiler.SaveCode(code, physType.Identifier);
        }
    }
}
