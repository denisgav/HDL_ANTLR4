using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MessageWindow
{
    public enum MessageType
    {
        Warning,
        Error,
        Message
    };

    public class MessageItem
    {
        private MessageType messageType; //тип сообщения
        public MessageType MessageType
        {
            get
            {
                return messageType;
            }
            set
            {
                messageType = value;
            }
        }

        /*public System.Windows.UIElement MessageIcon
        {
            get
            {
                System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                if (messageType == MessageType.Error)
                {
                    BitmapImage bi3 = new BitmapImage();
                    bi3.BeginInit();
                    bi3.UriSource = new Uri("Icons/error.png", UriKind.Relative);
                    bi3.EndInit();
                    image.Stretch = Stretch.Fill;
                    image.Source = bi3;
                }
                if (messageType == MessageType.Warning)
                {
                    BitmapImage bi3 = new BitmapImage();
                    bi3.BeginInit();
                    bi3.UriSource = new Uri("Icons/warning.png", UriKind.Relative);
                    bi3.EndInit();
                    image.Stretch = Stretch.Fill;
                    image.Source = bi3;
                }
                if (messageType == MessageType.Message)
                {
                    BitmapImage bi3 = new BitmapImage();
                    bi3.BeginInit();
                    bi3.UriSource = new Uri("Icons/message.png", UriKind.Relative);
                    bi3.EndInit();
                    image.Stretch = Stretch.Fill;
                    image.Source = bi3;
                }
                return image;
            }
        }*/

        public string MessageIconPath
        {
            get
            {
                string res = string.Empty;
                if (messageType == MessageType.Error)
                    res = "/MessageWindow;component/Icons/error.png";
                if(messageType == MessageType.Warning)
                    res = "/MessageWindow;component/Icons/warning.png";
                if (messageType == MessageType.Message)
                    res = "/MessageWindow;component/Icons/message.png";
                return res;
            }
        }

        private int number; //номер записи
        public int Number
        {
            get
            {
                return number;
            }
            set
            {
                number = value;
            }
        }

        private string description; //описание
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }

        private string fileName; // имя файла
        public string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                fileName = value;
            }
        }

        private int lineNumber; //номер строки в файле
        public int LineNumber
        {
            get
            {
                return lineNumber;
            }
            set
            {
                lineNumber = value;
            }
        }

        public MessageItem()
        {
            messageType = MessageType.Message;
            number = 0;
            description = "some description";
            fileName = "some file";
            lineNumber = 0;
        }

        public MessageItem(MessageType messageType, int number, string description, string fileName, int lineNumber)
        {
            this.messageType = messageType;
            this.number = number;
            this.description = description;
            this.fileName = fileName;
            this.lineNumber = lineNumber;
        }
    }
}
