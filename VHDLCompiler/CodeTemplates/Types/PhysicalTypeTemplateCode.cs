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
using VHDLCompiler.CodeGenerator;

namespace VHDLCompiler.CodeTemplates.Types
{
    public partial class PhysicalTypeTemplate
    {
        private string nameSpaceName;
        public string NameSpaceName
        {
            get { return nameSpaceName; }
            set { nameSpaceName = value; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string rangeLeft;
        public string RangeLeft
        {
            get { return rangeLeft; }
            set { rangeLeft = value; }
        }

        private string rangeRight;
        public string RangeRight
        {
            get { return rangeRight; }
            set { rangeRight = value; }
        }

        private string rangeRirection;
        public string Direction
        {
            get { return rangeRirection; }
            set { rangeRirection = value; }
        }

        private List<PhysicalTypeBaseInfo> dict;
        public List<PhysicalTypeBaseInfo> Dict
        {
            get { return dict; }
            set { dict = value; }
        }
        
        
        public PhysicalTypeTemplate(string nameSpaceName, string name, List<PhysicalTypeBaseInfo> dict, Int64 rangeLeft, Int64 rangeRight, RangeDirection rangeDirection)
        {
            this.nameSpaceName = nameSpaceName;
            this.name = name;
            this.dict = dict;
            this.rangeLeft = rangeLeft.ToString();
            this.rangeRight = rangeRight.ToString();
            this.Direction = string.Format("RangeDirection.{0}", rangeDirection.ToString());            
        }

        public PhysicalTypeTemplate(string name, List<PhysicalTypeBaseInfo> dict, Int64 rangeLeft, Int64 rangeRight, RangeDirection rangeDirection)
            : this("Work", name, dict, rangeLeft, rangeRight, rangeDirection)
        {
        }
    }
}
