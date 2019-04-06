using System;
using System.Collections.Generic;
using System.Text;

namespace IRC.Client.Base
{
    /// <summary>
    /// Сессия на сервере
    /// </summary>
    public interface ISession: IItem
    {
        #region Properties

        /// <summary>
        /// Сервер, владеющий данной сесией
        /// </summary>
        IServer OwnerServer { get; }

        /// <summary>
        /// Имя сессии
        /// </summary>
        string Name { get; }

        #endregion

        #region Events

        event EventHandler<SessionMessageEventArgs> MessageReceived;

        #endregion

        /// <summary>
        /// Отправить сообщение
        /// </summary>
        /// <param name="message">Сообщение</param>
        void SendMessage(string message);
    }
}
