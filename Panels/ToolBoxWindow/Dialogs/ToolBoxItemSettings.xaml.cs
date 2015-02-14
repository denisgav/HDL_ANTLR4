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
    public partial class ToolBoxItemProperty: Window
    {
        public ToolBoxItem ToolBoxItem  { get; private set; }

        public ToolBoxItemProperty(ToolBoxItem ToolBoxItem)
        {
            this.ToolBoxItem = ToolBoxItem;
            InitializeComponent();

            Title = "Update ToolboxItem Data";

            TextBoxCaption.Text = ToolBoxItem.Caption;
            TextBoxDescription.Text = ToolBoxItem.Description;
            TextBoxCommand.Text = ToolBoxItem.Command;

            LoadIcons();
            for (int i = 0; i < ExpanderData.Icons.Length; i++)
            {
                if (ExpanderData.Icons[i].Equals(ToolBoxItem.GetOriginalIconPath()))
                {
                    ComboBoxIconPath.SelectedIndex = i;
                    break;
                }
            }
        }

        public ToolBoxItemProperty(ExpanderData expanderData)
        {
            ToolBoxItem = new ToolBoxItem(expanderData);
            InitializeComponent();
            Title = "New ToolboxItem Data";
            LoadIcons();
            ComboBoxIconPath.SelectedIndex = 0;
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
            ToolBoxItem.Caption = TextBoxCaption.Text;
            ToolBoxItem.Description = TextBoxDescription.Text;
            ToolBoxItem.IconPath = ExpanderData.Icons[ComboBoxIconPath.SelectedIndex];
            ToolBoxItem.Command = TextBoxCommand.Text;
            DialogResult = true;
            Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
