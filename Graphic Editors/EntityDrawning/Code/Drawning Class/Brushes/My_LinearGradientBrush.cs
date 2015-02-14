using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Schematix.EntityDrawning
{
    [Serializable]
    public class My_LinearGradientBrush : My_Brush, ISerializable 
    {
        public Color Color1 { get; set; }
        public Color Color2 { get; set; }
        public LinearGradientMode LinearGradientMode { get; set; }
        public Rectangle Rect { get; set; }

        public My_LinearGradientBrush(SerializationInfo info, StreamingContext ctxt)//необходим для десериализации
        {
            Color1 = (Color)info.GetValue("Color1", typeof(Color));
            Color2 = (Color)info.GetValue("Color2", typeof(Color));
            LinearGradientMode = (LinearGradientMode)info.GetValue("LinearGradientMode", typeof(LinearGradientMode));
            Rect = (Rectangle)info.GetValue("Rect", typeof(Rectangle));
            base.Brush = new LinearGradientBrush(Rect, Color1, Color2, LinearGradientMode);
        }

        public My_LinearGradientBrush(Color Color1, Color Color2, LinearGradientMode LinearGradientMode, Rectangle Rect)
            : base(new LinearGradientBrush(Rect, Color1, Color2, LinearGradientMode))
        {
            this.Color1 = Color1;
            this.Color2 = Color2;
            this.LinearGradientMode = LinearGradientMode;
            this.Rect = Rect;
        }

        public override void UpdateBrush(My_Figure fig)
        {
            Point[] points = fig.Points;
            Rectangle rect = new Rectangle(points[0], new Size(0, 0));
            foreach (Point p in points)
            {
                if (p.X < rect.X)
                    rect.X = p.X;
                if (p.Y < rect.Y)
                    rect.Y = p.Y;
                if (p.X > rect.Right)
                    rect.Width = p.X - rect.X;
                if (p.Y > rect.Bottom)
                    rect.Height = p.Y - rect.Y;
            }
            this.Rect = rect;
            Brush = new LinearGradientBrush(Rect, Color1, Color2, LinearGradientMode);
        }

        public static implicit operator Brush(My_LinearGradientBrush value)
        {
            return new LinearGradientBrush(value.Rect, value.Color1, value.Color2, value.LinearGradientMode);
        }

        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Color1", Color1);
            info.AddValue("Color2", Color2);
            info.AddValue("LinearGradientMode", LinearGradientMode);
            info.AddValue("Rect", Rect);
        }

        #endregion
    }
}