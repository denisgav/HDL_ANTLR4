using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataContainer.ValueDump;
using DataContainer.Value;
using DataContainer;
using DataContainer.Objects;

namespace VHDLModelingSystem
{
    public class ModelingSystemCore : IValueProviderContainer
    {
        /// <summary>
        /// Общий проект
        /// </summary>
        private readonly VHDL.RootDeclarativeRegion rootDeclarationRegion;
        public VHDL.RootDeclarativeRegion RootDeclarationRegion
        {
            get { return rootDeclarationRegion; }
        }

        /// <summary>
        /// Текущая библиотека
        /// </summary>
        private readonly VHDL.LibraryDeclarativeRegion library;
        public VHDL.LibraryDeclarativeRegion Library
        {
            get { return library; }
        }

        /// <summary>
        /// Моделируемая архитектура
        /// </summary>
        private readonly VHDL.libraryunit.Architecture architecture;
        public  VHDL.libraryunit.Architecture Architecture
        {
            get { return architecture; }
        }

        /// <summary>
        /// Результаты моделирования
        /// </summary>
        private SimulationScope result_scope;
        public SimulationScope ResultScope
        {
            get { return result_scope; }
            set { result_scope = value; }
        }
        

        /// <summary>
        /// Список моделируемых сигналов
        /// </summary>
        private List<Signal> signalScope;
        public List<Signal> SignalScope
        {
            get { return signalScope; }
            set { signalScope = value; }
        }

        public Signal GetSignal(VHDL.Object.Signal signal)
        {
            foreach (Signal s in signalScope)
            {
                if (s.DefaultVhdlObject == signal)
                    return s;
            }
            return null;
        }

        public IValueProvider GetValueProvider(VHDL.Object.VhdlObject o)
        {
            foreach (IValueProvider v in currentScope)
            {
                if (v.DefaultVhdlObject == o)
                    return v;
            }
            return null;
        }

        /// <summary>
        /// Получить обьект с текущей области видимости
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public IValueProvider GetValueProvider(string identifier)
        {
            foreach (IValueProvider val in currentScope)
                if (val.DefaultVhdlObject.Identifier.Equals(identifier, StringComparison.InvariantCultureIgnoreCase))
                    return val;
            return null;
        }

        /// <summary>
        /// Класс, отвечающий за вызов функции
        /// </summary>
        private FunctionAnalyser functionAnalyser;
        public FunctionAnalyser FunctionAnalyser
        {
            get { return functionAnalyser; }
        }

        /// <summary>
        /// Список текущих переменных
        /// </summary>
        private List<IValueProvider> currentScope;
        public List<IValueProvider> CurrentScope
        {
            get { return currentScope; }
            set { currentScope = value; }
        }

        /// <summary>
        /// Текущее модельное время
        /// </summary>
        private TIME_VALUE now = new TIME_VALUE(0);
        public  TIME_VALUE NOW
        {
            get { return now; }
            set { now = value; }
        }

        private void FormCurrentScope()
        {
            List<VHDL.Object.Variable> variables = architecture.Scope.GetListOfObjects<VHDL.Object.Variable>();
            foreach (VHDL.Object.Variable v in variables)
                currentScope.Add(new Variable(v));

            List<VHDL.Object.Constant> constants = architecture.Scope.GetListOfObjects<VHDL.Object.Constant>();
            foreach (VHDL.Object.Constant c in constants)
                currentScope.Add(new Constant(c));
        }
        
        

        public ModelingSystemCore(VHDL.libraryunit.Architecture architecture, VHDL.LibraryDeclarativeRegion library, VHDL.RootDeclarativeRegion rootDeclarationRegion)
        {
            ExpressionEvaluator.DefaultEvaluator = new ExpressionEvaluator(this);
            this.architecture = architecture;
            this.library = library;
            this.rootDeclarationRegion = rootDeclarationRegion;
            functionAnalyser = new FunctionAnalyser(architecture.Scope);
            currentScope = new List<IValueProvider>();
            signalScope = new List<Signal>();

            now = new TIME_VALUE(0);

            FormDefaultResultScope();
        }

        public ModelingSystemCore(VHDL.libraryunit.Architecture architecture, VHDL.LibraryDeclarativeRegion library, VHDL.RootDeclarativeRegion rootDeclarationRegion, SimulationScope scope)
        {
            ExpressionEvaluator.DefaultEvaluator = new ExpressionEvaluator(this);
            this.architecture = architecture;
            this.library = library;
            this.rootDeclarationRegion = rootDeclarationRegion;
            functionAnalyser = new FunctionAnalyser(architecture.Scope);
            currentScope = new List<IValueProvider>();
            signalScope = new List<Signal>();

            now = new TIME_VALUE(0);

            result_scope = scope;
            foreach (var s in scope.Variables)
                if (s is Signal)
                {
                    signalScope.Add(s as Signal);
                    currentScope.Add(s as Signal);
                }
        }

        /// <summary>
        /// Планировщик
        /// </summary>
        private Scheduler scheduler;
        public Scheduler Scheduler
        {
            get { return scheduler; }
            set { scheduler = value; }
        }


        private void FormDefaultResultScope()
        {
            result_scope = new SimulationScope("root", null);
            List<VHDL.Object.Signal> signals = architecture.Scope.GetListOfObjects<VHDL.Object.Signal>();
            foreach (VHDL.Object.Signal s in signals)
            {
                    Signal signal = Signal.CreateSignal(s);
                    signalScope.Add(signal);
                    currentScope.Add(signal);
                    result_scope.Variables.Add(signal);
            }
        }

        private void FormResultScope()
        {
            List<VHDL.Object.Signal> signals = architecture.Scope.GetListOfObjects<VHDL.Object.Signal>();
            foreach (VHDL.Object.Signal s in signals)
            {
                Signal signal = Signal.CreateSignal(s);
                signalScope.Add(signal);
                currentScope.Add(signal);
            }
        }

        /// <summary>
        /// ЗАпуск моделирования
        /// </summary>
        public void Start()
        {
            
            FormCurrentScope();
            scheduler = new Scheduler(this);

            scheduler.Start();

            //core.Dump = result_scope;
        }

        /// <summary>
        /// Сохранение в файл VCD
        /// </summary>
        public void SaveToVCD(string fileName)
        {
            VCDWriter vcd = new VCDWriter(result_scope, fileName);
            Schematix.Core.UserControls.ProgressWindow.Window.RunProcess(vcd);
        }
    }
}