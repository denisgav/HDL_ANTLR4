using System;
using System.IO;
using DataContainer;

namespace DataContainer.ValueDump
{
    public  abstract class ValueDumpWriter
    {
        /// <summary>
        /// Хранимый дамп памяти
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

        #region properties
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
            set
            {
                date = value;
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
            set
            {
                timescale = value;
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
            set
            {
                version = value;
            }
        }
        #endregion

        /// <summary>
        /// Поток, используемый для записи
        /// </summary>
        protected Stream stream;

        public ValueDumpWriter(SimulationScope dump, String FileName, Timescale Timescale, string Date, string Version)
        {
            this.version = Version;
            this.timescale = Timescale;
            this.date = Date;
            this.stream = new FileStream(FileName, System.IO.FileMode.Create, FileAccess.Write, FileShare.None, 2 << 24, FileOptions.None);
            this.dump = dump;
        }

        public ValueDumpWriter(SimulationScope dump, System.IO.Stream stream, Timescale Timescale, string Date, string Version)
        {
            this.version = Version;
            this.timescale = Timescale;
            this.date = Date;
            this.stream = stream;
            this.dump = dump;
        }

        public ValueDumpWriter(SimulationScope dump, Stream stream)
        {
            timescale = new Timescale(1, TimeUnit.fs);
            Date = DateTime.Now.ToString();
            version = System.Reflection.Assembly.GetExecutingAssembly().FullName;
            this.stream = stream;
            this.dump = dump;
        }

        public ValueDumpWriter(SimulationScope dump, String FileName)
        {
            timescale = new Timescale(1, TimeUnit.fs);
            Date = DateTime.Now.ToString();
            version = System.Reflection.Assembly.GetExecutingAssembly().FullName;
            stream = new FileStream(FileName, System.IO.FileMode.Create, FileAccess.Write, FileShare.None, 2 << 24, FileOptions.None);
            this.dump = dump;
        }

        /// <summary>
        /// Абстрактная функция для записи в файл
        /// </summary>
        public abstract void Write();
    }
}
