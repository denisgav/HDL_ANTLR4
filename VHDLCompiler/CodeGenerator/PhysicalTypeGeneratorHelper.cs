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
using VHDLRuntime;
using VHDLRuntime.Types;
using VHDLCompiler.CodeTemplates.Types;

namespace VHDLCompiler.CodeGenerator
{
    
    public static class PhysicalTypeGeneratorHelper
    {
        public static string GeneratePhysicalType(VHDLCompilerInterface compiler, VHDL.type.PhysicalType typeDeclaration)
        {
            Int64 rangeLeft = ((typeDeclaration.Range as VHDL.Range).From as VHDL.literal.IntegerLiteral).IntegerValue;
            Int64 rangeRight = ((typeDeclaration.Range as VHDL.Range).To as VHDL.literal.IntegerLiteral).IntegerValue;
            RangeDirection direction = ((typeDeclaration.Range as VHDL.Range).Direction == VHDL.Range.RangeDirection.TO) ? RangeDirection.To : RangeDirection.DownTo;

            List<PhysicalTypeBaseInfo> dict = FormPhysicalValueDictionary(typeDeclaration);

            PhysicalTypeTemplate template = new PhysicalTypeTemplate(typeDeclaration.Identifier, dict, rangeLeft, rangeRight, direction);
            string text = template.TransformText();

            return text;
        }

        public static PhysicalTypeBaseInfo GetPhysicalUnit(VHDL.type.PhysicalType.Unit literal)
        {
            return new PhysicalTypeBaseInfo()
            {
                IsBaseUnit = false,
                BaseUnitName = literal.BaseUnit,
                UnitName = literal.Identifier,
                Multiplier = literal.Factor
            };
        }

        public static PhysicalTypeBaseInfo GetPhysicalUnit(string baseUnitName)
        {
            return new PhysicalTypeBaseInfo()
            {
                IsBaseUnit = true,
                BaseUnitName = string.Empty,
                UnitName = baseUnitName,
                Multiplier = 1
            };
        }

        public static List<PhysicalTypeBaseInfo> FormPhysicalValueDictionary(VHDL.type.PhysicalType typeDeclaration)
        {
            List<PhysicalTypeBaseInfo> dict = new List<PhysicalTypeBaseInfo>();
            int counter = 0;

            foreach (var literal in typeDeclaration.Units)
            {
                dict.Add(GetPhysicalUnit(literal));
                counter++;
            }
            return dict;
        }

        public static PhysicalTypeBaseInfo GetPrimaryUnit(List<PhysicalTypeBaseInfo> dict)
        {
            PhysicalTypeBaseInfo res = null;

            foreach (PhysicalTypeBaseInfo u in dict)
            {
                if (u.IsBaseUnit == true)
                {
                    if (res == null)
                    {
                        res = u;
                    }
                    else
                    {
                        throw new Exception("Physical value has more than one basic unit");
                    }
                }
            }

            if (res == null)
                throw new Exception("Physical value has no basic unit");

            return res;
        }

        public static List<PhysicalTypeBaseInfo> GetNotPrimaryUnits(List<PhysicalTypeBaseInfo> dict)
        {
            List<PhysicalTypeBaseInfo> res = new List<PhysicalTypeBaseInfo>();
            foreach (PhysicalTypeBaseInfo u in dict)
            {
                if (u.IsBaseUnit == true)
                    continue;
                else
                    res.Add(u);
            }
            return res;
        }
        
    }
}
