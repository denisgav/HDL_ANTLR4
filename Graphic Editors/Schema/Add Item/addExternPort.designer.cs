namespace csx
{
    partial class addExternPort
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.portName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.right = new System.Windows.Forms.RadioButton();
            this.left = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.InOut = new System.Windows.Forms.RadioButton();
            this.Out = new System.Windows.Forms.RadioButton();
            this.In = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rightBound = new System.Windows.Forms.NumericUpDown();
            this.leftBound = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // portName
            // 
            this.portName.Location = new System.Drawing.Point(50, 6);
            this.portName.Name = "portName";
            this.portName.Size = new System.Drawing.Size(156, 20);
            this.portName.TabIndex = 1;
            this.portName.TextChanged += new System.EventHandler(this.portName_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.right);
            this.groupBox1.Controls.Add(this.left);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(112, 32);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(94, 76);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Orientation";
            // 
            // right
            // 
            this.right.AutoSize = true;
            this.right.Location = new System.Drawing.Point(6, 50);
            this.right.Name = "right";
            this.right.Size = new System.Drawing.Size(50, 17);
            this.right.TabIndex = 1;
            this.right.Text = "Right";
            this.right.UseVisualStyleBackColor = true;
            // 
            // left
            // 
            this.left.AutoSize = true;
            this.left.Checked = true;
            this.left.Location = new System.Drawing.Point(6, 20);
            this.left.Name = "left";
            this.left.Size = new System.Drawing.Size(43, 17);
            this.left.TabIndex = 0;
            this.left.TabStop = true;
            this.left.Text = "Left";
            this.left.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.InOut);
            this.groupBox2.Controls.Add(this.Out);
            this.groupBox2.Controls.Add(this.In);
            this.groupBox2.Location = new System.Drawing.Point(12, 32);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(94, 110);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Type";
            // 
            // InOut
            // 
            this.InOut.AutoSize = true;
            this.InOut.Location = new System.Drawing.Point(8, 80);
            this.InOut.Name = "InOut";
            this.InOut.Size = new System.Drawing.Size(51, 17);
            this.InOut.TabIndex = 2;
            this.InOut.Text = "InOut";
            this.InOut.UseVisualStyleBackColor = true;
            this.InOut.CheckedChanged += new System.EventHandler(this.InOut_CheckedChanged);
            // 
            // Out
            // 
            this.Out.AutoSize = true;
            this.Out.Location = new System.Drawing.Point(8, 50);
            this.Out.Name = "Out";
            this.Out.Size = new System.Drawing.Size(42, 17);
            this.Out.TabIndex = 1;
            this.Out.Text = "Out";
            this.Out.UseVisualStyleBackColor = true;
            this.Out.CheckedChanged += new System.EventHandler(this.Out_CheckedChanged);
            // 
            // In
            // 
            this.In.AutoSize = true;
            this.In.Checked = true;
            this.In.Location = new System.Drawing.Point(8, 20);
            this.In.Name = "In";
            this.In.Size = new System.Drawing.Size(34, 17);
            this.In.TabIndex = 0;
            this.In.TabStop = true;
            this.In.Text = "In";
            this.In.UseVisualStyleBackColor = true;
            this.In.CheckedChanged += new System.EventHandler(this.In_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rightBound);
            this.groupBox3.Controls.Add(this.leftBound);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new System.Drawing.Point(12, 148);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(194, 59);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Limits";
            // 
            // rightBound
            // 
            this.rightBound.Location = new System.Drawing.Point(120, 23);
            this.rightBound.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.rightBound.Name = "rightBound";
            this.rightBound.Size = new System.Drawing.Size(55, 20);
            this.rightBound.TabIndex = 3;
            // 
            // leftBound
            // 
            this.leftBound.Location = new System.Drawing.Point(39, 23);
            this.leftBound.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.leftBound.Name = "leftBound";
            this.leftBound.Size = new System.Drawing.Size(55, 20);
            this.leftBound.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(97, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "To:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "From:";
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(112, 213);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 26);
            this.button1.TabIndex = 5;
            this.button1.Text = "Ok";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(12, 213);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(94, 26);
            this.button2.TabIndex = 6;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(112, 112);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(94, 30);
            this.button3.TabIndex = 7;
            this.button3.Text = "&Add";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // addExternPort
            // 
            this.AcceptButton = this.button3;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(217, 251);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.portName);
            this.Controls.Add(this.label1);
            this.Name = "addExternPort";
            this.Text = "Add extern port";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        public System.Windows.Forms.NumericUpDown rightBound;
        public System.Windows.Forms.NumericUpDown leftBound;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        public System.Windows.Forms.TextBox portName;
        public System.Windows.Forms.RadioButton right;
        public System.Windows.Forms.RadioButton left;
        public System.Windows.Forms.RadioButton InOut;
        public System.Windows.Forms.RadioButton Out;
        public System.Windows.Forms.RadioButton In;
    }
}