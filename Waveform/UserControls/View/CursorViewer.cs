using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Windows;
using System.ComponentModel;
using DataContainer;

namespace Schematix.Waveform.UserControls
{
    /// <summary>
    /// Класс, используемый для отображения курсора
    /// </summary>
    public class CursorViewer : IDisposable
    {
        private WaveformUserControl wuc;
        private WaveformCore core;
        private Grid GridMain;
        private Line line;
        private ScaleManager scaleManager;

        private MouseEventHandler mouseHandler;

        public event UpdateView CursorPositionChangedEvent;

        public CursorViewer(WaveformUserControl wuc, WaveformCore core)
        {
            this.wuc = wuc;
            this.core = core;
            GridMain = wuc.GridMain;
            line = wuc.LineCursor;
            scaleManager = core.ScaleManager;

            mouseHandler = new MouseEventHandler(GridMain_PreviewMouseMove);
            GridMain.PreviewMouseMove += mouseHandler;
        }

        /// <summary>
        /// Позиция курсора
        /// </summary>
        private double posX;
        public double PosX
        {
            get
            {
                return posX;
            }
        }

        /// <summary>
        /// Момент времени, на котором находится курсор
        /// </summary>
        private UInt64 time;
        public UInt64 Time
        {
            get { return time; }
        }

        /// <summary>
        /// Выбранная область
        /// </summary>
        private Selection selection;
        public Selection Selection
        {
            get { return selection; }
            set
            {
                selection = value;
                CursorPositionChangedEvent();
            }
        }

        /// <summary>
        /// Возвращает значение текущего момента времени в формате string
        /// </summary>
        public string TimeString
        {
            get
            {
                return TimeInterval.ToString(time);
            }
        }

        /// <summary>
        /// Ширина элемента управления
        /// </summary>
        private double ControlWidth
        {
            get { return scaleManager.Width; }
        }

        public void Update()
        {
            CursorPositionChangedEvent();
        }

        void GridMain_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            double oldPos = posX;
            Point pt = e.GetPosition(GridMain);

            double WidthTimescale = ControlWidth;
            double WidthGridMain = GridMain.ActualWidth;

            if (pt.X < (WidthGridMain - WidthTimescale))
                pt.X = (WidthGridMain - WidthTimescale);
            if (pt.X > WidthGridMain - 15)
                pt.X = WidthGridMain - 15;

            line.X1 = pt.X;
            line.X2 = pt.X;
            line.Y1 = 0;
            line.Y2 = GridMain.ActualHeight;

            posX = e.GetPosition(wuc.timeScaleViewer).X;
            if (posX < 0)
                posX = 0;

            //вычисление текущего момента времени
            time = scaleManager.GetTime(posX);
            
            if (oldPos != posX)
                CursorPositionChangedEvent();
        }

        #region IDisposable Members
        public void Dispose()
        {
            if (mouseHandler != null)
                GridMain.PreviewMouseMove -= mouseHandler;
        }
        #endregion
    }
}