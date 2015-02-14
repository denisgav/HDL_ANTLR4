using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Numerics;
using System.Windows.Shapes;
using System.Windows;
using Schematix.Waveform.UserControls;
using DataContainer;
using DataContainer.MySortedDictionary;
using DataContainer.Value;
using DataContainer.SignalDump;

namespace Schematix.Waveform.SignalViews
{
    /// <summary>
    /// Класс, используемый для отображения данных STD_LOGIC
    /// </summary>
    public class STD_LOGIC_VECTOR_View : BusViewBase
    {
        public STD_LOGIC_VECTOR_View(SignalScopeDump data, ScaleManager scaleManager)
            : base(data, scaleManager)
        {
        }

        public override void DrawValue(Canvas canvas, ulong start, ulong end, ScaleManager manager, string value)
        {
            Brush stroce = (value.Contains('X') || value.Contains('U')) ? Brushes.Red : Brushes.Black;
            AddBusElement(canvas, scaleManager.GetOffset(start), scaleManager.GetOffset(end), stroce, Brushes.Transparent, 1, value);
        }

        public override void RenderAnalog(Canvas canvas, IValueIterator iter)
        {
            VectorDataRepresentation datapres = iter.DataRepresentation as VectorDataRepresentation;

            RenderAnalogBus(canvas, iter, datapres, GetValueFromTime, GetValueFromItarotor);
        }

        private Double GetValueFromTime(UInt64 time, DataRepresentation DataPres)
        {
            return (double)(DataContainer.ValueDump.DataConvertorUtils.ToBigInteger((data.GetValue(time).LastValue as STD_ULOGIC_VECTOR_VALUE), DataPres as VectorDataRepresentation).GetValueOrDefault(0));
        }

        private Double GetValueFromItarotor(IValueIterator iter, DataRepresentation DataPres)
        {
            return (double)(DataContainer.ValueDump.DataConvertorUtils.ToBigInteger((iter.CurrentValue.LastValue as STD_ULOGIC_VECTOR_VALUE), DataPres as VectorDataRepresentation).GetValueOrDefault(0));
        }
    }
}
