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
    /// Базовый клас, служащий для представления простого сигнала
    /// </summary>
    public abstract class SimpleSignalViewBase : SignalViewBase
    {
        public SimpleSignalViewBase(SimpleSignalDump data, ScaleManager scaleManager)
            : base(data, scaleManager)
        {

        }

        public override void UpdateCanvas(Canvas canvas, IValueIterator iter)
        {
            RenderSimpleSignal(canvas, iter, DrawValue);
        }

        public abstract void DrawValue(Canvas canvas, UInt64 start, UInt64 end, ScaleManager manager, string value);
    }
}
