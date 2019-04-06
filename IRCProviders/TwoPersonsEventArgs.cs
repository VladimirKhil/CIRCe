using System;
using System.Collections.Generic;
using System.Text;

namespace IRCProviders
{
    /// <summary>
    /// Событие, касающееся двух (имён) пользователей
    /// </summary>
    [Serializable]
    public class TwoPersonsEventArgs: PersonEventArgs
    {
        /// <summary>
        /// Второй участник событий
        /// </summary>
        public string AnotherNickName { get; set; }

        /// <summary>
        /// Создание события
        /// </summary>
        /// <param name="nickName">Первый ник</param>
        /// <param name="anotherNickName">Второй ник</param>
        public TwoPersonsEventArgs(string nickName, string anotherNickName)
            : base(nickName)
        {
            this.AnotherNickName = anotherNickName;
        }
    }
}
