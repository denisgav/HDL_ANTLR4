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
using Schematix.Waveform.Value_Dump;
using DataContainer;

namespace Schematix.Waveform.UserControls
{
    /// <summary>
    /// Interaction logic for TimeMeasureDataView.xaml
    /// </summary>
    public partial class TimeMeasureDataView : UserControl
    {
        /// <summary>
        /// Высота одного слоя
        /// </summary>
        const double LayerHeight = 15;


        public TimeMeasureDataView()
        {
            InitializeComponent();
            lines = new List<Line>();
        }

        void ScaleManager_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UpdateCanvas();
        }

        void TimeMeasureList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateCanvas();
        }

        private WaveformCore  core;
        public WaveformCore  Core
        {
            get { return core; }
            set { core = value; }
        }

        private List<Line> lines;

        /// <summary>
        /// Обновление отображения данных
        /// </summary>
        public void UpdateCanvas()
        {
            lines.Clear();
            MainCanvas.Children.Clear();

            if (core == null)
                return;

            if (core.TimeMeasureList == null)
                return;

            //Формируем список данных для отображения
            List<TimeMeasureData> viewList = new List<TimeMeasureData>();
            foreach (TimeMeasureData data in core.TimeMeasureList)
            {
                if ((core.ScaleManager.IsContainsTime(data.TimeStart) == true) && (core.ScaleManager.IsContainsTime(data.TimeEnd) == true))
                    viewList.Add(data);
            }

            //если список не пуст
            if (viewList.Count != 0)
            {
                //сортируем список по значению начального времени
                viewList.Sort(delegate(TimeMeasureData d1, TimeMeasureData d2) { return d1.TimeStart.CompareTo(d2.TimeStart); });
                int layerNumber = 1;
                while (viewList.Count != 0)
                {
                    TimeMeasureData cur_data = viewList[0];
                    viewList.Remove(cur_data);
                    DrawData(cur_data, layerNumber);
                    List<TimeMeasureData> drawList = new List<TimeMeasureData>();
                    foreach (TimeMeasureData d in viewList)
                    {
                        if (d.TimeStart >= cur_data.TimeEnd)
                        {
                            DrawData(d, layerNumber);
                            drawList.Add(d);
                            cur_data = d;
                        }
                    }
                    foreach (TimeMeasureData d in drawList)
                        viewList.Remove(d);
                    drawList.Clear();
                    layerNumber++;
                }
            }

            core.WaveformUserControl.SetLines(lines);
        }

        /// <summary>
        /// Выбранное TimeMeasureData
        /// </summary>
        private TimeMeasureData selectedTimeMeasureData;
        public TimeMeasureData SelectedTimeMeasureData
        {
            get { return selectedTimeMeasureData; }
            set { selectedTimeMeasureData = value; }
        }
        

        /// <summary>
        /// Отобразить данные на MainCanvas
        /// </summary>
        /// <param name="data"></param>
        /// <param name="layerNumber"></param>
        private void DrawData(TimeMeasureData data, int layerNumber)
        {
            double? pos1 = core.WaveformUserControl.GetYPos(data.Variable1);
            double? pos2 = core.WaveformUserControl.GetYPos(data.Variable2);
            if ((pos1 != null) && (pos2 != null))
            {
                Point p1 = new Point(core.ScaleManager.GetOffset(data.TimeStart), 80-layerNumber * LayerHeight);
                Point p2 = new Point(core.ScaleManager.GetOffset(data.TimeEnd), 80-layerNumber * LayerHeight);

                if (Math.Abs(p1.X - p2.X) < 16.0d)
                    return;

                Arrow arrow = new Arrow();
                arrow.X1 = p1.X;
                arrow.Y1 = p1.Y;
                arrow.X2 = p2.X;
                arrow.Y2 = p2.Y;
                arrow.HeadWidth = 10;
                arrow.HeadHeight = 5;
                arrow.StrokeThickness = 1;
                arrow.Stroke = Brushes.Gray;
                arrow.ContextMenu = Resources["ContextMenuArrow"] as ContextMenu;
                arrow.Tag = data;
                arrow.MouseEnter += new MouseEventHandler(element_MouseEnter);
                arrow.MouseLeave += new MouseEventHandler(element_MouseLeave);

                Point center = new Point((p1.X + p2.X) / 2.0, (p1.Y + p2.Y) / 2.0 - 10.0);

                TextBlock textBlock = new TextBlock();
                textBlock.Background = Brushes.White;
                textBlock.Text = TimeInterval.ToString(data.TimeEnd - data.TimeStart);
                textBlock.FontSize = 10;
                textBlock.Width = 50;
                textBlock.Height = 10;
                textBlock.TextAlignment = TextAlignment.Center;
                textBlock.TextWrapping = TextWrapping.Wrap;
                textBlock.HorizontalAlignment = HorizontalAlignment.Stretch;
                textBlock.VerticalAlignment = VerticalAlignment.Stretch;
                textBlock.ContextMenu = Resources["ContextMenuArrow"] as ContextMenu;
                textBlock.Tag = data;
                textBlock.MouseEnter += new MouseEventHandler(element_MouseEnter);
                textBlock.MouseLeave += new MouseEventHandler(element_MouseLeave);

                MainCanvas.Children.Add(textBlock);
                Canvas.SetLeft(textBlock, center.X - textBlock.Width / 2.0);
                Canvas.SetTop(textBlock, center.Y);

                MainCanvas.Children.Add(arrow);

                Point pt1 = MainCanvas.PointToScreen(p1);
                Point pt2 = MainCanvas.PointToScreen(p2);
                Line line1 = new Line();
                line1.X1 = pt1.X;
                line1.X2 = pt1.X;
                line1.Y1 = pt1.Y;
                line1.Y2 = pos1.Value;

                Line line2 = new Line();
                line2.X1 = pt2.X;
                line2.X2 = pt2.X;
                line2.Y1 = pt2.Y;
                line2.Y2 = pos2.Value;

                lines.Add(line1);
                lines.Add(line2);
            }
        }

        void element_MouseLeave(object sender, MouseEventArgs e)
        {
            //selectedTimeMeasureData = null;
        }

        void element_MouseEnter(object sender, MouseEventArgs e)
        {
            TimeMeasureData data = null;
            if (sender is TextBlock)
                data = (sender as TextBlock).Tag as TimeMeasureData;
            if (sender is Arrow)
                data = (sender as Arrow).Tag as TimeMeasureData;
            selectedTimeMeasureData = data;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (core != null)
            {
                core.TimeMeasureList.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(TimeMeasureList_CollectionChanged);
                core.ScaleManager.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ScaleManager_PropertyChanged);
                core.CurrentDump.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(CurrentDump_CollectionChanged);
            }
        }

        void CurrentDump_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateCanvas();
        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTimeMeasureData != null)
                core.TimeMeasureList.Remove(selectedTimeMeasureData);
        }
    }
}
