using System;
using System.Collections.Generic;
using System.Text;

namespace IRCProviders
{
    /// <summary>
    /// Базовое окно канала
    /// </summary>
    public interface IChannelWindow: ICommunicationWindow
    {
        #region Properties

        /// <summary>
        /// Тема канала
        /// </summary>
        string Topic { get; }

        /// <summary>
        /// Полное имя канала (с именем сервера и ником)
        /// </summary>
        string FullChannelName { get; }

        /// <summary>
        /// Список пользователей канала
        /// </summary>
        IListView UsersList { get; }

        #endregion

        #region Events

        /// <summary>
        /// Изменилась тема
        /// </summary>
        event EventHandler ChannelTopicChanged;

        /// <summary>
        /// Изменились режимы канала
        /// </summary>
        event EventHandler ChannelModesChanged;

        /// <summary>
        /// Изменились режимы пользователя
        /// </summary>
        event EventHandler<PersonEventArgs> PersonModesChanged;

        /// <summary>
        /// Пользователь выкинут
        /// </summary>
        event EventHandler<PersonEventArgs> PersonKicked;

        #endregion
    }
}
