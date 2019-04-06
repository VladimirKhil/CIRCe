using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Xml.Serialization;
using IRCConnection.Properties;

namespace IRCConnection
{
    /// <summary>
    /// Ник
    /// </summary>
    [Serializable]
    public class NickName: INick, ICloneable
    {
        private string nick = string.Empty;
        private IUser user = new User();

        /// <summary>
        /// Ник пользователя
        /// </summary>
        [UserScopedSetting()]
        public string Nick
        {
            get { return nick; }
            set 
            { 
                nick = value;
            }
        }

        [XmlIgnore]
        public IUser User
        {
            get { return user; }
            set { user = value; }
        }

        /// <summary>
        /// Создание ника
        /// </summary>
        public NickName()
        {
            
        }

        /// <summary>
        /// Создание ника
        /// </summary>
        /// <param name="nick">Никнейм</param>
        public NickName(string nick)
        {
            this.nick = nick;
        }

        public NickName(INick nick)
        {
            this.nick = nick.Nick;
            this.user = nick.User;
        }
        
        public override string ToString()
        {
            return this.Nick;
        }

        public override bool Equals(object obj)
        {
            NickName nickName = obj as NickName;
            if (obj == null)
                return false;
            return this.nick == nickName.nick;
        }

        public override int GetHashCode()
        {
            return this.nick.Length;
        }

        #region ICloneable Members

        public virtual object Clone()
        {
            return new NickName(this.nick);
        }

        #endregion
    }
}
