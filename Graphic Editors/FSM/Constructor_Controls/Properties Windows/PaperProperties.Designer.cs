namespace Schematix.FSM
{
    partial class PaperProperties
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
        	this.buttonChangeBGColor = new System.Windows.Forms.Button();
        	this.labelBGColor = new System.Windows.Forms.Label();
        	this.groupBoxColor = new System.Windows.Forms.GroupBox();
        	this.buttonChangeLineColor = new System.Windows.Forms.Button();
        	this.labelLineColor = new System.Windows.Forms.Label();
        	this.groupBox1 = new System.Windows.Forms.GroupBox();
        	this.buttonOk = new System.Windows.Forms.Button();
        	this.buttonCancel = new System.Windows.Forms.Button();
        	this.colorDialogBG = new System.Windows.Forms.ColorDialog();
        	this.colorDialogLine = new System.Windows.Forms.ColorDialog();
        	this.tabControl1 = new System.Windows.Forms.TabControl();
        	this.tabPage1 = new System.Windows.Forms.TabPage();
        	this.checkBoxShowGrid = new System.Windows.Forms.CheckBox();
        	this.checkBoxShowBorder = new System.Windows.Forms.CheckBox();
        	this.tabPageVHDLModule = new System.Windows.Forms.TabPage();
        	this.groupBox5 = new System.Windows.Forms.GroupBox();
        	this.textBoxVHDLArchitectureName = new System.Windows.Forms.TextBox();
        	this.groupBox4 = new System.Windows.Forms.GroupBox();
        	this.textBoxVHDLEntityName = new System.Windows.Forms.TextBox();
        	this.tabPageVerilogModule = new System.Windows.Forms.TabPage();
        	this.groupBox3 = new System.Windows.Forms.GroupBox();
        	this.buttonDefaultDUH = new System.Windows.Forms.Button();
        	this.richTextBoxDesignUnitHeader = new System.Windows.Forms.RichTextBox();
        	this.groupBox2 = new System.Windows.Forms.GroupBox();
        	this.textBoxVerilogModuleName = new System.Windows.Forms.TextBox();
        	this.groupBoxColor.SuspendLayout();
        	this.groupBox1.SuspendLayout();
        	this.tabControl1.SuspendLayout();
        	this.tabPage1.SuspendLayout();
        	this.tabPageVHDLModule.SuspendLayout();
        	this.groupBox5.SuspendLayout();
        	this.groupBox4.SuspendLayout();
        	this.tabPageVerilogModule.SuspendLayout();
        	this.groupBox3.SuspendLayout();
        	this.groupBox2.SuspendLayout();
        	this.SuspendLayout();
        	// 
        	// buttonChangeBGColor
        	// 
        	this.buttonChangeBGColor.Location = new System.Drawing.Point(12, 40);
        	this.buttonChangeBGColor.Name = "buttonChangeBGColor";
        	this.buttonChangeBGColor.Size = new System.Drawing.Size(75, 23);
        	this.buttonChangeBGColor.TabIndex = 1;
        	this.buttonChangeBGColor.Text = "Color...";
        	this.buttonChangeBGColor.UseVisualStyleBackColor = true;
        	this.buttonChangeBGColor.Click += new System.EventHandler(this.buttonChangeBGColor_Click);
        	// 
        	// labelBGColor
        	// 
        	this.labelBGColor.AutoSize = true;
        	this.labelBGColor.Location = new System.Drawing.Point(7, 20);
        	this.labelBGColor.Name = "labelBGColor";
        	this.labelBGColor.Size = new System.Drawing.Size(0, 13);
        	this.labelBGColor.TabIndex = 0;
        	// 
        	// groupBoxColor
        	// 
        	this.groupBoxColor.Controls.Add(this.buttonChangeBGColor);
        	this.groupBoxColor.Controls.Add(this.labelBGColor);
        	this.groupBoxColor.Location = new System.Drawing.Point(6, 6);
        	this.groupBoxColor.Name = "groupBoxColor";
        	this.groupBoxColor.Size = new System.Drawing.Size(356, 69);
        	this.groupBoxColor.TabIndex = 2;
        	this.groupBoxColor.TabStop = false;
        	this.groupBoxColor.Text = "Background color";
        	// 
        	// buttonChangeLineColor
        	// 
        	this.buttonChangeLineColor.Location = new System.Drawing.Point(12, 40);
        	this.buttonChangeLineColor.Name = "buttonChangeLineColor";
        	this.buttonChangeLineColor.Size = new System.Drawing.Size(75, 23);
        	this.buttonChangeLineColor.TabIndex = 1;
        	this.buttonChangeLineColor.Text = "Color...";
        	this.buttonChangeLineColor.UseVisualStyleBackColor = true;
        	this.buttonChangeLineColor.Click += new System.EventHandler(this.buttonChangeLineColor_Click);
        	// 
        	// labelLineColor
        	// 
        	this.labelLineColor.AutoSize = true;
        	this.labelLineColor.Location = new System.Drawing.Point(7, 20);
        	this.labelLineColor.Name = "labelLineColor";
        	this.labelLineColor.Size = new System.Drawing.Size(0, 13);
        	this.labelLineColor.TabIndex = 0;
        	// 
        	// groupBox1
        	// 
        	this.groupBox1.Controls.Add(this.buttonChangeLineColor);
        	this.groupBox1.Controls.Add(this.labelLineColor);
        	this.groupBox1.Location = new System.Drawing.Point(6, 81);
        	this.groupBox1.Name = "groupBox1";
        	this.groupBox1.Size = new System.Drawing.Size(356, 69);
        	this.groupBox1.TabIndex = 3;
        	this.groupBox1.TabStop = false;
        	this.groupBox1.Text = "Line color";
        	// 
        	// buttonOk
        	// 
        	this.buttonOk.Location = new System.Drawing.Point(238, 266);
        	this.buttonOk.Name = "buttonOk";
        	this.buttonOk.Size = new System.Drawing.Size(75, 23);
        	this.buttonOk.TabIndex = 8;
        	this.buttonOk.Text = "Ok";
        	this.buttonOk.UseVisualStyleBackColor = true;
        	this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
        	// 
        	// buttonCancel
        	// 
        	this.buttonCancel.Location = new System.Drawing.Point(319, 266);
        	this.buttonCancel.Name = "buttonCancel";
        	this.buttonCancel.Size = new System.Drawing.Size(75, 23);
        	this.buttonCancel.TabIndex = 9;
        	this.buttonCancel.Text = "Cancel";
        	this.buttonCancel.UseVisualStyleBackColor = true;
        	this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
        	// 
        	// tabControl1
        	// 
        	this.tabControl1.Controls.Add(this.tabPage1);
        	this.tabControl1.Controls.Add(this.tabPageVHDLModule);
        	this.tabControl1.Controls.Add(this.tabPageVerilogModule);
        	this.tabControl1.Location = new System.Drawing.Point(12, 12);
        	this.tabControl1.Name = "tabControl1";
        	this.tabControl1.SelectedIndex = 0;
        	this.tabControl1.Size = new System.Drawing.Size(384, 248);
        	this.tabControl1.TabIndex = 10;
        	// 
        	// tabPage1
        	// 
        	this.tabPage1.Controls.Add(this.checkBoxShowGrid);
        	this.tabPage1.Controls.Add(this.checkBoxShowBorder);
        	this.tabPage1.Controls.Add(this.groupBoxColor);
        	this.tabPage1.Controls.Add(this.groupBox1);
        	this.tabPage1.Location = new System.Drawing.Point(4, 22);
        	this.tabPage1.Name = "tabPage1";
        	this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
        	this.tabPage1.Size = new System.Drawing.Size(376, 222);
        	this.tabPage1.TabIndex = 0;
        	this.tabPage1.Text = "General";
        	this.tabPage1.UseVisualStyleBackColor = true;
        	// 
        	// checkBoxShowGrid
        	// 
        	this.checkBoxShowGrid.AutoSize = true;
        	this.checkBoxShowGrid.Checked = true;
        	this.checkBoxShowGrid.CheckState = System.Windows.Forms.CheckState.Checked;
        	this.checkBoxShowGrid.Location = new System.Drawing.Point(115, 153);
        	this.checkBoxShowGrid.Name = "checkBoxShowGrid";
        	this.checkBoxShowGrid.Size = new System.Drawing.Size(75, 17);
        	this.checkBoxShowGrid.TabIndex = 8;
        	this.checkBoxShowGrid.Text = "Show Grid";
        	this.checkBoxShowGrid.UseVisualStyleBackColor = true;
        	// 
        	// checkBoxShowBorder
        	// 
        	this.checkBoxShowBorder.AutoSize = true;
        	this.checkBoxShowBorder.Checked = true;
        	this.checkBoxShowBorder.CheckState = System.Windows.Forms.CheckState.Checked;
        	this.checkBoxShowBorder.Location = new System.Drawing.Point(7, 153);
        	this.checkBoxShowBorder.Name = "checkBoxShowBorder";
        	this.checkBoxShowBorder.Size = new System.Drawing.Size(87, 17);
        	this.checkBoxShowBorder.TabIndex = 7;
        	this.checkBoxShowBorder.Text = "Show Border";
        	this.checkBoxShowBorder.UseVisualStyleBackColor = true;
        	// 
        	// tabPageVHDLModule
        	// 
        	this.tabPageVHDLModule.Controls.Add(this.groupBox5);
        	this.tabPageVHDLModule.Controls.Add(this.groupBox4);
        	this.tabPageVHDLModule.Location = new System.Drawing.Point(4, 22);
        	this.tabPageVHDLModule.Name = "tabPageVHDLModule";
        	this.tabPageVHDLModule.Padding = new System.Windows.Forms.Padding(3);
        	this.tabPageVHDLModule.Size = new System.Drawing.Size(376, 222);
        	this.tabPageVHDLModule.TabIndex = 1;
        	this.tabPageVHDLModule.Text = "VHDL module";
        	this.tabPageVHDLModule.UseVisualStyleBackColor = true;
        	// 
        	// groupBox5
        	// 
        	this.groupBox5.Controls.Add(this.textBoxVHDLArchitectureName);
        	this.groupBox5.Location = new System.Drawing.Point(7, 64);
        	this.groupBox5.Name = "groupBox5";
        	this.groupBox5.Size = new System.Drawing.Size(363, 51);
        	this.groupBox5.TabIndex = 1;
        	this.groupBox5.TabStop = false;
        	this.groupBox5.Text = "Architecture Name";
        	// 
        	// textBoxVHDLArchitectureName
        	// 
        	this.textBoxVHDLArchitectureName.Location = new System.Drawing.Point(7, 20);
        	this.textBoxVHDLArchitectureName.Name = "textBoxVHDLArchitectureName";
        	this.textBoxVHDLArchitectureName.Size = new System.Drawing.Size(350, 20);
        	this.textBoxVHDLArchitectureName.TabIndex = 0;
        	// 
        	// groupBox4
        	// 
        	this.groupBox4.Controls.Add(this.textBoxVHDLEntityName);
        	this.groupBox4.Location = new System.Drawing.Point(7, 7);
        	this.groupBox4.Name = "groupBox4";
        	this.groupBox4.Size = new System.Drawing.Size(363, 50);
        	this.groupBox4.TabIndex = 0;
        	this.groupBox4.TabStop = false;
        	this.groupBox4.Text = "Entity Name";
        	// 
        	// textBoxVHDLEntityName
        	// 
        	this.textBoxVHDLEntityName.Location = new System.Drawing.Point(6, 19);
        	this.textBoxVHDLEntityName.Name = "textBoxVHDLEntityName";
        	this.textBoxVHDLEntityName.Size = new System.Drawing.Size(350, 20);
        	this.textBoxVHDLEntityName.TabIndex = 2;
        	// 
        	// tabPageVerilogModule
        	// 
        	this.tabPageVerilogModule.Controls.Add(this.groupBox3);
        	this.tabPageVerilogModule.Controls.Add(this.groupBox2);
        	this.tabPageVerilogModule.Location = new System.Drawing.Point(4, 22);
        	this.tabPageVerilogModule.Name = "tabPageVerilogModule";
        	this.tabPageVerilogModule.Size = new System.Drawing.Size(376, 222);
        	this.tabPageVerilogModule.TabIndex = 2;
        	this.tabPageVerilogModule.Text = "Verilog module";
        	this.tabPageVerilogModule.UseVisualStyleBackColor = true;
        	// 
        	// groupBox3
        	// 
        	this.groupBox3.Controls.Add(this.buttonDefaultDUH);
        	this.groupBox3.Controls.Add(this.richTextBoxDesignUnitHeader);
        	this.groupBox3.Location = new System.Drawing.Point(4, 66);
        	this.groupBox3.Name = "groupBox3";
        	this.groupBox3.Size = new System.Drawing.Size(369, 139);
        	this.groupBox3.TabIndex = 9;
        	this.groupBox3.TabStop = false;
        	this.groupBox3.Text = "Design Unit Header";
        	// 
        	// buttonDefaultDUH
        	// 
        	this.buttonDefaultDUH.Location = new System.Drawing.Point(211, 107);
        	this.buttonDefaultDUH.Name = "buttonDefaultDUH";
        	this.buttonDefaultDUH.Size = new System.Drawing.Size(152, 23);
        	this.buttonDefaultDUH.TabIndex = 1;
        	this.buttonDefaultDUH.Text = "Default Design Unit Header";
        	this.buttonDefaultDUH.UseVisualStyleBackColor = true;
        	this.buttonDefaultDUH.Click += new System.EventHandler(this.buttonDefaultDUH_Click);
        	// 
        	// richTextBoxDesignUnitHeader
        	// 
        	this.richTextBoxDesignUnitHeader.Location = new System.Drawing.Point(6, 19);
        	this.richTextBoxDesignUnitHeader.Name = "richTextBoxDesignUnitHeader";
        	this.richTextBoxDesignUnitHeader.Size = new System.Drawing.Size(357, 82);
        	this.richTextBoxDesignUnitHeader.TabIndex = 0;
        	this.richTextBoxDesignUnitHeader.Text = "";
        	// 
        	// groupBox2
        	// 
        	this.groupBox2.Controls.Add(this.textBoxVerilogModuleName);
        	this.groupBox2.Location = new System.Drawing.Point(4, 4);
        	this.groupBox2.Name = "groupBox2";
        	this.groupBox2.Size = new System.Drawing.Size(369, 56);
        	this.groupBox2.TabIndex = 0;
        	this.groupBox2.TabStop = false;
        	this.groupBox2.Text = "Module Name:";
        	// 
        	// textBoxVerilogModuleName
        	// 
        	this.textBoxVerilogModuleName.Location = new System.Drawing.Point(7, 20);
        	this.textBoxVerilogModuleName.Name = "textBoxVerilogModuleName";
        	this.textBoxVerilogModuleName.Size = new System.Drawing.Size(356, 20);
        	this.textBoxVerilogModuleName.TabIndex = 0;
        	// 
        	// PaperProperties
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(402, 297);
        	this.Controls.Add(this.tabControl1);
        	this.Controls.Add(this.buttonCancel);
        	this.Controls.Add(this.buttonOk);
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
        	this.Name = "PaperProperties";
        	this.Text = "Paper Properties";
        	this.groupBoxColor.ResumeLayout(false);
        	this.groupBoxColor.PerformLayout();
        	this.groupBox1.ResumeLayout(false);
        	this.groupBox1.PerformLayout();
        	this.tabControl1.ResumeLayout(false);
        	this.tabPage1.ResumeLayout(false);
        	this.tabPage1.PerformLayout();
        	this.tabPageVHDLModule.ResumeLayout(false);
        	this.groupBox5.ResumeLayout(false);
        	this.groupBox5.PerformLayout();
        	this.groupBox4.ResumeLayout(false);
        	this.groupBox4.PerformLayout();
        	this.tabPageVerilogModule.ResumeLayout(false);
        	this.groupBox3.ResumeLayout(false);
        	this.groupBox2.ResumeLayout(false);
        	this.groupBox2.PerformLayout();
        	this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button buttonChangeBGColor;
        private System.Windows.Forms.Label labelBGColor;
        private System.Windows.Forms.GroupBox groupBoxColor;
        private System.Windows.Forms.Button buttonChangeLineColor;
        private System.Windows.Forms.Label labelLineColor;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ColorDialog colorDialogBG;
        private System.Windows.Forms.ColorDialog colorDialogLine;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPageVHDLModule;
        private System.Windows.Forms.TabPage tabPageVerilogModule;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxVerilogModuleName;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox textBoxVHDLArchitectureName;
        private System.Windows.Forms.TextBox textBoxVHDLEntityName;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button buttonDefaultDUH;
        private System.Windows.Forms.RichTextBox richTextBoxDesignUnitHeader;
        private System.Windows.Forms.CheckBox checkBoxShowGrid;
        private System.Windows.Forms.CheckBox checkBoxShowBorder;
    }
}