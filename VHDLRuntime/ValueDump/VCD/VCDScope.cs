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
using System.Text;

namespace VHDLRuntime.ValueDump
{
    public struct VCDScope
    {
        public enum ScopeType
        {
            begin,
            fork,
            function,
            module,
            task
        }

        /// <summary>
        /// Тип Scope
        /// </summary>
        private ScopeType scType;
        public ScopeType ScType
        {
            get
            {
                return scType;
            }
        }

        /// <summary>
        /// Имя Scope
        /// </summary>
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
        }

        public static VCDScope Parse(string[] Words)
        {
            VCDScope sc = new VCDScope();
            switch (Words[1])
            {
                case "begin": sc.scType = ScopeType.begin; break;
                case "fork": sc.scType = ScopeType.fork; break;
                case "function": sc.scType = ScopeType.function; break;
                case "module": sc.scType = ScopeType.begin; break;
                case "task": sc.scType = ScopeType.begin; break;
                default: throw new Exception("Scope Type is incorrect");
            }
            sc.name = Words[2];
            return sc;
        }
    }
}
