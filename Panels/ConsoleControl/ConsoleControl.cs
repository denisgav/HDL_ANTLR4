using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace ConsoleControl
{
    /// <summary>
    /// The console event handler is used for console events.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The <see cref="ConsoleEventArgs"/> instance containing the event data.</param>
    public delegate void ConsoleEventHanlder(object sender, ConsoleEventArgs args);

    /// <summary>
    /// The Console Control allows you to embed a basic console in your application.
    /// </summary>
    [ToolboxBitmap(typeof(resfinder), "ConsoleControl.ConsoleControl.bmp")]
    public partial class ConsoleControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleControl"/> class.
        /// </summary>
        public ConsoleControl()
        {
            //  Initialise the component.
            InitializeComponent();

            //  Show diagnostics disabled by default.
            ShowDiagnostics = false;

            //  Input enabled by default.
            IsInputEnabled = true;

            //  Disable special commands by default.
            SendKeyboardCommandsToProcess = false;

            //  Initialise the keymappings.
            InitialiseKeyMappings();

            //  Handle process events.
            processInterace.OnProcessOutput += new ProcessInterface.ProcessEventHanlder(processInterace_OnProcessOutput);
            processInterace.OnProcessError += new ProcessInterface.ProcessEventHanlder(processInterace_OnProcessError);
            processInterace.OnProcessInput += new ProcessInterface.ProcessEventHanlder(processInterace_OnProcessInput);
            processInterace.OnProcessExit += new ProcessInterface.ProcessEventHanlder(processInterace_OnProcessExit);

            //  Wait for key down messages on the rich text box.
            richTextBoxConsole.KeyDown += new KeyEventHandler(richTextBoxConsole_KeyDown);
        }

        /// <summary>
        /// Handles the OnProcessError event of the processInterace control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="ProcessInterface.ProcessEventArgs"/> instance containing the event data.</param>
        void processInterace_OnProcessError(object sender, ProcessInterface.ProcessEventArgs args)
        {
            //  Write the output, in red
            WriteOutput(args.Content, Color.Red);

            //  Fire the output event.
            FireConsoleOutputEvent(args.Content);
        }

        /// <summary>
        /// Handles the OnProcessOutput event of the processInterace control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="ProcessInterface.ProcessEventArgs"/> instance containing the event data.</param>
        void processInterace_OnProcessOutput(object sender, ProcessInterface.ProcessEventArgs args)
        {
            //  Write the output, in white
            WriteOutput(args.Content, Color.Black);

            //  Fire the output event.
            FireConsoleOutputEvent(args.Content);
        }

        /// <summary>
        /// Handles the OnProcessInput event of the processInterace control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="ProcessInterface.ProcessEventArgs"/> instance containing the event data.</param>
        void processInterace_OnProcessInput(object sender, ProcessInterface.ProcessEventArgs args)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Handles the OnProcessExit event of the processInterace control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="ProcessInterface.ProcessEventArgs"/> instance containing the event data.</param>
        void processInterace_OnProcessExit(object sender, ProcessInterface.ProcessEventArgs args)
        {
            //  Are we showing diagnostics?
            if (ShowDiagnostics)
            {
                WriteOutput(System.Environment.NewLine + processInterace.ProcessFileName + " exited.", Color.FromArgb(255, 0, 255, 0));
            }

            //  Read only again.
            Invoke((Action)(() =>
            {
                richTextBoxConsole.ReadOnly = true;
            }));
        }

        /// <summary>
        /// Initialises the key mappings.
        /// </summary>
        private void InitialiseKeyMappings()
        {
            //  Map 'tab'.
            keyMappings.Add(new KeyMapping(false, false, false, Keys.Tab, "{TAB}", "\t"));

            //  Map 'Ctrl-C'.
            keyMappings.Add(new KeyMapping(true, false, false, Keys.C, "^(c)", "\x03\r\n"));
        }

        /// <summary>
        /// Handles the KeyDown event of the richTextBoxConsole control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        void richTextBoxConsole_KeyDown(object sender, KeyEventArgs e)
        {
            //  Are we sending keyboard commands to the process?
            if (SendKeyboardCommandsToProcess && IsProcessRunning)
            {
                //  Get key mappings for this key event?
                var mappings = from k in keyMappings
                               where 
                               (k.KeyCode == e.KeyCode &&
                               k.IsAltPressed == e.Alt &&
                               k.IsControlPressed == e.Control &&
                               k.IsShiftPressed == e.Shift)
                               select k;

                //  Go through each mapping, send the message.
                foreach (var mapping in mappings)
                {
                    //SendKeysEx.SendKeys(CurrentProcessHwnd, mapping.SendKeysMapping);
                    //inputWriter.WriteLine(mapping.StreamMapping);
                    //WriteInput("\x3", Color.White, false);
                }

                //  If we handled a mapping, we're done here.
                if (mappings.Count() > 0)
                {
                    e.SuppressKeyPress = true;
                    return;
                }
            }

            //  If we're at the input point and it's backspace, bail.
            if ((richTextBoxConsole.SelectionStart <= inputStart) && e.KeyCode == Keys.Back) e.SuppressKeyPress = true;

            //  Are we in the read-only zone?
            if (richTextBoxConsole.SelectionStart < inputStart)
            {
                //  Allow arrows and Ctrl-C.
                if (!(e.KeyCode == Keys.Left ||
                    e.KeyCode == Keys.Right ||
                    e.KeyCode == Keys.Up ||
                    e.KeyCode == Keys.Down ||
                    (e.KeyCode == Keys.C && e.Control)))
                {
                    e.SuppressKeyPress = true;
                }
            }

            //  Is it the return key?
            if (e.KeyCode == Keys.Return)
            {
                //  Get the input.
                int length = (richTextBoxConsole.SelectionStart) - inputStart;
                if (length < 0)
                    length = 0;
                string input = richTextBoxConsole.Text.Substring(inputStart, length);

                //  Write the input (without echoing).
                WriteInput(input, Color.Black, false);
            }
        }

        /// <summary>
        /// Used as critical section
        /// </summary>
        private object lockObject = new object();
        /// <summary>
        /// Writes the output to the console control.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="color">The color.</param>
        public void WriteOutput(string output, Color color)
        {

            string print = Encoding.GetEncoding(866).GetString(Encoding.Default.GetBytes(output));
            if (string.IsNullOrEmpty(lastInput) == false &&
                (print == lastInput || print.Replace("\r\n", "") == lastInput))
                return;

            if (InvokeRequired == true)
            {
                Invoke((Action)(() =>
                {
                    WriteString(print, color);
                }));
            }
            else
            {
                WriteString(print, color);
            }
        }

        private void WriteString(string print, Color color)
        {
            lock (lockObject)
            {
                //  Write the output.
                richTextBoxConsole.SelectionStart = richTextBoxConsole.Text.Length;
                richTextBoxConsole.SelectionColor = color;
                richTextBoxConsole.SelectedText += print;
                inputStart = richTextBoxConsole.SelectionStart;
                richTextBoxConsole.ScrollToCaret();
            }
        }

        public void ClearOutput()
        {
            lock (lockObject)
            {
                richTextBoxConsole.Clear();
                inputStart = 0;
            }
        }

        /// <summary>
        /// Writes the input to the console control.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="color">The color.</param>
        /// <param name="echo">if set to <c>true</c> echo the input.</param>
        public void WriteInput(string input, Color color, bool echo)
        {
            Invoke((Action)(() =>
            {
                //  Are we echoing?
                if (echo)
                {
                    lock (lockObject)
                    {
                        richTextBoxConsole.SelectionColor = color;
                        richTextBoxConsole.SelectedText += input;
                        inputStart = richTextBoxConsole.SelectionStart;
                    }
                }

                lastInput = input;

                //  Write the input.
                processInterace.WriteInput(input);

                //  Fire the event.
                FireConsoleInputEvent(input);
            }));
        }

        /// <summary>
        /// Runs a process.
        /// All log data will be stored in outputStream
        /// </summary>
        /// <param name="processStartInfo"></param>
        /// <param name="outputStream"></param>
        public void StartProcess(string fileName, string arguments, Stream outputStream)
        {
            //  Create the process start info.
            ProcessStartInfo processStartInfo = new ProcessStartInfo(fileName, arguments);
            StartProcess(processStartInfo, outputStream);
        }

        /// <summary>
        /// Runs a process.
        /// All log data will be stored in outputStream
        /// </summary>
        /// <param name="processStartInfo"></param>
        /// <param name="outputStream"></param>
        public void StartProcess(ProcessStartInfo processStartInfo, Stream outputStream)
        {
            //  Are we showing diagnostics?
            if (ShowDiagnostics)
            {
                WriteOutput("Preparing to run " + processStartInfo.FileName, Color.FromArgb(255, 0, 255, 0));
                if (!string.IsNullOrEmpty(processStartInfo.Arguments))
                    WriteOutput(" with arguments " + processStartInfo.Arguments + "." + Environment.NewLine, Color.FromArgb(255, 0, 255, 0));
                else
                    WriteOutput("." + Environment.NewLine, Color.FromArgb(255, 0, 255, 0));
            }

            //  Start the process.
            processInterace.StartProcess(processStartInfo, outputStream);

            //  If we enable input, make the control not read only.
            if (IsInputEnabled)
                richTextBoxConsole.ReadOnly = false;
        }

        /// <summary>
        /// Runs a process.
        /// All log data will be stored in outputStream
        /// </summary>
        /// <param name="processStartInfo"></param>
        /// <param name="outputStream"></param>
        public void StartProcess(string fileName, string arguments, string outputFilePath)
        {
            //  Create the process start info.
            ProcessStartInfo processStartInfo = new ProcessStartInfo(fileName, arguments);
            StartProcess(processStartInfo, outputFilePath);
        }

        /// <summary>
        /// Runs a process.
        /// All log data will be stored in outputFilePath
        /// </summary>
        /// <param name="processStartInfo"></param>
        /// <param name="outputFilePath"></param>
        public void StartProcess(ProcessStartInfo processStartInfo, string outputFilePath)
        {
            //  Are we showing diagnostics?
            if (ShowDiagnostics)
            {
                WriteOutput("Preparing to run " + processStartInfo.FileName, Color.FromArgb(255, 0, 255, 0));
                if (!string.IsNullOrEmpty(processStartInfo.Arguments))
                    WriteOutput(" with arguments " + processStartInfo.Arguments + "." + Environment.NewLine, Color.FromArgb(255, 0, 255, 0));
                else
                    WriteOutput("." + Environment.NewLine, Color.FromArgb(255, 0, 255, 0));
            }

            //  Start the process.
            processInterace.StartProcess(processStartInfo, outputFilePath);

            //  If we enable input, make the control not read only.
            if (IsInputEnabled)
                richTextBoxConsole.ReadOnly = false;
        }

        /// <summary>
        /// Runs a process.
        /// </summary>
        /// <param name="processStartInfo"></param>
        public void StartProcess(ProcessStartInfo processStartInfo)
        {
            //  Are we showing diagnostics?
            if (ShowDiagnostics)
            {
                WriteOutput("Preparing to run " + processStartInfo.FileName, Color.FromArgb(255, 0, 255, 0));
                if (!string.IsNullOrEmpty(processStartInfo.Arguments))
                    WriteOutput(" with arguments " + processStartInfo.Arguments + "." + Environment.NewLine, Color.FromArgb(255, 0, 255, 0));
                else
                    WriteOutput("." + Environment.NewLine, Color.FromArgb(255, 0, 255, 0));
            }

            //  Start the process.
            processInterace.StartProcess(processStartInfo);

            //  If we enable input, make the control not read only.
            if (IsInputEnabled)
                richTextBoxConsole.ReadOnly = false;
        }

        /// <summary>
        /// Runs a process.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="arguments">The arguments.</param>
        public void StartProcess(string fileName, string arguments)
        {
            //  Create the process start info.
            ProcessStartInfo processStartInfo = new ProcessStartInfo(fileName, arguments);
            StartProcess(processStartInfo);
        }

        /// <summary>
        /// Stops the process.
        /// </summary>
        public void StopProcess()
        {
            //  Stop the interface.
            processInterace.StopProcess();
        }
        
        /// <summary>
        /// Fires the console output event.
        /// </summary>
        /// <param name="content">The content.</param>
        private void FireConsoleOutputEvent(string content)
        {
            //  Get the event.
            var theEvent = OnConsoleOutput;
            if (theEvent != null)
                theEvent(this, new ConsoleEventArgs(content));
        }

        /// <summary>
        /// Fires the console input event.
        /// </summary>
        /// <param name="content">The content.</param>
        private void FireConsoleInputEvent(string content)
        {
            //  Get the event.
            var theEvent = OnConsoleInput;
            if (theEvent != null)
                theEvent(this, new ConsoleEventArgs(content));
        }

        /// <summary>
        /// The internal process interface used to interface with the process.
        /// </summary>
        private ProcessInterface.ProcessInterface processInterace = new ProcessInterface.ProcessInterface();
        
        /// <summary>
        /// Current position that input starts at.
        /// </summary>
        int inputStart = -1;

        /// <summary>
        /// The is input enabled flag.
        /// </summary>
        private bool isInputEnabled = true;

        /// <summary>
        /// The last input string (used so that we can make sure we don't echo input twice).
        /// </summary>
        private string lastInput;

        /// <summary>
        /// The key mappings.
        /// </summary>
        private List<KeyMapping> keyMappings = new List<KeyMapping>();

        /// <summary>
        /// Occurs when console output is produced.
        /// </summary>
        public event ConsoleEventHanlder OnConsoleOutput;

        /// <summary>
        /// Occurs when console input is produced.
        /// </summary>
        public event ConsoleEventHanlder OnConsoleInput;

        /// <summary>
        /// Gets or sets a value indicating whether to show diagnostics.
        /// </summary>
        /// <value>
        ///   <c>true</c> if show diagnostics; otherwise, <c>false</c>.
        /// </value>
        [Category("Console Control"), Description("Show diagnostic information, such as exceptions.")]
        public bool ShowDiagnostics
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is input enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is input enabled; otherwise, <c>false</c>.
        /// </value>
        [Category("Console Control"), Description("If true, the user can key in input.")]
        public bool IsInputEnabled
        {
            get { return isInputEnabled; }
            set
            {
                isInputEnabled = value;
                if (IsProcessRunning)
                    richTextBoxConsole.ReadOnly = !value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [send keyboard commands to process].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [send keyboard commands to process]; otherwise, <c>false</c>.
        /// </value>
        [Category("Console Control"), Description("If true, special keyboard commands like Ctrl-C and tab are sent to the process.")]
        public bool SendKeyboardCommandsToProcess
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is process running.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is process running; otherwise, <c>false</c>.
        /// </value>
        [Browsable(false)]
        public bool IsProcessRunning
        {
            get { return processInterace.IsProcessRunning; }
        }

        /// <summary>
        /// Gets the internal rich text box.
        /// </summary>
        [Browsable(false)]
        public RichTextBox InternalRichTextBox
        {
            get { return richTextBoxConsole; }
        }

        /// <summary>
        /// Gets the process interface.
        /// </summary>
        [Browsable(false)]
        public ProcessInterface.ProcessInterface ProcessInterface
        {
            get { return processInterace; }
        }

        /// <summary>
        /// Gets the key mappings.
        /// </summary>
        [Browsable(false)]
        public List<KeyMapping> KeyMappings
        {
            get { return keyMappings; }
        }

        /// <summary>
        /// Gets or sets the font of the text displayed by the control.
        /// </summary>
        /// <returns>The <see cref="T:System.Drawing.Font" /> to apply to the text displayed by the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultFont" /> property.</returns>
        ///   <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
        ///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   </PermissionSet>
        public override Font Font
        {
            get
            {
                //  Return the base class font.
                return base.Font;
            }
            set
            {
                //  Set the base class font...
                base.Font = value;

                //  ...and the internal control font.
                richTextBoxConsole.Font = value;
            }
        }

        #region ToolStrip handlers
        /// <summary>
        /// Handles the Click event of the toolStripButtonNewProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void toolStripButtonNewProcess_Click(object sender, EventArgs e)
        {
            //  Create the new process form.
            FormNewProcess formNewProcess = new FormNewProcess();

            //  If the form is shown OK, start the process.
            if (formNewProcess.ShowDialog() == DialogResult.OK)
            {
                //  Start the proces.
                StartProcess(formNewProcess.FileName, formNewProcess.Arguments);

                //  Update the UI state.
                UpdateUIState();
            }
        }

        /// <summary>
        /// Handles the Click event of the toolStripButtonStopProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void toolStripButtonStopProcess_Click(object sender, EventArgs e)
        {
            StopProcess();

            //  Update the UI state.
            UpdateUIState();
        }

        /// <summary>
        /// Updates the state of the UI.
        /// </summary>
        private void UpdateUIState()
        {
            //  Update the state.
            if (IsProcessRunning)
                toolStripStatusLabelConsoleState.Text = "Running " + System.IO.Path.GetFileName(ProcessInterface.ProcessFileName);
            else
                toolStripStatusLabelConsoleState.Text = "Not Running";

            //  Update toolbar buttons.
            toolStripButtonRunCMD.Enabled = !IsProcessRunning;
            toolStripButtonNewProcess.Enabled = !IsProcessRunning;
            toolStripButtonStopProcess.Enabled = IsProcessRunning;
            toolStripButtonShowDiagnostics.Checked = ShowDiagnostics;
            toolStripButtonInputEnabled.Checked = IsInputEnabled;
            toolStripButtonSendKeyboardCommandsToProcess.Checked = SendKeyboardCommandsToProcess;
        }

        /// <summary>
        /// Handles the Click event of the toolStripButtonShowDiagnostics control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void toolStripButtonShowDiagnostics_Click(object sender, EventArgs e)
        {
            ShowDiagnostics = !ShowDiagnostics;
            UpdateUIState();
        }

        /// <summary>
        /// Handles the Tick event of the timerUpdateUI control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void timerUpdateUI_Tick(object sender, EventArgs e)
        {
            UpdateUIState();
        }

        /// <summary>
        /// Handles the Click event of the toolStripButtonInputEnabled control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void toolStripButtonInputEnabled_Click(object sender, EventArgs e)
        {
            IsInputEnabled = !IsInputEnabled;
            UpdateUIState();
        }

        /// <summary>
        /// Handles the Click event of the toolStripButtonRunCMD control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void toolStripButtonRunCMD_Click(object sender, EventArgs e)
        {
            StartProcess("cmd", null);
            UpdateUIState();
        }

        /// <summary>
        /// Handles the Click event of the toolStripButtonSendKeyboardCommandsToProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void toolStripButtonSendKeyboardCommandsToProcess_Click(object sender, EventArgs e)
        {
            SendKeyboardCommandsToProcess = !SendKeyboardCommandsToProcess;
            UpdateUIState();
        }

        /// <summary>
        /// Handles the Click event of the toolStripButtonClearOutput control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void toolStripButtonClearOutput_Click(object sender, EventArgs e)
        {
            ClearOutput();
        }
        #endregion

        /// <summary>
        /// Handles the Load event of the FormConsoleControlSample control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ConsoleControl_Load(object sender, EventArgs e)
        {
            //  Update the UI state.
            UpdateUIState();
        }
    }
}