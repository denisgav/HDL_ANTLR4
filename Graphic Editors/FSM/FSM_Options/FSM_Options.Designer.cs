namespace Schematix.FSM
{
    partial class FSM_Options
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
        	this.numericUpDownNumStates = new System.Windows.Forms.NumericUpDown();
        	this.label2 = new System.Windows.Forms.Label();
        	this.comboBoxStatesLayout = new System.Windows.Forms.ComboBox();
        	this.label3 = new System.Windows.Forms.Label();
        	this.comboBoxTransition = new System.Windows.Forms.ComboBox();
        	this.label4 = new System.Windows.Forms.Label();
        	this.comboBoxResetState = new System.Windows.Forms.ComboBox();
        	this.groupBox1 = new System.Windows.Forms.GroupBox();
        	this.buttonDefaultDUH = new System.Windows.Forms.Button();
        	this.richTextBoxDesignUnitHeader = new System.Windows.Forms.RichTextBox();
        	this.buttonCancel = new System.Windows.Forms.Button();
        	this.buttonOk = new System.Windows.Forms.Button();
        	((System.ComponentModel.ISupportInitialize)(this.numericUpDownNumStates)).BeginInit();
        	this.groupBox1.SuspendLayout();
        	this.SuspendLayout();
        	// 
        	// label1
        	// 
        	this.label1.AutoSize = true;
        	this.label1.Location = new System.Drawing.Point(12, 13);
        	this.label1.Name = "label1";
        	this.label1.Size = new System.Drawing.Size(129, 13);
        	this.label1.TabIndex = 0;
        	this.label1.Text = "Number of states (0 - 20) :";
        	// 
        	// numericUpDownNumStates
        	// 
        	this.numericUpDownNumStates.Location = new System.Drawing.Point(148, 11);
        	this.numericUpDownNumStates.Maximum = new decimal(new int[] {
        	        	        	20,
        	        	        	0,
        	        	        	0,
        	        	        	0});
        	this.numericUpDownNumStates.Name = "numericUpDownNumStates";
        	this.numericUpDownNumStates.Size = new System.Drawing.Size(95, 20);
        	this.numericUpDownNumStates.TabIndex = 1;
        	this.numericUpDownNumStates.ValueChanged += new System.EventHandler(this.numericUpDownNumStates_ValueChanged);
        	// 
        	// label2
        	// 
        	this.label2.AutoSize = true;
        	this.label2.Location = new System.Drawing.Point(12, 40);
        	this.label2.Name = "label2";
        	this.label2.Size = new System.Drawing.Size(71, 13);
        	this.label2.TabIndex = 2;
        	this.label2.Text = "States layout:";
        	// 
        	// comboBoxStatesLayout
        	// 
        	this.comboBoxStatesLayout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        	this.comboBoxStatesLayout.FormattingEnabled = true;
        	this.comboBoxStatesLayout.Items.AddRange(new object[] {
        	        	        	"Circular",
        	        	        	"Linear Horisontal",
        	        	        	"Linear Vertical"});
        	this.comboBoxStatesLayout.Location = new System.Drawing.Point(103, 40);
        	this.comboBoxStatesLayout.Name = "comboBoxStatesLayout";
        	this.comboBoxStatesLayout.Size = new System.Drawing.Size(140, 21);
        	this.comboBoxStatesLayout.TabIndex = 3;
        	this.comboBoxStatesLayout.SelectedIndexChanged += new System.EventHandler(this.comboBoxStatesLayout_SelectedIndexChanged);
        	// 
        	// label3
        	// 
        	this.label3.AutoSize = true;
        	this.label3.Location = new System.Drawing.Point(12, 71);
        	this.label3.Name = "label3";
        	this.label3.Size = new System.Drawing.Size(59, 13);
        	this.label3.TabIndex = 4;
        	this.label3.Text = "Transition :";
        	// 
        	// comboBoxTransition
        	// 
        	this.comboBoxTransition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        	this.comboBoxTransition.FormattingEnabled = true;
        	this.comboBoxTransition.Items.AddRange(new object[] {
        	        	        	"None",
        	        	        	"Forward",
        	        	        	"Backward",
        	        	        	"Both"});
        	this.comboBoxTransition.Location = new System.Drawing.Point(103, 68);
        	this.comboBoxTransition.Name = "comboBoxTransition";
        	this.comboBoxTransition.Size = new System.Drawing.Size(140, 21);
        	this.comboBoxTransition.TabIndex = 5;
        	this.comboBoxTransition.SelectedIndexChanged += new System.EventHandler(this.comboBoxTransition_SelectedIndexChanged);
        	// 
        	// label4
        	// 
        	this.label4.AutoSize = true;
        	this.label4.Location = new System.Drawing.Point(12, 102);
        	this.label4.Name = "label4";
        	this.label4.Size = new System.Drawing.Size(67, 13);
        	this.label4.TabIndex = 6;
        	this.label4.Text = "Reset state :";
        	// 
        	// comboBoxResetState
        	// 
        	this.comboBoxResetState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        	this.comboBoxResetState.FormattingEnabled = true;
        	this.comboBoxResetState.Items.AddRange(new object[] {
        	        	        	"none"});
        	this.comboBoxResetState.Location = new System.Drawing.Point(103, 99);
        	this.comboBoxResetState.Name = "comboBoxResetState";
        	this.comboBoxResetState.Size = new System.Drawing.Size(140, 21);
        	this.comboBoxResetState.TabIndex = 7;
        	this.comboBoxResetState.SelectedIndexChanged += new System.EventHandler(this.comboBoxResetState_SelectedIndexChanged);
        	// 
        	// groupBox1
        	// 
        	this.groupBox1.Controls.Add(this.buttonDefaultDUH);
        	this.groupBox1.Controls.Add(this.richTextBoxDesignUnitHeader);
        	this.groupBox1.Location = new System.Drawing.Point(15, 136);
        	this.groupBox1.Name = "groupBox1";
        	this.groupBox1.Size = new System.Drawing.Size(246, 190);
        	this.groupBox1.TabIndex = 8;
        	this.groupBox1.TabStop = false;
        	this.groupBox1.Text = "Design Unit Header";
        	// 
        	// buttonDefaultDUH
        	// 
        	this.buttonDefaultDUH.Location = new System.Drawing.Point(88, 158);
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
        	this.richTextBoxDesignUnitHeader.Size = new System.Drawing.Size(234, 132);
        	this.richTextBoxDesignUnitHeader.TabIndex = 0;
        	this.richTextBoxDesignUnitHeader.Text = "";
        	// 
        	// buttonCancel
        	// 
        	this.buttonCancel.Location = new System.Drawing.Point(186, 332);
        	this.buttonCancel.Name = "buttonCancel";
        	this.buttonCancel.Size = new System.Drawing.Size(75, 23);
        	this.buttonCancel.TabIndex = 10;
        	this.buttonCancel.Text = "Cancel";
        	this.buttonCancel.UseVisualStyleBackColor = true;
        	this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
        	// 
        	// buttonOk
        	// 
        	this.buttonOk.Location = new System.Drawing.Point(105, 331);
        	this.buttonOk.Name = "buttonOk";
        	this.buttonOk.Size = new System.Drawing.Size(75, 23);
        	this.buttonOk.TabIndex = 11;
        	this.buttonOk.Text = "Ok";
        	this.buttonOk.UseVisualStyleBackColor = true;
        	this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
        	// 
        	// FSM_Options
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(269, 359);
        	this.Controls.Add(this.buttonOk);
        	this.Controls.Add(this.buttonCancel);
        	this.Controls.Add(this.groupBox1);
        	this.Controls.Add(this.comboBoxResetState);
        	this.Controls.Add(this.label4);
        	this.Controls.Add(this.comboBoxTransition);
        	this.Controls.Add(this.label3);
        	this.Controls.Add(this.comboBoxStatesLayout);
        	this.Controls.Add(this.label2);
        	this.Controls.Add(this.numericUpDownNumStates);
        	this.Controls.Add(this.label1);
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        	this.MaximizeBox = false;
        	this.MinimizeBox = false;
        	this.Name = "FSM_Options";
        	this.Text = "FSM Options";
        	this.Load += new System.EventHandler(this.FSM_Options_Load);
        	((System.ComponentModel.ISupportInitialize)(this.numericUpDownNumStates)).EndInit();
        	this.groupBox1.ResumeLayout(false);
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownNumStates;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxStatesLayout;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxTransition;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxResetState;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox richTextBoxDesignUnitHeader;
        private System.Windows.Forms.Button buttonDefaultDUH;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
    }
}