using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataContainer.MySortedDictionary;
using DataContainer.Value;

namespace DataContainer.SignalDump
{
    /// <summary>
    /// Класс, хранящий дамп для сигнала
    /// </summary>
    [System.Serializable]
    public class SimpleSignalDump : AbstractSimpleSignalDump<AbstractValue>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name"></param>
        public SimpleSignalDump(string name, ModellingType type)
            :base (name, type)
        {
        } 
    }
}