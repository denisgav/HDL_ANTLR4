namespace Schematix.FSM
{
    partial class LineProperties
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
        	this.tabControl1 = new System.Windows.Forms.TabControl();
        	this.tabPage1 = new System.Windows.Forms.TabPage();
        	this.groupBox1 = new System.Windows.Forms.GroupBox();
        	this.comboBoxStyle = new System.Windows.Forms.ComboBox();
        	this.groupBoxName = new System.Windows.Forms.GroupBox();
        	this.textBoxName = new System.Windows.Forms.TextBox();
        	this.groupBoxColor = new System.Windows.Forms.GroupBox();
        	this.button1 = new System.Windows.Forms.Button();
        	this.labelColor = new System.Windows.Forms.Label();
        	this.groupBoxState = new System.Windows.Forms.GroupBox();
        	this.numericUpDownPriority = new System.Windows.Forms.NumericUpDown();
        	this.label2 = new System.Windows.Forms.Label();
        	this.tabPage2 = new System.Windows.Forms.TabPage();
        	this.groupBox3 = new System.Windows.Forms.GroupBox();
        	this.richTextBoxAction = new System.Windows.Forms.RichTextBox();
        	this.groupBox2 = new System.Windows.Forms.GroupBox();
        	this.richTextBoxCondition = new System.Windows.Forms.RichTextBox();
        	this.colorDialog1 = new System.Windows.Forms.ColorDialog();
        	this.button2 = new System.Windows.Forms.Button();
        	this.button3 = new System.Windows.Forms.Button();
        	this.tabControl1.SuspendLayout();
        	this.tabPage1.SuspendLayout();
        	this.groupBox1.SuspendLayout();
        	this.groupBoxName.SuspendLayout();
        	this.groupBoxColor.SuspendLayout();
        	this.groupBoxState.SuspendLayout();
        	((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriority)).BeginInit();
        	this.tabPage2.SuspendLayout();
        	this.groupBox3.SuspendLayout();
        	this.groupBox2.SuspendLayout();
        	this.SuspendLayout();
        	// 
        	// tabControl1
        	// 
        	this.tabControl1.Controls.Add(this.tabPage1);
        	this.tabControl1.Controls.Add(this.tabPage2);
        	this.tabControl1.Location = new System.Drawing.Point(12, 12);
        	this.tabControl1.Name = "tabControl1";
        	this.tabControl1.SelectedIndex = 0;
        	this.tabControl1.Size = new System.Drawing.Size(379, 222);
        	this.tabControl1.TabIndex = 0;
        	// 
        	// tabPage1
        	// 
        	this.tabPage1.Controls.Add(this.groupBox1);
        	this.tabPage1.Controls.Add(this.groupBoxName);
        	this.tabPage1.Controls.Add(this.groupBoxColor);
        	this.tabPage1.Controls.Add(this.groupBoxState);
        	this.tabPage1.Location = new System.Drawing.Point(4, 22);
        	this.tabPage1.Name = "tabPage1";
        	this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
        	this.tabPage1.Size = new System.Drawing.Size(371, 196);
        	this.tabPage1.TabIndex = 0;
        	this.tabPage1.Text = "View/State";
        	this.tabPage1.UseVisualStyleBackColor = true;
        	// 
        	// groupBox1
        	// 
        	this.groupBox1.Controls.Add(this.comboBoxStyle);
        	this.groupBox1.Location = new System.Drawing.Point(192, 7);
        	this.groupBox1.Name = "groupBox1";
        	this.groupBox1.Size = new System.Drawing.Size(170, 46);
        	this.groupBox1.TabIndex = 3;
        	this.groupBox1.TabStop = false;
        	this.groupBox1.Text = "Style";
        	// 
        	// comboBoxStyle
        	// 
        	this.comboBoxStyle.FormattingEnabled = true;
        	this.comboBoxStyle.Location = new System.Drawing.Point(6, 15);
        	this.comboBoxStyle.Name = "comboBoxStyle";
        	this.comboBoxStyle.Size = new System.Drawing.Size(158, 21);
        	this.comboBoxStyle.TabIndex = 0;
        	// 
        	// groupBoxName
        	// 
        	this.groupBoxName.Controls.Add(this.textBoxName);
        	this.groupBoxName.Location = new System.Drawing.Point(6, 59);
        	this.groupBoxName.Name = "groupBoxName";
        	this.groupBoxName.Size = new System.Drawing.Size(356, 53);
        	this.groupBoxName.TabIndex = 2;
        	this.groupBoxName.TabStop = false;
        	this.groupBoxName.Text = "Name";
        	// 
        	// textBoxName
        	// 
        	this.textBoxName.Location = new System.Drawing.Point(12, 20);
        	this.textBoxName.Name = "textBoxName";
        	this.textBoxName.Size = new System.Drawing.Size(329, 20);
        	this.textBoxName.TabIndex = 0;
        	// 
        	// groupBoxColor
        	// 
        	this.groupBoxColor.Controls.Add(this.button1);
        	this.groupBoxColor.Controls.Add(this.labelColor);
        	this.groupBoxColor.Location = new System.Drawing.Point(6, 118);
        	this.groupBoxColor.Name = "groupBoxColor";
        	this.groupBoxColor.Size = new System.Drawing.Size(356, 69);
        	this.groupBoxColor.TabIndex = 1;
        	this.groupBoxColor.TabStop = false;
        	this.groupBoxColor.Text = "Color";
        	// 
        	// button1
        	// 
        	this.button1.Location = new System.Drawing.Point(12, 40);
        	this.button1.Name = "button1";
        	this.button1.Size = new System.Drawing.Size(75, 23);
        	this.button1.TabIndex = 1;
        	this.button1.Text = "Select color";
        	this.button1.UseVisualStyleBackColor = true;
        	this.button1.Click += new System.EventHandler(this.button1_Click);
        	// 
        	// labelColor
        	// 
        	this.labelColor.AutoSize = true;
        	this.labelColor.Location = new System.Drawing.Point(7, 20);
        	this.labelColor.Name = "labelColor";
        	this.labelColor.Size = new System.Drawing.Size(0, 13);
        	this.labelColor.TabIndex = 0;
        	// 
        	// groupBoxState
        	// 
        	this.groupBoxState.Controls.Add(this.numericUpDownPriority);
        	this.groupBoxState.Controls.Add(this.label2);
        	this.groupBoxState.Location = new System.Drawing.Point(6, 6);
        	this.groupBoxState.Name = "groupBoxState";
        	this.groupBoxState.Size = new System.Drawing.Size(179, 47);
        	this.groupBoxState.TabIndex = 0;
        	this.groupBoxState.TabStop = false;
        	// 
        	// numericUpDownPriority
        	// 
        	this.numericUpDownPriority.Location = new System.Drawing.Point(80, 19);
        	this.numericUpDownPriority.Maximum = new decimal(new int[] {
        	        	        	16,
        	        	        	0,
        	        	        	0,
        	        	        	0});
        	this.numericUpDownPriority.Name = "numericUpDownPriority";
        	this.numericUpDownPriority.ReadOnly = true;
        	this.numericUpDownPriority.Size = new System.Drawing.Size(93, 20);
        	this.numericUpDownPriority.TabIndex = 4;
        	// 
        	// label2
        	// 
        	this.label2.AutoSize = true;
        	this.label2.Location = new System.Drawing.Point(12, 19);
        	this.label2.Name = "label2";
        	this.label2.Size = new System.Drawing.Size(38, 13);
        	this.label2.TabIndex = 3;
        	this.label2.Text = "Priority";
        	// 
        	// tabPage2
        	// 
        	this.tabPage2.Controls.Add(this.groupBox3);
        	this.tabPage2.Controls.Add(this.groupBox2);
        	this.tabPage2.Location = new System.Drawing.Point(4, 22);
        	this.tabPage2.Name = "tabPage2";
        	this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
        	this.tabPage2.Size = new System.Drawing.Size(371, 196);
        	this.tabPage2.TabIndex = 1;
        	this.tabPage2.Text = "VHDL";
        	this.tabPage2.UseVisualStyleBackColor = true;
        	// 
        	// groupBox3
        	// 
        	this.groupBox3.Controls.Add(this.richTextBoxAction);
        	this.groupBox3.Location = new System.Drawing.Point(10, 103);
        	this.groupBox3.Name = "groupBox3";
        	this.groupBox3.Size = new System.Drawing.Size(355, 87);
        	this.groupBox3.TabIndex = 2;
        	this.groupBox3.TabStop = false;
        	this.groupBox3.Text = "Action";
        	// 
        	// richTextBoxAction
        	// 
        	this.richTextBoxAction.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.richTextBoxAction.Location = new System.Drawing.Point(3, 16);
        	this.richTextBoxAction.Name = "richTextBoxAction";
        	this.richTextBoxAction.Size = new System.Drawing.Size(349, 68);
        	this.richTextBoxAction.TabIndex = 0;
        	this.richTextBoxAction.Text = "";
        	// 
        	// groupBox2
        	// 
        	this.groupBox2.Controls.Add(this.richTextBoxCondition);
        	this.groupBox2.Location = new System.Drawing.Point(7, 7);
        	this.groupBox2.Name = "groupBox2";
        	this.groupBox2.Size = new System.Drawing.Size(358, 89);
        	this.groupBox2.TabIndex = 1;
        	this.groupBox2.TabStop = false;
        	this.groupBox2.Text = "Condition";
        	// 
        	// richTextBoxCondition
        	// 
        	this.richTextBoxCondition.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.richTextBoxCondition.Location = new System.Drawing.Point(3, 16);
        	this.richTextBoxCondition.Name = "richTextBoxCondition";
        	this.richTextBoxCondition.Size = new System.Drawing.Size(352, 70);
        	this.richTextBoxCondition.TabIndex = 0;
        	this.richTextBoxCondition.Text = "";
        	// 
        	// button2
        	// 
        	this.button2.Location = new System.Drawing.Point(233, 240);
        	this.button2.Name = "button2";
        	this.button2.Size = new System.Drawing.Size(75, 23);
        	this.button2.TabIndex = 1;
        	this.button2.Text = "OK";
        	this.button2.UseVisualStyleBackColor = true;
        	this.button2.Click += new System.EventHandler(this.button2_Click);
        	// 
        	// button3
        	// 
        	this.button3.Location = new System.Drawing.Point(312, 240);
        	this.button3.Name = "button3";
        	this.button3.Size = new System.Drawing.Size(75, 23);
        	this.button3.TabIndex = 2;
        	this.button3.Text = "Cancel";
        	this.button3.UseVisualStyleBackColor = true;
        	this.button3.Click += new System.EventHandler(this.button3_Click);
        	// 
        	// LineProperties
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(407, 269);
        	this.Controls.Add(this.button3);
        	this.Controls.Add(this.button2);
        	this.Controls.Add(this.tabControl1);
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
        	this.Name = "LineProperties";
        	this.Text = "Line Properties";
        	this.tabControl1.ResumeLayout(false);
        	this.tabPage1.ResumeLayout(false);
        	this.groupBox1.ResumeLayout(false);
        	this.groupBoxName.ResumeLayout(false);
        	this.groupBoxName.PerformLayout();
        	this.groupBoxColor.ResumeLayout(false);
        	this.groupBoxColor.PerformLayout();
        	this.groupBoxState.ResumeLayout(false);
        	this.groupBoxState.PerformLayout();
        	((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriority)).EndInit();
        	this.tabPage2.ResumeLayout(false);
        	this.groupBox3.ResumeLayout(false);
        	this.groupBox2.ResumeLayout(false);
        	this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBoxState;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBoxColor;
        private System.Windows.Forms.GroupBox groupBoxName;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label labelColor;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.RichTextBox richTextBoxCondition;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBoxStyle;
        private System.Windows.Forms.NumericUpDown numericUpDownPriority;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RichTextBox richTextBoxAction;
    }
}