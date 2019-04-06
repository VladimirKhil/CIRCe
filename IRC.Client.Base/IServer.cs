using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace IRC.Client.Base
{
    /// <summary>
    /// IRC-сервер
    /// </summary>
    public interface IServer: IItem, IDisposable
    {
        #region Properties

        /// <summary>
        /// Информация о сервере
        /// </summary>
        ConnectionInfo Info { get; }

        /// <summary>
        /// Каналы, открытые на сервере
        /// </summary>
        IChangeable<IChannel> Channels { get; }
        /// <summary>
        /// Окна личной переписки на сервере
        /// </summary>
        IChangeable<IPrivateSession> Privates { get; }

        /// <summary>
        /// Имеется ли подключение
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Получить окно дочерней сессии по имени
        /// </summary>
        /// <param name="name">Имя сессии</param>
        /// <returns>Сессия с таким именем или null</returns>
        ISession this[string name] { get; }

        #endregion

        #region Events

        event Func<MessageEventArgs, bool> MessageReceived;

        #endregion

        #region Methods

        /// <summary>
        /// Подключиться
        /// </summary>
        bool Connect();

        /// <summary>
        /// Зайти на канал
        /// </summary>
        /// <param name="channel">Имя канала</param>
        IChannel JoinChannel(string channel);

        /// <summary>
        /// Открыть приватное подключение
        /// </summary>
        /// <param name="name">Ник пользователя, к которому открывается приват</param>
        /// <returns>Открытая приватная сессия</returns>
        IPrivateSession OpenPrivate(string name);

        /// <summary>
        /// Отправить сообщение
        /// </summary>
        /// <param name="receiver">Имя получателя или канала</param>
        /// <param name="message">Текст сообщения</param>
        void SendMessage(string receiver, string message);

        #endregion
    }
}
