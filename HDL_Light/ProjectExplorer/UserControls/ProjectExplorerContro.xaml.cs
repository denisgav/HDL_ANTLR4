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
using Schematix.Waveform.UserControls;
using System.Collections.ObjectModel;

namespace Schematix.ProjectExplorer
{
    /// <summary>
    /// Interaction logic for ProjectExplorerControl.xaml
    /// </summary>
    public partial class ProjectExplorerControl : UserControl
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

        

        #region CommandsRegion
        private RoutedCommand commandPaste;
        public RoutedCommand CommandPaste
        { get { return commandPaste; } }

        private RoutedCommand commandCut;
        public RoutedCommand CommandCut
        { get { return commandCut; } }

        private RoutedCommand commandCopy;
        public RoutedCommand CommandCopy
        { get { return commandCopy; } }

        private RoutedCommand commandRemove;
        public RoutedCommand CommandRemove
        { get { return commandRemove; } }

       private CommandBinding bindingPasteCommand;
       public CommandBinding BindingPasteCommand
       { get { return bindingPasteCommand; } }

       private CommandBinding bindingCutCommand;
       public CommandBinding BindingCutCommand
       { get { return bindingCutCommand; } }

       private CommandBinding bindingCopyCommand;
       public CommandBinding BindingCopyCommand
       { get { return bindingCopyCommand; } }

       private CommandBinding bindingRemoveCommand;
       public CommandBinding BindingRemoveCommand
       { get { return bindingRemoveCommand; } }

        private void CreateCommands()
        {
            commandPaste = new RoutedCommand("Paste", typeof(ProjectExplorerControl), new InputGestureCollection(new InputGesture[] {  }));
            bindingPasteCommand = new CommandBinding(commandPaste, new ExecutedRoutedEventHandler(Paste_Executed), new CanExecuteRoutedEventHandler(Paste_CanExecute));

            commandCut = new RoutedCommand("Cut", typeof(ProjectExplorerControl), new InputGestureCollection(new InputGesture[] {  }));
            bindingCutCommand = new CommandBinding(commandCut, new ExecutedRoutedEventHandler(Cut_Executed), new CanExecuteRoutedEventHandler(Cut_CanExecute));

            commandCopy = new RoutedCommand("Copy", typeof(ProjectExplorerControl), new InputGestureCollection(new InputGesture[] {  }));
            bindingCopyCommand = new CommandBinding(commandCopy, new ExecutedRoutedEventHandler(Copy_Executed), new CanExecuteRoutedEventHandler(Copy_CanExecute));

            commandRemove = new RoutedCommand("Remove", typeof(ProjectExplorerControl), new InputGestureCollection(new InputGesture[] {  }));
            bindingRemoveCommand = new CommandBinding(commandRemove, new ExecutedRoutedEventHandler(Remove_Executed), new CanExecuteRoutedEventHandler(Remove_CanExecute));
        }

        private void Paste_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //1 выбранный элемент
            //этот элемент может принять данные
            if (selectedItems.Count != 0)
            {
                TreeViewItem item = selectedItems[0];
                if (item != null)
                {
                    ProjectElementBase elem = item.Tag as ProjectElementBase;
                    elem.Paste_CanExecute(sender, e);
                }
            }
        }

        private void Paste_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (selectedItems.Count != 0)
            {
                TreeViewItem item = selectedItems[0];
                if (item != null)
                {
                    ProjectElementBase elem = item.Tag as ProjectElementBase;
                    elem.Paste_Executed(sender, e);
                }

                Solution.Save();
                UpdateInfo();
            }
        }

        private void Remove_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (selectedItems.Count != 0)
            {
                e.CanExecute = true;

                foreach (TreeViewItem i in selectedItems)
                {
                    if (i != null)
                    {
                        ProjectElementBase elem = i.Tag as ProjectElementBase;
                        elem.Remove_CanExecute(sender, e);
                    }
                }
            }
        }

        private void Remove_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (selectedItems.Count != 0)
            {
                foreach (TreeViewItem i in selectedItems)
                {
                    if (i != null)
                    {
                        ProjectElementBase elem = i.Tag as ProjectElementBase;
                        elem.Remove_Executed(sender, e);
                    }
                }
            }
        }

        private void Cut_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (selectedItems.Count != 0)
            {
                List<ProjectElementBase> elements = new List<ProjectElementBase>();
                foreach (TreeViewItem i in selectedItems)
                {
                    if (i != null)
                    {
                        ProjectElementBase elem = i.Tag as ProjectElementBase;
                        elements.Add(elem);
                    }
                }

                e.CanExecute = (ClipboardBufferData.CheckGroupValid(elements) != GroupType.IllegalGroup);
            }

            TreeViewItem item = TreeViewExplorer.SelectedItem as TreeViewItem;
            if (item != null)
            {
                ProjectElementBase elem = item.Tag as ProjectElementBase;
                elem.Copy_CanExecute(sender, e);
            }
        }

        private void Cut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (selectedItems.Count != 0)
            {
                foreach (TreeViewItem i in selectedItems)
                {
                    if (i != null)
                    {
                        ProjectElementBase elem = i.Tag as ProjectElementBase;
                        elem.Cut_Executed(sender, e);
                    }
                }

                List<ProjectElementBase> elements = new List<ProjectElementBase>();
                foreach (TreeViewItem i in selectedItems)
                {
                    if (i != null)
                    {
                        ProjectElementBase elem = i.Tag as ProjectElementBase;
                        elements.Add(elem);
                    }
                }

                ClipboardBufferData data = new ClipboardBufferData(ClipboardOperationType.Cut, elements);
                if (ClipboardBufferData.CanSendToClipboard(data))
                    ClipboardBufferData.SendToClipboard(data);                
            }
        }

        private void Copy_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (selectedItems.Count != 0)
            {
                List<ProjectElementBase> elements = new List<ProjectElementBase>();
                foreach (TreeViewItem i in selectedItems)
                {
                    if (i != null)
                    {
                        ProjectElementBase elem = i.Tag as ProjectElementBase;
                        elements.Add(elem);
                    }
                }
                    
                e.CanExecute = (ClipboardBufferData.CheckGroupValid(elements) != GroupType.IllegalGroup);

                foreach (TreeViewItem i in selectedItems)
                {
                    if (i != null)
                    {
                        ProjectElementBase elem = i.Tag as ProjectElementBase;
                        elem.Copy_CanExecute(sender, e);
                    }
                }
            }
        }

        private void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (selectedItems.Count != 0)
            {
                List<ProjectElementBase> elements = new List<ProjectElementBase>();
                foreach (TreeViewItem i in selectedItems)
                {
                    if (i != null)
                    {
                        ProjectElementBase elem = i.Tag as ProjectElementBase;
                        elements.Add(elem);
                    }
                }

                ClipboardBufferData data = new ClipboardBufferData(ClipboardOperationType.Copy, elements);
                if (ClipboardBufferData.CanSendToClipboard(data))
                    ClipboardBufferData.SendToClipboard(data);

                foreach (TreeViewItem i in selectedItems)
                {
                    if (i != null)
                    {
                        ProjectElementBase elem = i.Tag as ProjectElementBase;
                        elem.Copy_Executed(sender, e);
                    }
                }
            }
        }
        
        #endregion

        public ProjectExplorerControl()
            : this(SchematixCore.Core)
        { }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="core"></param>
        public ProjectExplorerControl(SchematixCore core)
        {
            this.core = core;
            selectedItems = new ObservableCollection<TreeViewItem>();
            CreateCommands();
            InitializeComponent();
        }

        /// <summary>
        /// Компонент загрузился
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewExplorer_Loaded(object sender, RoutedEventArgs e)
        {
            TreeViewExplorer.DataContext = this;
            UpdateInfo();
        }

        private void LoadSubelements(ProjectElementBase elem, ItemCollection item_parent)
        {
            TreeViewItem item_sc = new TreeViewItem();
            item_sc.Tag = elem;

            item_sc.Header = CreateHeader(elem);

            if (elem is ProjectFolder)
                item_sc.IsExpanded = (elem as ProjectFolder).Expanded;
            if (elem is SolutionFolder)
                item_sc.IsExpanded = (elem as SolutionFolder).Expanded;
            if (elem is Solution)
                item_sc.IsExpanded = (elem as Solution).RootFolder.Expanded;
            if (elem is Project)
                item_sc.IsExpanded = (elem as Project).RootFolder.Expanded;

            if ((elem.Childrens != null) && (elem.Childrens.Count != 0))
            {
                foreach (ProjectElementBase item in elem.Childrens)
                {
                    LoadSubelements(item, item_sc.Items);
                }
            }
            item_parent.Add(item_sc);
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
            EditableTextBlock text = new EditableTextBlock() { Text = elem.Caption };
            text.Tag = elem;
            elem.Tag = panel;

            text.PreviewMouseDoubleClick += new MouseButtonEventHandler(TreeViewIcon_PreviewMouseDoubleClick);
            //text.PreviewMouseDoubleClick += new MouseButtonEventHandler(EditableTextBlock_PreviewMouseDoubleClick);
            text.OnIsInEditModeChanged += new EditableTextBlock.OnIsInEditModeChangedDelegate(text_OnIsInEditModeChanged);
            panel.Children.Add(img);
            panel.Children.Add(text);
            panel.ContextMenu = elem.CreateElementContextMenu(this);
            return panel;
        }        

        void text_OnIsInEditModeChanged(EditableTextBlock sender, bool IsEditable)
        {
            if (IsEditable == false)
            {
                ProjectElementBase elem = (sender.Tag) as ProjectElementBase;
                try
                {
                    elem.Caption = sender.Text;
                }
                catch (Exception ex)
                {
                    sender.Text = elem.Caption;
                    Schematix.Core.Logger.Log.Error("OnCaptionChange error.", ex);
                    System.Windows.MessageBox.Show(string.Format("Could not rename file.\n", ex.Message), "File system error ocurred", System.Windows.MessageBoxButton.OK);
                }
            }
        }

        /// <summary>
        /// обновить отображение
        /// </summary>
        public void UpdateInfo()
        {
            TreeViewItem item_root = new TreeViewItem();
            TreeViewExplorer.Items.Clear();
            if (this.Solution != null)
            {
                LoadSubelements(this.Solution, TreeViewExplorer.Items);                
            }
        }

        public void Solution_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {           
            UpdateInfo();           
        }

        private void EditableTextBlock_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditableTextBlock etb = sender as EditableTextBlock;

            if (etb != null)
            {
                // Finally make sure that we are
                // allowed to edit the TextBlock
                if (etb.IsEditable)
                    etb.IsInEditMode = true;
            }
        }

        private void TreeViewIcon_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditableTextBlock etb = sender as EditableTextBlock;

            if (etb != null)
            {
                ProjectElementBase elem = etb.Tag as ProjectElementBase;
                if (elem != null)
                {
                    core.OpenNewWindow(elem);
                }
            }
        }

        private void TreeViewExplorer_Collapsed(object sender, RoutedEventArgs e)
        {
            ProjectElementBase elem = (e.OriginalSource as TreeViewItem).Tag as ProjectElementBase;
            if (elem is Solution)
                (elem as Solution).RootFolder.Expanded = true;
            if (elem is Project)
                (elem as Project).RootFolder.Expanded = true;
            if (elem is ProjectFolder)
                (elem as ProjectFolder).Expanded = true;
            if (elem is SolutionFolder)
                (elem as SolutionFolder).Expanded = true;
        }

        private void TreeViewExplorer_Expanded(object sender, RoutedEventArgs e)
        {
            ProjectElementBase elem = (e.OriginalSource as TreeViewItem).Tag as ProjectElementBase;
            if (elem is Solution)
                (elem as Solution).RootFolder.Expanded = true;
            if (elem is Project)
                (elem as Project).RootFolder.Expanded = true;
            if (elem is ProjectFolder)
                (elem as ProjectFolder).Expanded = true;
            if (elem is SolutionFolder)
                (elem as SolutionFolder).Expanded = true;
        }

        #region Drag/Drop Events


        public ObservableCollection<TreeViewItem> selectedItems
        {
            get { return (ObservableCollection<TreeViewItem>)GetValue(selectedItemsProperty); }
            set { SetValue(selectedItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for selectedItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty selectedItemsProperty =
            DependencyProperty.Register("MySelectedItems", typeof(ObservableCollection<TreeViewItem>), typeof(ProjectExplorerControl), new UIPropertyMetadata(null));

        

        

        private Point startPoint;

        private void TreeViewExplorer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        private void TreeViewExplorer_MouseMove(object sender, MouseEventArgs e)
        {
            TreeViewItem item = TreeViewExtensions.GetNearestContainer(e.OriginalSource as UIElement);
            if ((e.LeftButton == MouseButtonState.Pressed) && (item != null))
            {
                var mousePos = e.GetPosition(null);
                var diff = startPoint - mousePos;

                if (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance
                    || Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    if (selectedItems.Count != 0)
                    {
                        List<ProjectElementBase> elements = new List<ProjectElementBase>();
                        foreach (TreeViewItem i in selectedItems)
                        {
                            if (i != null)
                            {
                                ProjectElementBase elem = i.Tag as ProjectElementBase;
                                elements.Add(elem);
                            }
                        }

                        if (ClipboardBufferData.CheckGroupValid(elements) != GroupType.IllegalGroup)
                        {
                            var dragData = new DataObject(new ClipboardBufferData(ClipboardOperationType.Cut, elements));
                            DragDrop.DoDragDrop(TreeViewExplorer, dragData, DragDropEffects.Move | DragDropEffects.None);
                        }                        
                    }                    
                }
            }
        }

        private void TreeViewExplorer_Drop(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent(typeof(ClipboardBufferData)))
            {
                ClipboardBufferData data = e.Data.GetData(typeof(ClipboardBufferData)) as ClipboardBufferData;

                Point currentPosition = e.GetPosition(TreeViewExplorer);

                TreeViewItem item = TreeViewExtensions.GetNearestContainer(e.OriginalSource as UIElement);

                //Можно ли вставить данные сюда?

                if (item == null)
                    return;

                var elem = item.Tag as ProjectElementBase;

                if (elem == null)
                    return;

                if (elem is IDropable)
                    (elem as IDropable).Drop(data);
            }
            
        }

        private void TreeViewExplorer_DragOver(object sender, DragEventArgs e)
        {
            try
            {
                Point currentPosition = e.GetPosition(TreeViewExplorer);


                if ((Math.Abs(currentPosition.X - startPoint.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                    (Math.Abs(currentPosition.Y - startPoint.Y) > SystemParameters.MinimumVerticalDragDistance))
                {
                    // Verify that this is a valid drop and then store the drop target
                    TreeViewItem item = TreeViewExtensions.GetNearestContainer(e.OriginalSource as UIElement);

                    //Можно ли вставить данные сюда?

                    var elem = item.Tag as ProjectElementBase;
                    ClipboardBufferData data = e.Data.GetData(typeof(ClipboardBufferData)) as ClipboardBufferData;

                    e.Effects = ((elem is IDropable) && ((elem as IDropable).CanDrop(data))) ? DragDropEffects.Move : DragDropEffects.None;
                    e.Handled = true;
                    return;
                    
                }
                e.Effects = DragDropEffects.None;
                e.Handled = true;
                return;
            }
            catch (Exception)
            {
            }
        }
        #endregion        
    }
}
