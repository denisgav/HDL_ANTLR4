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
    public class My_TextureBrush : My_Brush, ISerializable 
    {
        public Image Image { get; set; }

        public My_TextureBrush(SerializationInfo info, StreamingContext ctxt)//необходим для десериализации
        {
            this.Image = (Image)info.GetValue("Image", typeof(Image));
            base.Brush = new TextureBrush(Image);
        }

        public My_TextureBrush(Image Image)
            : base(new TextureBrush(Image))
        {
            this.Image = Image;
        }

        public static implicit operator Brush(My_TextureBrush value)
        {
            return new TextureBrush(value.Image);
        }

        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Image", Image);
        }

        #endregion
    }
}