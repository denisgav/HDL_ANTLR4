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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataContainer;

namespace Schematix.Waveform.UserControls
{
    /// <summary>
    /// Interaction logic for TimeUnitUserControl.xaml
    /// </summary>
    public partial class TimeUnitUserControl : UserControl
    {
        public TimeUnitUserControl()
        {
            InitializeComponent();
            ComboBoxClockTimeStepUnit.Items.Add(TimeUnit.fs);
            ComboBoxClockTimeStepUnit.Items.Add(TimeUnit.ps);
            ComboBoxClockTimeStepUnit.Items.Add(TimeUnit.us);
            ComboBoxClockTimeStepUnit.Items.Add(TimeUnit.ns);
            ComboBoxClockTimeStepUnit.Items.Add(TimeUnit.ms);
            ComboBoxClockTimeStepUnit.Items.Add(TimeUnit.s);
            ComboBoxClockTimeStepUnit.SelectedIndex = 0;
            TimeInterval = new TimeInterval(1, TimeUnit.fs);
        }

        /// <summary>
        /// Интервал времени, хранящийся в данном елементе управления
        /// </summary>
        public TimeInterval TimeInterval
        {
            get
            {
                TimeInterval timeInterval = new TimeInterval();
                timeInterval.TimeNumber = TextBoxClockTimeStep.GetIntegerValue();
                timeInterval.Unit = (TimeUnit)ComboBoxClockTimeStepUnit.SelectedItem;
                return timeInterval; 
            }
            set
            {
                TextBoxClockTimeStep.Text = value.TimeNumber.ToString();
                ComboBoxClockTimeStepUnit.SelectedItem = value.Unit;
            }
        }
    }
}
