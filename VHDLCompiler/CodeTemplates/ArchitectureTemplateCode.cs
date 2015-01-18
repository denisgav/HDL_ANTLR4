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

namespace VHDLCompiler.CodeTemplates
{
    public partial class ArchitectureTemplate
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string nameSpaceName;
        public string NameSpaceName
        {
            get { return nameSpaceName; }
            set { nameSpaceName = value; }
        }

        private List<string> declarationList;
        public List<string> DeclarationList
        {
            get { return declarationList; }
            set { declarationList = value; }
        }

        private List<string> signalNameList;
        public List<string> SignalNameList
        {
            get { return signalNameList; }
            set { signalNameList = value; }
        }

        private List<ProcesRoutineInfo> processList;
        public List<ProcesRoutineInfo> ProcessList
        {
            get { return processList; }
            set { processList = value; }
        }

        public ArchitectureTemplate(string name)
            : this(name, "Work", new List<string>(), new List<string>(), new List<ProcesRoutineInfo>())
        {
        }

        public ArchitectureTemplate(string name, string nameSpaceName, List<string> declarationList, List<string> signalNameList, List<ProcesRoutineInfo> processList)
        {
            this.name = name;
            this.nameSpaceName = nameSpaceName;
            this.declarationList = declarationList;
            this.signalNameList = signalNameList;
            this.processList = processList;
        }
    }
}