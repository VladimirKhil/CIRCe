using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIRCe.Base;
using IRC.Client.Base;

namespace IRCWindow.ViewModel
{
    internal sealed class CIRCePanel : InfiniteMarshalByRefObject, IPanel
    {
        private IRCWindow.IRCPanel panel = null;

        private Dictionary<IntPtr, System.Windows.Forms.Panel> index = new Dictionary<IntPtr, System.Windows.Forms.Panel>();

        internal IRCWindow.IRCPanel Panel
        {
            set { this.panel = value; }
        }

        public void AddBottom(IntPtr handle)
        {
            if (this.panel.InvokeRequired)
            {
                this.panel.BeginInvoke(new Action<IntPtr>(AddBottom), handle);
                return;
            }

            IRCProviders.Win32.RECT rect;
            IRCProviders.Win32.GetWindowRect(handle, out rect);

            var hostPanel = new HwndHost { Width = rect.right - rect.left, Height = rect.bottom - rect.top + 2, Dock = System.Windows.Forms.DockStyle.Bottom };
            hostPanel.Child = handle;
            
            index[handle] = hostPanel;
            this.panel.AddControl(hostPanel);
        }

        public void RemoveBottom(IntPtr handle)
        {
            if (this.panel.InvokeRequired)
            {
                this.panel.BeginInvoke(new Action<IntPtr>(RemoveBottom), handle);
                return;
            }

            System.Windows.Forms.Panel host;
            if (this.index.TryGetValue(handle, out host))
            {
                this.panel.RemoveControl(host);
                this.index.Remove(handle);
            }
        }
    }
}
