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
using VHDL.literal;

namespace VHDLCompiler.CodeGenerator
{
    public class PhysicalTypeBaseInfo
    {
        private Int64 multiplier;
        public Int64 Multiplier
        {
            get { return multiplier; }
            set { multiplier = value; }
        }

        private string unitName;
        public string UnitName
        {
            get { return unitName; }
            set { unitName = value; }
        }

        private string baseUnitName;
        public string BaseUnitName
        {
            get { return baseUnitName; }
            set { baseUnitName = value; }
        }

        private bool isBaseUnit;
        public bool IsBaseUnit
        {
            get { return isBaseUnit; }
            set { isBaseUnit = value; }
        }
        
    }
}
