using System;
using System.Collections.Generic;
using System.Text;

namespace IRCProviders
{
    /// <summary>
    /// Информация о пользователе
    /// </summary>
    [Serializable]
    public class PersonEventArgs: EventArgs
    {
        private string nickName = string.Empty;

        /// <summary>
        /// Ник
        /// </summary>
        public string NickName
        {
            get { return nickName; }
        }

        public PersonEventArgs(string nickName)
        {
            this.nickName = nickName;
        }
    }
}
