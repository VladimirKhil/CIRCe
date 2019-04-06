using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIRCe.Base;
using System.Threading;
using IRC.Client.Base;
using IRC.Client;
using System.ComponentModel;

namespace IRCWindow.ViewModel
{
    internal sealed class CIRCeServer: CIRCeAppItem, ICIRCeServer
    {
        private MDIChildServer server = null;

        public CIRCeServer(MDIChildServer server)
            : base(server)
        {
            this.server = server;
            this.server.IRCMessageReceived += server_IRCMessageReceived;
            this.server.DownloadFinished += server_DownloadFinished;
        }

        void server_DownloadFinished(string url)
        {
            if (DownloadFinished != null)
                DownloadFinished(url);
        }

        void server_IRCMessageReceived(object sender, IRCProviders.IRCMessageEventArgs e)
        {
            if (this.MessageReceived != null)
            {
                var args = new MessageEventArgs(new IRC.Client.Message { Command = e.Message.Command, Host = e.Message.Host, Name = e.Message.Name, Params = e.Message.Param.ToArray(), Tail = e.Message.Tail, User = e.Message.User }, this);
                e.Cancel = this.MessageReceived(args);
            }
        }

        #region Члены ICIRCeServer

        private AutoResetEvent joinSync = new AutoResetEvent(false);

        public ICIRCeChannel JoinChannel(string channel)
        {
            var channelWindow = this.server.Channels.FirstOrDefault(chnl => chnl.WindowName == channel);
            if (channelWindow != null)
                return ((MDIChildChannel)channelWindow).DataContext as ICIRCeChannel;

            EventHandler<JoinEventArgs> handler = null;
            handler = (sender, e) =>
                 {
                     if (e.Window.WindowName == channel)
                     {
                         joinSync.Set();
                         this.server.NewWindow -= handler;
                     }
                 };

            joinSync.Reset();
            this.server.NewWindow += handler;
            this.server.JoinChannel(channel);
            if (joinSync.WaitOne(TimeSpan.FromMinutes(1.0)))
            {
                return ((MDIChildChannel)this.server[channel]).DataContext as ICIRCeChannel;
            }
            else
                return null;
        }

        #endregion

        #region Члены IServer

        public string NickName
        {
            get { return this.server.Nick; }
        }

        private Changeable<ICIRCeChannel> channels = new Changeable<ICIRCeChannel>();

        public new IChangeable<ICIRCeChannel> Channels
        {
            get { return this.channels; }//new System.ComponentModel.BindingList<IChannel>(this.server.Channels.Select(ch => (ch as MDIChildChannel).DataContext as IChannel).ToList()); }
        }

        public IChangeable<IRC.Client.Base.IPrivateSession> Privates
        {
            get { return new Changeable<IPrivateSession>(this.server.Privates.Select(ch => (ch as MDIChildPrivate).DataContext as IPrivateSession).ToList()); }
        }

        public bool IsConnected
        {
            get { return this.server.IsConnected; }
        }

        public IRC.Client.Base.ISession this[string name]
        {
            get { return (this.server[name] as MDIChildCommunication).DataContext as ISession; }
        }

        public event Func<MessageEventArgs, bool> MessageReceived;

        private AutoResetEvent connectionSync = new AutoResetEvent(false);

        public bool Connect()
        {
            if (!this.server.IsConnected)
            {
                connectionSync.Reset();
                this.server.OnConnected += server_OnConnected;
                this.server.Connect();

                connectionSync.WaitOne(TimeSpan.FromMinutes(1.0));
                return this.server.IsConnected;
            }

            return true;
        }

        void server_OnConnected(object sender, EventArgs e)
        {
            connectionSync.Set();
            this.server.OnConnected -= server_OnConnected;
        }

        IRC.Client.Base.IChannel IRC.Client.Base.IServer.JoinChannel(string channel)
        {
            return JoinChannel(channel);
        }

        public IRC.Client.Base.IPrivateSession OpenPrivate(string name)
        {
            this.server.JoinChannel(name);
            return (this.server[name] as MDIChildPrivate).DataContext as IPrivateSession;
        }

        #endregion

        #region Члены IDisposable

        public void Dispose()
        {
            
        }

        #endregion

        public ConnectionInfo Info
        {
            get
            {
                return this.server.ConnectionInfo.Data;
            }
        }

        public void SendMessage(string receiver, string message, bool silent)
        {
            this.server.Send(receiver, message, silent);
        }

        public void SendMessage(string receiver, string message)
        {
            this.server.Send(receiver, message, false);
        }
        
        IChangeable<IChannel> IServer.Channels
        {
            get { return this.Channels; }
        }

        public event Action<string> DownloadFinished;
    }
}
