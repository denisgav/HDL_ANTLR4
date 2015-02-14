namespace ConsoleControl
{
  partial class ConsoleControl
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.richTextBoxConsole = new System.Windows.Forms.RichTextBox();
        this.toolStrip = new System.Windows.Forms.ToolStrip();
        this.toolStripButtonRunCMD = new System.Windows.Forms.ToolStripButton();
        this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
        this.toolStripButtonNewProcess = new System.Windows.Forms.ToolStripButton();
        this.toolStripButtonStopProcess = new System.Windows.Forms.ToolStripButton();
        this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
        this.toolStripButtonShowDiagnostics = new System.Windows.Forms.ToolStripButton();
        this.toolStripButtonInputEnabled = new System.Windows.Forms.ToolStripButton();
        this.toolStripButtonSendKeyboardCommandsToProcess = new System.Windows.Forms.ToolStripButton();
        this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
        this.toolStripButtonClearOutput = new System.Windows.Forms.ToolStripButton();
        this.statusStrip = new System.Windows.Forms.StatusStrip();
        this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
        this.toolStripStatusLabelConsoleState = new System.Windows.Forms.ToolStripStatusLabel();
        this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
        this.timerUpdateUI = new System.Windows.Forms.Timer(this.components);
        this.toolStrip.SuspendLayout();
        this.statusStrip.SuspendLayout();
        this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
        this.toolStripContainer1.ContentPanel.SuspendLayout();
        this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
        this.toolStripContainer1.SuspendLayout();
        this.SuspendLayout();
        // 
        // richTextBoxConsole
        // 
        this.richTextBoxConsole.AcceptsTab = true;
        this.richTextBoxConsole.BackColor = System.Drawing.SystemColors.Control;
        this.richTextBoxConsole.Dock = System.Windows.Forms.DockStyle.Fill;
        this.richTextBoxConsole.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.richTextBoxConsole.ForeColor = System.Drawing.Color.Black;
        this.richTextBoxConsole.Location = new System.Drawing.Point(0, 0);
        this.richTextBoxConsole.Name = "richTextBoxConsole";
        this.richTextBoxConsole.ReadOnly = true;
        this.richTextBoxConsole.Size = new System.Drawing.Size(816, 297);
        this.richTextBoxConsole.TabIndex = 0;
        this.richTextBoxConsole.Text = "";
        // 
        // toolStrip
        // 
        this.toolStrip.Dock = System.Windows.Forms.DockStyle.None;
        this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonRunCMD,
            this.toolStripSeparator2,
            this.toolStripButtonNewProcess,
            this.toolStripButtonStopProcess,
            this.toolStripSeparator1,
            this.toolStripButtonShowDiagnostics,
            this.toolStripButtonInputEnabled,
            this.toolStripButtonSendKeyboardCommandsToProcess,
            this.toolStripSeparator3,
            this.toolStripButtonClearOutput});
        this.toolStrip.Location = new System.Drawing.Point(3, 0);
        this.toolStrip.Name = "toolStrip";
        this.toolStrip.Size = new System.Drawing.Size(191, 25);
        this.toolStrip.TabIndex = 1;
        // 
        // toolStripButtonRunCMD
        // 
        this.toolStripButtonRunCMD.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this.toolStripButtonRunCMD.Image = global::ConsoleControl.Resource.ConsoleControl;
        this.toolStripButtonRunCMD.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.toolStripButtonRunCMD.Name = "toolStripButtonRunCMD";
        this.toolStripButtonRunCMD.Size = new System.Drawing.Size(23, 22);
        this.toolStripButtonRunCMD.Text = "Run CMD";
        this.toolStripButtonRunCMD.Click += new System.EventHandler(this.toolStripButtonRunCMD_Click);
        // 
        // toolStripSeparator2
        // 
        this.toolStripSeparator2.Name = "toolStripSeparator2";
        this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
        // 
        // toolStripButtonNewProcess
        // 
        this.toolStripButtonNewProcess.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this.toolStripButtonNewProcess.Image = global::ConsoleControl.Resource.Play;
        this.toolStripButtonNewProcess.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.toolStripButtonNewProcess.Name = "toolStripButtonNewProcess";
        this.toolStripButtonNewProcess.Size = new System.Drawing.Size(23, 22);
        this.toolStripButtonNewProcess.Text = "New Process";
        this.toolStripButtonNewProcess.Click += new System.EventHandler(this.toolStripButtonNewProcess_Click);
        // 
        // toolStripButtonStopProcess
        // 
        this.toolStripButtonStopProcess.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this.toolStripButtonStopProcess.Image = global::ConsoleControl.Resource.Stop;
        this.toolStripButtonStopProcess.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.toolStripButtonStopProcess.Name = "toolStripButtonStopProcess";
        this.toolStripButtonStopProcess.Size = new System.Drawing.Size(23, 22);
        this.toolStripButtonStopProcess.Text = "Stop Process";
        this.toolStripButtonStopProcess.Click += new System.EventHandler(this.toolStripButtonStopProcess_Click);
        // 
        // toolStripSeparator1
        // 
        this.toolStripSeparator1.Name = "toolStripSeparator1";
        this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
        // 
        // toolStripButtonShowDiagnostics
        // 
        this.toolStripButtonShowDiagnostics.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this.toolStripButtonShowDiagnostics.Image = global::ConsoleControl.Resource.Information;
        this.toolStripButtonShowDiagnostics.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.toolStripButtonShowDiagnostics.Name = "toolStripButtonShowDiagnostics";
        this.toolStripButtonShowDiagnostics.Size = new System.Drawing.Size(23, 22);
        this.toolStripButtonShowDiagnostics.Text = "Show Diagnostics";
        this.toolStripButtonShowDiagnostics.Click += new System.EventHandler(this.toolStripButtonShowDiagnostics_Click);
        // 
        // toolStripButtonInputEnabled
        // 
        this.toolStripButtonInputEnabled.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this.toolStripButtonInputEnabled.Image = global::ConsoleControl.Resource.Control_TextBox;
        this.toolStripButtonInputEnabled.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.toolStripButtonInputEnabled.Name = "toolStripButtonInputEnabled";
        this.toolStripButtonInputEnabled.Size = new System.Drawing.Size(23, 22);
        this.toolStripButtonInputEnabled.Text = "Input Enabled";
        this.toolStripButtonInputEnabled.Click += new System.EventHandler(this.toolStripButtonInputEnabled_Click);
        // 
        // toolStripButtonSendKeyboardCommandsToProcess
        // 
        this.toolStripButtonSendKeyboardCommandsToProcess.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this.toolStripButtonSendKeyboardCommandsToProcess.Image = global::ConsoleControl.Resource.GotoShortcuts;
        this.toolStripButtonSendKeyboardCommandsToProcess.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.toolStripButtonSendKeyboardCommandsToProcess.Name = "toolStripButtonSendKeyboardCommandsToProcess";
        this.toolStripButtonSendKeyboardCommandsToProcess.Size = new System.Drawing.Size(23, 22);
        this.toolStripButtonSendKeyboardCommandsToProcess.Text = "Send Keyboard Commands to Process";
        this.toolStripButtonSendKeyboardCommandsToProcess.Click += new System.EventHandler(this.toolStripButtonSendKeyboardCommandsToProcess_Click);
        // 
        // toolStripSeparator3
        // 
        this.toolStripSeparator3.Name = "toolStripSeparator3";
        this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
        // 
        // toolStripButtonClearOutput
        // 
        this.toolStripButtonClearOutput.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        this.toolStripButtonClearOutput.Image = global::ConsoleControl.Resource.Delete;
        this.toolStripButtonClearOutput.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.toolStripButtonClearOutput.Name = "toolStripButtonClearOutput";
        this.toolStripButtonClearOutput.Size = new System.Drawing.Size(23, 22);
        this.toolStripButtonClearOutput.Text = "Clear Output";
        this.toolStripButtonClearOutput.Click += new System.EventHandler(this.toolStripButtonClearOutput_Click);
        // 
        // statusStrip
        // 
        this.statusStrip.Dock = System.Windows.Forms.DockStyle.None;
        this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabelConsoleState});
        this.statusStrip.Location = new System.Drawing.Point(0, 0);
        this.statusStrip.Name = "statusStrip";
        this.statusStrip.Size = new System.Drawing.Size(816, 22);
        this.statusStrip.TabIndex = 2;
        this.statusStrip.Text = "statusStrip1";
        // 
        // toolStripStatusLabel1
        // 
        this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.Black;
        this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
        this.toolStripStatusLabel1.Size = new System.Drawing.Size(82, 17);
        this.toolStripStatusLabel1.Text = "Console State:";
        // 
        // toolStripStatusLabelConsoleState
        // 
        this.toolStripStatusLabelConsoleState.ForeColor = System.Drawing.Color.Black;
        this.toolStripStatusLabelConsoleState.Name = "toolStripStatusLabelConsoleState";
        this.toolStripStatusLabelConsoleState.Size = new System.Drawing.Size(75, 17);
        this.toolStripStatusLabelConsoleState.Text = "Not Running";
        // 
        // toolStripContainer1
        // 
        // 
        // toolStripContainer1.BottomToolStripPanel
        // 
        this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip);
        // 
        // toolStripContainer1.ContentPanel
        // 
        this.toolStripContainer1.ContentPanel.Controls.Add(this.richTextBoxConsole);
        this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(816, 297);
        this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
        this.toolStripContainer1.Name = "toolStripContainer1";
        this.toolStripContainer1.Size = new System.Drawing.Size(816, 344);
        this.toolStripContainer1.TabIndex = 3;
        this.toolStripContainer1.Text = "toolStripContainer1";
        // 
        // toolStripContainer1.TopToolStripPanel
        // 
        this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip);
        // 
        // timerUpdateUI
        // 
        this.timerUpdateUI.Enabled = true;
        this.timerUpdateUI.Tick += new System.EventHandler(this.timerUpdateUI_Tick);
        // 
        // ConsoleControl
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.Controls.Add(this.toolStripContainer1);
        this.ForeColor = System.Drawing.Color.White;
        this.Name = "ConsoleControl";
        this.Size = new System.Drawing.Size(816, 344);
        this.Load += new System.EventHandler(this.ConsoleControl_Load);
        this.toolStrip.ResumeLayout(false);
        this.toolStrip.PerformLayout();
        this.statusStrip.ResumeLayout(false);
        this.statusStrip.PerformLayout();
        this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
        this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
        this.toolStripContainer1.ContentPanel.ResumeLayout(false);
        this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
        this.toolStripContainer1.TopToolStripPanel.PerformLayout();
        this.toolStripContainer1.ResumeLayout(false);
        this.toolStripContainer1.PerformLayout();
        this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.RichTextBox richTextBoxConsole;
    private System.Windows.Forms.ToolStrip toolStrip;
    private System.Windows.Forms.ToolStripButton toolStripButtonRunCMD;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripButton toolStripButtonNewProcess;
    private System.Windows.Forms.ToolStripButton toolStripButtonStopProcess;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripButton toolStripButtonShowDiagnostics;
    private System.Windows.Forms.ToolStripButton toolStripButtonInputEnabled;
    private System.Windows.Forms.ToolStripButton toolStripButtonSendKeyboardCommandsToProcess;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private System.Windows.Forms.ToolStripButton toolStripButtonClearOutput;
    private System.Windows.Forms.StatusStrip statusStrip;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelConsoleState;
    private System.Windows.Forms.ToolStripContainer toolStripContainer1;
    private System.Windows.Forms.Timer timerUpdateUI;
  }
}
