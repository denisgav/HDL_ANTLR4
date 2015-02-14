using System;
using System.Collections.Generic;
using Schematix.Waveform.Value_Dump;
using Schematix.Waveform.Iterators;
using System.Collections.ObjectModel;
using Schematix.Waveform.Configuration;
using Parser;
using Schematix.Waveform.TestBenchGenerator;
using System.IO;
using Schematix.Waveform.UserControls;
using System.Windows;
using Schematix.Core.UserControls;
using DataContainer;
using DataContainer.ValueDump;
using DataContainer.Objects;

namespace Schematix.Waveform
{
    public class WaveformCore
    {
        /// <summary>
        /// Список сигналов
        /// </summary>
        protected SimulationScope dump;
        public SimulationScope Dump
        {
            get
            {
                return dump;
            }
            set
            {
                dump = value;
            }
        }

        /// <summary>
        /// Список сигналов с которыми в текущий момент идет работа
        /// </summary>
        private ObservableCollection<My_Variable> currentDump;
        public ObservableCollection<My_Variable> CurrentDump
        {
            get
            {
                return currentDump;
            }
            set
            {
                currentDump = value;
            }
        }

        /// <summary>
        /// начальное время
        /// </summary>
        public UInt64 StartTime 
        {
            get
            {
                if (currentDump.Count == 0)
                    return 0;
                UInt64 min = currentDump[0].Signal.Dump.StartTime;
                foreach (My_Variable v in currentDump)
                {
                    if (v.Signal.Dump.StartTime < min)
                        min = v.Signal.Dump.StartTime;
                }
                return min;
            }
        }

        /// <summary>
        /// конечное время
        /// </summary>
        public UInt64 EndTime 
        {
            get
            {
                if (currentDump.Count == 0)
                    return 0;
                UInt64 max = currentDump[0].Signal.Dump.EndTime;
                foreach (My_Variable v in currentDump)
                {
                    if (v.Signal.Dump.EndTime > max)
                        max = v.Signal.Dump.EndTime;
                }
                return max;
            }
        }

        /// <summary>
        /// перечень измерений времени
        /// </summary>
        private ObservableCollection<TimeMeasureData> timeMeasureList;
        public ObservableCollection<TimeMeasureData> TimeMeasureList
        {
            get { return timeMeasureList; }
            set { timeMeasureList = value; }
        }
        

        /// <summary>
        /// Временная шкала
        /// </summary>
        private Timescale timescale;
        public Timescale Timescale
        {
            get
            {
                return timescale;
            }
        }

        /// <summary>
        /// Общая информация об открытом файле
        /// </summary>
        private string summaryInfo;
        public string SummaryInfo
        {
            get { return summaryInfo; }
        }

        /// <summary>
        /// описание моделируемого Entity
        /// </summary>
        private Entity entity;
        public Entity Entity
        {
            get { return entity; }
            set { entity = value; }
        }

        /// <summary>
        /// Имя архитектуры
        /// </summary>
        private string architectureName = string.Empty;
        public string ArchitectureName
        {
            get { return architectureName; }
            set { architectureName = value; }
        }

        /// <summary>
        /// Имя архитектуры
        /// </summary>
        private string entityName = string.Empty;
        public string EntityName
        {
            get { return entityName; }
            set { entityName = value; }
        }

        /// <summary>
        /// Файл в котором находится архитектура
        /// </summary>
        private string fileName = string.Empty;
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        /// <summary>
        /// Класс, отвечающий за масштабирование
        /// </summary>
        private ScaleManager scaleManager;
        public ScaleManager ScaleManager
        {
            get { return scaleManager; }
            set { scaleManager = value; }
        }

        private CursorViewer cursorViewer;
        public CursorViewer CursorViewer
        {
            get { return cursorViewer; }
            set { cursorViewer = value; }
        }
        

        /// <summary>
        /// Список пометок
        /// </summary>
        private ObservableCollection<BookMark> bookMarks;
        public ObservableCollection<BookMark> BookMarks
        {
            get { return bookMarks; }
            set { bookMarks = value; }
        }

        private WaveformUserControl waveformUserControl;
        public WaveformUserControl WaveformUserControl
        {
            get { return waveformUserControl; }
            set { waveformUserControl = value; }
        }


        public WaveformCore(WaveformUserControl waveformUserControl)
        {
            this.waveformUserControl = waveformUserControl;
            dump = new SimulationScope("Root", null);
            timescale = new Timescale(1, TimeUnit.fs);
            currentDump = new ObservableCollection<My_Variable>();
            entity = new Entity();
            bookMarks = new ObservableCollection<BookMark>();
            timeMeasureList = new ObservableCollection<TimeMeasureData>();
        }

        private void LoadConfiguration(string confPath)
        {
            currentDump.Clear();
            WaveformConfiguration conf = WaveformConfiguration.LoadConfiguration(confPath);
            if (conf != null)
            {
                foreach (My_VariableConfiguration varc in conf.VariablesConfiguration)
                {
                    foreach (IValueProvider var in dump.GetVariablesEnumerator())
                    {
                        if (var is Signal)
                        {
                            if ((var as Signal).Idx.Equals(varc.Idx))
                            {
                                My_Variable variable = new My_Variable(var as Signal);
                                variable.DataRepresentation = varc.DataRepresentation;
                                currentDump.Add(variable);
                                break;
                            }
                        }
                    }
                }
                ScaleManager.StartTime = StartTime;
                ScaleManager.EndTime = EndTime;
                ScaleManager.VisibleStartTime = conf.VisibleStartTime;
                ScaleManager.VisibleTimeDiapasone = conf.VisibleTimeDiapasone;
                entity = conf.Entity;
                architectureName = conf.ArchitectureName;
                entityName = conf.EntityName;
                fileName = conf.FileName;
                bookMarks = conf.BookMarks;
            }
            else
            {
                foreach (IValueProvider var in dump.Variables)
                {
                    if(var is Signal)
                        currentDump.Add(new My_Variable(var.Name, var.Name, var as Signal));
                }
                entity = new Entity();
            }
        }

        /// <summary>
        /// Загрузка VCD файла
        /// </summary>
        /// <param name="FileName"></param>
        public void LoadVCDFile(string FileName)
        {
            VCDReader vcd = new VCDReader(FileName);
            dump.Clear();

            Schematix.Core.UserControls.ProgressWindow.Window.RunProcess(vcd);
            
            dump = vcd.Dump;
            
            timescale = vcd.Timescale;
            summaryInfo = vcd.SummaryInfo;

            string confPath = System.IO.Path.ChangeExtension(FileName, "conf");
            LoadConfiguration(confPath);
        }

        /// <summary>
        /// Загрузка VCD файла
        /// </summary>
        /// <param name="FileName"></param>
        public void LoadVCDFile(Stream stream, string confPath)
        {
            VCDReader vcd = new VCDReader(stream);
            dump.Clear();

            Schematix.Core.UserControls.ProgressWindow.Window.RunProcess(vcd);

            dump = vcd.Dump;

            timescale = vcd.Timescale;
            summaryInfo = vcd.SummaryInfo;

            LoadConfiguration(confPath);
        }

        /// <summary>
        /// Сохранение VCD файла
        /// </summary>
        /// <param name="FileName"></param>
        public void SaveVCDFile(string FileName)
        {
            if ((IsModified == true) || (System.IO.File.Exists(FileName) == false))
            {
                VCDWriter vcd = new VCDWriter(dump, FileName);
                Schematix.Core.UserControls.ProgressWindow.Window.RunProcess(vcd);
            }
            
            string confPath = System.IO.Path.ChangeExtension(FileName, "conf");
            WaveformConfiguration.SaveConfiguration(this, confPath);

            isModified = false;
        }

        /// <summary>
        /// Сохранение VCD файла
        /// </summary>
        /// <param name="FileName"></param>
        public void SaveVCDFile(Stream stream, string confPath)
        {
            if (IsModified == true)
            {
                VCDWriter vcd = new VCDWriter(dump, stream);
                Schematix.Core.UserControls.ProgressWindow.Window.RunProcess(vcd);
            }

            WaveformConfiguration.SaveConfiguration(this, confPath);
            isModified = false;
        }

        /// <summary>
        /// Проанализировать VHDL файл и добавить все имеющиеся сигналы
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="ArchitectureName"></param>
        /// <param name="EntityName"></param>
        public void AnalyseVHDLFile(string FilePath, string ArchitectureName, string EntityName)
        {
            Parser.Parser parser = new Parser.Parser(FilePath);
            parser.Parse();
            foreach (Parser.Entity entity in parser.EntityList)
            {
                //мы нашли необходимый Entity
                if (entity.name.EndsWith(EntityName, StringComparison.InvariantCultureIgnoreCase))
                {
                    this.entity = entity;
                    break;
                }
            }
        }

        /// <summary>
        /// Проанализировать VHDL файл и добавить все имеющиеся сигналы
        /// </summary>
        public void AnalyseVHDLFile()
        {
            AnalyseVHDLFile(FileName, ArchitectureName, EntityName);
        }

        /// <summary>
        /// Можно ли редактировать открытую волновую диаграмму
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="ArchitectureName"></param>
        /// <param name="EntityName"></param>
        /// <returns></returns>
        public bool AllowEditForEntity(string FilePath, string ArchitectureName, string EntityName)
        {
            try
            {
                Parser.Parser parser = new Parser.Parser(FilePath);
                parser.Parse();
                foreach (Parser.Entity entity in parser.EntityList)
                {
                    //мы нашли необходимый Entity
                    if (entity.name.EndsWith(EntityName))
                    {
                        if (entity.Port_items.Count != 0)
                            return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Schematix.Core.Logger.Log.Error("Parser Error.", ex);
                MessageBox.Show(ex.Message, "Parser Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        /// <summary>
        /// Индикатор, что текущая временная диаграмма модифицирована
        /// </summary>
        private bool isModified = false;
        public bool IsModified
        {
            get { return isModified; }
            set { isModified = value; }
        }
        

        /// <summary>
        /// Генерация TestBench по полученной волновой диаграмме
        /// </summary>
        /// <param name="FilePath"></param>
        public void GenerateTestBench(string FilePath)
        {
            VHDLTestBenchGenerator vhdl_tb_gen = new VHDLTestBenchGenerator(this, new FileStream(FilePath, FileMode.Create), entity.name, entity.name);
            Schematix.Core.UserControls.ProgressWindow.Window.RunProcess(vhdl_tb_gen);
        }
        
        /// <summary>
        /// Возвращает итератор для простого просмотра данных
        /// </summary>
        /// <returns></returns>
        public IEnumerable<List<string []>> GetSimpleEnumerator()
        {
            return new SimpleIterator(this);
        }

        
        /// <summary>
        /// Возвращает итератор для просмотра записей в определенном временном интервале
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public IEnumerable<List<string []>> GetTimeBasegEnumerator(UInt64 startTime, UInt64 endTime)
        {
            return new TimeIterator(this, startTime, endTime);
        }
    }
}