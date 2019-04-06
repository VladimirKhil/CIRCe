using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIRCe.Base;
using IRC.Client.Base;

namespace IRCWindow.ViewModel
{
    /// <summary>
    /// Базовый класс для сессии в Цирцеи
    /// </summary>
    internal abstract class CIRCeSession: CIRCeAppItem, ICIRCeSession
    {
        private MDIChildCommunication communicationWindow = null;

        public CIRCeSession(MDIChildCommunication communicationWindow)
            : base(communicationWindow)
        {
            this.communicationWindow = communicationWindow;
            this.communicationWindow.MessageReceived += communicationWindow_MessageReceived;
        }

        void communicationWindow_MessageReceived(object sender, IRCProviders.MessageEventArgs e)
        {
            if (MessageReceived != null)
                MessageReceived(this, new SessionMessageEventArgs(e.Sender, e.Message));
        }

        #region Члены ISession

        public IRC.Client.Base.IServer OwnerServer
        {
            get { return (this.communicationWindow.OwnerServerWindow as MDIChildServer).DataContext as IServer; }
        }

        public string Name
        {
            get { return this.communicationWindow.WindowName; }
        }

        public event EventHandler<IRC.Client.Base.SessionMessageEventArgs> MessageReceived;

        public void SendMessage(string message)
        {
            (this.communicationWindow.OwnerServerWindow as MDIChildServer).Send(this.communicationWindow.WindowName, message, false);
        }

        #endregion

        ICIRCeServer ICIRCeSession.OwnerServer
        {
            get { return (ICIRCeServer)this.OwnerServer; }
        }
    }
}
