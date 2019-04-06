using System;
using System.Collections.Generic;
using System.Text;

namespace IRC.Client.Base
{
    [Serializable]
    public sealed class SessionMessageEventArgs: EventArgs
    {
        public string Sender { get; private set; }
        public FormattedText Text { get; private set; }

        /// <summary>
        /// Создание аргументов
        /// </summary>
        /// <param name="message">Полученное сообщение</param>
        public SessionMessageEventArgs(string sender, string text)
        {
            this.Sender = sender;
            this.Text = new FormattedText(text, 1, 0);
        }
    }
}
