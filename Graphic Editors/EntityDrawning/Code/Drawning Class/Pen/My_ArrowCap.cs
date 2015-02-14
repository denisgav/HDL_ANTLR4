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
    public class My_ArrowCap : ISerializable
    {
        public LineCap l_cap { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public My_ArrowCap(SerializationInfo info, StreamingContext ctxt)//необходим для десериализации
        {
            l_cap = (LineCap)info.GetValue("l_cap", typeof(LineCap));
            Width = (int)info.GetValue("Width", typeof(int));
            Height = (int)info.GetValue("Height", typeof(int));
        }

        public My_ArrowCap(LineCap l_cap, int Width, int Height)
        {
            this.l_cap = l_cap;
            this.Width = Width;
            this.Height = Height;
        }

        public static implicit operator AdjustableArrowCap(My_ArrowCap value)
        {
            AdjustableArrowCap cap = new AdjustableArrowCap(value.Width, value.Height);
            cap.BaseCap = value.l_cap;
            return cap;
        }

        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("l_cap", l_cap);
            info.AddValue("Width", Width);
            info.AddValue("Height", Height);
        }

        #endregion
    }
}