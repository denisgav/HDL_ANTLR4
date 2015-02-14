using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Schematix.Panels;
using AvalonDock.Layout;
using Schematix.ProjectExplorer;
using Schematix.DesignBrowser;
using AvalonDock.Layout.Serialization;
using System.IO;

namespace Schematix
{
    /// <summary>
    /// Класс, который выступает самым главным связующим звеном
    /// Очень важный класс
    /// </summary>
    public class SchematixCore
    {
        /// <summary>
        /// Главный узел
        /// </summary>
        private static SchematixCore core;
        public static SchematixCore Core
        {
            get { return core; }
        }

        /// <summary>
        /// Панель инструментов
        /// </summary>
        private ToolBoxPanel toolBoxPanel;
        public ToolBoxPanel ToolBoxPanel
        {
            get { return toolBoxPanel; }
        }

        /// <summary>
        /// Панель для отображения сообщений
        /// </summary>
        private MessageWindowPanel messageWindowPanel;
        public MessageWindowPanel MessageWindowPanel
        {
            get { return messageWindowPanel; }
        }

        /// <summary>
        /// Панель для навигации по проекту
        /// </summary>
        private ProjectExplorerPanel projectExplorerPanel;
        public ProjectExplorerPanel ProjectExplorerPanel
        {
            get { return projectExplorerPanel; }
        }

        /// <summary>
        /// Панель для навигации по дизайну
        /// </summary>
        private DesignBrowserPanel designBrowserPanel;
        public DesignBrowserPanel DesignBrowserPanel
        {
            get { return designBrowserPanel; }
        }

        /// <summary>
        /// Стартовая страница
        /// </summary>
        private StartPagePanel startPagePanel;
        public StartPagePanel StartPagePanel
        {
            get { return startPagePanel; }
        }

        /// <summary>
        /// Главная консоль (без нее никак)
        /// </summary>
        private Schematix.Panels.ConsolePanel _console;
        public Schematix.Panels.ConsolePanel CmdConsole
        {
            get { return _console; }
        }

        /// <summary>
        /// Поле для поиска
        /// </summary>
        private SearchReplace _search;
        public SearchReplace Search
        {
            get 
            {
                if (_search == null)
                {
                    _search = new SearchReplace();
                }
                return _search; 
            }
        }
        

        /// <summary>
        /// Текущее решение проекта
        /// </summary>
        private Solution solution;
        public Solution Solution
        {
            get { return solution; }
            set 
            {
                solution = value;
                if (projectExplorerPanel != null)
                    projectExplorerPanel.UpdateInfo();
                if (designBrowserPanel != null)
                    designBrowserPanel.UpdateInfo();
                if(mainWindow != null)
                    mainWindow.UpdateSolutionData();
            }
        }

        /// <summary>
        /// Текущий выбраный проект
        /// </summary>
        public Project CurrentSelectedProject
        {
            get 
            {
                if (solution == null)
                    return null;
                return solution.CurrentSelectedProject; 
            }
        }

        /// <summary>
        /// Текущий компилятор
        /// </summary>
        public Compiler CurrentCompiler
        {
            get
            {
                Project proj = CurrentSelectedProject;
                if (proj == null)
                    return null;
                return proj.Compiler;
            }
        }

        /// <summary>
        /// Проверка занят ли компилятор
        /// </summary>
        public bool IsCompilerBusy
        {
            get 
            {
                if ((solution != null) && (solution.CurrentSelectedProject != null))
                    return (solution.CurrentSelectedProject.Compiler.CurrentCompiler.IsBusy == true);
                return true;
            }
        }
        

        /// <summary>
        /// Папка в которой находится исполняемый файл
        /// </summary>
        private static string processDirectory;
        public static string ProcessDirectory
        {
            get { return processDirectory; }
        }
        

        public void UpdateExplorerPanel()
        {
            mainWindow.Dispatcher.Invoke(
                System.Windows.Threading.DispatcherPriority.Background, new System.Windows.Threading.DispatcherOperationCallback(delegate
            {
                //solution.OnPropertyChanged("updated");
                projectExplorerPanel.UpdateInfo();
                designBrowserPanel.UpdateInfo();
                mainWindow.UpdateSolutionData();
                return null;
            }
            ), null);            
        }
        

        /// <summary>
        /// Создание дочерних элементов
        /// </summary>
        private void CreateInstances()
        {
            Schematix.Core.Logger.Log.Info("Creating ToolBox Panel");
            toolBoxPanel = new ToolBoxPanel();

            Schematix.Core.Logger.Log.Info("Creating MessageWindow Panel");
            messageWindowPanel = new MessageWindowPanel();

            Schematix.Core.Logger.Log.Info("Creating ProjectExplorer Panel");
            projectExplorerPanel = new ProjectExplorerPanel(this);

            Schematix.Core.Logger.Log.Info("Creating DesignBrowser Panel");
            designBrowserPanel = new DesignBrowserPanel(this);

            Schematix.Core.Logger.Log.Info("Creating Console Panel");
            _console = new ConsolePanel();

            Schematix.Core.Logger.Log.Info("Creating StartPage Panel");
            startPagePanel = new StartPagePanel();

            
        }

        private void FindInstances()
        {
            toolBoxPanel = FindPanel<ToolBoxPanel>();
            if (toolBoxPanel == null)
                toolBoxPanel = new ToolBoxPanel();

            messageWindowPanel = FindPanel<MessageWindowPanel>();
            if (messageWindowPanel == null)
                messageWindowPanel = new MessageWindowPanel();

            projectExplorerPanel = FindPanel<ProjectExplorerPanel>();
            if (projectExplorerPanel == null)
                projectExplorerPanel = new ProjectExplorerPanel(this);

            designBrowserPanel = FindPanel<DesignBrowserPanel>();
            if (designBrowserPanel == null)
                designBrowserPanel = new DesignBrowserPanel(this);


            _console = FindPanel<ConsolePanel>();
            if (_console == null)
                _console = new ConsolePanel();
            _console.IsActive = true;

            startPagePanel = FindPanel<StartPagePanel>();
            if (startPagePanel == null)
                startPagePanel = new StartPagePanel();
            //_search = new SearchReplace();
        }

        /// <summary>
        /// Главное окно
        /// </summary>
        private MainWindow mainWindow;
        public MainWindow MainWindow
        {
            get { return mainWindow; }
            set { mainWindow = value; }
        }
        
        /// <summary>
        /// Коструктор
        /// </summary>
        /// <param name="mainWindow"></param>
        public SchematixCore (MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            core = this;            
        }

        static SchematixCore()
        {
            //Получение рабочей папки
            processDirectory = Directory.GetCurrentDirectory();
        }

        /// <summary>
        /// Загрузка решения
        /// </summary>
        public void LoadSolution(string path)
        {
            Solution = Solution.LoadSolutionFromXml(path);
            if (Solution != null)
            {
                mainWindow.Title = string.Format("{0} - HDL Light solution", Solution.Caption);
                //Открываем окна
                string filePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path), "WindowsState.dat");
                LoadOpenedWindows(filePath);
            }
        }

        /// <summary>
        /// Восстановить список открытых окон
        /// </summary>
        /// <param name="filePath"></param>
        private void LoadOpenedWindows(string filePath)
        {
            //открываем файлы
            if (File.Exists(filePath))
            {
                FileStream In = null;
                StreamReader reader = null;
                try
                {
                    In = new FileStream(filePath, FileMode.Open);
                    reader = new StreamReader(In);
                    while (reader.EndOfStream == false)
                    {
                        string file = reader.ReadLine();
                        if (string.IsNullOrEmpty(file) == false)
                        {
                            OpenNewWindow(file);
                        }
                    }                    
                }
                catch (Exception ex)
                { }
                finally 
                {
                    if(reader != null)
                        reader.Close();
                    if(In != null)
                        In.Close();
                }
            }
        }

        /// <summary>
        /// Сохранить список открытых окон
        /// </summary>
        /// <param name="filePath"></param>
        private void SaveOpenedWindows(string filePath)
        {
            FileStream Out = null;
            StreamWriter writer = null;
            try
            {
                //сохраняем положение всех окон
                Out = new FileStream(filePath, FileMode.Create);
                writer = new StreamWriter(Out);
                List<Schematix.Windows.SchematixBaseWindow> windows = OpenedWindows().ToList();
                List<string> files = new List<string>();
                foreach (Schematix.Windows.SchematixBaseWindow f in windows)
                {
                    string file = f.ProjectElement.Path;
                    if (files.Contains(file))
                        continue;
                    else
                        files.Add(file);
                }

                foreach (string file in files)
                {
                    writer.WriteLine(file);
                }
                files.Clear();
            }
            catch (Exception ex)
            { }
            finally
            {
                if (writer != null)
                    writer.Close();
                if (Out != null)
                    Out.Close();
            }
            ///////////////////////////////
        }

        /// <summary>
        /// Создание проекта
        /// </summary>
        /// <returns></returns>
        public Solution CreateProject()
        {
            if (solution != null)
                CloseSolution();
            Schematix.Dialogs.NewProjectDialog dialog = null;
            if (core.Solution != null)
            {
                dialog = new Schematix.Dialogs.NewProjectDialog(core.Solution.RootFolder, core.Solution);
            }
            else
            {
                dialog = new Schematix.Dialogs.NewProjectDialog();
            }

            if (dialog.ShowDialog() == true)
            {
                core.Solution = dialog.Solution;
                startPagePanel.AddNewRecentProject(core.Solution.Path);
            }
            return core.Solution;
        }

        /// <summary>
        /// Открыть проект
        /// </summary>
        /// <returns></returns>
        public Solution OpenProject()
        {            
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.ValidateNames = true;
            dialog.Filter = "Solutio File|*.sol";
            if (dialog.ShowDialog() == true)
            {
                if (solution != null)
                    CloseSolution();
                core.LoadSolution(dialog.FileName);
            }
            return solution;
        }

        /// <summary>
        /// Открыть проект
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public Solution OpenProject(string filePath)
        {
            if (solution != null)
                CloseSolution();
            core.LoadSolution(filePath);
            startPagePanel.AddNewRecentProject(filePath);
            return solution;
        }

        /// <summary>
        /// Сохранение решения
        /// </summary>
        public void SaveSolution()
        {
            if(solution != null)
                solution.Save();
        }

        /// <summary>
        /// Закрыть решение
        /// </summary>
        public void CloseSolution()
        {
            if (solution != null)
            {
                //Сохраняем окна
                if (Solution != null)
                {
                    string filePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Solution.Path), "WindowsState.dat");
                    SaveOpenedWindows(filePath);
                }
                solution.Save();
                CloseAllWindows();                
            }
            Solution = null;
        }

        /// <summary>
        /// Используется для задания размещения панелей по-умолчанию
        /// </summary>
        public void ResetLayout()
        {
            try
            {
                if (LoadLayout() == false)
                {
                    DefaultLayout();
                }
                else
                {
                    FindInstances();

                    if (FindPanel<StartPagePanel>() == null)
                    {
                        var firstDocumentPane = mainWindow.dockManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
                        if (firstDocumentPane != null)
                        {
                            firstDocumentPane.Children.Add(startPagePanel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DefaultLayout();
            }
        }

        /// <summary>
        /// Разместить окна по-умолчанию
        /// </summary>
        public void DefaultLayout()
        {
            try
            {
                ClearLayout();

                CreateInstances();                

                toolBoxPanel.AddToLayout(mainWindow.dockManager, AnchorableShowStrategy.Right);
                _console.AddToLayout(mainWindow.dockManager, AnchorableShowStrategy.Bottom);
                messageWindowPanel.AddToLayout(mainWindow.dockManager, AnchorableShowStrategy.Bottom);
                projectExplorerPanel.AddToLayout(mainWindow.dockManager, AnchorableShowStrategy.Left);
                designBrowserPanel.AddToLayout(mainWindow.dockManager, AnchorableShowStrategy.Left);
                
                foreach (LayoutAnchorablePane pane in mainWindow.dockManager.Layout.Descendents().OfType<LayoutAnchorablePane>())
                {
                    pane.DockHeight = new System.Windows.GridLength(200);
                    pane.DockWidth = new System.Windows.GridLength(200);
                }

                if (FindPanel<StartPagePanel>() == null)
                {
                    var firstDocumentPane = mainWindow.dockManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
                    if (firstDocumentPane != null)
                    {
                        firstDocumentPane.Children.Add(startPagePanel);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Очистить информацию о привязке
        /// </summary>
        private void ClearLayout()
        {
            if (toolBoxPanel != null)
                toolBoxPanel.Close();

            if (_console != null)
                _console.Close();

            if (messageWindowPanel != null)
                messageWindowPanel.Close();

            if (projectExplorerPanel != null)
                projectExplorerPanel.Close();

            if (designBrowserPanel != null)
                designBrowserPanel.Close();

            mainWindow.dockManager.Layout.LeftSide.Children.Clear();
            mainWindow.dockManager.Layout.TopSide.Children.Clear();
            mainWindow.dockManager.Layout.RightSide.Children.Clear();
            mainWindow.dockManager.Layout.BottomSide.Children.Clear();
            //mainWindow.dockManager.Layout.RootPanel.Children.Clear();

            mainWindow.dockManager.Layout.FloatingWindows.Clear();
        }


        /// <summary>
        /// Функция, которая открывает новое окно
        /// </summary>
        /// <param name="window"></param>
        public void OpenNewWindow(Schematix.Windows.SchematixBaseWindow window)
        {
            mainWindow.Dispatcher.Invoke(
                System.Windows.Threading.DispatcherPriority.Background, new System.Windows.Threading.DispatcherOperationCallback(delegate
            {
                var firstDocumentPane = mainWindow.dockManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
                if (firstDocumentPane != null)
                {
                    if (firstDocumentPane.Children.Contains(window) == false)
                        firstDocumentPane.Children.Add(window);
                    window.IsActive = true;
                }
                return null;
            }
            ), null);  
        }

        /// <summary>
        /// Открыть новое окно
        /// </summary>
        /// <param name="projectElement"></param>
        public Schematix.Windows.SchematixBaseWindow OpenNewWindow(ProjectElementBase projectElement)
        {
            try
            {
                if (projectElement == null)
                    return null;

                if ((System.IO.File.Exists(projectElement.Path) == false) && ((System.IO.Directory.Exists(projectElement.Path) == false)))
                {
                    var DialogResult = System.Windows.MessageBox.Show(string.Format("Could not find file {0}. Remove it from prject?", projectElement.Path), "File not found", System.Windows.MessageBoxButton.YesNo);
                    if (DialogResult == System.Windows.MessageBoxResult.Yes)
                    {
                        (projectElement.Parent as ProjectFolder).RemoveElement(projectElement);
                        SaveSolution();
                        UpdateExplorerPanel();
                    }
                    return null;
                }

                Schematix.Windows.SchematixBaseWindow window = null;

                mainWindow.Dispatcher.Invoke(
                    System.Windows.Threading.DispatcherPriority.Background, new System.Windows.Threading.DispatcherOperationCallback(delegate
                    {
                        foreach (Schematix.Windows.SchematixBaseWindow v in OpenedWindows())
                        {
                            if ((v.ProjectElement == projectElement) || (v.ProjectElement.Path == projectElement.Path))
                            {
                                window = v;
                                v.IsActive = true;
                                return null;
                            }
                        }
                        window = projectElement.CreateNewWindow();
                        return null;
                    }
                ), null);
                if (window != null)
                {
                    OpenNewWindow(window);
                }
                return window;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Открыть файл
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="searchItemInSolution"></param>
        /// <param name="projFolder"></param>
        public Schematix.Windows.SchematixBaseWindow OpenNewWindow(string filePath, bool searchItemInSolution = true, ProjectFolder projFolder = null)
        {
            ProjectElementBase elem = null;
            //Ищем в решении файл (если необходимо)
            if (searchItemInSolution == true)
            {
                elem = SearchItemInSolution(filePath);
            }
            //Если файл не нашелся, создаем объект ProjectElementBase вручную
            if (elem == null)
            {
                elem = ProjectElementBase.CreateProjectElementByPath(filePath, projFolder);
            }
            return OpenNewWindow(elem);
        }

        /// <summary>
        /// Открыть файл с исходным кодом и поставить курсор в нужную позицию
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="searchItemInSolution"></param>
        /// <param name="projFolder"></param>
        /// <param name="LineNumber"></param>
        public Schematix.Windows.Code.Code OpenSource(string filePath, bool searchItemInSolution = true, ProjectFolder projFolder = null, int LineNumber = 0, int position = 0)
        {
            ProjectElementBase elem = null;
            //Ищем в решении файл (если необходимо)
            if (searchItemInSolution == true)
            {
                elem = SearchItemInSolution(filePath);
            }
            //Если файл не нашелся, создаем объект ProjectElementBase вручную
            if (elem == null)
            {
                elem = ProjectElementBase.CreateProjectElementByPath(filePath, projFolder);
            }

            Schematix.Windows.SchematixBaseWindow window = null;
            Schematix.Windows.Code.Code codeWindow = null;

            mainWindow.Dispatcher.Invoke(
                System.Windows.Threading.DispatcherPriority.Background, new System.Windows.Threading.DispatcherOperationCallback(delegate
                {
                    window = OpenNewWindow(elem);
                    codeWindow = window as Schematix.Windows.Code.Code;
                    if (LineNumber >= 0)
                    {
                        codeWindow.textEditor.Loaded += new System.Windows.RoutedEventHandler(delegate{codeWindow.SetPosition(LineNumber, position);});                                                
                    }
                    if (codeWindow.ProjectElement.Parent == null)
                        codeWindow.textEditor.IsReadOnly = true;
                    return null;
                }
            ), null);

            

            return codeWindow;
        }

        /// <summary>
        /// Поиск файла в решении
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public ProjectElementBase SearchItemInSolution(string filePath)
        {
            if (solution != null)
            {
                foreach (Project proj in solution.ProjectList)
                {
                    foreach (ProjectElement el in proj.GetAllProjectElements())
                    {
                        if ((el.Path.Equals(filePath, StringComparison.InvariantCultureIgnoreCase) == true))
                            return el;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Вызывается при закрытии приложения
        /// </summary>
        public void OnCloseApplication(System.ComponentModel.CancelEventArgs e)
        {
            SaveLayout();
            if ((solution != null) && (solution.CurrentSelectedProject != null) && (solution.CurrentSelectedProject.Compiler.CurrentCompiler.IsBusy))
            {
                System.Windows.MessageBoxResult res = System.Windows.MessageBox.Show(string.Format("Compiler of project {0} is active now. Terminate it anyway?", Solution.CurrentSelectedProject.Caption), "Compiler is active now", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);
                if(res == System.Windows.MessageBoxResult.No)
                {
                    e.Cancel = true;
                    return;
                }
                if (res == System.Windows.MessageBoxResult.Yes)
                {
                    solution.CurrentSelectedProject.Compiler.Dispose();
                }

            }
            //Проверяем есть ли несохраненные файлы
            foreach (Schematix.Windows.SchematixBaseWindow v in OpenedWindows())
            {
                System.ComponentModel.CancelEventArgs arg = new System.ComponentModel.CancelEventArgs();
                Schematix.Windows.SchematixBaseWindow window = v as Schematix.Windows.SchematixBaseWindow;
                window.SchematixBaseWindow_Closing(this, arg);
                if (arg.Cancel == true)
                {
                    e.Cancel = true;
                    return;
                }
            }
            //вызываем необходимые методы для панелей
            foreach (Schematix.Panels.SchematixPanelBase panel in OpenedPanels())
            {
                panel.OnClosePanel();
            }
            if (Solution != null)
            {
                //Сохраняем окна
                string filePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Solution.Path), "WindowsState.dat");
                SaveOpenedWindows(filePath);
            }
        }

        private void SaveLayout()
        {
            string FileName = Schematix.CommonProperties.Configuration.CurrentConfiguration.LayoutPath;
            FileStream fStream = null;
            try
            {
                var serializer = new XmlLayoutSerializer(mainWindow.dockManager);
                fStream = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter stream = new StreamWriter(fStream);
                    serializer.Serialize(stream);
                    stream.Close();
            }
            catch (Exception ex)
            {
                //fStream.Close();
            }
            finally
            {
                if (fStream != null)
                    fStream.Close();
            }
        }

        private bool LoadLayout()
        {
            string FileName = Schematix.CommonProperties.Configuration.CurrentConfiguration.LayoutPath;
            var currentContentsList = mainWindow.dockManager.Layout.Descendents().OfType<LayoutContent>().Where(c => c.ContentId != null).ToArray();

            FileStream fStream = null;
            try
            {
                var serializer = new XmlLayoutSerializer(mainWindow.dockManager);
                fStream = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                StreamReader stream = new StreamReader(fStream);
                serializer.Deserialize(stream);
                stream.Close();
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (fStream != null)
                    fStream.Close();
            }
            return true;
        }

        /// <summary>
        /// Закрыть все открытые окна
        /// </summary>
        public void CloseAllWindows()
        {
            List<Schematix.Windows.SchematixBaseWindow> windows = new List<Windows.SchematixBaseWindow>(OpenedWindows());
            foreach (Schematix.Windows.SchematixBaseWindow v in windows)
            {
                v.Close();
            }
        }

        /// <summary>
        /// Закрыть все окна за исключением одного
        /// </summary>
        /// <param name="window"></param>
        public void CloseAllWindowsExcept(Schematix.Windows.SchematixBaseWindow window)
        {
            List<Schematix.Windows.SchematixBaseWindow> windows = new List<Windows.SchematixBaseWindow>(OpenedWindows());
            foreach (Schematix.Windows.SchematixBaseWindow v in windows)
            {
                if (v != window)
                {
                    v.Close();
                }
            }
        }

        /// <summary>
        /// Сохранить все
        /// </summary>
        public void SaveAll()
        {
            List<Schematix.Windows.SchematixBaseWindow> windows = new List<Windows.SchematixBaseWindow>(OpenedWindows());
            foreach (Schematix.Windows.SchematixBaseWindow v in windows)
            {
                v.Save();
            }
        }

        /// <summary>
        /// Перечислить все открытые окна
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Schematix.Windows.SchematixBaseWindow> OpenedWindows()
        {
            foreach (var el in mainWindow.dockManager.Layout.Children)
            {
                foreach (var v in el.Descendents())
                {
                    if (v is Schematix.Windows.SchematixBaseWindow)
                    {
                        yield return (v as Schematix.Windows.SchematixBaseWindow);
                    }
                }
            }
        }

        /// <summary>
        /// Найти панель по ее типу
        /// </summary>
        /// <returns></returns>
        public T FindPanel<T>() where T : Schematix.Panels.SchematixPanelBase
        {
            foreach (var el in mainWindow.dockManager.Layout.Children)
            {
                foreach (var v in el.Descendents())
                {
                    if (v is T)
                    {
                        return v as T;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Перечислить все открытые панели
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Schematix.Panels.SchematixPanelBase> OpenedPanels()
        {
            foreach (var el in mainWindow.dockManager.Layout.Children)
            {
                foreach (var v in el.Descendents())
                {
                    if (v is Schematix.Panels.SchematixPanelBase)
                    {
                        yield return (v as Schematix.Panels.SchematixPanelBase);
                    }
                }
            }
        }

        /// <summary>
        /// Запустить процесс моделирования VHDL компонента
        /// </summary>
        /// <param name="archName"></param>
        /// <param name="entityName"></param>
        /// <param name="fileName"></param>
        public void StartVHDLSimulation(string archName, string entityName, string vhdlFile)
        {
            if ((string.IsNullOrEmpty(archName) == false) && ((string.IsNullOrEmpty(entityName) == false)) && ((string.IsNullOrEmpty(vhdlFile) == false)))
            {
                if ((CurrentCompiler != null) && (CurrentCompiler.CurrentCompiler.IsBusy == false))
                {
                    if (CmdConsole != null)
                        CurrentCompiler.ProcessInterface = CmdConsole.ProcessInterface;
                    CurrentCompiler.CreateDiagram(vhdlFile, entityName, archName);
                    //CurrentSelectedProject.UpdateSimulationFolderContent();
                }
            }
        }

        /// <summary>
        /// Запустить процесс моделирования VHDL компонента c 
        /// указанием пути для сохранения vcdФайла
        /// </summary>
        /// <param name="archName"></param>
        /// <param name="entityName"></param>
        /// <param name="vhdlFile"></param>
        /// <param name="vcdFile"></param>
        public void StartVHDLSimulation(string archName, string entityName, string vhdlFile, string vcdFile)
        {
            if ((string.IsNullOrEmpty(archName) == false) && ((string.IsNullOrEmpty(entityName) == false)) && ((string.IsNullOrEmpty(vhdlFile) == false)))
            {
                if ((CurrentCompiler != null) && (CurrentCompiler.CurrentCompiler.IsBusy == false))
                {
                    if (CmdConsole != null)
                        CurrentCompiler.ProcessInterface = CmdConsole.ProcessInterface;
                    CurrentCompiler.CreateDiagram(vhdlFile, entityName, archName, vcdFile);
                    //CurrentSelectedProject.UpdateSimulationFolderContent();
                }
            }
        }

        #region
        /// <summary>
        /// Открыть окно ToolBox, или активировать если оно открыто
        /// </summary>
        public void ShowToolBox()
        {
            ShowPanel(toolBoxPanel, AnchorableShowStrategy.Right);
        }

        /// <summary>
        /// открыть ProjectExplorer
        /// </summary>
        public void ShowProjectExplorer()
        {
            ShowPanel(projectExplorerPanel, AnchorableShowStrategy.Left);
        }

        /// <summary>
        /// Открыть DesignBrowser
        /// </summary>
        public void ShowDesignBrowser()
        {
            ShowPanel(designBrowserPanel, AnchorableShowStrategy.Left);
        }

        /// <summary>
        /// Открыть поле поиска
        /// </summary>
        public void ShowSearchPanel()
        {
            ShowPanel(Search, AnchorableShowStrategy.Most);
            Search.Refresh();
        }

        /// <summary>
        /// Отобразить консоль
        /// </summary>
        public void ShowConsole()
        {
            ShowPanel(_console, AnchorableShowStrategy.Bottom);
        }

        /// <summary>
        /// Отобразить окно с сообщениями
        /// </summary>
        public void ShowMessageWindow()
        {
            ShowPanel(messageWindowPanel, AnchorableShowStrategy.Bottom);
        }

        /// <summary>
        /// Создать новое окно с консолью
        /// </summary>
        public void CreateNewConsole()
        {
            Schematix.Panels.ConsolePanel console = new ConsolePanel("cmd", string.Empty);
            ShowPanel(console, AnchorableShowStrategy.Bottom);
        }

        /// <summary>
        /// Общая функция для отображения панели
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="showSrtrategy"></param>
        private void ShowPanel(Schematix.Panels.SchematixPanelBase panel, AnchorableShowStrategy showSrtrategy)
        {
            if (panel != null)
            {
                if ((panel.IsVisible == false) && (panel.IsHidden == false))
                    panel.AddToLayout(mainWindow.dockManager, showSrtrategy);
                panel.Show();
            }
        }
        #endregion
    }
}
