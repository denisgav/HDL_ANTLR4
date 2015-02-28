using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.Windows.Threading;
using AvalonDock.Layout;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using AvalonDock.Layout.Serialization;
using System.Resources;
using System.Collections;
using Schematix.Panels;
using Schematix.Dialogs;
using Schematix.Dialogs.Search_Replace.Code;

namespace Schematix
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Связующее звено
        /// </summary>
        private SchematixCore core;
        public SchematixCore Core
        {
            get { return core; }
        }

        public MainWindow()
        {
            InitializeComponent();
            core = new SchematixCore(this);            
        }

        /// <summary>
        /// Получение текущего активного окна
        /// </summary>
        /// <returns></returns>
        private Schematix.Windows.SchematixBaseWindow currentActiveWindow;
        public Schematix.Windows.SchematixBaseWindow CurrentActiveWindow
        {
            get
            {
                /*
                LayoutContent activeContent = dockManager.Layout.ActiveContent;
                if (activeContent == null)
                    return null;
                if (activeContent is Schematix.Windows.SchematixBaseWindow)
                    return activeContent as Schematix.Windows.SchematixBaseWindow;
                return null;
                */

                return currentActiveWindow;
            }
        }

        #region File submenu

        /// <summary>
        /// Создавать проект можно только когда создано ядро
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewProject_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ((core != null));
        }

        private void NewProject_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            core.CreateProject();
        }

        /// <summary>
        /// Открывать проект можно всегда
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenProject_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// Открытие проекта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenProject_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            core.OpenProject();
        }

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if ((core != null) && (CurrentActiveWindow != null))
                CurrentActiveWindow.Save_CanExecute(sender, e);
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if ((core != null) && (CurrentActiveWindow != null))
            {
                if (CurrentActiveWindow.ProjectElement.Removed == false)
                    CurrentActiveWindow.Save_Executed(sender, e);
                else
                    CurrentActiveWindow.SaveAs_Executed(sender, e);
            }
        }

        private void SaveAs_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if ((core != null) && (CurrentActiveWindow != null))
                CurrentActiveWindow.SaveAs_CanExecute(sender, e);
        }

        private void SaveAs_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if ((core != null) && (CurrentActiveWindow != null))
                CurrentActiveWindow.SaveAs_Executed(sender, e);
        }

        private void SaveAll_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (IsLoaded)
            {
                foreach (Schematix.Windows.SchematixBaseWindow el in core.OpenedWindows())
                {
                    Schematix.Windows.SchematixBaseWindow window = el;
                    if (window.IsModified == true)
                        e.CanExecute = true;
                }
            }
        }

        private void SaveAll_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (IsLoaded)
            {
                core.SaveAll();
            }
        }

        private void Close_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (dockManager.IsLoaded == false)
                return;
            LayoutContent activeContent = dockManager.Layout.ActiveContent;
            if (activeContent == null)
            {
                e.CanExecute = false;
                return;
            }
            e.CanExecute = (activeContent is LayoutDocument) && (activeContent.CanClose == true);
        }

        private void Close_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            LayoutContent activeContent = dockManager.Layout.ActiveContent;
            activeContent.Close();
        }

        private void CloseProject_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (Core != null) && (core.Solution != null);
        }

        private void CloseProject_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            core.CloseSolution();
        }

        private void Exit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Exit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            core.CloseAllWindows();
            Close();
        }
        #endregion

        #region Edit submenu
        private void Undo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if ((core != null) && (CurrentActiveWindow != null))
                CurrentActiveWindow.Undo_CanExecute(sender, e);
        }

        private void Undo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if ((core != null) && (CurrentActiveWindow != null))
                CurrentActiveWindow.Undo_Executed(sender, e);
        }
        private void Redo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if ((core != null) && (CurrentActiveWindow != null))
                CurrentActiveWindow.Redo_CanExecute(sender, e);
        }

        private void Redo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if ((core != null) && (CurrentActiveWindow != null))
                CurrentActiveWindow.Redo_Executed(sender, e);
        }
        private void Cut_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if ((core != null) && (CurrentActiveWindow != null))
                CurrentActiveWindow.Cut_CanExecute(sender, e);
        }

        private void Cut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if ((core != null) && (CurrentActiveWindow != null))
                CurrentActiveWindow.Cut_Executed(sender, e);
        }
        private void Copy_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if ((core != null) && (CurrentActiveWindow != null))
                CurrentActiveWindow.Copy_CanExecute(sender, e);
        }

        private void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if ((core != null) && (CurrentActiveWindow != null))
                CurrentActiveWindow.Copy_Executed(sender, e);
        }
        private void Paste_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if ((core != null) && (CurrentActiveWindow != null))
                CurrentActiveWindow.Paste_CanExecute(sender, e);
        }

        private void Paste_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if ((core != null) && (CurrentActiveWindow != null))
                CurrentActiveWindow.Paste_Executed(sender, e);
        }

        private void Delete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if ((core != null) && (CurrentActiveWindow != null))
                CurrentActiveWindow.Delete_CanExecute(sender, e);
        }

        private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if ((core != null) && (CurrentActiveWindow != null))
                CurrentActiveWindow.Delete_Executed(sender, e);
        }
        private void SelectAll_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if ((core != null) && (CurrentActiveWindow != null))
                CurrentActiveWindow.SelectAll_CanExecute(sender, e);
        }

        private void SelectAll_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if ((core != null) && (CurrentActiveWindow != null))
                CurrentActiveWindow.SelectAll_Executed(sender, e);
        }
        #endregion

        #region View menu
        private void Toolbox_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            core.ShowToolBox();
        }

        private void ProjectExplorer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            core.ShowProjectExplorer();
        }

        private void DesignBrowser_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            core.ShowDesignBrowser();
        }

        private void MessageWindow_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            core.ShowMessageWindow();
        }

        private void ConsoleWindow_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            core.ShowConsole();
        }

        private void NewConsoleWindow_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            core.CreateNewConsole();
        }
        #endregion

        #region Window menu
        private void CloseAll_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            core.CloseAllWindows();
        }

        private void CloseAllButThis_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            core.CloseAllWindowsExcept(CurrentActiveWindow);
        }

        private void ResetLayout_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            core.DefaultLayout();
        }
        #endregion

        #region Help menu
        private void Contents_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void About_Executed(object sender, ExecutedRoutedEventArgs e)
        {
        	AboutWindow about = new AboutWindow();
        	about.ShowDialog();
        }
        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            core.ResetLayout();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            core.OnCloseApplication(e);
        }

        private void MenuItemDefault_Click(object sender, RoutedEventArgs e)
        {
            dockManager.Theme = null;
        }

        private void MenuItemVS2010_Click(object sender, RoutedEventArgs e)
        {
            dockManager.Theme = new AvalonDock.Themes.VS2010Theme();
        }

        private void MenuItemAero_Click(object sender, RoutedEventArgs e)
        {
            dockManager.Theme = new AvalonDock.Themes.AeroTheme();
        }

        private void MenuItemExpressionDark_Click(object sender, RoutedEventArgs e)
        {
            dockManager.Theme = new AvalonDock.Themes.ExpressionDarkTheme();
        }

        private void MenuItemExpressionLight_Click(object sender, RoutedEventArgs e)
        {
            dockManager.Theme = new AvalonDock.Themes.ExpressionLightTheme();
        }

        private void MenuItemMetro_Click(object sender, RoutedEventArgs e)
        {
            dockManager.Theme = new AvalonDock.Themes.MetroTheme();
        }

        private void dockManager_ActiveContentChanged(object sender, EventArgs e)
        {
            LayoutContent activeContent = dockManager.Layout.ActiveContent;
            if ((((activeContent == null) || (activeContent is Schematix.Panels.SchematixPanelBase))) && (activeContent is Schematix.Panels.StartPagePanel == false))
                return;

            currentActiveWindow = (activeContent is Schematix.Windows.SchematixBaseWindow) ? activeContent as Schematix.Windows.SchematixBaseWindow : null;

            if (core.ToolBoxPanel != null)
            {
                core.ToolBoxPanel.OnActivateChild(currentActiveWindow);
            }

            UpdateToolBox();
            UpdateStatusBar();

            return;
        }

        ToolBarTray OldChildToolBarTray;
        private void UpdateToolBox()
        {
            if (OldChildToolBarTray != null)
            {
                layoutRoot.Children.Remove(OldChildToolBarTray);
            }
            ToolBarTrayCurrentActiveWindow.ToolBars.Clear();
            if (currentActiveWindow != null)
            {
                ToolBarTray ChildToolBarTray = currentActiveWindow.GetToolBarTray();
                OldChildToolBarTray = ChildToolBarTray;
                layoutRoot.Children.Add(ChildToolBarTray);
                Grid.SetRow(ChildToolBarTray, 2);
            }
        }

        System.Windows.Controls.Primitives.StatusBar OldStatusBar; 
        private void UpdateStatusBar()
        {
            if (IsLoaded == false)
                return;

            if ((OldStatusBar != null) && (GridStatusBars.Children.Contains(OldStatusBar)))
            {
                GridStatusBars.Children.Remove(OldStatusBar);
            }
            if (currentActiveWindow != null)
            {
                System.Windows.Controls.Primitives.StatusBar ChildStatusBar = currentActiveWindow.GetStatusBar();
                OldStatusBar = ChildStatusBar;
                GridStatusBars.Children.Add(ChildStatusBar);
                Grid.SetColumn(ChildStatusBar, 1);
            }
        }

        private void MenuItemOptions_Click(object sender, RoutedEventArgs e)
        {
            Schematix.CommonProperties.Options options = new Schematix.CommonProperties.Options();
            options.ShowDialog();
        }

        private void ComboBoxTopProject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((core.Solution != null) && (core.Solution.CurrentSelectedProject != null) && (ComboBoxTopProject.SelectedItem as Schematix.ProjectExplorer.Project != core.Solution.CurrentSelectedProject) && (core.Solution.CurrentSelectedProject.Compiler.CurrentCompiler.IsBusy == true))
            {
                MessageBox.Show(string.Format("Compiler of project {0} is busy now", core.Solution.CurrentSelectedProject.Caption), "Change project error", MessageBoxButton.OK, MessageBoxImage.Error);
                ComboBoxTopProject.SelectedItem = core.Solution.CurrentSelectedProject;
            }
            else
            {
                if (core.Solution != null)
                {
                    Schematix.ProjectExplorer.Project selProject = ComboBoxTopProject.SelectedItem as Schematix.ProjectExplorer.Project;
                    core.Solution.CurrentSelectedProject = selProject;

                    if (selProject != null)
                    {
                        ContextMenu cm = selProject.CreateElementContextMenu(core.ProjectExplorerPanel.projectExplorerControl);
                        MenuItemProject.Items.Clear();
                        MenuItemProject.Visibility = System.Windows.Visibility.Visible;
                        List<object> items = new List<object>();
                        foreach (object o in cm.Items)
                        {
                            items.Add(o);
                        }
                        cm.Items.Clear();
                        foreach (object o in items)
                            MenuItemProject.Items.Add(o);
                    }
                }
            }
        }

        public void UpdateSolutionData()
        {
            Schematix.ProjectExplorer.Project selProject = null;
            if ((core != null) && (core.Solution != null))
            {
                selProject = core.Solution.CurrentSelectedProject;
                if (selProject == null)
                {
                    if (core.Solution.ProjectList.Count >= 1)
                        selProject = core.Solution.ProjectList[0];
                }
            }
            ComboBoxTopProject.Items.Clear();
            if ((core != null) && (core.Solution != null))
            {
                foreach (Schematix.ProjectExplorer.Project proj in core.Solution.ProjectList)
                {
                    ComboBoxTopProject.Items.Add(proj);
                }
                ComboBoxTopProject.SelectedItem = selProject;
            }
            if ((selProject != null) && (core != null) && (core.ProjectExplorerPanel != null) && (core.ProjectExplorerPanel.projectExplorerControl != null))
            {
                ContextMenu cm = selProject.CreateElementContextMenu(core.ProjectExplorerPanel.projectExplorerControl);
                MenuItemProject.Items.Clear();
                MenuItemProject.Visibility = System.Windows.Visibility.Visible;
                List<object> items = new List<object>();
                foreach(object o in cm.Items)
                {
                    items.Add(o);
                }
                cm.Items.Clear();
                foreach (object o in items)
                    MenuItemProject.Items.Add(o);
            }
            else
            {
                MenuItemProject.Visibility = System.Windows.Visibility.Collapsed;
            }
            textSearcher = new TextSearcher();
            textSearcher.Searcher.StartOffset = 0;
            if (RadioButtonCurrentFileSearch.IsChecked == true)
                textSearcher.Reset(SearchType.CurrentDocument);
            if (RadioButtonCurrentProjectSearch.IsChecked == true)
                textSearcher.Reset(SearchType.CurrentProject);
            if (RadioButtonEntireSolutionSearchSearch.IsChecked == true)
                textSearcher.Reset(SearchType.EntireSolution);
        }

        private void Compile_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (core.CmdConsole != null)
                core.Solution.CurrentSelectedProject.Compiler.ProcessInterface = core.CmdConsole.ProcessInterface;
            core.SaveAll();
            core.Solution.CurrentSelectedProject.Compiler.CompileProject();
        }

        private void Compile_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ((core != null) && (core.IsCompilerBusy != true));
        }

        private void RebuildLibrary_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Schematix.Core.UserControls.ProgressWindow.Window.RunProcess(
            new Schematix.Core.UserControls.MyBackgroundWorker(
                new Action(() =>
                {
                    try
                    {
                        throw new NotSupportedException();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(string.Format("Message: {0}\n StackTrace: {1}\n TargetSite: {2}\n Source: {3}", ex.Message, ex.StackTrace, ex.TargetSite, ex.Source), "Error :(", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void RebuildLibrary_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ((core != null) && (core.IsCompilerBusy != true));
        }

        private void CheckSyntax_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if(core.CmdConsole != null)
                core.Solution.CurrentSelectedProject.Compiler.ProcessInterface = core.CmdConsole.ProcessInterface;
            core.SaveAll();
            core.Solution.CurrentSelectedProject.Compiler.CheckSyntaxProject();
        }

        private void CheckSyntax_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ((core != null) && (core.IsCompilerBusy != true));
        }

        private void Clear_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {            
            e.CanExecute = ((core != null) && (core.IsCompilerBusy != true));
        }

        private void Clear_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (core.CmdConsole != null)
                core.Solution.CurrentSelectedProject.Compiler.ProcessInterface = core.CmdConsole.ProcessInterface;
            core.SaveAll();
            core.Solution.CurrentSelectedProject.Compiler.CleanProject();
        }

        private void Search_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (core != null) && (core.Solution != null) && (core.CurrentSelectedProject != null);
        }

        private void Search_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            core.ShowSearchPanel();
        }

        private void Find_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (core != null) && (core.Solution != null) && (core.CurrentSelectedProject != null);
        }

        private TextSearcher textSearcher;
        private void Find_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            textSearcher.Searcher.SearchedText = SearchBox.Text;

            SearchResult curSearchResult = textSearcher.GetNextSearchResult();
            if (curSearchResult != null)
            {
                Schematix.Windows.Code.Code window = core.OpenNewWindow(curSearchResult.Code.Path) as Schematix.Windows.Code.Code;
                if (window != null)
                {
                    if (window.textEditor.IsLoaded == false)
                    {
                        window.textEditor.Loaded += new RoutedEventHandler(delegate
                            {
                                window.textEditor.Select(curSearchResult.Segment.StartOffset, curSearchResult.Segment.Length);
                                window.IsActive = false;
                                SearchBox.Focus();
                            });
                    }
                    else
                    {
                        window.textEditor.Select(curSearchResult.Segment.StartOffset, curSearchResult.Segment.Length);
                        window.IsActive = false;
                        SearchBox.Focus();
                    }
                }
            }
            else
            {
                MessageBox.Show("Nothing was found, Try again");
                textSearcher.Searcher.StartOffset = 0;
                textSearcher.Searcher.SearchedText = SearchBox.Text;
                if (RadioButtonCurrentFileSearch.IsChecked == true)
                    textSearcher.Reset(SearchType.CurrentDocument);
                if (RadioButtonCurrentProjectSearch.IsChecked == true)
                    textSearcher.Reset(SearchType.CurrentProject);
                if (RadioButtonEntireSolutionSearchSearch.IsChecked == true)
                    textSearcher.Reset(SearchType.EntireSolution);
            }
        }

        private void UpdateSearchType(SearchType searchType)
        {
            if (textSearcher != null)
            {
                textSearcher.Reset(searchType);
            }
        }

        private void RadioButtonCurrentFileSearch_Checked(object sender, RoutedEventArgs e)
        {
            UpdateSearchType(SearchType.CurrentDocument);
        }

        private void RadioButtonCurrentProjectSearch_Checked(object sender, RoutedEventArgs e)
        {
            UpdateSearchType(SearchType.CurrentProject);
        }

        private void RadioButtonEntireSolutionSearchSearch_Checked(object sender, RoutedEventArgs e)
        {
            UpdateSearchType(SearchType.EntireSolution);
        }

        private void SearchBox_TextInput(object sender, TextCompositionEventArgs e)
        {
            textSearcher.Searcher.SearchedText = SearchBox.Text;
            textSearcher.Searcher.StartOffset = 0;
            if (RadioButtonCurrentFileSearch.IsChecked == true)
                textSearcher.Reset(SearchType.CurrentDocument);
            if (RadioButtonCurrentProjectSearch.IsChecked == true)
                textSearcher.Reset(SearchType.CurrentProject);
            if (RadioButtonEntireSolutionSearchSearch.IsChecked == true)
                textSearcher.Reset(SearchType.EntireSolution);
        }
        
        System.Windows.Threading.DispatcherTimer perfTimer;
        PerformanceCounter cpuCounter;
		PerformanceCounter ramCounter;
		
		void StatusBarMain_Loaded(object sender, RoutedEventArgs e)
		{
			cpuCounter = new PerformanceCounter();
		
			cpuCounter.CategoryName = "Processor";
			cpuCounter.CounterName = "% Processor Time";
			cpuCounter.InstanceName = "_Total";		
			
			ramCounter = new PerformanceCounter("Memory", "Available MBytes");
				
			perfTimer = new System.Windows.Threading.DispatcherTimer();
			perfTimer.Tick += new EventHandler(perfTimer_Tick); 
    		perfTimer.Interval = new TimeSpan(0, 0, 1); 
    		perfTimer.Start();
		}
		
		private void perfTimer_Tick(object sender, EventArgs e)
		{
			float cpuUsage = getCurrentCpuUsage();
			float memAvaliable = getAvailableRAM();
			
			LabelMemUsage.Content = memAvaliable + "MB";
			LabelProcUsage.Content = (int)cpuUsage + "%";
			ProgressBarProc.Value = cpuUsage;
			
			MemoryStatus stat = new MemoryStatus();
		    GlobalMemoryStatus(out stat);
			ProgressBarMem.Value = stat.MemoryLoad;
		}
		
		
		public float getCurrentCpuUsage(){
		            return cpuCounter.NextValue();
		}
		
		public float getAvailableRAM(){
		            return ramCounter.NextValue();
		}	

		
		public struct MemoryStatus
		{
				public uint Length;
		       public uint MemoryLoad;
		       public uint TotalPhysical;
		       public uint AvailablePhysical;
		       public uint TotalPageFile;
		       public uint AvailablePageFile;
		       public uint TotalVirtual;
		       public uint AvailableVirtual;
		}
		
		[System.Runtime.InteropServices.DllImport("kernel32.dll")]
		public static extern void GlobalMemoryStatus(out MemoryStatus stat);


    }
}
