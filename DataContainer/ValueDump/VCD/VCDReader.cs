using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using DataContainer;
using Schematix.Core.UserControls;
using DataContainer.Objects;

namespace DataContainer.ValueDump
{
    /// <summary>
    /// Класс, который хранит в себе все
    /// значения сигналов в волновой диаграмме
    /// </summary>
    public class VCDReader : ValueDumpReader, ITask
    {
        /// <summary>
        /// Стек, хранящий полный путь к текущему блоку $Scope
        /// </summary>
        private Stack<VCDScope> CurrentVCDScope;

        private SimulationScope CurrentScope;

        private UInt64 CurrentTime;

        /// <summary>
        /// Комментарий
        /// </summary>
        private string comment;
        public string Comment
        {
            get { return comment; }
        }

        private UInt64 signalCounter = 0;

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


        private void Init()
        {
            timescale = new Timescale(1, TimeUnit.fs);
            variables = new List<VCD_Variable>();
            CurrentVCDScope = new Stack<VCDScope>();
            CurrentScope = dump;
            CurrentTime = 0;
        }

        public VCDReader(System.IO.Stream stream)
            : base(stream)
        {
            Init();
        }

        public VCDReader(string FileName)
            : base(FileName)
        {
            Init();
        }

        private string ReadBlock(StreamReader reader)
        {
            StringBuilder res = new StringBuilder();
            bool found_char = false;
            int index = 0;
            while (reader.EndOfStream == false)
            {
                char symbol = (char)reader.Read();
                if ((symbol == '$') && (index != 0))
                    found_char = true;
                res.Append(symbol);
                if ((found_char == true) & (res.ToString().EndsWith("$end\r\n") == true))
                    break;
                index++;
            }
            return res.ToString();
        }


        public override void Parse()
        {
            //очищаем хранимые данные (во избежание казусов при повторном использовании)
            dump.Clear();
            variables.Clear();

            //читаем ввесь текст
            StreamReader reader = new StreamReader(stream);

            while (reader.EndOfStream == false)
            {
                string Text = ReadBlock(reader);
                string[] blocks = Text.Split(new char[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                ParseHeaderItem(blocks);
                if (Text.Contains("$enddefinitions"))
                {
                    break;
                }
            }
            try
            {
                while (reader.EndOfStream == false)
                {
                    string Text = reader.ReadLine();
                    string[] blocks = Text.Split(new char[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    ParseVariable(blocks);
                }
            }
            catch
            {
            }
            finally
            {
                reader.Close();
                stream.Close();
                stream.Dispose();

                GC.Collect();
            }
        }

        /// <summary>
        /// Разпарсивает переменную и добавляет ее значение в дамп
        /// </summary>
        /// <param name="Words"></param>
        private void ParseVariable(string[] Words)
        {
            if (Words[0][0] == '#')
                OnTimestamp(Words[0]);
            else
            {
                if ((Words[0][0] == 'b') || (Words[0][0] == 'r'))
                {
                    OnComplexValueChange(Words[0], Words[1]);
                }
                else
                    OnSimpleValueChange(Words[0]);
            }
        }

        /// <summary>
        /// Проводит разпарсивание одного из элементов заголовка
        /// </summary>
        /// <param name="Words"></param>
        private void ParseHeaderItem(string[] Words)
        {
            switch (Words[0])
            {
                case "$comment": OnComment(Words); break;
                case "$date": OnDate(Words); break;
                case "$enddefinitions": OnEndDefinitions(Words); break;
                case "$scope": OnScope(Words); break;
                case "$timescale": OnTimescale(Words); break;
                case "$upscope": OnUpScope(Words); break;
                case "$var": OnVar(Words); break;
                case "$version": OnVersion(Words); break;
                case "$dumpall": OnDumpAll(Words); break;
                case "$dumpoff": OnDumpOff(Words); break;
                case "$dumpon": OnDumpOn(Words); break;
                case "$dumpvars": OnDumpVars(Words); break;
                default: throw new Exception("Parsing Failed in ParseHeaderItem");
            }
        }

        /// <summary>
        /// The $comment section allows for single or multiple line comments 
        /// (Вдруг в бужущем пригодится)
        /// </summary>
        /// <param name="Words"></param>
        private void OnComment(string[] Words)
        { }

        /// <summary>
        /// The $date section indicates the date the VCD file was created. 
        /// </summary>
        /// <param name="Words"></param>
        private void OnDate(string[] Words)
        {
            StringBuilder builder = new StringBuilder();
            for(int i=1; i<Words.Length-1; i++)
                builder.AppendFormat("{0} ", Words[i]);
            date = builder.ToString();
        }

        /// <summary>
        /// The $enddefinition section marks the end of the header information and variable definitions. 
        /// </summary>
        /// <param name="Words"></param>
        private void OnEndDefinitions(string[] Words)
        {
        }

        /// <summary>
        /// The $scope section defines the scope of which the variables belong in. 
        /// </summary>
        /// <param name="Words"></param>
        private void OnScope(string[] Words)
        {
            VCDScope sc = VCDScope.Parse(Words);
            CurrentVCDScope.Push(sc);
            CurrentScope = CurrentScope.AddNewScope(sc.Name);
        }

        /// <summary>
        /// The $timescale section specifies what timescale was used for the simulation. 
        /// </summary>
        /// <param name="Words"></param>
        private void OnTimescale(string[] Words)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 1; i < Words.Length - 1; i++)
                builder.AppendFormat("{0} ", Words[i]);
            timescale = Timescale.Parse(builder.ToString());
        }

        /// <summary>
        /// The $upscope section indicates a scope change to a higher level in the hierachy.
        /// </summary>
        /// <param name="Words"></param>
        private void OnUpScope(string[] Words)
        {
            CurrentVCDScope.Pop();
            CurrentScope = CurrentScope.Parent;
        }

        /// <summary>
        /// The $var section lists the names and identifier codes of all the variables.
        /// </summary>
        /// <param name="Words"></param>
        private void OnVar(string[] Words)
        {
            VCD_Variable var = VCD_Variable.Parse(Words);
            var.Variable.Idx = signalCounter;
            signalCounter++;
            var.Scope = CurrentVCDScope.ToArray();
            variables.Add(var);

            /*if (var.Size == 1)
            //Простой тип данных
            {
                My_Variable new_var = new My_Variable(var.Reference, var.FullName, new Schematix.Waveform.DataTypes.STD_LOGIC());
                var.Variable = new_var;
                CurrentScope.Variables.Add(new_var);
            }
            //векторный тип данных
            else
            {
                My_Variable new_var = new My_Variable(var.Reference, var.FullName, new Schematix.Waveform.DataTypes.STD_LOGIC_VECTOR(0, (int)var.Size));
                var.Variable = new_var;
                CurrentScope.Variables.Add(new_var);
            }*/

            Signal new_var = var.Variable;
            var.Variable = new_var;
            CurrentScope.Variables.Add(new_var);
        }

        /// <summary>
        /// The $version section indicates the version of the VCD software used to produce the VCD file. 
        /// </summary>
        /// <param name="Words"></param>
        private void OnVersion(string[] Words)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 1; i < Words.Length - 1; i++)
                builder.AppendFormat("{0} ", Words[i]);
            version = builder.ToString();
        }

        /// <summary>
        /// The $dumpall section specifies the current values of all the variables. 
        /// </summary>
        /// <param name="Words"></param>
        private void OnDumpAll(string[] Words)
        { }

        /// <summary>
        /// The $dumpoff section specifies the current value of all the variables to be X values. 
        /// </summary>
        /// <param name="Words"></param>
        private void OnDumpOff(string[] Words)
        { }

        /// <summary>
        /// The $dumpon section specifies resumption of capturing values changes and lists the current values of all variables. 
        /// </summary>
        /// <param name="Words"></param>
        private void OnDumpOn(string[] Words)
        { }

        /// <summary>
        /// The $dumpvars section lists initial values of all variables 
        /// </summary>
        /// <param name="Words"></param>
        private void OnDumpVars(string[] Words)
        { }

        private VCD_Variable GetVCDVariableByIdentifier(string identifier, bool isSimple)
        {
            foreach (VCD_Variable v in variables)
            {
                if (v.Identifier.Equals(identifier) == true)
                    return v;
            }
            return null;
        }

        /// <summary>
        /// The timestamp statement indicates the current
        /// time where the following value changes take place.
        /// The timestamps doesn't not need to be continuously.
        /// Only when there are value changes will
        /// there be a need to indicate the current timestamp. 
        /// </summary>
        /// <param name="value"></param>
        private void OnTimestamp(string value)
        {
            string time = value.Substring(1);
            CurrentTime = UInt64.Parse(time) * timescale.GetTimeUnitInFS();
        }

        private void OnSimpleValueChange(string value)
        {
            string identifier = value.Substring(1);
            string newvalue = new string(new char[]{value[0]});
            VCD_Variable vcd_var = GetVCDVariableByIdentifier(identifier, true);
            vcd_var.AppendValue(CurrentTime, newvalue);
        }

        private void OnComplexValueChange(string value, string identifier)
        {
            VCD_Variable vcd_var = GetVCDVariableByIdentifier(identifier, false);
            vcd_var.AppendValue(CurrentTime, value);
        }

        public override string SummaryInfo
        {
            get 
            {
                StringBuilder res = new StringBuilder();
                res.AppendLine("Data format: VCD file");
                if (string.IsNullOrEmpty(comment) == false)
                    res.AppendFormat("Comment: /n {0} /n", comment);
                res.AppendFormat("Date: {0} \n", date);
                res.AppendFormat("Timescale: {0}\n", timescale);
                res.AppendFormat("Version: {0}\n", version);
                return res.ToString();
            }
        }

        #region ITask Members

        public void Start()
        {
            Parse();
        }

        public int PercentComplete
        {
            get
            {
                try
                {
                    return (int)((stream.Position * 100) / stream.Length);
                }
                catch (Exception ex)
                {
                    return 100;
                }
            }
        }

        public string Name
        {
            get 
            {
                return "Parsing vcd file";
            }
        }

        public bool IsIndeterminate
        {
            get { return false; }
        }

        public bool IsComplete
        {
            get
            {
                return stream.CanRead == false;
            }
        }

        public void OnCancel()
        {
            stream.Close();
        }

        #endregion
    }
}
