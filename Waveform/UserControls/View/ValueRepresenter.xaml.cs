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
using System.ComponentModel;
using Schematix.Waveform.UserControls;
using Schematix.Waveform.SignalViews;
using DataContainer;

namespace Schematix.Waveform.UserControls
{
    /// <summary>
    /// Interaction logic for ValueRepresenter.xaml
    /// </summary>
    public partial class ValueRepresenter : UserControl
    {
        /// <summary>
        /// Минимальное расстояние к маркеру для того чтобы произошло "Подмагничивание"
        /// </summary>
        const double MinimumMarkerDistance = 10.0;

        /// <summary>
        /// Отображаемые данные
        /// </summary>
        public My_Variable Variable
        {
            get { return (My_Variable)GetValue(VariableProperty); }
            set { SetValue(VariableProperty, value); }
        }

        /// <summary>
        /// Выбранная в текущий момент заметка
        /// </summary>
        private BookMark selectedBookMark;
        public BookMark SelectedBookMark
        {
            get { return selectedBookMark; }
            set { selectedBookMark = value; }
        }

        public static readonly DependencyProperty VariableProperty =
            DependencyProperty.Register("Variable", typeof(My_Variable), typeof(ValueRepresenter));

        /// <summary>
        /// Ядро системы
        /// </summary>
        public WaveformCore Core
        {
            get { return (WaveformCore)GetValue(CoreProperty); }
            set { SetValue(CoreProperty, value); }
        }

        public static readonly DependencyProperty CoreProperty =
            DependencyProperty.Register("Core", typeof(WaveformCore), typeof(ValueRepresenter));


        /// <summary>
        /// Класс, отвечающий за масштабирование
        /// </summary>
        public ScaleManager ScaleManager
        {
            get { return (ScaleManager)GetValue(ScaleManagerProperty); }
            set { SetValue(ScaleManagerProperty, value); }
        }

        public static readonly DependencyProperty ScaleManagerProperty =
            DependencyProperty.Register("ScaleManager", typeof(ScaleManager), typeof(ValueRepresenter));

        /// <summary>
        /// Класс, отвечающий за работу с курсором
        /// </summary>
        public CursorViewer CursorViewer
        {
            get { return (CursorViewer)GetValue(CursorViewerProperty); }
            set { SetValue(CursorViewerProperty, value); }
        }

        public static readonly DependencyProperty CursorViewerProperty =
            DependencyProperty.Register("CursorViewer", typeof(CursorViewer), typeof(ValueRepresenter));


        //события
        private UpdateView updateView;
        private PropertyChangedEventHandler propertyChangedEventHandler;

        public ValueRepresenter()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Маркеры
        /// </summary>
        public List<TimeMarker> Markers
        {
            get { return signalViewer.Markers; }
        }

        public TimeMarker GetNearestMarker(double offset)
        {
            double minDist = double.MaxValue;;
            TimeMarker res = null;
            foreach (TimeMarker marker in Markers)
            {
                double dist = Math.Abs(offset - marker.Offset);
                if (dist < minDist)
                {
                    minDist = dist;
                    res = marker;
                }
            }
            if (minDist <= MinimumMarkerDistance)
                return res;
            else
                return null;
        }

        private SignalViewBase signalViewer;

        void ScaleManager_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UpdateCanvas();
            UpdateSelection();
        }

        /// <summary>
        /// Обновление содержимого канваса
        /// </summary>
        private void UpdateCanvas()
        {
            if (Core.ScaleManager.EndTime != 0)
            {
                if (signalViewer != null)
                {
                    signalViewer.UpdateCanvas(CanvasWaveform, Variable.Iterator);
                    signalViewer.Reset();
                }
                UpdateBookmarks();
            }
            else
                CanvasWaveform.Children.Clear();
        }

        /// <summary>
        /// Обновление заметок
        /// </summary>
        private void UpdateBookmarks()
        {
            List<object> images = new List<object>();
            foreach (var child in CanvasWaveform.Children)
            {
                if (child is Image)
                {
                    images.Add(child);
                }
            }
            foreach (var child in images)
            {
                CanvasWaveform.Children.Remove(child as UIElement);
            }

            foreach (BookMark bm in Core.BookMarks)
            {
                if ((bm.VariableName.Equals(Variable.FullName) == true) && (ScaleManager.IsContainsTime(bm.Time)))
                    AddBookmark(bm);
            }
        }

        /// <summary>
        /// Добавление заметки на долновую диаграмму
        /// </summary>
        /// <param name="bookMark"></param>
        private void AddBookmark(BookMark bookMark)
        {
            Uri bookmarkUri = new Uri("pack://application:,,,/Schematix.Waveform;component/icons/BookMark.jpg", UriKind.RelativeOrAbsolute);

            Image img = new Image();
            JpegBitmapDecoder iconDecoder = new JpegBitmapDecoder(bookmarkUri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            ImageSource bitmapSource2 = iconDecoder.Frames[0];
            img.Source = bitmapSource2;
            img.Stretch = Stretch.Fill;
            img.Margin = new Thickness(0);
            img.Width = 24;
            img.Height = 24;

            img.Tag = bookMark;
            img.ContextMenu = Resources["BookmarkMenu"] as ContextMenu;

            img.MouseEnter += new MouseEventHandler(img_MouseEnter);
            img.MouseLeave += new MouseEventHandler(img_MouseLeave);

            CanvasWaveform.Children.Add(img);
            Canvas.SetLeft(img, ScaleManager.GetOffset(bookMark.Time));
            Canvas.SetTop(img, 2);
        }

        void img_MouseLeave(object sender, MouseEventArgs e)
        {
            popBookmark.IsOpen = false;
            //selectedBookMark = null;
        }

        void img_MouseEnter(object sender, MouseEventArgs e)
        {
            BookMark bookMark = ((sender as Image).Tag) as BookMark;
            selectedBookMark = bookMark;
            TextBlockBookMarkHeader.Text = bookMark.Header;
            TextBlockBookMarkText.Text = bookMark.Text;
            popBookmark.IsOpen = true;
        }


        private void CanvasWaveform_Loaded(object sender, RoutedEventArgs e)
        {
            propertyChangedEventHandler = new System.ComponentModel.PropertyChangedEventHandler(ScaleManager_PropertyChanged);
            ScaleManager.PropertyChanged += propertyChangedEventHandler;
            signalViewer = SignalViewBase.GetSignalViewer(Variable, ScaleManager);

            Core.BookMarks.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(BookMarks_CollectionChanged);

            UpdateCanvas();

            updateView = new UpdateView(CursorViewer_CursorPositionChangedEvent);
            CursorViewer.CursorPositionChangedEvent += updateView;

            Height = Variable.Height;

            this.InvalidateVisual();
        }

        void BookMarks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateBookmarks();
        }

        void CursorViewer_CursorPositionChangedEvent()
        {
            UInt64 time = CursorViewer.Time;
            if (time < ScaleManager.EndTime)
            {
                TimeStampInfo inf = Variable.Signal.Dump.GetValue(time);
                string value = "<NULL>";
                if(inf != null)
                    value = DataContainer.ValueDump.DataConvertorUtils.ToString(inf.LastValue, Variable.DataRepresentation);
                Variable.CursorValue = value;
            }
            else
                Variable.CursorValue = "?";

            UpdateSelection();
        }

        private void UpdateSelection()
        {
            if ((CursorViewer.Selection != null) && (CursorViewer.Selection.Variable == Variable))
            {
                RectangleSelection.Visibility = System.Windows.Visibility.Visible;
                double x1 = ScaleManager.GetOffset(CursorViewer.Selection.Start);
                double x2 = ScaleManager.GetOffset(CursorViewer.Selection.End);

                RectangleSelection.Width = x2 - x1;
                RectangleSelection.Height = CanvasWaveform.ActualHeight;

                if (CanvasWaveform.Children.Contains(RectangleSelection) == false)
                    CanvasWaveform.Children.Add(RectangleSelection);

                Canvas.SetLeft(RectangleSelection, x1);
                Canvas.SetTop(RectangleSelection, 0);
            }
            else
            {
                RectangleSelection.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void MenuItemSignalProperties_Click(object sender, RoutedEventArgs e)
        {
            SignalProperties prop = new SignalProperties(Variable);
            if (prop.ShowDialog() == true)
            {
                UpdateCanvas();
            }
        }

        private void ButtonZoomPlus_Click(object sender, RoutedEventArgs e)
        {
            this.Height += 10;
        }

        private void ButtonZoomMinus_Click(object sender, RoutedEventArgs e)
        {
            if (this.Height > 20)
                this.Height -= 10;
            else
                this.Height = 20;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            ScaleManager.PropertyChanged -= propertyChangedEventHandler;
            CursorViewer.CursorPositionChangedEvent -= updateView;
        }        

        private void MenuItemAddBookMark_Click(object sender, RoutedEventArgs e)
        {
            if (ScaleManager.EndTime == 0)
                return;
            BookMark bm = new BookMark("<header>", "<text>", Variable.FullName,CursorViewer.Time);
            BookMarkProperties bmp = new BookMarkProperties(bm);
            if(bmp.ShowDialog() == true)
            {
                Core.BookMarks.Add(bm);
            }
        }

        private void ButtonCorrectBookMark_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBookMark != null)
            {
                BookMarkProperties prop = new BookMarkProperties(selectedBookMark);
                if (prop.ShowDialog() == true)
                {
                    TextBlockBookMarkHeader.Text = selectedBookMark.Header;
                    TextBlockBookMarkText.Text = selectedBookMark.Text;
                    UpdateBookmarks();
                }
            }
        }

        private void ButtonDeleteBookMark_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBookMark != null)
            {
                Core.BookMarks.Remove(selectedBookMark);
                popBookmark.IsOpen = false;
            }
        }

        private void CanvasWaveform_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.HeightChanged == true)
            {
                Variable.Height = e.NewSize.Height;
                UpdateCanvas();
            }
        }        
    }
}
