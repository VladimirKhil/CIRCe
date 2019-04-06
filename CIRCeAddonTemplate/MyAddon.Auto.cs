using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using CIRCe.Base;

namespace CIRCeAddonTemplate
{
    /// <summary>
    /// Дополнение для Цирцеи. Этот файл в большинстве случаев не нужно редактировать
    /// </summary>
    public partial class MyAddon: CIRCe.Base.Addon
    {
        /// <summary>
        /// Приложение Цирцеи
        /// </summary>
        private ICIRCeApplication application = null;
        /// <summary>
        /// Сервер
        /// </summary>
        private ICIRCeServer server = null;
        /// <summary>
        /// Канал
        /// </summary>
        private ICIRCeChannel channel = null;
        /// <summary>
        /// Форма для подключения
        /// </summary>
        private ConnectionForm connectionForm = null;
        /// <summary>
        /// Данные аддона
        /// </summary>
        private Data data = null;

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

        /// <summary>
        /// Здесь запускается дополнение
        /// </summary>
        /// <param name="application">Ссылка на Цирцею</param>
        public override void Run(ICIRCeApplication application)
        {
            this.application = application;
            this.data = Data.Load();

            var activeItem = application.ActiveItem;
            if (activeItem is ICIRCeServer)
            {
                var server = activeItem as ICIRCeServer;
                this.data.Info.Server.Name = server.Info.Server.Name;
                this.data.Info.Nick = server.Info.Nick;
            }
            else if (activeItem is ICIRCeChannel)
            {
                var channel = activeItem as ICIRCeChannel;
                this.data.Info.Server.Name = channel.OwnerServer.Info.Server.Name;
                this.data.Info.Nick = channel.OwnerServer.Info.Nick;
                this.data.Channel = channel.Name;
            }
            else if (activeItem is ICIRCePrivateSession)
            {
                var privateSession = activeItem as ICIRCePrivateSession;
                this.data.Info.Server.Name = privateSession.OwnerServer.Info.Server.Name;
                this.data.Info.Nick = privateSession.OwnerServer.Info.Nick;
            }
            else
            {
                this.data.Info.Nick = application.DefaultNickName;
            }

            this.connectionForm = new ConnectionForm(this.data);
            this.application.AddOwnedWindow(this.connectionForm.Handle);

            if (this.connectionForm.ShowDialog() == DialogResult.OK)
            {
                this.data = this.connectionForm.Data;
                this.server = application.CreateConnection(this.data.Info);

                if (!this.server.IsConnected && !this.server.Connect())
                {
                    Stop();
                    return;
                }

                this.channel = this.server.JoinChannel(this.data.Channel);

                if (channel == null)
                {
                    Stop();
                    return;
                }

                this.channel.Closed += Wrap(Stop);

                Init();
            }
            else
                Stop();
        }
    }
}
