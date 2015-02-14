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
using AvalonDock.Layout;

namespace Schematix.Panels
{
    /// <summary>
    /// Interaction logic for SearchReplace.xaml
    /// </summary>
    public partial class SearchReplace : SchematixPanelBase
    {
        public SearchReplace()
        {
            InitializeComponent();
            this.IconSource = new BitmapImage(new Uri("Images/Search.png", UriKind.Relative));
        }

        public void Refresh()
        {
            SearchReplaceUserControl1.Refresh();
        }
    }
}
