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
    /// <summary>
    /// Комбинированный компилятор (используются встроенные возможности),
    /// моделирование проводится на GHDL
    /// </summary>
    public class CombinedCompiler : VHDLCompiler
    {
        /// <summary>
        /// Имеются ли ошибки во время разбора
        /// </summary>
        protected bool hasErrors = false;
        private string currentFile;

        private GHDLLibraryParser libraryParser;

        private string GetLibraryFile()
        {
            return Path.Combine(LibraryDirectory, "work-obj93.cf");
        }

        /// <summary>
        /// Сформировать относительный путь для среды моделирования
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private string FormRelativePath(string filePath)
        {
            string res = filePath;
            res = res.Replace(LibraryDirectory, "");
            if (res[0] == '\\')
                res = res.Substring(1);

            return res;
        }

        public CombinedCompiler(string ProgramPath)
            : base(ProgramPath)
        {
            libraryParser = new GHDLLibraryParser();
            ErrorStringBuilder = new StringBuilder();
        }

        public override SortedList<string, GHDLCompiledFile> ReparseLibrary()
        {
            return libraryParser.Reparse(GetLibraryFile());
        }

        /// <summary>
        /// Если запустить компилятор, графический интерфейс пользователя заблокируется
        /// Необходимо выполнять процесс в отдельном потоке
        /// </summary>
        public override bool IsLockGUI
        {
            get { return ProcessInterface != null; }
        }

        public override bool CompileOne(string filePath)
        {
            hasErrors = false;

            currentFile = filePath;
            filePath = FormRelativePath(currentFile);

            string startMessage = "Compile " + filePath;

            if (!GHDLCompile(filePath))
            {
                messages.Add(new DiagnosticMessage(startMessage + " ... Fail"));
                return false;
            }
            else
                messages.Add(new DiagnosticMessage(startMessage + " ... Ok"));
            return true;
        }

        public override bool CheckSyntaxOne(string fileName)
        {
            hasErrors = false;
            string startMessage = "Check syntax " + fileName;

            currentFile = fileName;
            fileName = FormRelativePath(currentFile);

            if (!GHDLCheckSyntax(fileName))
            {
                messages.Add(new DiagnosticMessage(startMessage + " ... Fail"));
                return false;
            }
            else
                messages.Add(new DiagnosticMessage(startMessage + " ... Ok"));
            return true;
        }

        /// <summary>
        /// Очистить проект
        /// </summary>
        /// <returns></returns>
        public override bool CleanProject()
        {
            // Clear collection with diagnostic compile messages
            messages.Clear();

            try
            {
                string Arguments = string.Empty;
                string AppName = string.Empty;
                CommonProperties.Configuration.CurrentConfiguration.GHDLOptions.FormCleanCommand(GetLibraryFile(), out AppName, out Arguments);
                return CreateExecuteProcess(AppName, Arguments, string.Format("Cleaning of the project"));
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(string.Format("Clean project Error. Message: {0}", ex.Message), "Error ocrred", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                Schematix.Core.Logger.Log.Error("Clean project error.", ex);
                isBusy = false;
                return false;
            }
        }

        public override bool CreateDiagram(string vhdFile, string EntityName, string ArchitectureName, string vcdFile, bool noRun = false)
        {
            hasErrors = false;

            currentFile = vhdFile;
            vhdFile = FormRelativePath(currentFile);
            string startMessage = "Simulate " + vcdFile;

            if (!GHDLCompile(vhdFile))
            {
                messages.Add(new DiagnosticMessage(startMessage + " ... Fail"));
                return false;
            }
            else
                messages.Add(new DiagnosticMessage(startMessage + " ... Ok"));


            if (!GHDLSimulation(EntityName, ArchitectureName, vcdFile))
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
            file = FormRelativePath(currentFile);
            GHDLCompile(file);

            currentFile = testBenchFilename;
            testBenchFilename = FormRelativePath(currentFile);
            string startMessage = "Simulate " + file + " TestBench";

            if (!GHDLCompile(testBenchFilename))
            {
                messages.Add(new DiagnosticMessage(startMessage + " ... Fail"));
                return false;
            }
            else
                messages.Add(new DiagnosticMessage(startMessage + " ... Ok"));


            if (!GHDLSimulation(testBenchEntity, testBenchArchitecture, outFile))
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

        /// <summary>
        /// Запуск процесса GHDL со специфическими аргументами
        /// </summary>
        /// <param name="AppName"></param>
        /// <param name="Switch"></param>
        /// <param name="args"></param>
        /// <param name="TaskName"></param>
        private bool CreateExecuteProcess(string AppName, string args, string TaskName)
        {
            ErrorStringBuilder.Clear();
            if (ProcessInterface != null)
            {
                return ExecuteProcess(ProcessInterface, AppName, args, TaskName);
            }
            else
            {
                return ExecuteProcess(AppName, args, TaskName);
            }
        }

        /// <summary>
        /// Создание объекта ProcessStartInfo
        /// </summary>
        /// <param name="AppName"></param>
        /// <param name="Switch"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private ProcessStartInfo CreateProcessStartInfo(string AppName, string args)
        {
            // Create process start up info object
            ProcessStartInfo psi = new ProcessStartInfo(AppName, args);
            psi.WorkingDirectory = this.LibraryDirectory;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;

            try
            {
            	if(Schematix.CommonProperties.Configuration.CurrentConfiguration.GHDLOptions.IsEnvirPathUsed == false)
                	WriteRegKey(AppName);
            }
            catch (Exception ex)
            {
                Schematix.Core.Logger.Log.Error("Create process error.", ex);
                //System.Windows.MessageBox.Show(string.Format("Message: {0}\n\rStackTrace: {1}\n\rSource: {2}", ex.Message, ex.StackTrace, ex.Source), "Error ocrred", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }

            return psi;
        }

        private void WriteRegKey(string AppName)
        {
            string GHDLInstallDir = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(AppName));

            Microsoft.Win32.RegistryKey LocalMachineKey = Microsoft.Win32.Registry.LocalMachine;
            if (LocalMachineKey != null)
            {
                Microsoft.Win32.RegistryKey SoftwareKey = LocalMachineKey.OpenSubKey("Software");
                if (SoftwareKey != null)
                {
                    Microsoft.Win32.RegistryKey GhdlKey = SoftwareKey.OpenSubKey("Ghdl");
                    if (GhdlKey == null)
                    {
                        GhdlKey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Ghdl");
                    }
                    if (GhdlKey != null)
                    {
                        GhdlKey.SetValue("Install_Dir", GHDLInstallDir);
                        GhdlKey.Close();
                    }
                    SoftwareKey.Close();
                }
                LocalMachineKey.Close();
            }
        }

        /// <summary>
        /// Просто выполнить процесс
        /// </summary>
        /// <param name="AppName"></param>
        /// <param name="Switch"></param>
        /// <param name="args"></param>
        /// <param name="TaskName"></param>
        /// <returns></returns>
        private bool ExecuteProcess(string AppName, string args, string TaskName)
        {
            Process compileProcess = new Process();
            ProcessStartInfo psi = CreateProcessStartInfo(AppName, args);
            compileProcess.StartInfo = psi;
            string erroroutput = string.Empty;

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
        /// Запустить внешний процесс, используя procInterface
        /// </summary>
        /// <param name="procInterface"></param>
        /// <param name="AppName"></param>
        /// <param name="Switch"></param>
        /// <param name="args"></param>
        /// <param name="TaskName"></param>
        /// <returns></returns>
        private bool ExecuteProcess(ProcessInterface.ProcessInterface procInterface, string AppName, string args, string TaskName)
        {
            isBusy = true;
            ProcessStartInfo psi = CreateProcessStartInfo(AppName, args);
            string erroroutput = string.Empty;

            // Start process
            bool res = procInterface.StartProcess(psi);
            procInterface.WaitForExitProcess();

            try
            {
                erroroutput = ErrorStringBuilder.ToString();

                if (erroroutput.Length > 0)
                {
                    res = false;
                    ParseErrorString(erroroutput);
                }
            }
            catch (Exception ex)
            {
                Schematix.Core.Logger.Log.Error("Execute process error.", ex);
                System.Windows.MessageBox.Show(string.Format("Execute process error. Message: {0}", ex.Message), "Error ocrred", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                isBusy = false;
            }
            isBusy = false;

            return res;
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
                string Arguments = string.Empty;
                string AppName = string.Empty;
                CommonProperties.Configuration.CurrentConfiguration.GHDLOptions.FormElaborateCommand(_Entity, _Architecture, GetLibraryFile(), out AppName, out Arguments);

                return CreateExecuteProcess(AppName, Arguments, string.Format("GHDL Elaboration;\nEntity: {0}, Architecture: {1}", _Entity, _Architecture));
            }
            catch (Exception ex)
            {
                Schematix.Core.Logger.Log.Error("Elaboration process error.", ex);
                System.Windows.MessageBox.Show(string.Format("Elaboration process error. Message: {0}", ex.Message), "Error ocrred", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                isBusy = false;
                return false;
            }
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
                string Arguments = string.Empty;
                string AppName = string.Empty;
                CommonProperties.Configuration.CurrentConfiguration.GHDLOptions.FormSimulationCommand(_Entity, _Architecture, _OutFile, GetLibraryFile(), out AppName, out Arguments);

                return CreateExecuteProcess(AppName, Arguments, string.Format("Simulation of architecture {0}", _Architecture));
            }
            catch (Exception ex)
            {
                Schematix.Core.Logger.Log.Error("Simulation process error.", ex);
                System.Windows.MessageBox.Show(string.Format("Simulation process error. Message: {0}", ex.Message), "Error ocrred", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                isBusy = false;
                return false;
            }
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
                string Arguments = string.Empty;
                string AppName = string.Empty;
                CommonProperties.Configuration.CurrentConfiguration.GHDLOptions.FormCompileCommand(_strFileToCompile, GetLibraryFile(), out AppName, out Arguments);
                return CreateExecuteProcess(AppName, Arguments, string.Format("Compiling the file {0}", _strFileToCompile));
            }
            catch (Exception ex)
            {
                Schematix.Core.Logger.Log.Error("Compilation process error.", ex);
                System.Windows.MessageBox.Show(string.Format("Compilation process error. Message: {0}", ex.Message), "Error ocrred", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                isBusy = false;
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
                string Arguments = string.Empty;
                string AppName = string.Empty;
                CommonProperties.Configuration.CurrentConfiguration.GHDLOptions.FormSyntaxAnalyseCommand(_strFileToCheck, GetLibraryFile(), out AppName, out Arguments);
                return CreateExecuteProcess(AppName, Arguments, string.Format("Check syntax of {0}", _strFileToCheck));
            }
            catch (Exception ex)
            {
                Schematix.Core.Logger.Log.Error("CheckSyntax process error.", ex);
                System.Windows.MessageBox.Show(string.Format("CheckSyntax process error. Message: {0}", ex.Message), "Error ocrred", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                isBusy = false;
                return false;
            }
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

        protected override void processInterface_OnProcessExit(object sender, ProcessInterface.ProcessEventArgs args)
        {

        }

        StringBuilder ErrorStringBuilder;
        protected override void processInterface_OnProcessError(object sender, ProcessInterface.ProcessEventArgs args)
        {
            base.processInterface_OnProcessError(sender, args);
            ErrorStringBuilder.Append(args.Content);
        }
    }
}
