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
    /// Interaction logic for CorrectSelectedTime.xaml
    /// </summary>
    public partial class CorrectSelectedTime : Window
    {
        private Selection selection;
        private ScaleManager scaleManager;

        public CorrectSelectedTime(Selection selection, ScaleManager scaleManager)
        {
            this.scaleManager = scaleManager;
            this.selection = selection;
            InitializeComponent();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            selection.X1 = timeUnitUserControlStart.TimeInterval.GetTimeUnitInFS();
            selection.X2 = timeUnitUserControlEnd.TimeInterval.GetTimeUnitInFS();
            DialogResult = true;
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timeUnitUserControlStart.TimeInterval = new TimeInterval(selection.Start);
            timeUnitUserControlEnd.TimeInterval = new TimeInterval(selection.End);
        }

        private void ButtonSelectAll_Click(object sender, RoutedEventArgs e)
        {
            timeUnitUserControlStart.TimeInterval = new TimeInterval(scaleManager.StartTime);
            timeUnitUserControlEnd.TimeInterval = new TimeInterval(scaleManager.EndTime);
        }
    }
}
