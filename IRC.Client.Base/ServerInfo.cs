using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace IRC.Client.Base
{
    /// <summary>
    /// Информация о сервере
    /// </summary>
    [Serializable]
    public sealed class ServerInfo: ICloneable, IEquatable<ServerInfo>
    {
        /// <summary>
        /// Имя сервера
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Номер порта
        /// </summary>
        public int Port { get; set; }

        public ServerInfo()
        {
            this.Name = "";
            this.Port = 6667;
        }

        #region Члены ICloneable

        public object Clone()
        {
            return new ServerInfo { Name = this.Name, Port = this.Port };
        }

        #endregion

        #region Члены IEquatable<ServerInfo>

        public bool Equals(ServerInfo other)
        {
            return this.Name == other.Name && this.Port == other.Port;
        }

        #endregion

        public override string ToString()
        {
            return string.Format("{0}:{1}", this.Name, this.Port);
        }
    }
}
