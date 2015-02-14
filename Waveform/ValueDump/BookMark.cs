using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schematix.Waveform.Value_Dump
{
    /// <summary>
    /// Заметка
    /// </summary>
    [Serializable]
    public class BookMark
    {
        /// <summary>
        /// Заголовок
        /// </summary>
        private string header;
        public string Header
        {
            get { return header; }
            set { header = value; }
        }

        /// <summary>
        /// Текстовое содержимое 
        /// </summary>
        private string text;
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        /// <summary>
        /// Положение заметки на временной шкале
        /// </summary>
        private UInt64 time;
        public UInt64 Time
        {
            get { return time; }
            set { time = value; }
        }

        /// <summary>
        /// Имя переменной
        /// </summary>
        private string variableName;
        public string VariableName
        {
            get { return variableName; }
            set { variableName = value; }
        }
        

        public BookMark()
            :this(string.Empty)
        {}

        public BookMark(string variableName)
        {
            this.variableName = variableName;
            header = "<header>";
            text = "<text>";
            time = 0;
        }

        public BookMark(string header, string text, string variableName, UInt64 time)
        {
            this.variableName = variableName;
            this.header = header;
            this.text = text;
            this.time = time;
        }
    }
}