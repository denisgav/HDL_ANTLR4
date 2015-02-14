using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Schematix.FSM
{
    [Serializable]
    public class My_Constant : My_Signal
    {
        public enum GenerationType
        {
            Constant,
            Generic
        };

        public GenerationType Gen_Type { get; set; }

        public My_Constant(Schematix.FSM.Constructor_Core core):
            base (core)
        {
            Gen_Type = GenerationType.Generic;
            Color = Color.Green;

            base.name = ("Constant" + core.Graph.Constants.Count.ToString());
            base.label_name.Text = name + " = " + Default_Value;
        }

        public My_Constant(My_Constant item) :
            base(item as My_Signal)
        {
            this.Gen_Type = item.Gen_Type;
        }

        public My_Constant(string name, string Type, string Default_Value, Point center_point, GenerationType Gen_Type, Schematix.FSM.Constructor_Core core) :
            base(name, Type, Default_Value, center_point, core)
        {
            this.Gen_Type = Gen_Type;
            Color = Color.Green;
        }

        public override void Draw(object sender, PaintEventArgs e)
        {
            base.Draw(sender, e);

            System.Drawing.StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            if(Gen_Type == GenerationType.Constant)
                e.Graphics.DrawString("C", new Font("Times New Roman", 10, FontStyle.Bold), new SolidBrush(Color.Black), rect, format);
            else
                e.Graphics.DrawString("g", new Font("Times New Roman", 10, FontStyle.Bold), new SolidBrush(Color.Black), rect, format);
        }
    }
}