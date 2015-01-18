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
using VHDL.literal;
using VHDLRuntime.Values;
using System.Collections;
using VHDLCompiler.CodeTemplates.Types;

namespace VHDLCompiler.CodeGenerator
{
    
    public static class EnumTypeGeneratorHelper
    {
        public static string GenerateEnumerationType(VHDLCompilerInterface compiler, VHDL.type.EnumerationType typeDeclaration)
        {
            string typeName = typeDeclaration.Identifier;
            string enumTypeName = string.Format("{0}_Enum", typeDeclaration.Identifier);

            List<EnumBaseTypeInfo> parameters = FormEnumDictionary(typeDeclaration);
            EnumTypeTemplate template = new EnumTypeTemplate(typeName, parameters);
            string text = template.TransformText();

            foreach (EnumBaseTypeInfo i in parameters)
            {
                compiler.LiteralDictionary.AddItem(i, string.Format("{0}.{1}", enumTypeName, i.FieldName));
            }
            compiler.TypeRangeDictionary.AddItem(typeDeclaration, string.Format("EnumRange<{0}>", enumTypeName), string.Format("{0}.{1}", enumTypeName, parameters[0].FieldName), string.Format("{0}.{1}", enumTypeName, parameters[parameters.Count - 1].FieldName), "RangeDirection.To");

            return text;
        }


        public static string GetEnumerationLiteralString(EnumerationLiteral l)
        {
            if (l is VHDL.type.EnumerationType.IdentifierEnumerationLiteral)
                return (l as VHDL.type.EnumerationType.IdentifierEnumerationLiteral).getLiteral();

            if (l is VHDL.type.EnumerationType.CharacterEnumerationLiteral)
                return string.Format("item_{0}", (l as VHDL.type.EnumerationType.CharacterEnumerationLiteral).getLiteral());

            return l.ToString();
        }

        public static string GetEnumerationLiteralValue(EnumerationLiteral l)
        {
            if (l is VHDL.type.EnumerationType.IdentifierEnumerationLiteral)
                return (l as VHDL.type.EnumerationType.IdentifierEnumerationLiteral).getLiteral();

            if (l is VHDL.type.EnumerationType.CharacterEnumerationLiteral)
                return string.Format("{0}", (l as VHDL.type.EnumerationType.CharacterEnumerationLiteral).getLiteral());

            return l.ToString();
        }

        public static List<EnumBaseTypeInfo> FormEnumDictionary(VHDL.type.EnumerationType typeDeclaration)
        {
            List<EnumBaseTypeInfo> dict = new List<EnumBaseTypeInfo>();
            int counter = 0;
            foreach (var literal in typeDeclaration.Literals)
            {
                dict.Add(new EnumBaseTypeInfo() { Key = counter, FieldName = GetEnumerationLiteralString(literal), Value = GetEnumerationLiteralValue(literal), Literal = literal});
                counter++;
            }

            return dict;
        }        
    }
}
