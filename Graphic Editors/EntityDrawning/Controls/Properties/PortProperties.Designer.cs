namespace Schematix.EntityDrawning
{
    partial class PortProperties
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxInverse = new System.Windows.Forms.CheckBox();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.buttonChangePen = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.maskedTextBoxRightBound = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxLeftBound = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxIsBus = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxName);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(267, 42);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Port Name";
            // 
            // textBoxName
            // 
            this.textBoxName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxName.Location = new System.Drawing.Point(3, 16);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(261, 20);
            this.textBoxName.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBoxInverse);
            this.groupBox2.Controls.Add(this.comboBoxType);
            this.groupBox2.Location = new System.Drawing.Point(13, 56);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(267, 43);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Type";
            // 
            // checkBoxInverse
            // 
            this.checkBoxInverse.AutoSize = true;
            this.checkBoxInverse.Location = new System.Drawing.Point(200, 16);
            this.checkBoxInverse.Name = "checkBoxInverse";
            this.checkBoxInverse.Size = new System.Drawing.Size(61, 17);
            this.checkBoxInverse.TabIndex = 1;
            this.checkBoxInverse.Text = "Inverse";
            this.checkBoxInverse.UseVisualStyleBackColor = true;
            // 
            // comboBoxType
            // 
            this.comboBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Location = new System.Drawing.Point(7, 16);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(187, 21);
            this.comboBoxType.TabIndex = 0;
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(205, 206);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 2;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(124, 206);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 3;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // buttonChangePen
            // 
            this.buttonChangePen.Location = new System.Drawing.Point(13, 206);
            this.buttonChangePen.Name = "buttonChangePen";
            this.buttonChangePen.Size = new System.Drawing.Size(105, 23);
            this.buttonChangePen.TabIndex = 4;
            this.buttonChangePen.Text = "Change Pen";
            this.buttonChangePen.UseVisualStyleBackColor = true;
            this.buttonChangePen.Click += new System.EventHandler(this.buttonChangePen_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.maskedTextBoxRightBound);
            this.groupBox3.Controls.Add(this.maskedTextBoxLeftBound);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.checkBoxIsBus);
            this.groupBox3.Location = new System.Drawing.Point(13, 106);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(267, 94);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Bus Settings";
            // 
            // maskedTextBoxRightBound
            // 
            this.maskedTextBoxRightBound.Location = new System.Drawing.Point(111, 67);
            this.maskedTextBoxRightBound.Mask = "00000";
            this.maskedTextBoxRightBound.Name = "maskedTextBoxRightBound";
            this.maskedTextBoxRightBound.Size = new System.Drawing.Size(150, 20);
            this.maskedTextBoxRightBound.TabIndex = 4;
            // 
            // maskedTextBoxLeftBound
            // 
            this.maskedTextBoxLeftBound.Location = new System.Drawing.Point(111, 41);
            this.maskedTextBoxLeftBound.Mask = "00000";
            this.maskedTextBoxLeftBound.Name = "maskedTextBoxLeftBound";
            this.maskedTextBoxLeftBound.Size = new System.Drawing.Size(150, 20);
            this.maskedTextBoxLeftBound.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "right Bound";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "left Bound";
            // 
            // checkBoxIsBus
            // 
            this.checkBoxIsBus.AutoSize = true;
            this.checkBoxIsBus.Location = new System.Drawing.Point(7, 20);
            this.checkBoxIsBus.Name = "checkBoxIsBus";
            this.checkBoxIsBus.Size = new System.Drawing.Size(55, 17);
            this.checkBoxIsBus.TabIndex = 0;
            this.checkBoxIsBus.Text = "Is Bus";
            this.checkBoxIsBus.UseVisualStyleBackColor = true;
            // 
            // PortProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 241);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.buttonChangePen);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PortProperties";
            this.Text = "PortProperties";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.CheckBox checkBoxInverse;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button buttonChangePen;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBoxIsBus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxRightBound;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxLeftBound;
        private System.Windows.Forms.Label label2;
    }
}