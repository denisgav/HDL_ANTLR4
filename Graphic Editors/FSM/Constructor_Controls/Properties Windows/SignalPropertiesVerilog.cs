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
    public partial class SignalPropertiesVerilog : Form
    {
        private Schematix.FSM.My_Signal signal;
        private Schematix.FSM.Constructor_Core core;

        private static readonly string[] VerilogLogicalDataTypes = new string[]
        {
           "supply0",
           "supply1",
           "tri",
           "tri0",
           "tri1",
           "triand",
           "trior",
           "trireg",
           "wand",
           "wire",
           "wor"
        };

        public SignalPropertiesVerilog(Schematix.FSM.My_Signal signal, Schematix.FSM.Constructor_Core core)
        {
            InitializeComponent();
            this.signal = signal;
            this.core = core;
            LoadData();
        }

        #region load_data
        private void LoadData()
        {
            textBoxName.Text = signal.name;
            textBoxValue.Text = signal.Default_Value;
            if (signal.Type_inf.avaliable == true)
            {
                checkBoxIsBus.Checked = true;
                LeftIndex.Value = int.Parse(signal.Type_inf.range1);
                LeftIndex.Value = int.Parse(signal.Type_inf.range2);
            }

            switch (signal.Type)
            {
                case "supply0":
                case "supply1":
                case "tri":
                case "tri0":
                case "tri1":
                case "triand":
                case "trior":
                case "trireg":
                case "wand":
                case "wire":
                case "wor":
                    cbPortType.SelectedItem = signal.Type;
                    radioButtonLogic.Checked = true;
                    break;

                case "Integer":
                    radioButtonInteger.Checked = true;
                    if (signal.Type_inf.avaliable == true)
                    {
                        radioButtonInteger.Checked = true;
                    }
                    break;               

                default:
                    radioButtonUserDefined.Checked = true;
                    textBoxType.Text = signal.Type;
                    break;
            }

            if (signal is Schematix.FSM.My_Constant)
            {
                this.Text = "Constant properties";
            }
            else
            {
                if (signal is Schematix.FSM.My_Port)
                {
                    this.Text = "Port properties";
                    switch ((signal as Schematix.FSM.My_Port).Direction)
                    {
                        case Schematix.FSM.My_Port.PortDirection.In:
                            {
                                radioButtonInput.Checked = true;
                            }
                            break;

                        case Schematix.FSM.My_Port.PortDirection.InOut:
                            {
                                radioButtonInout.Checked = true;
                            }
                            break;

                        case Schematix.FSM.My_Port.PortDirection.Out:
                            {
                                radioButtonOutput.Checked = true;
                            }
                            break;

                        default:
                            break;
                    }

                    switch ((signal as Schematix.FSM.My_Port).Port_Type)
                    {
                        case Schematix.FSM.My_Port.PortType.Combinatioral:
                            {
                                radioButtonCombinatioral.Checked = true;
                            }
                            break;

                        case Schematix.FSM.My_Port.PortType.Registered:
                            {
                                radioButtonRegistered.Checked = true;
                            }
                            break;

                        case Schematix.FSM.My_Port.PortType.Clocked:
                            {
                                radioButtonClocked.Checked = true;
                            }
                            break;

                        case Schematix.FSM.My_Port.PortType.ClockEnable:
                            {
                                checkBoxClockEnable.Checked = true;
                            }
                            break;

                        case Schematix.FSM.My_Port.PortType.Clock:
                            {
                                checkBoxClock.Checked = true;
                            }
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                }
            }

        }
        #endregion

        private void radioButtonLogic_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxBusSize.Enabled = true;
            groupBoxType.Enabled = false;
            radioButtonInteger.Enabled = false;
            cbPortType.Enabled = true;
        }

        private void radioButtonInteger_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxBusSize.Enabled = false;
            groupBoxType.Enabled = false;
            cbPortType.Enabled = false;
        }

        private void radioButtonUserDefined_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxBusSize.Enabled = false;
            groupBoxType.Enabled = true;
            radioButtonInteger.Enabled = false;
            cbPortType.Enabled = false;
        }

        private void radioButtonInput_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxClock.Enabled = true;
            textBoxValue.Enabled = false;
            groupBoxType2.Enabled = false;
            radioButtonInteger.Enabled = false;
        }

        private void radioButtonOutput_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxClock.Enabled = false;
            textBoxValue.Enabled = true;
            groupBoxType2.Enabled = false;
            radioButtonInteger.Enabled = true;
        }

        private void radioButtonInout_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxClock.Enabled = true;
            textBoxValue.Enabled = true;
            groupBoxType2.Enabled = false;
            radioButtonInteger.Enabled = true;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            OK();
            Close();
        }

        #region ok
        private void OK()
        {
            signal.name = textBoxName.Text;
            signal.label_name.Text = textBoxName.Text;
            if (radioButtonUserDefined.Checked == false)
                signal.Default_Value = textBoxValue.Text;
            else
                signal.Default_Value = "";

            if (radioButtonInteger.Checked == true)
            {
                signal.Type = "Integer";
            }
            if (radioButtonLogic.Checked == true)
            {
                signal.Type = cbPortType.SelectedItem as string;
                if (checkBoxIsBus.Checked == true)
                {
                    signal.Type_inf.avaliable = true;
                    signal.Type_inf.range1 = LeftIndex.ToString();
                    signal.Type_inf.range2 = RightIndex.ToString();
                }
            }
            if (radioButtonUserDefined.Checked == true)
            {
                signal.Type = textBoxType.Text;
            }

            if (signal is Schematix.FSM.My_Constant)
            {
                core.AddToHistory("Constant " + signal.name + " change properties");
            }
            if (signal is Schematix.FSM.My_Port)
            {
                Schematix.FSM.My_Port p = signal as Schematix.FSM.My_Port;
                if (radioButtonInput.Checked == true)
                    p.Direction = Schematix.FSM.My_Port.PortDirection.In;
                if (radioButtonOutput.Checked == true)
                    p.Direction = Schematix.FSM.My_Port.PortDirection.Out;
                if (radioButtonInout.Checked == true)
                    p.Direction = Schematix.FSM.My_Port.PortDirection.InOut;

                if (radioButtonCombinatioral.Checked == true)
                    p.Port_Type = Schematix.FSM.My_Port.PortType.Combinatioral;
                if (radioButtonRegistered.Checked == true)
                    p.Port_Type = Schematix.FSM.My_Port.PortType.Registered;
                if (radioButtonClocked.Checked == true)
                    p.Port_Type = Schematix.FSM.My_Port.PortType.Clocked;

                if (checkBoxClock.Checked == true)
                    p.Port_Type = Schematix.FSM.My_Port.PortType.Clock;
                if (checkBoxClockEnable.Checked == true)
                    p.Port_Type = Schematix.FSM.My_Port.PortType.ClockEnable;
            }
            if (signal is Schematix.FSM.My_Signal)
            {
                core.AddToHistory("Signal " + signal.name + " change properties");
            }
        }
        #endregion

        private void checkBoxIsBus_CheckedChanged(object sender, EventArgs e)
        {
            LeftIndex.Enabled = checkBoxIsBus.Checked;
            RightIndex.Enabled = checkBoxIsBus.Checked;
        }
    }
}