using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VHDL.parser;
using VHDL;
using Schematix.Core.Compiler;
using VHDL.ParseError;
using Antlr4.Runtime.Tree;
using Antlr4.Runtime;

namespace Schematix.Core.Model
{
    public class VHDL_CodeFile: CodeFile
    {
        #region exceptions
        private vhdlParseException parseSyntaxException;
        public vhdlParseException ParseSyntaxException
        {
            get { return parseSyntaxException; }
            set { parseSyntaxException = value; }
        }
        private vhdlSemanticException parseSemanticException;
        public vhdlSemanticException ParseSemanticException
        {
            get { return parseSemanticException; }
            set { parseSemanticException = value; }
        }
        private Exception parseException;
        public Exception ParseException
        {
            get { return parseException; }
            set { parseException = value; }
        }
        #endregion
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

        private IParseTree tree;
        public IParseTree Tree
        {
            get { return tree; }
            set { tree = value; }
        }

        private CommonTokenStream tokenStream;
        public CommonTokenStream TokenStream
        {
            get { return tokenStream; }
            set { tokenStream = value; }
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

            if (parseSyntaxException != null)
            {
                Console.WriteLine(parseSyntaxException.Message);
                messages.Add(new DiagnosticMessage(parseSyntaxException.Message, new SourcePosition(parseSyntaxException.FilePath, parseSyntaxException.Line, parseSyntaxException.CharPositionInLine), MessageWindow.MessageType.Error));
            }
            if (parseSemanticException != null)
            {
                Console.WriteLine(parseSemanticException.Message);
                messages.Add(new DiagnosticMessage(parseSemanticException.Message, new SourcePosition(parseSemanticException.FileName, parseSemanticException.Context.Start.Line, parseSemanticException.Context.Start.Column), MessageWindow.MessageType.Error));
            }
            if (parseException != null)
            {
                messages.Add(new DiagnosticMessage(parseException.Message, false));
            }

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
