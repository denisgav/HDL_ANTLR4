using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Specialized;
using System.Diagnostics;

using VHDL;
using VHDL.parser;
using VHDL.parser.util;
using VHDL.libraryunit;

using Schematix.Core.UserControls;
using Schematix.Core.Model;
using Schematix.Waveform;
using VHDL.ParseError;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using VHDL_ANTLR4;

namespace Schematix.Core.Compiler
{
    public class VHDLCompiler : AbstractCompiler
    {
        #region Parsing settings
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
        
        

        private VhdlParserSettings settings;
        public VhdlParserSettings Settings
        {
            get { return settings; }
            set { settings = value; }
        }

        private RootDeclarativeRegion rootScope;
        public RootDeclarativeRegion RootScope
        {
            get { return rootScope; }
            set { rootScope = value; }
        }

        private LibraryDeclarativeRegion currentLibrary;
        public LibraryDeclarativeRegion CurrentLibrary
        {
            get { return currentLibrary; }
            set { currentLibrary = value; }
        }

        private VHDL_Library_Manager libraryManager;
        public VHDL_Library_Manager LibraryManager
        {
            get { return libraryManager; }
        }

        #endregion

        private VhdlFile file;

        /// <summary>
        /// Добавление нового файла в проект
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public override CodeFile AddCodeFile(string filePath)
        {
            LibraryDeclarativeRegion lib = GetLibraryForFile(filePath);
            VHDL_CodeFile newCode = new VHDL_CodeFile(filePath, lib.Identifier, this);
            files.Add(newCode);
            return newCode;
        }


        /// <summary>
        /// Удаление файла с кодом с проекта
        /// </summary>
        /// <param name="filePath"></param>
        public override void RemoveCodeFile(string filePath)
        {
            CodeFile file = GetFileByPath(filePath);
            if (file != null)
            {
                files.Remove(file);
                currentLibrary.Files.Remove((file as VHDL_CodeFile).File);
            }
        }

        public override bool IsLockGUI
        {
            get
            {
                return false;
            }
        }


        public override bool CompileOne(string filePath)
        {
            string startMessage = "Compile " + filePath;
            bool success = true;

            Schematix.Core.UserControls.ProgressWindow.Window.RunProcess(
                new MyBackgroundWorker(
                    new Action(() =>
                    {
                        VhdlFile file = null;
                        try
                        {
                            file = VhdlParserWrapper.parseFile(filePath, libraryManager);
                        }
                        catch (vhdlParseException ex)
                        {
                            string message = string.Format("{0} {1}:{2} {3} {4} {5}", ex.FilePath, ex.Line, ex.CharPositionInLine, ex.OffendingSymbol.Text, ex.Message, ex.InnerException);
                            messages.Add(new DiagnosticMessage(message, new SourcePosition(ex.FilePath, ex.Line, ex.CharPositionInLine), MessageWindow.MessageType.Error));
                            success = false;
                        }
                        catch (vhdlSemanticException ex)
                        {
                            string message = string.Format("{0} {1}:{2} {3} {4} {5}", ex.FileName, ex.Context.Start.Line, ex.Context.Start.Column, ex.Context.GetText(), ex.Message, ex.InnerException);
                            messages.Add(new DiagnosticMessage(message, new SourcePosition(ex.FileName, ex.Context.Start.Line, ex.Context.Start.Column), MessageWindow.MessageType.Error));
                            Console.WriteLine("Parsing failed");
                            success = false;
                        }
                        catch (Exception ex)
                        {
                            string message = string.Format("{0} {1} {2} \n {3}", ex.Message, ex.InnerException, ex.Source, ex.StackTrace);
                            messages.Add(new DiagnosticMessage(message, false));
                            Console.WriteLine("Parsing failed");
                            success = false;
                        }
                        if (success)
                            Console.WriteLine("Parsing complete");

                        if (file != null)
                        {
                            string compiledFile = Path.ChangeExtension(filePath, "obj");
                            Stream stream = new FileStream(compiledFile, FileMode.Create);
                            BinaryFormatter serializer = new BinaryFormatter();
                            serializer.Serialize(stream, file);
                            stream.Close();
                        }
                    }),
                    new Action(() =>
                    {

                    }), string.Format("Compile {0}", filePath)));



            if (success)
            {
                messages.Add(new DiagnosticMessage(startMessage + " ... Fail"));
                return false;
            }
            else
                messages.Add(new DiagnosticMessage(startMessage + " ... Ok"));

            return base.CompileOne(filePath);
        }

        public override bool CheckSyntaxOne(string filePath)
        {
            string startMessage = "Check syntax of " + filePath;

            bool success = true;

            Schematix.Core.UserControls.ProgressWindow.Window.RunProcess(
                new MyBackgroundWorker(
                    new Action(() =>
                    {
                        try
                        {
                            file = VhdlParserWrapper.parseFile(filePath, libraryManager);
                        }
                        catch (vhdlParseException ex)
                        {
                            string message = string.Format("{0} {1}:{2} {3} {4} {5}", ex.FilePath, ex.Line, ex.CharPositionInLine, ex.OffendingSymbol.Text, ex.Message, ex.InnerException);
                            messages.Add(new DiagnosticMessage(message, new SourcePosition(ex.FilePath, ex.Line, ex.CharPositionInLine), MessageWindow.MessageType.Error));
                            success = false;
                        }
                        catch (vhdlSemanticException ex)
                        {
                            string message = string.Format("{0} {1}:{2} {3} {4} {5}", ex.FileName, ex.Context.Start.Line, ex.Context.Start.Column, ex.Context.GetText(), ex.Message, ex.InnerException);
                            messages.Add(new DiagnosticMessage(message, new SourcePosition(ex.FileName, ex.Context.Start.Line, ex.Context.Start.Column), MessageWindow.MessageType.Error));
                            Console.WriteLine("Parsing failed");
                            success = false;
                        }
                        catch (Exception ex)
                        {
                            string message = string.Format("{0} {1} {2} \n {3}", ex.Message, ex.InnerException, ex.Source, ex.StackTrace);
                            messages.Add(new DiagnosticMessage(message, false));
                            Console.WriteLine("Parsing failed");
                            success = false;
                        }
                        if (success)
                            Console.WriteLine("Parsing complete");

                        if (file != null)
                        {
                            string compiledFile = Path.ChangeExtension(filePath, "obj");
                            Stream stream = new FileStream(compiledFile, FileMode.Create);
                            BinaryFormatter serializer = new BinaryFormatter();
                            serializer.Serialize(stream, file);
                            stream.Close();
                        }
                    }),
                    new Action(() =>
                    {

                    }), string.Format("Check Syntax of {0}", filePath)));


            if (success)
            {
                messages.Add(new DiagnosticMessage(startMessage + " ... Fail"));
                return false;
            }
            else
                messages.Add(new DiagnosticMessage(startMessage + " ... Ok"));

            return base.CheckSyntaxOne(filePath);
        }



        public override bool CreateDiagram(string vhdFile, string EntityName, string ArchitectureName, string vcdFile, bool noRun = false)
        {
            VhdlFile file = null;
            foreach (CodeFile f in Files)
            {
                if (f is VHDL_CodeFile)
                {
                    if ((f as VHDL_CodeFile).FilePath.Equals(vhdFile))
                    {
                        file = (f as VHDL_CodeFile).File;
                        break;
                    }
                }
            }
            if (file == null)
                return false;

            Architecture arch = null;

            foreach (LibraryUnit unit in file.Elements)
            {
                if (unit is Architecture)
                {
                    if ((unit as Architecture).Identifier.Equals(ArchitectureName))
                    {
                        arch = (unit as Architecture);
                        break;
                    }
                }
            }
            if (arch == null)
                return false;

            if (arch.Entity.Identifier.EqualsIdentifier(EntityName) == false)
                return false;
            /*
            VHDLModelingSystem.ModelingSystemCore model = new VHDLModelingSystem.ModelingSystemCore(arch, currentLibrary, rootScope);
            model.Start();
            model.SaveToVCD(vcdFile);
            */
            return true;
        }

        public override bool CreateTestBenchDiagram(string testBenchEntity, string testBenchArchitecture, string testBenchFilename, string file, string entity, string architecture, string outFile)
        {
            return true;
        }

        public override bool CreateTestBenchDiagram(string vhdFile, string EntityName, string ArchitectureName, string vcdFile, WaveformCore core)
        {
            VhdlFile file = null;
            foreach (CodeFile f in Files)
            {
                if (f is VHDL_CodeFile)
                {
                    if ((f as VHDL_CodeFile).FilePath.Equals(vhdFile))
                    {
                        file = (f as VHDL_CodeFile).File;
                        break;
                    }
                }
            }
            if (file == null)
                return false;

            Architecture arch = null;

            foreach (LibraryUnit unit in file.Elements)
            {
                if (unit is Architecture)
                {
                    if ((unit as Architecture).Identifier.EqualsIdentifier(ArchitectureName))
                    {
                        arch = (unit as Architecture);
                        break;
                    }
                }
            }
            if (arch == null)
                return false;

            if (arch.Entity.Identifier.EqualsIdentifier(EntityName) == false)
                return false;
            /*
            VHDLModelingSystem.ModelingSystemCore model = new VHDLModelingSystem.ModelingSystemCore(arch, currentLibrary, rootScope, core.Dump);
            model.Start();
            model.SaveToVCD(vcdFile);
            */
            return true;
        }

        public override SortedList<string, GHDLCompiledFile> ReparseLibrary()
        {
            SortedList<string, GHDLCompiledFile> library = new SortedList<string, GHDLCompiledFile>();
            string head = string.Empty;
            foreach (CodeFile file in Files)
            {
                if (file is VHDL_CodeFile)
                {
                    GHDLCompiledFile ghdlFile = new GHDLCompiledFile();
                    library.Add(file.FilePath, ghdlFile);
                    head = file.FilePath;
                    VHDL_CodeFile vhdlCode = file as VHDL_CodeFile;
                    if (vhdlCode.File != null)
                    {
                        foreach (LibraryUnit unit in vhdlCode.File.Elements)
                        {
                            if (unit is Entity)
                            {
                                string tmp_entity_name = (unit as Entity).Identifier;
                                if (!library[head].vhdlStruct.ContainsKey(tmp_entity_name))
                                {
                                    library[head].vhdlStruct.Add(tmp_entity_name, new List<string>());
                                }
                            }
                            if (unit is Architecture)
                            {
                                string tmp_arch_name = (unit as Architecture).Identifier;
                                string tmp_entity_name = (unit as Architecture).Entity.Identifier;
                                if (library[head].vhdlStruct.ContainsKey(tmp_entity_name))
                                    if (!library[head].vhdlStruct[tmp_entity_name].Contains(tmp_arch_name))
                                        library[head].vhdlStruct[tmp_entity_name].Add(tmp_arch_name);
                            }
                        }
                    }
                }
            }
            return library;
        }


        public VHDLCompiler(string ProgramPath)
        {
            string appBase = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            VHDL.parser.Logger loggercompile = VHDL.parser.Logger.CreateLogger(appBase, "compiler");

            libraryManager = new VHDL_Library_Manager("", @"Libraries\LibraryRepository.xml", loggercompile);
            libraryManager.Logger.OnWriteEvent += new VHDL.parser.Logger.OnWriteDeleagte(Logger_OnWriteEvent);
            libraryManager.LoadData(@"Libraries");

            settings = VhdlParserWrapper.DEFAULT_SETTINGS;
            settings.AddPositionInformation = true;
            rootScope = new RootDeclarativeRegion();
            currentLibrary = new LibraryDeclarativeRegion("work");
            rootScope.Libraries.Add(currentLibrary);
            rootScope.Libraries.Add(libraryManager.GetLibrary("STD"));
        }

        static void Logger_OnWriteEvent(VHDL.parser.LoggerMessageVerbosity verbosity, string message)
        {
            Console.WriteLine(String.Format("[{0}] {1}", verbosity, message));
        }


        
        #region Parsing methods

        private void ShowErrorMessages()
        {
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
        }

        /// <summary>
        /// Распарсить входной поток
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="libraryScope"></param>
        /// <param name="tokenStream"></param>
        /// <param name="tree"></param>
        /// <returns></returns>
        private VhdlFile Parse(ICharStream stream, LibraryDeclarativeRegion libraryScope, out CommonTokenStream tokenStream, out IParseTree tree)
        {
            VhdlFile res = null;
            file = null;
            tree = null;
            tokenStream = null;
            lock (this)
            {
                try
                {                    
                    vhdlLexer lexer = new vhdlLexer(stream);

                    tokenStream = new CommonTokenStream(lexer);
                    vhdlParser parser = new vhdlParser(tokenStream);

                    //--------------------------------------------
                    //Optional - add listener
                    //vhdlListener listener = new vhdlListener();
                    //parser.AddParseListener(listener);
                    //--------------------------------------------
                    vhdlSemanticErrorListener vhdlSemanticErrorListener = new vhdlSemanticErrorListener(stream.SourceName);
                    parser.AddErrorListener(vhdlSemanticErrorListener);

                    tree = parser.design_file();
                    //Console.WriteLine(tree.ToStringTree(parser));
                    vhdlVisitor visitor = new vhdlVisitor(settings, rootScope, libraryScope, libraryManager) { FileName = stream.SourceName };
                    res = visitor.Visit(tree) as VhdlFile;
                }
                catch (vhdlSemanticException ex)
                {
                    parseSemanticException = ex;
                }
                catch (vhdlParseException ex)
                {
                    parseSyntaxException = ex;
                }
                catch (Exception ex)
                {
                    parseException = ex;
                }
            }
            ShowErrorMessages();
            return res;
        }

        /// <summary>
        /// Распарсить файл по адрессу filePath без создания параллельного потока
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="tokenStream"></param>
        /// <param name="tree"></param>
        /// <returns></returns>
        public VhdlFile ParseFile(string filePath, out CommonTokenStream tokenStream, out IParseTree tree)
        {
            VhdlFile file = null;
            lock (this)
            {
                if (File.Exists(filePath) == false)
                {
                    parseException = new Exception(string.Format("File {0} not found", filePath));
                    tree = null;
                    tokenStream = null;
                    return null;
                }

                LibraryDeclarativeRegion libraryScope = GetLibraryForFile(filePath);
                // Start process
                ICharStream stream = new CaseInsensitiveFileStream(filePath);
                file = Parse(stream, libraryScope, out tokenStream, out tree);
            }
            return file;
        }

        /// <summary>
        /// Провести семантический анализ текста
        /// </summary>
        /// <param name="text"></param>
        /// <param name="libraryScope"></param>
        /// <param name="tokenStream"></param>
        /// <param name="tree"></param>
        /// <returns></returns>
        public VhdlFile ParseText(string text, string filePath, out CommonTokenStream tokenStream, out IParseTree tree)
        {
            VhdlFile file = null;
            lock (this)
            {
                // Start process
                ICharStream stream = new CaseInsensitiveStringStream(text, filePath);
                file = Parse(stream, currentLibrary, out tokenStream, out tree);
            }
            return file;
        }

        /// <summary>
        /// Если обьект класса  CodeFile является VHDL_CodeFile,
        /// то провести его обработку и встроить в общую библиотеку,
        /// новый поток не создается,
        /// ошибки выписываются в обьект file
        /// </summary>
        /// <param name="file"></param>
        public override void ProcessCodeFile(CodeFile file)
        {
            try
            {
                lock (this)
                {
                    isBusy = true;
                    if ((file is VHDL_CodeFile) && ((file as VHDL_CodeFile).LibraryName == "work"))
                    {
                        VHDL_CodeFile vhdlCode = (file as VHDL_CodeFile);

                        vhdlCode.ParseException = null;
                        vhdlCode.ParseSyntaxException = null;
                        vhdlCode.ParseSemanticException = null;

                        VhdlFile oldFile = null;
                        foreach (VhdlFile f in currentLibrary.Files)
                        {
                            if (f.FilePath == file.FilePath)
                            {
                                oldFile = f;
                                break;
                            }
                        }
                        if (oldFile != null)
                            currentLibrary.Files.Remove(oldFile);

                        IParseTree tree;
                        CommonTokenStream tokenStream;
                        ICharStream stream = new CaseInsensitiveStringStream(file.Text, file.FilePath);
                        VhdlFile ParsedFile = Parse(stream, currentLibrary, out tokenStream, out tree);
                        if (ParsedFile != null)
                        {
                            ParsedFile.FilePath = file.FilePath;
                        }

                        vhdlCode.File = ParsedFile;
                        vhdlCode.ParseSemanticException = parseSemanticException;
                        vhdlCode.ParseSyntaxException = parseSyntaxException;
                        vhdlCode.ParseException = parseException;
                        vhdlCode.TokenStream = tokenStream;
                        vhdlCode.Tree = tree;

                        vhdlCode.LibraryFileInfo = LibraryFileInfo.AnalyseText(vhdlCode.Text, vhdlCode.FilePath, vhdlCode.LibraryName);

                        foreach (CodeFile f in vhdlCode.Dependencies)
                            f.NeedForUpdate = true;

                        SetDependencies();
                    }
                    base.ProcessCodeFile(file);
                    isBusy = false;
                }
            }
            catch (Exception ex)
            {
                Messages.Add(new DiagnosticMessage(string.Format("Message: {0}\n Source: {1}\n StackTrace: {2}\n HelpLink: {3}", ex.Message, ex.Source, ex.StackTrace, ex.HelpLink)));
                NotifyCollectionChanged();
            }
        }
        #endregion

        /// <summary>
        /// Определить обьект класса LibraryDeclarativeRegion по пути к файлу
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private LibraryDeclarativeRegion GetLibraryForFile(string filePath)
        {
            DirectoryInfo dir = Directory.GetParent(filePath);
            string libraryName = dir.Name;
            if (dir.FullName.Contains(SourceDirectory))
                return currentLibrary;
            else
                return this.libraryManager.GetLibrary(libraryName);
        }

        /// <summary>
        /// Начальная обработка проекта
        /// </summary>
        public override void ProcessProject()
        {
            SetDependencies();
            List<CodeFile> compileQueue = CreateQueue();
            foreach (CodeFile file in compileQueue)
                ProcessCodeFile(file);
            NotifyCollectionChanged();
        }

        /// <summary>
        /// Установление связей между файлами
        /// </summary>
        private void SetDependencies()
        {
            foreach (CodeFile f1 in Files)
                f1.Dependencies.Clear();
            foreach (CodeFile f1 in Files)
                foreach (CodeFile f2 in Files)
                {
                    if ((f1 != f2) && (f1 is VHDL_CodeFile) && (f2 is VHDL_CodeFile))
                    {
                        VHDL_CodeFile vf1 = (f1 as VHDL_CodeFile);
                        VHDL_CodeFile vf2 = (f2 as VHDL_CodeFile);
                        if ((vf1.LibraryFileInfo.IsDependedTo(vf2.LibraryFileInfo) == true))
                            f1.Dependencies.Add(f2);
                    }
                }
        }

        /// <summary>
        /// Организация очереди для компиляции
        /// </summary>
        public override List<CodeFile> CreateQueue()
        {
            List<CodeFile> tmp = new List<CodeFile>(Files);
            List<CodeFile> compileQueue = new List<CodeFile>();

            foreach (CodeFile file in tmp)
                if (file.Dependencies.Count == 0)
                    compileQueue.Add(file);

            foreach (CodeFile file in compileQueue)
                tmp.Remove(file);

            while (tmp.Count != 0)
            {
                CodeFile fileToRemove = null;
                foreach (CodeFile file in tmp)
                {
                    bool include = true;
                    foreach (CodeFile dep in file.Dependencies)
                        if (compileQueue.Contains(dep) == false)
                            include = false;

                    if (include == true)
                    {
                        compileQueue.Add(file);
                        fileToRemove = file;
                        break;
                    }
                }
                if (fileToRemove != null)
                    tmp.Remove(fileToRemove);
                else
                {
                    compileQueue.AddRange(tmp);
                    tmp.Clear();
                }
            }
            return compileQueue;
        }
        
    }
}
