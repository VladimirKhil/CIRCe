using System;
using System.Collections.Generic;
using System.Text;
using IRC.Client;
using CIRCe.Base;
using System.Linq;
using System.Drawing;
using System.ComponentModel;
using IRC.Client.Base;

namespace IRCWindow.ViewModel
{
    // TODO: обновить протокол взаимодействия с сервером (убрать Host)

    /// <summary>
    /// Приложение Цирцеи (корневой класс модели клиента)
    /// </summary>
    internal sealed class CIRCeApplication: Application, ICIRCeApplication
    {
        private AddonsManager addonsManager = null;

        /// <summary>
        /// Диспетчер дополнений
        /// </summary>
        public AddonsManager AddonsManager { get { return this.addonsManager; } }

        private MDIParent parent = null;

        public CIRCeApplication(MDIParent parent)
        {
            this.addonsManager = new AddonsManager(this);
            this.parent = parent;
        }

        /// <summary>
        /// Завершить Цирцею
        /// </summary>
        public override void Dispose()
        {
            this.addonsManager.Dispose();

            base.Dispose();
        }

        #region Члены ICIRCeApplication

        private Changeable<ICIRCeServer> servers = new Changeable<ICIRCeServer>();

        public new IChangeable<ICIRCeServer> Servers
        {
            get
            {
                return this.servers;//new System.ComponentModel.BindingList<ICIRCeServer>(this.parent.ServerWindows.Select(sw => (sw as MDIChildServer).DataContext as ICIRCeServer).ToList());
            }
        }

        public ICIRCeServer CreateConnection(ExtendedConnectionInfo connectionInfo)
        {
            return ((MDIChildServer)this.parent.OpenConnection(connectionInfo)).DataContext as CIRCeServer;
        }

        public new ICIRCeServer CreateConnection(ConnectionInfo connectionInfo)
        {
            return CreateConnection(new ExtendedConnectionInfo { Data = connectionInfo, Server = new ExtendedServerInfo("", connectionInfo.Server.Name, connectionInfo.Server.Port) });
        }

        public ICIRCeItem ActiveItem
        {
            get
            {
                var child = this.parent.ActiveIRCWindow as MDIChild;
                return child == null ? null : child.DataContext;
            }
        }

        public string DefaultNickName
        {
            get
            {
                return this.parent.DefaultNick;
            }
        }

        public void AddOwnedWindow(IntPtr handle)
        {
            if (this.parent.InvokeRequired)
            {
                this.parent.EndInvoke(this.parent.BeginInvoke(new Action<IntPtr>(AddOwnedWindow), handle));
                return;
            }

            IRCProviders.Win32.SetWindowLongFunc(handle, -8, (uint)this.parent.Handle);
        }

        public System.Drawing.Color[] ColorsTable
        {
            get { return new IRCProviders.Colors().ColorList; }
        }

        public ICIRCeItem AddItem(ICIRCeItem parent, IntPtr handle, string title, IntPtr hIcon)
        {
            if (this.parent.InvokeRequired)
            {
                return (ICIRCeItem)this.parent.EndInvoke(this.parent.BeginInvoke(new Func<ICIRCeItem, IntPtr, string, IntPtr, ICIRCeItem>(AddItem), parent, handle, title, hIcon));
            }

            var parentItem = parent as CIRCeItem;
            if (parentItem == null)
                return null;

            IRCProviders.Win32.RECT rect;
            IRCProviders.Win32.GetWindowRect(handle, out rect);

            var icon = hIcon != IntPtr.Zero ? Icon.FromHandle(hIcon) : null;

            var hostForm = new IRCProviders.IRCForm { Icon = icon, Text = title, Width = rect.right - rect.left, Height = rect.bottom - rect.top + 2 };
            
            var hostPanel = new HwndHost { Dock = System.Windows.Forms.DockStyle.Fill };
            hostPanel.Child = handle;

            hostForm.Controls.Add(hostPanel);

            this.parent.RegisterAsMDIChild(parentItem.Form, hostForm, null);

            if (icon != null)
            {
                IRCProviders.Win32.DestroyIcon(icon.Handle);
            }

            return new CIRCeItem(hostForm);
        }

        public void Status(string message)
        {
            this.parent.Status(message);
        }

        #endregion

        private ICommandsList commands = null;

        public ICommandsList Commands
        {
            get
            {
                if (this.commands == null && this.parent.MainMenuStrip != null)
                {
                    this.commands = new CIRCeMenuCommands { Dispatcher = this.parent.MainMenuStrip, Items = this.parent.MainMenuStrip.Items };
                }

                return this.commands;
            }
        }
        
        protected internal void OnDeactivated()
        {
            if (Deactivated != null)
                Deactivated();
        }

        public event Action Deactivated;
    }
}
