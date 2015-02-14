using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Schematix.Waveform.Value_Dump;
using DataContainer;

namespace Schematix.Waveform.UserControls
{
    /// <summary>
    /// Клас, который отвечает за масштабирование
    /// и определяет границы отображения волновой диаграммы
    /// </summary>
    public class ScaleManager : INotifyPropertyChanged
    {
        private WaveformCore core;
        private TimeScaleViewer timeScaleViewer;

        public ScaleManager(WaveformCore core, TimeScaleViewer timeScaleViewer)
        {
            this.core = core;
            this.timeScaleViewer = timeScaleViewer;
            visibleTimeDiapasone = 10;
        }

        public void Update()
        {
            startTime = core.StartTime;
            endTime = core.EndTime;

            if(endTime == 0)
            {
                endTime = new TimeInterval(1, TimeUnit.ms).GetTimeUnitInFS();
                visibleStartTime = 0;
                visibleTimeDiapasone = endTime;
            }
            else
            {
                if (visibleStartTime >= EndTime)
                {
                    visibleStartTime = startTime;
                    visibleTimeDiapasone = 10;
                }
            }
            timeScaleViewer.UpdateView();
        }

        #region Properties

        /// <summary>
        /// Момент времени, который определяет начало отображаемого участка
        /// </summary>
        private UInt64 visibleStartTime;
        public UInt64 VisibleStartTime
        {
            get { return visibleStartTime; }
            set 
            {
                if((value<startTime)||(value>endTime))
                    throw new Exception("incorrect start time");
                visibleStartTime = value;
                NotifyPropertyChanged("VisibleStartTime");
            }
        }

        /// <summary>
        /// размер видимого промежутка времени
        /// </summary>
        private UInt64 visibleTimeDiapasone;
        public UInt64 VisibleTimeDiapasone
        {
            get { return visibleTimeDiapasone; }
            set 
            {
                if (value < 10)
                    visibleTimeDiapasone = 10;
                else
                    visibleTimeDiapasone = value;
                NotifyPropertyChanged("VisibleTimeDiapasone");
            }
        }

        /// <summary>
        /// Момент времени, который определяет конец отображаемого участка
        /// </summary>
        public UInt64 VisibleEndTime
        {
            get 
            {
                UInt64 res = visibleStartTime + visibleTimeDiapasone;
                if (res < endTime)
                    return res;
                else
                    return endTime;
            }
        }

        /// <summary>
        /// Момент времени, который определяет начало волновой диаграммы
        /// </summary>
        private UInt64 startTime;
        public UInt64 StartTime
        {
            get { return startTime; }
            set
            {
                startTime = value;
                NotifyPropertyChanged("StartTime");
            }
        }

        /// <summary>
        /// Момент времени, который определяет конец волновой диаграммы
        /// </summary>
        private UInt64 endTime;
        public UInt64 EndTime
        {
            get { return endTime; }
            set
            {
                endTime = value;
                if (endTime > VisibleEndTime)
                {
                    if (endTime < visibleTimeDiapasone)
                    {
                        visibleStartTime = 0;
                        visibleTimeDiapasone = endTime;
                    }
                    else
                    {
                        visibleStartTime = endTime - visibleTimeDiapasone;
                    }
                }
                NotifyPropertyChanged("EndTime");
            }
        }

        /// <summary>
        /// Можно ли увеличить масштаб
        /// </summary>
        public bool CanZoomIn
        {
            get
            {
                return visibleTimeDiapasone > 10;
            }
        }

        /// <summary>
        /// Mожно ли уменьшить масштаб
        /// </summary>
        public bool CanZoomOut
        {
            get
            {
                return (visibleTimeDiapasone < (endTime - startTime)); 
            }
        }

        /// <summary>
        /// Можно ли промасштабировать по ширине
        /// </summary>
        public bool CanZoomToFit
        {
            get
            {
                return (visibleTimeDiapasone >= 10) && (visibleTimeDiapasone < (endTime-startTime));
            }
        }

        /// <summary>
        /// Определяет, принадлежит ли конкретный момент
        /// времени отображаемому временному диапазону
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool IsContainsTime(UInt64 time)
        {
            return ((time > VisibleStartTime) && (time <= VisibleEndTime));
        }

        /// <summary>
        /// Ширина элемента отображения
        /// </summary>
        private double width;
        public double Width
        {
            get { return width; }
            set 
            {
                width = value;
                NotifyPropertyChanged("VisibleTimeDiapasone");
            }
        }

        /// <summary>
        /// минимальный промежуток времени, который может отображать изменение сигнала
        /// </summary>
        public UInt64 MinimumVisibleChange
        {
            get 
            {
                return (UInt64)(((double)visibleTimeDiapasone / Width) * (double)MinimumChangePointsCount); 
            }
        }

        /// <summary>
        /// Увеличение масштаба
        /// </summary>
        public void ZoomIn()
        {
            if ((visibleTimeDiapasone / 2) <= MinimumViewedTime)
                visibleTimeDiapasone = MinimumViewedTime;
            else
                visibleTimeDiapasone /= 2;
            NotifyPropertyChanged("VisibleTimeDiapasone");
        }

        /// <summary>
        /// Уменшение масштаба
        /// </summary>
        public void ZoomOut()
        {
            if ((visibleTimeDiapasone * 2) >= (endTime - startTime))
                visibleTimeDiapasone = endTime - startTime;
            else
                visibleTimeDiapasone *= 2;
            NotifyPropertyChanged("VisibleTimeDiapasone");
        }

        /// <summary>
        /// Устанавливает масштаб по ширине экрана
        /// </summary>
        public void ZoomToFit()
        {
            visibleTimeDiapasone = endTime - startTime;
            visibleStartTime = 0;
            NotifyPropertyChanged("VisibleTimeDiapasone");
        }

        /// <summary>
        /// Масштабирование по выбранной области
        /// </summary>
        public void ZoomToSelection(Selection selection)
        {
            visibleStartTime = selection.Start;
            visibleTimeDiapasone = (selection.TimeInterval > 10)?selection.TimeInterval:10;
            NotifyPropertyChanged("VisibleTimeDiapasone");
        }

        /// <summary>
        /// Количество пикселей для минимального изменения сигнала
        /// (после которого он стает видимым)
        /// </summary>
        private const uint MinimumChangePointsCount = 10;

        /// <summary>
        /// Минимальный промежуток времени, который отображается на волновой диаграмме
        /// </summary>
        private const uint MinimumViewedTime = 10;

        #endregion

        #region Функции преобразования экранных координат и времени

        /// <summary>
        /// Преобразует экранные координаты во время
        /// </summary>
        /// <param name="X"></param>
        /// <returns></returns>
        public UInt64 GetTime(double X)
        {
            UInt64 time1 = VisibleStartTime + (UInt64)((double)VisibleTimeDiapasone * X / Width);
            UInt64 time2 = VisibleStartTime + (UInt64)((double)VisibleTimeDiapasone * (X + 1) / Width);
            UInt64 delta = time2 - time1;
            UInt64 time = time1 + delta / 2;
            if (delta > 0)
            {
                UInt64 pow = (UInt64)Math.Pow(10, Math.Floor(Math.Log10(delta)));
                time = (time / pow) * pow;
            }
            return time;
        }

        /// <summary>
        /// Расстояние по времени между двумя смещениями
        /// </summary>
        /// <param name="X1"></param>
        /// <param name="X2"></param>
        /// <returns></returns>
        public Int64 Delta(double X1, double X2)
        {
            return (Int64)(VisibleTimeDiapasone * (X2 - X1) / Width);
        }

        /// <summary>
        /// Преобразует время к экранным координатам
        /// </summary>
        /// <param name="Time"></param>
        /// <returns></returns>
        public double GetOffset(UInt64 Time)
        {
            if (Time < VisibleStartTime)
                return 0;
            return (double)(Time - VisibleStartTime) * Width / (double)VisibleTimeDiapasone;
        }

        #endregion

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
    }
}
