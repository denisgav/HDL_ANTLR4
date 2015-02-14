using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Schematix.FSM
{
    public partial class PaperProperties : Form
    {
        private Schematix.FSM.Constructor_Core core;
        private Schematix.FSM.My_Graph graph;
        private Schematix.FSM.My_Paper paper;
        public PaperProperties(Schematix.FSM.Constructor_Core core, Schematix.FSM.My_Paper paper)
        {
            InitializeComponent();
            this.core = core;
            graph = core.Graph;
            this.paper = paper;
            LoadData();
        }

        private void LoadData()
        {
            colorDialogBG.Color = paper.BGColor;
            colorDialogLine.Color = paper.LineColor;
            Schematix.FSM.Constructor_Core.SetColorText(labelBGColor, paper.BGColor);
            Schematix.FSM.Constructor_Core.SetColorText(labelLineColor, paper.LineColor);
            checkBoxShowBorder.Checked = paper.DrawBorder;
            checkBoxShowGrid.Checked = paper.DrawGrig;

            if ((graph.Language == Schematix.FSM.FSM_Language.Verilog) && (graph.VerilogModule != null))
            {
                textBoxVerilogModuleName.Text = graph.VerilogModule.ModuleName;
                richTextBoxDesignUnitHeader.Text = graph.VerilogModule.Timescale;
                tabControl1.TabPages.Remove(tabPageVHDLModule);
            }

            if ((graph.Language == Schematix.FSM.FSM_Language.VHDL) && (graph.VHDLModule != null))
            {
                textBoxVHDLArchitectureName.Text = graph.VHDLModule.ArchitectureName;
                textBoxVHDLEntityName.Text = graph.VHDLModule.EntityName;
                tabControl1.TabPages.Remove(tabPageVerilogModule);
            }
        }

        private void buttonChangeBGColor_Click(object sender, EventArgs e)
        {
            DialogResult res = colorDialogBG.ShowDialog();
            if (res == DialogResult.OK)
            {
                Schematix.FSM.Constructor_Core.SetColorText(labelBGColor, colorDialogBG.Color);
            }
        }

        private void buttonChangeLineColor_Click(object sender, EventArgs e)
        {
            DialogResult res = colorDialogLine.ShowDialog();
            if (res == DialogResult.OK)
            {
                Schematix.FSM.Constructor_Core.SetColorText(labelLineColor, colorDialogLine.Color);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            paper.BGColor = colorDialogBG.Color;
            paper.LineColor = colorDialogLine.Color;
            paper.DrawBorder = checkBoxShowBorder.Checked;
            paper.DrawGrig = checkBoxShowGrid.Checked;
            paper.ChangeScroll();

            if ((graph.Language == Schematix.FSM.FSM_Language.Verilog) && (graph.VerilogModule != null))
            {
                graph.VerilogModule.ModuleName = textBoxVerilogModuleName.Text;
                graph.VerilogModule.Timescale = richTextBoxDesignUnitHeader.Text;
            }

            if ((graph.Language == Schematix.FSM.FSM_Language.VHDL) && (graph.VHDLModule != null))
            {
                graph.VHDLModule.ArchitectureName = textBoxVHDLArchitectureName.Text;
                graph.VHDLModule.EntityName = textBoxVHDLEntityName.Text;
            }

            core.Bitmap.UpdateBitmap();
            core.form.Invalidate();
            this.Close();
        }

        private void buttonDefaultDUH_Click(object sender, EventArgs e)
        {
            richTextBoxDesignUnitHeader.Text = FSM_OptionsHelper.Default_Design_Unit_Header_Verilog;
        }
    }
}
