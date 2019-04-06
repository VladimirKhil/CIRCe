using System;
using System.Collections.Generic;
using System.Text;
using IRCProviders;
using IRCConnection;
using System.Windows.Forms;

namespace CIRCeAddonTemplate
{
    /// <summary>
    /// Аддон для Цирцеи
    /// </summary>
    public partial class Addon: IRCAddon
    {
        /// <summary>
        /// Главное окно Цирцеи
        /// </summary>
        private IMainWindow mainWindow = null;
        /// <summary>
        /// Окно сервера
        /// </summary>
        private IServerWindow serverWindow = null;
        /// <summary>
        /// Окно канала
        /// </summary>
        private IChannelWindow channelWindow = null;
        /// <summary>
        /// Форма для подключения
        /// </summary>
        private ConnectionForm connectionForm = null;
        /// <summary>
        /// Данные аддона
        /// </summary>
        private Data data = null;

        /// <summary>
        /// Закрыть аддон
        /// </summary>
        private void ManualClose()
        {
            RaiseStop();
        }

        private void serverWindow_OnConnected(object sender, EventArgs e)
        {
            serverWindow.OnConnected -= new EventHandler(serverWindow_OnConnected);
            Futher();
        }

        private void Futher()
        {
            if (serverWindow.ContainsWindow(this.data.Channel))
                this.mainWindow.RunCallback(new Special.Action(Futher2));
            else
            {
                serverWindow.NewWindow += new EventHandler<JoinEventArgs>(serverWindow_NewWindow);
                serverWindow.JoinChannel(this.data.Channel);
            }
        }

        private void serverWindow_NewWindow(object sender, JoinEventArgs e)
        {
            if (e.Window.WindowName == this.data.Channel)
            {
                serverWindow.NewWindow -= new EventHandler<JoinEventArgs>(serverWindow_NewWindow);
                Futher2();
            }
        }

        private void Futher2()
        {
            channelWindow = (IChannelWindow)serverWindow[this.data.Channel];
            if (channelWindow == null)
            {
                ManualClose();
                return;
            }

            Init();

            this.channelWindow.Disposed += (sender, e) => this.ManualClose();
        }

        #region IIRCAddon Members

        /// <summary>
        /// Здесь запускается аддон
        /// </summary>
        /// <param name="mainWindow">Главное окно Цирцеи</param>
        public override void Run(IMainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            this.data = Data.Load();

            IBaseWindow activeWindow = mainWindow.ActiveIRCWindow;
            if (activeWindow is IServerWindow)
            {
                IServerWindow server = activeWindow as IServerWindow;
                this.data.Info.Server.Name = server.Server.Name;
                this.data.Info.Nick = new NickName(server.Nick);
            }
            else if (activeWindow is IChannelWindow)
            {
                IChannelWindow channelWindow2 = activeWindow as IChannelWindow;
                this.data.Info.Server.Name = channelWindow2.OwnerServerWindow.Server.Name;
                this.data.Info.Nick = new NickName(channelWindow2.OwnerServerWindow.Nick);
                this.data.Channel = channelWindow2.WindowName;
            }
            else if (activeWindow is IPrivateWindow)
            {
                IPrivateWindow privateWindow = activeWindow as IPrivateWindow;
                this.data.Info.Server.Name = privateWindow.OwnerServerWindow.Server.Name;
                this.data.Info.Nick = new NickName(privateWindow.OwnerServerWindow.Nick);
            }
            else
            {
                this.data.Info.Nick = new NickName(mainWindow.DefaultNick);
            }

            this.connectionForm = (ConnectionForm)this.mainWindow.CreateObject(AppDomain.CurrentDomain, typeof(ConnectionForm), this.data);

            if (this.connectionForm == null)
            {
                ManualClose();
                return;
            }

            if (this.mainWindow.ShowDialog(connectionForm) == DialogResult.OK)
            {
                this.data = this.connectionForm.Data;
                this.serverWindow = mainWindow.OpenConnection(this.data.Info);
                this.serverWindow.Disposed += (sender, e) => ManualClose();
                if (this.serverWindow.IsConnected)
                    this.mainWindow.RunCallback(new Special.Action(Futher));
                else
                {
                    this.serverWindow.OnConnected += new EventHandler(serverWindow_OnConnected);
                    this.serverWindow.Connect();
                }
            }
            else
                ManualClose();
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// При закрытии аддона
        /// Не вызывается явно
        /// </summary>
        public override void Dispose()
        {
            Finish();
            if (this.data != null)
                data.Save();
        }

        #endregion
    }
}
