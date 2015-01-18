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
using System.IO;

namespace VHDLRuntime.ValueDump
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
