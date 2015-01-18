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
using System.IO;
using VHDLRuntime.Objects;

namespace VHDLRuntime.ValueDump
{
    public class VCDWriter : ValueDumpWriter, ITask
    {
        /// <summary>
        /// /используется для форматированного вывода данных
        /// </summary>
        private StreamWriter writer;

        /// <summary>
        /// используется для заполнения идентификаторов в переменных
        /// </summary>
        private int identifier = 0x21;
		
		/// <summary>
        /// Начальный символ для идентификатора
        /// </summary>
		private const int startIdentifier = 0x21;


        /// <summary>
        /// Список сигналов (спацифическая информация для VCD)
        /// </summary>
        private List<VCD_Variable> variables;
        public List<VCD_Variable> Variables
        {
            get
            {
                return variables;
            }
        }


        #region constructors
        public VCDWriter(SimulationScope dump, System.IO.Stream stream, Timescale Timescale, string Date, string Version)
            : base(dump, stream, Timescale, Date, Version)
        {
            writer = new StreamWriter(stream);
            variables = new List<VCD_Variable>();
        }

        public VCDWriter(SimulationScope dump, String FileName, Timescale Timescale, string Date, string Version)
            : base(dump, FileName, Timescale, Date, Version)
        {
            writer = new StreamWriter(stream);
            variables = new List<VCD_Variable>();
        }

        public VCDWriter(SimulationScope dump, System.IO.Stream stream)
            : base(dump, stream)
        {
            variables = new List<VCD_Variable>();
            writer = new StreamWriter(stream);
        }

        public VCDWriter(SimulationScope dump, String FileName)
            : base(dump, FileName)
        {
            variables = new List<VCD_Variable>();
            writer = new StreamWriter(stream);
        }
        #endregion

        /// <summary>
        /// Запись в дамп переменных (с одновременной инициализацией списка переменны[)
        /// </summary>
        /// <param name="scope"></param>
        private void WriteScopeVariables(SimulationScope scope)
        {
            if(scope.Parent != null)
                writer.WriteLine( string.Format("$scope module {0}  $end", scope.Name));
			
			string str_identifier = new string(new char[]{(char)identifier});
            foreach (ValueProvider var in scope.Variables)
            {
                if (var is Signal)
                {
                    VCD_Variable vcd_var = new VCD_Variable((var as Signal), str_identifier);
                    variables.Add(vcd_var);
                    identifier++;
                    StringBuilder newIdentf = new StringBuilder();

                    for (int i = identifier; i > 0; i /= 256)
                        newIdentf.Append((char)(i % 256));

                    str_identifier = newIdentf.ToString();
                    writer.WriteLine(string.Format("$var {0} $end", vcd_var.VCDVariableDeclaration));
                }
            }

            foreach (SimulationScope s in scope.Items)
                WriteScopeVariables(s);

            if(scope.Parent != null)
                writer.WriteLine("$upscope $end");
        }

        /// <summary>
        /// запись данных в файл
        /// </summary>
        public override void Write()
        {
            complete = false;

            WriteHeader();
            WriteDump();
            writer.Close();
            stream.Close();
            stream.Dispose();

            complete = true;
        }

        /// <summary>
        /// Запись заголовка VCD файла
        /// </summary>
        private void WriteHeader()
        {
            writer.WriteLine(string.Format("$date {0} $end", date));
            writer.WriteLine(string.Format("$version {0} $end", version));
            writer.WriteLine(string.Format("$timescale {0} $end", timescale));
            WriteScopeVariables(dump);
            writer.WriteLine("$enddefinitions $end");
        }

        /// <summary>
        /// Запись значения сигналов
        /// </summary>
        private void WriteDump()
        {
            while (true)
            {
                //Проверяем, есть ли ещо данные для выборки
                bool IsEndOfData = true;
                foreach (var variable in variables)
                    if (variable.Iterator.IsEndOfIteration == false)
                    {
                        IsEndOfData = false;
                        break;
                    }
                if (IsEndOfData == true)
                    break;

                //выбираем первое событие (точнее минимальное время)
                UInt64 CurrentTime = UInt64.MaxValue;
                foreach (var variable in variables)
                {
                    if ((variable.Iterator.IsEndOfIteration == false) && (variable.Iterator.LastEvent < CurrentTime))
                        CurrentTime = variable.Iterator.LastEvent;
                }

                //Выписываем время
                writer.WriteLine("#{0}", CurrentTime);

                //передвигаем курсоры
                foreach (var variable in variables)
                {
                    if (variable.Iterator.LastEvent == CurrentTime)
                    {
                        variable.Iterator.MoveNext();
                        if ((variable.Iterator.CurrentValue != null))
                            writer.WriteLine(ValueDump.VCDConvertor.ToVCDString(variable.Iterator.CurrentValue.LastValue, variable.Identifier));
                    }
                }
            }
            foreach (var variable in variables)
                variable.Iterator.Reset();
        }



        #region ITask Members

        public void Start()
        {
            Write();
        }

        public int PercentComplete
        {
            get { return 1; }
        }

        public string Name
        {
            get { return "Writing data to VCD file"; }
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
