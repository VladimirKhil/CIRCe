using System;
using System.Collections.Generic;
using System.Text;

namespace IRCProviders
{  
    /// <summary>
    /// Базовое окно для общения
    /// </summary>
    public interface ICommunicationWindow: IBaseWindow
    {
        #region Properties

        /// <summary>
        /// Окно сервера, к которому принадлежит канал текущего окна
        /// </summary>
        IServerWindow OwnerServerWindow { get; }

        /// <summary>
        /// Имя окна
        /// </summary>
        string WindowName { get; }

        /// <summary>
        /// Имя получателя сообщений
        /// </summary>
        string ReceiverName { get; }

        #endregion

        #region Events

        /// <summary>
        /// Пользователь присоединился к окну
        /// </summary>
        event EventHandler<PersonEventArgs> PersonJoined;
        
        /// <summary>
        /// Пользователь вышел
        /// </summary>
        event EventHandler<PersonEventArgs> PersonLeaved;

        /// <summary>
        /// Получено сообщение
        /// </summary>
        event EventHandler<MessageEventArgs> MessageReceived;

        /// <summary>
        /// Введена команда
        /// </summary>
        event CmdDel Cmd;

        #endregion

        /// <summary>
        /// Вывести информацию о своём действии на канале (команда /me)
        /// </summary>
        /// <param name="actionText">Описание действия</param>
        void Action(string actionText);

        /// <summary>
        /// Записать в чат обычное сообщение
        /// </summary>
        /// <param name="name">Отправитель</param>
        /// <param name="text">Текст сообщения</param>
        void PutTextMessage(string name, string text);

        /// <summary>
        /// Содержит ли имя пользователя
        /// </summary>
        /// <param name="name">Имя</param>
        /// <returns>Содержит ли имя данного пользователя</returns>
        bool HasPerson(string name);
    }
}
