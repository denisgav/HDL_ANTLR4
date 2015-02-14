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
using System.ComponentModel;
using System.Windows.Controls.Primitives;

namespace MessageWindow
{
    /// <summary>
    /// Interaction logic for MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow : UserControl
    {
        private MessageList messageList = new MessageList();

        #region MessageSelected event
        public delegate void MessageSelectedDelegate(object sender, MessageItem e);
        private event MessageSelectedDelegate messageSelectedEvent;
        public event MessageSelectedDelegate MessageSelected
        {
            add { messageSelectedEvent += value; }
            remove { messageSelectedEvent -= value; }
        }
        private void MessageSelected_DefaultHandler(object sender, MessageItem e)
        { }
        #endregion

        public List<MessageItem> Items
        {
            get
            {
                return messageList.messages;
            }
            set
            {
                messageList.messages = value;
                UpdateBinding();
            }
        }

        private MessageItem selectedMessge;
        public MessageItem SelectedMessage
        {
            get
            {
                return selectedMessge;
            }
        }

        public string NumErrors
        {
            get
            {
                int n = 0;
                if (messageList.messages != null)
                {
                    foreach (MessageItem item in messageList.messages)
                    {
                        if (item.MessageType == MessageType.Error)
                            n++;
                    }
                }
                if (n == 0)
                    return "Errors";
                else
                    return "Errors (" + n.ToString() + ")";
            }
        }

        public string NumWarnings
        {
            get
            {
                int n = 0;
                if (messageList.messages != null)
                {
                    foreach (MessageItem item in messageList.messages)
                    {
                        if (item.MessageType == MessageType.Warning)
                            n++;
                    }
                }
                if (n == 0)
                    return "Warnings";
                else
                    return "Warnings (" + n.ToString() + ")";
            }
        }

        public string NumMessages
        {
            get
            {
                int n = 0;
                if (messageList.messages != null)
                {
                    foreach (MessageItem item in messageList.messages)
                    {
                        if (item.MessageType == MessageType.Message)
                            n++;
                    }
                }
                if (n == 0)
                    return "Messages";
                else
                    return "Messages (" + n.ToString() + ")";
            }
        }

        public MessageWindow()
        {
            InitializeComponent();
            messageSelectedEvent = new MessageWindow.MessageSelectedDelegate(MessageSelected_DefaultHandler);
            UpdateBinding();
        }

        public void UpdateBinding()
        {
            listView.ItemsSource = messageList.GetMessages() as System.Collections.IEnumerable;
            ErrorTextBlock.Text = NumErrors;
            WarningTextBlock.Text = NumWarnings;
            MessageTextBlock.Text = NumMessages;

            Button_Checked(ErrorButton);
            Button_Checked(WarningButton);
            Button_Checked(MessageButton);
        }

        private void ErrorButton_Click(object sender, RoutedEventArgs e)
        {
            messageList.ShowErrors = ErrorButton.IsChecked == true;
            Button_Checked(ErrorButton);
            UpdateBinding();
        }

        private void WarningButton_Click(object sender, RoutedEventArgs e)
        {
            messageList.ShowWarnings = WarningButton.IsChecked == true;
            Button_Checked(WarningButton);
            UpdateBinding();
        }

        private void MessageButton_Click(object sender, RoutedEventArgs e)
        {
            messageList.ShowMessages = MessageButton.IsChecked == true;
            Button_Checked(MessageButton);
            UpdateBinding();
        }

        protected void HandleDoubleClick(object sender, MouseButtonEventArgs e)
        {
            selectedMessge = ((ListViewItem)sender).Content as MessageItem;
            messageSelectedEvent(this, selectedMessge);
        }

        #region Sorting

        private ListSortDirection num_direction = ListSortDirection.Ascending;
        private GridViewColumnHeader last_column;
        private void GridViewColumnHeaderNumber_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = sender as GridViewColumnHeader;
            String field = column.Tag as String;

            listView.Items.SortDescriptions.Clear();

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (num_direction == ListSortDirection.Ascending)
                newDir = ListSortDirection.Descending;

            num_direction = newDir;
            listView.Items.SortDescriptions.Add(
                new SortDescription(field, newDir));

            if (num_direction == ListSortDirection.Ascending)
            {
                column.Column.HeaderTemplate =
                  Resources["HeaderTemplateArrowUp"] as DataTemplate;
            }
            else
            {
                column.Column.HeaderTemplate =
                  Resources["HeaderTemplateArrowDown"] as DataTemplate;
            }
            if (last_column != null && last_column != column)
            {
                last_column.Column.HeaderTemplate = null;
            }

            last_column = column;
        }

        private ListSortDirection desc_direction = ListSortDirection.Ascending;
        private void GridViewColumnHeaderDescription_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = sender as GridViewColumnHeader;
            String field = column.Tag as String;

            listView.Items.SortDescriptions.Clear();

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (desc_direction == ListSortDirection.Ascending)
                newDir = ListSortDirection.Descending;

            desc_direction = newDir;
            listView.Items.SortDescriptions.Add(
                new SortDescription(field, newDir));
            if (desc_direction == ListSortDirection.Ascending)
            {
                column.Column.HeaderTemplate =
                  Resources["HeaderTemplateArrowUp"] as DataTemplate;
            }
            else
            {
                column.Column.HeaderTemplate =
                  Resources["HeaderTemplateArrowDown"] as DataTemplate;
            }
            if (last_column != null && last_column != column)
            {
                last_column.Column.HeaderTemplate = null;
            }

            last_column = column;
        }

        private ListSortDirection file_direction = ListSortDirection.Ascending;
        private void GridViewColumnHeaderFile_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = sender as GridViewColumnHeader;
            String field = column.Tag as String;

            listView.Items.SortDescriptions.Clear();

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (file_direction == ListSortDirection.Ascending)
                newDir = ListSortDirection.Descending;

            file_direction = newDir;
            listView.Items.SortDescriptions.Add(
                new SortDescription(field, newDir));
            if (file_direction == ListSortDirection.Ascending)
            {
                column.Column.HeaderTemplate =
                  Resources["HeaderTemplateArrowUp"] as DataTemplate;
            }
            else
            {
                column.Column.HeaderTemplate =
                  Resources["HeaderTemplateArrowDown"] as DataTemplate;
            }
            if (last_column != null && last_column != column)
            {
                last_column.Column.HeaderTemplate = null;
            }

            last_column = column;
        }
        #endregion

        private void Button_Checked(object sender)
        {
            ToggleButton button = (ToggleButton)sender;
            if (button.IsChecked  == false)
            {
                Brush br = new SolidColorBrush(Color.FromArgb(50, 50, 50, 50));
                button.OpacityMask = br;
            }
            else
            {
                Brush br = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                button.OpacityMask = br;
            }
        }
    }
}
