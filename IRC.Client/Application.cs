using System;
using System.Collections.Generic;
using System.Text;
using IRC.Client.Base;
using System.ComponentModel;

namespace IRC.Client
{
    /// <summary>
    /// Приложение IRC-клиента
    /// </summary>
    public class Application : InfiniteMarshalByRefObject, IApplication
    {
        private UserInfo user = new UserInfo();
        private Changeable<IServer> servers = new Changeable<IServer>();

        /// <summary>
        /// Таблица соответствия режимов канала и обозначающих их символов
        /// </summary>
        public static Dictionary<ChannelModes, char> ChannelModesTable = new Dictionary<ChannelModes, char>();
        /// <summary>
        /// Таблица соответствия режимов пользователя и соответствующих им символов
        /// </summary>
        public static Dictionary<ChannelUserModes, char> ChannelUserModesTable = new Dictionary<ChannelUserModes, char>();

        public static ChannelModes ChannelModeByChar(char c)
        {
            foreach (var item in ChannelModesTable)
            {
                if (item.Value == c)
                    return item.Key;
            }
            return ChannelModes.None;
        }

        public static ChannelUserModes ChannelUserModeByChar(char c)
        {
            foreach (var item in ChannelUserModesTable)
            {
                if (item.Value == c)
                    return item.Key;
            }
            return ChannelUserModes.None;
        }

        static Application()
        {
            ChannelUserModesTable[ChannelUserModes.Op] = 'o';
            ChannelUserModesTable[ChannelUserModes.Halfop] = 'h';
            ChannelUserModesTable[ChannelUserModes.Voice] = 'v';
        }

        /// <summary>
        /// Создать приложение IRC-клиента
        /// </summary>
        public Application()
        {
            
        }

        #region Члены IApplication

        /// <summary>
        /// Набор открытых подключений к IRC-серверам
        /// </summary>
        public IChangeable<IServer> Servers
        {
            get { return this.servers; }
        }

        /// <summary>
        /// Информация о текущем пользователе
        /// </summary>
        public UserInfo User
        {
            get { return this.user; }
        }

        /// <summary>
        /// Создать подключение к серверу
        /// </summary>
        /// <param name="connectionInfo">Информация о подключении</param>
        /// <returns>Созданное подключение</returns>
        public IServer CreateConnection(ConnectionInfo connectionInfo)
        {
            var server = new Server(connectionInfo);
            this.servers.Add(server);
            return server;
        }

        #endregion

        #region Члены IDisposable

        public virtual void Dispose()
        {
            
        }

        #endregion
    }
}
