namespace Schematix.FSM
{
    partial class Reset_Properties
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
        	this.buttonOk = new System.Windows.Forms.Button();
        	this.buttonCancel = new System.Windows.Forms.Button();
        	this.groupBox1 = new System.Windows.Forms.GroupBox();
        	this.richTextBoxCondition = new System.Windows.Forms.RichTextBox();
        	this.labelTrackingSignal = new System.Windows.Forms.Label();
        	this.comboBoxTrackingSignal = new System.Windows.Forms.ComboBox();
        	this.radioButtonAsynchonous = new System.Windows.Forms.RadioButton();
        	this.radioButtonSynchonous = new System.Windows.Forms.RadioButton();
        	this.groupBox1.SuspendLayout();
        	this.SuspendLayout();
        	// 
        	// buttonOk
        	// 
        	this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        	this.buttonOk.Location = new System.Drawing.Point(12, 225);
        	this.buttonOk.Name = "buttonOk";
        	this.buttonOk.Size = new System.Drawing.Size(75, 23);
        	this.buttonOk.TabIndex = 0;
        	this.buttonOk.Text = "Ok";
        	this.buttonOk.UseVisualStyleBackColor = true;
        	this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
        	// 
        	// buttonCancel
        	// 
        	this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        	this.buttonCancel.Location = new System.Drawing.Point(94, 225);
        	this.buttonCancel.Name = "buttonCancel";
        	this.buttonCancel.Size = new System.Drawing.Size(75, 23);
        	this.buttonCancel.TabIndex = 1;
        	this.buttonCancel.Text = "Cancel";
        	this.buttonCancel.UseVisualStyleBackColor = true;
        	this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
        	// 
        	// groupBox1
        	// 
        	this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.groupBox1.Controls.Add(this.richTextBoxCondition);
        	this.groupBox1.Location = new System.Drawing.Point(13, 13);
        	this.groupBox1.Name = "groupBox1";
        	this.groupBox1.Size = new System.Drawing.Size(321, 142);
        	this.groupBox1.TabIndex = 2;
        	this.groupBox1.TabStop = false;
        	this.groupBox1.Text = "Condition";
        	// 
        	// richTextBoxCondition
        	// 
        	this.richTextBoxCondition.Location = new System.Drawing.Point(7, 20);
        	this.richTextBoxCondition.Name = "richTextBoxCondition";
        	this.richTextBoxCondition.Size = new System.Drawing.Size(308, 116);
        	this.richTextBoxCondition.TabIndex = 0;
        	this.richTextBoxCondition.Text = "";
        	// 
        	// labelTrackingSignal
        	// 
        	this.labelTrackingSignal.AutoSize = true;
        	this.labelTrackingSignal.Location = new System.Drawing.Point(18, 165);
        	this.labelTrackingSignal.Name = "labelTrackingSignal";
        	this.labelTrackingSignal.Size = new System.Drawing.Size(81, 13);
        	this.labelTrackingSignal.TabIndex = 3;
        	this.labelTrackingSignal.Text = "Tracking Signal";
        	// 
        	// comboBoxTrackingSignal
        	// 
        	this.comboBoxTrackingSignal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        	this.comboBoxTrackingSignal.FormattingEnabled = true;
        	this.comboBoxTrackingSignal.Location = new System.Drawing.Point(105, 162);
        	this.comboBoxTrackingSignal.Name = "comboBoxTrackingSignal";
        	this.comboBoxTrackingSignal.Size = new System.Drawing.Size(229, 21);
        	this.comboBoxTrackingSignal.TabIndex = 4;
        	// 
        	// radioButtonAsynchonous
        	// 
        	this.radioButtonAsynchonous.AutoSize = true;
        	this.radioButtonAsynchonous.Location = new System.Drawing.Point(20, 191);
        	this.radioButtonAsynchonous.Name = "radioButtonAsynchonous";
        	this.radioButtonAsynchonous.Size = new System.Drawing.Size(89, 17);
        	this.radioButtonAsynchonous.TabIndex = 5;
        	this.radioButtonAsynchonous.TabStop = true;
        	this.radioButtonAsynchonous.Text = "Asynchonous";
        	this.radioButtonAsynchonous.UseVisualStyleBackColor = true;
        	// 
        	// radioButtonSynchonous
        	// 
        	this.radioButtonSynchonous.AutoSize = true;
        	this.radioButtonSynchonous.Location = new System.Drawing.Point(124, 191);
        	this.radioButtonSynchonous.Name = "radioButtonSynchonous";
        	this.radioButtonSynchonous.Size = new System.Drawing.Size(84, 17);
        	this.radioButtonSynchonous.TabIndex = 6;
        	this.radioButtonSynchonous.TabStop = true;
        	this.radioButtonSynchonous.Text = "Synchonous";
        	this.radioButtonSynchonous.UseVisualStyleBackColor = true;
        	// 
        	// Reset_Properties
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(346, 260);
        	this.Controls.Add(this.radioButtonSynchonous);
        	this.Controls.Add(this.radioButtonAsynchonous);
        	this.Controls.Add(this.comboBoxTrackingSignal);
        	this.Controls.Add(this.labelTrackingSignal);
        	this.Controls.Add(this.groupBox1);
        	this.Controls.Add(this.buttonCancel);
        	this.Controls.Add(this.buttonOk);
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
        	this.Name = "Reset_Properties";
        	this.Text = "Reset Properties";
        	this.groupBox1.ResumeLayout(false);
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox richTextBoxCondition;
        private System.Windows.Forms.Label labelTrackingSignal;
        private System.Windows.Forms.ComboBox comboBoxTrackingSignal;
        private System.Windows.Forms.RadioButton radioButtonAsynchonous;
        private System.Windows.Forms.RadioButton radioButtonSynchonous;
    }
}