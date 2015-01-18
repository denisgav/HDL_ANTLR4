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

namespace VHDLCompiler.CodeTemplates.Statements
{
    public partial class ForRangeTemplate
    {
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

        private string rangeDirection;
        public string RangeDirection
        {
            get { return rangeDirection; }
            set { rangeDirection = value; }
        }

        private string rangeType;
        public string RangeType
        {
            get { return rangeType; }
            set { rangeType = value; }
        }

        private string indexName;
        public string IndexName
        {
            get { return indexName; }
            set { indexName = value; }
        }

        private List<string> statements;
        public List<string> Statements
        {
            get { return statements; }
            set { statements = value; }
        }


        public ForRangeTemplate(string rangeLeft, string rangeRight, string rangeDirection, string rangeType, string indexName, List<string> statements)
        {
            this.rangeLeft = rangeLeft;
            this.rangeRight = rangeRight;
            this.rangeDirection = rangeDirection;
            this.rangeType = rangeType;
            this.indexName = indexName;
            this.statements = statements;
        }
    }
}
