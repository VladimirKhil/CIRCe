using System;
using System.Collections.Generic;

namespace IRC.Client.Base
{
    /// <summary>
    /// Сообщение IRC
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// Команда
        /// </summary>
        string Command { get; }

        /// <summary>
        /// Имя хоста отправителя
        /// </summary>
        string Host { get; }

        /// <summary>
        /// Имя отправителя
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Параметры команды
        /// </summary>
        string[] Params { get; }

        /// <summary>
        /// Префикс отправителя
        /// </summary>
        string Prefix { get; }

        /// <summary>
        /// Хвост сообщения
        /// </summary>
        string Tail { get; }

        /// <summary>
        /// Имя юзера отправителя
        /// </summary>
        string User { get; }
    }
}
