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
using VHDLRuntime.Values;

namespace VHDLRuntime
{
    [Serializable]
    public class ProcessScheduler : SimulationBaseClass
    {
        private ArchitectureBase architecture;
        public ArchitectureBase Architecture
        {
            get { return architecture; }
            set { architecture = value; }
        }

        private List<Signal> signalList;
        public List<Signal> SignaList
        {
            get { return signalList; }
            protected set { signalList = value; }
        }

        private List<ProcessRuntimeInfo> processRuntime;
        public List<ProcessRuntimeInfo> ProcessRuntime
        {
            get { return processRuntime; }
            set { processRuntime = value; }
        }

        private Int64 currentTime;
        public Int64 CurrentTime
        {
            get { return currentTime; }
            protected set { currentTime = value; }
        }

        private int currentDutyCycle;
        public int CurrentDutyCycle
        {
            get { return currentDutyCycle; }
            set { currentDutyCycle = value; }
        }
        

        public ProcessScheduler(ArchitectureBase architecture)
        {
            this.architecture = architecture;
            signalList = new List<Signal>();
            processRuntime = new List<ProcessRuntimeInfo>();
        }

        public override void MainFunction()
        {
            foreach (var pi in processRuntime)
            {
                pi.MainFunction();
            }
        }

        public void RegisterSignal(Signal s)
        {
            signalList.Add(s);
        }

        public void RegisterProcess(ProcessRuntimeDelegare runtime, List<Signal> sensitivityList)
        {
            ProcessRuntimeInfo pi = new ProcessRuntimeInfo(runtime, this, sensitivityList);
            processRuntime.Add(pi);
        }

        public void RegisterProcess(ProcessRuntimeDelegare runtime)
        {
            RegisterProcess(runtime, new List<Signal>());
        }

        public virtual SimulationScope GetSimulationScope(SimulationScope parent)
        {
            SimulationScope res = new SimulationScope(architecture.InstName, parent);
            foreach (var s in signalList)
            {
                res.Variables.Add(s);
            }
            return res;
        }

        public void RegisterTransportDelayEvent(Signal signal, params ScheduledEvent[] newEvents)
        {
            signal.RegisterTransportDelayEvent((UInt64)CurrentTime, newEvents);
        }

        public void RegisterInertialDelayEvent(Signal signal, params ScheduledEvent[] newEvents)
        {
            signal.RegisterInertialDelayEvent((UInt64)CurrentTime, newEvents);
        }

        public void RegisterInertialDelayEvent(Signal signal, UInt64 RejectionTime, params ScheduledEvent[] newEvents)
        {
            signal.RegisterInertialDelayEvent((UInt64)CurrentTime, RejectionTime, newEvents);
        }

        public void RegisterDutyCycleDelayEvent(Signal signal, VHDLBaseValue value)
        {
            signal.RegisterDutyCycleDelayEvent(currentDutyCycle, value);
        }
    }
}
