using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Schematix.EntityDrawning
{
    public partial class EntityDrawningForm : UserControl
    {
        public readonly EntityDrawningCore core;
        public EntityDrawningForm()
        {
            InitializeComponent();
            core = new EntityDrawningCore(this);
            this.MouseDown += core.MouseDown;
            this.MouseMove += core.MouseMove;
            this.MouseUp += core.MouseUp;
            this.Paint += core.Draw;
            this.KeyDown += core.KeyDown;
            this.KeyUp += core.KeyUp;
        }

        private void brushToolStripMenuItem_Click(object sender, EventArgs e)
        {
            core.ShowBrushProperties();
        }

        private void penToolStripMenuItem_Click(object sender, EventArgs e)
        {
            core.ShowPenProperties();
        }

        private void brushToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            core.ShowBrushProperties();
        }

        private void penToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            core.ShowPenProperties();
        }

        private void textProperiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            core.ShowTextproperties();
        }

        private void penToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            core.ShowPenProperties();
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            core.ShowPaperProperties();
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            Invalidate();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            Invalidate();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            core.ShowPenProperties();
        }

        private void propertiesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            core.ShowPortProperties();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            core.DeleteFigure();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            core.DeleteFigure();
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            core.DeleteFigure();
        }

        private void toolStripMenuItem13_Click(object sender, EventArgs e)
        {
            core.DeleteFigure();
        }

        private void toolStripMenuItem17_Click(object sender, EventArgs e)
        {
            core.DeleteFigure();
        }

        private void toolStripMenuItem14_Click(object sender, EventArgs e)
        {
            core.CutToClipboard();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            core.CutToClipboard();
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            core.CutToClipboard();
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            core.CutToClipboard();
        }

        private void cutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            core.CutToClipboard();
        }

        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            core.CopyToClipboard();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            core.CopyToClipboard();
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            core.CopyToClipboard();
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            core.CopyToClipboard();
        }

        private void toolStripMenuItem15_Click(object sender, EventArgs e)
        {
            core.CopyToClipboard();
        }

        private void pasteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            core.GetFromClipboard();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            core.GetFromClipboard();
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            core.GetFromClipboard();
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            core.GetFromClipboard();
        }

        private void toolStripMenuItem16_Click(object sender, EventArgs e)
        {
            core.GetFromClipboard();
        }

        private void EntityDrawningForm_SizeChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void windingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (core.SelectedFigure as My_Path).fillMode = System.Drawing.Drawing2D.FillMode.Winding;
            //core.AddToHistory();
            Invalidate();
        }

        private void alternateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (core.SelectedFigure as My_Path).fillMode = System.Drawing.Drawing2D.FillMode.Alternate;
            //core.AddToHistory();
            Invalidate();
        }
    }
}
