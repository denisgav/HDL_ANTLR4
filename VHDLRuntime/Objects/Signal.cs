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
using VHDLRuntime.Values;
using VHDLRuntime.MySortedDictionary;

namespace VHDLRuntime.Objects
{
    [Serializable]
    public class Signal: ValueProvider
    {
        /// <summary>
        /// Id текущего сигнала
        /// </summary>
        private UInt64 idx;
        public UInt64 Idx
        {
            get { return idx; }
            set { idx = value; }
        }

        /// <summary>
        /// Дамп данных
        /// </summary>
        private NewSortedDictionary<TimeStampInfo<VHDLBaseValue>> dump;
        public NewSortedDictionary<TimeStampInfo<VHDLBaseValue>> Dump
        {
            get { return dump; }
            set { dump = value; }
        }

        public Signal(Type type, VHDLBaseValue value_ = null, string name = "", VHDLDirection direction = VHDLDirection.InOut)
            : base(type, value_, name, direction)
        {
            dump = new NewSortedDictionary<TimeStampInfo<VHDLBaseValue>>();
        }

        public void RegisterDutyCycleDelayEvent(int CurrentDutyCycle, VHDLBaseValue value)
        {
            dump.LastValue.Info.Add(CurrentDutyCycle + 1, value);
        }

        public void RegisterTransportDelayEvent(UInt64 CurrentTime, UInt64 DelayTime, VHDLBaseValue value)
        {
            dump.AddTransportEvent(CurrentTime + DelayTime, new TimeStampInfo<VHDLBaseValue>(value));
        }

        public void RegisterTransportDelayEvent(UInt64 CurrentTime, ScheduledEvent newEvent)
        {
            RegisterTransportDelayEvent(CurrentTime, newEvent.Delay, newEvent.Value);
        }

        public void RegisterTransportDelayEvent(UInt64 CurrentTime, params ScheduledEvent[] newEvents)
        {
            foreach (ScheduledEvent newEvent in newEvents)
            {
                RegisterTransportDelayEvent(CurrentTime, newEvent);
            }
        }

        public void RegisterInertialDelayEvent(UInt64 CurrentTime, UInt64 DelayTime, VHDLBaseValue value)
        {
            dump.AddInertialEvent(CurrentTime, CurrentTime + DelayTime, new TimeStampInfo<VHDLBaseValue>(value));
        }

        public void RegisterInertialDelayEvent(UInt64 CurrentTime, ScheduledEvent newEvent)
        {
            RegisterInertialDelayEvent(CurrentTime, newEvent.Delay, newEvent.Value);
        }

        public void RegisterInertialDelayEvent(UInt64 CurrentTime, params ScheduledEvent[] newEvents)
        {
            foreach (ScheduledEvent newEvent in newEvents)
            {
                RegisterInertialDelayEvent(CurrentTime, newEvent);
            }
        }

        public void RegisterInertialDelayEvent(UInt64 CurrentTime, UInt64 DelayTime, UInt64 RejectionTime, VHDLBaseValue value)
        {
            dump.AddInertialEvent(CurrentTime, CurrentTime + DelayTime, new TimeStampInfo<VHDLBaseValue>(value), RejectionTime);
        }

        public void RegisterInertialDelayEvent(UInt64 CurrentTime, UInt64 RejectionTime, ScheduledEvent newEvent)
        {
            RegisterInertialDelayEvent(CurrentTime, newEvent.Delay, RejectionTime, newEvent.Value);
        }

        public void RegisterInertialDelayEvent(UInt64 CurrentTime, UInt64 RejectionTime, params ScheduledEvent[] newEvents)
        {
            foreach (ScheduledEvent newEvent in newEvents)
            {
                RegisterInertialDelayEvent(CurrentTime, RejectionTime, newEvent);
            }
        }
    }

    [Serializable]
    public class Signal<T> : Signal where T : VHDLBaseValue
    {
        public Signal(VHDLBaseValue value_ = null, string name = "", VHDLDirection direction = VHDLDirection.InOut)
            : base(typeof(T), value_, name, direction)
        {
        }

        public new T GetValue()
        {
            return base.GetValue() as T;
        }

        public new T Value
        {
            get
            {
                return base.Value as T;
            }
            set
            {
                base.Value = value;
            }
        }
    }
}
