namespace csx
{
    partial class redactorPL
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
            this.portName = new System.Windows.Forms.Label();
            this.lineName1 = new System.Windows.Forms.Label();
            this.lineName2 = new System.Windows.Forms.Label();
            this.vS = new System.Windows.Forms.VScrollBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.hS = new System.Windows.Forms.HScrollBar();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // portName
            // 
            this.portName.AutoSize = true;
            this.portName.Location = new System.Drawing.Point(12, 9);
            this.portName.Name = "portName";
            this.portName.Size = new System.Drawing.Size(35, 13);
            this.portName.TabIndex = 0;
            this.portName.Text = "label1";
            // 
            // lineName1
            // 
            this.lineName1.AutoSize = true;
            this.lineName1.Location = new System.Drawing.Point(12, 77);
            this.lineName1.Name = "lineName1";
            this.lineName1.Size = new System.Drawing.Size(35, 13);
            this.lineName1.TabIndex = 1;
            this.lineName1.Text = "label2";
            // 
            // lineName2
            // 
            this.lineName2.AutoSize = true;
            this.lineName2.Location = new System.Drawing.Point(227, 77);
            this.lineName2.Name = "lineName2";
            this.lineName2.Size = new System.Drawing.Size(35, 13);
            this.lineName2.TabIndex = 2;
            this.lineName2.Text = "label3";
            // 
            // vS
            // 
            this.vS.Dock = System.Windows.Forms.DockStyle.Right;
            this.vS.Location = new System.Drawing.Point(275, 0);
            this.vS.Name = "vS";
            this.vS.Size = new System.Drawing.Size(20, 255);
            this.vS.TabIndex = 3;
            this.vS.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vS_Scroll);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.hS);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 255);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(295, 22);
            this.panel1.TabIndex = 4;
            // 
            // hS
            // 
            this.hS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hS.Location = new System.Drawing.Point(0, 0);
            this.hS.Name = "hS";
            this.hS.Size = new System.Drawing.Size(275, 22);
            this.hS.TabIndex = 1;
            this.hS.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hS_Scroll);
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(275, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(20, 22);
            this.panel2.TabIndex = 0;
            // 
            // redactorPL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(295, 277);
            this.Controls.Add(this.vS);
            this.Controls.Add(this.lineName2);
            this.Controls.Add(this.lineName1);
            this.Controls.Add(this.portName);
            this.Controls.Add(this.panel1);
            this.Name = "redactorPL";
            this.Text = "Redactor";
            this.Load += new System.EventHandler(this.redactorPL_Load);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.redactorPL_MouseUp);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.redactorPL_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.redactorPL_MouseDown);
            this.Resize += new System.EventHandler(this.redactorPL_Resize);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.redactorPL_MouseMove);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label portName;
        private System.Windows.Forms.Label lineName1;
        private System.Windows.Forms.Label lineName2;
        private System.Windows.Forms.VScrollBar vS;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.HScrollBar hS;
        private System.Windows.Forms.Panel panel2;
    }
}