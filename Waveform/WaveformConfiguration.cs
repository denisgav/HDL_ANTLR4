using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Schematix.Waveform.Value_Dump;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using Parser;
using System.Windows;
using System.Collections.ObjectModel;
using DataContainer;

namespace Schematix.Waveform.Configuration
{
    [Serializable()]
    public class WaveformConfiguration
    {
        /// <summary>
        /// Параметры переменных
        /// </summary>
        private List<My_VariableConfiguration> variablesConfiguration;
        public List<My_VariableConfiguration> VariablesConfiguration
        {
            get { return variablesConfiguration; }
            set { variablesConfiguration = value; }
        }

        /// <summary>
        /// Момент времени, который определяет начало отображаемого участка
        /// </summary>
        private UInt64 visibleStartTime;
        public UInt64 VisibleStartTime
        {
            get { return visibleStartTime; }
            set { visibleStartTime = value; }
        }

        /// <summary>
        /// размер видимого промежутка времени
        /// </summary>
        private UInt64 visibleTimeDiapasone;
        public UInt64 VisibleTimeDiapasone
        {
            get { return visibleTimeDiapasone; }
            set { visibleTimeDiapasone = value; }
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
        /// Список пометок
        /// </summary>
        private ObservableCollection<BookMark> bookMarks;
        public ObservableCollection<BookMark> BookMarks
        {
            get { return bookMarks; }
            set { bookMarks = value; }
        }

        public WaveformConfiguration(WaveformCore core)
        {
            entity = core.Entity;
            entityName = core.EntityName;
            architectureName = core.ArchitectureName;
            fileName = core.FileName;
            bookMarks = core.BookMarks;
            variablesConfiguration = new List<My_VariableConfiguration>();
            visibleStartTime = core.ScaleManager.VisibleStartTime;
            visibleTimeDiapasone = core.ScaleManager.VisibleTimeDiapasone;
            foreach(My_Variable variable in core.CurrentDump)
            {
                variablesConfiguration.Add(new My_VariableConfiguration(variable));
            }
        }

        public WaveformConfiguration()
        {
            variablesConfiguration = new List<My_VariableConfiguration>();
        }

        /// <summary>
        /// Загрузка файла с конфигурацией
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static WaveformConfiguration LoadConfiguration(String FileName)
        {
            if (System.IO.File.Exists(FileName) == false)
                return null;
            WaveformConfiguration conf = null;
            FileStream stream = null;
            try
            {
                IFormatter formatter = new BinaryFormatter();
                stream = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.None);
                conf = formatter.Deserialize(stream) as WaveformConfiguration;
            }
            catch (Exception ex)
            {
            }
            finally 
            {
                if(stream != null)
                    stream.Close();
            }
            return conf;
        }

        /// <summary>
        /// Сохранение файла с конфигурацией
        /// </summary>
        /// <param name="core"></param>
        /// <param name="FileName"></param>
        public static void SaveConfiguration(WaveformCore core, string FileName)
        {
            WaveformConfiguration conf = new WaveformConfiguration(core);
            BinaryFormatter bformatter = new BinaryFormatter();
            FileStream file = null;
            try
            {
                file = new FileStream(FileName, FileMode.OpenOrCreate);
                bformatter.Serialize(file, conf);
            }
            catch (Exception ex)
            { }
            finally
            {
                if(file != null)
                    file.Close();
            }
        }
    }

    [Serializable()]
    public class My_VariableConfiguration
    {
        /// <summary>
        /// Id сигнала
        /// </summary>
        private UInt64 idx;
        public UInt64 Idx
        {
            get { return idx; }
        }

        /// <summary>
        /// Представление данных
        /// </summary>
        private DataRepresentation dataRepresentation;
        public DataRepresentation DataRepresentation
        {
            get
            {
                return dataRepresentation;
            }
            set
            {
                dataRepresentation = value;
            }
        }

        public My_VariableConfiguration() { }

        public My_VariableConfiguration(My_Variable variable)
        {
            idx = variable.Signal.Idx;
            dataRepresentation = variable.DataRepresentation;
        }
    }
}
