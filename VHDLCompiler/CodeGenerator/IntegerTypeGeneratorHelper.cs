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
using VHDLRuntime.Values;
using VHDLRuntime.Range;
using VHDLRuntime.Types;
using System.Reflection;
using VHDLCompiler.CodeTemplates.Types;

namespace VHDLCompiler.CodeGenerator
{
    /// <summary>
    /// Этот класс используется для генерации класса, который описывает тип данных, базирующийся на integer
    /// </summary>
    public static class IntegerTypeGeneratorHelper
    {
        public static string GenerateIntegerType(VHDLCompilerInterface compiler, VHDL.type.IntegerType typeDeclaration)
        {
            string typeName = typeDeclaration.Identifier;
            Int64 rangeLeft = ((typeDeclaration.Range as VHDL.Range).From as VHDL.literal.IntegerLiteral).IntegerValue;
            Int64 rangeRight = ((typeDeclaration.Range as VHDL.Range).To as VHDL.literal.IntegerLiteral).IntegerValue;
            RangeDirection rangeRirection = ((typeDeclaration.Range as VHDL.Range).Direction == VHDL.Range.RangeDirection.TO) ? RangeDirection.To : RangeDirection.DownTo;

            IntegerTypeTemplate template = new IntegerTypeTemplate(typeName, rangeLeft, rangeRight, rangeRirection);
            string text = template.TransformText();

            compiler.TypeRangeDictionary.AddItem(typeDeclaration, "IntegerRange", template.RangeLeft, template.RangeRight, template.Direction);

            return text;
        }
    }
}
