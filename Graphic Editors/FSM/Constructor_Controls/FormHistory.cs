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
    public partial class FormHistory : Form
    {
        private Schematix.FSM.Constructor_Core core;
        public FormHistory(Schematix.FSM.Constructor_Core core)
        {
            InitializeComponent();
            this.core = core;

            foreach (Schematix.FSM.HistoryElem history_elem in core.Graph_History.History)
            {
                listBoxHistory.Items.Add(history_elem.Name);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            string SelectedElem = (string)listBoxHistory.SelectedItem;
            core.Graph_History.SetPosition(SelectedElem);
            this.Close();
        }
    }
}
