using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Configure log4net using the .config file
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
// This will cause log4net to look for a configuration file
// called ConsoleApp.exe.config in the application base
// directory (i.e. the directory containing ConsoleApp.exe)

namespace Schematix.Core
{
    /// <summary>
    /// Используется для ведения лога событий
    /// </summary>
    public static class Logger
    {
        // Create a logger for use in this class
        private static readonly log4net.ILog log;

        static Logger()
        {
            log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }

        /// <summary>
        /// Получить системный логгер
        /// </summary>
        public static log4net.ILog Log
        {
            get { return log; }
        }
    }
}
