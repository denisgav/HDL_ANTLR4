using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Schematix.Waveform.UserControls;
using System.Windows.Media;
using Schematix.Waveform.Value_Dump;
using System.Windows.Controls;
using DataContainer;
using DataContainer.MySortedDictionary;
using DataContainer.SignalDump;

namespace Schematix.Waveform.SignalViews
{
    public class CompositeDataTypeView : SignalViewBase
    {
        public CompositeDataTypeView(SignalScopeDump data, ScaleManager scaleManager)
            :base (data, scaleManager)
        {
        }

        public override void UpdateCanvas(System.Windows.Controls.Canvas canvas, IValueIterator iter)
        {
            RenderSimpleBus(canvas, iter, DrawValue);
        }

        public void DrawValue(Canvas canvas, ulong start, ulong end, ScaleManager manager, string value)
        {
            AddBusElement(canvas, scaleManager.GetOffset(start), scaleManager.GetOffset(end), Brushes.Green, Brushes.Transparent, 2, value);
        }
    }
}
