using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Schematix.FSM;

namespace Schematix.FSM
{
    public partial class ConstantProperties : Form
    {
        Constructor_Core core;
        My_Constant constant;

        public ConstantProperties(My_Constant constant)
        {
            this.constant = constant;
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            textBoxName.Text = constant.name;
            textBoxValue.Text = constant.Default_Value;
            if (constant.Gen_Type == My_Constant.GenerationType.Generic)
                radioButtonGeneric.Checked = true;
            if (constant.Gen_Type == My_Constant.GenerationType.Constant)
                radioButtonParameter.Checked = true;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            constant.name = textBoxName.Text;
            constant.Default_Value = textBoxValue.Text;
            if (radioButtonGeneric.Checked == true)
                constant.Gen_Type = My_Constant.GenerationType.Generic;
            if (radioButtonParameter.Checked == true)
                constant.Gen_Type = My_Constant.GenerationType.Constant;

            Close();
        }
    }
}
