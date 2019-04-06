using System;
using System.Collections.Generic;
using System.Text;

namespace IRCProviders
{
    /// <summary>
    /// Элемент списка
    /// </summary>
    public interface IChannelUser
    {
        /// <summary>
        /// Заголовок
        /// </summary>
        string NickName { get; }

        /// <summary>
        /// Режимы пользователя
        /// </summary>
        Dictionary<char, bool> Modes { get; }
    }
}
