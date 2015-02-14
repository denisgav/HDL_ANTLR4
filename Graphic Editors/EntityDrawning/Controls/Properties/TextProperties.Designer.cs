namespace Schematix.EntityDrawning
{
    partial class TextProperties
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
            this.richTextBoxText = new System.Windows.Forms.RichTextBox();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.buttonChangeFont = new System.Windows.Forms.Button();
            this.buttonChangeBrush = new System.Windows.Forms.Button();
            this.buttonChangePen = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.richTextBoxText);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(297, 190);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Text";
            // 
            // richTextBoxText
            // 
            this.richTextBoxText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxText.Location = new System.Drawing.Point(3, 16);
            this.richTextBoxText.Name = "richTextBoxText";
            this.richTextBoxText.Size = new System.Drawing.Size(291, 171);
            this.richTextBoxText.TabIndex = 0;
            this.richTextBoxText.Text = "";
            // 
            // buttonChangeFont
            // 
            this.buttonChangeFont.Location = new System.Drawing.Point(215, 209);
            this.buttonChangeFont.Name = "buttonChangeFont";
            this.buttonChangeFont.Size = new System.Drawing.Size(95, 23);
            this.buttonChangeFont.TabIndex = 1;
            this.buttonChangeFont.Text = "Change Font";
            this.buttonChangeFont.UseVisualStyleBackColor = true;
            this.buttonChangeFont.Click += new System.EventHandler(this.buttonChangeFont_Click);
            // 
            // buttonChangeBrush
            // 
            this.buttonChangeBrush.Location = new System.Drawing.Point(114, 209);
            this.buttonChangeBrush.Name = "buttonChangeBrush";
            this.buttonChangeBrush.Size = new System.Drawing.Size(95, 23);
            this.buttonChangeBrush.TabIndex = 2;
            this.buttonChangeBrush.Text = "Change Brush";
            this.buttonChangeBrush.UseVisualStyleBackColor = true;
            this.buttonChangeBrush.Click += new System.EventHandler(this.buttonChangeBrush_Click);
            // 
            // buttonChangePen
            // 
            this.buttonChangePen.Location = new System.Drawing.Point(13, 209);
            this.buttonChangePen.Name = "buttonChangePen";
            this.buttonChangePen.Size = new System.Drawing.Size(95, 23);
            this.buttonChangePen.TabIndex = 3;
            this.buttonChangePen.Text = "Change Pen";
            this.buttonChangePen.UseVisualStyleBackColor = true;
            this.buttonChangePen.Click += new System.EventHandler(this.buttonChangePen_Click);
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(114, 239);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(95, 23);
            this.buttonOk.TabIndex = 4;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(216, 239);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(94, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // TextProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(321, 273);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonChangePen);
            this.Controls.Add(this.buttonChangeBrush);
            this.Controls.Add(this.buttonChangeFont);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "TextProperties";
            this.Text = "TextProperties";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox richTextBoxText;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.Button buttonChangeFont;
        private System.Windows.Forms.Button buttonChangeBrush;
        private System.Windows.Forms.Button buttonChangePen;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
    }
}