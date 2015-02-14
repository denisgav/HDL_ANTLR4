using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Schematix.Core.UserControls;
using System.IO;
using Schematix.Waveform.TestBenchGenerator;
using Schematix.Waveform;

namespace Schematix.Core.Compiler
{
    public class GHDLCompiler : AbstractCompiler
    {
        /// <summary>
        /// Имеются ли ошибки во время разбора
        /// </summary>
        protected bool hasErrors = false;
        private string currentFile;

        private GHDLLibraryParser libraryParser;

        string GetLibraryFile()
        {
            return Path.Combine(LibraryDirectory, "work-obj93.cf");
        }

        public GHDLCompiler()
        {
            libraryParser = new GHDLLibraryParser();
        }

        public override SortedList<string, GHDLCompiledFile> ReparseLibrary()
        {
            return libraryParser.Reparse(GetLibraryFile());
        }

        public override Model.CodeFile AddCodeFile(string filePath)
        {
            throw new NotImplementedException();
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
            hasErrors = false;

            currentFile = filePath;
            filePath = GetRelativePath(currentFile);

            string startMessage = "Compile " + filePath;

            if (!GHDLCompile(filePath))
            {
                messages.Add(new DiagnosticMessage(startMessage + " ... Fail"));
                return false;
            }
            else
                messages.Add(new DiagnosticMessage(startMessage + " ... Ok"));

            return base.CompileOne(filePath);
        }

        public override bool CheckSyntaxOne(string fileName)
        {
            hasErrors = false;
            string startMessage = "Check syntax " + fileName;

            currentFile = fileName;
            fileName = GetRelativePath(currentFile);

            if (!GHDLCompile(fileName))
            {
                messages.Add(new DiagnosticMessage(startMessage + " ... Fail"));
                return false;
            }
            else
                messages.Add(new DiagnosticMessage(startMessage + " ... Ok"));

            return base.CheckSyntaxOne(fileName);
        }

        public override bool CreateDiagram(string vhdFile, string EntityName, string ArchitectureName, string vcdFile, bool noRun = false)
        {
            hasErrors = false;

            currentFile = vhdFile;
            vhdFile = GetRelativePath(currentFile);
            string startMessage = "Simulate " + vcdFile;
            if (!GHDLCompile(vhdFile))
            {
                messages.Add(new DiagnosticMessage(startMessage + " ... Fail"));
                return false;
            }
            else
                messages.Add(new DiagnosticMessage(startMessage + " ... Ok"));

            if (!GHDLSimulation(EntityName, ArchitectureName, GetRelativePath(vcdFile)))
            {
                messages.Add(new DiagnosticMessage(startMessage + " ... Fail"));
                return false;
            }
            else
                messages.Add(new DiagnosticMessage(startMessage + " ... Ok"));
            return true;
        }

        public override bool CreateTestBenchDiagram(string testBenchEntity, string testBenchArchitecture, string testBenchFilename, string file, string entity, string architecture, string outFile)
        {
            hasErrors = false;
            currentFile = file;
            file = GetRelativePath(currentFile);
            GHDLCompile(file);
            currentFile = testBenchFilename;
            testBenchFilename = GetRelativePath(currentFile);
            string startMessage = "Simulate " + file + " TestBench";


            if (!GHDLCompile(testBenchFilename))
            {
                messages.Add(new DiagnosticMessage(startMessage + " ... Fail"));
                return false;
            }
            else
                messages.Add(new DiagnosticMessage(startMessage + " ... Ok"));
            currentFile = testBenchFilename;

            

            if (!GHDLSimulation(testBenchEntity, testBenchArchitecture, GetRelativePath(outFile)))
            {
                messages.Add(new DiagnosticMessage(startMessage + " ... Fail"));
                return false;
            }
            else
                messages.Add(new DiagnosticMessage(startMessage + " ... Ok"));
            return true;
        }

        public override bool CreateTestBenchDiagram(string file, string entity, string architecture, string outFile, WaveformCore core)
        {
            core.SaveVCDFile(outFile);
            string testEntity = entity + "_testbench";
            string testArch = "testbench_architecture";

            string TestBenchFile = Path.GetDirectoryName(file) + "\\" + Path.GetFileNameWithoutExtension(file) + "_test_bench.vhd";

            core.GenerateTestBench(TestBenchFile);

            bool res = CreateTestBenchDiagram(testEntity, testArch, TestBenchFile, file, entity, architecture, outFile);
            if (res == false)
                return false;

            core.LoadVCDFile(outFile);
            return true;
        }

        private string GetRelativePath(string path)
        {
            return ".." + path.Substring(ProjectDirectory.Length);
        }

        /// <summary>
        /// Запуск процесса GHDL со специфическими аргументами
        /// </summary>
        /// <param name="AppName"></param>
        /// <param name="Switch"></param>
        /// <param name="args"></param>
        /// <param name="TaskName"></param>
        private bool CreateExecuteProcess(string AppName, string Switch, string args, string TaskName)
        {
            Process compileProcess = new Process();

            // Create process start up info object
            ProcessStartInfo psi = new ProcessStartInfo(AppName, Switch + args);
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            psi.WorkingDirectory = LibraryDirectory;

            compileProcess.StartInfo = psi;
            string erroroutput = string.Empty; ;

            Schematix.Core.UserControls.ProgressWindow.Window.RunProcess(
                    new MyBackgroundWorker(
                        new Action(() =>
                        {
                            isBusy = true;
                            // Start process
                            compileProcess.Start();
                            StreamReader stdout = compileProcess.StandardOutput;
                            string output = stdout.ReadToEnd();

                            StreamReader errout = compileProcess.StandardError;
                            erroroutput = errout.ReadToEnd();
                        }),
                        new Action(() =>
                        {
                            if (compileProcess != null)
                                compileProcess.Kill();
                            compileProcess.Close();
                            isBusy = false;
                        }), TaskName));

            if (compileProcess != null)
            {
                if (erroroutput.Length > 0)
                {
                    ParseErrorString(erroroutput);
                }
                if (!compileProcess.HasExited)
                {
                    compileProcess.Kill();
                    compileProcess.Close();
                }
            }
            foreach (DiagnosticMessage msg in messages)
                if (msg.MessageType == MessageWindow.MessageType.Error)
                    return false;
            return true;
        }

        /// <summary>
        /// Процесс выбора архитектуры
        /// </summary>
        /// <param name="_Entity"></param>
        /// <param name="_Architecture"></param>
        /// <returns></returns>
        public bool GHDLElaboration(string _Entity, string _Architecture)
        {
            // Clear collection with diagnostic compile messages
            messages.Clear();

            try
            {
                return CreateExecuteProcess("ghdl", "-e -fexplicit ", _Entity + " " + _Architecture, string.Format("GHDL Elaboration;\nEntity: {0}, Architecture: {1}", _Entity, _Architecture));
            }
            catch { return false; }
        }

        /// <summary>
        /// выполнение симуляции
        /// </summary>
        /// <param name="_Entity"></param>
        /// <param name="_Architecture"></param>
        /// <param name="_OutFile"></param>
        /// <param name="noRun"></param>
        /// <returns></returns>
        public bool GHDLSimulation(string _Entity, string _Architecture, string _OutFile, bool noRun = false)
        {
            messages.Clear();

            if (!GHDLElaboration(_Entity, _Architecture))
                return false;
            try
            {
                string arguments = string.Empty;
                if (noRun)
                    arguments = string.Format("{0} {1}  --no-run --vcd=\"{2}\"", _Entity, _Architecture, _OutFile);
                else
                    arguments = string.Format(" {0} {1}  --vcd=\"{2}\" --stack-size=128m --stack-max-size=256m ", _Entity, _Architecture, _OutFile);
                return CreateExecuteProcess("ghdl", "-r -fexplicit ", arguments, string.Format("Simulation of architecture {0}", _Architecture));
            }
            catch { return false; }
        }

        /// <summary>
        /// Выполнение компиляции
        /// </summary>
        /// <param name="_strFileToCompile"></param>
        /// <returns></returns>
        public bool GHDLCompile(string _strFileToCompile)
        {
            // Clear collection with diagnostic compile messages
            messages.Clear();

            try
            {
                return CreateExecuteProcess("ghdl", "-a -fexplicit ", string.Format("\"{0}\"", _strFileToCompile), string.Format("Compiling the file {0}", _strFileToCompile));
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Проверка синтаксиса
        /// </summary>
        /// <param name="_strFileToCheck"></param>
        /// <returns></returns>
        public bool GHDLCheckSyntax(string _strFileToCheck)
        {
            // Clear collection with diagnostic compile messages
            messages.Clear();

            try
            {
                return CreateExecuteProcess("ghdl", "-s -fexplicit ", string.Format("\"{0}\"", _strFileToCheck), string.Format("Check syntax of {0}", _strFileToCheck));
            }
            catch { /*Console.Text = "Some exception on compile.";*/return false; }
        }


        /// <summary>
        /// Распознавание сообщений от GHDL
        /// </summary>
        /// <param name="erroroutput"></param>
        private void ParseErrorString(string errorOutput)
        {
            GHDLErrorStringParser parser = new GHDLErrorStringParser(currentFile);
            parser.ParseErrorString(errorOutput);
            foreach (DiagnosticMessage msg in parser.DiagnosticMsgs)
                messages.Add(msg);
            base.NotifyCollectionChanged();
        }

        public override void ProcessCodeFile(Model.CodeFile file)
        {
            
        }

        public override void ProcessProject()
        {
        }
    }    
}