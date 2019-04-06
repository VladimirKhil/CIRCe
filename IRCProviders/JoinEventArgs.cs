using System;
using System.Collections.Generic;
using System.Text;

namespace IRCProviders
{
    /// <summary>
    /// Аргументы присоединения к каналу/привату
    /// </summary>
    [Serializable]
    public sealed class JoinEventArgs: EventArgs
    {
        ICommunicationWindow window;

        /// <summary>
        /// Новое окно
        /// </summary>
        public ICommunicationWindow Window
        {
            get { return window; }
        }

        public JoinEventArgs(ICommunicationWindow window)
        {
            this.window = window;
        }
    }
}
