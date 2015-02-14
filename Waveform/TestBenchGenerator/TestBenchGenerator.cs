using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Schematix.Waveform.TestBenchGenerator
{
    /// <summary>
    /// Базовый класс для генератора TestBench
    /// </summary>
    public abstract class TestBenchGenerator
    {
        /// <summary>
        /// Данные для сохранения
        /// </summary>
        protected WaveformCore core;
        public WaveformCore Core
        {
            get { return core; }
            set { core = value; }
        }

        /// <summary>
        /// Поток, в который выводить данные
        /// </summary>
        protected Stream stream;
        public Stream Stream
        {
            get { return stream; }
            set { stream = value; }
        }

        public TestBenchGenerator(WaveformCore core, Stream stream)
        {
            this.core = core;
            this.stream = stream;
        }

        /// <summary>
        /// Сохранение данных
        /// </summary>
        public abstract void Save();
        
    }
}
