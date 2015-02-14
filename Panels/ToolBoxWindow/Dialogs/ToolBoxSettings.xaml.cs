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
    public partial class ToolBoxWindowSettings : Window
    {
        public List<ExpanderData> expanderList;

        public ToolBoxWindowSettings()
        {
            expanderList = ExpanderData.LoadUserData();
            InitializeComponent();

            ListViewGroups.ItemsSource = expanderList;
        }

        private void ListViewGroupClick(object sender, MouseButtonEventArgs e)
        {
            ExpanderData data = ((ListViewItem)sender).Content as ExpanderData;
            if(data != null)
                ListViewElements.ItemsSource = data.Items;
        }

        private void ButtonDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            if ((ListViewElements.SelectedItem != null) && (ListViewElements.SelectedItem is ToolBoxItem))
            {
                ToolBoxItem item = ListViewElements.SelectedItem as ToolBoxItem;
                if (item != null)
                {
                    //Находим группу которой принадлежит выделенный элемент
                    ExpanderData selectedGroup = null;
                    foreach (ExpanderData data in expanderList)
                    {
                        if (data.Items.Contains(item))
                        {
                            selectedGroup = data;
                            break;
                        }
                    }

                    selectedGroup.Items.Remove(item);
                    ListViewElements.ItemsSource = null;
                    ListViewElements.ItemsSource = selectedGroup.Items;
                }
            }
        }

        private void ButtonUpdateItem_Click(object sender, RoutedEventArgs e)
        {
            if ((ListViewElements.SelectedItem != null) && (ListViewElements.SelectedItem is ToolBoxItem))
            {
                ToolBoxItem item = ListViewElements.SelectedItem as ToolBoxItem;
                if (item != null)
                {
                    //Находим группу которой принадлежит выделенный элемент
                    ExpanderData selectedGroup = null;
                    foreach (ExpanderData data in expanderList)
                    {
                        if (data.Items.Contains(item))
                        {
                            selectedGroup = data;
                            break;
                        }
                    }

                    ToolBoxItemProperty tprop = new ToolBoxItemProperty(item);
                    if (tprop.ShowDialog() == true)
                    {
                        ListViewElements.ItemsSource = null;
                        selectedGroup.Items.Remove(item);
                        selectedGroup.Items.Add(tprop.ToolBoxItem);
                        ListViewElements.ItemsSource = selectedGroup.Items;
                    }
                }
            }
        }

        private void ButtonNewItem_Click(object sender, RoutedEventArgs e)
        {
            if ((ListViewGroups.SelectedItem != null) && (ListViewGroups.SelectedItem is ExpanderData))
            {
                ExpanderData selectedGroup = ListViewGroups.SelectedItem as ExpanderData;
                if (selectedGroup != null)
                {
                    ToolBoxItemProperty tprop = new ToolBoxItemProperty(selectedGroup);
                    if (tprop.ShowDialog() == true)
                    {
                        ListViewElements.ItemsSource = null;
                        selectedGroup.Items.Add(tprop.ToolBoxItem);
                        ListViewElements.ItemsSource = selectedGroup.Items;
                    }
                }
            }
        }

        private void ButtonDeleteGroup_Click(object sender, RoutedEventArgs e)
        {
            if ((ListViewGroups.SelectedItem != null) && (ListViewGroups.SelectedItem is ExpanderData))
            {
                ExpanderData data = ListViewGroups.SelectedItem as ExpanderData;
                if (data != null)
                {
                    expanderList.Remove(data);
                    ListViewGroups.ItemsSource = null;
                    ListViewGroups.ItemsSource = expanderList;
                    ListViewElements.ItemsSource = null;
                }
            }
        }

        private void ButtonUpdateGroup_Click(object sender, RoutedEventArgs e)
        {
            if ((ListViewGroups.SelectedItem != null) && (ListViewGroups.SelectedItem is ExpanderData))
            {
                ExpanderData data = ListViewGroups.SelectedItem as ExpanderData;
                if (data != null)
                {
                    ExpanderItem window = new ExpanderItem(data);
                    if (window.ShowDialog() == true)
                    {
                        ExpanderData newdata = window.ExpanderData;
                        expanderList.Remove(data);
                        expanderList.Add(newdata);
                        ListViewGroups.ItemsSource = null;
                        ListViewGroups.ItemsSource = expanderList;
                    }
                }
            }
        }

        private void ButtonNewGroup_Click(object sender, RoutedEventArgs e)
        {
            ExpanderItem window = new ExpanderItem();
            if (window.ShowDialog() == true)
            {
                ExpanderData data = window.ExpanderData;
                expanderList.Add(data);
                ListViewGroups.ItemsSource = null;
                ListViewGroups.ItemsSource = expanderList;
            }
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            ExpanderData.SaveToXML(expanderList);
            DialogResult = true;
            Close();
        }
    }
}
