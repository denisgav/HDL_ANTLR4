using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Schematix.EntityDrawning
{
    public partial class TextProperties : Form
    {
        private My_Text figure;
        private EntityDrawningCore core;
        public TextProperties(My_Text figure, EntityDrawningCore core)
        {
            InitializeComponent();
            this.figure = figure;
            this.core = core;
            LoadData();
        }

        private void LoadData()
        {
            richTextBoxText.Text = figure.Text;
            richTextBoxText.Font = figure.Font;
            fontDialog1.Font = figure.Font;
        }

        private void buttonChangePen_Click(object sender, EventArgs e)
        {
            PenProperties pen_prop = new PenProperties(figure, core);
            pen_prop.ShowDialog();
        }

        private void buttonChangeBrush_Click(object sender, EventArgs e)
        {
            BrushProperies brush_prop = new BrushProperies(figure, core);
            brush_prop.ShowDialog();
        }

        private void buttonChangeFont_Click(object sender, EventArgs e)
        {
            DialogResult res = fontDialog1.ShowDialog();
            if (res == DialogResult.OK)
                richTextBoxText.Font = fontDialog1.Font;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            figure.Text = richTextBoxText.Text;
            figure.Font = fontDialog1.Font;
            Close();
        }
    }
}
