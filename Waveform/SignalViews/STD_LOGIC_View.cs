using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using Schematix.Waveform.UserControls;
using DataContainer;
using DataContainer.SignalDump;

namespace Schematix.Waveform.SignalViews
{
    /// <summary>
    /// Класс, используемый для отображения данных STD_LOGIC
    /// </summary>
    public class STD_LOGIC_View : SimpleSignalViewBase
    {
        public STD_LOGIC_View(SimpleSignalDump data, ScaleManager scaleManager)
            : base(data, scaleManager)
        {
            prevValue = 0;
            drawVerticalLine = false;
        }

        private double prevValue;
        private bool drawVerticalLine;

        public override void Reset()
        {
            prevValue = 0;
            drawVerticalLine = false;
        }

        public override void DrawValue(Canvas canvas, UInt64 start, UInt64 end, ScaleManager manager, string value)
        {
            double d_start = scaleManager.GetOffset(start);
            double d_end = scaleManager.GetOffset(end);

            if (start == end)
                return;

            switch (value)
            {
                case "1":
                    if(drawVerticalLine == true)
                        AddVerticalLine(canvas, d_start, prevValue, 2);
                    AddLine(canvas, d_start, d_end, prevValue = 2, Brushes.Black, 1);
                    drawVerticalLine = true;
                    break;
                case "0":
                    if (drawVerticalLine == true)
                        AddVerticalLine(canvas, d_start, prevValue, canvas.ActualHeight - 2);
                    AddLine(canvas, d_start, d_end, prevValue = canvas.ActualHeight - 2, Brushes.Black, 1);
                    drawVerticalLine = true;
                    break;
                case "U":
                    AddRectangle(canvas, d_start, d_end, Brushes.Red, Brushes.Transparent, 1, "U");
                    drawVerticalLine = false;
                    break;
                case "X":
                    AddRectangle(canvas, d_start, d_end, Brushes.Red, Brushes.Transparent, 1, "X");
                    drawVerticalLine = false;
                    break;
                case "Z":
                    if (drawVerticalLine == true)
                        AddVerticalLine(canvas, d_start, prevValue, (canvas.ActualHeight - 2) / 2);
                    AddLine(canvas, d_start, d_end, prevValue =  (canvas.ActualHeight - 2) / 2, Brushes.LightGray, 1);
                    drawVerticalLine = true;
                    break;
                case "L":
                    if (drawVerticalLine == true)
                        AddVerticalLine(canvas, d_start, prevValue, canvas.ActualHeight - 2);
                    AddLine(canvas, d_start, d_end, prevValue = canvas.ActualHeight - 2, Brushes.Purple, 1);
                    drawVerticalLine = true;
                    break;
                case "H":
                    if (drawVerticalLine == true)
                        AddVerticalLine(canvas, d_start, prevValue, 2);
                    AddLine(canvas, d_start, d_end, prevValue = 2, Brushes.Pink, 1);
                    drawVerticalLine = true;
                    break;
                case "W":
                    if (drawVerticalLine == true)
                        AddVerticalLine(canvas, d_start, prevValue, (canvas.ActualHeight - 2) / 2);
                    AddLine(canvas, d_start, d_end, prevValue = (canvas.ActualHeight - 2) / 2, Brushes.Green, 1);
                    drawVerticalLine = true;
                    break;
            }
        }
    }
}
