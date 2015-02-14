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
    public partial class StateProperties : Form
    {
        Schematix.FSM.Constructor_Core core;
        Schematix.FSM.My_State state;

        private void LoadState()
        {
            textBoxLocationX.Text = state.rect.Location.X.ToString();
            textBoxLocationY.Text = state.rect.Location.Y.ToString();
            textBoxWidth.Text = state.rect.Width.ToString();
            textBoxHeight.Text = state.rect.Height.ToString();

            textBoxName.Text = state.name;
            Schematix.FSM.Constructor_Core.SetColorText(labelColor, state.color);
            colorDialog1.Color = state.color;
            richTextBoxCondition.Text = state.condition;
            richTextBoxActivityInput.Text = state.ActivityInput;
            richTextBoxActivityExit.Text = state.ActivityExit;
        }

        private void OK()
        {
            state.rect = new Rectangle(int.Parse(textBoxLocationX.Text), int.Parse(textBoxLocationY.Text), int.Parse(textBoxWidth.Text), int.Parse(textBoxHeight.Text));
            state.name = textBoxName.Text;
            state.label_name.Text = textBoxName.Text;
            state.color = colorDialog1.Color;
            state.condition = richTextBoxCondition.Text;
            state.ActivityInput = richTextBoxActivityInput.Text;
            state.ActivityExit = richTextBoxActivityExit.Text;
            core.AddToHistory("State " + state.name + " change properties");
        }

        public StateProperties(Schematix.FSM.My_State state, Schematix.FSM.Constructor_Core core)
        {
            InitializeComponent();
            this.core = core;
            this.state = state;
            LoadState();
        }

        private void buttonColor_Click(object sender, EventArgs e)
        {
            DialogResult res = colorDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                Schematix.FSM.Constructor_Core.SetColorText(labelColor, colorDialog1.Color);
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
    }
}
