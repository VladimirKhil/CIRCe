using System;
using System.Collections.Generic;
using System.Text;
using ObjectEditors;

namespace IRCConnection
{
    /// <summary>
    /// Ник
    /// </summary>
    public class NickName: ICloneable
    {
        string nick = string.Empty;

        /// <summary>
        /// Ник пользователя
        /// </summary>
        [Editable("Ник", EditableAttribute.BaseTypes.SingleString, -1)]
        public string Nick
        {
            get { return nick; }
            set 
            { 
                nick = value;
                if (nick.Length == 0) 
                    nick = "Unknown"; 
            }
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
        
        public override string ToString()
        {
            return this.Nick;
        }

        #region ICloneable Members

        public virtual object Clone()
        {
            return new NickName(this.nick);
        }

        #endregion
    }
}
