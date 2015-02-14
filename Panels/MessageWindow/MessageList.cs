using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MessageWindow
{
    public class MessageList //отвечает за заполнение списка сообщений
    {
        public List<MessageItem> messages;
        public bool ShowErrors = true;
        public bool ShowWarnings = true;
        public bool ShowMessages = true;

        public MessageList(List<MessageItem> messages)
        {
            this.messages = new List<MessageItem>();
            this.messages.AddRange(messages);
        }
        public MessageList()
        {
            this.messages = new List<MessageItem>();
        }

        public List<MessageItem> GetMessages()
        {
            List<MessageItem> res = new List<MessageItem>();
            foreach (MessageItem item in messages)
            {
                if(
                    ((item.MessageType == MessageType.Error) && (ShowErrors == true)) ||
                    ((item.MessageType == MessageType.Warning) && (ShowWarnings == true)) ||
                    ((item.MessageType == MessageType.Message) && (ShowMessages == true))
                  )
                    res.Add(item);
            }
            return res;
        }
    }
}
