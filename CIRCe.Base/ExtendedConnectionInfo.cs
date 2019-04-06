using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRC.Client.Base;

namespace CIRCe.Base
{
    public sealed class ExtendedConnectionInfo
    {
        public ConnectionInfo Data { get; set; }

        private ExtendedServerInfo server;

        /// <summary>
        /// Сервер
        /// </summary>
        public ExtendedServerInfo Server
        {
            get { return this.server; }
            set
            {
                this.server = value;
                this.Data.Server = this.server.Data;
            }
        }

        public string Nick { get { return this.Data.Nick; } set { this.Data.Nick = value; } }

        public UserInfo User { get { return this.Data.User; } set { this.Data.User = value; } }

        public ExtendedConnectionInfo()
        {
            this.Data = new ConnectionInfo();
        }
    }
}
