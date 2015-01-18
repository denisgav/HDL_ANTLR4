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
    public class ProcesRoutineInfo
    {
        private List<string> sensitivityList;
        public List<string> SensitivityList
        {
            get { return sensitivityList; }
            set { sensitivityList = value; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string routine;
        public string Routine
        {
            get { return routine; }
            set { routine = value; }
        }

        public ProcesRoutineInfo(string name, string routine)
            : this(name, routine, new List<string>())
        { }

        public ProcesRoutineInfo(string name, string routine, List<string> sensitivityList)
        {
            this.name = name;
            this.routine = routine;
            this.sensitivityList = sensitivityList;
        }

    }
}
