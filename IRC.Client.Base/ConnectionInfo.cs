using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace IRC.Client.Base
{
    /// <summary>
    /// Информация о подключении к cерверу
    /// </summary>
    [Serializable]
    public sealed class ConnectionInfo
    {
        /// <summary>
        /// Сервер
        /// </summary>
        public ServerInfo Server { get; set; }

        /// <summary>
        /// Ник
        /// </summary>
        public string Nick { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Информация о пользователе
        /// </summary>
        public UserInfo User { get; set; }
    }
}
