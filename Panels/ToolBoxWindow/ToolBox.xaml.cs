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
    /// Interaction logic for ToolBox.xaml
    /// </summary>
    public partial class ToolBox : UserControl
    {
        public List<ExpanderData> expanderList;
        private List<ExpanderData> sortedList;
        private System.Windows.Window Popup;
        public ToolBoxItem selectedToolBoxItem = null;
        public string SelectedType;

        public event ToggleButtonClickDelegate ToggleButtonClick
        {
            add
            {
                click += value;
            }
            remove
            {
                click -= value;
            }

        }
        public delegate void ToggleButtonClickDelegate(object sender, EventArgs e);
        private ToggleButtonClickDelegate click;

        private void LoadAllData()
        {
            string CurDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            expanderList = ExpanderData.LoadUserData();
            
            expanderList.AddRange(ExpanderData.LoadSystemData());
        }

        public ToolBox()
        {
            InitializeComponent();
            
            //System.Reflection.Assembly asm = System.Reflection.Assembly.GetAssembly(GetType());
            //LoadExpanders(expanderList);
            LoadAllData();
        }

        public void SelectContent(string type)
        {
            SelectedType = type;
            sortedList = new List<ExpanderData>();
            foreach (ExpanderData d in expanderList)
            {
                if (d.TypeOfElements == type)
                    sortedList.Add(d);
            }
            LoadExpanders(sortedList);
        }

        public void LoadExpanders(List<ExpanderData> List)
        {
            sortedList = List;
            MainStackPanel.Children.Clear();
            foreach (ExpanderData data in List)
            {
                data.CreateExpander();
                foreach (ToolBoxItem item in data.Items)
                {
                    item.ToggleButton.Style = MainStackPanel.FindResource("MyToggleButtonStyle") as System.Windows.Style;
                }
                MainStackPanel.Children.Add(data.Expander);
            }
        }

        private void UncheckAllButtons()
        {
            foreach (ExpanderData data in sortedList)
            {
                foreach (ToolBoxItem item in data.Items)
                {
                    item.ToggleButton.IsChecked = false;
                }
            }
        }

        private void CreatePopupWindow()
        {
            if (selectedToolBoxItem == null)
                return;

            Popup = new System.Windows.Window();

            Popup.Topmost = true;

            Border border = new Border();
            border.BorderBrush = Brushes.Black;
            border.Background = System.Windows.Media.Brushes.WhiteSmoke;
            border.BorderThickness = new Thickness(2);

            Popup.Content = border;
            border.Child = selectedToolBoxItem.CreateToggleButtonContent();
            Popup.Width = selectedToolBoxItem.ToggleButton.ActualWidth;
            Popup.Height = selectedToolBoxItem.ToggleButton.ActualHeight + 12;
            Popup.WindowStyle = WindowStyle.None;
            Popup.ShowInTaskbar = false;
            Popup.PreviewMouseUp += new MouseButtonEventHandler(Popup_PreviewMouseUp);
            Popup.CaptureMouse();
        }

        void Popup_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Popup.ReleaseMouseCapture();
            Popup.Close();
            Popup = null;
        }

        private void ToggleButtonMouseDown(object sender, MouseButtonEventArgs e)
        {
            UncheckAllButtons();
            System.Windows.Controls.Primitives.ToggleButton selected_button = (sender as System.Windows.Controls.Primitives.ToggleButton);
            selected_button.IsChecked = true;

            foreach (ExpanderData data in sortedList)
            {
                foreach (ToolBoxItem item in data.Items)
                {
                    if (item.ToggleButton == selected_button)
                    {
                        selectedToolBoxItem = item;
                        break;
                    }
                }
            }
            e.Handled = true;
        }

        private void ToggleButtonMouseUp(object sender, RoutedEventArgs e)
        {
            UncheckAllButtons();
            System.Windows.Controls.Primitives.ToggleButton selected_button = (sender as System.Windows.Controls.Primitives.ToggleButton);
            selected_button.IsChecked = true;

            foreach (ExpanderData data in sortedList)
            {
                foreach (ToolBoxItem item in data.Items)
                {
                    if (item.ToggleButton == selected_button)
                    {
                        selectedToolBoxItem = item;
                        break;
                    }
                }
            }

            click(this, new EventArgs());
        }

        private void ToggleButtonMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;
            Point pt = e.GetPosition(this);
            pt = this.PointToScreen(pt);

            if ((Popup == null) || (Popup.Visibility != Visibility.Visible))
            {
                CreatePopupWindow();
                Popup.Left = pt.X;
                Popup.Top = pt.Y;
                Popup.Show();
                Popup.DragMove();


                DataObject data = selectedToolBoxItem.GetData(SelectedType);
                DragDrop.DoDragDrop(this, data, DragDropEffects.Copy);
            }
        }

        private void MenuItemProperties_Click(object sender, RoutedEventArgs e)
        {
            Window toolboxSettings = new ToolBoxWindow.ToolBoxWindowSettings();
            if (toolboxSettings.ShowDialog() == true)
            {
                expanderList.Clear();
                MainStackPanel.Children.Clear();
                LoadAllData();
                SelectContent(SelectedType);
            }
        }
    }
}
