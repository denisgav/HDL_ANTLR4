using System;
using MessageWindow;

namespace Schematix.Core.Compiler
{
    public struct DiagnosticMessage
    {
        /// <summary>
        /// Задана ли позиция в коде
        /// </summary>
        public bool isPointed
        {
            get { return (position.LineNumber != -1) && (position.ColumnNumber != -1); }
        }

        /// <summary>
        /// Тип сообщения
        /// </summary>
        private MessageType messageType;
        public MessageType MessageType
        {
            get { return messageType; }
            set { messageType = value; }
        }
        

        /// <summary>
        /// Позиция
        /// </summary>
        private SourcePosition position;
        public SourcePosition Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Само мообщение
        /// </summary>
        private string message;
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public DiagnosticMessage(string message, SourcePosition position, MessageType messageType)
        {
            this.message = message;
            this.position = position;
            this.messageType = messageType;
        }

        public DiagnosticMessage(SourcePosition errorPosition, string msg)
        {
            this.position = errorPosition;
            this.message = msg;
            this.messageType = MessageType.Error;
        }
        public DiagnosticMessage (string msg)
        {
            this.position = new SourcePosition("", -1, -1);
            this.message = msg;
            this.messageType = MessageType.Message;
        }
        public DiagnosticMessage(SourcePosition errorPosition, string msg, bool isWarning)
        {
            this.position = errorPosition;
            this.message = msg;
            this.messageType = isWarning ? MessageType.Warning : MessageType.Error;
        }
        public DiagnosticMessage(string msg, bool isWarning)
        {
            this.position = new SourcePosition("", -1, -1);
            this.message = msg;
            this.messageType = MessageType.Error;
        }
    }

    /// <summary>
    /// Структура, описывающая позицию в тексте
    /// </summary>
    public struct SourcePosition
    {
        /// <summary>
        /// номер строки
        /// </summary>
        private int lineNumber;
        public int LineNumber
        {
            get { return lineNumber; }
            set { lineNumber = value; }
        }

        /// <summary>
        /// номер столбца
        /// </summary>
        private int columnNumber;
        public int ColumnNumber
        {
            get { return columnNumber; }
            set { columnNumber = value; }
        }

        /// <summary>
        /// Имя файла
        /// </summary>
        private string fileName;
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        

        public SourcePosition(string fileName, int lineNumber, int columnNumber)
        {
            this.fileName = fileName;
            this.lineNumber = lineNumber;
            this.columnNumber = columnNumber;
        }        
    }
}