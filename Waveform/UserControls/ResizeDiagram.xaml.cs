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
using DataContainer;

namespace Schematix.Waveform.UserControls
{
    /// <summary>
    /// Interaction logic for ResizeDiagram.xaml
    /// </summary>
    public partial class ResizeDiagram : Window
    {
        private ScaleManager scaleManager;
        public ResizeDiagram(ScaleManager scaleManager)
        {
            this.scaleManager = scaleManager;
            InitializeComponent();
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            scaleManager.EndTime = TimeUnitEndTime.TimeInterval.GetTimeUnitInFS();
            DialogResult = true;
            Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TimeUnitEndTime.TimeInterval = new TimeInterval(scaleManager.EndTime);
        }
    }
}
