namespace csx
{
    partial class redactorPP
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
            this.vS = new System.Windows.Forms.VScrollBar();
            this.portName2 = new System.Windows.Forms.Label();
            this.portName1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // vS
            // 
            this.vS.Dock = System.Windows.Forms.DockStyle.Right;
            this.vS.LargeChange = 20;
            this.vS.Location = new System.Drawing.Point(280, 0);
            this.vS.Name = "vS";
            this.vS.Size = new System.Drawing.Size(20, 282);
            this.vS.TabIndex = 2;
            this.vS.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vS_Scroll);
            // 
            // portName2
            // 
            this.portName2.AutoSize = true;
            this.portName2.Dock = System.Windows.Forms.DockStyle.Right;
            this.portName2.Location = new System.Drawing.Point(280, 0);
            this.portName2.Name = "portName2";
            this.portName2.Size = new System.Drawing.Size(0, 13);
            this.portName2.TabIndex = 6;
            // 
            // portName1
            // 
            this.portName1.AutoSize = true;
            this.portName1.Dock = System.Windows.Forms.DockStyle.Left;
            this.portName1.Location = new System.Drawing.Point(0, 0);
            this.portName1.Name = "portName1";
            this.portName1.Size = new System.Drawing.Size(0, 13);
            this.portName1.TabIndex = 5;
            // 
            // redactorPP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 282);
            this.Controls.Add(this.portName2);
            this.Controls.Add(this.portName1);
            this.Controls.Add(this.vS);
            this.Name = "redactorPP";
            this.Text = "Redactor";
            this.Load += new System.EventHandler(this.redactorPP_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.redactorPP_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.redactorPP_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.redactorPP_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.redactorPP_MouseUp);
            this.Resize += new System.EventHandler(this.redactorPP_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.VScrollBar vS;
        private System.Windows.Forms.Label portName2;
        private System.Windows.Forms.Label portName1;
    }
}