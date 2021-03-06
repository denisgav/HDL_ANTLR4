namespace Schematix.CommonProperties
{
    partial class Options
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
            this.tabPageProject = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonDefaultProjectLocation = new System.Windows.Forms.Button();
            this.textBoxDefaultProjectLocation = new System.Windows.Forms.TextBox();
            this.tabPageTextEditor = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.labelTextEditorPreview = new System.Windows.Forms.Label();
            this.comboBoxTextEditorFontSizeUnits = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxTextEditorUnderLine = new System.Windows.Forms.CheckBox();
            this.checkBoxTextEditorStrikeout = new System.Windows.Forms.CheckBox();
            this.checkBoxTextEditorItalic = new System.Windows.Forms.CheckBox();
            this.checkBoxTextEditorBold = new System.Windows.Forms.CheckBox();
            this.comboBoxTextEditorFontSize = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxTextEditorSystemFonts = new System.Windows.Forms.ComboBox();
            this.tabPageLibraries = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.buttonDeleteVHDLLibrary = new System.Windows.Forms.Button();
            this.buttonAddVHDLLibrary = new System.Windows.Forms.Button();
            this.listViewLibrariesVHDL = new System.Windows.Forms.ListView();
            this.columnHeaderPath2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.buttonDeleteVerilogLibrary = new System.Windows.Forms.Button();
            this.buttonAddVerilogLibrary = new System.Windows.Forms.Button();
            this.listViewLibrariesVerilog = new System.Windows.Forms.ListView();
            this.columnHeaderPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPageCompilerOptions = new System.Windows.Forms.TabPage();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.richTextBoxGHDLSimulateCommand = new System.Windows.Forms.RichTextBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.richTextBoxGHDLCheckSyntaxCommand = new System.Windows.Forms.RichTextBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.richTextBoxGHDLElaborateCommand = new System.Windows.Forms.RichTextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.richTextBoxGHDLCompileCommand = new System.Windows.Forms.RichTextBox();
            this.groupBoxGHDLPath = new System.Windows.Forms.GroupBox();
            this.buttonGHDLPathBrowse = new System.Windows.Forms.Button();
            this.textBoxGHDLPathSpecified = new System.Windows.Forms.TextBox();
            this.textBoxGHDLPathEnvir = new System.Windows.Forms.TextBox();
            this.radioButtonGHDLSpecifiedPath = new System.Windows.Forms.RadioButton();
            this.radioButtonGHDLEnvirPath = new System.Windows.Forms.RadioButton();
            this.tabPageFSM = new System.Windows.Forms.TabPage();
            this.graphicsOptionsUserControlFSM = new Schematix.CommonProperties.GraphicsOptionsUserControl();
            this.tabPageEntityDrawningOptions = new System.Windows.Forms.TabPage();
            this.graphicsOptionsUserControlEntityDrawning = new Schematix.CommonProperties.GraphicsOptionsUserControl();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.folderBrowserDialogDefaultProjectLocation = new System.Windows.Forms.FolderBrowserDialog();
            this.buttonSetDefaultSettings = new System.Windows.Forms.Button();
            this.folderBrowserDialogLibraries = new System.Windows.Forms.FolderBrowserDialog();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.richTextBoxGHDLCleanCommand = new System.Windows.Forms.RichTextBox();
            this.tabControl1.SuspendLayout();
            this.tabPageProject.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPageTextEditor.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPageLibraries.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tabPageCompilerOptions.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBoxGHDLPath.SuspendLayout();
            this.tabPageFSM.SuspendLayout();
            this.tabPageEntityDrawningOptions.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageProject);
            this.tabControl1.Controls.Add(this.tabPageTextEditor);
            this.tabControl1.Controls.Add(this.tabPageLibraries);
            this.tabControl1.Controls.Add(this.tabPageCompilerOptions);
            this.tabControl1.Controls.Add(this.tabPageFSM);
            this.tabControl1.Controls.Add(this.tabPageEntityDrawningOptions);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(608, 495);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageProject
            // 
            this.tabPageProject.Controls.Add(this.groupBox1);
            this.tabPageProject.Location = new System.Drawing.Point(4, 22);
            this.tabPageProject.Name = "tabPageProject";
            this.tabPageProject.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProject.Size = new System.Drawing.Size(600, 403);
            this.tabPageProject.TabIndex = 0;
            this.tabPageProject.Text = "Project";
            this.tabPageProject.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonDefaultProjectLocation);
            this.groupBox1.Controls.Add(this.textBoxDefaultProjectLocation);
            this.groupBox1.Location = new System.Drawing.Point(7, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(576, 59);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Default Projects Location";
            // 
            // buttonDefaultProjectLocation
            // 
            this.buttonDefaultProjectLocation.Location = new System.Drawing.Point(490, 20);
            this.buttonDefaultProjectLocation.Name = "buttonDefaultProjectLocation";
            this.buttonDefaultProjectLocation.Size = new System.Drawing.Size(75, 23);
            this.buttonDefaultProjectLocation.TabIndex = 1;
            this.buttonDefaultProjectLocation.Text = "Browse...";
            this.buttonDefaultProjectLocation.UseVisualStyleBackColor = true;
            this.buttonDefaultProjectLocation.Click += new System.EventHandler(this.buttonDefaultProjectLocation_Click);
            // 
            // textBoxDefaultProjectLocation
            // 
            this.textBoxDefaultProjectLocation.Location = new System.Drawing.Point(7, 20);
            this.textBoxDefaultProjectLocation.Name = "textBoxDefaultProjectLocation";
            this.textBoxDefaultProjectLocation.Size = new System.Drawing.Size(476, 20);
            this.textBoxDefaultProjectLocation.TabIndex = 0;
            // 
            // tabPageTextEditor
            // 
            this.tabPageTextEditor.Controls.Add(this.groupBox3);
            this.tabPageTextEditor.Controls.Add(this.comboBoxTextEditorFontSizeUnits);
            this.tabPageTextEditor.Controls.Add(this.label3);
            this.tabPageTextEditor.Controls.Add(this.groupBox2);
            this.tabPageTextEditor.Controls.Add(this.comboBoxTextEditorFontSize);
            this.tabPageTextEditor.Controls.Add(this.label2);
            this.tabPageTextEditor.Controls.Add(this.label1);
            this.tabPageTextEditor.Controls.Add(this.comboBoxTextEditorSystemFonts);
            this.tabPageTextEditor.Location = new System.Drawing.Point(4, 22);
            this.tabPageTextEditor.Name = "tabPageTextEditor";
            this.tabPageTextEditor.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTextEditor.Size = new System.Drawing.Size(600, 403);
            this.tabPageTextEditor.TabIndex = 1;
            this.tabPageTextEditor.Text = "TextEditor";
            this.tabPageTextEditor.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.labelTextEditorPreview);
            this.groupBox3.Location = new System.Drawing.Point(413, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(181, 108);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Preview";
            // 
            // labelTextEditorPreview
            // 
            this.labelTextEditorPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTextEditorPreview.Location = new System.Drawing.Point(3, 16);
            this.labelTextEditorPreview.Name = "labelTextEditorPreview";
            this.labelTextEditorPreview.Size = new System.Drawing.Size(175, 89);
            this.labelTextEditorPreview.TabIndex = 0;
            this.labelTextEditorPreview.Text = "Some Text";
            this.labelTextEditorPreview.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBoxTextEditorFontSizeUnits
            // 
            this.comboBoxTextEditorFontSizeUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTextEditorFontSizeUnits.FormattingEnabled = true;
            this.comboBoxTextEditorFontSizeUnits.Location = new System.Drawing.Point(310, 37);
            this.comboBoxTextEditorFontSizeUnits.Name = "comboBoxTextEditorFontSizeUnits";
            this.comboBoxTextEditorFontSizeUnits.Size = new System.Drawing.Size(88, 21);
            this.comboBoxTextEditorFontSizeUnits.TabIndex = 6;
            this.comboBoxTextEditorFontSizeUnits.SelectedIndexChanged += new System.EventHandler(this.TextEditorOptionsFontChanged);
            this.comboBoxTextEditorFontSizeUnits.Click += new System.EventHandler(this.TextEditorOptionsFontChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(222, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Font Size Units:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBoxTextEditorUnderLine);
            this.groupBox2.Controls.Add(this.checkBoxTextEditorStrikeout);
            this.groupBox2.Controls.Add(this.checkBoxTextEditorItalic);
            this.groupBox2.Controls.Add(this.checkBoxTextEditorBold);
            this.groupBox2.Location = new System.Drawing.Point(9, 40);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(183, 74);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Font Style";
            // 
            // checkBoxTextEditorUnderLine
            // 
            this.checkBoxTextEditorUnderLine.AutoSize = true;
            this.checkBoxTextEditorUnderLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxTextEditorUnderLine.Location = new System.Drawing.Point(93, 43);
            this.checkBoxTextEditorUnderLine.Name = "checkBoxTextEditorUnderLine";
            this.checkBoxTextEditorUnderLine.Size = new System.Drawing.Size(75, 17);
            this.checkBoxTextEditorUnderLine.TabIndex = 3;
            this.checkBoxTextEditorUnderLine.Text = "UnderLine";
            this.checkBoxTextEditorUnderLine.UseVisualStyleBackColor = true;
            this.checkBoxTextEditorUnderLine.CheckedChanged += new System.EventHandler(this.TextEditorOptionsFontChanged);
            // 
            // checkBoxTextEditorStrikeout
            // 
            this.checkBoxTextEditorStrikeout.AutoSize = true;
            this.checkBoxTextEditorStrikeout.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxTextEditorStrikeout.Location = new System.Drawing.Point(93, 20);
            this.checkBoxTextEditorStrikeout.Name = "checkBoxTextEditorStrikeout";
            this.checkBoxTextEditorStrikeout.Size = new System.Drawing.Size(68, 17);
            this.checkBoxTextEditorStrikeout.TabIndex = 2;
            this.checkBoxTextEditorStrikeout.Text = "Strikeout";
            this.checkBoxTextEditorStrikeout.UseVisualStyleBackColor = true;
            this.checkBoxTextEditorStrikeout.CheckedChanged += new System.EventHandler(this.TextEditorOptionsFontChanged);
            // 
            // checkBoxTextEditorItalic
            // 
            this.checkBoxTextEditorItalic.AutoSize = true;
            this.checkBoxTextEditorItalic.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxTextEditorItalic.Location = new System.Drawing.Point(6, 43);
            this.checkBoxTextEditorItalic.Name = "checkBoxTextEditorItalic";
            this.checkBoxTextEditorItalic.Size = new System.Drawing.Size(48, 17);
            this.checkBoxTextEditorItalic.TabIndex = 1;
            this.checkBoxTextEditorItalic.Text = "Italic";
            this.checkBoxTextEditorItalic.UseVisualStyleBackColor = true;
            this.checkBoxTextEditorItalic.CheckedChanged += new System.EventHandler(this.TextEditorOptionsFontChanged);
            // 
            // checkBoxTextEditorBold
            // 
            this.checkBoxTextEditorBold.AutoSize = true;
            this.checkBoxTextEditorBold.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxTextEditorBold.Location = new System.Drawing.Point(7, 20);
            this.checkBoxTextEditorBold.Name = "checkBoxTextEditorBold";
            this.checkBoxTextEditorBold.Size = new System.Drawing.Size(51, 17);
            this.checkBoxTextEditorBold.TabIndex = 0;
            this.checkBoxTextEditorBold.Text = "Bold";
            this.checkBoxTextEditorBold.UseVisualStyleBackColor = true;
            this.checkBoxTextEditorBold.CheckedChanged += new System.EventHandler(this.TextEditorOptionsFontChanged);
            // 
            // comboBoxTextEditorFontSize
            // 
            this.comboBoxTextEditorFontSize.FormattingEnabled = true;
            this.comboBoxTextEditorFontSize.Items.AddRange(new object[] {
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "18",
            "20",
            "22",
            "24",
            "28",
            "30",
            "32",
            "34",
            "40",
            "48",
            "72"});
            this.comboBoxTextEditorFontSize.Location = new System.Drawing.Point(277, 6);
            this.comboBoxTextEditorFontSize.Name = "comboBoxTextEditorFontSize";
            this.comboBoxTextEditorFontSize.Size = new System.Drawing.Size(121, 21);
            this.comboBoxTextEditorFontSize.TabIndex = 3;
            this.comboBoxTextEditorFontSize.SelectedIndexChanged += new System.EventHandler(this.TextEditorOptionsFontChanged);
            this.comboBoxTextEditorFontSize.Click += new System.EventHandler(this.TextEditorOptionsFontChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(220, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Font Size";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Font Name :";
            // 
            // comboBoxTextEditorSystemFonts
            // 
            this.comboBoxTextEditorSystemFonts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTextEditorSystemFonts.FormattingEnabled = true;
            this.comboBoxTextEditorSystemFonts.Location = new System.Drawing.Point(71, 6);
            this.comboBoxTextEditorSystemFonts.Name = "comboBoxTextEditorSystemFonts";
            this.comboBoxTextEditorSystemFonts.Size = new System.Drawing.Size(121, 21);
            this.comboBoxTextEditorSystemFonts.TabIndex = 0;
            this.comboBoxTextEditorSystemFonts.SelectedIndexChanged += new System.EventHandler(this.TextEditorOptionsFontChanged);
            this.comboBoxTextEditorSystemFonts.Click += new System.EventHandler(this.TextEditorOptionsFontChanged);
            // 
            // tabPageLibraries
            // 
            this.tabPageLibraries.Controls.Add(this.splitContainer1);
            this.tabPageLibraries.Location = new System.Drawing.Point(4, 22);
            this.tabPageLibraries.Name = "tabPageLibraries";
            this.tabPageLibraries.Size = new System.Drawing.Size(600, 403);
            this.tabPageLibraries.TabIndex = 2;
            this.tabPageLibraries.Text = "Libraries";
            this.tabPageLibraries.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox4);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox5);
            this.splitContainer1.Size = new System.Drawing.Size(600, 403);
            this.splitContainer1.SplitterDistance = 300;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.buttonDeleteVHDLLibrary);
            this.groupBox4.Controls.Add(this.buttonAddVHDLLibrary);
            this.groupBox4.Controls.Add(this.listViewLibrariesVHDL);
            this.groupBox4.Location = new System.Drawing.Point(3, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(294, 397);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "VHDL Libraries";
            // 
            // buttonDeleteVHDLLibrary
            // 
            this.buttonDeleteVHDLLibrary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDeleteVHDLLibrary.Enabled = false;
            this.buttonDeleteVHDLLibrary.Location = new System.Drawing.Point(132, 368);
            this.buttonDeleteVHDLLibrary.Name = "buttonDeleteVHDLLibrary";
            this.buttonDeleteVHDLLibrary.Size = new System.Drawing.Size(75, 23);
            this.buttonDeleteVHDLLibrary.TabIndex = 2;
            this.buttonDeleteVHDLLibrary.Text = "Delete";
            this.buttonDeleteVHDLLibrary.UseVisualStyleBackColor = true;
            this.buttonDeleteVHDLLibrary.Click += new System.EventHandler(this.buttonDeleteVHDLLibrary_Click);
            // 
            // buttonAddVHDLLibrary
            // 
            this.buttonAddVHDLLibrary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddVHDLLibrary.Location = new System.Drawing.Point(213, 368);
            this.buttonAddVHDLLibrary.Name = "buttonAddVHDLLibrary";
            this.buttonAddVHDLLibrary.Size = new System.Drawing.Size(75, 23);
            this.buttonAddVHDLLibrary.TabIndex = 1;
            this.buttonAddVHDLLibrary.Text = "Add";
            this.buttonAddVHDLLibrary.UseVisualStyleBackColor = true;
            this.buttonAddVHDLLibrary.Click += new System.EventHandler(this.buttonAddVHDLLibrary_Click);
            // 
            // listViewLibrariesVHDL
            // 
            this.listViewLibrariesVHDL.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewLibrariesVHDL.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderPath2});
            this.listViewLibrariesVHDL.GridLines = true;
            this.listViewLibrariesVHDL.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewLibrariesVHDL.LabelEdit = true;
            this.listViewLibrariesVHDL.Location = new System.Drawing.Point(6, 19);
            this.listViewLibrariesVHDL.Name = "listViewLibrariesVHDL";
            this.listViewLibrariesVHDL.Size = new System.Drawing.Size(282, 343);
            this.listViewLibrariesVHDL.TabIndex = 0;
            this.listViewLibrariesVHDL.UseCompatibleStateImageBehavior = false;
            this.listViewLibrariesVHDL.View = System.Windows.Forms.View.Details;
            this.listViewLibrariesVHDL.SelectedIndexChanged += new System.EventHandler(this.listViewLibrariesVHDL_SelectedIndexChanged);
            // 
            // columnHeaderPath2
            // 
            this.columnHeaderPath2.Text = "Path";
            this.columnHeaderPath2.Width = 244;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.buttonDeleteVerilogLibrary);
            this.groupBox5.Controls.Add(this.buttonAddVerilogLibrary);
            this.groupBox5.Controls.Add(this.listViewLibrariesVerilog);
            this.groupBox5.Location = new System.Drawing.Point(3, 3);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(290, 368);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Verilog Libraries";
            // 
            // buttonDeleteVerilogLibrary
            // 
            this.buttonDeleteVerilogLibrary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDeleteVerilogLibrary.Enabled = false;
            this.buttonDeleteVerilogLibrary.Location = new System.Drawing.Point(128, 339);
            this.buttonDeleteVerilogLibrary.Name = "buttonDeleteVerilogLibrary";
            this.buttonDeleteVerilogLibrary.Size = new System.Drawing.Size(75, 23);
            this.buttonDeleteVerilogLibrary.TabIndex = 6;
            this.buttonDeleteVerilogLibrary.Text = "Delete";
            this.buttonDeleteVerilogLibrary.UseVisualStyleBackColor = true;
            this.buttonDeleteVerilogLibrary.Click += new System.EventHandler(this.buttonDeleteVerilogLibrary_Click);
            // 
            // buttonAddVerilogLibrary
            // 
            this.buttonAddVerilogLibrary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddVerilogLibrary.Location = new System.Drawing.Point(209, 339);
            this.buttonAddVerilogLibrary.Name = "buttonAddVerilogLibrary";
            this.buttonAddVerilogLibrary.Size = new System.Drawing.Size(75, 23);
            this.buttonAddVerilogLibrary.TabIndex = 5;
            this.buttonAddVerilogLibrary.Text = "Add";
            this.buttonAddVerilogLibrary.UseVisualStyleBackColor = true;
            this.buttonAddVerilogLibrary.Click += new System.EventHandler(this.buttonAddVerilogLibrary_Click);
            // 
            // listViewLibrariesVerilog
            // 
            this.listViewLibrariesVerilog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewLibrariesVerilog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderPath});
            this.listViewLibrariesVerilog.GridLines = true;
            this.listViewLibrariesVerilog.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewLibrariesVerilog.LabelEdit = true;
            this.listViewLibrariesVerilog.Location = new System.Drawing.Point(7, 19);
            this.listViewLibrariesVerilog.Name = "listViewLibrariesVerilog";
            this.listViewLibrariesVerilog.Size = new System.Drawing.Size(277, 314);
            this.listViewLibrariesVerilog.TabIndex = 1;
            this.listViewLibrariesVerilog.UseCompatibleStateImageBehavior = false;
            this.listViewLibrariesVerilog.View = System.Windows.Forms.View.Details;
            this.listViewLibrariesVerilog.SelectedIndexChanged += new System.EventHandler(this.listViewLibrariesVerilog_SelectedIndexChanged);
            // 
            // columnHeaderPath
            // 
            this.columnHeaderPath.Text = "Path";
            this.columnHeaderPath.Width = 239;
            // 
            // tabPageCompilerOptions
            // 
            this.tabPageCompilerOptions.Controls.Add(this.groupBox10);
            this.tabPageCompilerOptions.Controls.Add(this.groupBox9);
            this.tabPageCompilerOptions.Controls.Add(this.groupBox8);
            this.tabPageCompilerOptions.Controls.Add(this.groupBox7);
            this.tabPageCompilerOptions.Controls.Add(this.groupBox6);
            this.tabPageCompilerOptions.Controls.Add(this.groupBoxGHDLPath);
            this.tabPageCompilerOptions.Location = new System.Drawing.Point(4, 22);
            this.tabPageCompilerOptions.Name = "tabPageCompilerOptions";
            this.tabPageCompilerOptions.Size = new System.Drawing.Size(600, 469);
            this.tabPageCompilerOptions.TabIndex = 3;
            this.tabPageCompilerOptions.Text = "Compiler Options";
            this.tabPageCompilerOptions.UseVisualStyleBackColor = true;
            // 
            // groupBox9
            // 
            this.groupBox9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox9.Controls.Add(this.richTextBoxGHDLSimulateCommand);
            this.groupBox9.Location = new System.Drawing.Point(4, 312);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(588, 88);
            this.groupBox9.TabIndex = 4;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "GHDL Simulate Command Expression";
            // 
            // richTextBoxGHDLSimulateCommand
            // 
            this.richTextBoxGHDLSimulateCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxGHDLSimulateCommand.Location = new System.Drawing.Point(7, 20);
            this.richTextBoxGHDLSimulateCommand.Name = "richTextBoxGHDLSimulateCommand";
            this.richTextBoxGHDLSimulateCommand.Size = new System.Drawing.Size(575, 62);
            this.richTextBoxGHDLSimulateCommand.TabIndex = 0;
            this.richTextBoxGHDLSimulateCommand.Text = "{GHDL} {TbEntityName} {TbArchitectureName}  --vcd=\"{FilePath}\" --stack-size=128m " +
                "--stack-max-size=256m";
            // 
            // groupBox8
            // 
            this.groupBox8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox8.Controls.Add(this.richTextBoxGHDLCheckSyntaxCommand);
            this.groupBox8.Location = new System.Drawing.Point(4, 237);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(588, 69);
            this.groupBox8.TabIndex = 3;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "GHDL Check Syntax Command Expression";
            // 
            // richTextBoxGHDLCheckSyntaxCommand
            // 
            this.richTextBoxGHDLCheckSyntaxCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxGHDLCheckSyntaxCommand.Location = new System.Drawing.Point(7, 20);
            this.richTextBoxGHDLCheckSyntaxCommand.Name = "richTextBoxGHDLCheckSyntaxCommand";
            this.richTextBoxGHDLCheckSyntaxCommand.Size = new System.Drawing.Size(575, 43);
            this.richTextBoxGHDLCheckSyntaxCommand.TabIndex = 0;
            this.richTextBoxGHDLCheckSyntaxCommand.Text = "{GHDL} -s -fexplicit {filePath}";
            // 
            // groupBox7
            // 
            this.groupBox7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox7.Controls.Add(this.richTextBoxGHDLElaborateCommand);
            this.groupBox7.Location = new System.Drawing.Point(3, 162);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(588, 69);
            this.groupBox7.TabIndex = 2;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "GHDL Elaborate Command Expression";
            // 
            // richTextBoxGHDLElaborateCommand
            // 
            this.richTextBoxGHDLElaborateCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxGHDLElaborateCommand.Location = new System.Drawing.Point(7, 20);
            this.richTextBoxGHDLElaborateCommand.Name = "richTextBoxGHDLElaborateCommand";
            this.richTextBoxGHDLElaborateCommand.Size = new System.Drawing.Size(575, 43);
            this.richTextBoxGHDLElaborateCommand.TabIndex = 0;
            this.richTextBoxGHDLElaborateCommand.Text = "{GHDL} -e -fexplicit {TbArchitectureName}";
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this.richTextBoxGHDLCompileCommand);
            this.groupBox6.Location = new System.Drawing.Point(4, 89);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(587, 66);
            this.groupBox6.TabIndex = 1;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "GHDL Compile Command Expression";
            // 
            // richTextBoxGHDLCompileCommand
            // 
            this.richTextBoxGHDLCompileCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxGHDLCompileCommand.Location = new System.Drawing.Point(7, 19);
            this.richTextBoxGHDLCompileCommand.Name = "richTextBoxGHDLCompileCommand";
            this.richTextBoxGHDLCompileCommand.Size = new System.Drawing.Size(574, 41);
            this.richTextBoxGHDLCompileCommand.TabIndex = 0;
            this.richTextBoxGHDLCompileCommand.Text = "{GHDL} -a -fexplicit {filePath}";
            // 
            // groupBoxGHDLPath
            // 
            this.groupBoxGHDLPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxGHDLPath.Controls.Add(this.buttonGHDLPathBrowse);
            this.groupBoxGHDLPath.Controls.Add(this.textBoxGHDLPathSpecified);
            this.groupBoxGHDLPath.Controls.Add(this.textBoxGHDLPathEnvir);
            this.groupBoxGHDLPath.Controls.Add(this.radioButtonGHDLSpecifiedPath);
            this.groupBoxGHDLPath.Controls.Add(this.radioButtonGHDLEnvirPath);
            this.groupBoxGHDLPath.Location = new System.Drawing.Point(4, 4);
            this.groupBoxGHDLPath.Name = "groupBoxGHDLPath";
            this.groupBoxGHDLPath.Size = new System.Drawing.Size(593, 78);
            this.groupBoxGHDLPath.TabIndex = 0;
            this.groupBoxGHDLPath.TabStop = false;
            this.groupBoxGHDLPath.Text = "GHDL Path";
            // 
            // buttonGHDLPathBrowse
            // 
            this.buttonGHDLPathBrowse.Location = new System.Drawing.Point(512, 42);
            this.buttonGHDLPathBrowse.Name = "buttonGHDLPathBrowse";
            this.buttonGHDLPathBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonGHDLPathBrowse.TabIndex = 5;
            this.buttonGHDLPathBrowse.Text = "Browse...";
            this.buttonGHDLPathBrowse.UseVisualStyleBackColor = true;
            this.buttonGHDLPathBrowse.Click += new System.EventHandler(this.buttonGHDLPathBrowse_Click);
            // 
            // textBoxGHDLPathSpecified
            // 
            this.textBoxGHDLPathSpecified.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxGHDLPathSpecified.Location = new System.Drawing.Point(139, 44);
            this.textBoxGHDLPathSpecified.Name = "textBoxGHDLPathSpecified";
            this.textBoxGHDLPathSpecified.Size = new System.Drawing.Size(362, 20);
            this.textBoxGHDLPathSpecified.TabIndex = 4;
            this.textBoxGHDLPathSpecified.Text = "SpecifiedPath";
            // 
            // textBoxGHDLPathEnvir
            // 
            this.textBoxGHDLPathEnvir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxGHDLPathEnvir.Location = new System.Drawing.Point(139, 20);
            this.textBoxGHDLPathEnvir.Name = "textBoxGHDLPathEnvir";
            this.textBoxGHDLPathEnvir.ReadOnly = true;
            this.textBoxGHDLPathEnvir.Size = new System.Drawing.Size(448, 20);
            this.textBoxGHDLPathEnvir.TabIndex = 3;
            this.textBoxGHDLPathEnvir.Text = "GHDL Environment path";
            // 
            // radioButtonGHDLSpecifiedPath
            // 
            this.radioButtonGHDLSpecifiedPath.AutoSize = true;
            this.radioButtonGHDLSpecifiedPath.Location = new System.Drawing.Point(7, 44);
            this.radioButtonGHDLSpecifiedPath.Name = "radioButtonGHDLSpecifiedPath";
            this.radioButtonGHDLSpecifiedPath.Size = new System.Drawing.Size(94, 17);
            this.radioButtonGHDLSpecifiedPath.TabIndex = 2;
            this.radioButtonGHDLSpecifiedPath.TabStop = true;
            this.radioButtonGHDLSpecifiedPath.Text = "Specified Path";
            this.radioButtonGHDLSpecifiedPath.UseVisualStyleBackColor = true;
            // 
            // radioButtonGHDLEnvirPath
            // 
            this.radioButtonGHDLEnvirPath.AutoSize = true;
            this.radioButtonGHDLEnvirPath.Location = new System.Drawing.Point(7, 20);
            this.radioButtonGHDLEnvirPath.Name = "radioButtonGHDLEnvirPath";
            this.radioButtonGHDLEnvirPath.Size = new System.Drawing.Size(125, 17);
            this.radioButtonGHDLEnvirPath.TabIndex = 0;
            this.radioButtonGHDLEnvirPath.TabStop = true;
            this.radioButtonGHDLEnvirPath.Text = "Environment Variable";
            this.radioButtonGHDLEnvirPath.UseVisualStyleBackColor = true;
            // 
            // tabPageFSM
            // 
            this.tabPageFSM.Controls.Add(this.graphicsOptionsUserControlFSM);
            this.tabPageFSM.Location = new System.Drawing.Point(4, 22);
            this.tabPageFSM.Name = "tabPageFSM";
            this.tabPageFSM.Size = new System.Drawing.Size(600, 403);
            this.tabPageFSM.TabIndex = 4;
            this.tabPageFSM.Text = "FSM Options";
            this.tabPageFSM.UseVisualStyleBackColor = true;
            // 
            // graphicsOptionsUserControlFSM
            // 
            this.graphicsOptionsUserControlFSM.Location = new System.Drawing.Point(3, 3);
            this.graphicsOptionsUserControlFSM.Name = "graphicsOptionsUserControlFSM";
            this.graphicsOptionsUserControlFSM.Size = new System.Drawing.Size(402, 294);
            this.graphicsOptionsUserControlFSM.TabIndex = 0;
            // 
            // tabPageEntityDrawningOptions
            // 
            this.tabPageEntityDrawningOptions.Controls.Add(this.graphicsOptionsUserControlEntityDrawning);
            this.tabPageEntityDrawningOptions.Location = new System.Drawing.Point(4, 22);
            this.tabPageEntityDrawningOptions.Name = "tabPageEntityDrawningOptions";
            this.tabPageEntityDrawningOptions.Size = new System.Drawing.Size(600, 403);
            this.tabPageEntityDrawningOptions.TabIndex = 8;
            this.tabPageEntityDrawningOptions.Text = "Entity Drawning Options";
            this.tabPageEntityDrawningOptions.UseVisualStyleBackColor = true;
            // 
            // graphicsOptionsUserControlEntityDrawning
            // 
            this.graphicsOptionsUserControlEntityDrawning.Location = new System.Drawing.Point(3, 3);
            this.graphicsOptionsUserControlEntityDrawning.Name = "graphicsOptionsUserControlEntityDrawning";
            this.graphicsOptionsUserControlEntityDrawning.Size = new System.Drawing.Size(402, 294);
            this.graphicsOptionsUserControlEntityDrawning.TabIndex = 0;
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.Location = new System.Drawing.Point(464, 522);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 1;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(545, 522);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // folderBrowserDialogDefaultProjectLocation
            // 
            this.folderBrowserDialogDefaultProjectLocation.Description = "Select default project location";
            // 
            // buttonSetDefaultSettings
            // 
            this.buttonSetDefaultSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSetDefaultSettings.Location = new System.Drawing.Point(12, 522);
            this.buttonSetDefaultSettings.Name = "buttonSetDefaultSettings";
            this.buttonSetDefaultSettings.Size = new System.Drawing.Size(127, 23);
            this.buttonSetDefaultSettings.TabIndex = 3;
            this.buttonSetDefaultSettings.Text = "Set default settings";
            this.buttonSetDefaultSettings.UseVisualStyleBackColor = true;
            this.buttonSetDefaultSettings.Click += new System.EventHandler(this.buttonSetDefaultSettings_Click);
            // 
            // folderBrowserDialogLibraries
            // 
            this.folderBrowserDialogLibraries.Description = "Select Library Folder";
            this.folderBrowserDialogLibraries.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 13);
            this.label4.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 13);
            this.label5.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 13);
            this.label6.TabIndex = 0;
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.richTextBoxGHDLCleanCommand);
            this.groupBox10.Location = new System.Drawing.Point(4, 401);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(588, 65);
            this.groupBox10.TabIndex = 5;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "GHDL Clean Command";
            // 
            // richTextBoxGHDLCleanCommand
            // 
            this.richTextBoxGHDLCleanCommand.Location = new System.Drawing.Point(6, 19);
            this.richTextBoxGHDLCleanCommand.Name = "richTextBoxGHDLCleanCommand";
            this.richTextBoxGHDLCleanCommand.Size = new System.Drawing.Size(575, 40);
            this.richTextBoxGHDLCleanCommand.TabIndex = 0;
            this.richTextBoxGHDLCleanCommand.Text = "\"{GHDL}\" --remove";
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 548);
            this.Controls.Add(this.buttonSetDefaultSettings);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Options";
            this.Text = "Options";
            this.tabControl1.ResumeLayout(false);
            this.tabPageProject.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPageTextEditor.ResumeLayout(false);
            this.tabPageTextEditor.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPageLibraries.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.tabPageCompilerOptions.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBoxGHDLPath.ResumeLayout(false);
            this.groupBoxGHDLPath.PerformLayout();
            this.tabPageFSM.ResumeLayout(false);
            this.tabPageEntityDrawningOptions.ResumeLayout(false);
            this.groupBox10.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageProject;
        private System.Windows.Forms.TabPage tabPageTextEditor;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TabPage tabPageLibraries;
        private System.Windows.Forms.TabPage tabPageCompilerOptions;
        private System.Windows.Forms.TabPage tabPageFSM;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.TextBox textBoxDefaultProjectLocation;
        public System.Windows.Forms.Button buttonDefaultProjectLocation;
        public System.Windows.Forms.FolderBrowserDialog folderBrowserDialogDefaultProjectLocation;
        private System.Windows.Forms.Button buttonSetDefaultSettings;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxTextEditorSystemFonts;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxTextEditorFontSize;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBoxTextEditorUnderLine;
        private System.Windows.Forms.CheckBox checkBoxTextEditorStrikeout;
        private System.Windows.Forms.CheckBox checkBoxTextEditorItalic;
        private System.Windows.Forms.CheckBox checkBoxTextEditorBold;
        private System.Windows.Forms.ComboBox comboBoxTextEditorFontSizeUnits;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label labelTextEditorPreview;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ListView listViewLibrariesVHDL;
        private System.Windows.Forms.ListView listViewLibrariesVerilog;
        private System.Windows.Forms.Button buttonDeleteVHDLLibrary;
        private System.Windows.Forms.Button buttonAddVHDLLibrary;
        private System.Windows.Forms.Button buttonDeleteVerilogLibrary;
        private System.Windows.Forms.Button buttonAddVerilogLibrary;
        private System.Windows.Forms.ColumnHeader columnHeaderPath2;
        private System.Windows.Forms.ColumnHeader columnHeaderPath;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogLibraries;
        private System.Windows.Forms.TabPage tabPageEntityDrawningOptions;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private GraphicsOptionsUserControl graphicsOptionsUserControlFSM;
        private GraphicsOptionsUserControl graphicsOptionsUserControlEntityDrawning;
        private System.Windows.Forms.GroupBox groupBoxGHDLPath;
        private System.Windows.Forms.Button buttonGHDLPathBrowse;
        private System.Windows.Forms.TextBox textBoxGHDLPathSpecified;
        private System.Windows.Forms.TextBox textBoxGHDLPathEnvir;
        private System.Windows.Forms.RadioButton radioButtonGHDLSpecifiedPath;
        private System.Windows.Forms.RadioButton radioButtonGHDLEnvirPath;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.RichTextBox richTextBoxGHDLCompileCommand;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.RichTextBox richTextBoxGHDLElaborateCommand;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.RichTextBox richTextBoxGHDLCheckSyntaxCommand;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.RichTextBox richTextBoxGHDLSimulateCommand;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.RichTextBox richTextBoxGHDLCleanCommand;
    }
}

