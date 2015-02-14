using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VHDL.concurrent;

namespace VHDLModelingSystem
{
    public class Scheduler : AbstractScheduler
    {
        /// <summary>
        /// Ядро системы моделирования
        /// </summary>
        private ModelingSystemCore core;
        public ModelingSystemCore Core
        {
            get { return core; }
            set { core = value; }
        }

        public Scheduler(ModelingSystemCore core)
        {
            this.core = core;
            statementRunners = new List<ConcurrentStatementRunner>();

            foreach (ConcurrentStatement st in core.Architecture.Statements)
            {
                statementRunners.Add(new ConcurrentStatementRunner(st, core));
            }

            iterators = new Dictionary<DataContainer.Objects.Signal, DataContainer.MySortedDictionary.IValueIterator>();

            iterators.Clear();
            foreach (var variable in core.SignalScope)
            {
                DataContainer.MySortedDictionary.IValueIterator i = variable.Dump.Iterator;
                i.Reset();
                iterators.Add(variable, i);
            }
        }        

        /// <summary>
        /// Запуск на выполнение паралельного процесса
        /// </summary>
        /// <param name="runner"></param>
        protected override void StartConcurentStatement(ConcurrentStatementRunner runner)
        {
            runner.Start();
        }

        /// <summary>
        /// Обновление текущего модельного времени
        /// </summary>
        /// <param name="Time"></param>
        protected override UInt64 CurrentVirtualTime
        {
            set
            {
                core.NOW.DoubleValue = value;
            }
            get
            {
                return (UInt64)core.NOW.DoubleValue;
            }
        }

        protected override void EndOfCycle()
        {
            base.EndOfCycle();
        }
    }
}
