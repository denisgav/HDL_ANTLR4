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
    public partial class AddNewVHDL : Form
    {
        private SchematixCore core;
        private VHDL_EntityDrawning EntityDrawning;
        private List<VHDL_Port> PortList;
        public String sEntity, sArchitecture;

        public VHDL_Module VHDLModule { get; private set; }

        public AddNewVHDL()
            : this(SchematixCore.Core)
        { }

        public AddNewVHDL(SchematixCore core)
        {
            this.core = core;
            InitializeComponent();
            EntityDrawning = new VHDL_EntityDrawning(this.Font);
            PortList = new List<VHDL_Port>();
        }

        private void pictureBoxPreview_Paint(object sender, PaintEventArgs e)
        {
            EntityDrawning.EntityName = EntityName.Text;
            EntityDrawning.PortList = PortList;
            EntityDrawning.Draw(e.Graphics);
        }

        private void tbPortName_TextChanged(object sender, EventArgs e)
        {
            bool bEnabled = tbPortName.Text != "";
            int iSelIndex = lbPortList.SelectedIndex;
            if (iSelIndex >= 0)
            {
                VHDL_Port pi = PortList[iSelIndex];

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
                VHDL_Port pi = PortList[iSelIndex];
                pi.LeftIndex = (int)LeftIndex.Value;
                pi.RightIndex = (int)RightIndex.Value;
                PortList[iSelIndex] = pi;
                pictureBoxPreview.Invalidate();
            }
        }

        private void RadioButtonDirection_CheckedChanged(object sender, EventArgs e)
        {
            int iSelIndex = lbPortList.SelectedIndex;
            if (iSelIndex >= 0)
            {
                VHDL_Port pi = PortList[iSelIndex];
                if (rbIn.Checked) pi.Direction = VHDLPortDirection.In;
                if (rbOut.Checked) pi.Direction = VHDLPortDirection.Out;
                if (rbInOut.Checked) pi.Direction = VHDLPortDirection.InOut;
                if (rbBuffer.Checked) pi.Direction = VHDLPortDirection.Buffer;
                PortList[iSelIndex] = pi;
                pictureBoxPreview.Invalidate();
            }
        }

        private void cbPortType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iSelIndex = cbPortType.SelectedIndex;
            if (iSelIndex >= 0)
            {
                bool bEnable = (iSelIndex == 1) || (iSelIndex == 3); // BIT_VECTOR or STD_LOGIC_VECTOR
                LeftIndex.Enabled = bEnable;
                RightIndex.Enabled = bEnable;
            }
            comboBoxUserDefinitionType.Visible = (iSelIndex == 7);
            iSelIndex = lbPortList.SelectedIndex;
            if ((iSelIndex >= 0) && (iSelIndex != 7))
            {
                VHDL_Port pi = PortList[iSelIndex];
                pi.Type = (string)cbPortType.Items[cbPortType.SelectedIndex];   //SelectedText;
                PortList[iSelIndex] = pi;
                pictureBoxPreview.Invalidate();
            }
        }

        private void comboBoxUserDefinitionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string type = comboBoxUserDefinitionType.Text;
            if (string.IsNullOrEmpty(type))
                type = comboBoxUserDefinitionType.SelectedText;

            int iSelIndex = lbPortList.SelectedIndex;
            if (iSelIndex >= 0)
            {
                VHDL_Port pi = PortList[iSelIndex];
                pi.Type = type;   //SelectedText;
                PortList[iSelIndex] = pi;
                pictureBoxPreview.Invalidate();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            VHDLPortDirection inout = VHDLPortDirection.In;
            if (rbOut.Checked) inout = VHDLPortDirection.Out;
            if (rbInOut.Checked) inout = VHDLPortDirection.InOut;
            if (rbBuffer.Checked) inout = VHDLPortDirection.Buffer;

            VHDL_Port newPort = new VHDL_Port("", "STD_LOGIC", inout);
            tbPortName.Enabled = true;
            rbIn.Enabled = true;
            rbOut.Enabled = true;
            rbInOut.Enabled = true;
            rbBuffer.Enabled = true;
            cbPortType.Enabled = true;

            PortList.Add(newPort);
            lbPortList.Items.Add(newPort.Name);
            lbPortList.SelectedIndex = PortList.Count - 1;
            pictureBoxPreview.Invalidate();
            tbPortName.Text=string.Empty;
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

        private void lbPortList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iSelIndex = lbPortList.SelectedIndex;
            if (iSelIndex >= 0)
            {
                tbPortName.Text = PortList[iSelIndex].Name;
                LeftIndex.Value = PortList[iSelIndex].LeftIndex;
                RightIndex.Value = PortList[iSelIndex].RightIndex;

                string type = PortList[iSelIndex].Type;

                if (cbPortType.Items.Contains(type) == false)
                {
                    cbPortType.SelectedIndex = 7;
                    if (comboBoxUserDefinitionType.Items.Contains(type) == false)
                        comboBoxUserDefinitionType.Text = type;
                    else
                        comboBoxUserDefinitionType.SelectedItem = type;
                }
                else
                    cbPortType.SelectedIndex = cbPortType.Items.IndexOf(type);
                

                switch (PortList[iSelIndex].Direction)
                {
                    case VHDLPortDirection.In:
                        rbIn.Checked = true;
                        rbOut.Checked = false;
                        rbInOut.Checked = false;
                        rbBuffer.Checked = false;
                        break;
                    case VHDLPortDirection.Out:
                        rbIn.Checked = false;
                        rbOut.Checked = true;
                        rbInOut.Checked = false;
                        rbBuffer.Checked = false;
                        break;
                    case VHDLPortDirection.InOut:
                        rbIn.Checked = false;
                        rbOut.Checked = false;
                        rbInOut.Checked = true;
                        rbBuffer.Checked = false;
                        break;
                    case VHDLPortDirection.Buffer:
                        rbIn.Checked = false;
                        rbOut.Checked = false;
                        rbInOut.Checked = false;
                        rbBuffer.Checked = true;
                        break;
                }
                tbPortName.Focus();
            }
        }

        private void AddNewVHDLCode_Load(object sender, EventArgs e)
        {
            tabControl1.TabPages.Remove(tabPage2);
            ButtonOk.Enabled = false;
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FileName.Text) == false)
            {
                tabControl1.TabPages.Add(tabPage2);
                tabControl1.SelectedTab = tabPage2;
                ButtonOk.Enabled = true;
                buttonNext.Visible = false;
            }
        }

        private void FileName_TextChanged(object sender, EventArgs e)
        {
            ButtonOk.Enabled = !(string.IsNullOrEmpty(FileName.Text));
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            string sArch = ArchName.Text;
            string sEntity = EntityName.Text;
            if(string.IsNullOrEmpty(EntityName.Text) == true)
                sEntity = FileName.Text;
            if(string.IsNullOrEmpty(ArchName.Text) == true)
                sArch = FileName.Text;

            VHDLModule = new VHDL_Module();
            VHDLModule.ArchitectureName = sArch;
            VHDLModule.EntityName = sEntity;
            VHDLModule.PortList = PortList;

            DialogResult = System.Windows.Forms.DialogResult.OK;

            Close();
        }
    }
}
