using System;
using System.Collections.Generic;
using System.Text;

namespace IRCWindow
{
    /// <summary>
    /// Аргументы присоединения к каналу/привату
    /// </summary>
    [Serializable]
    internal sealed class JoinEventArgs : EventArgs
    {
        MDIChildCommunication window;

        /// <summary>
        /// Новое окно
        /// </summary>
        public MDIChildCommunication Window
        {
            get { return window; }
        }

        public JoinEventArgs(MDIChildCommunication window)
        {
            this.window = window;
        }
    }
}
