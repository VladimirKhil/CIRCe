using System;
using System.Collections.Generic;
using System.Text;
using IRCConnection;

namespace IRCProviders
{
    /// <summary>
    /// Базовое окно сервера
    /// </summary>
    public interface IServerWindow: IBaseWindow
    {
        #region Properties

        /// <summary>
        /// Сервер
        /// </summary>
        IServer Server { get; }

        /// <summary>
        /// Пользователь
        /// </summary>
        INick Nick { get; }

        /// <summary>
        /// Имеется ли подключение
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Каналы, открытые на сервере
        /// </summary>
        IChannelWindow[] Channels { get; }

        /// <summary>
        /// Окна личной переписки на сервере
        /// </summary>
        IPrivateWindow[] Privates { get; }

        /// <summary>
        /// Получить окно чата/канала по имени
        /// </summary>
        /// <param name="name">Имя окна</param>
        /// <returns>Окно с таким именем или null</returns>
        ICommunicationWindow this[string name] { get; }

        #endregion

        #region Events

        /// <summary>
        /// Подключение состоялось (в полном объёме)
        /// </summary>
        event EventHandler OnConnected;

        /// <summary>
        /// Открыто новое окно
        /// </summary>
        event EventHandler<JoinEventArgs> NewWindow;

        /// <summary>
        /// Пользователь переименовался
        /// </summary>
        event EventHandler<TwoPersonsEventArgs> PersonRenames;

        /// <summary>
        /// Получено IRC-сообщение
        /// </summary>
        event EventHandler<IRCMessageEventArgs> IRCMessageReceived;

        /// <summary>
        /// Обновлена информация о пользователе
        /// </summary>
        event EventHandler<PersonEventArgs> WhoisUpdated;

        #endregion

        #region Methods

        /// <summary>
        /// Подключиться
        /// </summary>
        void Connect();

        /// <summary>
        /// Открыть новое окно
        /// </summary>
        /// <param name="name">Имя окна</param>
        /// <returns>Созданное окно</returns>
        ICommunicationWindow OpenWindow(string name);

        /// <summary>
        /// Сказать/выполнить команду
        /// </summary>
        /// <param name="name">Кому: ник получателя/Имя канала</param>
        /// <param name="message">Сообщение</param>
        /// <param name="silent">Добавить ли сообщение "молча" (без вывода в окно)</param>
        void Send(string name, string message, bool silent);

        /// <summary>
        /// Зайти на канал
        /// </summary>
        /// <param name="channel">Имя канала</param>
        void JoinChannel(string channel);

        /// <summary>
        /// Открыто ли окно
        /// </summary>
        /// <param name="name">Имя окна</param>
        /// <returns></returns>
        bool ContainsWindow(string name);

        /// <summary>
        /// Установить новый ник
        /// </summary>
        /// <param name="name">Новый ник</param>
        void SetNick(string name);

        /// <summary>
        /// Выполнить команду
        /// </summary>
        /// <param name="windowName">Имя окна, для которого выполняется команда или null</param>
        /// <param name="cmd">Параметры команды</param>
        void Execute(string windowName, params string[] cmd);

        #endregion
    }
}
