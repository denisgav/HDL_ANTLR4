using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Schematix.Waveform.Value_Dump;
using Parser;
using Schematix.Waveform.UserControls;
using Schematix.Core.UserControls;
using DataContainer.MySortedDictionary;
using DataContainer;
using DataContainer.Objects;
using DataContainer.ValueDump;

namespace Schematix.Waveform.TestBenchGenerator
{
    /// <summary>
    /// Класс для генерации TestBench по заданной волновой диаграмме для языка VHDL
    /// </summary>
    public class VHDLTestBenchGenerator :TestBenchGenerator, ITask
    {
        /// <summary>
        /// Имя entity
        /// </summary>
        protected string entityName;
        public string EntityName
        {
            get { return entityName; }
            set { entityName = value; }
        }

        /// <summary>
        /// Список сигналов (спацифическая информация для VCD)
        /// </summary>
        private List<My_Variable> variables;
        public List<My_Variable> Variables
        {
            get
            {
                return variables;
            }
        }

        /// <summary>
        /// Имя архитектуры
        /// </summary>
        private string architectureName;
        public string ArchitectureName
        {
            get { return architectureName; }
            set { architectureName = value; }
        }

        /// <summary>
        /// Используется для форматированного вывода данных
        /// </summary>
        private StreamWriter writer;

        public VHDLTestBenchGenerator(WaveformCore core, Stream stream, string entityName, string architectureName)
            : base(core, stream)
        {
            this.entityName = entityName;
            this.architectureName = architectureName;

            writer = new StreamWriter(stream);
            variables = new List<My_Variable>();
            foreach (IValueProvider v in core.Dump.GetVariablesEnumerator())
            {
                if (v is Signal)
                {
                    InterfaceMode mode = GetPortInterfaceMode(v.Name, core.Entity);
                    if ((mode == InterfaceMode._in) || (mode == InterfaceMode._inout) || (mode == InterfaceMode._buffer))
                        variables.Add(new My_Variable(v as Signal));
                }
            }
        }

        public override void Save()
        {
            //Подключаем библиотеки
            writer.WriteLine("library ieee;");
            writer.WriteLine("use ieee.std_logic_1164.all;");

            //Обьявляем entity
            writer.WriteLine(string.Format("entity {0}_testbench is", entityName));
            writer.WriteLine("end entity;");

            //Обьявляем архитектуру
            writer.WriteLine(string.Format("architecture testbench_architecture of {0}_testbench is", architectureName));

            //обьявляем компонент
            writer.WriteLine(string.Format("\tcomponent {0}", architectureName));
            //Выписываем декларацию Entity
            writer.WriteLine(core.Entity.entityDeclarativePart);
            writer.WriteLine("\tend component;");
            writer.Write("\n");
            writer.Write("\n");

            writer.Write(SignalDeclaration(core.Entity));
            writer.WriteLine("\nbegin");

            writer.WriteLine(string.Format("\tUUT : {0}", architectureName));
            writer.WriteLine("\tport map (");

            //выписываем port map
            writer.WriteLine(PortMapDeclaration(core.Entity));
            writer.WriteLine("\t);");

            writer.WriteLine("process");
            writer.WriteLine("begin");

            //Выписываем значения всех переменных
            WriteDump();

            writer.WriteLine("\twait;");
            writer.WriteLine("end process;");
            writer.WriteLine("end architecture testbench_architecture;");

            writer.Close();
            stream.Close();
            complete = true;
        }

        public void WriteDump()
        {
            UInt64 PrevTime = 0;

            List<IValueIterator> iterators = new List<IValueIterator>();
            foreach (var variable in core.Dump.GetVariablesEnumerator())
            {
                InterfaceMode mode = GetPortInterfaceMode(variable.Name, core.Entity);
                if ((mode == InterfaceMode._in) || (mode == InterfaceMode._inout) || (mode == InterfaceMode._buffer))
                    if(variable is Signal)
                        iterators.Add((variable as Signal).Dump.Iterator);
            }

            while (true)
            {
                //Проверяем, есть ли ещо данные для выборки
                bool IsEndOfData = true;
                foreach (var iterator in iterators)
                    if (iterator.IsEndOfIteration == false)
                    {
                        IsEndOfData = false;
                        break;
                    }
                if (IsEndOfData == true)
                    break;

                //выбираем первое событие (точнее минимальное время)
                UInt64 CurrentTime = UInt64.MaxValue;
                foreach (var iterator in iterators)
                {
                    if ((iterator.IsEndOfIteration == false) && (iterator.LastEvent < CurrentTime))
                        CurrentTime = iterator.LastEvent;
                }

                //мы достигли конца модельного времени
                if (CurrentTime > core.ScaleManager.EndTime)
                    return;

                //Выписываем время
                writer.WriteLine("\n\twait for {0} fs;", CurrentTime - PrevTime);
                PrevTime = CurrentTime;

                //передвигаем курсоры
                foreach (var iterator in iterators)
                {
                    if (iterator.LastEvent == CurrentTime)
                    {
                        iterator.MoveNext();
                        string value = VHDLTestBenchGeneratorConvertorsion.ToTestBenchString(iterator.CurrentValue.LastValue);
                        if (string.IsNullOrEmpty(value) == false)
                        {
                            if (value.Length == 1)
                                writer.WriteLine("\t{1} <= '{0}';", value, variables[iterators.IndexOf(iterator)].Name);
                            else
                                writer.WriteLine("\t{1} <= \"{0}\";", value, variables[iterators.IndexOf(iterator)].Name);
                        }
                    }
                }
            }
            foreach (var variable in variables)
                variable.Iterator.Reset();
        }

        public InterfaceMode GetPortInterfaceMode(string portName, Entity entity)
        {
            foreach (PortItem port in entity.Port_items)
            {
                foreach (string name in port.id_list)
                {
                    if (name.Equals(portName))
                        return port.interface_mode;
                }
            }
            return InterfaceMode._not_defined;
        }

        public string PortMapDeclaration(Entity entity)
        {
            StringBuilder res = new StringBuilder();

            foreach (PortItem port in entity.Port_items)
            {
                foreach(string portName in port.id_list)
                    res.AppendLine(string.Format("\t\t {0} => {0},", portName));
            }

            string resString = res.ToString();
            return resString.Substring(0, resString.Length - 3);
        }

        public string SignalDeclaration(Entity entity)
        {
            StringBuilder res = new StringBuilder();

            foreach (PortItem port in entity.Port_items)
            {
                res.Append("\n").Append("\tsignal ");
                for (int i = 0; i < port.id_list.Count; i++)
                    if (i < port.id_list.Count - 1)
                        res.Append(string.Format("{0}, ", port.id_list[i]));
                    else
                        res.Append(port.id_list[i]);

                res.Append(" : ").Append(port.subtype.sub_type_indication_string.ToLower());
                if (port.subtype.hasRange)
                {
                    if (port.subtype.range.isReverseRange)
                        res.Append(" ( " + port.subtype.range.leftRange + " downto " + port.subtype.range.rightRange + " ) ");
                    else
                        res.Append(" ( " + port.subtype.range.leftRange + " to " + port.subtype.range.rightRange + " ) ");
                }
                res.Append(";");
            }

            return res.ToString();
        }

        #region ITask Members

        public void Start()
        {
            Save();
        }

        public int PercentComplete
        {
            get { return 1; }
        }

        public string Name
        {
            get { return "Generating Test Bench..."; }
        }

        public bool IsIndeterminate
        {
            get { return true; }
        }

        bool complete = false;
        public bool IsComplete
        {
            get { return complete; }
        }

        public void OnCancel()
        {
            stream.Close();
        }

        #endregion
    }
}
