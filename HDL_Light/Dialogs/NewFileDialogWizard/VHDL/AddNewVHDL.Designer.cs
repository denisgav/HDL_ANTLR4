namespace Schematix.Dialogs.NewFileDialogWizard
{
    partial class AddNewVHDL
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
        	this.btDelete = new System.Windows.Forms.Button();
        	this.btnAdd = new System.Windows.Forms.Button();
        	this.ButtonOk = new System.Windows.Forms.Button();
        	this.RightIndex = new System.Windows.Forms.NumericUpDown();
        	this.LeftIndex = new System.Windows.Forms.NumericUpDown();
        	this.groupBox1 = new System.Windows.Forms.GroupBox();
        	this.rbBuffer = new System.Windows.Forms.RadioButton();
        	this.rbInOut = new System.Windows.Forms.RadioButton();
        	this.rbOut = new System.Windows.Forms.RadioButton();
        	this.rbIn = new System.Windows.Forms.RadioButton();
        	this.lbPortList = new System.Windows.Forms.ListBox();
        	this.label4 = new System.Windows.Forms.Label();
        	this.tbPortName = new System.Windows.Forms.TextBox();
        	this.label3 = new System.Windows.Forms.Label();
        	this.label2 = new System.Windows.Forms.Label();
        	this.label1 = new System.Windows.Forms.Label();
        	this.ArchName = new System.Windows.Forms.TextBox();
        	this.EntityName = new System.Windows.Forms.TextBox();
        	this.FileName = new System.Windows.Forms.TextBox();
        	this.panel1 = new System.Windows.Forms.Panel();
        	this.tabControl1 = new System.Windows.Forms.TabControl();
        	this.tabPage1 = new System.Windows.Forms.TabPage();
        	this.buttonNext = new System.Windows.Forms.Button();
        	this.tabPage2 = new System.Windows.Forms.TabPage();
        	this.groupBox2 = new System.Windows.Forms.GroupBox();
        	this.comboBoxUserDefinitionType = new System.Windows.Forms.ComboBox();
        	this.cbPortType = new System.Windows.Forms.ComboBox();
        	this.label5 = new System.Windows.Forms.Label();
        	this.pictureBoxPreview = new System.Windows.Forms.PictureBox();
        	this.buttonCancel = new System.Windows.Forms.Button();
        	((System.ComponentModel.ISupportInitialize)(this.RightIndex)).BeginInit();
        	((System.ComponentModel.ISupportInitialize)(this.LeftIndex)).BeginInit();
        	this.groupBox1.SuspendLayout();
        	this.panel1.SuspendLayout();
        	this.tabControl1.SuspendLayout();
        	this.tabPage1.SuspendLayout();
        	this.tabPage2.SuspendLayout();
        	this.groupBox2.SuspendLayout();
        	((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).BeginInit();
        	this.SuspendLayout();
        	// 
        	// btDelete
        	// 
        	this.btDelete.Location = new System.Drawing.Point(245, 204);
        	this.btDelete.Name = "btDelete";
        	this.btDelete.Size = new System.Drawing.Size(60, 23);
        	this.btDelete.TabIndex = 27;
        	this.btDelete.Text = "Delete";
        	this.btDelete.UseVisualStyleBackColor = true;
        	this.btDelete.Click += new System.EventHandler(this.btDelete_Click);
        	// 
        	// btnAdd
        	// 
        	this.btnAdd.Location = new System.Drawing.Point(245, 178);
        	this.btnAdd.Name = "btnAdd";
        	this.btnAdd.Size = new System.Drawing.Size(60, 23);
        	this.btnAdd.TabIndex = 24;
        	this.btnAdd.Text = "Add";
        	this.btnAdd.UseVisualStyleBackColor = true;
        	this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
        	// 
        	// ButtonOk
        	// 
        	this.ButtonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        	this.ButtonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
        	this.ButtonOk.Location = new System.Drawing.Point(226, 369);
        	this.ButtonOk.Name = "ButtonOk";
        	this.ButtonOk.Size = new System.Drawing.Size(60, 23);
        	this.ButtonOk.TabIndex = 17;
        	this.ButtonOk.Text = "Ok";
        	this.ButtonOk.UseVisualStyleBackColor = true;
        	this.ButtonOk.Click += new System.EventHandler(this.ButtonOk_Click);
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
        	this.groupBox1.Controls.Add(this.rbBuffer);
        	this.groupBox1.Controls.Add(this.rbInOut);
        	this.groupBox1.Controls.Add(this.rbOut);
        	this.groupBox1.Controls.Add(this.rbIn);
        	this.groupBox1.Location = new System.Drawing.Point(245, 59);
        	this.groupBox1.Name = "groupBox1";
        	this.groupBox1.Size = new System.Drawing.Size(89, 113);
        	this.groupBox1.TabIndex = 23;
        	this.groupBox1.TabStop = false;
        	this.groupBox1.Text = "PortDirection";
        	// 
        	// rbBuffer
        	// 
        	this.rbBuffer.AutoSize = true;
        	this.rbBuffer.Enabled = false;
        	this.rbBuffer.Location = new System.Drawing.Point(11, 89);
        	this.rbBuffer.Name = "rbBuffer";
        	this.rbBuffer.Size = new System.Drawing.Size(53, 17);
        	this.rbBuffer.TabIndex = 3;
        	this.rbBuffer.TabStop = true;
        	this.rbBuffer.Text = "Buffer";
        	this.rbBuffer.UseVisualStyleBackColor = true;
        	this.rbBuffer.CheckedChanged += new System.EventHandler(this.RadioButtonDirection_CheckedChanged);
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
        	// lbPortList
        	// 
        	this.lbPortList.FormattingEnabled = true;
        	this.lbPortList.Location = new System.Drawing.Point(152, 53);
        	this.lbPortList.Name = "lbPortList";
        	this.lbPortList.Size = new System.Drawing.Size(87, 173);
        	this.lbPortList.TabIndex = 22;
        	this.lbPortList.SelectedIndexChanged += new System.EventHandler(this.lbPortList_SelectedIndexChanged);
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
        	// tbPortName
        	// 
        	this.tbPortName.Enabled = false;
        	this.tbPortName.Location = new System.Drawing.Point(215, 6);
        	this.tbPortName.Name = "tbPortName";
        	this.tbPortName.Size = new System.Drawing.Size(126, 20);
        	this.tbPortName.TabIndex = 20;
        	this.tbPortName.TextChanged += new System.EventHandler(this.tbPortName_TextChanged);
        	// 
        	// label3
        	// 
        	this.label3.AutoSize = true;
        	this.label3.Location = new System.Drawing.Point(3, 82);
        	this.label3.Name = "label3";
        	this.label3.Size = new System.Drawing.Size(235, 13);
        	this.label3.TabIndex = 35;
        	this.label3.Text = "Architecture name (same as entity name if blank)";
        	// 
        	// label2
        	// 
        	this.label2.AutoSize = true;
        	this.label2.Location = new System.Drawing.Point(3, 36);
        	this.label2.Name = "label2";
        	this.label2.Size = new System.Drawing.Size(192, 13);
        	this.label2.TabIndex = 34;
        	this.label2.Text = "Entity name (same as file name if blank)";
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
        	// ArchName
        	// 
        	this.ArchName.Location = new System.Drawing.Point(6, 108);
        	this.ArchName.Name = "ArchName";
        	this.ArchName.Size = new System.Drawing.Size(322, 20);
        	this.ArchName.TabIndex = 32;
        	// 
        	// EntityName
        	// 
        	this.EntityName.Location = new System.Drawing.Point(6, 53);
        	this.EntityName.Name = "EntityName";
        	this.EntityName.Size = new System.Drawing.Size(322, 20);
        	this.EntityName.TabIndex = 31;
        	// 
        	// FileName
        	// 
        	this.FileName.Location = new System.Drawing.Point(124, 13);
        	this.FileName.Name = "FileName";
        	this.FileName.Size = new System.Drawing.Size(204, 20);
        	this.FileName.TabIndex = 30;
        	this.FileName.TextChanged += new System.EventHandler(this.FileName_TextChanged);
        	// 
        	// panel1
        	// 
        	this.panel1.Controls.Add(this.label1);
        	this.panel1.Controls.Add(this.ArchName);
        	this.panel1.Controls.Add(this.label3);
        	this.panel1.Controls.Add(this.FileName);
        	this.panel1.Controls.Add(this.label2);
        	this.panel1.Controls.Add(this.EntityName);
        	this.panel1.Location = new System.Drawing.Point(6, 9);
        	this.panel1.Name = "panel1";
        	this.panel1.Size = new System.Drawing.Size(333, 131);
        	this.panel1.TabIndex = 36;
        	// 
        	// tabControl1
        	// 
        	this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        	        	        	| System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.tabControl1.Controls.Add(this.tabPage1);
        	this.tabControl1.Controls.Add(this.tabPage2);
        	this.tabControl1.Location = new System.Drawing.Point(12, 12);
        	this.tabControl1.Name = "tabControl1";
        	this.tabControl1.SelectedIndex = 0;
        	this.tabControl1.Size = new System.Drawing.Size(355, 351);
        	this.tabControl1.TabIndex = 38;
        	// 
        	// tabPage1
        	// 
        	this.tabPage1.Controls.Add(this.buttonNext);
        	this.tabPage1.Controls.Add(this.panel1);
        	this.tabPage1.Location = new System.Drawing.Point(4, 22);
        	this.tabPage1.Name = "tabPage1";
        	this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
        	this.tabPage1.Size = new System.Drawing.Size(347, 325);
        	this.tabPage1.TabIndex = 0;
        	this.tabPage1.Text = "Step One";
        	this.tabPage1.UseVisualStyleBackColor = true;
        	// 
        	// buttonNext
        	// 
        	this.buttonNext.Location = new System.Drawing.Point(266, 294);
        	this.buttonNext.Name = "buttonNext";
        	this.buttonNext.Size = new System.Drawing.Size(75, 23);
        	this.buttonNext.TabIndex = 37;
        	this.buttonNext.Text = "Next -->>";
        	this.buttonNext.UseVisualStyleBackColor = true;
        	this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
        	// 
        	// tabPage2
        	// 
        	this.tabPage2.Controls.Add(this.groupBox2);
        	this.tabPage2.Controls.Add(this.label5);
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
        	this.tabPage2.Size = new System.Drawing.Size(347, 325);
        	this.tabPage2.TabIndex = 1;
        	this.tabPage2.Text = "Step Two";
        	this.tabPage2.UseVisualStyleBackColor = true;
        	// 
        	// groupBox2
        	// 
        	this.groupBox2.Controls.Add(this.comboBoxUserDefinitionType);
        	this.groupBox2.Controls.Add(this.cbPortType);
        	this.groupBox2.Location = new System.Drawing.Point(6, 232);
        	this.groupBox2.Name = "groupBox2";
        	this.groupBox2.Size = new System.Drawing.Size(325, 79);
        	this.groupBox2.TabIndex = 38;
        	this.groupBox2.TabStop = false;
        	this.groupBox2.Text = "Port Type";
        	// 
        	// comboBoxUserDefinitionType
        	// 
        	this.comboBoxUserDefinitionType.FormattingEnabled = true;
        	this.comboBoxUserDefinitionType.Items.AddRange(new object[] {
        	        	        	"STD_LOGIC_VECTOR (0 to 7)",
        	        	        	"STD_LOGIC_VECTOR (7 downto 0) ",
        	        	        	"STD_ULOGIC",
        	        	        	"STD_ULOGIC_VECTOR (0 to 7) ",
        	        	        	"STD_ULOGIC_VECTOR (7 downto 0)",
        	        	        	"BIT_VECTOR (0 to 7)",
        	        	        	"BIT_VECTOR (7 downto 0)",
        	        	        	"INTEGER range 0 to 127",
        	        	        	"CHARACTER"});
        	this.comboBoxUserDefinitionType.Location = new System.Drawing.Point(6, 46);
        	this.comboBoxUserDefinitionType.Name = "comboBoxUserDefinitionType";
        	this.comboBoxUserDefinitionType.Size = new System.Drawing.Size(313, 21);
        	this.comboBoxUserDefinitionType.TabIndex = 29;
        	this.comboBoxUserDefinitionType.Visible = false;
        	this.comboBoxUserDefinitionType.SelectedIndexChanged += new System.EventHandler(this.comboBoxUserDefinitionType_SelectedIndexChanged);
        	// 
        	// cbPortType
        	// 
        	this.cbPortType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        	this.cbPortType.Enabled = false;
        	this.cbPortType.FormattingEnabled = true;
        	this.cbPortType.Items.AddRange(new object[] {
        	        	        	"STD_LOGIC",
        	        	        	"STD_LOGIC_VECTOR",
        	        	        	"BIT",
        	        	        	"BIT_VECTOR",
        	        	        	"BOOLEAN",
        	        	        	"INTEGER",
        	        	        	"REAL",
        	        	        	"User Defined Type"});
        	this.cbPortType.Location = new System.Drawing.Point(6, 19);
        	this.cbPortType.Name = "cbPortType";
        	this.cbPortType.Size = new System.Drawing.Size(313, 21);
        	this.cbPortType.TabIndex = 28;
        	this.cbPortType.SelectedIndexChanged += new System.EventHandler(this.cbPortType_SelectedIndexChanged);
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
        	// pictureBoxPreview
        	// 
        	this.pictureBoxPreview.Location = new System.Drawing.Point(6, 6);
        	this.pictureBoxPreview.Name = "pictureBoxPreview";
        	this.pictureBoxPreview.Size = new System.Drawing.Size(140, 220);
        	this.pictureBoxPreview.TabIndex = 19;
        	this.pictureBoxPreview.TabStop = false;
        	this.pictureBoxPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxPreview_Paint);
        	// 
        	// buttonCancel
        	// 
        	this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        	this.buttonCancel.Location = new System.Drawing.Point(292, 369);
        	this.buttonCancel.Name = "buttonCancel";
        	this.buttonCancel.Size = new System.Drawing.Size(75, 23);
        	this.buttonCancel.TabIndex = 39;
        	this.buttonCancel.Text = "Cancel";
        	this.buttonCancel.UseVisualStyleBackColor = true;
        	this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
        	// 
        	// AddNewVHDL
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(374, 404);
        	this.Controls.Add(this.buttonCancel);
        	this.Controls.Add(this.tabControl1);
        	this.Controls.Add(this.ButtonOk);
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        	this.MaximizeBox = false;
        	this.MinimizeBox = false;
        	this.Name = "AddNewVHDL";
        	this.Text = "Add New VHDL Item";
        	this.Load += new System.EventHandler(this.AddNewVHDLCode_Load);
        	((System.ComponentModel.ISupportInitialize)(this.RightIndex)).EndInit();
        	((System.ComponentModel.ISupportInitialize)(this.LeftIndex)).EndInit();
        	this.groupBox1.ResumeLayout(false);
        	this.groupBox1.PerformLayout();
        	this.panel1.ResumeLayout(false);
        	this.panel1.PerformLayout();
        	this.tabControl1.ResumeLayout(false);
        	this.tabPage1.ResumeLayout(false);
        	this.tabPage2.ResumeLayout(false);
        	this.tabPage2.PerformLayout();
        	this.groupBox2.ResumeLayout(false);
        	((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).EndInit();
        	this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button btDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.PictureBox pictureBoxPreview;
        private System.Windows.Forms.Button ButtonOk;
        private System.Windows.Forms.NumericUpDown RightIndex;
        private System.Windows.Forms.NumericUpDown LeftIndex;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbInOut;
        private System.Windows.Forms.RadioButton rbOut;
        private System.Windows.Forms.RadioButton rbIn;
        private System.Windows.Forms.ListBox lbPortList;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbPortName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ArchName;
        private System.Windows.Forms.TextBox EntityName;
        public System.Windows.Forms.TextBox FileName;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboBoxUserDefinitionType;
        private System.Windows.Forms.ComboBox cbPortType;
        private System.Windows.Forms.RadioButton rbBuffer;
    }
}