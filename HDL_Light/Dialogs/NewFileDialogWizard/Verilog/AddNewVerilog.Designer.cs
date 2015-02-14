namespace Schematix.Dialogs.NewFileDialogWizard
{
    partial class AddNewVerilog
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
        	this.buttonCancel = new System.Windows.Forms.Button();
        	this.tabControl1 = new System.Windows.Forms.TabControl();
        	this.tabPage1 = new System.Windows.Forms.TabPage();
        	this.buttonNext = new System.Windows.Forms.Button();
        	this.panel1 = new System.Windows.Forms.Panel();
        	this.label1 = new System.Windows.Forms.Label();
        	this.ModuleName = new System.Windows.Forms.TextBox();
        	this.label3 = new System.Windows.Forms.Label();
        	this.FileName = new System.Windows.Forms.TextBox();
        	this.tabPage2 = new System.Windows.Forms.TabPage();
        	this.checkBoxIsBus = new System.Windows.Forms.CheckBox();
        	this.groupBox3 = new System.Windows.Forms.GroupBox();
        	this.comboBoxRightTimeScaleIndex = new System.Windows.Forms.ComboBox();
        	this.label2 = new System.Windows.Forms.Label();
        	this.comboBoxLeftTimeScaleIndex = new System.Windows.Forms.ComboBox();
        	this.label5 = new System.Windows.Forms.Label();
        	this.groupBox2 = new System.Windows.Forms.GroupBox();
        	this.cbPortType = new System.Windows.Forms.ComboBox();
        	this.pictureBoxPreview = new System.Windows.Forms.PictureBox();
        	this.btDelete = new System.Windows.Forms.Button();
        	this.label4 = new System.Windows.Forms.Label();
        	this.btnAdd = new System.Windows.Forms.Button();
        	this.tbPortName = new System.Windows.Forms.TextBox();
        	this.LeftIndex = new System.Windows.Forms.NumericUpDown();
        	this.groupBox1 = new System.Windows.Forms.GroupBox();
        	this.rbInOut = new System.Windows.Forms.RadioButton();
        	this.rbOut = new System.Windows.Forms.RadioButton();
        	this.rbIn = new System.Windows.Forms.RadioButton();
        	this.RightIndex = new System.Windows.Forms.NumericUpDown();
        	this.lbPortList = new System.Windows.Forms.ListBox();
        	this.ButtonOk = new System.Windows.Forms.Button();
        	this.tabControl1.SuspendLayout();
        	this.tabPage1.SuspendLayout();
        	this.panel1.SuspendLayout();
        	this.tabPage2.SuspendLayout();
        	this.groupBox3.SuspendLayout();
        	this.groupBox2.SuspendLayout();
        	((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).BeginInit();
        	((System.ComponentModel.ISupportInitialize)(this.LeftIndex)).BeginInit();
        	this.groupBox1.SuspendLayout();
        	((System.ComponentModel.ISupportInitialize)(this.RightIndex)).BeginInit();
        	this.SuspendLayout();
        	// 
        	// buttonCancel
        	// 
        	this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        	this.buttonCancel.Location = new System.Drawing.Point(290, 385);
        	this.buttonCancel.Name = "buttonCancel";
        	this.buttonCancel.Size = new System.Drawing.Size(75, 23);
        	this.buttonCancel.TabIndex = 42;
        	this.buttonCancel.Text = "Cancel";
        	this.buttonCancel.UseVisualStyleBackColor = true;
        	this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
        	// 
        	// tabControl1
        	// 
        	this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        	        	        	| System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.tabControl1.Controls.Add(this.tabPage1);
        	this.tabControl1.Controls.Add(this.tabPage2);
        	this.tabControl1.Location = new System.Drawing.Point(10, 12);
        	this.tabControl1.Name = "tabControl1";
        	this.tabControl1.SelectedIndex = 0;
        	this.tabControl1.Size = new System.Drawing.Size(355, 367);
        	this.tabControl1.TabIndex = 41;
        	// 
        	// tabPage1
        	// 
        	this.tabPage1.Controls.Add(this.buttonNext);
        	this.tabPage1.Controls.Add(this.panel1);
        	this.tabPage1.Location = new System.Drawing.Point(4, 22);
        	this.tabPage1.Name = "tabPage1";
        	this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
        	this.tabPage1.Size = new System.Drawing.Size(347, 341);
        	this.tabPage1.TabIndex = 0;
        	this.tabPage1.Text = "Step One";
        	this.tabPage1.UseVisualStyleBackColor = true;
        	// 
        	// buttonNext
        	// 
        	this.buttonNext.Location = new System.Drawing.Point(266, 312);
        	this.buttonNext.Name = "buttonNext";
        	this.buttonNext.Size = new System.Drawing.Size(75, 23);
        	this.buttonNext.TabIndex = 37;
        	this.buttonNext.Text = "Next -->>";
        	this.buttonNext.UseVisualStyleBackColor = true;
        	this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
        	// 
        	// panel1
        	// 
        	this.panel1.Controls.Add(this.label1);
        	this.panel1.Controls.Add(this.ModuleName);
        	this.panel1.Controls.Add(this.label3);
        	this.panel1.Controls.Add(this.FileName);
        	this.panel1.Location = new System.Drawing.Point(6, 9);
        	this.panel1.Name = "panel1";
        	this.panel1.Size = new System.Drawing.Size(333, 131);
        	this.panel1.TabIndex = 36;
        	// 
        	// label1
        	// 
        	this.label1.AutoSize = true;
        	this.label1.Location = new System.Drawing.Point(3, 16);
        	this.label1.Name = "label1";
        	this.label1.Size = new System.Drawing.Size(111, 13);
        	this.label1.TabIndex = 33;
        	this.label1.Text = "Input source file name";
        	// 
        	// ModuleName
        	// 
        	this.ModuleName.Location = new System.Drawing.Point(6, 62);
        	this.ModuleName.Name = "ModuleName";
        	this.ModuleName.Size = new System.Drawing.Size(322, 20);
        	this.ModuleName.TabIndex = 32;
        	// 
        	// label3
        	// 
        	this.label3.AutoSize = true;
        	this.label3.Location = new System.Drawing.Point(3, 36);
        	this.label3.Name = "label3";
        	this.label3.Size = new System.Drawing.Size(213, 13);
        	this.label3.TabIndex = 35;
        	this.label3.Text = "Module name (same as entity name if blank)";
        	// 
        	// FileName
        	// 
        	this.FileName.Location = new System.Drawing.Point(124, 13);
        	this.FileName.Name = "FileName";
        	this.FileName.Size = new System.Drawing.Size(204, 20);
        	this.FileName.TabIndex = 30;
        	this.FileName.TextChanged += new System.EventHandler(this.FileName_TextChanged);
        	// 
        	// tabPage2
        	// 
        	this.tabPage2.Controls.Add(this.checkBoxIsBus);
        	this.tabPage2.Controls.Add(this.groupBox3);
        	this.tabPage2.Controls.Add(this.label5);
        	this.tabPage2.Controls.Add(this.groupBox2);
        	this.tabPage2.Controls.Add(this.pictureBoxPreview);
        	this.tabPage2.Controls.Add(this.btDelete);
        	this.tabPage2.Controls.Add(this.label4);
        	this.tabPage2.Controls.Add(this.btnAdd);
        	this.tabPage2.Controls.Add(this.tbPortName);
        	this.tabPage2.Controls.Add(this.LeftIndex);
        	this.tabPage2.Controls.Add(this.groupBox1);
        	this.tabPage2.Controls.Add(this.RightIndex);
        	this.tabPage2.Controls.Add(this.lbPortList);
        	this.tabPage2.Location = new System.Drawing.Point(4, 22);
        	this.tabPage2.Name = "tabPage2";
        	this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
        	this.tabPage2.Size = new System.Drawing.Size(347, 341);
        	this.tabPage2.TabIndex = 1;
        	this.tabPage2.Text = "Step Two";
        	this.tabPage2.UseVisualStyleBackColor = true;
        	// 
        	// checkBoxIsBus
        	// 
        	this.checkBoxIsBus.AutoSize = true;
        	this.checkBoxIsBus.Location = new System.Drawing.Point(152, 59);
        	this.checkBoxIsBus.Name = "checkBoxIsBus";
        	this.checkBoxIsBus.Size = new System.Drawing.Size(61, 17);
        	this.checkBoxIsBus.TabIndex = 39;
        	this.checkBoxIsBus.Text = "Is Bus?";
        	this.checkBoxIsBus.UseVisualStyleBackColor = true;
        	this.checkBoxIsBus.CheckedChanged += new System.EventHandler(this.checkBoxIsBus_CheckedChanged);
        	// 
        	// groupBox3
        	// 
        	this.groupBox3.Controls.Add(this.comboBoxRightTimeScaleIndex);
        	this.groupBox3.Controls.Add(this.label2);
        	this.groupBox3.Controls.Add(this.comboBoxLeftTimeScaleIndex);
        	this.groupBox3.Location = new System.Drawing.Point(9, 288);
        	this.groupBox3.Name = "groupBox3";
        	this.groupBox3.Size = new System.Drawing.Size(332, 46);
        	this.groupBox3.TabIndex = 38;
        	this.groupBox3.TabStop = false;
        	this.groupBox3.Text = "Time Scale";
        	// 
        	// comboBoxRightTimeScaleIndex
        	// 
        	this.comboBoxRightTimeScaleIndex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        	this.comboBoxRightTimeScaleIndex.FormattingEnabled = true;
        	this.comboBoxRightTimeScaleIndex.Items.AddRange(new object[] {
        	        	        	"1 fs",
        	        	        	"10 fs",
        	        	        	"100 fs",
        	        	        	"1 ps",
        	        	        	"10 ps",
        	        	        	"100 ps",
        	        	        	"1 ns",
        	        	        	"10 ns",
        	        	        	"100 ns",
        	        	        	"1 us",
        	        	        	"10 us",
        	        	        	"100 us",
        	        	        	"1 ms",
        	        	        	"10 ms",
        	        	        	"100 ms",
        	        	        	"1 s",
        	        	        	"10 s",
        	        	        	"100 s"});
        	this.comboBoxRightTimeScaleIndex.Location = new System.Drawing.Point(152, 20);
        	this.comboBoxRightTimeScaleIndex.Name = "comboBoxRightTimeScaleIndex";
        	this.comboBoxRightTimeScaleIndex.Size = new System.Drawing.Size(121, 21);
        	this.comboBoxRightTimeScaleIndex.TabIndex = 2;
        	// 
        	// label2
        	// 
        	this.label2.AutoSize = true;
        	this.label2.Location = new System.Drawing.Point(134, 23);
        	this.label2.Name = "label2";
        	this.label2.Size = new System.Drawing.Size(12, 13);
        	this.label2.TabIndex = 1;
        	this.label2.Text = "/";
        	// 
        	// comboBoxLeftTimeScaleIndex
        	// 
        	this.comboBoxLeftTimeScaleIndex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        	this.comboBoxLeftTimeScaleIndex.FormattingEnabled = true;
        	this.comboBoxLeftTimeScaleIndex.Items.AddRange(new object[] {
        	        	        	"1 fs",
        	        	        	"10 fs",
        	        	        	"100 fs",
        	        	        	"1 ps",
        	        	        	"10 ps",
        	        	        	"100 ps",
        	        	        	"1 ns",
        	        	        	"10 ns",
        	        	        	"100 ns",
        	        	        	"1 us",
        	        	        	"10 us",
        	        	        	"100 us",
        	        	        	"1 ms",
        	        	        	"10 ms",
        	        	        	"100 ms",
        	        	        	"1 s",
        	        	        	"10 s",
        	        	        	"100 s"});
        	this.comboBoxLeftTimeScaleIndex.Location = new System.Drawing.Point(7, 20);
        	this.comboBoxLeftTimeScaleIndex.Name = "comboBoxLeftTimeScaleIndex";
        	this.comboBoxLeftTimeScaleIndex.Size = new System.Drawing.Size(121, 21);
        	this.comboBoxLeftTimeScaleIndex.TabIndex = 0;
        	// 
        	// label5
        	// 
        	this.label5.AutoSize = true;
        	this.label5.Location = new System.Drawing.Point(152, 35);
        	this.label5.Name = "label5";
        	this.label5.Size = new System.Drawing.Size(51, 13);
        	this.label5.TabIndex = 22;
        	this.label5.Text = "Bus Size:";
        	// 
        	// groupBox2
        	// 
        	this.groupBox2.Controls.Add(this.cbPortType);
        	this.groupBox2.Location = new System.Drawing.Point(9, 232);
        	this.groupBox2.Name = "groupBox2";
        	this.groupBox2.Size = new System.Drawing.Size(325, 49);
        	this.groupBox2.TabIndex = 37;
        	this.groupBox2.TabStop = false;
        	this.groupBox2.Text = "Port Type";
        	// 
        	// cbPortType
        	// 
        	this.cbPortType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        	this.cbPortType.Enabled = false;
        	this.cbPortType.FormattingEnabled = true;
        	this.cbPortType.Items.AddRange(new object[] {
        	        	        	"supply0",
        	        	        	"supply1",
        	        	        	"tri",
        	        	        	"tri0",
        	        	        	"tri1",
        	        	        	"triand",
        	        	        	"trior",
        	        	        	"trireg",
        	        	        	"reg",
        	        	        	"wand",
        	        	        	"wire",
        	        	        	"wor"});
        	this.cbPortType.Location = new System.Drawing.Point(6, 19);
        	this.cbPortType.Name = "cbPortType";
        	this.cbPortType.Size = new System.Drawing.Size(313, 21);
        	this.cbPortType.TabIndex = 28;
        	this.cbPortType.SelectedIndexChanged += new System.EventHandler(this.cbPortType_SelectedIndexChanged);
        	// 
        	// pictureBoxPreview
        	// 
        	this.pictureBoxPreview.Location = new System.Drawing.Point(6, 6);
        	this.pictureBoxPreview.Name = "pictureBoxPreview";
        	this.pictureBoxPreview.Size = new System.Drawing.Size(140, 220);
        	this.pictureBoxPreview.TabIndex = 19;
        	this.pictureBoxPreview.TabStop = false;
        	this.pictureBoxPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxPreview_Paint);
        	// 
        	// btDelete
        	// 
        	this.btDelete.Location = new System.Drawing.Point(245, 203);
        	this.btDelete.Name = "btDelete";
        	this.btDelete.Size = new System.Drawing.Size(60, 23);
        	this.btDelete.TabIndex = 27;
        	this.btDelete.Text = "Delete";
        	this.btDelete.UseVisualStyleBackColor = true;
        	this.btDelete.Click += new System.EventHandler(this.btDelete_Click);
        	// 
        	// label4
        	// 
        	this.label4.AutoSize = true;
        	this.label4.Location = new System.Drawing.Point(152, 6);
        	this.label4.Name = "label4";
        	this.label4.Size = new System.Drawing.Size(57, 13);
        	this.label4.TabIndex = 21;
        	this.label4.Text = "Port Name";
        	// 
        	// btnAdd
        	// 
        	this.btnAdd.Location = new System.Drawing.Point(245, 177);
        	this.btnAdd.Name = "btnAdd";
        	this.btnAdd.Size = new System.Drawing.Size(60, 23);
        	this.btnAdd.TabIndex = 24;
        	this.btnAdd.Text = "Add";
        	this.btnAdd.UseVisualStyleBackColor = true;
        	this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
        	// 
        	// tbPortName
        	// 
        	this.tbPortName.Enabled = false;
        	this.tbPortName.Location = new System.Drawing.Point(215, 6);
        	this.tbPortName.Name = "tbPortName";
        	this.tbPortName.Size = new System.Drawing.Size(126, 20);
        	this.tbPortName.TabIndex = 20;
        	this.tbPortName.TextChanged += new System.EventHandler(this.tbPortName_TextChanged);
        	// 
        	// LeftIndex
        	// 
        	this.LeftIndex.Enabled = false;
        	this.LeftIndex.Location = new System.Drawing.Point(215, 33);
        	this.LeftIndex.Maximum = new decimal(new int[] {
        	        	        	32,
        	        	        	0,
        	        	        	0,
        	        	        	0});
        	this.LeftIndex.Name = "LeftIndex";
        	this.LeftIndex.Size = new System.Drawing.Size(40, 20);
        	this.LeftIndex.TabIndex = 25;
        	this.LeftIndex.ValueChanged += new System.EventHandler(this.BoundsChanged);
        	// 
        	// groupBox1
        	// 
        	this.groupBox1.Controls.Add(this.rbInOut);
        	this.groupBox1.Controls.Add(this.rbOut);
        	this.groupBox1.Controls.Add(this.rbIn);
        	this.groupBox1.Location = new System.Drawing.Point(245, 59);
        	this.groupBox1.Name = "groupBox1";
        	this.groupBox1.Size = new System.Drawing.Size(89, 91);
        	this.groupBox1.TabIndex = 23;
        	this.groupBox1.TabStop = false;
        	this.groupBox1.Text = "PortDirection";
        	// 
        	// rbInOut
        	// 
        	this.rbInOut.AutoSize = true;
        	this.rbInOut.Enabled = false;
        	this.rbInOut.Location = new System.Drawing.Point(11, 65);
        	this.rbInOut.Name = "rbInOut";
        	this.rbInOut.Size = new System.Drawing.Size(51, 17);
        	this.rbInOut.TabIndex = 2;
        	this.rbInOut.Text = "InOut";
        	this.rbInOut.UseVisualStyleBackColor = true;
        	this.rbInOut.CheckedChanged += new System.EventHandler(this.RadioButtonDirection_CheckedChanged);
        	// 
        	// rbOut
        	// 
        	this.rbOut.AutoSize = true;
        	this.rbOut.Enabled = false;
        	this.rbOut.Location = new System.Drawing.Point(11, 42);
        	this.rbOut.Name = "rbOut";
        	this.rbOut.Size = new System.Drawing.Size(42, 17);
        	this.rbOut.TabIndex = 1;
        	this.rbOut.Text = "Out";
        	this.rbOut.UseVisualStyleBackColor = true;
        	this.rbOut.CheckedChanged += new System.EventHandler(this.RadioButtonDirection_CheckedChanged);
        	// 
        	// rbIn
        	// 
        	this.rbIn.AutoSize = true;
        	this.rbIn.Checked = true;
        	this.rbIn.Enabled = false;
        	this.rbIn.Location = new System.Drawing.Point(11, 19);
        	this.rbIn.Name = "rbIn";
        	this.rbIn.Size = new System.Drawing.Size(34, 17);
        	this.rbIn.TabIndex = 0;
        	this.rbIn.TabStop = true;
        	this.rbIn.Text = "In";
        	this.rbIn.UseVisualStyleBackColor = true;
        	this.rbIn.CheckedChanged += new System.EventHandler(this.RadioButtonDirection_CheckedChanged);
        	// 
        	// RightIndex
        	// 
        	this.RightIndex.Enabled = false;
        	this.RightIndex.Location = new System.Drawing.Point(254, 33);
        	this.RightIndex.Maximum = new decimal(new int[] {
        	        	        	32,
        	        	        	0,
        	        	        	0,
        	        	        	0});
        	this.RightIndex.Name = "RightIndex";
        	this.RightIndex.Size = new System.Drawing.Size(40, 20);
        	this.RightIndex.TabIndex = 26;
        	this.RightIndex.ValueChanged += new System.EventHandler(this.BoundsChanged);
        	// 
        	// lbPortList
        	// 
        	this.lbPortList.FormattingEnabled = true;
        	this.lbPortList.Location = new System.Drawing.Point(152, 79);
        	this.lbPortList.Name = "lbPortList";
        	this.lbPortList.Size = new System.Drawing.Size(87, 147);
        	this.lbPortList.TabIndex = 22;
        	this.lbPortList.SelectedIndexChanged += new System.EventHandler(this.lbPortList_SelectedIndexChanged);
        	// 
        	// ButtonOk
        	// 
        	this.ButtonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        	this.ButtonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
        	this.ButtonOk.Location = new System.Drawing.Point(224, 385);
        	this.ButtonOk.Name = "ButtonOk";
        	this.ButtonOk.Size = new System.Drawing.Size(60, 23);
        	this.ButtonOk.TabIndex = 40;
        	this.ButtonOk.Text = "Ok";
        	this.ButtonOk.UseVisualStyleBackColor = true;
        	this.ButtonOk.Click += new System.EventHandler(this.ButtonOk_Click);
        	// 
        	// AddNewVerilog
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(374, 420);
        	this.Controls.Add(this.buttonCancel);
        	this.Controls.Add(this.tabControl1);
        	this.Controls.Add(this.ButtonOk);
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        	this.MaximizeBox = false;
        	this.MinimizeBox = false;
        	this.Name = "AddNewVerilog";
        	this.Text = "Add New Verilog Item";
        	this.Load += new System.EventHandler(this.AddNewVerilogCode_Load);
        	this.tabControl1.ResumeLayout(false);
        	this.tabPage1.ResumeLayout(false);
        	this.panel1.ResumeLayout(false);
        	this.panel1.PerformLayout();
        	this.tabPage2.ResumeLayout(false);
        	this.tabPage2.PerformLayout();
        	this.groupBox3.ResumeLayout(false);
        	this.groupBox3.PerformLayout();
        	this.groupBox2.ResumeLayout(false);
        	((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).EndInit();
        	((System.ComponentModel.ISupportInitialize)(this.LeftIndex)).EndInit();
        	this.groupBox1.ResumeLayout(false);
        	this.groupBox1.PerformLayout();
        	((System.ComponentModel.ISupportInitialize)(this.RightIndex)).EndInit();
        	this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ModuleName;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox FileName;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cbPortType;
        private System.Windows.Forms.PictureBox pictureBoxPreview;
        private System.Windows.Forms.Button btDelete;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox tbPortName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbInOut;
        private System.Windows.Forms.RadioButton rbOut;
        private System.Windows.Forms.RadioButton rbIn;
        private System.Windows.Forms.NumericUpDown RightIndex;
        private System.Windows.Forms.ListBox lbPortList;
        private System.Windows.Forms.Button ButtonOk;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox comboBoxRightTimeScaleIndex;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxLeftTimeScaleIndex;
        private System.Windows.Forms.NumericUpDown LeftIndex;
        private System.Windows.Forms.CheckBox checkBoxIsBus;
    }
}