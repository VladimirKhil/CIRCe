using System;
using System.Collections.Generic;
using System.Text;

namespace IRCWindow
{
    /// <summary>
    /// Параметры сгенерированного сообщения
    /// </summary>
    public sealed class EnterPushedEventArgs: EventArgs
    {
        private string message = "";

        /// <summary>
        /// Сгенерированное сообщение
        /// </summary>
        public string Message
        {
            get { return message; }
        }

        public EnterPushedEventArgs(string message)
        {
            this.message = message;
        }
    }
}
