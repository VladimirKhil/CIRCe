using System;
using System.Collections.Generic;
using System.Text;

namespace IRCProviders
{
    /// <summary>
    /// Аргументы сообщения IRC-протокола
    /// </summary>
    [Serializable]
    public class IRCMessageEventArgs: EventArgs
    {
        private IRCMessage message = null;

        /// <summary>
        /// Полученное сообщение
        /// </summary>
        public IRCMessage Message { get { return message; } }
        /// <summary>
        /// Отмена стандартной реакции клиента на сообщение (выставить в true)
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Создание аргументов
        /// </summary>
        /// <param name="message">Полученное сообщение</param>
        public IRCMessageEventArgs(IRCMessage message)
        {
            this.message = message;
            this.Cancel = false;
        }
    }
}
