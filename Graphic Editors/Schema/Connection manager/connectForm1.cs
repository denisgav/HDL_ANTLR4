using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace csx
{
    public partial class connectForm1 : Form
    {
        private bool last;
        cnManager parent;
        public connectForm1(int left, int right, object parent, bool last)
        {
            this.last = last;
            this.parent = (cnManager)parent;
            this.parent.resultOk = false;
            InitializeComponent();
            numericUpDown1.Maximum = Math.Max(left, right);
            numericUpDown1.Minimum = Math.Min(left, right);
            label4.Text = "min = " + numericUpDown1.Minimum.ToString() + " ,max = " + numericUpDown1.Maximum.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            parent.resultOk = true;
            if (last)
                parent.assign.Add(0, (int)numericUpDown1.Value);
            else
                parent.assign.Add((int)numericUpDown1.Value, 0);
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}