using System;
using System.Collections.Generic;
using System.Text;

namespace IRCConnection
{
    /// <summary>
    /// Сервер в IRC
    /// </summary>
    public interface IServer: IIRCObject
    {
        /// <summary>
        /// Описание сервера
        /// </summary>
        string Description { get; }
        /// <summary>
        /// Имя сервера
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Номер порта
        /// </summary>
        int Port { get; }
        /// <summary>
        /// Каналы сервера
        /// </summary>
        ChannelList Channels { get; }
        /// <summary>
        /// Пароли ников на сервере
        /// </summary>
        PasswordList Passwords { get; }
    }
}
