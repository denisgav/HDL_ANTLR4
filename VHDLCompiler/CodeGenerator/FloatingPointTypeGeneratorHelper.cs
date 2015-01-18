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
using VHDLRuntime.Range;
using VHDLRuntime.Values;
using VHDLRuntime.Types;
using VHDLCompiler.CodeTemplates.Types;

namespace VHDLCompiler.CodeGenerator
{
    public static class FloatingPointTypeGeneratorHelper
    {
        public static string GenerateFloatingPointType(VHDL.type.RealType typeDeclaration)
        {
            string typeName = typeDeclaration.Identifier;
            double rangeLeft = ((typeDeclaration.Range as VHDL.Range).From as VHDL.literal.RealLiteral).RealValue;
            double rangeRight = ((typeDeclaration.Range as VHDL.Range).To as VHDL.literal.RealLiteral).RealValue;
            RangeDirection rangeRirection = ((typeDeclaration.Range as VHDL.Range).Direction == VHDL.Range.RangeDirection.TO) ? RangeDirection.To : RangeDirection.DownTo;

            return GenerateFloatingPointType(typeName, rangeLeft, rangeRight, rangeRirection);
        }

        public static string GenerateFloatingPointType(string typeName, double rangeLeft, double rangeRight, RangeDirection rangeRirection)
        {
            FloatPointTypeTemplate template = new FloatPointTypeTemplate(typeName, rangeLeft, rangeRight, rangeRirection);
            string text = template.TransformText();

            return text;
        }        
    }
}
