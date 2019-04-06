using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace IRCConnection
{
    /// <summary>
    /// Информация о подключении
    /// </summary>
    [Serializable]
    public class ConnectionInfo
    {
        private Server server = null;
        private NickName nickName = null;

        /// <summary>
        /// Сервер
        /// </summary>
        public Server Server
        {
            get { return server; }
            set { server = value; }
        }

        /// <summary>
        /// Ник
        /// </summary>
        public NickName Nick
        {
            get { return nickName; }
            set { nickName = value; }
        }

        public ConnectionInfo() { }

        /// <summary>
        /// Создание информации о подключении
        /// </summary>
        /// <param name="server">Сервер</param>
        /// <param name="nick">Ник</param>
        public ConnectionInfo(Server server, NickName nick)
        {
            this.server = server;
            this.nickName = nick;
        }
    }
}
