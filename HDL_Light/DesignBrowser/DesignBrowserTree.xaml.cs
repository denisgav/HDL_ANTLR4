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
using Schematix.ProjectExplorer;

namespace Schematix.DesignBrowser
{
    /// <summary>
    /// Interaction logic for DesignBrowserTree.xaml
    /// </summary>
    public partial class DesignBrowserTree : UserControl
    {
        /// <summary>
        /// Текущее решение проекта
        /// </summary>
        public Solution Solution
        {
            get { return core.Solution; }
        }

        /// <summary>
        /// Главный узел
        /// </summary>
        private SchematixCore core;
        public SchematixCore Core
        {
            get { return core; }
        }

        public DesignBrowserTree()
            : this(SchematixCore.Core)
        { }

        public DesignBrowserTree(SchematixCore core)
        {
            this.core = core;
            InitializeComponent();
            CreateCommands();
        }

        /// <summary>
        /// обновить отображение
        /// </summary>
        public void UpdateInfo()
        {
            TreeViewItem item_root = new TreeViewItem();
            TreeViewDesignBrowser.Items.Clear();
            if (this.Solution != null)
            {
                LoadSubelements(TreeViewDesignBrowser.Items);
            }
        }

        private void LoadSubelements(ItemCollection item_parent)
        {
            TreeViewItem item_sc = new TreeViewItem();
            item_sc.Tag = Solution;

            item_sc.Header = CreateHeader(Solution);
            item_sc.IsExpanded = true;
            item_parent.Add(item_sc);

            foreach (Schematix.ProjectExplorer.Project pr in Solution.ProjectList)
            {
                TreeViewItem item_pr = new TreeViewItem();
                item_pr.Tag = pr;

                item_pr.Header = CreateHeader(pr);
                item_pr.IsExpanded = false;
                item_sc.Items.Add(item_pr);
                if (pr.Compiler.CurrentCompiler is Schematix.Core.Compiler.VHDLCompiler)
                {
                    Schematix.Core.Compiler.VHDLCompiler compiler = pr.Compiler.CurrentCompiler as Schematix.Core.Compiler.VHDLCompiler;
                    foreach (VHDL.VhdlElement el in DesignChildProvider.GetSubElements(compiler.RootScope))
                        if (el != null)
                            LoadSubelements(item_pr.Items, el);
                }
            }
        }

        private void LoadSubelements(ItemCollection item_parent, VHDL.VhdlElement designElement)
        {
            TreeViewItem item_el = new TreeViewItem();
            item_el.Tag = designElement;

            item_el.Header = CreateHeader(designElement);
            item_parent.Add(item_el);
            IList<VHDL.VhdlElement> childrens = DesignChildProvider.GetSubElements(designElement);
            if (childrens.Count != 0)
            {
                item_el.IsExpanded = ((designElement is VHDL.libraryunit.PackageDeclaration) == false);
                foreach (VHDL.VhdlElement el in childrens)
                    if (el != null)
                        LoadSubelements(item_el.Items, el);
            }
        }

        private static Image CreateImage(VHDL.VhdlElement designElement)
        {
            string path = string.Empty;

            if (designElement is VHDL.libraryunit.LibraryClause)
                path = "/HDL_Light;component/Images/Design/library.png";
            if (designElement is VHDL.libraryunit.PackageDeclaration)
                path = "/HDL_Light;component/Images/Design/package.png";
            if (designElement is VHDL.libraryunit.Architecture)
                path = "/HDL_Light;component/Images/Design/architecture.png";
            if (designElement is VHDL.libraryunit.Entity)
                path = "/HDL_Light;component/Images/Design/chip.png";
            if (designElement is VHDL.declaration.FunctionDeclaration)
                path = "/HDL_Light;component/Images/Design/function.png";
            if (designElement is VHDL.declaration.ProcedureDeclaration)
                path = "/HDL_Light;component/Images/Design/procedure.png";
            if (designElement is VHDL.concurrent.ProcessStatement)
                path = "/HDL_Light;component/Images/Design/process.png";

            BitmapImage b_img = new BitmapImage(new Uri(path, UriKind.Relative));
            Image img = new Image() { Source = b_img, Width = 16, Height = 16, };

            return img;
        }

        /// <summary>
        /// Создание объекта Header для элемента TreeView
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        private object CreateHeader(ProjectElementBase elem)
        {
            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;
            BitmapImage b_img = new BitmapImage(new Uri(elem.Icon, UriKind.Relative));
            Image img = new Image() { Source = b_img, Width = 16, Height = 16, };
            TextBlock text = new TextBlock() { Text = elem.Caption };
            text.Tag = elem;
            elem.Tag = panel;

            panel.Children.Add(img);
            panel.Children.Add(text);

            return panel;
        }

        /// <summary>
        /// Создание объекта Header для элемента TreeView
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        private object CreateHeader(VHDL.VhdlElement elem)
        {
            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;
            Image img = CreateImage(elem);
            TextBlock text = new TextBlock() { Text = elem.ToString() };
            text.Tag = elem;

            panel.Children.Add(img);
            panel.Children.Add(text);

            ContextMenu menu = CreateContextMenuForElement(elem);
            if (menu.HasItems == true)
                panel.ContextMenu = menu;

            return panel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private ContextMenu CreateContextMenuForElement(VHDL.VhdlElement elem)
        {
            ContextMenu res = new ContextMenu();
            if (elem is VHDL.libraryunit.Architecture)
            {
                MenuItem simulateItem = new MenuItem();
                simulateItem.Header = "Simulate";
                simulateItem.Command = commandSimulate;
                simulateItem.CommandBindings.Add(bindingSimulateCommand);
                res.Items.Add(simulateItem);

                MenuItem createTBDiagramItem = new MenuItem();
                createTBDiagramItem.Header = "Create TB diagramm";
                createTBDiagramItem.Command = commandCreateTestBenchDiadramm;
                createTBDiagramItem.CommandBindings.Add(bindingCreateTestBenchDiadrammCommand);
                res.Items.Add(createTBDiagramItem);
            }
            MenuItem ShowCodeItem = new MenuItem();
            ShowCodeItem.Header = "Show Code";
            ShowCodeItem.Tag = elem;
            ShowCodeItem.Click += new RoutedEventHandler(ShowCodeItem_Click);
            res.Items.Add(ShowCodeItem);
            return res;
        }

        void ShowCodeItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item_el = sender as MenuItem;
            if (item_el != null)
            {
                VHDL.VhdlElement designElement = item_el.Tag as VHDL.VhdlElement;
                if ((designElement != null) && (designElement.AnnotationList != null))
                {
                    List<object> annotations = designElement.AnnotationList[designElement];
                    if (annotations != null)
                    {
                        foreach (object o in annotations)
                        {
                            if (o is VHDL.annotation.PositionInformation)
                            {
                                VHDL.annotation.PositionInformation pos = (o as VHDL.annotation.PositionInformation);
                                string FileName = pos.FileName;
                                int line = pos.Begin.Line;
                                int col = pos.Begin.Column;
                                core.OpenSource(FileName, true, null, line, col);
                                e.Handled = true;
                                return;
                            }
                        }
                    }
                }
            }
        }

        #region Commands

        private RoutedCommand commandSimulate;
        public RoutedCommand CommandSimulate
        { get { return commandSimulate; } }
        private CommandBinding bindingSimulateCommand;
        public CommandBinding BindingSimulateCommand
        { get { return bindingSimulateCommand; } }

        private RoutedCommand commandCreateTestBenchDiadramm;
        public RoutedCommand CommandCreateTestBenchDiadramm
        { get { return commandCreateTestBenchDiadramm; } }
        private CommandBinding bindingCreateTestBenchDiadrammCommand;
        public CommandBinding BindingCreateTestBenchDiadrammCommand
        { get { return bindingCreateTestBenchDiadrammCommand; } }

        private void CreateCommands()
        {
            commandSimulate = new RoutedCommand("Simulate", typeof(DesignBrowserTree), new InputGestureCollection(new InputGesture[] { }));
            bindingSimulateCommand = new CommandBinding(commandSimulate, new ExecutedRoutedEventHandler(Simulate_Executed), new CanExecuteRoutedEventHandler(Simulate_CanExecute));

            commandCreateTestBenchDiadramm = new RoutedCommand("CreateTestBenchDiadramm", typeof(DesignBrowserTree), new InputGestureCollection(new InputGesture[] { }));
            bindingCreateTestBenchDiadrammCommand = new CommandBinding(commandCreateTestBenchDiadramm, new ExecutedRoutedEventHandler(CreateTestBenchDiadramm_Executed), new CanExecuteRoutedEventHandler(CreateTestBenchDiadramm_CanExecute));
        }
        #endregion


        

        private void ButtomUpdate_Click(object sender, RoutedEventArgs e)
        {
            UpdateInfo();
        }

        /// <summary>
        /// Получение параметров {имя архитектуры, entity, имя файла}
        /// с текущего выбранного элемента в дереве
        /// </summary>
        /// <param name="ArchName"></param>
        /// <param name="EntityName"></param>
        /// <param name="fileName"></param>
        private bool FormArgumentsForSimulation(out string ArchName, out string EntityName, out string FileName)
        {
            ArchName = string.Empty;
            EntityName = string.Empty;
            FileName = string.Empty;

            if ((TreeViewDesignBrowser.SelectedItem == null) || ((TreeViewDesignBrowser.SelectedItem is TreeViewItem) == false))
                return false;

            VHDL.libraryunit.Architecture arch = (TreeViewDesignBrowser.SelectedItem as TreeViewItem).Tag as VHDL.libraryunit.Architecture;
            if (arch != null)
            {
                ArchName = arch.Identifier;
                EntityName = arch.Entity.Identifier;

                if (arch.AnnotationList.ContainsKey(arch))
                {
                    List<object> annotations = arch.AnnotationList[arch];
                    if (annotations != null)
                    {
                        foreach (object o in annotations)
                        {
                            if (o is VHDL.annotation.PositionInformation)
                            {
                                FileName = (o as VHDL.annotation.PositionInformation).FileName;
                                return true;
                            }
                        }
                    }
                }                
            }
            return false;
        }

        private void Simulate_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string archName = string.Empty;
            string entityName = string.Empty;
            string fileName = string.Empty;

            bool res = FormArgumentsForSimulation(out archName, out entityName, out fileName);

            if(res == true)
                core.StartVHDLSimulation(archName, entityName, fileName);
        }

        private void Simulate_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ((core != null) && (core.IsCompilerBusy != true));
        }

        private void CreateTestBenchDiadramm_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string archName = string.Empty;
            string entityName = string.Empty;
            string fileName = string.Empty;

            bool res = FormArgumentsForSimulation(out archName, out entityName, out fileName);

            if (res == true)
            {
                Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
                sfd.Filter = "VCD Files|*.vcd";
                sfd.InitialDirectory = System.IO.Path.GetDirectoryName(fileName);
                sfd.FileName = System.IO.Path.GetFileNameWithoutExtension(fileName)+"_TB_Diagramm";
                sfd.Title = "Save VCD file";
                if (sfd.ShowDialog() == true)
                {
                    core.StartVHDLSimulation(archName, entityName, fileName, sfd.FileName);
                }
            }
        }

        private void CreateTestBenchDiadramm_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            bool suggestion1 = ((core != null) && (core.IsCompilerBusy != true));
            if(suggestion1 == false)
            {
                e.CanExecute = false;
                return;
            }
            bool suggestion2 = ((TreeViewDesignBrowser != null) && (TreeViewDesignBrowser.SelectedItem != null));
            if (suggestion2 == false)
            {
                e.CanExecute = false;
                return;
            }
            VHDL.libraryunit.Architecture arch = (TreeViewDesignBrowser.SelectedItem as TreeViewItem).Tag as VHDL.libraryunit.Architecture;
            if (arch != null)
            {
                VHDL.libraryunit.Entity entity = arch.Entity;
                if ((entity != null) && (entity.Port.Count != 0))
                {
                    e.CanExecute = true;
                    return;
                }
            }
            e.CanExecute = false;
        }
    }
}
