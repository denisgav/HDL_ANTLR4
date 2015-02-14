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
using DataContainer.Generator;
using DataContainer;
using DataContainer.Objects;

namespace Schematix.Waveform
{
    /// <summary>
    /// Текущий тип операции для Waveform
    /// </summary>
    public enum WaveformActionMode
    {
        Hand,
        Cursor,
        Measure
    }

    public partial class WaveformUserControl
    {
        private void TreeViewScopes_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)e.OriginalSource;
            item.Items.Clear();

            if (item.Tag is SimulationScope)
            {
                SimulationScope scope = item.Tag as SimulationScope;
                foreach (SimulationScope sc in scope.Items)
                {
                    item.Items.Add(CreateTreeViewItem(sc));
                }
                foreach (IValueProvider var in scope.Variables)
                {
                    if (var is Signal)
                    {                        
                        item.Items.Add(CreateTreeViewItem(var as Signal));
                    }
                }
            }

            if (item.Tag is Signal)
            {
                Signal selectedSignal = item.Tag as Signal;
                foreach (Signal s in selectedSignal.Childrens)
                {
                    item.Items.Add(CreateTreeViewItem(s));
                }
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
                    IEnumerable<My_Variable> contain = core.CurrentDump.Where(c => c.Signal.Equals(var));
                    if (contain.ToList().Count == 0)
                    {
                        core.CurrentDump.Add(new My_Variable(var));
                        UpdateSignalView();
                    }
                }
            }
        }

        private void ListViewMain_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)e.OriginalSource;
            item.Items.Clear();

            My_VariableBindingData data = item.Header as My_VariableBindingData;
            List<My_Variable> variableCollection = null;
            if (data != null)
                variableCollection = data.Variable.Childrens;

            if (variableCollection != null)
            {
                foreach (My_Variable v in variableCollection)
                {
                    TreeViewItem i = new TreeViewItem();
                    i.Header = new My_VariableBindingData(v, core, core.ScaleManager, cursorViewer);
                    if (v.HasChildrens == true)
                    {
                        i.Items.Add("ZZZ");
                    }
                    item.Items.Add(i);
                }
            }
        }        

        private void Grid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            dragDropManager.ListView_PreviewMouseLeftButtonDown(sender, e);
        }

        private void MenuIteddeleteSignal_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewMain.SelectedItem != null)
            {
                My_Variable mv = ((ListViewMain.SelectedItem as TreeViewItem).Header as My_VariableBindingData).Variable;
                ListViewMain.Items.Remove(ListViewMain.SelectedItem);
                core.CurrentDump.Remove(mv);
            }
        }

        private void MenuItemProperties_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewMain.SelectedItem != null)
            {
                My_Variable mv = ((ListViewMain.SelectedItem as TreeViewItem).Header as My_VariableBindingData).Variable;
                SignalProperties prop = new SignalProperties(mv);
                if (prop.ShowDialog() == true)
                {
                    UpdateSignalView();
                }
            }
        }

        private void GridViewColumnHeaderTimescale_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //Для расщета берется вся ширина GridMain и от нее вычетается ширина 2-х столбцов
            //Магическое число 9 - ширина для разделителя столбцов
            //timeScaleViewer.Width = e.NewSize.Width - 9;
            double width = GridMain.ActualWidth - GridViewColumnName.ActualWidth - GridViewColumnValue.ActualWidth - 9;
            if (width > 0)
            {
                timeScaleViewer.Width = width;
                TimeScaleRepresentation.Width = width;
                TimeScaleRepresentationBrush.Visual = timeScaleViewer.CanvasTimeScale;
            }
        }

        private void ButtonResizeDiagramm_Click(object sender, RoutedEventArgs e)
        {
            ResizeDiagram resize = new ResizeDiagram(core.ScaleManager);
            if (resize.ShowDialog() == true)
                core.ScaleManager.ZoomToFit();
        }


        private void Image_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Image img = (Image)sender;
            if (img.IsEnabled == false)
            {
                Brush br = new SolidColorBrush(Color.FromArgb(50, 50, 50, 50));
                img.OpacityMask = br;
            }
            else
            {
                Brush br = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                img.OpacityMask = br;
            }
        }

        private void ListViewMain_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //ButtonCreateArtificialBus.IsEnabled = (ListViewMain.SelectedItems.Count >= 1);
            if (ListViewMain.SelectedItem != null)
            {
                My_VariableBindingData data = (ListViewMain.SelectedItem as TreeViewItem).Header as My_VariableBindingData;
                //if (data != null)
                //{
                //    ButtonRemoveArtificialBus.IsEnabled = data.Variable.DataType is ArtificialBus;
                //}
            }
        }

        private void EditableTextBlock_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditableTextBlock etb = sender as EditableTextBlock;

            if (etb != null)
            {
                // Make sure that the SelectedItem is actually a TreeViewItem
                // and not null or something else
                if (ListViewMain.SelectedItem is TreeViewItem)
                {
                    TreeViewItem tvi = ListViewMain.SelectedItem as TreeViewItem;
                    My_VariableBindingData data = tvi.Header as My_VariableBindingData;
                    if ((data != null) && (data.Variable.IsEditableName == true))
                    {
                        // Finally make sure that we are
                        // allowed to edit the TextBlock
                        if (etb.IsEditable)
                            etb.IsInEditMode = true;
                    }
                }
            }
        }

        #region valueRepresenter Mouse Events
        private void valueRepresenter_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            switch (actionMode)
            {
                case WaveformActionMode.Cursor:
                    valueRepresenter_MouseLeftButtonDown_Cursor(sender, e);
                    break;
                case WaveformActionMode.Hand:
                    valueRepresenter_MouseLeftButtonDown_Hand(sender, e);
                    break;
                case WaveformActionMode.Measure:
                    valueRepresenter_MouseLeftButtonDown_Measure(sender, e);
                    break;
                default:
                    break;
            }
        }

        private void valueRepresenter_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            switch (actionMode)
            {
                case WaveformActionMode.Cursor:
                    valueRepresenter_MouseLeftButtonUp_Cursor(sender, e);
                    break;
                case WaveformActionMode.Hand:
                    valueRepresenter_MouseLeftButtonUp_Hand(sender, e);
                    break;
                case WaveformActionMode.Measure:
                    valueRepresenter_MouseLeftButtonUp_Measure(sender, e);
                    break;
                default:
                    break;
            }
        }

        private void valueRepresenter_MouseMove(object sender, MouseEventArgs e)
        {
            switch (actionMode)
            {
                case WaveformActionMode.Cursor:
                    valueRepresenter_MouseMove_Cursor(sender, e);
                    break;
                case WaveformActionMode.Hand:
                    valueRepresenter_MouseMove_Hand(sender, e);
                    break;
                case WaveformActionMode.Measure:
                    valueRepresenter_MouseMove_Measure(sender, e);
                    break;
                default:
                    break;
            }
        }

        private void valueRepresenter_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            switch (actionMode)
            {
                case WaveformActionMode.Cursor:
                    valueRepresenter_MouseDoubleClick_Cursor(sender, e);
                    break;
                case WaveformActionMode.Hand:
                    valueRepresenter_MouseDoubleClick_Hand(sender, e);
                    break;
                case WaveformActionMode.Measure:
                    valueRepresenter_MouseDoubleClick_Measure(sender, e);
                    break;
                default:
                    break;
            }
        }

        #region Cursor
        private void valueRepresenter_MouseLeftButtonDown_Cursor(object sender, MouseButtonEventArgs e)
        {
            ValueRepresenter repr = sender as ValueRepresenter;
            if (core.ScaleManager.EndTime == 0)
                return;
            Point pt = e.GetPosition(repr);
            UInt64 time = core.ScaleManager.GetTime(pt.X);
            Selection sel = new Selection(time, time, repr.Variable);
            cursorViewer.Selection = sel;
        }

        private void valueRepresenter_MouseLeftButtonUp_Cursor(object sender, MouseButtonEventArgs e)
        {
            if (core.ScaleManager.EndTime == 0)
                return;
            ValueRepresenter repr = sender as ValueRepresenter;
            if ((cursorViewer.Selection != null) && (cursorViewer.Selection.End != core.ScaleManager.EndTime) && (cursorViewer.Selection.Start != core.ScaleManager.StartTime))
            {
                Point pt = e.GetPosition(repr);
                UInt64 time = core.ScaleManager.GetTime(pt.X);

                cursorViewer.Selection.X2 = time;

                //если выделенная область слишком мала, то она обнуляется
                if (cursorViewer.Selection.TimeInterval <= core.ScaleManager.MinimumVisibleChange)
                    cursorViewer.Selection = null;
            }
        }

        private void valueRepresenter_MouseMove_Cursor(object sender, MouseEventArgs e)
        {
            if (core.ScaleManager.EndTime == 0)
                return;
            ValueRepresenter repr = sender as ValueRepresenter;
            if ((e.LeftButton == MouseButtonState.Pressed) && (cursorViewer.Selection != null) && (cursorViewer.Selection.End != core.ScaleManager.EndTime) && (cursorViewer.Selection.Start != core.ScaleManager.StartTime))
            {
                Point pt = e.GetPosition(repr);
                UInt64 time = core.ScaleManager.GetTime(pt.X);

                cursorViewer.Selection.X2 = time;
            }
        }

        private void valueRepresenter_MouseDoubleClick_Cursor(object sender, MouseButtonEventArgs e)
        {
            ValueRepresenter repr = sender as ValueRepresenter;
            if (core.ScaleManager.EndTime != 0)
            {
                cursorViewer.Selection = new Selection(core.ScaleManager.StartTime, core.ScaleManager.EndTime, repr.Variable);
                e.Handled = true;
            }
        }
        #endregion


        #region Hand
        /// <summary>
        /// Используется для режима Hand и запоминает
        /// смещение курсора при нажатии левой кнопки мишы
        /// </summary>
        private double PositionX = 0;
        /// <summary>
        /// Используется для режима Hand и запоминает
        /// время при нажатии левой кнопки мишы
        /// </summary>
        private UInt64 StartTime = 0;

        private void valueRepresenter_MouseLeftButtonDown_Hand(object sender, MouseButtonEventArgs e)
        {
            ValueRepresenter repr = sender as ValueRepresenter;
            Point pt = e.GetPosition(repr);
            PositionX = pt.X;
            StartTime = core.ScaleManager.VisibleStartTime;
        }

        private void valueRepresenter_MouseMove_Hand(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                ValueRepresenter repr = sender as ValueRepresenter;
                Point pt = e.GetPosition(repr);
                double PositionX_new = pt.X;
                UInt64 deltaTime = (UInt64)Math.Abs(core.ScaleManager.Delta(PositionX_new, PositionX));
                if (PositionX_new > PositionX)
                {
                    if (deltaTime < StartTime)
                    {
                        UInt64 newPos = StartTime - deltaTime;
                        core.ScaleManager.VisibleStartTime = newPos;
                    }
                    else
                        core.ScaleManager.VisibleStartTime = 0;
                }
                else
                {
                    UInt64 newPos = Math.Min(StartTime + deltaTime, core.ScaleManager.EndTime - core.ScaleManager.VisibleTimeDiapasone);
                    core.ScaleManager.VisibleStartTime = newPos;
                }
            }
        }

        private void valueRepresenter_MouseLeftButtonUp_Hand(object sender, MouseButtonEventArgs e)
        { }

        private void valueRepresenter_MouseDoubleClick_Hand(object sender, MouseButtonEventArgs e)
        { }
        #endregion

        #region Measure

        /// <summary>
        /// Первый маркер для измерения времени
        /// </summary>
        private TimeMarker marker1;

        /// <summary>
        /// Второй маркер для измерения времени
        /// </summary>
        private TimeMarker marker2;

        /// <summary>
        /// Элемент управления для первого маркера
        /// </summary>
        private ValueRepresenter repr1;

        /// <summary>
        /// Элемент управления для второго маркера
        /// </summary>
        private ValueRepresenter repr2;

        private void valueRepresenter_MouseLeftButtonDown_Measure(object sender, MouseButtonEventArgs e)
        {
            LineCursor.Visibility = System.Windows.Visibility.Collapsed;
            LineMarker1.Visibility = System.Windows.Visibility.Visible;
            LineMarker2.Visibility = System.Windows.Visibility.Visible;
            ValueRepresenter repr = sender as ValueRepresenter;
            repr1 = repr;
            Point pt = e.GetPosition(repr);
            Point ptr = e.GetPosition(ListViewMain);
            double PositionX = pt.X;
            marker1 = repr.GetNearestMarker(PositionX);
            if (marker1 == null)
            {
                marker1 = new TimeMarker(core.ScaleManager.GetTime(PositionX), PositionX);
            }
            LineMarker1.X1 = LineMarker1.X2 = ptr.X - PositionX + marker1.Offset;
            LineMarker1.Y1 = LineCursor.Y1;
            LineMarker1.Y2 = LineCursor.Y2;
        }        
        private void valueRepresenter_MouseLeftButtonUp_Measure(object sender, MouseButtonEventArgs e)
        {
            LineCursor.Visibility = System.Windows.Visibility.Visible;
            LineMarker1.Visibility = System.Windows.Visibility.Collapsed;
            LineMarker2.Visibility = System.Windows.Visibility.Collapsed;
            ValueRepresenter repr = sender as ValueRepresenter;
            repr2 = repr;
            Point pt = e.GetPosition(repr);
            Point ptr = e.GetPosition(ListViewMain);
            double PositionX = pt.X;
            marker2 = repr.GetNearestMarker(PositionX);
            if (marker2 == null)
            {
                marker2 = new TimeMarker(core.ScaleManager.GetTime(PositionX), PositionX);
            }
            TimeMeasureDataView1.Visibility = Visibility.Visible;

            if(marker1.Time != marker2.Time)
                core.TimeMeasureList.Add(new TimeMeasureData(marker1, marker2, repr1.Variable, repr2.Variable));
        }
        private void valueRepresenter_MouseMove_Measure(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                ValueRepresenter repr = sender as ValueRepresenter;
                Point pt = e.GetPosition(repr);
                Point ptr = e.GetPosition(ListViewMain);
                double PositionX = pt.X;
                marker2 = repr.GetNearestMarker(PositionX);
                if (marker2 == null)
                {
                    marker2 = new TimeMarker(core.ScaleManager.GetTime(PositionX), PositionX);
                }
                LineMarker2.X1 = LineMarker2.X2 = ptr.X - PositionX + marker2.Offset;
                LineMarker2.Y1 = LineCursor.Y1;
                LineMarker2.Y2 = LineCursor.Y2;
            }
        }
        private void valueRepresenter_MouseDoubleClick_Measure(object sender, MouseButtonEventArgs e)
        { }
        #endregion
        #endregion


        private void ToggleButtonCursor_Checked(object sender, RoutedEventArgs e)
        {
            if(ToggleButtonCursor.IsChecked == true)
                ActionMode = WaveformActionMode.Cursor;
        }

        private void ToggleButtonHand_Checked(object sender, RoutedEventArgs e)
        {
            if (ToggleButtonHand.IsChecked == true)
                ActionMode = WaveformActionMode.Hand;
        }

        private void ToggleButtonMeasureTime_Checked(object sender, RoutedEventArgs e)
        {
            if (ToggleButtonMeasureTime.IsChecked == true)
                ActionMode = WaveformActionMode.Measure;
        }

        private void ListViewMain_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            TimeMeasureDataView1.UpdateCanvas();
        }

        private void ListViewMain_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Delete:
                    onDelete();
                    break;
                default:
                    break;
            }            
        }

        private void ListViewMain_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void onDelete()
        {
            if (ListViewMain.SelectedItems.Count != 0)
            {
                foreach (TreeViewItem i in ListViewMain.SelectedItems)
                {
                    My_VariableBindingData data = i.Header as My_VariableBindingData;
                    if (data != null)
                    {
                        core.CurrentDump.Remove(data.Variable);
                    }
                }
                UpdateSignalView();
            }
        }

        #region Command handlers

        private void ZoomIn_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CanZoomIn;
        }

        private void ZoomIn_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ZoomIn();
        }

        private void ZoomOut_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CanZoomOut;
        }

        private void ZoomOut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ZoomOut();
        }

        private void ZoomToSelection_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CanZoomToSelection;
        }

        private void ZoomToSelection_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ZoomToSelection();
        }

        private void ZoomToFit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CanZoomToFit;
        }

        private void ZoomToFit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ZoomToFit();
        }

        private void SetOneGenerator_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (cursorViewer != null) && (cursorViewer.Selection != null) && (cursorViewer.Selection.Variable.Signal.Type.Dimension[0].Length == 1);
        }

        private void SetOneGenerator_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                DataContainer.Generator.Constant.One.Fill(cursorViewer.Selection.Variable.Signal, cursorViewer.Selection.Start, cursorViewer.Selection.End);
                cursorViewer.Selection = null;
                UpdateSignalView();
                core.IsModified = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetZeroGenerator_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (cursorViewer != null) && (cursorViewer.Selection != null) && (cursorViewer.Selection.Variable.Signal.Type.Dimension[0].Length == 1);
        }

        private void SetZeroGenerator_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                DataContainer.Generator.Constant.Zero.Fill(cursorViewer.Selection.Variable.Signal, cursorViewer.Selection.Start, cursorViewer.Selection.End);
                cursorViewer.Selection = null;
                UpdateSignalView();
                core.IsModified = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetConstantGenerator_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (cursorViewer != null) && (cursorViewer.Selection != null);
        }

        private void SetConstantGenerator_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                GeneratorDialog diag = new GeneratorDialog(cursorViewer.Selection.Variable, GeneratorType.Constant);
                if (diag.ShowDialog() == true)
                {
                    BaseGenerator generator = diag.Generator;
                    generator.Fill(cursorViewer.Selection.Variable.Signal, cursorViewer.Selection.Start, cursorViewer.Selection.End);
                    cursorViewer.Selection = null;
                    UpdateSignalView();
                    core.IsModified = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetClockGenerator_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (cursorViewer != null) && (cursorViewer.Selection != null) && (cursorViewer.Selection.Variable.Signal.Type.Dimension[0].Length == 1);
        }

        private void SetClockGenerator_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                GeneratorDialog diag = new GeneratorDialog(cursorViewer.Selection.Variable, GeneratorType.Clock);
                if (diag.ShowDialog() == true)
                {
                    BaseGenerator generator = diag.Generator;
                    generator.Fill(cursorViewer.Selection.Variable.Signal, cursorViewer.Selection.Start, cursorViewer.Selection.End);
                    cursorViewer.Selection = null;
                    UpdateSignalView();
                    core.IsModified = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetRandomGenerator_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (cursorViewer != null) && (cursorViewer.Selection != null) && (cursorViewer.Selection.Variable.Signal.Type.Dimension[0].Length != 1);
        }

        private void SetRandomGenerator_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                GeneratorDialog diag = new GeneratorDialog(cursorViewer.Selection.Variable, GeneratorType.DiscretteRandom);
                if (diag.ShowDialog() == true)
                {
                    BaseGenerator generator = diag.Generator;
                    generator.Fill(cursorViewer.Selection.Variable.Signal, cursorViewer.Selection.Start, cursorViewer.Selection.End);
                    cursorViewer.Selection = null;
                    UpdateSignalView();
                    core.IsModified = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetCounterGenerator_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (cursorViewer != null) && (cursorViewer.Selection != null) && (cursorViewer.Selection.Variable.Signal.Type.Dimension[0].Length != 1);
        }

        private void SetCounterGenerator_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                GeneratorDialog diag = new GeneratorDialog(cursorViewer.Selection.Variable, GeneratorType.Counter);
                if (diag.ShowDialog() == true)
                {
                    BaseGenerator generator = diag.Generator;
                    generator.Fill(cursorViewer.Selection.Variable.Signal, cursorViewer.Selection.Start, cursorViewer.Selection.End);
                    cursorViewer.Selection = null;
                    UpdateSignalView();
                    core.IsModified = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetGenerator_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (cursorViewer != null) && (cursorViewer.Selection != null);
        }

        private void SetGenerator_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (cursorViewer.Selection != null)
            {
                GeneratorDialog diag = new GeneratorDialog(cursorViewer.Selection.Variable);
                if (diag.ShowDialog() == true)
                {
                    try
                    {
                        BaseGenerator generator = diag.Generator;
                        generator.Fill(cursorViewer.Selection.Variable.Signal, cursorViewer.Selection.Start, cursorViewer.Selection.End);
                        cursorViewer.Selection = null;
                        UpdateSignalView();
                        core.IsModified = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void CorrectSelectionInterval_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (cursorViewer != null) && (cursorViewer.Selection != null);
        }

        private void CorrectSelectionInterval_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (cursorViewer.Selection != null)
            {
                CorrectSelectedTime corr = new CorrectSelectedTime(cursorViewer.Selection, core.ScaleManager);
                if (corr.ShowDialog() == true)
                {
                    cursorViewer.Update();
                }
            }
        }

        #region Run command
        public delegate void RunDelegate(Schematix.Waveform.WaveformCore core);
        private event RunDelegate runEvent;
        public event RunDelegate Run
        {
            add { runEvent += value; }
            remove { runEvent -= value; }
        }
        private void CustomRunCommandHandler(Schematix.Waveform.WaveformCore core)
        { }

        private void StartSimulation_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (core == null)
            {
                e.CanExecute = false;
                return;
            }

            bool suggestion1 = ((core.Entity != null) && (string.IsNullOrEmpty(core.EntityName) == false) && (string.IsNullOrEmpty(core.ArchitectureName) == false) && (string.IsNullOrEmpty(core.FileName) == false));
            if (suggestion1 == false)
            {
                e.CanExecute = false;
                return;
            }

            bool suggestion2 = ((core.Entity.Port_items != null) && (core.Entity.Port_items.Count != 0));
            if (suggestion2 == false)
            {
                e.CanExecute = false;
                return;
            }
            e.CanExecute = true;
        }

        private void StartSimulation_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            runEvent(core);
        }
        #endregion
        #endregion
    }
}
