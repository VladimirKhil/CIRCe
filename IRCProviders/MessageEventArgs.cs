using System;
using System.Collections.Generic;
using System.Text;

namespace IRCProviders
{
    /// <summary>
    /// Параметры полученного сообщения
    /// </summary>
    public class MessageEventArgs: EventArgs
    {
        string message = string.Empty;
        string sender = string.Empty;

        /// <summary>
        /// Текст сообщения
        /// </summary>
        public string Message
        {
            get { return message; }
        }

        /// <summary>
        /// Отправитель
        /// </summary>
        public string Sender
        {
            get { return sender; }
        }

        /// <summary>
        /// Создать параметры сообщения
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        /// <param name="sender">Отправитель</param>
        public MessageEventArgs(string message, string sender)
        {
            this.message = message;
            this.sender = sender;
        }
    }
}
