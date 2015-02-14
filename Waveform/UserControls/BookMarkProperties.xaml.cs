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
using Schematix.Waveform.Value_Dump;
using DataContainer;

namespace Schematix.Waveform.UserControls
{
    /// <summary>
    /// Interaction logic for BookMarkProperties.xaml
    /// </summary>
    public partial class BookMarkProperties : Window
    {
        /// <summary>
        /// обрабатываемая заметка
        /// </summary>
        private BookMark bookMark;
        public BookMark BookMark
        {
            get { return bookMark; }
            set { bookMark = value; }
        }


        public BookMarkProperties(BookMark bookMark)
        {
            this.bookMark = bookMark;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TextBoxHeader.Text = bookMark.Header;
            TextBoxText.Text = bookMark.Text;
            TimeUnitUserControlTime.TimeInterval = new TimeInterval(bookMark.Time);
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            bookMark.Text = TextBoxText.Text;
            bookMark.Header = TextBoxHeader.Text;
            bookMark.Time = TimeUnitUserControlTime.TimeInterval.GetTimeUnitInFS();

            DialogResult = true;
            Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
