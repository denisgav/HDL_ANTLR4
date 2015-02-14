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
    public partial class Reset_Properties : Form
    {
        private Schematix.FSM.My_Reset reset;
        private Schematix.FSM.Constructor_Core core;

        public Reset_Properties(Schematix.FSM.My_Reset reset, Schematix.FSM.Constructor_Core core)
        {
            this.core = core;
            this.reset = reset;
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            richTextBoxCondition.Text = reset.condition;
            foreach (Schematix.FSM.My_Signal signal in core.Graph.Signals)
            {
                comboBoxTrackingSignal.Items.Add(signal.name);
            }
            foreach (Schematix.FSM.My_Port port in core.Graph.Ports)
            {
                comboBoxTrackingSignal.Items.Add(port.name);
            }
            comboBoxTrackingSignal.SelectedItem = reset.signal;
            if (reset.res_type == Schematix.FSM.My_Reset.Reset_Type.Asynchonous)
                radioButtonAsynchonous.Checked = true;
            else
                radioButtonSynchonous.Checked = true;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            reset.condition = richTextBoxCondition.Text;
            reset.signal = (string)comboBoxTrackingSignal.SelectedItem;
            if (radioButtonSynchonous.Checked == true)
                reset.res_type = Schematix.FSM.My_Reset.Reset_Type.Synchonous;
            else
                reset.res_type = Schematix.FSM.My_Reset.Reset_Type.Asynchonous;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
