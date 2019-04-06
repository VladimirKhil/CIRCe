using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Configuration;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using IRCConnection.Properties;
using System.Resources;

namespace IRCConnection
{
    /// <summary>
    /// Сервер
    /// </summary>
    [Serializable]
    public class Server: IRCObject, IServer, ICloneable
    {
        /// <summary>
        /// Описание сервера
        /// </summary>
        [UserScopedSetting()]
        public string Description { get; set; }

        /// <summary>
        /// Имя сервера
        /// </summary>
        [UserScopedSetting()]
        public string Name { get; set; }

        /// <summary>
        /// Номер порта
        /// </summary>
        [UserScopedSetting()]
        public int Port { get; set; }

        private ChannelList channels = new ChannelList();
        private PasswordList passwords = new PasswordList();

        /// <summary>
        /// Каналы сервера
        /// </summary>
        public ChannelList Channels { get { return channels; } }

        /// <summary>
        /// Создание нового сервера
        /// </summary>
        public Server()
        {
            this.Description = string.Empty;
            this.Name = string.Empty;
            this.Port = 0;
        }

        /// <summary>
        /// Создание сервера
        /// </summary>
        /// <param name="description">Описание</param>
        /// <param name="name">Имя</param>
        /// <param name="port">Номер порта</param>
        public Server(string description, string name, int port)
        {
            this.Description = description;
            this.Name = name;
            this.Port = port;
        }

        /// <summary>
        /// Строковое представление сервера
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}{3}({1}: {2})", this.Description, this.Name, this.Port, string.IsNullOrEmpty(this.Description) ? string.Empty : " ");
        }

        /// <summary>
        /// Проверка на равенство сервера другому объекту
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Server another = obj as Server;
            if (another == null)
                return false;
            return another.Name == this.Name && another.Port == this.Port;
        }

        /// <summary>
        /// Хэш-код объекта
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #region ICloneable Members

        /// <summary>
        /// Создать копию сервера
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new Server(this.Description, this.Name, this.Port);
        }

        #endregion

        #region IServer Members
        
        public PasswordList Passwords
        {
            get { return this.passwords; }
        }

        #endregion
    }
}
