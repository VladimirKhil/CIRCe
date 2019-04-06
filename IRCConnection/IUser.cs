using System;
using System.Collections.Generic;
using System.Text;

namespace IRCConnection
{
    /// <summary>
    /// Юзер в IRC
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// Пользователь
        /// </summary>
        string UserName { get; }
        /// <summary>
        /// Хост
        /// </summary>
        string Host { get; }
        /// <summary>
        /// Реальное имя
        /// </summary>
        string Name { get; }
        /// <summary>
        /// EMail
        /// </summary>
        string EMail { get; }
        /// <summary>
        /// Полное имя в IRC
        /// </summary>
        string FullName { get; }
    }
}
