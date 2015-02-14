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
    public partial class PaperProperties : Form
    {
        private EntityDrawningCore core;
        private My_Paper paper;
        public PaperProperties(EntityDrawningCore core, My_Paper paper)
        {
            InitializeComponent();
            this.core = core;
            this.paper = paper;
            colorDialogBG.Color = paper.BGColor;
            colorDialogLine.Color = paper.LineColor;
            colorDialogSelectColor.Color = EntityDrawningCore.SelectedColor;
            labelBGColor.Text = paper.BGColor.ToKnownColor().ToString();
            labelLineColor.Text = paper.LineColor.ToKnownColor().ToString();
            labelSelectColor.Text = EntityDrawningCore.SelectedColor.ToKnownColor().ToString();
            checkBoxShowBorder.Checked = paper.DrawBorder;
            checkBoxShowGrid.Checked = paper.DrawGrig;
        }

        private void buttonChangeBGColor_Click(object sender, EventArgs e)
        {
            colorDialogBG.ShowDialog();
        }

        private void buttonChangeLineColor_Click(object sender, EventArgs e)
        {
            colorDialogLine.ShowDialog();
        }

        private void buttonSelectColor_Click(object sender, EventArgs e)
        {
            colorDialogSelectColor.ShowDialog();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            paper.BGColor = colorDialogBG.Color;
            paper.LineColor = colorDialogLine.Color;
            EntityDrawningCore.SelectedColor = colorDialogSelectColor.Color;
            paper.DrawBorder = checkBoxShowBorder.Checked;
            paper.DrawGrig = checkBoxShowGrid.Checked;
            core.Form.Invalidate();
            this.Close();
        }
    }
}
