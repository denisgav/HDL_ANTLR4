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
using DataContainer;

namespace Schematix.Waveform.UserControls
{
    /// <summary>
    /// Interaction logic for TimeScale.xaml
    /// </summary>
    public partial class TimeScaleViewer : UserControl
    {
        private ScaleManager scaleManager;
        public ScaleManager ScaleManager
        {
            get
            {
                return scaleManager;
            }
            set
            {
                scaleManager = value;
            }
        }

        /// <summary>
        /// Обновление информации, отображаемой элементом
        /// </summary>
        public void UpdateView()
        {
            ScrollBarTimeScale.Minimum = scaleManager.StartTime;
            ScrollBarTimeScale.Maximum = scaleManager.EndTime - scaleManager.VisibleTimeDiapasone;
            ScrollBarTimeScale.Value = scaleManager.VisibleStartTime;
            ScrollBarTimeScale.SmallChange = scaleManager.VisibleTimeDiapasone / 10;
            ScrollBarTimeScale.LargeChange = scaleManager.VisibleTimeDiapasone / 2;

            UpdateCanvas();            
        }

        public TimeScaleViewer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// обновление линейки
        /// </summary>
        private void UpdateCanvas()
        {
            CanvasTimeScale.Children.Clear();
            double Delta = CanvasTimeScale.ActualWidth / (double)scaleManager.VisibleTimeDiapasone;
            UInt64 CeiledVisibleTimeDiapasone = MyRound(scaleManager.VisibleTimeDiapasone);
            UInt64 StartOffset = (scaleManager.VisibleStartTime / CeiledVisibleTimeDiapasone/10) * CeiledVisibleTimeDiapasone*10;
            AddRecord(0, 5, 2, "");
            for (UInt64 i = StartOffset; i <= scaleManager.VisibleEndTime; i += CeiledVisibleTimeDiapasone / 10)
            {
                UInt64 offset = i - scaleManager.VisibleStartTime;
                AddRecord(Delta * (double)offset, 10, 2, TimeInterval.ToString(i));
                if (scaleManager.VisibleTimeDiapasone > 100)
                    for (UInt64 j = i; j < i + CeiledVisibleTimeDiapasone / 10; j += CeiledVisibleTimeDiapasone / 100)
                    {
                        offset = j - scaleManager.VisibleStartTime;
                        AddRecord(Delta * (double)(offset), 5, 2, "");
                    }
            }
        }

        /// <summary>
        /// Округление
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private UInt64 MyRound(UInt64 x)
        {
            /*int digits = -(int)Math.Log10(x);
            if (x < 1) digits++;
            double pow = Math.Pow(10, digits);
            return (UInt64)Math.Round((Math.Round(x * pow) / pow));
             */
            return (UInt64)Math.Pow(10, Math.Ceiling(Math.Log10(x)));
        }
        

        /// <summary>
        /// Добавление новой метки
        /// </summary>
        /// <param name="posX"></param>
        /// <param name="text"></param>
        private void AddRecord(double posX, double height, double thickness, string text) 
        {
            Line line = new Line();
            line.Stroke = Brushes.LightSteelBlue;
            line.X1 = posX + thickness / 2.0;
            line.X2 = posX + thickness / 2.0;
            line.Y1 = CanvasTimeScale.ActualHeight - height;
            line.Y2 = CanvasTimeScale.ActualHeight;
            line.StrokeThickness = thickness;

            CanvasTimeScale.Children.Add(line);

            if (string.IsNullOrEmpty(text) == false)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = text;
                textBlock.TextAlignment = TextAlignment.Center;
                textBlock.TextWrapping = TextWrapping.Wrap;
                textBlock.MaxWidth = 50;
                
                CanvasTimeScale.Children.Add(textBlock);
                Canvas.SetLeft(textBlock, posX - textBlock.ActualWidth / 2.0);
                Canvas.SetTop(textBlock, 10);
            }
        }

        private void ScrollBarTimeScale_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            scaleManager.VisibleStartTime = (UInt64)e.NewValue;
            UpdateCanvas();
        }

        void scaleManager_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ScrollBarTimeScale.Minimum = scaleManager.StartTime;
            ScrollBarTimeScale.Maximum = (scaleManager.VisibleTimeDiapasone >= scaleManager.EndTime)?0:scaleManager.EndTime - scaleManager.VisibleTimeDiapasone;
            ScrollBarTimeScale.Value = scaleManager.VisibleStartTime;
            ScrollBarTimeScale.SmallChange = scaleManager.VisibleTimeDiapasone / 10;
            ScrollBarTimeScale.LargeChange = scaleManager.VisibleTimeDiapasone / 2;
            UpdateCanvas();
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (scaleManager != null)
            {
                scaleManager.Width = e.NewSize.Width;
                UpdateCanvas();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (scaleManager != null)
            {
                scaleManager.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(scaleManager_PropertyChanged);
            }
        }
    }
}
