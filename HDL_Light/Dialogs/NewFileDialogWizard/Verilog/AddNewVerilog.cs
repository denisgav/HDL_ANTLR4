using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Schematix.Core;

namespace Schematix.Dialogs.NewFileDialogWizard
{
    public partial class AddNewVerilog : Form
    {
        private SchematixCore core;
        private Verilog_EntityDrawning EntityDrawning;
        private List<Verilog_Port> PortList;
        public String sModule;

        public Verilog_Module VerilogModule { get; private set; }

        public AddNewVerilog()
            :this(SchematixCore.Core)
        { }

        public AddNewVerilog(SchematixCore core)
        {
            this.core = core;
            InitializeComponent();
            EntityDrawning = new Verilog_EntityDrawning(this.Font);
            PortList = new List<Verilog_Port>();
        }

        private void AddNewVerilogCode_Load(object sender, EventArgs e)
        {
            comboBoxLeftTimeScaleIndex.SelectedItem = "1 ns";
            comboBoxRightTimeScaleIndex.SelectedItem = "1 ps";
            cbPortType.SelectedItem = "wire";

            tabControl1.TabPages.Remove(tabPage2);
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FileName.Text) == false)
            {
                ButtonOk.Enabled = true;
                tabControl1.TabPages.Add(tabPage2);
                tabControl1.SelectedTab = tabPage2;
                buttonNext.Visible = false;
            }
        }

        private void FileName_TextChanged(object sender, EventArgs e)
        {
            ButtonOk.Enabled = !(string.IsNullOrEmpty(FileName.Text));
        }

        private void tbPortName_TextChanged(object sender, EventArgs e)
        {
            bool bEnabled = tbPortName.Text != "";
            int iSelIndex = lbPortList.SelectedIndex;
            if (iSelIndex >= 0)
            {
                Verilog_Port pi = PortList[iSelIndex];

                pi.Name = tbPortName.Text;
                PortList[iSelIndex] = pi;

                lbPortList.Items[iSelIndex] = tbPortName.Text;
                pictureBoxPreview.Invalidate();
                if (bEnabled)
                    for (int i = 0; i < PortList.Count; i++)
                        if ((PortList[i].Name == tbPortName.Text) &&
                            (i != iSelIndex))
                            bEnabled = false;
                btnAdd.Enabled = bEnabled;
                lbPortList.Enabled = bEnabled;
                cbPortType.Enabled = bEnabled;
            }
            if (string.IsNullOrEmpty(tbPortName.Text))
                ButtonOk.Enabled = false;
            else
                ButtonOk.Enabled = true;
        }

        private void BoundsChanged(object sender, EventArgs e)
        {
            int iSelIndex = lbPortList.SelectedIndex;
            if (iSelIndex >= 0)
            {
                Verilog_Port pi = PortList[iSelIndex];
                pi.LeftIndex = (int)LeftIndex.Value;
                pi.RightIndex = (int)RightIndex.Value;
                PortList[iSelIndex] = pi;
                pictureBoxPreview.Invalidate();
            }
        }

        private void pictureBoxPreview_Paint(object sender, PaintEventArgs e)
        {
            EntityDrawning.ModuleName = ModuleName.Text;
            EntityDrawning.PortList = PortList;
            EntityDrawning.Draw(e.Graphics);
        }

        private void RadioButtonDirection_CheckedChanged(object sender, EventArgs e)
        {
            int iSelIndex = lbPortList.SelectedIndex;
            if (iSelIndex >= 0)
            {
                Verilog_Port pi = PortList[iSelIndex];
                if (rbIn.Checked) pi.Direction = VerilogPortDirection.In;
                if (rbOut.Checked) pi.Direction = VerilogPortDirection.Out;
                if (rbInOut.Checked) pi.Direction = VerilogPortDirection.InOut;
                PortList[iSelIndex] = pi;
                pictureBoxPreview.Invalidate();
            }
        }

        private void lbPortList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iSelIndex = lbPortList.SelectedIndex;
            if (iSelIndex >= 0)
            {
                tbPortName.Text = PortList[iSelIndex].Name;
                LeftIndex.Value = PortList[iSelIndex].LeftIndex;
                RightIndex.Value = PortList[iSelIndex].RightIndex;
                cbPortType.SelectedItem = PortList[iSelIndex].Type;
                checkBoxIsBus.Checked = PortList[iSelIndex].isBus;

                switch (PortList[iSelIndex].Direction)
                {
                    case VerilogPortDirection.In:
                        rbIn.Checked = true;
                        rbOut.Checked = false;
                        rbInOut.Checked = false;
                        break;
                    case VerilogPortDirection.Out:
                        rbIn.Checked = false;
                        rbOut.Checked = true;
                        rbInOut.Checked = false;
                        break;
                    case VerilogPortDirection.InOut:
                        rbIn.Checked = false;
                        rbOut.Checked = false;
                        rbInOut.Checked = true;
                        break;
                }
                tbPortName.Focus();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            VerilogPortDirection inout = VerilogPortDirection.In;
            if (rbOut.Checked) inout = VerilogPortDirection.Out;
            if (rbInOut.Checked) inout = VerilogPortDirection.InOut;

            Verilog_Port newPort = new Verilog_Port("", "wire", inout);
            tbPortName.Enabled = true;
            rbIn.Enabled = true;
            rbOut.Enabled = true;
            rbInOut.Enabled = true;
            cbPortType.Enabled = true;

            PortList.Add(newPort);
            lbPortList.Items.Add(newPort.Name);
            lbPortList.SelectedIndex = PortList.Count - 1;
            pictureBoxPreview.Invalidate();
            tbPortName.Text = string.Empty;
            tbPortName.Focus();
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            int iSelIndex = lbPortList.SelectedIndex;
            if (iSelIndex >= 0)
            {
                PortList.RemoveAt(iSelIndex);
                lbPortList.Items.RemoveAt(iSelIndex);
                if (PortList.Count > 0)
                {
                    iSelIndex = (iSelIndex > 0) ? iSelIndex - 1 : iSelIndex;
                    lbPortList.SelectedIndex = iSelIndex;
                }
                else
                {
                    tbPortName.Text = "";
                    tbPortName.Enabled = false;
                    LeftIndex.Enabled = RightIndex.Enabled = false;
                    btnAdd.Enabled = true;
                }
                pictureBoxPreview.Invalidate();
            }
        }

        private void cbPortType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iSelIndex = lbPortList.SelectedIndex;
            if ((iSelIndex >= 0))
            {
                Verilog_Port pi = PortList[iSelIndex];
                pi.Type = (string)cbPortType.Items[cbPortType.SelectedIndex];   //SelectedText;
                PortList[iSelIndex] = pi;
                pictureBoxPreview.Invalidate();
            }
        }

        private void checkBoxIsBus_CheckedChanged(object sender, EventArgs e)
        {
            int iSelIndex = lbPortList.SelectedIndex;
            if (iSelIndex >= 0)
            {
                Verilog_Port pi = PortList[iSelIndex];
                pi.isBus = checkBoxIsBus.Checked;
                PortList[iSelIndex] = pi;
                pictureBoxPreview.Invalidate();

                LeftIndex.Enabled = checkBoxIsBus.Checked;
                RightIndex.Enabled = checkBoxIsBus.Checked;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            string sModuleName = ModuleName.Text;
            string timescale = string.Format(" {0} / {1}", (string)comboBoxLeftTimeScaleIndex.SelectedItem, (string)comboBoxRightTimeScaleIndex.SelectedItem);
            if (string.IsNullOrEmpty(ModuleName.Text) == true)
                sModuleName = FileName.Text;

            VerilogModule = new Verilog_Module();
            VerilogModule.ModuleName = sModuleName;
            VerilogModule.Timescale = timescale;
            VerilogModule.PortList = PortList;

            DialogResult = System.Windows.Forms.DialogResult.OK;

            Close();
        }
    }
}
