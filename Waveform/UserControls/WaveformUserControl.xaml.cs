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
using System.IO;
using Schematix.Waveform.Value_Dump;
using Schematix.Waveform.UserControls;
using System.ComponentModel;
using DataContainer;
using DataContainer.Objects;

namespace Schematix.Waveform
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class WaveformUserControl : UserControl, INotifyPropertyChanged
    {
        private WaveformCore core;
        private ListViewDragDropManager dragDropManager;
        private CursorViewer cursorViewer;
        private List<Line> lines;

        public WaveformCore Core
        {
            get { return core; }
        }

        public WaveformUserControl()
        {
            InitializeComponent();
            core = new WaveformCore(this);
            dragDropManager = new ListViewDragDropManager(core, ListViewMain);
            dragDropManager.UpdateViewEvent += new UpdateView(dragDropManager_UpdateViewEvent);
            core.ScaleManager = new ScaleManager(core, timeScaleViewer);
            timeScaleViewer.ScaleManager = core.ScaleManager;
            timeScaleViewer.ScaleManager.PropertyChanged += new PropertyChangedEventHandler(ScaleManager_PropertyChanged);
            cursorViewer = new CursorViewer(this, core);
            cursorViewer.CursorPositionChangedEvent += new UpdateView(cursorViewer_CursorPositionChangedEvent);
            TreeListView.AllowMultiSelection(ListViewMain);
            core.CursorViewer = cursorViewer;
            actionMode = WaveformActionMode.Cursor;
            TimeMeasureDataView1.Core = core;
            lines = new List<Line>(); ;
            core.TimeMeasureList.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(TimeMeasureList_CollectionChanged);

            runEvent = new RunDelegate(CustomRunCommandHandler);
        }

        void TimeMeasureList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (core.TimeMeasureList.Count == 0)
                TimeMeasureDataView1.Visibility = System.Windows.Visibility.Collapsed;
            else
                TimeMeasureDataView1.Visibility = System.Windows.Visibility.Visible;
        }

        public double? GetYPos(My_Variable var)
        {
            foreach(object o in ListViewMain.Items)
            {
                TreeViewItem tlw = o as TreeViewItem;
                My_VariableBindingData bd = tlw.Header as My_VariableBindingData;
                if (bd.Variable == var)
                {
                    try
                    {
                        Point pt = tlw.PointToScreen(new Point(0, 0));
                        return pt.Y;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Тип текущих действий
        /// </summary>
        private WaveformActionMode actionMode;
        public WaveformActionMode ActionMode
        {
            get { return actionMode; }
            set 
            {
                if ((ToggleButtonHand != null) && (ToggleButtonCursor != null) && (ToggleButtonMeasureTime != null))
                {
                    actionMode = value;
                    switch (value)
                    {
                        case WaveformActionMode.Cursor:
                            ToggleButtonCursor.IsChecked = true;
                            ToggleButtonHand.IsChecked = false;
                            ToggleButtonMeasureTime.IsChecked = false;
                            break;
                        case WaveformActionMode.Hand:
                            ToggleButtonCursor.IsChecked = false;
                            ToggleButtonHand.IsChecked = true;
                            ToggleButtonMeasureTime.IsChecked = false;
                            break;
                        case WaveformActionMode.Measure:
                            ToggleButtonCursor.IsChecked = false;
                            ToggleButtonHand.IsChecked = false;
                            ToggleButtonMeasureTime.IsChecked = true;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public void SetLines(List<Line> newLines)
        {
            foreach (Line l in lines)
                GridMain.Children.Remove(l);
            lines.Clear();
            lines.AddRange(newLines);
            foreach (Line l in lines)
            {
                Point p1 = GridMain.PointFromScreen(new Point(l.X1, l.Y1));
                Point p2 = GridMain.PointFromScreen(new Point(l.X2, l.Y2));

                if (p1.Y < GridViewColumnHeaderTimescale.ActualHeight)
                {
                    p1.Y = GridViewColumnHeaderTimescale.ActualHeight;
                }

                if (p2.Y < GridViewColumnHeaderTimescale.ActualHeight)
                {
                    p2.Y = GridViewColumnHeaderTimescale.ActualHeight;
                }

                l.X1 = p1.X;
                l.X2 = p2.X;
                l.Y1 = p1.Y;
                l.Y2 = p2.Y;
                l.Stroke = new SolidColorBrush(Color.FromArgb(127, 127, 127, 127));
                l.StrokeThickness = 1.0;
                l.IsHitTestVisible = false;
                Grid.SetRow(l, 0);
                Grid.SetRowSpan(l, 2);

                GridMain.Children.Add(l);
            }                
        }

        void ScaleManager_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged("ScaleManager");
        }

        /// <summary>
        /// Доступна ли волновая диаграмма для редактирования
        /// </summary>
        private bool allowEdit;
        public bool AllowEdit
        {
            get { return allowEdit; }
            set 
            {
                allowEdit = value;
                //ToolBarTrayEdit.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                ToolBarSimpleGenerator.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                ToolBarBusGenerator.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                ButtonSetGenerator.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                ButtonResizeDiagramm.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }


        void cursorViewer_CursorPositionChangedEvent()
        {
            timeScaleViewer.TextBlockCurrentTime.Margin = new Thickness(cursorViewer.PosX + 3, 3, 0, 0);
            timeScaleViewer.TextBlockCurrentTime.Text = cursorViewer.TimeString;

            //Работа с выделением
            Selection selection = cursorViewer.Selection;
            if (selection != null)
            {
                NotifyPropertyChanged("Cursor Position");
            }
        }

        void dragDropManager_UpdateViewEvent()
        {
            //TimeMeasureDataView1.UpdateCanvas();
        }

        /// <summary>
        /// Загрузка данных с VCD файла
        /// </summary>
        /// <param name="FileName"></param>
        public void LoadVCDFile(Stream stream, string confPath)
        {
            core.LoadVCDFile(stream, confPath);
            UpdateView();
        }

        /// <summary>
        /// Загрузка данных с VCD файла
        /// </summary>
        /// <param name="FileName"></param>
        public void LoadVCDFile(string FileName)
        {
            core.LoadVCDFile(FileName);
            UpdateView();
        }

        /// <summary>
        /// Сохранение данных в VCD файл
        /// </summary>
        /// <param name="FileName"></param>
        public void SaveVCDFile(string FileName)
        {
            core.SaveVCDFile(FileName);
        }

        /// <summary>
        /// Сохранение данных в VCD файл
        /// </summary>
        /// <param name="FileName"></param>
        public void SaveVCDFile(Stream stream, string confPath)
        {
            core.SaveVCDFile(stream, confPath);
        }

        /// <summary>
        /// Проанализировать конкретный Entity и вывести порты
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="ArchitectureName"></param>
        /// <param name="EntityName"></param>
        public void AnalyseVHDLFile(string FilePath, string ArchitectureName, string EntityName)
        {
            core.AnalyseVHDLFile(FilePath, ArchitectureName, EntityName);
            //UpdateView();
        }

        /// <summary>
        /// Разрешить ли редактирование открытой волновой диаграммы
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="ArchitectureName"></param>
        /// <param name="EntityName"></param>
        /// <returns></returns>
        public bool AllowEditForEntity(string FilePath, string ArchitectureName, string EntityName)
        {
            return core.AllowEditForEntity(FilePath, ArchitectureName, EntityName);
        }

        /// <summary>
        /// Генерация TestBench по полученной волновой диаграмме
        /// </summary>
        /// <param name="FilePath"></param>
        public void GenerateTestBench(string FilePath)
        {
            core.GenerateTestBench(FilePath);
        }

        public void ShowTableView()
        {
            TableView table = new TableView(core);
            table.ShowDialog();
        }

        /// <summary>
        /// Обновление данных на ListView
        /// </summary>
        public void UpdateView()
        {
            TextBlockInfo.Text = core.SummaryInfo;
            UpdateTreeView();
            UpdateSignalView();

            TimeMeasureDataView1.UpdateCanvas();

            core.ScaleManager.Update();            
        }

        public void UpdateTreeView()
        {
            TreeViewScopes.Items.Clear();
            foreach (SimulationScope sc in core.Dump.Items)
            {
                TreeViewScopes.Items.Add(CreateTreeViewItem(sc));
            }
            foreach (IValueProvider var in core.Dump.Variables)
            {
                if (var is Signal)
                {
                    TreeViewScopes.Items.Add(CreateTreeViewItem((var as Signal)));
                }
            }
        }

        public void UpdateSignalView()
        {
            //ListViewMain.ItemsSource = core.CurrentDump;
            ListViewMain.Items.Clear();
            foreach (My_Variable variable in core.CurrentDump)
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = new My_VariableBindingData(variable, core, core.ScaleManager, cursorViewer);
                if (variable.HasChildrens == true)
                {
                    item.Items.Add("ZZZ");
                }
                ListViewMain.Items.Add(item);
            }
        }

        public static TreeViewItem CreateTreeViewItem(SimulationScope scope)
        {
            TreeViewItem item = new TreeViewItem();

            item.Tag = scope;
            item.Items.Add("zzz");

            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;
            BitmapImage b_img = new BitmapImage(new Uri("/Waveform;component/icons/Chip.png", UriKind.Relative));
            Image img = new Image() { Source = b_img, Width = 16, Height = 16, };
            TextBlock text = new TextBlock() { Text = scope.Name };
            text.Tag = scope;

            panel.Children.Add(img);
            panel.Children.Add(text);

            item.Header = panel;

            return item;
        }

        public static TreeViewItem CreateTreeViewItem(Signal signal)
        {
            TreeViewItem item = new TreeViewItem();

            BitmapImage b_img = null;
            item.Tag = signal;
            if ((signal.Childrens != null) && (signal.Childrens.Count != 0))
            {
                item.Items.Add("zzz");
                b_img = new BitmapImage(new Uri("/Waveform;component/icons/Bus.png", UriKind.Relative));
            }
            else
            {
                b_img = new BitmapImage(new Uri("/Waveform;component/icons/Signal2.jpeg", UriKind.Relative));
            }

            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;
            
            Image img = new Image() { Source = b_img, Width = 16, Height = 16, };
            TextBlock text = new TextBlock() { Text = signal.Name };
            text.Tag = signal;

            panel.Children.Add(img);
            panel.Children.Add(text);

            item.Header = panel;

            return item;
        }

        public void ZoomIn()
        {
            core.ScaleManager.ZoomIn();
        }

        public void ZoomOut()
        {
            core.ScaleManager.ZoomOut();
        }

        public void ZoomToFit()
        {
            core.ScaleManager.ZoomToFit();
        }

        public void ZoomToSelection()
        {
            if (cursorViewer.Selection != null)
            {
                core.ScaleManager.ZoomToSelection(cursorViewer.Selection);
            }
        }

        /// <summary>
        /// Можно ли увеличить масштаб
        /// </summary>
        public bool CanZoomIn
        {
            get
            {
                return (core != null) && (core.ScaleManager != null) && (core.ScaleManager.CanZoomIn);
            }
        }

        /// <summary>
        /// Mожно ли уменьшить масштаб
        /// </summary>
        public bool CanZoomOut
        {
            get
            {
                return (core != null) && (core.ScaleManager != null) && (core.ScaleManager.CanZoomOut);
            }
        }

        /// <summary>
        /// Можно ли промаштабировать по выделенному фрагменту
        /// </summary>
        public bool CanZoomToSelection
        {
            get
            {
                return (cursorViewer != null) && (cursorViewer.Selection != null);
            }
        }

        /// <summary>
        /// Можно ли промасштабировать по ширине
        /// </summary>
        public bool CanZoomToFit
        {
            get
            {
                return (core != null) && (core.ScaleManager != null) && (core.ScaleManager.CanZoomToFit);
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion

        private void SetCurrentItemInEditMode(bool EditMode)
        {
            // Make sure that the SelectedItem is actually a TreeViewItem
            // and not null or something else
            if (ListViewMain.SelectedItem is TreeViewItem)
            {
                TreeViewItem tvi = ListViewMain.SelectedItem as TreeViewItem;

                My_VariableBindingData data = tvi.Header as My_VariableBindingData;
                if ((data != null) && (data.Variable.IsEditableName == true))
                {
                    // Also make sure that the TreeViewItem
                    // uses an EditableTextBlock as its header
                    if (tvi.Header is EditableTextBlock)
                    {
                        EditableTextBlock etb = tvi.Header as EditableTextBlock;

                        // Finally make sure that we are
                        // allowed to edit the TextBlock
                        if (etb.IsEditable)
                            etb.IsInEditMode = EditMode;
                    }
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TimeMeasureDataView1.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void ButtonTableView_Click(object sender, RoutedEventArgs e)
        {
            ShowTableView();
        }

        public IList<ToolBar> GetListOfToolBars()
        {
            return ToolBarTrayEdit.ToolBars;
        }

        public ToolBarTray GetToolBarTray()
        {
            if (ToolBarTrayEdit.Parent != null)
                layoutRoot.Children.Remove(ToolBarTrayEdit);
            return ToolBarTrayEdit;
        }
    }
}
