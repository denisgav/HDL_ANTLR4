namespace Schematix_all
{
    partial class SchemaUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SchemaUserControl));
            this.popupTimer = new System.Windows.Forms.Timer(this.components);
            this.popTimer = new System.Windows.Forms.Timer(this.components);
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.m_AddSignal = new System.Windows.Forms.ToolStripMenuItem();
            this.m_AddPort = new System.Windows.Forms.ToolStripMenuItem();
            this.m_ImportElements = new System.Windows.Forms.ToolStripMenuItem();
            this.m_GenerateCode = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // popupTimer
            // 
            this.popupTimer.Interval = 1000;
            this.popupTimer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // popTimer
            // 
            this.popTimer.Interval = 5000;
            this.popTimer.Tick += new System.EventHandler(this.popTimer_Tick);
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar1.Location = new System.Drawing.Point(445, 25);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(17, 413);
            this.vScrollBar1.TabIndex = 5;
            this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hScrollBar1.Location = new System.Drawing.Point(0, 421);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(445, 17);
            this.hScrollBar1.TabIndex = 6;
            this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_AddSignal,
            this.m_AddPort,
            this.m_ImportElements,
            this.m_GenerateCode});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(462, 25);
            this.toolStrip2.TabIndex = 3;
            this.toolStrip2.Text = "toolStrip2";
            this.toolStrip2.Visible = false;
            // 
            // m_AddSignal
            // 
            this.m_AddSignal.Image = ((System.Drawing.Image)(resources.GetObject("m_AddSignal.Image")));
            this.m_AddSignal.ImageTransparentColor = System.Drawing.SystemColors.ButtonFace;
            this.m_AddSignal.Name = "m_AddSignal";
            this.m_AddSignal.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.S)));
            this.m_AddSignal.Size = new System.Drawing.Size(100, 25);
            this.m_AddSignal.Text = "Add &signal...";
            this.m_AddSignal.Click += new System.EventHandler(this.m_AddSignal_Click);
            // 
            // m_AddPort
            // 
            this.m_AddPort.Image = ((System.Drawing.Image)(resources.GetObject("m_AddPort.Image")));
            this.m_AddPort.ImageTransparentColor = System.Drawing.SystemColors.ButtonFace;
            this.m_AddPort.Name = "m_AddPort";
            this.m_AddPort.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.P)));
            this.m_AddPort.Size = new System.Drawing.Size(91, 25);
            this.m_AddPort.Text = "Add por&t...";
            this.m_AddPort.Click += new System.EventHandler(this.m_AddPort_Click);
            // 
            // m_ImportElements
            // 
            this.m_ImportElements.Image = ((System.Drawing.Image)(resources.GetObject("m_ImportElements.Image")));
            this.m_ImportElements.ImageTransparentColor = System.Drawing.SystemColors.ButtonFace;
            this.m_ImportElements.Name = "m_ImportElements";
            this.m_ImportElements.Size = new System.Drawing.Size(131, 25);
            this.m_ImportElements.Text = "&Import elements...";
            this.m_ImportElements.Click += new System.EventHandler(this.m_ImportElements_Click);
            // 
            // m_GenerateCode
            // 
            this.m_GenerateCode.Image = ((System.Drawing.Image)(resources.GetObject("m_GenerateCode.Image")));
            this.m_GenerateCode.ImageTransparentColor = System.Drawing.SystemColors.ButtonFace;
            this.m_GenerateCode.Name = "m_GenerateCode";
            this.m_GenerateCode.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.m_GenerateCode.Size = new System.Drawing.Size(111, 25);
            this.m_GenerateCode.Text = "&Generate code";
            this.m_GenerateCode.Click += new System.EventHandler(this.m_GenerateCode_Click);
            // 
            // SchemaUserControl
            // 
            this.AllowDrop = true;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.toolStrip2);
            this.DoubleBuffered = true;
            this.Name = "SchemaUserControl";
            this.Size = new System.Drawing.Size(462, 438);
            this.Scroll += new System.Windows.Forms.ScrollEventHandler(this.Schema_Scroll);
            this.SizeChanged += new System.EventHandler(this.Schema_SizeChanged);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Schema_DragEnter);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.schema_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.schema_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.schema_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.schema_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.schema_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.schema_MouseUp);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Timer popupTimer;
        private System.Windows.Forms.Timer popTimer;
        public System.Windows.Forms.VScrollBar vScrollBar1;
        public System.Windows.Forms.HScrollBar hScrollBar1;
        public System.Windows.Forms.ToolStrip toolStrip2;
        public System.Windows.Forms.ToolStripMenuItem m_AddSignal;
        public System.Windows.Forms.ToolStripMenuItem m_AddPort;
        public System.Windows.Forms.ToolStripMenuItem m_ImportElements;
        public System.Windows.Forms.ToolStripMenuItem m_GenerateCode;



    }
}