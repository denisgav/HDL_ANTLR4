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
    /// Interaction logic for DesignBrowserPanel.xaml
    /// </summary>
    public partial class DesignBrowserPanel : SchematixPanelBase
    {
        /// <summary>
        /// Текущее решение проекта
        /// </summary>
        public Solution Solution
        {
            get { return core.Solution; }
        }

        public DesignBrowserTree DesignBrowser
        {
            get { return designBrowser; }
        }

        /// <summary>
        /// Главный узел
        /// </summary>
        private SchematixCore core;
        public SchematixCore Core
        {
            get { return core; }
        }

        public DesignBrowserPanel()
            : this(SchematixCore.Core)
        { }

        public DesignBrowserPanel(SchematixCore core)
        {
            InitializeComponent();
            this.Title = "Design Browser";
            this.IconSource = new BitmapImage(new Uri("Images/Design/chip.png", UriKind.Relative));
        }

        /// <summary>
        /// обновить отображение
        /// </summary>
        public void UpdateInfo()
        {
            designBrowser.UpdateInfo();
        }
    }
}
