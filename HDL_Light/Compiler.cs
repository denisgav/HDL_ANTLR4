using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using Schematix.Core.Compiler;
using Schematix.Core;
using Schematix.Core.Model;
using Schematix.Waveform;
using Schematix.Core.UserControls;
using Schematix.ProjectExplorer;

namespace Schematix
{    
    public class Compiler : IDisposable
    {
        private AbstractCompiler compiler;
        public AbstractCompiler CurrentCompiler
        {
            get { return compiler; }
            set { compiler = value; }
        }

        private SchematixCore core;
        public SchematixCore Core
        {
            get { return core; }
        }

        private Project project;
        public Project Project
        {
            get { return project; }            
        }
        

        /// <summary>
        /// Текущая папка проекта
        /// </summary>
        public string ProjectDirectory
        {
            get 
            {
                return project.RootFolder.Path; 
            }
        }

        /// <summary>
        /// Имеются ли ошибки
        /// </summary>
        /// <returns></returns>
        private bool HasErrors()
        {
            foreach (DiagnosticMessage msg in compiler.Messages)
            {
                if (msg.MessageType == MessageWindow.MessageType.Error)
                    return true;
            }
            return false;
        }

        public Compiler(ModelingLanguage ModelingLanguage, Project project)
            : this(ModelingLanguage, project, SchematixCore.Core)
        { }

        public Compiler(ModelingLanguage ModelingLanguage, Project project, SchematixCore core)
        {
            this.project = project;
            this.core = core;
            LoadCompiler(ModelingLanguage);
        }

        /// <summary>
        /// Использовать объект ProcessInterface для запуска внешнего приложения
        /// </summary>
        public ProcessInterface.ProcessInterface ProcessInterface
        {
            get { return CurrentCompiler.ProcessInterface; }
            set { CurrentCompiler.ProcessInterface = value; }
        }

        /// <summary>
        /// Выполнение инициализации компилятора
        /// </summary>
        /// <param name="ModelingLanguage"></param>
        private void LoadCompiler(ModelingLanguage ModelingLanguage)
        {
            switch (ModelingLanguage)
            {
                case ModelingLanguage.VHDL_GHDL:
                    compiler = new GHDLCompiler();
                    break;
                case ModelingLanguage.VHDL:
                    {
                        Schematix.Core.UserControls.ProgressWindow.Window.RunProcess(
                            new MyBackgroundWorker(
                                new Action(() =>
                                {
                                    try
                                    {
                                        // Start process
                                        compiler = new VHDLCompiler(SchematixCore.ProcessDirectory);
                                        compiler.CollectionChangedEvent += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(messages_CollectionChanged);
                                        PrepareCompilator();
                                        UpdateFileList();
                                    }
                                    catch (Exception ex)
                                    {
                                        Schematix.Core.Logger.Log.Error("Load compile error.", ex);
                                        MessageBox.Show(string.Format("Load compile error. Message: {0}", ex.Message), "Error :(", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    catch
                                    {
                                        MessageBox.Show("Some Error");
                                    }
                                }),
                                new Action(() =>
                                {

                                }), "Initializing Compiler for VHDL..."));
                    }
                    break;
                case ModelingLanguage.VHDL_Combined:
                    {
                        Schematix.Core.UserControls.ProgressWindow.Window.RunProcess(
                            new MyBackgroundWorker(
                                new Action(() =>
                                {
                                    try
                                    {
                                        // Start process
                                        compiler = new CombinedCompiler(SchematixCore.ProcessDirectory);
                                        compiler.CollectionChangedEvent += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(messages_CollectionChanged);
                                        PrepareCompilator();
                                        UpdateFileList();
                                    }
                                    catch (Exception ex)
                                    {
                                        Schematix.Core.Logger.Log.Error("Load compile error.", ex);
                                        MessageBox.Show(string.Format("Load compile error. Message: {0}", ex.Message), "Error :(", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    catch
                                    {
                                        MessageBox.Show("Some Error");
                                    }
                                }),
                                new Action(() =>
                                {

                                }), "Initializing Compiler for VHDL..."));
                    }
                    break;
                case ModelingLanguage.Verilog:
                    throw new Exception("This Compiler Doe's not support yet");
                case ModelingLanguage.SystemC:
                    throw new Exception("This Compiler Doe's not support yet");
            }
        }

        /// <summary>
        /// Обновление списка файлов в проэкте
        /// </summary>
        public void UpdateFileList()
        {
            compiler.Files.Clear();
            foreach (var vhdlCode in project.GetProjectElements<Schematix.ProjectExplorer.VHDL_Code_File>())
            {
                compiler.Files.Add(new VHDL_CodeFile(vhdlCode.Path, "work", compiler as VHDLCompiler));
            }

            compiler.ProcessProject();
        }

        void messages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (core.MessageWindowPanel != null)
                core.MessageWindowPanel.UpdateMessages(compiler.Messages);
        }

        private void PrepareCompilator()
        {
            compiler.ProjectDirectory = ProjectDirectory;
            compiler.LibraryDirectory = ProjectDirectory;
            compiler.SourceDirectory = ProjectDirectory;
        }

        public void ReparseLibrary()
        {
            PrepareCompilator();
            //superior.mProjectExplorer.CorrectTree(compiler.ReparseLibrary());
        }

        /// <summary>
        /// компиляция файла по его пути
        /// </summary>
        /// <param name="path"></param>
        public void CompileOne(string path)
        {
            PrepareCompilator();
            compiler.CompileOne(path);
        }

        /// <summary>
        /// компиляция файла
        /// </summary>
        /// <param name="vhdl_code"></param>
        public void CompileOne(Schematix.ProjectExplorer.VHDL_Code_File vhdl_code)
        {
            CompileOne(vhdl_code.Path);
        }

        /// <summary>
        /// Проверка синтаксиса файла по его пути
        /// </summary>
        /// <param name="path"></param>
        public void CheckSyntaxOne(string path)
        {
            PrepareCompilator();
            compiler.CheckSyntaxOne(path);
        }

        /// <summary>
        /// Проверка синтаксиса файла
        /// </summary>
        /// <param name="vhdl_code"></param>
        public void CheckSyntaxOne(Schematix.ProjectExplorer.VHDL_Code_File vhdl_code)
        {
            CheckSyntaxOne(vhdl_code.Path);
        }

        private System.Threading.Thread compiler_thread;

        /// <summary>
        /// Скомпилировать  полностью проект
        /// </summary>
        public void CompileProject()
        {
            if (compiler.IsBusy == true)
                throw new Exception("Compiler is busy now");
            if (compiler.IsLockGUI == true)
            {
                compiler_thread = new System.Threading.Thread(CompileProjectFunction);
                compiler_thread.Start();
            }
            else
            {
                CompileProjectFunction();
            }
        }

        private void CompileProjectFunction()
        {
            foreach (CodeFile file in compiler.CreateQueue())
            {
                CompileOne(file.FilePath);
                WaitForCompilerFree();
            }
        }


        /// <summary>
        /// Провести проверку синтаксиса всего проекта
        /// </summary>
        public void CheckSyntaxProject()
        {
            if (compiler.IsBusy == true)
                throw new Exception("Compiler is busy now");
            if (compiler.IsLockGUI == true)
            {
                compiler_thread = new System.Threading.Thread(CheckSyntaxProjectFunction);
                compiler_thread.Start();
            }
            else
            {
                CheckSyntaxProjectFunction();
            }
        }        

        private void CheckSyntaxProjectFunction()
        {
            foreach (CodeFile file in compiler.CreateQueue())
            {
                CheckSyntaxOne(file.FilePath);
                WaitForCompilerFree();
            }
        }

        /// <summary>
        /// Ожидание завершения работы компилятора
        /// </summary>
        private void WaitForCompilerFree()
        {
            while (CurrentCompiler.IsBusy == true)
                System.Threading.Thread.Sleep(500);
        }

        /// <summary>
        /// Создание волновой диаграммы
        /// </summary>
        /// <param name="id"></param>
        /// <param name="noRun"></param>
        public void CreateDiagram(string vhdFile, string EntityName, string ArchitectureName, string vcdFile, bool noRun = false)
        {
            string[] arguments = new string[] { vhdFile, EntityName, ArchitectureName, vcdFile };
            if (compiler.IsBusy == true)
                throw new Exception("Compiler is busy now");
            if (compiler.IsLockGUI == true)
            {
                compiler_thread = new System.Threading.Thread(CreateDiagramFunction);
                compiler_thread.Start(arguments);
            }
            else
            {
                CreateDiagramFunction(arguments);
            }                    
        }

        private void CreateDiagramFunction(object o)
        {
            string[] arguments = o as string[];
            string vhdFile = arguments[0];
            string EntityName = arguments[1];
            string ArchitectureName = arguments[2];
            string vcdFile = arguments[3];
            PrepareCompilator();
            compiler.Messages.Clear();
            bool res = compiler.CreateDiagram(vhdFile, EntityName, ArchitectureName, vcdFile);

            project.UpdateSimulationFolderContent();
            bool isContainsFile = false;
            foreach (Schematix.ProjectExplorer.Waveform_File w in project.GetProjectElements<Schematix.ProjectExplorer.Waveform_File>())
            {
                if (w.Path == vcdFile)
                {
                    isContainsFile = true;
                    break;
                }
            }
            if (isContainsFile == false)
            {                
                Schematix.ProjectExplorer.ProjectElementBase vhdl = core.SearchItemInSolution(vhdFile);
                Schematix.ProjectExplorer.ProjectFolder folder = vhdl.Parent as Schematix.ProjectExplorer.ProjectFolder;
                Schematix.ProjectExplorer.ProjectElement vcd = Schematix.ProjectExplorer.ProjectElement.CreateProjectElementByPath(vcdFile, folder);
                folder.AddElement(vcd);
            }
            if (System.IO.File.Exists(vcdFile) == true)
            {
                core.SaveSolution();
                core.UpdateExplorerPanel();
                Schematix.Windows.Waveform.Waveform waveformWindow = core.OpenNewWindow(vcdFile, true) as Schematix.Windows.Waveform.Waveform;
                waveformWindow.SetParameters(vhdFile, EntityName, ArchitectureName);
                waveformWindow.Save();
            }
        }

        /// <summary>
        /// Создание волновой диаграммы
        /// </summary>
        /// <param name="id"></param>
        /// <param name="noRun"></param>
        public void CreateDiagram(string vhdFile, string EntityName, string ArchitectureName, bool noRun = false)
        {
            string vcdFile = GenerateVCDFileName(vhdFile, EntityName, ArchitectureName);
            CreateDiagram(vhdFile, EntityName, ArchitectureName, vcdFile, noRun);
        }

        /// <summary>
        /// Сгенерировать имя VCD файла для моделирования
        /// </summary>
        /// <param name="vhdFile"></param>
        /// <param name="EntityName"></param>
        /// <param name="ArchitectureName"></param>
        /// <returns></returns>
        public string GenerateVCDFileName(string vhdFile, string EntityName, string ArchitectureName)
        {
            string fileName = string.Format("{0}_{1}.vcd", ArchitectureName, DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            string filePath = System.IO.Path.Combine(project.SimulationFolder.Path, fileName);
            return filePath;
        }

        /// <summary>
        /// Сгенерировать имя TestBench файла
        /// </summary>
        /// <param name="vhdFile"></param>
        /// <param name="EntityName"></param>
        /// <param name="ArchitectureName"></param>
        /// <returns></returns>
        public string GenerateTestBenchFileName(string vhdFile, string EntityName, string ArchitectureName)
        {
            string fileName = string.Format("{0}_TB_{1}.vhdl", ArchitectureName, DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            string filePath = System.IO.Path.Combine(project.SimulationFolder.Path, fileName);
            return filePath;
        }

        /// <summary>
        /// Создание волновой диаграммы для test bench
        /// </summary>
        /// <param name="vcdFile"></param>
        /// <param name="testBenchFilename"></param>
        /// <param name="TBentity"></param>
        /// <param name="Tbarchitecture"></param>
        /// <param name="vhdlFile"></param>
        /// <param name="entity"></param>
        /// <param name="architecture"></param>
        public void CreateTestBenchDiagram(string vcdFile, string testBenchFilename, string TBentity, string Tbarchitecture, string vhdlFile, string entity, string architecture)
        {
            string[] arguments = new string[] { vcdFile, testBenchFilename, TBentity, Tbarchitecture, vhdlFile, entity, architecture };
            if (compiler.IsBusy == true)
                throw new Exception("Compiler is busy now");
            if (compiler.IsLockGUI == true)
            {
                compiler_thread = new System.Threading.Thread(CreateTestBenchDiagramFunction);
                compiler_thread.Start(arguments);
            }
            else
            {
                CreateTestBenchDiagramFunction(arguments);
            }         
        }

        private void CreateTestBenchDiagramFunction(object o)
        {
            string[] arguments = o as string[];
            string vcdFile = arguments[0];
            string testBenchFilename = arguments[1];
            string TBentity = arguments[2];
            string Tbarchitecture = arguments[3];
            string vhdlFile = arguments[4];
            string entity = arguments[5];
            string architecture = arguments[6];

            PrepareCompilator();
            compiler.Messages.Clear();
            bool res = compiler.CreateTestBenchDiagram(
                testBenchFilename: testBenchFilename,
                outFile: vcdFile,
                testBenchEntity: TBentity,
                testBenchArchitecture: Tbarchitecture,
                file: vhdlFile,
                entity: entity,
                architecture: architecture);

            if (System.IO.File.Exists(vcdFile) == false)
                return;

            project.UpdateSimulationFolderContent();
            core.SaveSolution();
            core.UpdateExplorerPanel();
            Schematix.Windows.Waveform.Waveform waveformWindow = core.OpenNewWindow(vcdFile, true) as Schematix.Windows.Waveform.Waveform;
            //waveformWindow.Save();

        }

        #region IDisposable Members

        public void Dispose()
        {
            compiler.CollectionChangedEvent -= messages_CollectionChanged;
            if ((compiler_thread != null) && (compiler_thread.IsAlive == true))
            {
                compiler_thread.Abort();
            }
            compiler.Dispose();
        }

        #endregion

        public bool CleanProject()
        {
            return compiler.CleanProject();
        }
    }
     
}