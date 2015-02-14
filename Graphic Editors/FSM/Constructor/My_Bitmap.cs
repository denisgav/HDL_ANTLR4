using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Schematix.FSM
{ 
    public class My_Bitmap
    {
        private Bitmap Virtual_bitmap;
        private Schematix.FSM.Constructor_Core core;
        public Color SelectedColor;
        public bool ArrowPaint = true;
        public Bitmap bitmap
        {
            get
            {
                return Virtual_bitmap;
            }
        }

        public My_Bitmap(Schematix.FSM.Constructor_Core core)
        {
            this.core = core;
        }

        public virtual void UpdateBitmap()
        {
            Rectangle rect = new Rectangle(core.form.Location, core.form.Size);
            if (Virtual_bitmap != null)
                Virtual_bitmap.Dispose();
            Size max_size = core.Graph.MaxSize;
            Virtual_bitmap = new Bitmap(max_size.Width, max_size.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(Virtual_bitmap);
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            /*Рисуем Битмап*/
            foreach (Schematix.FSM.My_Figure fig in core.Graph.Figures)
            {
                fig.Draw_Bitmap(this, new PaintEventArgs(graphics, rect));
            }            
            if (core.Graph.Reset != null)
            {
                core.Graph.Reset.Draw_Bitmap(this, new PaintEventArgs(graphics, rect));
            }
        }

        public Schematix.FSM.My_Figure SelectElem(Point pt)
        {
            if (Virtual_bitmap == null)
                return null;
            if ((pt.X >= Virtual_bitmap.Width) || (pt.Y >= Virtual_bitmap.Height) || (pt.X<=0) || (pt.Y<=0))
                return null;
            /*находим выделенный елемент*/
            SelectedColor = Virtual_bitmap.GetPixel(pt.X, pt.Y);
            foreach (Schematix.FSM.My_Line line in core.Graph.Lines)
            {
                bool res = line.IsSelected(SelectedColor);
                if (res == true)
                {
                    return line;
                }
                res = line.label_condition.IsSelected(SelectedColor);
                if (res == true)
                {
                    return line.label_condition;
                }
            }
            foreach (Schematix.FSM.My_State state in core.Graph.States)
            {
                bool res = state.IsSelected(SelectedColor);
                if (res == true)
                {
                    return state;
                }
                res = state.label_name.IsSelected(SelectedColor);
                if (res == true)
                {
                    return state.label_name;
                }
            }
            foreach (Schematix.FSM.My_Comment comment in core.Graph.Comments)
            {
                bool res = comment.IsSelected(SelectedColor);
                if (res == true)
                {
                    return comment;
                }
            }
            foreach (Schematix.FSM.My_Signal signal in core.Graph.Signals)
            {
                bool res = signal.IsSelected(SelectedColor);
                if (res == true)
                {
                    return signal;
                }
                res = signal.label_name.IsSelected(SelectedColor);
                if (res == true)
                {
                    return signal.label_name;
                }
            }
            foreach (Schematix.FSM.My_Constant c in core.Graph.Constants)
            {
                bool res = c.IsSelected(SelectedColor);
                if (res == true)
                {
                    return c;
                }
                res = c.label_name.IsSelected(SelectedColor);
                if (res == true)
                {
                    return c.label_name;
                }
            }
            foreach (Schematix.FSM.My_Port p in core.Graph.Ports)
            {
                bool res = p.IsSelected(SelectedColor);
                if (res == true)
                {
                    return p;
                }
                res = p.label_name.IsSelected(SelectedColor);
                if (res == true)
                {
                    return p.label_name;
                }
            }
            if (core.Graph.Reset != null)
            {
                bool res = core.Graph.Reset.IsSelected(SelectedColor);
                if (res == true)
                {
                    return core.Graph.Reset;
                }
            }
            return null;
        }

        public Point GetCommonPoint(Schematix.FSM.My_State state, Schematix.FSM.My_Line line, bool angle /*true - angle1 else - angle2*/)
        {
            Point p = state.CenterPoint;
            Point line_angle;
            if (angle == true)
            {
                line_angle = line.Angle1;
            }
            else
            {
                line_angle = line.Angle2;
            }
            if (ArrowPaint == true)
            {
                Rectangle rect = state.rect;
                //Найдем уравнение прямой 
                //y = k*x+b
                double K = (double)(state.CenterPoint.Y - line_angle.Y) / (double)(state.CenterPoint.X - line_angle.X);
                //данные для еллипса
                double a = (double)(rect.Width / 2);
                if (a < 10)
                    return state.CenterPoint;
                double b = (double)(rect.Height / 2);
                //x^2/a^2 + y^2/b^2 = 1;
                if (line_angle.Y > state.CenterPoint.Y)
                {
                    double x = (b * b) / (K * K + ((b * b) / (a * a)));
                    x = Math.Sqrt(x);
                    if (line_angle.X < state.CenterPoint.X)
                    {
                        x = -x;
                    }
                    double y = (b / a) * Math.Sqrt(a * a - x * x);
                    y += state.CenterPoint.Y;
                    x += state.CenterPoint.X;

                    p.X = (int)x;
                    p.Y = (int)y;
                    return p;
                }
                else
                {
                    double x = (b * b) / (K * K + ((b * b) / (a * a)));
                    x = Math.Sqrt(x);
                    if (line_angle.X < state.CenterPoint.X)
                    {
                        x = -x;
                    }
                    double y = -(b / a) * Math.Sqrt(a * a - x * x);
                    y += state.CenterPoint.Y;
                    x += state.CenterPoint.X;

                    p.X = (int)x;
                    p.Y = (int)y;
                    return p;
                }
            }
            return p;
        }
    }
}