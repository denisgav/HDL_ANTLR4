using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VHDL.parser;
using VHDL;
using Schematix.Core.Compiler;

namespace Schematix.Core.Model
{
    public class VHDL_CodeFile: CodeFile
    {
        /// <summary>
        /// Служебная информация о файле
        /// </summary>
        private LibraryFileInfo libraryFileInfo;
        public LibraryFileInfo LibraryFileInfo
        {
            get { return libraryFileInfo; }
            set { libraryFileInfo = value; }
        }

        /// <summary>
        /// Обновление служебной информации
        /// </summary>
        public void UpdateLibraryInfo()
        {
            libraryFileInfo = LibraryFileInfo.AnalyseText(text, filePath, LibraryName);
        }
        

        /// <summary>
        /// Распарсеный VHDL файл
        /// </summary>
        private VhdlFile file;
        public VhdlFile File
        {
            get { return file; }
            set 
            {
                file = value;
                if(file != null)
                    file.FilePath = filePath;
                NotifyPropertyChanged("SemanticErrors");
            }
        }


        /// <summary>
        /// Используемый компилятор
        /// </summary>
        private VHDLCompiler compiler;
        public override AbstractCompiler Compiler
        {
            get { return compiler; }
        }

        /// <summary>
        /// Сформировать список сообщений об ошибках
        /// </summary>
        /// <returns></returns>
        public override List<DiagnosticMessage> GetMessages()
        {
            List<DiagnosticMessage> messages = new List<DiagnosticMessage>();

            return messages;
        }

        public VHDL_CodeFile(string filePath, string libraryName, VHDLCompiler compiler)
            : base(filePath, libraryName, ModelingLanguage.VHDL)
        {
            this.compiler = compiler;
            libraryFileInfo = LibraryFileInfo.AnalyseText(text, filePath, LibraryName);
        }
    }
}
