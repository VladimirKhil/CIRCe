using System;
using System.Collections.Generic;
using System.Text;

namespace IRCConnection
{
    /// <summary>
    /// Ник в IRC
    /// </summary>
    public interface INick
    {
        /// <summary>
        /// Ник
        /// </summary>
        string Nick { get; }
        /// <summary>
        /// Информация о пользователе
        /// </summary>
        IUser User { get; }
    }
}
