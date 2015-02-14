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
    public class My_Pen : ISerializable 
    {
        public My_ArrowCap cap1 { get; set; }
        public My_ArrowCap cap2 { get; set; }
        public Color Color { get; set; }
        public int Width { get; set; }
        public DashStyle DashStyle { get; set; }

        public My_Pen(SerializationInfo info, StreamingContext ctxt)//необходим для десериализации
        {
            Color = (Color)info.GetValue("Color", typeof(Color));
            Width = (int)info.GetValue("Width", typeof(int));
            DashStyle = (DashStyle)info.GetValue("DashStyle", typeof(DashStyle));

            try
            {
                cap1 = (My_ArrowCap)info.GetValue("cap1", typeof(My_ArrowCap));
                cap2 = (My_ArrowCap)info.GetValue("cap2", typeof(My_ArrowCap));
            }
            catch (ArgumentNullException ex)
            {
                cap1 = null;
                cap2 = null;
            }
            catch (InvalidCastException ex)
            {
                cap1 = null;
                cap2 = null;
            }
            catch (System.Runtime.Serialization.SerializationException ex)
            {
                cap1 = null;
                cap2 = null;
            }
        }

        public My_Pen(Color Color)
        {
            this.Color = Color;
            this.Width = 1;
            this.DashStyle = DashStyle.Solid;
        }

        public My_Pen(Color Color, int Width, DashStyle DashStyle)
        {
            this.Color = Color;
            this.Width = Width;
            this.DashStyle = DashStyle;
        }

        public My_Pen(Color Color, int Width, DashStyle DashStyle, My_ArrowCap cap1, My_ArrowCap cap2)
        {
            this.Color = Color;
            this.Width = Width;
            this.DashStyle = DashStyle;
            this.cap1 = cap1;
            this.cap2 = cap2;
        }

        public static implicit operator Pen(My_Pen value)
        {
            Pen pen = new Pen(value.Color, value.Width);
            pen.DashStyle = value.DashStyle;
            if(value.cap1 != null)
                pen.CustomStartCap = value.cap1;
            if (value.cap2 != null)
                pen.CustomEndCap = value.cap2;
            return pen;
        }

        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Color", Color);
            info.AddValue("Width", Width);
            info.AddValue("DashStyle", DashStyle);
            if(cap1 != null)
                info.AddValue("cap1", cap1);
            if (cap2 != null)
                info.AddValue("cap2", cap2);
        }

        #endregion
    }
}