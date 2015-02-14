using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Schematix.EntityDrawning
{
    [Serializable]
    public class My_SolidBrush : My_Brush, ISerializable 
    {
        public Color Color { get; set; }

        public My_SolidBrush(SerializationInfo info, StreamingContext ctxt)//необходим для десериализации
        {
            this.Color = (Color)info.GetValue("Color", typeof(Color));
            base.Brush = new SolidBrush(Color);
        }

        public My_SolidBrush(Color Color)
            :base(new SolidBrush(Color))
        {
            this.Color = Color;
        }

        public static implicit operator Brush(My_SolidBrush value)
        {
            return new SolidBrush(value.Color);
        }

        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Color", Color);
        }

        #endregion
    }
}