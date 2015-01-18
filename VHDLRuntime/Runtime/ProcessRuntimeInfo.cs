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
using VHDLRuntime.Objects;

namespace VHDLRuntime
{
    public delegate void ProcessRuntimeDelegare(ProcessRuntimeInfo pi);

    [Serializable]
    public class ProcessRuntimeInfo : SimulationBaseClass
    {
        private ProcessRuntimeDelegare runtime;
        public ProcessRuntimeDelegare Runtime
        {
            get { return runtime; }
            set { runtime = value; }
        }

        private ProcessScheduler parent;
        public ProcessScheduler Parent
        {
            get { return parent; }
            set { parent = value; }
        }


        private List<Signal> sensitivityList;
        public List<Signal> SensitivityList
        {
            get { return sensitivityList; }
            protected set { sensitivityList = value; }
        }

        public ProcessRuntimeInfo(ProcessRuntimeDelegare runtime, ProcessScheduler parent)
            : this(runtime, parent, new List<Signal>())
        { }

        public ProcessRuntimeInfo(ProcessRuntimeDelegare runtime, ProcessScheduler parent, List<Signal> sensitivityList)
        {
            this.runtime = runtime;
            this.parent = parent;
            this.sensitivityList = sensitivityList;
        }

        public override void MainFunction()
        {
            runtime(this);
        }

    }
}
