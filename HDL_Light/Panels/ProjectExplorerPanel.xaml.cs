using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvalonDock.Layout;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Schematix.ProjectExplorer;

namespace Schematix.Panels
{
    /// <summary>
    /// Interaction logic for ProjectExplorerPanel.xaml
    /// </summary>
    public partial class ProjectExplorerPanel : SchematixPanelBase
    {
        /// <summary>
        /// Текущее решение проекта
        /// </summary>
        public Solution Solution
        {
            get { return core.Solution; }
        }

        public ProjectExplorerControl ProjectExplorerContro
        {
            get { return projectExplorerControl; }
        }

        /// <summary>
        /// Главный узел
        /// </summary>
        private SchematixCore core;
        public SchematixCore Core
        {
            get { return core; }
        }


        public ProjectExplorerPanel()
            : this(SchematixCore.Core)
        { }

        public ProjectExplorerPanel(SchematixCore core)
        {
            InitializeComponent();
            this.core = core;
            this.IconSource = new BitmapImage(new Uri("Images/OpenProject.png", UriKind.Relative));
        }

        /// <summary>
        /// обновить отображение
        /// </summary>
        public void UpdateInfo()
        {
            projectExplorerControl.UpdateInfo();
        }
    }
}
