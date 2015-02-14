using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using Schematix.Waveform.UserControls;
using System.Numerics;
using DataContainer;
using DataContainer.MySortedDictionary;
using DataContainer.Value;
using DataContainer.SignalDump;

namespace Schematix.Waveform.SignalViews
{
    /// <summary>
    /// Класс, используемый для отображения данных STD_LOGIC
    /// </summary>
    public class Real_View : SignalViewBase
    {
        public Real_View(SimpleSignalDump data, ScaleManager scaleManager)
            : base(data, scaleManager)
        {
        }

        public override void UpdateCanvas(Canvas canvas, IValueIterator iter)
        {
            DataRepresentation datapres = iter.DataRepresentation;
            if (datapres.IsAnalog == true)
                RenderAnalog(canvas, iter);
            else
                RenderSimpleSignal(canvas, iter, DrawValue);
        }

        public void RenderAnalog(Canvas canvas, IValueIterator iter)
        {
            DataRepresentation datapres = iter.DataRepresentation;
            RenderAnalogSimpleSignal(canvas, iter, datapres, GetValueFromTime, GetValueFromItarotor);
        }

        private Double GetValueFromTime(UInt64 time, DataRepresentation DataPres)
        {
            return (data.GetValue(time).LastValue as RealValue).Value;
        }

        private Double GetValueFromItarotor(IValueIterator iter, DataRepresentation DataPres)
        {
            return (iter.CurrentValue.LastValue as RealValue).Value;
        }

        public void DrawValue(Canvas canvas, UInt64 start, UInt64 end, ScaleManager manager, string value)
        {
            double d_start = scaleManager.GetOffset(start);
            double d_end = scaleManager.GetOffset(end);

            if (start == end)
                return;

            AddBusElement(canvas, d_start, d_end, Brushes.Chocolate, Brushes.Transparent, 2, value);
        }
    }
}
