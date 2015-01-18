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
    [Serializable]
    public class ArchitectureBase : SimulationBaseClass
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string instName;
        public string InstName
        {
            get { return instName; }
            set { instName = value; }
        }

        private ProcessScheduler scheduler;
        public ProcessScheduler Scheduler
        {
            get { return scheduler; }
            set { scheduler = value; }
        }

        public VHDLRuntime.Values.BuiltIn.TIME CurrentTime
        {
            get { return new Values.BuiltIn.TIME(Scheduler.CurrentTime); }
        }

        public int CurrentDutyCycle
        {
            get { return scheduler.CurrentDutyCycle; }
        }
        

        public ArchitectureBase(string name)
            : this(name, "noname")
        { }

        public ArchitectureBase(string name, string instName)
            : base()
        {
            this.name = name;
            this.instName = instName;
            scheduler = new ProcessScheduler(this);
        }

        protected void RegisterSignal(Signal s)
        {
            scheduler.RegisterSignal(s);
        }

        protected void RegisterProcess(ProcessRuntimeDelegare runtime, List<Signal> sensitivityList)
        {
            scheduler.RegisterProcess(runtime, sensitivityList);
        }

        protected void RegisterProcess(ProcessRuntimeDelegare runtime)
        {
            scheduler.RegisterProcess(runtime);
        }

        public override void MainFunction()
        {
            scheduler.MainFunction();
        }

        public virtual SimulationScope GetSimulationScope(SimulationScope parent)
        {
            return scheduler.GetSimulationScope(parent);
        }
    }
}
