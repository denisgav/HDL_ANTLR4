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
    public class My_Brush
    {
        [NonSerialized]
        private Brush brush;

        public Brush Brush
        {
            get
            {
                return brush;
            }
            set
            {
                brush = value;
            }
        }

        public My_Brush()
        {
            this.Brush = Brushes.Black;
        }

        public My_Brush(Brush Brush)
        {
            this.Brush = Brush;
        }

        public static implicit operator Brush(My_Brush value)
        {
            return value.Brush;
        }

        public virtual void UpdateBrush(My_Figure fig)
        {}
    }
}