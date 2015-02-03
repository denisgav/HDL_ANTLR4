//
//  Copyright (C) 2010-2014  Denis Gavrish
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace VHDL.parser
{
    public enum LoggerMessageVerbosity
    {
        Info,
        Error,
        Failue,
        Warning
    }

    public class Logger : IDisposable
    {

        public delegate void OnWriteDeleagte(LoggerMessageVerbosity verbosity, string message);
        private event OnWriteDeleagte onWriteEvent;
        public event OnWriteDeleagte OnWriteEvent
        {
            add { onWriteEvent += value; }
            remove { onWriteEvent -= value; }
        }

        private void OnWrite(LoggerMessageVerbosity verbosity, string message)
        { }
        
        private Stream stream;
        private TextWriter writer;
        public TextWriter Writer
        {
            get { return writer; }
        }

        public static System.ConsoleColor ConvertToConsoleColor(LoggerMessageVerbosity verbosity)
        {
            switch (verbosity)
            {
                case LoggerMessageVerbosity.Warning:
                    return ConsoleColor.Yellow;
                case LoggerMessageVerbosity.Error:
                    return ConsoleColor.Red;
                case LoggerMessageVerbosity.Failue:
                    return ConsoleColor.DarkRed;
                default:
                    return ConsoleColor.Gray;
            }
        }
        
        public Logger(string filePath)
        {
            stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read);
            writer = new StreamWriter(stream);
            onWriteEvent = new OnWriteDeleagte(OnWrite);
        }

        public static Logger CreateLogger(string folderPath)
        {
            return CreateLogger(folderPath, "compiler");
        }

        public static Logger CreateLogger(string folderPath, string log_name)
        {
            return new Logger(Path.Combine(folderPath, string.Format("{0}.log", log_name)));
        }

        public Logger(Stream stream)
        {
            this.stream = stream;
            writer = new StreamWriter(stream);
            onWriteEvent = new OnWriteDeleagte(OnWrite);
        }

        public void Flush()
        {
            writer.Flush();
        }

        public void Close()
        {
            stream.Close();
            writer.Close();
        }

        #region WriteFunctions
        public void Write(object o)
        {
            Write(LoggerMessageVerbosity.Info, o.ToString());
        }

        public void Write(string str)
        {
            Write(LoggerMessageVerbosity.Info, str);
        }

        public void WriteFormat(string str, params object[] args)
        {
            string message = string.Format(str, args);
            Write(LoggerMessageVerbosity.Info, str);
        }

        public void Write(LoggerMessageVerbosity verb, object o)
        {
            Write(verb, o.ToString());
        }

        public void Write(LoggerMessageVerbosity verb, string str)
        {
            string message = str;
            writer.Write(message);
            onWriteEvent(verb, message);
        }

        #endregion

        #region WriteLineFunctions
        public void WriteLine(object o)
        {
            WriteLine(LoggerMessageVerbosity.Info, o.ToString());
        }

        public void WriteLine(string str)
        {
            WriteLine(LoggerMessageVerbosity.Info, str);
        }

        public void WriteLineFormat(string str, params object[] args)
        {
            string message = string.Format(str, args);
            WriteLine(LoggerMessageVerbosity.Info, message);
        }

        public void WriteLine(LoggerMessageVerbosity verb, object o)
        {
            WriteLine(verb, o.ToString());
        }

        public void WriteLine(LoggerMessageVerbosity verb, string str)
        {
            string message = str;
            writer.WriteLine(message);
            onWriteEvent(verb, message);
        }
        #endregion




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
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    Close();
                }

                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.
                
                //-------------------------------------

                // Note disposing has been done.
                disposed = true;

            }
        }

        // Use C# destructor syntax for finalization code.
        // This destructor will run only if the Dispose method
        // does not get called.
        // It gives your base class the opportunity to finalize.
        // Do not provide destructors in types derived from this class.
        ~Logger()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }

    }
}
