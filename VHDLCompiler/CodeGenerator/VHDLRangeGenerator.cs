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
using VHDL;
using VHDLRuntime.Range;
using VHDLRuntime.Values;

namespace VHDLCompiler.CodeGenerator
{
    public static class VHDLRangeGenerator
    {
        public static void FormIntegerRange(Range range, VHDLCompilerInterface compiler, out string RangeType, out List<string> Parameters)
        {
            string opFrom = VHDLOperandGenerator.GetOperand(range.From, compiler);
            string opTo = VHDLOperandGenerator.GetOperand(range.To, compiler);
            VHDLRuntime.Range.RangeDirection dir = ((range.Direction == Range.RangeDirection.TO) ? VHDLRuntime.Range.RangeDirection.To : VHDLRuntime.Range.RangeDirection.DownTo);
            string dir_s = string.Format("RangeDirection.{0}", dir);

            RangeType = "IntegerRange";
            Parameters = new List<string>() { opFrom, opTo, dir_s };
        }

        public static void FormRange(DiscreteRange range, VHDLCompilerInterface compiler, out string RangeType, out List<string> Parameters)
        {
            if (range is Range)
            {
                FormIntegerRange(range as Range, compiler, out RangeType, out Parameters);
                return;
            }

            throw new NotImplementedException();
        }        
    }
}
