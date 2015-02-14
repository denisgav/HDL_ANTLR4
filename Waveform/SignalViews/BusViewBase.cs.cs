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
using DataContainer;
using DataContainer.MySortedDictionary;
using DataContainer.SignalDump;

namespace Schematix.Waveform.SignalViews
{
    /// <summary>
    /// Базовый класс для отображения шины
    /// </summary>
    public abstract class BusViewBase : SignalViewBase
    {
        public BusViewBase(SignalScopeDump data, ScaleManager scaleManager)
            : base(data, scaleManager)
        {
        }

        public override void UpdateCanvas(Canvas canvas, IValueIterator iter)
        {
            markers.Clear();
            if (iter.DataRepresentation.IsAnalog == true)
                RenderAnalog(canvas, iter);
            else
                RenderSimple(canvas, iter);
        }

        private void RenderSimple(Canvas canvas, IValueIterator iter)
        {
            RenderSimpleBus(canvas, iter, DrawValue);
        }

        public abstract void RenderAnalog(Canvas canvas, IValueIterator iter);

        public abstract void DrawValue(Canvas canvas, UInt64 start, UInt64 end, ScaleManager manager, string value);
    }
}
