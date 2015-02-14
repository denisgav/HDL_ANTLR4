using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Schematix.Panels
{
    /// <summary>
    /// Interaction logic for ConsolePanel.xaml
    /// </summary>
    public partial class ConsolePanel : SchematixPanelBase
    {
        private string program, arguments;

        /// <summary>
        /// Использовать объект ProcessInterface для запуска внешнего приложения
        /// </summary>
        public ProcessInterface.ProcessInterface ProcessInterface
        {
            get { return console.ProcessInterface; }
        }

        public ConsolePanel(string program, string arguments)
            : this()
        {
            this.program = program;
            this.arguments = arguments;
            InitializeComponent();
        }

        public ConsolePanel()
        {
            InitializeComponent();
            console.Load += new EventHandler(console_Load);
            this.IconSource = new BitmapImage(new Uri("Images/Console.ico", UriKind.Relative));
        }

        void console_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(program) == false)
                console.StartProcess(program, arguments);
            //TestConsole();
        }

        void TestConsole()
        {
            string AppName = "ghdl";
            string Switch = "-a -fexplicit ";
            string args = "\"C:\\Users\\Denis\\Documents\\Schematix\\Solution1\\Project1\\importatntFile.vhdl\"";
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(AppName, Switch + args);
            psi.WorkingDirectory = System.Diagnostics.Process.GetCurrentProcess().StartInfo.WorkingDirectory;
            ProcessInterface.StartProcess(psi);
        }
    }
}
