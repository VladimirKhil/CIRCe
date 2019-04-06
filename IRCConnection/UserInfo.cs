using System;
using System.Collections.Generic;
using System.Text;

namespace IRCConnection
{
    /// <summary>
    /// Подробная информация о пользователе
    /// </summary>
    public sealed class UserInfo
    {
        private Dictionary<string, char> modes = new Dictionary<string, char>();

        public string Nick { get; set; }
        public User User { get; set; }

        /// <summary>
        /// Сервер, на котором расположен пользователь
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Информация о сервере
        /// </summary>
        public string ServerInfo { get; set; }

        /// <summary>
        /// Режимы пользователя
        /// </summary>
        public Dictionary<string, char> Modes
        {
            get { return modes; }
        }

        /// <summary>
        /// Является ли ник зарегистрированным
        /// </summary>
        public bool IsRegistered { get; set; }

        /// <summary>
        /// Является ли оператором IRC
        /// </summary>
        public bool IsIRCOp { get; set; }

        /// <summary>
        /// Страница кодировки пользователя
        /// </summary>
        public string CodePage { get; set; }

        /// <summary>
        /// Время простоя
        /// </summary>
        public int Idle { get; set; }

        public string WhoisActially { get; set; }

        /// <summary>
        /// Является ли ботом
        /// </summary>
        public bool IsBot { get; set; }

        public string WhoisHost { get; set; }

        public string Ssl { get; set; }

        /// <summary>
        /// Подлинное имя
        /// </summary>
        public string Auth { get; set; }

        /// <summary>
        /// Используется для устаревания информации о юзере
        /// Информация считается устаревшей после прошествия некоторого числа секунд с момента получения
        /// </summary>
        public int Obsolete { get; set; }

        public UserInfo()
        {
            this.Server = string.Empty;
            this.ServerInfo = string.Empty;
            this.IsRegistered = false;
            this.IsIRCOp = false;
            this.CodePage = string.Empty;
            this.Idle = 0;
            this.IsBot = false;
            this.WhoisActially = string.Empty;
            this.WhoisHost = string.Empty;
            this.Ssl = string.Empty;
            this.Obsolete = 0;
            this.User = new User();
        }
    }
}
