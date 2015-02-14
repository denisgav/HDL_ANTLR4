using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Schematix.FSM
{
    public partial class CommentProperties : Form
    {
        private Schematix.FSM.My_Comment comment;
        private Schematix.FSM.Constructor_Core core;
        public CommentProperties(Schematix.FSM.My_Comment comment, Schematix.FSM.Constructor_Core core)
        {
            InitializeComponent();
            this.comment = comment;
            this.core = core;
            Point Loc = new Point(comment.rect.Location.X + core.Paper.ClientStartPoint.X, comment.rect.Location.Y + core.Paper.ClientStartPoint.Y);
            Location = Loc;
            Schematix.FSM.Constructor_Core.SetColorText(labelColor, comment.color);
            colorDialog1.Color = comment.color;
            richTextBoxCommentText.Text = comment.name;
            BackColor = comment.color;
        }

        private void buttonColor_Click(object sender, EventArgs e)
        {
            DialogResult res = colorDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                BackColor = colorDialog1.Color;
                Schematix.FSM.Constructor_Core.SetColorText(labelColor, colorDialog1.Color);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            comment.color = colorDialog1.Color;
            comment.name = richTextBoxCommentText.Text;
            core.AddToHistory("Comment change properties");
            this.Dispose();
        }
    }
}
