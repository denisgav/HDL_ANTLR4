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

namespace VHDLRuntime.ValueDump
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
            stream = new System.IO.FileStream(FileName, System.IO.FileMode.Open);
            dump = new SimulationScope("Root", null);            
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
