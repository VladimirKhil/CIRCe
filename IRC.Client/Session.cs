using System;
using System.Collections.Generic;
using System.Text;
using IRC.Client.Base;

namespace IRC.Client
{
    internal abstract class Session: ISession
    {
        private Server server = null;
        private string name = null;

        public Session(Server server, string name)
        {
            this.server = server;
            this.name = name;
        }

        #region Члены ISession

        public IServer OwnerServer
        {
            get { return this.server; }
        }

        public string Name
        {
            get { return this.name; }
        }

        public event EventHandler<SessionMessageEventArgs> MessageReceived;

        internal void OnMessageReceived(string sender, string text)
        {
            if (MessageReceived != null)
                MessageReceived(this, new SessionMessageEventArgs(sender, text));
        }

        public void SendMessage(string message)
        {
            this.server.Send(this.name, message);
        }

        #endregion
    }
}
