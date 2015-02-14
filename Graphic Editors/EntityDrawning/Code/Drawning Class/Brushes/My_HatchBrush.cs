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
    public class My_HatchBrush : My_Brush, ISerializable 
    {
        public Color Color1 { get; set; }
        public Color Color2 { get; set; }
        public HatchStyle HatchStyle { get; set; }

        public My_HatchBrush(Color Color1, Color Color2, HatchStyle HatchStyle)
            : base(new HatchBrush(HatchStyle, Color1, Color2))
        {
            this.Color1 = Color1;
            this.Color2 = Color2;
            this.HatchStyle = HatchStyle;
        }

        public My_HatchBrush(SerializationInfo info, StreamingContext ctxt)//необходим для десериализации
        {
            Color1 = (Color)info.GetValue("Color1", typeof(Color));
            Color2 = (Color)info.GetValue("Color2", typeof(Color));
            HatchStyle = (HatchStyle)info.GetValue("HatchStyle", typeof(HatchStyle));
            base.Brush = new HatchBrush(HatchStyle, Color1, Color2);
        }

        public static implicit operator Brush(My_HatchBrush value)
        {
            return new HatchBrush(value.HatchStyle, value.Color1, value.Color2);
        }

        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Color1", Color1);
            info.AddValue("Color2", Color2);
            info.AddValue("HatchStyle", HatchStyle);
        }

        #endregion
    }
}