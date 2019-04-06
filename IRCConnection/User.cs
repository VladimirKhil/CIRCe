using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using IRCConnection.Properties;

namespace IRCConnection
{
    /// <summary>
    /// Пользователь
    /// </summary>
    [Serializable]
    public class User: IUser, ICloneable
    {
        string user = string.Empty;
        string host = string.Empty;
        string name = string.Empty;
        string eMail = string.Empty;

        /// <summary>
        /// Пользователь
        /// </summary>
        [UserScopedSetting()]
        public string UserName
        {
            get { return user; }
            set { user = value; if (user.Length == 0) user = "Unknown"; }
        }

        /// <summary>
        /// Хост
        /// </summary>
        [UserScopedSetting()]
        public string Host
        {
            get { return host; }
            set { host = value; if (host.Length == 0) host = "Unknown"; }
        }

        /// <summary>
        /// Реальное имя
        /// </summary>
        [UserScopedSetting()]
        public string Name
        {
            get { return name; }
            set { name = value; if (name.Length == 0) name = "Unknown"; }
        }

        /// <summary>
        /// EMail
        /// </summary>
        [UserScopedSetting()]
        public string EMail
        {
            get { return eMail; }
            set { eMail = value; }
        }

        public string FullName { get { return this.user + "@" + this.host; } }

        /// <summary>
        /// Дополнительные сведения
        /// </summary>
        public string Info { get; set; }

        public User()
        {
        }

        /// <summary>
        /// Создание нового пользователя
        /// </summary>
        /// <param name="user">Имя пользователя</param>
        /// <param name="host">Имя хоста</param>
        /// <param name="name">Реальное имя</param>
        public User(string user, string host, string name, string eMail, string info)
        {
            this.user = user;
            this.host = host;
            this.name = name;
            this.eMail = eMail;
            this.Info = info;
        }

        /// <summary>
        /// Создание нового пользователя на основе существующего
        /// </summary>
        /// <param name="user">Существующий пользователь</param>
        public User(User user)
        {
            this.user = user.user;
            this.host = user.host;
            this.name = user.name;
            this.eMail = user.eMail;
            this.Info = user.Info;
        }

        public override string ToString()
        {
            return this.FullName;
        }

        #region ICloneable Members

        public object Clone()
        {
            return new User(this.user, this.host, this.name, this.eMail, this.Info);
        }

        #endregion
    }
}
