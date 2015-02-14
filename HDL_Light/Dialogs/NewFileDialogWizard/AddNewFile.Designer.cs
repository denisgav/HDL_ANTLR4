namespace Schematix.Dialogs.NewFileDialogWizard
{
    partial class AddNewFile
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
        	this.components = new System.ComponentModel.Container();
        	System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("VHDL Source Code", 0);
        	System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("Verilog Source Code", 0);
        	System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("Finite State Machine VHDL", 1);
        	System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem("Finite State Machine Verilog", 1);
        	System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem("Scheme", 1);
        	System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem("EDR file", 3);
        	System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddNewFile));
        	System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem("VHDL Source Code", 0);
        	System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem("Verilog Source Code", 0);
        	System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem("Finite State Machine VHDL", 1);
        	System.Windows.Forms.ListViewItem listViewItem10 = new System.Windows.Forms.ListViewItem("Finite State Machine Verilog", 1);
        	System.Windows.Forms.ListViewItem listViewItem11 = new System.Windows.Forms.ListViewItem("Scheme", 1);
        	this.tabControl1 = new System.Windows.Forms.TabControl();
        	this.tabPage1 = new System.Windows.Forms.TabPage();
        	this.textBoxEmptyFileName = new System.Windows.Forms.TextBox();
        	this.label1 = new System.Windows.Forms.Label();
        	this.listViewEmptyFileList = new System.Windows.Forms.ListView();
        	this.imageList1 = new System.Windows.Forms.ImageList(this.components);
        	this.tabPage2 = new System.Windows.Forms.TabPage();
        	this.listViewWizardList = new System.Windows.Forms.ListView();
        	this.buttonCancel = new System.Windows.Forms.Button();
        	this.buttonOk = new System.Windows.Forms.Button();
        	this.tabControl1.SuspendLayout();
        	this.tabPage1.SuspendLayout();
        	this.tabPage2.SuspendLayout();
        	this.SuspendLayout();
        	// 
        	// tabControl1
        	// 
        	this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        	        	        	| System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.tabControl1.Controls.Add(this.tabPage1);
        	this.tabControl1.Controls.Add(this.tabPage2);
        	this.tabControl1.Location = new System.Drawing.Point(13, 13);
        	this.tabControl1.Name = "tabControl1";
        	this.tabControl1.SelectedIndex = 0;
        	this.tabControl1.Size = new System.Drawing.Size(506, 247);
        	this.tabControl1.TabIndex = 0;
        	// 
        	// tabPage1
        	// 
        	this.tabPage1.Controls.Add(this.textBoxEmptyFileName);
        	this.tabPage1.Controls.Add(this.label1);
        	this.tabPage1.Controls.Add(this.listViewEmptyFileList);
        	this.tabPage1.Location = new System.Drawing.Point(4, 22);
        	this.tabPage1.Name = "tabPage1";
        	this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
        	this.tabPage1.Size = new System.Drawing.Size(498, 221);
        	this.tabPage1.TabIndex = 0;
        	this.tabPage1.Text = "Empty Files";
        	this.tabPage1.UseVisualStyleBackColor = true;
        	// 
        	// textBoxEmptyFileName
        	// 
        	this.textBoxEmptyFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.textBoxEmptyFileName.Enabled = false;
        	this.textBoxEmptyFileName.Location = new System.Drawing.Point(54, 191);
        	this.textBoxEmptyFileName.Name = "textBoxEmptyFileName";
        	this.textBoxEmptyFileName.Size = new System.Drawing.Size(438, 20);
        	this.textBoxEmptyFileName.TabIndex = 2;
        	// 
        	// label1
        	// 
        	this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        	this.label1.AutoSize = true;
        	this.label1.Location = new System.Drawing.Point(6, 191);
        	this.label1.Name = "label1";
        	this.label1.Size = new System.Drawing.Size(41, 13);
        	this.label1.TabIndex = 1;
        	this.label1.Text = "Name: ";
        	// 
        	// listViewEmptyFileList
        	// 
        	this.listViewEmptyFileList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        	        	        	| System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	listViewItem1.Tag = "Code_VHDL_Empty";
        	listViewItem1.ToolTipText = "Create new empty VHDL source code";
        	listViewItem2.Tag = "Code_Verilog_Empty";
        	listViewItem2.ToolTipText = "Create empty Verilog Source Code";
        	listViewItem3.Tag = "FSM_Empty_VHDL";
        	listViewItem3.ToolTipText = "create empty finite state machine vhdl";
        	listViewItem4.Tag = "FSM_Empty_Verilog";
        	listViewItem4.ToolTipText = "create empty finite state machine verilog";
        	listViewItem5.Tag = "Scheme_Empty";
        	listViewItem5.ToolTipText = "Create New Scheme";
        	listViewItem6.Tag = "Empty_EDR_file";
        	this.listViewEmptyFileList.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
        	        	        	listViewItem1,
        	        	        	listViewItem2,
        	        	        	listViewItem3,
        	        	        	listViewItem4,
        	        	        	listViewItem5,
        	        	        	listViewItem6});
        	this.listViewEmptyFileList.LargeImageList = this.imageList1;
        	this.listViewEmptyFileList.Location = new System.Drawing.Point(6, 6);
        	this.listViewEmptyFileList.Name = "listViewEmptyFileList";
        	this.listViewEmptyFileList.Size = new System.Drawing.Size(486, 173);
        	this.listViewEmptyFileList.SmallImageList = this.imageList1;
        	this.listViewEmptyFileList.StateImageList = this.imageList1;
        	this.listViewEmptyFileList.TabIndex = 0;
        	this.listViewEmptyFileList.UseCompatibleStateImageBehavior = false;
        	this.listViewEmptyFileList.View = System.Windows.Forms.View.Tile;
        	this.listViewEmptyFileList.SelectedIndexChanged += new System.EventHandler(this.listViewWizardList_SelectedIndexChanged);
        	// 
        	// imageList1
        	// 
        	this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
        	this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
        	this.imageList1.Images.SetKeyName(0, "NewCode.bmp");
        	this.imageList1.Images.SetKeyName(1, "new_FSM.png");
        	this.imageList1.Images.SetKeyName(2, "NewSchema.bmp");
        	this.imageList1.Images.SetKeyName(3, "EDRFile.png");
        	// 
        	// tabPage2
        	// 
        	this.tabPage2.Controls.Add(this.listViewWizardList);
        	this.tabPage2.Location = new System.Drawing.Point(4, 22);
        	this.tabPage2.Name = "tabPage2";
        	this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
        	this.tabPage2.Size = new System.Drawing.Size(498, 221);
        	this.tabPage2.TabIndex = 1;
        	this.tabPage2.Text = "Wizards";
        	this.tabPage2.UseVisualStyleBackColor = true;
        	// 
        	// listViewWizardList
        	// 
        	this.listViewWizardList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        	        	        	| System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	listViewItem7.Tag = "Code_VHDL_Wizard";
        	listViewItem7.ToolTipText = "Create new VHDL source code with wizard";
        	listViewItem8.Tag = "Code_Verilog_Wizard";
        	listViewItem8.ToolTipText = "Create new Verilog Source Code with wizard";
        	listViewItem9.Tag = "FSM_VHDL_Wizard";
        	listViewItem9.ToolTipText = "create finite state machine VHDL with wizard";
        	listViewItem10.Tag = "FSM_Verilog_Wizard";
        	listViewItem10.ToolTipText = "Create Finite State Machine Verilog With Wizard";
        	listViewItem11.Tag = "SCHEME_WIZARD";
        	listViewItem11.ToolTipText = "Create Scheme Wizard";
        	this.listViewWizardList.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
        	        	        	listViewItem7,
        	        	        	listViewItem8,
        	        	        	listViewItem9,
        	        	        	listViewItem10,
        	        	        	listViewItem11});
        	this.listViewWizardList.LargeImageList = this.imageList1;
        	this.listViewWizardList.Location = new System.Drawing.Point(3, 6);
        	this.listViewWizardList.Name = "listViewWizardList";
        	this.listViewWizardList.Size = new System.Drawing.Size(486, 209);
        	this.listViewWizardList.SmallImageList = this.imageList1;
        	this.listViewWizardList.StateImageList = this.imageList1;
        	this.listViewWizardList.TabIndex = 1;
        	this.listViewWizardList.UseCompatibleStateImageBehavior = false;
        	this.listViewWizardList.View = System.Windows.Forms.View.Tile;
        	this.listViewWizardList.SelectedIndexChanged += new System.EventHandler(this.listViewWizardList_SelectedIndexChanged);
        	// 
        	// buttonCancel
        	// 
        	this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        	this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        	this.buttonCancel.Location = new System.Drawing.Point(440, 271);
        	this.buttonCancel.Name = "buttonCancel";
        	this.buttonCancel.Size = new System.Drawing.Size(75, 23);
        	this.buttonCancel.TabIndex = 1;
        	this.buttonCancel.Text = "Cancel";
        	this.buttonCancel.UseVisualStyleBackColor = true;
        	this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
        	// 
        	// buttonOk
        	// 
        	this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        	this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
        	this.buttonOk.Location = new System.Drawing.Point(359, 271);
        	this.buttonOk.Name = "buttonOk";
        	this.buttonOk.Size = new System.Drawing.Size(75, 23);
        	this.buttonOk.TabIndex = 2;
        	this.buttonOk.Text = "Ok";
        	this.buttonOk.UseVisualStyleBackColor = true;
        	this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
        	// 
        	// AddNewFile
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(531, 306);
        	this.Controls.Add(this.buttonOk);
        	this.Controls.Add(this.buttonCancel);
        	this.Controls.Add(this.tabControl1);
        	this.Name = "AddNewFile";
        	this.ShowInTaskbar = false;
        	this.Text = "Create new file";
        	this.tabControl1.ResumeLayout(false);
        	this.tabPage1.ResumeLayout(false);
        	this.tabPage1.PerformLayout();
        	this.tabPage2.ResumeLayout(false);
        	this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.TextBox textBoxEmptyFileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView listViewEmptyFileList;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ListView listViewWizardList;
    }
}