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
using System.Data;
using System.Threading;
using System.ComponentModel;

using Schematix.Waveform.Value_Dump;
using Schematix.Waveform.Iterators;
using Schematix.Waveform.UserControls;
using DataContainer;
using DataContainer.MySortedDictionary;
using DataContainer.Objects;

namespace Schematix.Waveform
{
    /// <summary>
    /// Interaction logic for TableViewUserControl.xaml
    /// </summary>
    public partial class TableView : Window
    {
        private WaveformCore core;
        public TableView(WaveformCore core)
        {
            this.core = core;
            InitializeComponent();
        }

        /// <summary>
        /// Поток, выполняющий загрузку данных
        /// </summary>
        private Thread update_thread;

        /// <summary>
        /// Список сигналов с которыми в текущий момент идет работа
        /// </summary>
        private List<My_Variable> currentDump;
        public List<My_Variable> CurrentDump
        {
            get
            {
                return currentDump;
            }
            set
            {
                currentDump = value;
            }
        }

        public int PageIndex
        {
            get
            {
                return pageIterator.PageIndex;
            }
            set
            {
                pageIterator.PageIndex = value;
            }
        }

        public int TotalPageCount
        {
            get
            {
                if (pageIterator == null)
                    return 0;
                else
                    return pageIterator.TotalPageCount;
            }
        }

        #region TotalPageCount
        public string TotalPageCountStr
        {
            get
            {
                return string.Format("Total page count = {0}", TotalPageCount);
            }
        }
        #endregion

        public bool HaveSelectedSignals
        {
            get
            {
                return (ListViewCurrentSelectedSignals.SelectedIndex != -1);
            }
        }

        private PageIterator pageIterator;

        /// <summary>
        /// Обновление данных на ListView
        /// </summary>
        public void UpdateListViewData()
        {
            ListViewGrid.Columns.Clear();
            DataTable table = new DataTable();
            table.TableName = "Data";
            //Формирование заголовка таблицы
            DataColumn Num_col = new DataColumn();
            Num_col.DataType = System.Type.GetType("System.String");
            Num_col.Caption = "Number";
            Num_col.ColumnName = "Number";
            table.Columns.Add(Num_col);

            DataColumn Time_col = new DataColumn();
            Time_col.DataType = System.Type.GetType("System.String");
            Time_col.Caption = "Time";
            Time_col.ColumnName = "Time";
            table.Columns.Add(Time_col);

            DataColumn Delta_col = new DataColumn();
            Delta_col.DataType = System.Type.GetType("System.String");
            Delta_col.Caption = "Delta";
            Delta_col.ColumnName = "Delta";
            table.Columns.Add(Delta_col);

            GridViewColumn Num = new GridViewColumn();
            Num.Header = "Number";
            Num.DisplayMemberBinding = new Binding("Number");
            ListViewGrid.Columns.Add(Num);


            GridViewColumn Time = new GridViewColumn();
            Time.Header = "Time";
            Time.DisplayMemberBinding = new Binding("Time");
            ListViewGrid.Columns.Add(Time);

            GridViewColumn Delta = new GridViewColumn();
            Delta.Header = "Delta";
            Delta.DisplayMemberBinding = new Binding("Delta");
            ListViewGrid.Columns.Add(Delta);

            foreach (My_Variable var in CurrentDump)
            {
                GridViewColumn Name = new GridViewColumn();
                Name.Header = var.FullName;
                Name.DisplayMemberBinding = new Binding(var.FullName.Replace('.', '_'));
                ListViewGrid.Columns.Add(Name);
            }

            foreach (My_Variable var in CurrentDump)
            {
                DataColumn Name_col = new DataColumn();
                Name_col.DataType = System.Type.GetType("System.String");
                Name_col.ColumnName = var.FullName.Replace('.', '_');
                Name_col.Caption = var.FullName;
                table.Columns.Add(Name_col);
            }

            if (TotalPageCount != 0)
            {
                bool isEmpty = true;
                foreach (List<string []> lines in GetPagedEnumerator())
                {
                    foreach (string [] line in lines)
                    {
                        isEmpty = false;
                        DataRow row = table.NewRow();
                        row["Number"] = line[0];
                        row["Time"] = line[1];
                        row["Delta"] = line[2];
                        int i = 0;
                        foreach (My_Variable var in currentDump)
                        {
                            row[var.FullName.Replace('.', '_')] = line[i + 3];
                            i++;
                        }

                        table.Rows.Add(row);
                    }
                }
                if (isEmpty == true)
                {
                    DataEnumerator enumerator = pageIterator.GetEnumerator() as DataEnumerator;
                    List<string []> lines = enumerator.Current;
                    foreach (string[] line in lines)
                    {
                        isEmpty = false;
                        DataRow row = table.NewRow();
                        row["Number"] = line[0];
                        row["Time"] = line[1];
                        row["Delta"] = line[2];
                        int i = 0;
                        foreach (My_Variable var in currentDump)
                        {
                            row[var.FullName.Replace('.', '_')] = line[i + 3];
                            i++;
                        }

                        table.Rows.Add(row);
                    }
                }
            }

            ListViewData.DataContext = ((IListSource)table).GetList();
        }

        public void UpdateListViewCurrentSignals()
        {
            ListViewCurrentSelectedSignals.ItemsSource = null;
            ListViewCurrentSelectedSignals.ItemsSource = currentDump as System.Collections.IEnumerable;
        }

        public void UpdateTreeView()
        {
            TreeViewScopes.Items.Clear();
            foreach (SimulationScope sc in core.Dump.Items)
            {
                TreeViewScopes.Items.Add(WaveformUserControl.CreateTreeViewItem(sc));
            }
            foreach (IValueProvider var in core.Dump.Variables)
            {
                if (var is Signal)
                {
                    TreeViewScopes.Items.Add(WaveformUserControl.CreateTreeViewItem(var as Signal));
                }
            }
        }

        /// <summary>
        /// Функция, разбивающая входгые данные на страницы
        /// </summary>
        private void MapPages()
        {
            if ((update_thread != null))
                update_thread.Abort();

            int CurrentIndex = 0;
            int startIndex = 0;
            UInt64 PreviousTime = 0;

            List<IValueIterator> iterators = new List<IValueIterator>();
            foreach (My_Variable var in currentDump)
                iterators.Add(var.Iterator);

            DataPage page = new DataPage(iterators, PreviousTime, DataPage.PageItemsCount, 0);
            startIndex = DataPage.PageItemsCount;

            List<DataPage> res = new List<DataPage>();

            foreach (IValueIterator i in iterators)
                i.Reset();
            while (true)
            {
                //Проверяем, есть ли ещо данные для выборки
                bool IsEndOfData = true;
                foreach (var iter in iterators)
                    if (iter.IsEndOfIteration == false)
                    {
                        IsEndOfData = false;
                        break;
                    }
                if (IsEndOfData == true)
                    break;

                //выбираем первое событие (точнее минимальное время)
                UInt64 CurrentTime = UInt64.MaxValue;
                foreach (var iter in iterators)
                {
                    if ((iter.IsEndOfIteration == false) && (iter.LastEvent < CurrentTime))
                        CurrentTime = iter.LastEvent;
                }

                //передвигаем курсоры
                foreach (var iter in iterators)
                {
                    if (iter.LastEvent == CurrentTime)
                        iter.MoveNextWithoutRecalcValue();
                }

                CurrentIndex++;
                //если индекс равен DataPage.PageItemsCount добавляем новую страницу
                if (CurrentIndex == DataPage.PageItemsCount)
                {
                    page.Count = CurrentIndex;
                    res.Add(page);
                    page = new DataPage(iterators, PreviousTime, 0, startIndex);
                    startIndex += CurrentIndex;
                    CurrentIndex = 0;
                    break;
                }
                               
                PreviousTime = CurrentTime;
            }

            pageIterator.Pages = res;
            NumericUpDownPage.Maximum = 1;
            NumericUpDownPage.Value = 1;
            TextBlockPageCount.Text = "1";

            update_thread = new Thread(
                    (object o) =>
                    {
                        while (true)
                        {
                            //Проверяем, есть ли ещо данные для выборки
                            bool IsEndOfData = true;
                            foreach (var iter in iterators)
                                if (iter.IsEndOfIteration == false)
                                {
                                    IsEndOfData = false;
                                    break;
                                }
                            if (IsEndOfData == true)
                                break;

                            //выбираем первое событие (точнее минимальное время)
                            UInt64 CurrentTime = UInt64.MaxValue;
                            foreach (var iter in iterators)
                            {
                                if ((iter.IsEndOfIteration == false) && (iter.LastEvent < CurrentTime))
                                    CurrentTime = iter.LastEvent;
                            }

                            //передвигаем курсоры
                            foreach (var iter in iterators)
                            {
                                if (iter.LastEvent == CurrentTime)
                                    iter.MoveNextWithoutRecalcValue();
                            }

                            CurrentIndex++;

                            //если индекс равен DataPage.PageItemsCount добавляем новую страницу
                            if (CurrentIndex == DataPage.PageItemsCount)
                            {
                                page.Count = CurrentIndex;
                                res.Add(page);
                                page = new DataPage(iterators, PreviousTime, 0, startIndex);
                                startIndex += CurrentIndex;
                                CurrentIndex = 0;

                                NumericUpDownPage.Dispatcher.Invoke(new Action(
                                    () =>
                                    {
                                        NumericUpDownPage.Maximum = res.Count;
                                    }
                                ), null);

                                TextBlockPageCount.Dispatcher.Invoke(new Action(
                                    () =>
                                    {
                                        TextBlockPageCount.Text = TotalPageCountStr;
                                    }
                                ), null);
                            }
                            PreviousTime = CurrentTime;
                        }
                        //если остались необработанные данные, то добавляем их на последнюю страницу
                        if ((CurrentIndex != 0) && (page.Count != 0))
                        {
                            page.Count = CurrentIndex;
                            res.Add(page);
                            NumericUpDownPage.Dispatcher.Invoke(new Action(
                                    () =>
                                    {
                                        NumericUpDownPage.Maximum = res.Count + 1;
                                    }
                                ), null);

                            TextBlockPageCount.Dispatcher.Invoke(new Action(
                                () =>
                                {
                                    TextBlockPageCount.Text = TotalPageCountStr;
                                }
                            ), null);
                        }
                        //если остались необработанные данные, то добавляем их на последнюю страницу
                        if (CurrentIndex != 0)
                        {
                            page.Count = CurrentIndex;
                            res.Add(page);
                        }
                        //сбрасываем итераторы
                        foreach (var iter in iterators)
                            iter.Reset();
                    }
            );

            update_thread.Start();
        }

        /// <summary>
        /// Функция, возвращающая итератор для постраничного просмотра данных
        /// </summary>
        /// <returns></returns>
        public IEnumerable<List<string []>> GetPagedEnumerator()
        {
            return pageIterator;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            currentDump = new List<My_Variable>();
            foreach (IValueProvider var in core.Dump.Variables)
                if(var is Signal)
                    currentDump.Add(new My_Variable((var as Signal).Name, (var as Signal).Name, (var as Signal)));

            pageIterator = new PageIterator(this);
            pageIterator.UpdateIterators();
            MapPages();
            NumericUpDownPage.Value = 1;
            TextBlockPageCount.Text = TotalPageCountStr;

            UpdateListViewCurrentSignals();
            UpdateListViewData();
            UpdateTreeView();
        }

        private void NumericUpDownPage_ValueChanged(object sender, RoutedPropertyChangedEventArgs<decimal> e)
        {
            if (pageIterator != null)
            {
                PageIndex = (int)e.NewValue;
                UpdateListViewData();
                NumericUpDownPage.Maximum = TotalPageCount;
                TextBlockPageCount.Text = TotalPageCountStr;
            }
        }

        private void MenuItemAddSignal_Click(object sender, RoutedEventArgs e)
        {
            object item = TreeViewScopes.SelectedItem;
            if (item != null)
            {
                TreeViewItem tr_item = item as TreeViewItem;
                if (tr_item.Tag is Signal)
                {
                    Signal var = tr_item.Tag as Signal;
                    IEnumerable<My_Variable> contain = CurrentDump.Where(c => c.Signal.Equals(var));
                    if (contain.ToList().Count == 0)
                    {
                        CurrentDump.Add(new My_Variable(var));
                        pageIterator.PrepareIterators();
                        MapPages();
                        UpdateListViewCurrentSignals();
                        UpdateListViewData();
                        TextBlockPageCount.Text = TotalPageCountStr;
                        NumericUpDownPage.Value = 1;
                    }
                }
            }
        }

        private void TreeViewScopes_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)e.OriginalSource;
            item.Items.Clear();

            if (item.Tag is SimulationScope)
            {
                SimulationScope scope = item.Tag as SimulationScope;
                foreach (SimulationScope sc in scope.Items)
                {
                    item.Items.Add(WaveformUserControl.CreateTreeViewItem(sc));
                }
                foreach (IValueProvider var in scope.Variables)
                {
                    if (var is Signal)
                    {                        
                        item.Items.Add(WaveformUserControl.CreateTreeViewItem(var as Signal));
                    }
                }
            }

            if (item.Tag is Signal)
            {
                Signal selectedSignal = item.Tag as Signal;
                foreach (Signal s in selectedSignal.Childrens)
                {
                    TreeViewItem item_var = new TreeViewItem();
                    item_var.Header = s.Name;
                    item_var.Tag = s;
                    if (((s as Signal).Childrens != null) && ((s as Signal).Childrens.Count != 0))
                    {
                        item_var.Items.Add("zzz");
                    }
                    item.Items.Add(item_var);
                }
            }
        }

        private void MenuItemDeleteSignal_Click(object sender, RoutedEventArgs e)
        {
            object item = ListViewCurrentSelectedSignals.SelectedItem;
            if (item != null)
            {
                My_Variable var = (My_Variable)item;
                CurrentDump.Remove(var);
                pageIterator.PrepareIterators();
                MapPages();
                UpdateListViewCurrentSignals();
                UpdateListViewData();
                TextBlockPageCount.Text = TotalPageCountStr;
                NumericUpDownPage.Value = 1;
            }
        }

        private void MenuItemSignalProperties_Click(object sender, RoutedEventArgs e)
        {
            object item = ListViewCurrentSelectedSignals.SelectedItem;
            if (item != null)
            {
                My_Variable var = (My_Variable)item;

                SignalProperties prop = new SignalProperties(var);
                prop.ShowDialog();

                pageIterator.UpdateIterators();
                UpdateListViewData();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if ((update_thread != null))
                update_thread.Abort();
        }
    }
}
