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
    public partial class PortProperties : Form
    {
        private EntityDrawningCore core;
        private My_Port port;

        public PortProperties(EntityDrawningCore core, My_Port port)
        {
            InitializeComponent();
            this.core = core;
            this.port = port;
            LoadData();
        }

        private void LoadData()
        {
            textBoxName.Text = port.Name;

            comboBoxType.Items.Add(My_Port.PortType.Simple);
            comboBoxType.Items.Add(My_Port.PortType.Asynchronous);
            comboBoxType.Items.Add(My_Port.PortType.Simultaneous);
            comboBoxType.SelectedItem = port.type;

            checkBoxInverse.Checked = port.Inverse;

            if (port.vhdPort.bus == true)
            {
                checkBoxIsBus.Checked = true;
                maskedTextBoxLeftBound.Text = port.vhdPort.leftBound.ToString();
                maskedTextBoxRightBound.Text = port.vhdPort.rightBound.ToString();
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            port.Name = textBoxName.Text;
            port.type = (My_Port.PortType)comboBoxType.SelectedItem;
            port.Inverse = checkBoxInverse.Checked;

            if (checkBoxIsBus.Checked == true)
            {
                port.vhdPort.bus = true;
                port.vhdPort.leftBound = int.Parse(maskedTextBoxLeftBound.Text);
                port.vhdPort.rightBound = int.Parse(maskedTextBoxRightBound.Text);
            }
            else
            {
                port.vhdPort.bus = false;
            }

            core.Form.Invalidate();
            Close();
        }

        private void buttonChangePen_Click(object sender, EventArgs e)
        {
            PenProperties pen_prop = new PenProperties(port, core);
            pen_prop.ShowDialog();
        }
    }
}
