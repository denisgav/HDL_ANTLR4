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

namespace ToolBoxWindow
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ExpanderItem: Window
    {
        public ExpanderData ExpanderData { get; private set; }
        public ExpanderItem(ExpanderData ExpanderData)
        {
            this.ExpanderData = ExpanderData;
            InitializeComponent();
            Title = "Update Expander Data For " + ExpanderData.Caption;

            TextBoxCaption.Text = ExpanderData.Caption;
            TextBoxDescription.Text = ExpanderData.Description;

            LoadIcons();
            for (int i = 0; i < ExpanderData.Icons.Length; i++)
            {
                if (ExpanderData.Icons[i].Equals(ExpanderData.GetOriginalIconPath()))
                {
                    ComboBoxIconPath.SelectedIndex = i;
                    break;
                }
            }

            ComboBoxTypeOfElements.ItemsSource = ExpanderData.TypeOfElementsAlloved;
            ComboBoxTypeOfElements.SelectedItem = ExpanderData.TypeOfElements;
        }

        public ExpanderItem()
        {
            ExpanderData = new ExpanderData();
            InitializeComponent();
            Title = "New Expander Data";
            LoadIcons();
            ComboBoxIconPath.SelectedIndex = 0;
            ComboBoxTypeOfElements.ItemsSource = ExpanderData.TypeOfElementsAlloved;
            ComboBoxTypeOfElements.SelectedItem = ExpanderData.TypeOfElements;
        }

        private void LoadIcons()
        {
            ComboBoxIconPath.Items.Clear();
            foreach (string icon in ExpanderData.Icons)
            {
                System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                System.Windows.Media.Imaging.BitmapImage bi3 = new System.Windows.Media.Imaging.BitmapImage();
                bi3.BeginInit();
                bi3.UriSource = new Uri("pack://application:,,,/ToolBoxWindow;component/Icons/" + icon);
                bi3.EndInit();
                image.Stretch = System.Windows.Media.Stretch.Fill;
                image.Source = bi3;
                image.Width = 32;
                image.Height = 32;
                ComboBoxIconPath.Items.Add(image);
            }
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            ExpanderData.Caption = TextBoxCaption.Text;
            ExpanderData.Description = TextBoxDescription.Text;
            ExpanderData.IconPath = ExpanderData.Icons[ComboBoxIconPath.SelectedIndex];
            ExpanderData.TypeOfElements = ExpanderData.TypeOfElementsAlloved[ComboBoxTypeOfElements.SelectedIndex];
            DialogResult = true;
            Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
