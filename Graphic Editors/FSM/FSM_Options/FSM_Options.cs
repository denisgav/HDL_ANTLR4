using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Schematix.Core;

namespace Schematix.FSM
{
    public partial class FSM_Options : Form
    {
        private Schematix.FSM.FSM_Language language = Schematix.FSM.FSM_Language.VHDL;

        VHDL_Module vhdl_module = null;
        Verilog_Module verilog_module = null;

        public FSM_OptionsHelper Options { get; set; }

        public FSM_Options(VHDL_Module module)
        {
            this.vhdl_module = module;
            this.language = Schematix.FSM.FSM_Language.VHDL;
            Options = new FSM_OptionsHelper();
            InitializeComponent();
        }

        public FSM_Options(Verilog_Module module)
        {
            this.verilog_module = module;
            this.language = Schematix.FSM.FSM_Language.Verilog;
            Options = new FSM_OptionsHelper();
            InitializeComponent();
        }

        private void numericUpDownNumStates_ValueChanged(object sender, EventArgs e)
        {
            Options.NumberOfStates = (int)numericUpDownNumStates.Value;
            comboBoxResetState.Items.Clear();
            comboBoxResetState.Items.Add("none");
            for (int i = 0; i < Options.NumberOfStates; i++)
                comboBoxResetState.Items.Add(string.Format("S{0}", i));
            comboBoxResetState.SelectedItem = Options.ResetState;
        }

        private void comboBoxStatesLayout_SelectedIndexChanged(object sender, EventArgs e)
        {
            string layout = comboBoxStatesLayout.SelectedItem as string;
            switch (layout)
            {
                case "Circular":
                    Options.StatesLayout = FSM_OptionsHelper.FSMStatesLayout.Circular;
                    break;
                case "Linear Horisontal":
                    Options.StatesLayout = FSM_OptionsHelper.FSMStatesLayout.LinearHorisontal;
                    break;
                case "Linear Vertical":
                    Options.StatesLayout = FSM_OptionsHelper.FSMStatesLayout.LinearVertical;
                    break;
                default:
                    break;
            }
        }

        private void comboBoxTransition_SelectedIndexChanged(object sender, EventArgs e)
        {
            string transition = comboBoxTransition.SelectedItem as string;
            switch(transition)
            {
                case "None":
                    Options.Transition = FSM_OptionsHelper.FSMTransition.None;
                    break;
                case "Forward":
                    Options.Transition = FSM_OptionsHelper.FSMTransition.Forward;
                    break;
                case "Backward":
                    Options.Transition = FSM_OptionsHelper.FSMTransition.Backward;
                    break;
                case "Both":
                    Options.Transition = FSM_OptionsHelper.FSMTransition.Both;
                    break;
                default:
                    break;
            }
        }

        private void comboBoxResetState_SelectedIndexChanged(object sender, EventArgs e)
        {
            Options.ResetState = comboBoxResetState.SelectedItem as string;
        }

        private void buttonDefaultDUH_Click(object sender, EventArgs e)
        {
            switch (language)
            {
                case Schematix.FSM.FSM_Language.VHDL:
                    richTextBoxDesignUnitHeader.Text = FSM_OptionsHelper.Default_Design_Unit_Header_VHDL;
                    break;
                case Schematix.FSM.FSM_Language.Verilog:
                    richTextBoxDesignUnitHeader.Text = FSM_OptionsHelper.Default_Design_Unit_Header_Verilog;
                    break;
                default:
                    break;
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Options.Design_UnitHeader = richTextBoxDesignUnitHeader.Text;
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void FSM_Options_Load(object sender, EventArgs e)
        {
            switch (language)
            {
                case Schematix.FSM.FSM_Language.VHDL:
                    richTextBoxDesignUnitHeader.Text = FSM_OptionsHelper.Default_Design_Unit_Header_VHDL;
                    break;
                case Schematix.FSM.FSM_Language.Verilog:
                    richTextBoxDesignUnitHeader.Text = verilog_module.Timescale;
                    break;
                default:
                    break;
            }

            comboBoxStatesLayout.SelectedIndex = 0;
            comboBoxTransition.SelectedIndex = 0;
            comboBoxResetState.SelectedIndex = 0;
        }
    }
}
