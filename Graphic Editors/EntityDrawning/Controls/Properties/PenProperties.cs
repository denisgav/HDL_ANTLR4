using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Schematix.EntityDrawning
{
    public partial class PenProperties : Form
    {
        public My_Pen pen;
        private My_Figure figure;
        private EntityDrawningCore core;
        public PenProperties(My_Figure figure, EntityDrawningCore core)
        {
            InitializeComponent();
            this.pen = figure.pen;
            this.figure = figure;
            this.core = core;
            LoadData();
        }

        private void LoadData()
        {
            colorDialog1.Color = pen.Color;
            numericUpDownWidth.Value = (decimal)pen.Width;

            comboBoxDashStyle.Items.Add(System.Drawing.Drawing2D.DashStyle.Custom);
            comboBoxDashStyle.Items.Add(System.Drawing.Drawing2D.DashStyle.Dash);
            comboBoxDashStyle.Items.Add(System.Drawing.Drawing2D.DashStyle.DashDot);
            comboBoxDashStyle.Items.Add(System.Drawing.Drawing2D.DashStyle.DashDotDot);
            comboBoxDashStyle.Items.Add(System.Drawing.Drawing2D.DashStyle.Dot);
            comboBoxDashStyle.Items.Add(System.Drawing.Drawing2D.DashStyle.Solid);
            comboBoxDashStyle.SelectedItem = pen.DashStyle;

            comboBoxEndBaseCap.Items.Add(LineCap.AnchorMask);
            comboBoxEndBaseCap.Items.Add(LineCap.ArrowAnchor);
            comboBoxEndBaseCap.Items.Add(LineCap.Custom);
            comboBoxEndBaseCap.Items.Add(LineCap.DiamondAnchor);
            comboBoxEndBaseCap.Items.Add(LineCap.Flat);
            comboBoxEndBaseCap.Items.Add(LineCap.NoAnchor);
            comboBoxEndBaseCap.Items.Add(LineCap.Round);
            comboBoxEndBaseCap.Items.Add(LineCap.RoundAnchor);
            comboBoxEndBaseCap.Items.Add(LineCap.Square);
            comboBoxEndBaseCap.Items.Add(LineCap.SquareAnchor);
            comboBoxEndBaseCap.Items.Add(LineCap.Triangle);
 
            comboBoxStartBaseCap.Items.Add(LineCap.AnchorMask);
            comboBoxStartBaseCap.Items.Add(LineCap.ArrowAnchor);
            comboBoxStartBaseCap.Items.Add(LineCap.Custom);
            comboBoxStartBaseCap.Items.Add(LineCap.DiamondAnchor);
            comboBoxStartBaseCap.Items.Add(LineCap.Flat);
            comboBoxStartBaseCap.Items.Add(LineCap.NoAnchor);
            comboBoxStartBaseCap.Items.Add(LineCap.Round);
            comboBoxStartBaseCap.Items.Add(LineCap.RoundAnchor);
            comboBoxStartBaseCap.Items.Add(LineCap.Square);
            comboBoxStartBaseCap.Items.Add(LineCap.SquareAnchor);
            comboBoxStartBaseCap.Items.Add(LineCap.Triangle);

            if (pen.cap1 != null)
            {
                comboBoxStartBaseCap.SelectedItem = pen.cap1.l_cap;
                numericUpDownSrartCapWidth.Value = pen.cap1.Width;
                numericUpDownStartCapHeight.Value = pen.cap1.Height;
                checkBoxEnableStartCap.Checked = true;
            }

            if (pen.cap2 != null)
            {
                comboBoxEndBaseCap.SelectedItem = pen.cap2.l_cap;
                numericUpDownEndCapWidth.Value = pen.cap2.Width;
                numericUpDownEndCapHeight.Value = pen.cap2.Height;
                checkBoxEnableEndCap.Checked = true;
            }
        }

        private void pictureBoxPreview_Paint(object sender, PaintEventArgs e)
        {
            Graphics dc = e.Graphics;
                Point p1 = new Point(pictureBoxPreview.ClientRectangle.Left + 10, pictureBoxPreview.ClientRectangle.Top + 10);
                Point p2 = new Point(pictureBoxPreview.ClientRectangle.Right - 10, pictureBoxPreview.ClientRectangle.Bottom - 10);
                dc.DrawLine(pen, p1, p2);
        }

        private void buttonChangeColor_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            Color color = Color.FromArgb((byte)numericUpDownAlpha.Value, colorDialog1.Color);
            int width = (int)numericUpDownWidth.Value;

            pen = new My_Pen(color, width, (DashStyle)comboBoxDashStyle.SelectedItem);

            if (checkBoxEnableEndCap.Checked == true)
            {
                int e_width = (int)numericUpDownEndCapWidth.Value;
                int e_height = (int)numericUpDownEndCapHeight.Value;
                LineCap l_cap = (LineCap)comboBoxEndBaseCap.SelectedItem;

                My_ArrowCap cap = new My_ArrowCap(l_cap, e_width, e_height);
                pen.cap1 = cap;
            }

            if (checkBoxEnableStartCap.Checked == true)
            {
                int s_width = (int)numericUpDownSrartCapWidth.Value;
                int s_height = (int)numericUpDownStartCapHeight.Value;
                LineCap l_cap = (LineCap)comboBoxStartBaseCap.SelectedItem;

                My_ArrowCap cap = new My_ArrowCap(l_cap, s_width, s_height);
                pen.cap2 = cap;

            }
            pictureBoxPreview.Invalidate();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            buttonPreview_Click(sender, e);
            figure.pen = pen;
            core.Form.Invalidate();
            this.Close();
        }
    }
}
