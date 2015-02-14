namespace Schematix.FSM
{
    partial class ConstantProperties
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
        	this.textBoxName = new System.Windows.Forms.TextBox();
        	this.label2 = new System.Windows.Forms.Label();
        	this.textBoxValue = new System.Windows.Forms.TextBox();
        	this.groupBox1 = new System.Windows.Forms.GroupBox();
        	this.radioButtonParameter = new System.Windows.Forms.RadioButton();
        	this.radioButtonGeneric = new System.Windows.Forms.RadioButton();
        	this.buttonOk = new System.Windows.Forms.Button();
        	this.buttonCancel = new System.Windows.Forms.Button();
        	this.groupBox1.SuspendLayout();
        	this.SuspendLayout();
        	// 
        	// label1
        	// 
        	this.label1.AutoSize = true;
        	this.label1.Location = new System.Drawing.Point(12, 16);
        	this.label1.Name = "label1";
        	this.label1.Size = new System.Drawing.Size(38, 13);
        	this.label1.TabIndex = 0;
        	this.label1.Text = "Name:";
        	// 
        	// textBoxName
        	// 
        	this.textBoxName.Location = new System.Drawing.Point(73, 13);
        	this.textBoxName.Name = "textBoxName";
        	this.textBoxName.Size = new System.Drawing.Size(100, 20);
        	this.textBoxName.TabIndex = 1;
        	// 
        	// label2
        	// 
        	this.label2.AutoSize = true;
        	this.label2.Location = new System.Drawing.Point(12, 43);
        	this.label2.Name = "label2";
        	this.label2.Size = new System.Drawing.Size(37, 13);
        	this.label2.TabIndex = 2;
        	this.label2.Text = "Value:";
        	// 
        	// textBoxValue
        	// 
        	this.textBoxValue.Location = new System.Drawing.Point(73, 40);
        	this.textBoxValue.Name = "textBoxValue";
        	this.textBoxValue.Size = new System.Drawing.Size(100, 20);
        	this.textBoxValue.TabIndex = 3;
        	// 
        	// groupBox1
        	// 
        	this.groupBox1.Controls.Add(this.radioButtonParameter);
        	this.groupBox1.Controls.Add(this.radioButtonGeneric);
        	this.groupBox1.Location = new System.Drawing.Point(179, 12);
        	this.groupBox1.Name = "groupBox1";
        	this.groupBox1.Size = new System.Drawing.Size(152, 48);
        	this.groupBox1.TabIndex = 4;
        	this.groupBox1.TabStop = false;
        	this.groupBox1.Text = "Constant Type";
        	// 
        	// radioButtonParameter
        	// 
        	this.radioButtonParameter.AutoSize = true;
        	this.radioButtonParameter.Location = new System.Drawing.Point(75, 20);
        	this.radioButtonParameter.Name = "radioButtonParameter";
        	this.radioButtonParameter.Size = new System.Drawing.Size(73, 17);
        	this.radioButtonParameter.TabIndex = 1;
        	this.radioButtonParameter.TabStop = true;
        	this.radioButtonParameter.Text = "Parameter";
        	this.radioButtonParameter.UseVisualStyleBackColor = true;
        	// 
        	// radioButtonGeneric
        	// 
        	this.radioButtonGeneric.AutoSize = true;
        	this.radioButtonGeneric.Location = new System.Drawing.Point(7, 20);
        	this.radioButtonGeneric.Name = "radioButtonGeneric";
        	this.radioButtonGeneric.Size = new System.Drawing.Size(62, 17);
        	this.radioButtonGeneric.TabIndex = 0;
        	this.radioButtonGeneric.TabStop = true;
        	this.radioButtonGeneric.Text = "Generic";
        	this.radioButtonGeneric.UseVisualStyleBackColor = true;
        	// 
        	// buttonOk
        	// 
        	this.buttonOk.Location = new System.Drawing.Point(173, 72);
        	this.buttonOk.Name = "buttonOk";
        	this.buttonOk.Size = new System.Drawing.Size(75, 23);
        	this.buttonOk.TabIndex = 5;
        	this.buttonOk.Text = "Ok";
        	this.buttonOk.UseVisualStyleBackColor = true;
        	this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
        	// 
        	// buttonCancel
        	// 
        	this.buttonCancel.Location = new System.Drawing.Point(254, 72);
        	this.buttonCancel.Name = "buttonCancel";
        	this.buttonCancel.Size = new System.Drawing.Size(75, 23);
        	this.buttonCancel.TabIndex = 6;
        	this.buttonCancel.Text = "Cancel";
        	this.buttonCancel.UseVisualStyleBackColor = true;
        	this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
        	// 
        	// ConstantProperties
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(339, 107);
        	this.Controls.Add(this.buttonCancel);
        	this.Controls.Add(this.buttonOk);
        	this.Controls.Add(this.groupBox1);
        	this.Controls.Add(this.textBoxValue);
        	this.Controls.Add(this.label2);
        	this.Controls.Add(this.textBoxName);
        	this.Controls.Add(this.label1);
        	this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
        	this.Name = "ConstantProperties";
        	this.Text = "Constant Properties";
        	this.groupBox1.ResumeLayout(false);
        	this.groupBox1.PerformLayout();
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxValue;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonParameter;
        private System.Windows.Forms.RadioButton radioButtonGeneric;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
    }
}