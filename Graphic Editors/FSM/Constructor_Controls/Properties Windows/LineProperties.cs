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
    public partial class LineProperties : Form
    {
        Schematix.FSM.My_Line line;
        Schematix.FSM.Constructor_Core core;
        public LineProperties(Schematix.FSM.My_Line line, Schematix.FSM.Constructor_Core core)
        {
            InitializeComponent();
            this.line = line;
            this.core = core;

            textBoxName.Text = line.name;
            numericUpDownPriority.Value = line.priority;
            richTextBoxCondition.Text = line.condition;
            richTextBoxAction.Text = line.Action;
            Schematix.FSM.Constructor_Core.SetColorText(labelColor, line.color);
            comboBoxStyle.Items.Clear();
            comboBoxStyle.Items.Add(Schematix.FSM.My_Line.DrawningStyle.DrawningBezier);
            comboBoxStyle.Items.Add(Schematix.FSM.My_Line.DrawningStyle.DrawningCurve);
            comboBoxStyle.SelectedItem = line.DrawStyle;
        }

        private void Ok()
        {
            line.name = textBoxName.Text;
            line.priority = (int)numericUpDownPriority.Value;
            line.color = colorDialog1.Color;// и это тоже
            line.condition = richTextBoxCondition.Text;
            line.label_condition.Text = richTextBoxCondition.Text;
            line.Action = richTextBoxAction.Text;
            line.DrawStyle = (Schematix.FSM.My_Line.DrawningStyle)comboBoxStyle.SelectedItem;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult res = colorDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                Schematix.FSM.Constructor_Core.SetColorText(labelColor, colorDialog1.Color);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Ok();
            // вот ту думал изменение цвета применять
            core.AddToHistory("Line " + line.name + " change properties");
            core.Bitmap.UpdateBitmap();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
