using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;
using Schematix.Core.Compiler;

namespace Schematix.Core.Model
{
    /// <summary>
    /// Класс, представляющий информацию об исходном коде файла
    /// </summary>
    public abstract class CodeFile : INotifyPropertyChanged
    {
        /// <summary>
        /// Путь к файлу
        /// </summary>
        protected string filePath;
        public string FilePath
        {
            get { return filePath; }
            set 
            {
                filePath = value;
                NotifyPropertyChanged("FilePath");
            }
        }

        /// <summary>
        /// Зависимости от других файлов
        /// </summary>
        private List<CodeFile> dependencies;
        public List<CodeFile> Dependencies
        {
            get { return dependencies; }
            set { dependencies = value; }
        }
        

        /// <summary>
        /// Флаг, указывающий необходимость повторного парсинга файла
        /// </summary>
        private bool needForUpdate;
        public bool NeedForUpdate
        {
            get { return needForUpdate; }
            set { needForUpdate = value; }
        }
        

        /// <summary>
        /// Имя библиотеки
        /// </summary>
        protected string libraryName;
        public string LibraryName
        {
            get { return libraryName; }
            set 
            {
                libraryName = value;
                NotifyPropertyChanged("LibraryName");
            }
        }

        /// <summary>
        /// Текст
        /// </summary>
        protected string text;
        public string Text
        {
            get { return text; }
            set 
            {
                text = value;
                NotifyPropertyChanged("Text");
            }
        }

        private ModelingLanguage languade;
        public ModelingLanguage Language
        {
            get { return languade; }
        }


        public CodeFile(string filePath, string libraryName, ModelingLanguage languade)
        {
            this.languade = languade;
            this.filePath = filePath;
            this.libraryName = libraryName;
            if (File.Exists(filePath) == false)
                throw new FileNotFoundException("File Not Found :(", filePath);
            else
                text = File.ReadAllText(filePath);

            needForUpdate = false;
            dependencies = new List<CodeFile>();
        }

        /// <summary>
        /// Используемый для данного файла компилятор
        /// </summary>
        public abstract AbstractCompiler Compiler
        {
            get;
        }

        /// <summary>
        /// Получить все диагностический сообщения относительно файла
        /// </summary>
        /// <returns></returns>
        public abstract List<DiagnosticMessage> GetMessages();

        
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion
    }
}
