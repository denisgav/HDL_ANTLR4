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
    public class My_PathGradientBrush : My_Brush, ISerializable 
    {
        private Point[] points;
        public Point[] Points
        {
            get
            {
                return points;
            }
            set
            {
                points = new Point[value.Length];
                for(int i=0; i<value.Length; i++)
                {
                    points[i].X = value[i].X;
                    points[i].Y = value[i].Y;
                }
            }
        }

        public Color Color1 { get; set; }
        public Color Color2 { get; set; }

        public WrapMode WrapMode { get; set; }

        public My_PathGradientBrush(SerializationInfo info, StreamingContext ctxt)//необходим для десериализации
        {
            Color1 = (Color)info.GetValue("Color1", typeof(Color));
            Color2 = (Color)info.GetValue("Color2", typeof(Color));
            WrapMode = (WrapMode)info.GetValue("WrapMode", typeof(WrapMode));
            Points = (Point[])info.GetValue("Points", typeof(Point[]));
            base.Brush = CreateBrush(WrapMode, Points, Color1, Color2);
        }

        public My_PathGradientBrush(WrapMode WrapMode, Point[] Points, Color Color1, Color Color2)
            : base(CreateBrush(WrapMode, Points, Color1, Color2))
        {
            this.WrapMode = WrapMode;
            this.Color1 = Color1;
            this.Color2 = Color2;
            this.Points = Points;
        }

        public static implicit operator Brush(My_PathGradientBrush value)
        {
            return CreateBrush(value.WrapMode, value.points, value.Color1, value.Color2);
        }

        public override void UpdateBrush(My_Figure fig)
        {
            Points = fig.Points;
            Brush = CreateBrush(WrapMode, points, Color1, Color2);
        }

        private static Brush CreateBrush(WrapMode WrapMode, Point[] Points, Color Color1, Color Color2)
        {
            var Brush = new PathGradientBrush(Points, WrapMode);
            Brush.CenterColor = Color1;
            Brush.SurroundColors = new Color[1] { Color2 };
            return Brush;
        }

        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Color1", Color1);
            info.AddValue("Color2", Color2);
            info.AddValue("Points", Points);
            info.AddValue("WrapMode", WrapMode);
        }

        #endregion
    }
}