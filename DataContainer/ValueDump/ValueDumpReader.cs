using System;
using DataContainer;
using System.Windows;

namespace DataContainer.ValueDump
{
    public abstract class ValueDumpReader
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

        #region Properties

        /// <summary>
        /// Дата генерации волновой диаграммы
        /// </summary>
        protected string date;
        public string Date
        {
            get
            {
                return date;
            }
        }

        /// <summary>
        /// Временная шкала
        /// </summary>
        protected Timescale timescale;
        public Timescale Timescale
        {
            get
            {
                return timescale;
            }
        }

        /// <summary>
        /// Версия ПО которое сгенерировало VCD файл
        /// </summary>
        protected string version;
        public string Version
        {
            get
            {
                return version;
            }
        }

        #endregion

        /// <summary>
        /// Функция, которая должна получать данные о дампе с потока
        /// она будет перегружена дочерними классами
        /// </summary>
        /// <param name="stream"></param>
        public abstract void Parse();

        protected System.IO.Stream stream;

        public ValueDumpReader(System.IO.Stream stream)
        {
            this.stream = stream;
            dump = new SimulationScope("Root", null);
        }

        public ValueDumpReader(String FileName)
        {
            try
            {
                stream = new System.IO.FileStream(FileName, System.IO.FileMode.Open);
                dump = new SimulationScope("Root", null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Message: {0}\n StackTrace: {1}\n TargetSite: {2}\n Source: {3}", ex.Message, ex.StackTrace, ex.TargetSite, ex.Source), "Error :(", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        

        /// <summary>
        /// Общая информация о загруженном файле
        /// </summary>
        public abstract string SummaryInfo
        {
            get;
        }
    }
}
