using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvalonDock.Layout;
using System.Windows.Media.Imaging;
using MessageWindow;
using Schematix.Core.Compiler;
using System.Windows.Threading;

namespace Schematix.Panels
{
    /// <summary>
    /// Interaction logic for MessageWindowPanel.xaml
    /// </summary>
    public partial class MessageWindowPanel : SchematixPanelBase
    {
        public MessageWindow.MessageWindow MessageWindow
        {
            get { return messageWindow; }
        }

        public MessageWindowPanel()
        {
            InitializeComponent();
            messageWindow.MessageSelected += new global::MessageWindow.MessageWindow.MessageSelectedDelegate(messageWindow_MessageSelected);
            this.IconSource = new BitmapImage(new Uri("Images/Message.png", UriKind.Relative));
        }

        /// <summary>
        /// Пользователь выбрал сообщение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void messageWindow_MessageSelected(object sender, MessageItem e)
        {
            SchematixCore.Core.OpenSource(e.FileName, true, null, e.LineNumber, e.Number);
        }

        /// <summary>
        /// Добавление нового сообщения
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="fileName"></param>
        public void AddMessage(DiagnosticMessage msg)
        {
            messageWindow.Items.Add(new MessageItem(msg.MessageType, messageWindow.Items.Count, msg.Message, msg.Position.FileName, msg.Position.LineNumber));
            //messageWindow.UpdateBinding();
        }

        /// <summary>
        /// Обновить привязку данных
        /// </summary>
        public void UpdateBinding()
        {
            messageWindow.UpdateBinding();
        }

        /// <summary>
        /// Добавление нового сообщения
        /// </summary>
        /// <param name="message"></param>
        /// <param name="fileName"></param>
        /// <param name="lineNumber"></param>
        /// <param name="linePosition"></param>
        public void AddMessage(string message, string fileName, int lineNumber, int linePosition)
        {
            if (message.ToLower().Contains("error"))
            {
                messageWindow.Items.Add(new MessageItem(MessageType.Error, messageWindow.Items.Count, message, fileName, lineNumber));
                messageWindow.UpdateBinding();
                return;
            }
            if (message.ToLower().Contains("warning"))
            {
                messageWindow.Items.Add(new MessageItem(MessageType.Warning, messageWindow.Items.Count, message, fileName, lineNumber));
                messageWindow.UpdateBinding();
                return;
            }
            messageWindow.Items.Add(new MessageItem(MessageType.Message, messageWindow.Items.Count, message, fileName, lineNumber));
            //messageWindow.UpdateBinding();
        }

        public void ClearMessages()
        {
            messageWindow.Items.Clear();
            messageWindow.UpdateBinding();
        }

        /// <summary>
        /// Обновить перечень сообщений в окне
        /// </summary>
        /// <param name="newMessages"></param>
        public void UpdateMessages(IList<DiagnosticMessage> newMessages)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(
                    delegate
                    {
                        ClearMessages();
                        foreach (DiagnosticMessage msg in newMessages)
                            AddMessage(msg);
                        UpdateBinding();
                    }
                    )
                );
        }
    }
}
