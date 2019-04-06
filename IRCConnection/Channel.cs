using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using IRCConnection.Properties;

namespace IRCConnection
{
    /// <summary>
    /// Канал
    /// </summary>
    [Serializable]
    public class Channel: IRCObject, IChannel, ICloneable
    {
        /// <summary>
        /// Имя канала
        /// </summary>
        [UserScopedSetting()]
        public string Name { get; set; }

        /// <summary>
        /// Создание канала
        /// </summary>
        public Channel()
        {
            this.Name = "";
        }

        /// <summary>
        /// Создание канала
        /// </summary>
        /// <param name="name">Имя канала</param>
        public Channel(string name)
        {
            this.Name = name;
        }

        public override string ToString()
        {
            return this.Name;
        }

        #region ICloneable Members

        /// <summary>
        /// Клонировать канал
        /// </summary>
        /// <returns>Клон канала</returns>
        public object Clone()
        {
            return new Channel(this.Name);
        }

        #endregion
    }
}
