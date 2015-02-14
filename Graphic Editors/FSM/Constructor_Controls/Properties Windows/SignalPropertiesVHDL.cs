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
    public partial class SignalPropertiesVHDL : Form
    {
        private Schematix.FSM.My_Signal signal;
        private Schematix.FSM.Constructor_Core core;
        public SignalPropertiesVHDL(Schematix.FSM.My_Signal signal, Schematix.FSM.Constructor_Core core)
        {
            InitializeComponent();
            this.signal = signal;
            this.core = core;
            comboBoxGenType.Parent = this;
            comboBoxGenType.Location = new Point(12, 95);
            LoadData();
        }

        #region load_data
        private void LoadData()
        {           
            foreach (Schematix.FSM.My_Constant c in core.Graph.Constants)
            {
                if(c.Gen_Type == Schematix.FSM.My_Constant.GenerationType.Generic)
                {
                    comboBoxBusRange1.Items.Add(c.name);
                    comboBoxBusRange2.Items.Add(c.name);
                    comboBoxIntegerRange1.Items.Add(c.name);
                    comboBoxIntegerRange1.Items.Add(c.name);
                }
            }

            textBoxName.Text = signal.name;
            textBoxValue.Text = signal.Default_Value;

            switch (signal.Type)
            {
                case "Std_Logic":
                    radioButtonStd_Logic.Checked = true;
                    if (signal.Type_inf.avaliable == true)
                    {
                        comboBoxBusRange1.SelectedItem = signal.Type_inf.range1;
                        comboBoxBusRange2.SelectedItem = signal.Type_inf.range2;
                        if (signal.Type_inf.to == true)
                        {
                            radioButtonBusTo.Checked = true;
                        }
                        else
                        {
                            radioButtonBusDownTo.Checked = true;
                        }
                        checkBoxBusAvaliable.Checked = true;
                    }
                break;

                case "Std_uLogic":
                    radioButtonStd_uLogic.Checked = true;
                    if (signal.Type_inf.avaliable == true)
                    {
                        comboBoxIntegerRange1.SelectedItem = signal.Type_inf.range1;
                        comboBoxIntegerRange2.SelectedItem = signal.Type_inf.range2;
                        if (signal.Type_inf.to == true)
                        {
                            radioButtonBusTo.Checked = true;
                        }
                        else
                        {
                            radioButtonBusDownTo.Checked = true;
                        }
                        checkBoxBusAvaliable.Checked = true;
                    }
                break;

                case "Bit":
                    radioButtonBit.Checked = true;
                    if (signal.Type_inf.avaliable == true)
                    {
                        comboBoxIntegerRange1.SelectedItem = signal.Type_inf.range1;
                        comboBoxIntegerRange2.SelectedItem = signal.Type_inf.range2;
                        if (signal.Type_inf.to == true)
                        {
                            radioButtonBusTo.Checked = true;
                        }
                        else
                        {
                            radioButtonBusDownTo.Checked = true;
                        }
                        checkBoxBusAvaliable.Checked = true;
                    }
                break;

                case "Integer":
                    radioButtonInteger.Checked = true;
                    if (signal.Type_inf.avaliable == true)
                    {
                        comboBoxIntegerRange1.SelectedItem = signal.Type_inf.range1;
                        comboBoxIntegerRange2.SelectedItem = signal.Type_inf.range2;
                        if (signal.Type_inf.to == true)
                        {
                            radioButtonIntegerTo.Checked = true;
                        }
                        else
                        {
                            radioButtonIntegerDownTo.Checked = true;
                        }
                        checkBoxBusAvaliable.Checked = true;
                    }
                break;

                case "Boolean":
                    radioButtonBoolean.Checked = true;
                break;

                case "Character":
                    radioButtonCharacter.Checked = true;
                break;

                default:
                    radioButtonUserDefined.Checked = true;
                    comboBoxType.Text = signal.Type;
                break;
            }

            if (signal is Schematix.FSM.My_Constant)
            {
                this.Text = "Constant properties";
                comboBoxGenType.Items.Add(Schematix.FSM.My_Constant.GenerationType.Constant);
                comboBoxGenType.Items.Add(Schematix.FSM.My_Constant.GenerationType.Generic);
                comboBoxGenType.SelectedItem = (signal as Schematix.FSM.My_Constant).Gen_Type;
                comboBoxGenType.Visible = true;
                groupBoxPortProperties.Visible = false;
            }
            else
            {
                if (signal is Schematix.FSM.My_Port)
                {
                    this.Text = "Port properties";
                    comboBoxGenType.Visible = false;
                    this.ClientSize = new Size(ClientSize.Width + 105, ClientSize.Height);
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

                        case Schematix.FSM.My_Port.PortDirection.Buffer:
                            {
                                radioButtonBuffer.Checked = true;
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
                    comboBoxGenType.Visible = false;
                    groupBoxPortProperties.Visible = false;
                }
            }

        }
        #endregion

        private string GetDataFromCombobox(ComboBox box)
        {
            string res = box.Text;
            if (string.IsNullOrEmpty(res))
            {
                res = (string)box.SelectedItem;
            }
            return res;
        }

        private void radioButtonStd_Logic_CheckedChanged(object sender, EventArgs e)
        {
            if ((checkBoxClockEnable.Checked == true) || (checkBoxClock.Checked == true))
                return;

            foreach (Control c in groupBoxBusRange.Controls)
            {
                c.Enabled = true;
            }
            foreach (Control c in groupBoxIntegerRange.Controls)
            {
                c.Enabled = false;
            }
            foreach (Control c in groupBoxType.Controls)
            {
                c.Enabled = false;
            }
            foreach (Control c in groupBoxClock.Controls)
            {
                c.Enabled = true;
            }
        }

        private void radioButtonInteger_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Control c in groupBoxBusRange.Controls)
            {
                c.Enabled = false;
            }
            foreach (Control c in groupBoxIntegerRange.Controls)
            {
                c.Enabled = true;
            }
            foreach (Control c in groupBoxType.Controls)
            {
                c.Enabled = false;
            }
            foreach (Control c in groupBoxClock.Controls)
            {
                c.Enabled = false;
            }
        }

        private void radioButtonBoolean_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Control c in groupBoxBusRange.Controls)
            {
                c.Enabled = false;
            }
            foreach (Control c in groupBoxIntegerRange.Controls)
            {
                c.Enabled = false;
            }
            foreach (Control c in groupBoxType.Controls)
            {
                c.Enabled = false;
            }
        }

        private void radioButtonUserDefined_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Control c in groupBoxBusRange.Controls)
            {
                c.Enabled = false;
            }
            foreach (Control c in groupBoxIntegerRange.Controls)
            {
                c.Enabled = false;
            }
            foreach (Control c in groupBoxType.Controls)
            {
                c.Enabled = true;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            OK();
            this.Close();
        }

        private void SetBusRange()
        {
            if (checkBoxBusAvaliable.Checked == true)
            {
                signal.Type_inf.avaliable = true;
                if (radioButtonBusTo.Checked == true)
                    signal.Type_inf.to = true;
                else
                    signal.Type_inf.to = false;
                signal.Type_inf.range1 = GetDataFromCombobox(comboBoxBusRange1);
                signal.Type_inf.range2 = GetDataFromCombobox(comboBoxBusRange2);
            }
        }

        private void SetIntegerRange()
        {
            if (checkBoxIntegerAvalable.Checked == true)
            {
                signal.Type_inf.avaliable = true;
                if (radioButtonIntegerTo.Checked == true)
                    signal.Type_inf.to = true;
                else
                    signal.Type_inf.to = false;
                signal.Type_inf.range1 = GetDataFromCombobox(comboBoxIntegerRange1);
                signal.Type_inf.range2 = GetDataFromCombobox(comboBoxIntegerRange2);
            }
        }

        private void SetAnotherRange()
        {
            signal.Type_inf.avaliable = false;
        }

        #region ok
        private void OK()
        {
            signal.name = textBoxName.Text;
            signal.label_name.Text = textBoxName.Text;
            if(radioButtonUserDefined.Checked == false)
                signal.Default_Value = textBoxValue.Text;
            else
                signal.Default_Value = "";

            if (radioButtonStd_Logic.Checked == true)
            {
                signal.Type = "Std_Logic";
                SetBusRange();
            }
            if (radioButtonStd_uLogic.Checked == true)
            {
                signal.Type = "Std_uLogic";
                SetBusRange();
            }
            if (radioButtonBit.Checked == true)
            {
                signal.Type = "Bit";
                SetBusRange();
            }
            if (radioButtonInteger.Checked == true)
            {
                signal.Type = "Integer";
                SetIntegerRange();
            }
            if (radioButtonBoolean.Checked == true)
            {
                signal.Type = "Boolean";
            }
            if (radioButtonCharacter.Checked == true)
            {
                signal.Type = "Character";
            }
            if (radioButtonUserDefined.Checked == true)
            {
                string type = comboBoxType.Text;
                if (string.IsNullOrEmpty(type))
                {
                    type = GetDataFromCombobox(comboBoxType);
                }
                signal.Type = type;
            }

            if (signal is Schematix.FSM.My_Constant)
            {
                core.AddToHistory("Constant " + signal.name + " change properties");
                (signal as Schematix.FSM.My_Constant).Gen_Type = (Schematix.FSM.My_Constant.GenerationType)comboBoxGenType.SelectedItem;
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
                if (radioButtonBuffer.Checked == true)
                    p.Direction = Schematix.FSM.My_Port.PortDirection.Buffer;

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

        private void radioButtonInput_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Control c in groupBoxType2.Controls)
            {
                c.Enabled = false;
            }
            foreach (Control c in groupBoxClock.Controls)
            {
                c.Enabled = true;
            }
            textBoxValue.Enabled = false;
        }

        private void radioButtonOutput_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Control c in groupBoxClock.Controls)
            {
                c.Enabled = false;
            }
            foreach (Control c in groupBoxType2.Controls)
            {
                c.Enabled = true;
            }
            textBoxValue.Enabled = true;
        }

        private void checkBoxClock_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxClock.Checked == true)
            {
                checkBoxClockEnable.Enabled = false;
                radioButtonOutput.Enabled = false;
                radioButtonInout.Enabled = false;
                radioButtonBuffer.Enabled = false;

                foreach (Control c in groupBoxType2.Controls)
                {
                    c.Enabled = false;
                }
                foreach (Control c in groupBoxIntegerRange.Controls)
                {
                    c.Enabled = false;
                }
                foreach (Control c in groupBoxBusRange.Controls)
                {
                    c.Enabled = false;
                }
                foreach (Control c in groupBoxType.Controls)
                {
                    c.Enabled = false;
                }
                radioButtonInteger.Enabled = false;
                radioButtonCharacter.Enabled = false;
                radioButtonBoolean.Enabled = false;
                radioButtonUserDefined.Enabled = false;
            }
            else
            {
                checkBoxClockEnable.Enabled = true;
                radioButtonOutput.Enabled = true;
                radioButtonInout.Enabled = true;
                radioButtonBuffer.Enabled = true;

                foreach (Control c in groupBoxBusRange.Controls)
                {
                    c.Enabled = true;
                }
                radioButtonInteger.Enabled = true;
                radioButtonCharacter.Enabled = true;
                radioButtonBoolean.Enabled = true;
                radioButtonUserDefined.Enabled = true;
            }
        }

        private void checkBoxClockEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxClockEnable.Checked == true)
            {
                checkBoxClock.Enabled = false;
                radioButtonOutput.Enabled = false;
                radioButtonInout.Enabled = false;
                radioButtonBuffer.Enabled = false;

                foreach (Control c in groupBoxType2.Controls)
                {
                    c.Enabled = false;
                }
                foreach (Control c in groupBoxIntegerRange.Controls)
                {
                    c.Enabled = false;
                }
                foreach (Control c in groupBoxBusRange.Controls)
                {
                    c.Enabled = false;
                }
                foreach (Control c in groupBoxType.Controls)
                {
                    c.Enabled = false;
                }
                radioButtonInteger.Enabled = false;
                radioButtonCharacter.Enabled = false;
                radioButtonBoolean.Enabled = false;
                radioButtonUserDefined.Enabled = false;
            }
            else
            {
                checkBoxClock.Enabled = true;
                radioButtonOutput.Enabled = true;
                radioButtonInout.Enabled = true;
                radioButtonBuffer.Enabled = true;

                foreach (Control c in groupBoxBusRange.Controls)
                {
                    c.Enabled = true;
                }
                radioButtonInteger.Enabled = true;
                radioButtonCharacter.Enabled = true;
                radioButtonBoolean.Enabled = true;
                radioButtonUserDefined.Enabled = true;
            }
        }
    }
}
