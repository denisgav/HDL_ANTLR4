using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using Schematix.Waveform.UserControls;
using Schematix.Waveform.Value_Dump;
using System.Numerics;
using DataContainer;
using DataContainer.MySortedDictionary;
using VHDL.type;
using DataContainer.SignalDump;
using System.Globalization;

namespace Schematix.Waveform.SignalViews
{
    /// <summary>
    /// Базовый клас, служащий для представления сигнала
    /// </summary>
    public abstract class SignalViewBase
    {
        /// <summary>
        /// Информация о полосе прокрутки
        /// </summary>
        protected ScaleManager scaleManager;
        public ScaleManager ScaleManager
        {
            get { return scaleManager; }
            set { scaleManager = value; }
        }

        /// <summary>
        /// Представляемые данные
        /// </summary>
        protected AbstractSignalDump data;
        public AbstractSignalDump Data
        {
            get { return data; }
            set { data = value; }
        }

        /// <summary>
        /// Маркеры
        /// </summary>
        protected List<TimeMarker> markers;
        public List<TimeMarker> Markers
        {
            get { return markers; }
            set { markers = value; }
        }

        public SignalViewBase(AbstractSignalDump data, ScaleManager scaleManager)
        {
            this.data = data;
            this.scaleManager = scaleManager;
            markers = new List<TimeMarker>();
        }

        /// <summary>
        /// Обновление изображения
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="iter"></param>
        public abstract void UpdateCanvas(Canvas canvas, IValueIterator iter);

        /// <summary>
        /// Сброс параметров для отображения
        /// </summary>
        public virtual void Reset()
        {
            //markers.Clear();
        }


        /// <summary>
        /// Добавление горизонтальной линии на canvas
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="X1"></param>
        /// <param name="X2"></param>
        /// <param name="Y"></param>
        /// <param name="brush"></param>
        /// <param name="thickness"></param>
        protected static void AddLine(Canvas canvas, double X1, double X2, double Y, Brush brush, double thickness)
        {
            Line line = new Line();
            line.Stroke = brush;
            line.X1 = X1;
            line.X2 = X2;
            line.Y1 = Y;
            line.Y2 = Y;
            line.StrokeThickness = thickness;

            canvas.Children.Add(line);
        }

        /// <summary>
        /// Добавление на канвас "серой" области
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="X1"></param>
        /// <param name="X2"></param>
        /// <param name="manager"></param>
        protected static void AddWhitespace(Canvas canvas, double X1, double X2, ScaleManager manager)
        {
            if (X2 <= X1)
                return;

            double d_start = manager.GetOffset((UInt64)X1);
            double d_end = manager.GetOffset((UInt64)X2);

            Rectangle rect = new Rectangle();
            rect.Stroke = new SolidColorBrush(Colors.Green);
            rect.Fill = new SolidColorBrush(Color.FromArgb(100, 125, 125, 125));
            rect.Width = d_end - d_start;
            rect.Height = (canvas.ActualHeight != 0) ? canvas.ActualHeight - 4 : 4;

            canvas.Children.Add(rect);
            Canvas.SetLeft(rect, d_start);
            Canvas.SetBottom(rect, 2);
        }

        /// <summary>
        /// Добавление на канвас рерткальной линии
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="X"></param>
        /// <param name="Y1"></param>
        /// <param name="Y2"></param>
        protected static void AddVerticalLine(Canvas canvas, double X, double Y1, Double Y2)
        {
            Line line = new Line();
            line.Stroke = Brushes.Gray;
            line.X1 = X;
            line.X2 = X;
            line.Y1 = Y1;
            line.Y2 = Y2;
            line.StrokeThickness = 1;

            canvas.Children.Add(line);
        }

        /// <summary>
        /// Добавление на канвас прямоугольника с текстом внутри
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="X1"></param>
        /// <param name="X2"></param>
        /// <param name="stroke"></param>
        /// <param name="fill"></param>
        /// <param name="thickness"></param>
        /// <param name="text"></param>
        protected static void AddRectangle(Canvas canvas, double X1, double X2, Brush stroke, Brush fill, double thickness, string text)
        {
            Rectangle rect = new Rectangle();
            rect.Stroke = stroke;
            rect.StrokeThickness = thickness;
            rect.Fill = fill;
            rect.Width = X2 - X1;
            rect.Height = (canvas.ActualHeight != 0) ? (canvas.ActualHeight - 1 - 2 * thickness) : 0;

            canvas.Children.Add(rect);
            Canvas.SetLeft(rect, X1);
            Canvas.SetTop(rect, thickness + 1);

            if (string.IsNullOrEmpty(text) == false)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = text;
                textBlock.FontSize = 10;
                textBlock.Width = X2 - X1;
                textBlock.TextAlignment = TextAlignment.Center;
                textBlock.TextWrapping = TextWrapping.Wrap;
                textBlock.MaxHeight = (canvas.ActualHeight != 0) ? (canvas.ActualHeight - 4) : 0;

                canvas.Children.Add(textBlock);
                Canvas.SetLeft(textBlock, X1);
                Canvas.SetRight(textBlock, X2);
                Canvas.SetTop(textBlock, (canvas.ActualHeight - textBlock.FontSize) / 2.0);
            }
        }

        /// <summary>
        /// Добавление на канвас элемента в виде шины
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="X1"></param>
        /// <param name="X2"></param>
        /// <param name="stroke"></param>
        /// <param name="fill"></param>
        /// <param name="thickness"></param>
        /// <param name="text"></param>
        protected static void AddBusElement(Canvas canvas, double X1, double X2, Brush stroke, Brush fill, double thickness, string text)
        {
            if ((X2 - X1 - 6.0) <= 0)
                return;
            Polygon pol = new Polygon();
            pol.Stroke = stroke;
            pol.StrokeThickness = thickness;
            pol.Fill = fill;

            double center = canvas.ActualHeight / 2;

            pol.Points.Add(new Point(X1, center));
            pol.Points.Add(new Point(X1 + 3, 2));
            pol.Points.Add(new Point(X2 - 3, 2));
            pol.Points.Add(new Point(X2, center));
            pol.Points.Add(new Point(X2 - 3, canvas.ActualHeight - 2));
            pol.Points.Add(new Point(X1 + 3, canvas.ActualHeight - 2));
            canvas.Children.Add(pol);


            if (string.IsNullOrEmpty(text) == false)
            {
                Grid vb = new Grid();
                vb.HorizontalAlignment = HorizontalAlignment.Stretch;
                vb.VerticalAlignment = VerticalAlignment.Stretch;
                vb.ColumnDefinitions.Add(new ColumnDefinition());
                vb.RowDefinitions.Add(new RowDefinition());

                vb.Width = (X2 - X1 - 6);
                vb.Height = (canvas.ActualHeight > 4) ? (canvas.ActualHeight - 4) : 0;

                TextBlock textBlock = new TextBlock();
                textBlock.Text = text;
                textBlock.FontSize = 10;
                textBlock.TextAlignment = TextAlignment.Center;
                textBlock.TextWrapping = TextWrapping.Wrap;
                textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                textBlock.VerticalAlignment = VerticalAlignment.Center;

                vb.Children.Add(textBlock);
                Grid.SetColumn(textBlock, 0);
                Grid.SetRow(textBlock, 0);

                canvas.Children.Add(vb);
                Canvas.SetLeft(vb, X1 + 3);
                Canvas.SetTop(vb, 2);
            }
        }

        /// <summary>
        /// Делегат, который используется для функции RenderSimpleSignal и RenderSimpleBus
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="manager"></param>
        /// <param name="value"></param>
        protected delegate void DrawValueDelegate(Canvas canvas, UInt64 start, UInt64 end, ScaleManager manager, string value);

        /// <summary>
        /// Отображение простого сигнала
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="iter"></param>
        /// <param name="DrawValue"></param>
        protected void RenderSimpleSignal(Canvas canvas, IValueIterator iter, DrawValueDelegate DrawValue)
        {
            markers.Clear();
            canvas.Children.Clear();

            UInt64 MinimumVisibleChange = scaleManager.MinimumVisibleChange;

            //Рисование начала диаграммы
            UInt64 x1 = 0, x2 = 0;
            string value = string.Empty;

            x1 = scaleManager.VisibleStartTime;

            iter.SetCurrentIndexByKey(scaleManager.VisibleStartTime);
            x2 = iter.LastEvent;
            if (x2 > scaleManager.VisibleEndTime)
                x2 = scaleManager.VisibleEndTime;
            if (x2 < scaleManager.VisibleStartTime)
                x2 = scaleManager.VisibleStartTime;

            Markers.Add(new TimeMarker(x1, scaleManager.GetOffset(x1)));

            while (true)
            {
                //Началась "серая" область
                if ((x2 - x1) <= MinimumVisibleChange)
                {
                    UInt64 whitespace = x1;

                    while (true)
                    {
                        x1 = iter.LastEvent;
                        if (x1 > scaleManager.VisibleEndTime)
                            x1 = scaleManager.VisibleEndTime;
                        if (x1 < scaleManager.VisibleStartTime)
                            x1 = scaleManager.VisibleStartTime;
                        iter.MoveNext();
                        x2 = iter.LastEvent;
                        if (x2 > scaleManager.VisibleEndTime)
                            x2 = scaleManager.VisibleEndTime;
                        if (x2 < scaleManager.VisibleStartTime)
                            x2 = scaleManager.VisibleStartTime;
                        //закончилась "серая" область
                        if ((x2 - x1) >= scaleManager.MinimumVisibleChange)
                        {
                            AddWhitespace(canvas, whitespace, x1, scaleManager);
                            value = AbstractSignalDump.GetStringValue(iter);
                            Markers.Add(new TimeMarker(x1, scaleManager.GetOffset(x1)));
                            DrawValue(canvas, x1, x2, scaleManager, value);
                            break;
                        }

                        if ((iter.LastEvent >= scaleManager.VisibleEndTime) || (iter.IsEndOfIteration == true))
                        {
                            AddWhitespace(canvas, whitespace, iter.LastEvent, scaleManager);
                            Markers.Add(new TimeMarker(iter.LastEvent, scaleManager.GetOffset(iter.LastEvent)));
                            break;
                        }
                        iter.SetCurrentIndexByKey(x1 + MinimumVisibleChange);
                    }
                }
                else
                {
                    value = AbstractSignalDump.GetStringValue(iter);
                    Markers.Add(new TimeMarker(x1, scaleManager.GetOffset(x1)));
                    DrawValue(canvas, x1, x2, scaleManager, value);
                    iter.MoveNext();
                }

                x1 = x2;
                x2 = iter.LastEvent;

                if ((iter.LastEvent >= scaleManager.VisibleEndTime) || (iter.IsEndOfIteration == true))
                {
                    value = AbstractSignalDump.GetStringValue(iter);
                    Markers.Add(new TimeMarker(x1, scaleManager.GetOffset(x1)));
                    DrawValue(canvas, x1, scaleManager.VisibleEndTime, scaleManager, value);
                    break;
                }

                if (x2 > scaleManager.VisibleEndTime)
                    x2 = scaleManager.VisibleEndTime;
                if (x2 < scaleManager.VisibleStartTime)
                    x2 = scaleManager.VisibleStartTime;
            }
        }

        /// <summary>
        /// Отображение шины в обычном виде
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="iter"></param>
        /// <param name="DrawValue"></param>
        protected void RenderSimpleBus(Canvas canvas, IValueIterator iter, DrawValueDelegate DrawValue)
        {
            canvas.Children.Clear();

            UInt64 MinimumVisibleChange = scaleManager.MinimumVisibleChange;

            //Рисование начала диаграммы
            UInt64 x1 = 0, x2 = 0;
            string value = string.Empty;

            x1 = scaleManager.VisibleStartTime;

            iter.SetCurrentIndexByKey(scaleManager.VisibleStartTime);
            x2 = iter.LastEvent;
            if (x2 > scaleManager.VisibleEndTime)
                x2 = scaleManager.VisibleEndTime;
            if (x2 < scaleManager.VisibleStartTime)
                x2 = scaleManager.VisibleStartTime;

            Markers.Add(new TimeMarker(x1, scaleManager.GetOffset(x1)));

            while (true)
            {
                //Началась "серая" область
                if ((x2 - x1) <= MinimumVisibleChange)
                {
                    UInt64 whitespace = x1;

                    while (true)
                    {
                        x1 = iter.LastEvent;
                        if (x1 > scaleManager.VisibleEndTime)
                            x1 = scaleManager.VisibleEndTime;
                        if (x1 < scaleManager.VisibleStartTime)
                            x1 = scaleManager.VisibleStartTime;
                        iter.MoveNext();
                        x2 = iter.LastEvent;
                        if (x2 > scaleManager.VisibleEndTime)
                            x2 = scaleManager.VisibleEndTime;
                        if (x2 < scaleManager.VisibleStartTime)
                            x2 = scaleManager.VisibleStartTime;
                        //закончилась "серая" область
                        if ((x2 - x1) >= scaleManager.MinimumVisibleChange)
                        {
                            AddWhitespace(canvas, whitespace, x1, scaleManager);
                            value = AbstractSignalDump.GetStringValue(iter);
                            Markers.Add(new TimeMarker(x1, scaleManager.GetOffset(x1)));
                            DrawValue(canvas, x1, x2, scaleManager, value);
                            break;
                        }

                        if ((iter.LastEvent >= scaleManager.VisibleEndTime) || (iter.IsEndOfIteration == true))
                        {
                            Markers.Add(new TimeMarker(whitespace, scaleManager.GetOffset(whitespace)));
                            AddWhitespace(canvas, whitespace, iter.LastEvent, scaleManager);
                            break;
                        }
                        iter.SetCurrentIndexByKey(x1 + MinimumVisibleChange);
                    }
                }
                else
                {
                    value = AbstractSignalDump.GetStringValue(iter);
                    Markers.Add(new TimeMarker(x1, scaleManager.GetOffset(x1)));
                    DrawValue(canvas, x1, x2, scaleManager, value);
                    iter.MoveNext();
                }

                x1 = x2;
                x2 = iter.LastEvent;

                if ((iter.LastEvent >= scaleManager.VisibleEndTime) || (iter.IsEndOfIteration == true))
                {
                    value = AbstractSignalDump.GetStringValue(iter);
                    Markers.Add(new TimeMarker(x1, scaleManager.GetOffset(x1)));
                    DrawValue(canvas, x1, scaleManager.VisibleEndTime, scaleManager, value);
                    break;
                }

                if (x2 > scaleManager.VisibleEndTime)
                    x2 = scaleManager.VisibleEndTime;
                if (x2 < scaleManager.VisibleStartTime)
                    x2 = scaleManager.VisibleStartTime;
            }
        }



        /// <summary>
        /// Делегат для функции получения значения BigInteger с учетом DataPres
        /// </summary>
        /// <param name="time"></param>
        /// <param name="DataPres"></param>
        /// <returns></returns>
        protected delegate T GetValueDelegateFromTime<T>(UInt64 time, DataRepresentation DataPres) where T : IConvertible;
        protected delegate T GetValueDelegateFromIterator<T>(IValueIterator iter, DataRepresentation DataPres) where T : IConvertible;

        /// <summary>
        /// Отображение шины в аналоговом виде
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="iter"></param>
        /// <param name="DataPres"></param>
        /// <param name="GetValue"></param>
        protected void RenderAnalogBus<T>(Canvas canvas, IValueIterator iter, VectorDataRepresentation DataPres, GetValueDelegateFromTime<T> GetValueFromTime, GetValueDelegateFromIterator<T> GetValueFromIterator) where T : IConvertible
        {
            canvas.Children.Clear();

            Polyline polyLine = new Polyline();
            polyLine.Stroke = Brushes.Black;
            polyLine.StrokeThickness = 1;


            if (scaleManager.VisibleTimeDiapasone < (scaleManager.Width * 5.0))
            {
                iter.SetCurrentIndexByKey(scaleManager.VisibleStartTime);

                double min = GetValueFromTime(ScaleManager.VisibleStartTime, DataPres).ToDouble(CultureInfo.InvariantCulture);
                double max = min;
                double value = 0;

                double height = canvas.ActualHeight;


                iter.SetCurrentIndexByKey(scaleManager.VisibleStartTime);
                value = GetValueFromTime(ScaleManager.VisibleStartTime, DataPres).ToDouble(CultureInfo.InvariantCulture);
                polyLine.Points.Add(new Point(0, Convert.ToDouble(value)));

                //Рисование начала диаграммы
                UInt64 x1 = 0, x2 = 0;

                x1 = scaleManager.VisibleStartTime;
                UInt64 MinimumVisibleChange = scaleManager.MinimumVisibleChange * 2;

                iter.SetCurrentIndexByKey(scaleManager.VisibleStartTime);
                x2 = iter.LastEvent;
                if (x2 > scaleManager.VisibleEndTime)
                    x2 = scaleManager.VisibleEndTime;
                if (x2 < scaleManager.VisibleStartTime)
                    x2 = scaleManager.VisibleStartTime;
                while (true)
                {
                    value = GetValueFromIterator(iter, DataPres).ToDouble(CultureInfo.InvariantCulture);

                    if (min > value)
                        min = value;

                    if (max < value)
                        max = value;

                    polyLine.Points.Add(new Point(scaleManager.GetOffset(x1), Convert.ToDouble(value)));
                    polyLine.Points.Add(new Point(scaleManager.GetOffset(x2), Convert.ToDouble(value)));

                    iter.MoveNext();

                    x1 = x2;
                    x2 = iter.LastEvent;

                    if ((iter.LastEvent >= scaleManager.VisibleEndTime) || (iter.IsEndOfIteration == true))
                    {
                        value = GetValueFromTime(ScaleManager.VisibleEndTime, DataPres).ToDouble(CultureInfo.InvariantCulture);
                        if (min > value)
                            min = value;

                        if (max < value)
                            max = value;
                        polyLine.Points.Add(new Point(scaleManager.GetOffset(x1), Convert.ToDouble(value)));
                        polyLine.Points.Add(new Point(scaleManager.GetOffset(x2), Convert.ToDouble(value)));
                        break;
                    }

                    if (x2 > scaleManager.VisibleEndTime)
                        x2 = scaleManager.VisibleEndTime;
                    if (x2 < scaleManager.VisibleStartTime)
                        x2 = scaleManager.VisibleStartTime;
                }
                double DeltaY = (max != min) ? (height / Convert.ToDouble(max - min)) : 1;
                for (int i = 0; i < polyLine.Points.Count; i++)
                {
                    Point point = polyLine.Points[i];
                    point.Y = (Convert.ToDouble(max) - point.Y) * DeltaY;
                    polyLine.Points[i] = point;
                }
            }
            else
            {
                iter.SetCurrentIndexByKey(scaleManager.VisibleStartTime);

                double min = GetValueFromTime(ScaleManager.VisibleStartTime, DataPres).ToDouble(CultureInfo.InvariantCulture);
                double max = min;
                double value = 0;

                double height = canvas.ActualHeight;

                for (double x = 0; x < scaleManager.Width; x++)
                {
                    UInt64 time = scaleManager.GetTime(x);
                    iter.SetCurrentIndexByKey(time);
                    value = GetValueFromIterator(iter, DataPres).ToDouble(CultureInfo.InvariantCulture);
                    if (min > value)
                        min = value;

                    if (max < value)
                        max = value;
                    polyLine.Points.Add(new Point(x, Convert.ToDouble(value)));
                }
                double DeltaY = (max != min) ? (height / Convert.ToDouble(max - min)) : 1;
                for (int i = 0; i < polyLine.Points.Count; i++)
                {
                    Point point = polyLine.Points[i];
                    point.Y = (Convert.ToDouble(max) - point.Y) * DeltaY;
                    polyLine.Points[i] = point;
                }
            }

            canvas.Children.Add(polyLine);
        }

        /// <summary>
        /// Отображение сигнала в аналоговом виде
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="iter"></param>
        /// <param name="DataPres"></param>
        /// <param name="GetValue"></param>
        protected void RenderAnalogSimpleSignal<T>(Canvas canvas, IValueIterator iter, DataRepresentation DataPres, GetValueDelegateFromTime<T> GetValueFromTime, GetValueDelegateFromIterator<T> GetValueFromIterator) where T : IConvertible
        {
            canvas.Children.Clear();

            Polyline polyLine = new Polyline();
            polyLine.Stroke = Brushes.Black;
            polyLine.StrokeThickness = 1;


            if (scaleManager.VisibleTimeDiapasone < (scaleManager.Width * 5.0))
            {
                iter.SetCurrentIndexByKey(scaleManager.VisibleStartTime);

                double min = GetValueFromTime(ScaleManager.VisibleStartTime, DataPres).ToDouble(CultureInfo.InvariantCulture); ;
                double max = min;
                double value = 0;

                double height = canvas.ActualHeight;


                iter.SetCurrentIndexByKey(scaleManager.VisibleStartTime);
                value = GetValueFromTime(ScaleManager.VisibleStartTime, DataPres).ToDouble(CultureInfo.InvariantCulture); ;
                polyLine.Points.Add(new Point(0, (double)(value)));

                //Рисование начала диаграммы
                UInt64 x1 = 0, x2 = 0;

                x1 = scaleManager.VisibleStartTime;
                UInt64 MinimumVisibleChange = scaleManager.MinimumVisibleChange * 2;

                iter.SetCurrentIndexByKey(scaleManager.VisibleStartTime);
                x2 = iter.LastEvent;
                if (x2 > scaleManager.VisibleEndTime)
                    x2 = scaleManager.VisibleEndTime;
                if (x2 < scaleManager.VisibleStartTime)
                    x2 = scaleManager.VisibleStartTime;
                while (true)
                {
                    value = GetValueFromIterator(iter, DataPres).ToDouble(CultureInfo.InvariantCulture); ;

                    if (min > value)
                        min = value;

                    if (max < value)
                        max = value;

                    polyLine.Points.Add(new Point(scaleManager.GetOffset(x1), (double)(value)));
                    polyLine.Points.Add(new Point(scaleManager.GetOffset(x2), (double)(value)));

                    iter.MoveNext();

                    x1 = x2;
                    x2 = iter.LastEvent;

                    if ((iter.LastEvent >= scaleManager.VisibleEndTime) || (iter.IsEndOfIteration == true))
                    {
                        value = GetValueFromTime(ScaleManager.VisibleEndTime, DataPres).ToDouble(CultureInfo.InvariantCulture); ;
                        if (min > value)
                            min = value;

                        if (max < value)
                            max = value;
                        polyLine.Points.Add(new Point(scaleManager.GetOffset(x1), (double)(value)));
                        polyLine.Points.Add(new Point(scaleManager.GetOffset(x2), (double)(value)));
                        break;
                    }

                    if (x2 > scaleManager.VisibleEndTime)
                        x2 = scaleManager.VisibleEndTime;
                    if (x2 < scaleManager.VisibleStartTime)
                        x2 = scaleManager.VisibleStartTime;
                }
                double DeltaY = (max != min) ? (height / (double)(max - min)) : 1;
                for (int i = 0; i < polyLine.Points.Count; i++)
                {
                    Point point = polyLine.Points[i];
                    point.Y = ((double)max - point.Y) * DeltaY;
                    polyLine.Points[i] = point;
                }
            }
            else
            {
                iter.SetCurrentIndexByKey(scaleManager.VisibleStartTime);

                double min = GetValueFromTime(ScaleManager.VisibleStartTime, DataPres).ToDouble(CultureInfo.InvariantCulture); ;
                double max = min;
                double value = 0;

                double height = canvas.ActualHeight;

                for (double x = 0; x < scaleManager.Width; x++)
                {
                    UInt64 time = scaleManager.GetTime(x);
                    iter.SetCurrentIndexByKey(time);
                    value = GetValueFromIterator(iter, DataPres).ToDouble(CultureInfo.InvariantCulture); ;
                    if (min > value)
                        min = value;

                    if (max < value)
                        max = value;
                    polyLine.Points.Add(new Point(x, (double)(value)));
                }
                double DeltaY = (max != min) ? (height / (double)(max - min)) : 1;
                for (int i = 0; i < polyLine.Points.Count; i++)
                {
                    Point point = polyLine.Points[i];
                    point.Y = ((double)max - point.Y) * DeltaY;
                    polyLine.Points[i] = point;
                }
            }

            canvas.Children.Add(polyLine);
        }

        /// <summary>
        /// Функция, которая возвращает класс, отвечающий за отображение
        /// </summary>
        /// <param name="Variable"></param>
        /// <param name="ScaleManager"></param>
        /// <returns></returns>
        public static SignalViewBase GetSignalViewer(My_Variable Variable, ScaleManager ScaleManager)
        {
            ModellingType type = Variable.Signal.Type;
            if (type.Type is IntegerType)
            {
                return new INTEGER_View(Variable.Signal.Dump as SimpleSignalDump, ScaleManager);
            }
            if (type.Type is RealType)
            {
                return new Real_View(Variable.Signal.Dump as SimpleSignalDump, ScaleManager);
            }
            if (type.Type == VHDL.builtin.StdLogic1164.STD_ULOGIC)
            {
                return new STD_LOGIC_View(Variable.Signal.Dump as SimpleSignalDump, ScaleManager);
            }
            if (type.Type == VHDL.builtin.StdLogic1164.STD_ULOGIC_VECTOR)
            {
                return new STD_LOGIC_VECTOR_View(Variable.Signal.Dump as SignalScopeDump, ScaleManager);
            }
            if (type.Type == VHDL.builtin.StdLogic1164.STD_LOGIC_VECTOR)
            {
                return new STD_LOGIC_VECTOR_View(Variable.Signal.Dump as SignalScopeDump, ScaleManager);
            }
            if (type.Type is RecordType)
            {
                return new CompositeDataTypeView(Variable.Signal.Dump as SignalScopeDump, ScaleManager);
            }
            if (type.Type is ArrayType)
            {
                return new CompositeDataTypeView(Variable.Signal.Dump as SignalScopeDump, ScaleManager);
            }
            return null;
        }
    }
}