using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataContainer.Value;
using DataContainer.SignalDump;

namespace DataContainer.Objects
{
    [System.Serializable]
    public class Signal : IValueProvider
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

        private readonly VHDL.Object.Signal parsedVariable;

        #region Events
        public delegate void AddEventDelegate(Signal sender, UInt64 NOW, UInt64 after, AbstractValue value, VHDL.DelayMechanism delayMechanism);
        private event AddEventDelegate onAddEvent;
        public event AddEventDelegate OnAddEvent
        {
            add { onAddEvent += value; }
            remove { onAddEvent -= value; }
        }

        public delegate void AppendValueDelegate(Signal sender, UInt64 CurrentTime, AbstractValue value);
        private event AppendValueDelegate onAppendEvent;
        public event AppendValueDelegate OnAppendEvent
        {
            add { onAppendEvent += value; }
            remove { onAppendEvent -= value; }
        }
        #endregion


        public Signal(ModellingType type, VHDL.Object.Signal parsedVariable)
        {
            this.parsedVariable = parsedVariable;
            this.name = parsedVariable.Identifier;
            this.type = type;
            dump = new SimpleSignalDump(parsedVariable.Identifier, type);
            InitChildrens();
            InitValue();
        }

        public Signal(ModellingType type, VHDL.Object.Signal parsedVariable, List<AbstractSignalDump> dumps)
        {
            this.parsedVariable = parsedVariable;
            this.name = parsedVariable.Identifier;
            this.type = type;
            if ((type.Type is VHDL.type.RecordType) || (type.Type is VHDL.type.ArrayType))
                dump = new SignalScopeDump(parsedVariable.Identifier, type, dumps);
            InitChildrens();
            InitValue();
        }

        public Signal(AbstractSignalDump dump)
        {
            this.dump = dump;
            this.type = dump.Type;
            this.name = dump.Name;
            InitChildrens();
            InitValue();
        }

        private void InitValue()
        {
            if ((parsedVariable != null) && (parsedVariable.DefaultValue != null))
            {
                currentValue = ExpressionEvaluator.DefaultEvaluator.Evaluate(parsedVariable.DefaultValue);
                if (currentValue != null)
                    dump.AppendValue(0, currentValue);
            }
            onAddEvent = new AddEventDelegate(Signal_OnAddEvent);
            onAppendEvent = new AppendValueDelegate(Signal_onAppendEvent);
        }

        void Signal_onAppendEvent(Signal sender, ulong CurrentTime, AbstractValue value)
        {
        }

        void Signal_OnAddEvent(Signal sender, ulong NOW, ulong after, AbstractValue value, VHDL.DelayMechanism delayMechanism)
        {
        }

        /// <summary>
        /// Дамп данных
        /// </summary>
        private AbstractSignalDump dump;
        public AbstractSignalDump Dump
        {
            get { return dump; }
            set { dump = value; }
        }


        /// <summary>
        /// При составном типе данных - элементы массива или записи
        /// </summary>
        private List<Signal> childrens;
        public List<Signal> Childrens
        {
            get { return childrens; }
        }

        /// <summary>
        /// Выполняет инициализацию списка дочерних сигналов
        /// </summary>
        private void InitChildrens()
        {
            childrens = new List<Signal>();
            if (dump is SignalScopeDump)
            {
                foreach (AbstractSignalDump d in (dump as SignalScopeDump).Dumps)
                {
                    childrens.Add(d.CreateSignal());
                }
            }
        }
        

        #region IValueProvider Members

        public VHDL.Object.DefaultVhdlObject DefaultVhdlObject
        {
            get { return parsedVariable; }
        }

        private string name;
        public string Name
        {
            get { return name; }
        }

        private AbstractValue currentValue;
        public AbstractValue CurrentValue
        {
            get { return currentValue; }
            set { currentValue = value; }
        }

        private ModellingType type;
        public ModellingType Type
        {
            get
            {
                return type;
            }
        }

        public VHDL.ResolvedDiscreteRange[] Range
        {
            get { return type.Dimension; }
        }
        #endregion

        /// <summary>
        /// Добавление нового элемента в конец очереди газначения сигнала
        /// </summary>
        /// <param name="CurrentTime"></param>
        /// <param name="value"></param>
        public void AppendValue(UInt64 CurrentTime, AbstractValue value)
        {
            Dump.AppendValue(CurrentTime, value);
            onAppendEvent(this, CurrentTime, value);
        }

        /// <summary>
        /// Добавление нового элемента в конец очереди газначения сигнала
        /// </summary>
        /// <param name="CurrentTime"></param>
        /// <param name="value"></param>
        public void AppendValue(UInt64 CurrentTime, TimeStampInfo value)
        {
            Dump.AppendValue(CurrentTime, value);
        }

        public void AddEvent(UInt64 NOW, UInt64 after, AbstractValue value, VHDL.DelayMechanism delayMechanism)
        {
            Dump.AddEvent(NOW, after, value, delayMechanism);
            onAddEvent(this, NOW, after, value, delayMechanism);
        }

        public static Signal CreateSignal(VHDL.Object.Signal s)
        {
            return new Signal(TypeCreator.CreateType(s.Type), s);
        }

        public static Signal CreateSignal(VHDL.type.Type type, string identifier)
        {
            return new Signal(ModellingType.CreateModellingType(type), new VHDL.Object.Signal(identifier, type));
        }

        public static Signal CreateSignal(ModellingType type, string identifier)
        {
            return new Signal(type, new VHDL.Object.Signal(identifier, type.Type));
        }

        public static Signal CreateSignal(VHDL.type.Type type, string identifier, List<AbstractSignalDump> dumps)
        {
            return new Signal(ModellingType.CreateModellingType(type), new VHDL.Object.Signal(identifier, type), dumps);
        }

        public static Signal CreateSignal(ModellingType type, string identifier, List<AbstractSignalDump> dumps)
        {
            return new Signal(type, new VHDL.Object.Signal(identifier, type.Type), dumps);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
