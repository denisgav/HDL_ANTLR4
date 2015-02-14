using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvalonDock.Layout;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Schematix.ProjectExplorer;
using Schematix.DesignBrowser;

namespace Schematix.Panels
{
    /// <summary>
    /// Interaction logic for StartPagePanel.xaml
    /// </summary>
    public partial class StartPagePanel : SchematixPanelBase
    {
        private SchematixCore core;

        public StartPagePanel()
            : this(SchematixCore.Core)
        { }

        public StartPagePanel(SchematixCore core)
        {
            InitializeComponent();
            this.core = core;
            this.IconSource = new BitmapImage(new Uri("Images/StartPage.png", UriKind.Relative));
            Title = "Start Page";
            InitHandlers();
        }

        private void InitHandlers()
        {
            StartPageUserControl.OpenProject += new StartPage.StartPageUserControl.OpenProjectDelegate(StartPageUserControl_OpenProject);
            StartPageUserControl.OpenRecentProject += new StartPage.StartPageUserControl.OpenRecentProjectDelegate(StartPageUserControl_OpenRecentProject);
            StartPageUserControl.CreateProject += new StartPage.StartPageUserControl.CreateProjectDelegate(StartPageUserControl_CreateProject);
        }

        public void AddNewRecentProject(string filePath)
        {
            StartPageUserControl.AddNewRecentProject(filePath);
        }

        void StartPageUserControl_CreateProject(object sender)
        {
            //throw new NotImplementedException();
            Schematix.ProjectExplorer.Solution sol = core.CreateProject();
            if (sol != null)
                StartPageUserControl.AddNewRecentProject(sol.Path);
        }

        void StartPageUserControl_OpenRecentProject(object sender, string FilePath)
        {
            core.OpenProject(FilePath);
            Schematix.ProjectExplorer.Solution sol = core.Solution;
            if (sol != null)
                StartPageUserControl.AddNewRecentProject(sol.Path);
        }

        void StartPageUserControl_OpenProject(object sender)
        {
            core.OpenProject();
            Schematix.ProjectExplorer.Solution sol = core.Solution;
            if (sol != null)
                StartPageUserControl.AddNewRecentProject(sol.Path);
        }
    }
}
