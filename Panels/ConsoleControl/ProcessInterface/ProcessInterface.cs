using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;

namespace ProcessInterface
{
    /// <summary>
    /// A ProcessEventHandler is a delegate for process input/output events.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The <see cref="ProcessInterface.ProcessEventArgs"/> instance containing the event data.</param>
    public delegate void ProcessEventHanlder(object sender, ProcessEventArgs args);

    /// <summary>
    /// A class the wraps a process, allowing programmatic input and output.
    /// </summary>
    public class ProcessInterface : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessInterface"/> class.
        /// </summary>
        public ProcessInterface()
        {
            //  Configure the output worker.
            outputWorker.WorkerReportsProgress = true;
            outputWorker.WorkerSupportsCancellation = true;
            outputWorker.DoWork += new DoWorkEventHandler(outputWorker_DoWork);
            outputWorker.ProgressChanged += new ProgressChangedEventHandler(outputWorker_ProgressChanged);

            //  Configure the error worker.
            errorWorker.WorkerReportsProgress = true;
            errorWorker.WorkerSupportsCancellation = true;
            errorWorker.DoWork += new DoWorkEventHandler(errorWorker_DoWork);
            errorWorker.ProgressChanged += new ProgressChangedEventHandler(errorWorker_ProgressChanged);
        }

        /// <summary>
        /// Поток, в который выводить текст с процесса
        /// </summary>
        private Stream outputStream;
        public Stream OutputStream
        {
            get { return outputStream; }
            set { outputStream = value; }
        }

        /// <summary>
        /// Выводит текст в поток
        /// </summary>
        private StreamWriter outputStreamWriter;
        

        /// <summary>
        /// Handles the ProgressChanged event of the outputWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.ProgressChangedEventArgs"/> instance containing the event data.</param>
        void outputWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //  We must be passed a string in the user state.
            if (e.UserState is string)
            {
                if (outputStreamWriter != null)
                    outputStreamWriter.Write(e.UserState as string);
                //  Fire the output event.
                FireProcessOutputEvent(e.UserState as string);
            }
        }

        /// <summary>
        /// Handles the DoWork event of the outputWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        void outputWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                while (outputWorker.CancellationPending == false)
                {
                    if (outputReader != null)
                    {
                        //  Any lines to read?
                        int count = 0;
                        char[] buffer = new char[1024];

                        StringBuilder builder = new StringBuilder();
                        do
                        {
                            count = outputReader.Read(buffer, 0, 1024);
                            builder.Append(buffer, 0, count);

                        } while (count > 0);
                        outputWorker.ReportProgress(0, builder.ToString());
                        
                    }
                    if (IsProcessRunning == false)
                    {
                        return;
                    }
                    System.Threading.Thread.Sleep(200);
                }
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(string.Format("Message: {0}\n\rStackTrace: {1}\n\rSource: {2}", ex.Message, ex.StackTrace, ex.Source), "Error ocrred", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                Schematix.Core.Logger.Log.Error("Output worker error.", ex);
            }
            outputReader = null;
        }

        /// <summary>
        /// Handles the ProgressChanged event of the errorWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.ProgressChangedEventArgs"/> instance containing the event data.</param>
        void errorWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //  The userstate must be a string.
            if (e.UserState is string)
            {
                if (outputStreamWriter != null)
                    outputStreamWriter.Write(e.UserState as string);
                //  Fire the error event.
                FireProcessErrorEvent(e.UserState as string);
            }
        }

        /// <summary>
        /// Handles the DoWork event of the errorWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        void errorWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                while (errorWorker.CancellationPending == false)
                {
                    if (errorReader != null)
                    {
                        //  Any lines to read?
                        int count = 0;
                        char[] buffer = new char[1024];

                        StringBuilder builder = new StringBuilder();
                        do
                        {
                            count = errorReader.Read(buffer, 0, 1024);
                            builder.Append(buffer, 0, count);
                        } while (count > 0);
                        errorWorker.ReportProgress(0, builder.ToString());                        
                    }
                    if (IsProcessRunning == false)
                    {
                        return;
                    }
                    System.Threading.Thread.Sleep(200);
                }
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(string.Format("Message: {0}\n\rStackTrace: {1}\n\rSource: {2}", ex.Message, ex.StackTrace, ex.Source), "Error ocrred", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                Schematix.Core.Logger.Log.Error("Error worker error.", ex);
            }
            errorReader = null;
        }

        /// <summary>
        /// Runs a process.
        /// All log data will be stored in outputStream
        /// </summary>
        /// <param name="processStartInfo"></param>
        /// <param name="outputStream"></param>
        public bool StartProcess(string fileName, string arguments, Stream outputStream)
        {
            //  Create the process start info.
            ProcessStartInfo processStartInfo = new ProcessStartInfo(fileName, arguments);
            return StartProcess(processStartInfo, outputStream);
        }

        /// <summary>
        /// Runs a process.
        /// All log data will be stored in outputStream
        /// </summary>
        /// <param name="processStartInfo"></param>
        /// <param name="outputStream"></param>
        public bool StartProcess(ProcessStartInfo processStartInfo, Stream outputStream)
        {
            try
            {
                this.outputStream = outputStream;
                outputStreamWriter = new StreamWriter(outputStream);
                outputStreamWriter.Write("Started process {0} {1}", processStartInfo.FileName, processStartInfo.Arguments);
                outputStreamWriter.AutoFlush = true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(string.Format("Could not run process. Message: {0}\n\r", ex.Message), "Error ocrred", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                Schematix.Core.Logger.Log.Error("Start process error.", ex);
            }
            return StartProcess(processStartInfo);
        }

        /// <summary>
        /// Runs a process.
        /// All log data will be stored in outputStream
        /// </summary>
        /// <param name="processStartInfo"></param>
        /// <param name="outputStream"></param>
        public bool StartProcess(string fileName, string arguments, string outputFilePath)
        {
            //  Create the process start info.
            ProcessStartInfo processStartInfo = new ProcessStartInfo(fileName, arguments);
            return StartProcess(processStartInfo, outputFilePath);
        }

        /// <summary>
        /// Runs a process.
        /// All log data will be stored in outputFilePath
        /// </summary>
        /// <param name="processStartInfo"></param>
        /// <param name="outputFilePath"></param>
        public bool StartProcess(ProcessStartInfo processStartInfo, string outputFilePath)
        {
            try
            {
                outputStream = new System.IO.FileStream(outputFilePath, FileMode.Create, FileAccess.Write);
                outputStreamWriter = new StreamWriter(outputStream);
                outputStreamWriter.Write("Started process {0} {1}", processStartInfo.FileName, processStartInfo.Arguments);
                outputStreamWriter.AutoFlush = true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(string.Format("Could not run process. Message: {0}\n\r", ex.Message), "Error ocrred", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                Schematix.Core.Logger.Log.Error("Start process error.", ex);
            }
            return StartProcess(processStartInfo);
        }

        /// <summary>
        /// Runs a process.
        /// </summary>
        /// <param name="processStartInfo"></param>
        public bool StartProcess(ProcessStartInfo processStartInfo)
        {
            while (IsProcessRunning)
                System.Threading.Thread.Sleep(200);
            //  Set the options.
            processStartInfo.UseShellExecute = false;
            processStartInfo.ErrorDialog = false;
            processStartInfo.CreateNoWindow = true;

            //  Specify redirection.
            processStartInfo.RedirectStandardError = true;
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.RedirectStandardOutput = true;

            //  Create the process.
            process = new Process();
            process.EnableRaisingEvents = true;
            process.StartInfo = processStartInfo;
            process.Exited += new EventHandler(currentProcess_Exited);
            bool processStarted = false;

            //  Start the process.
            try
            {
                processStarted = process.Start();
            }
            catch (Exception ex)
            {
                //  Trace the exception.
                Trace.WriteLine("Failed to start process " + processStartInfo.FileName + " with arguments '" + processStartInfo.Arguments + "'");
                Trace.WriteLine(ex.ToString());
                System.Windows.Forms.MessageBox.Show(string.Format("Failed to start process " + processStartInfo.FileName + " with arguments '" + processStartInfo.Arguments + "'"), "Error ocrred", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                Schematix.Core.Logger.Log.Error("Start process error.", ex);
                return false;
            }

            //  Store name and arguments.
            processFileName = processStartInfo.FileName;
            processArguments = processStartInfo.Arguments;

            //  Create the readers and writers.
            inputWriter = process.StandardInput;
            outputReader = TextReader.Synchronized(process.StandardOutput);
            errorReader = TextReader.Synchronized(process.StandardError);            

            //  Run the workers that read output and error.
            bool started = false;
            while (started == false)
            {
                try
                {
                    outputWorker.RunWorkerAsync();
                    errorWorker.RunWorkerAsync();
                    started = true;
                }
                catch (Exception ex)
                {
                    started = false;
                    System.Threading.Thread.Sleep(500);
                }
            }

            FireProcessOutputEvent("start process '" + processStartInfo.FileName + "' with arguments '" + processStartInfo.Arguments + "'\n\r");
            return processStarted;
        }

        /// <summary>
        /// Runs a process.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="arguments">The arguments.</param>
        public bool StartProcess(string fileName, string arguments)
        {
            //  Create the process start info.
            ProcessStartInfo processStartInfo = new ProcessStartInfo(fileName, arguments);
            return StartProcess(processStartInfo);
        }

        /// <summary>
        /// Stops the process.
        /// </summary>
        public void StopProcess()
        {
            try
            {
                //  Handle the trivial case.
                if (IsProcessRunning == false)
                    return;

                if (outputStreamWriter != null)
                {
                    outputStreamWriter.Flush();
                    outputStreamWriter.Close();
                    outputStream.Close();
                    outputStreamWriter = null;
                }

                //  Kill the process.
                process.Kill();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(string.Format("Could not terminate process. Message: {0}\n\r", ex.Message), "Error ocrred", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                Schematix.Core.Logger.Log.Error("Terminate process error.", ex);
            }
        }

        /// <summary>
        /// Handles the Exited event of the currentProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void currentProcess_Exited(object sender, EventArgs e)
        {
            try
            {
                //  Fire process exited.
                FireProcessExitEvent(process.ExitCode);
                FireProcessOutputEvent("process exited'" + processFileName + "' with arguments '" + processArguments + "'\n\r");

                if (process != null)
                {
                    //  Disable the threads.
                    while (IsProcessRunning)
                        System.Threading.Thread.Sleep(200);
                }
                //outputWorker.CancelAsync();
                //errorWorker.CancelAsync();                

                inputWriter = null;                

                processFileName = string.Empty;
                processArguments = string.Empty;

                if (outputStreamWriter != null)
                {
                    outputStreamWriter.Flush();
                    outputStreamWriter.Close();
                    outputStream.Close();
                    outputStreamWriter = null;
                }                
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(string.Format("Message: {0}\n\rStackTrace: {1}\n\rSource: {2}", ex.Message, ex.StackTrace, ex.Source), "Error ocrred", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                Schematix.Core.Logger.Log.Error("Process_Exited error.", ex);
            }
        }

        #region Fire functions
        // uses as critical section
        private object lockObject = new object();
        /// <summary>
        /// Fires the process output event.
        /// </summary>
        /// <param name="content">The content.</param>
        private void FireProcessOutputEvent(string content)
        {
                //  Get the event and fire it.
                var theEvent = OnProcessOutput;
                if (theEvent != null)
                    theEvent(this, new ProcessEventArgs(content));
        }

        /// <summary>
        /// Fires the process error output event.
        /// </summary>
        /// <param name="content">The content.</param>
        private void FireProcessErrorEvent(string content)
        {
                //  Get the event and fire it.
                var theEvent = OnProcessError;
                if (theEvent != null)
                    theEvent(this, new ProcessEventArgs(content));
        }

        /// <summary>
        /// Fires the process input event.
        /// </summary>
        /// <param name="content">The content.</param>
        private void FireProcessInputEvent(string content)
        {
                //  Get the event and fire it.
                var theEvent = OnProcessInput;
                if (theEvent != null)
                    theEvent(this, new ProcessEventArgs(content));
        }

        /// <summary>
        /// Fires the process exit event.
        /// </summary>
        /// <param name="code">The code.</param>
        private void FireProcessExitEvent(int code)
        {
                //  Get the event and fire it.
                var theEvent = OnProcessExit;
                if (theEvent != null)
                    theEvent(this, new ProcessEventArgs(code));
        }
        #endregion 

        /// <summary>
        /// Writes the input.
        /// </summary>
        /// <param name="input">The input.</param>
        public void WriteInput(string input)
        {
            if (IsProcessRunning)
            {
                inputWriter.WriteLine(input);
                inputWriter.Flush();
            }
        }

        public void ResetProcess()
        {
            StartProcess("cmd", string.Empty);
        }

        /// <summary>
        /// The current process.
        /// </summary>
        private Process process;
        
        /// <summary>
        /// The input writer.
        /// </summary>
        private StreamWriter inputWriter;
        
        /// <summary>
        /// The output reader.
        /// </summary>
        private TextReader outputReader;
        
        /// <summary>
        /// The error reader.
        /// </summary>
        private TextReader errorReader;
        
        /// <summary>
        /// The output worker.
        /// </summary>
        private BackgroundWorker outputWorker = new BackgroundWorker();
        
        /// <summary>
        /// The error worker.
        /// </summary>
        private BackgroundWorker errorWorker = new BackgroundWorker();

        /// <summary>
        /// Current process file name.
        /// </summary>
        private string processFileName;

        /// <summary>
        /// Arguments sent to the current process.
        /// </summary>
        private string processArguments;
        
        /// <summary>
        /// Occurs when process output is produced.
        /// </summary>
        public event ProcessEventHanlder OnProcessOutput;

        /// <summary>
        /// Occurs when process error output is produced.
        /// </summary>
        public event ProcessEventHanlder OnProcessError;

        /// <summary>
        /// Occurs when process input is produced.
        /// </summary>
        public event ProcessEventHanlder OnProcessInput;

        /// <summary>
        /// Occurs when the process ends.
        /// </summary>
        public event ProcessEventHanlder OnProcessExit;
        
        /// <summary>
        /// Gets a value indicating whether this instance is process running.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is process running; otherwise, <c>false</c>.
        /// </value>
        public bool IsProcessRunning
        {
            get
            {
                try
                {
                    return (process != null && process.HasExited == false);
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets the internal process.
        /// </summary>
        public Process Process
        {
            get { return process; }
        }

        /// <summary>
        /// Gets the name of the process.
        /// </summary>
        /// <value>
        /// The name of the process.
        /// </value>
        public string ProcessFileName
        {
            get { return processFileName; }
        }

        /// <summary>
        /// Gets the process arguments.
        /// </summary>
        public string ProcessArguments
        {
            get { return processArguments; }
        }

        /// <summary>
        /// Ожидание завершения процесса
        /// </summary>
        public void WaitForExitProcess()
        {
            if (IsProcessRunning == true)
            {
                process.WaitForExit();
                while (IsProcessRunning)
                    System.Threading.Thread.Sleep(200);
                System.Threading.Thread.Sleep(500);
            }
        }

        #region IDispose region
        // Track whether Dispose has been called.
        private bool disposed = false;

        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the
        // runtime from inside the finalizer and you should not reference
        // other objects. Only unmanaged resources can be disposed.
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if(!this.disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if(disposing)
                {
                    // Dispose managed resources.
                }

                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.
                StopProcess();

                // Note disposing has been done.
                disposed = true;
            }
        }

        // Use C# destructor syntax for finalization code.
        // This destructor will run only if the Dispose method
        // does not get called.
        // It gives your base class the opportunity to finalize.
        // Do not provide destructors in types derived from this class.
        ~ProcessInterface()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }
        #endregion
    }
}
