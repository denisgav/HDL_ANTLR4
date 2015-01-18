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

namespace VHDLCompiler.CodeTemplates
{
    public partial class VariableAssignTemplate
    {
        private string target;
        public string Target
        {
            get { return target; }
            set { target = value; }
        }

        private string _value;
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        private string targetType;
        public string TargetType
        {
            get { return targetType; }
            set { targetType = value; }
        }


        public VariableAssignTemplate(string target, string _value, string targetType)
        {
            this.target = target;
            this._value = _value;
            this.targetType = targetType;
        }        
    }
}
