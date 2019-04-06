using System;
using System.Collections.Generic;
using System.Text;

namespace IRC.Client.Base
{
    /// <summary>
    /// Аргументы сообщения IRC-протокола
    /// </summary>
    [Serializable]
    public sealed class MessageEventArgs: EventArgs
    {
        /// <summary>
        /// Полученное сообщение
        /// </summary>
        public IMessage Message { get ; private set; }
        /// <summary>
        /// Отмена стандартной реакции клиента на сообщение (выставить в true)
        /// </summary>
        public object Sender { get; private set; }

        /// <summary>
        /// Создание аргументов
        /// </summary>
        /// <param name="message">Полученное сообщение</param>
        public MessageEventArgs(IMessage message, object sender)
        {
            this.Message = message;
            this.Sender = sender;
        }
    }
}
