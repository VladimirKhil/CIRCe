using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using IRCProviders;
using System.ComponentModel;

namespace IRCWindow
{
    public class IRCContextMenuStrip: ContextMenuStrip, IContextMenuStrip
    {
        internal IRCContextMenuStrip(): base() { }
        internal IRCContextMenuStrip(IContainer container) : base(container) { }

        #region IContextMenuStrip Members

        private delegate void Del2(int index);
        private delegate int Del3(ToolStripItem item);

        public void Add(ToolStripItem item)
        {
            if (this.InvokeRequired)
            {
                Action<ToolStripItem> del = new Action<ToolStripItem>(Add);
                IAsyncResult result = this.BeginInvoke(del, new object[] { item });
                this.EndInvoke(result);
            }
            else
                Items.Add(item);
        }

        public void Remove(ToolStripItem item)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<ToolStripItem>(Remove), item);
            }
            else
                Items.Remove(item);
        }

        public void RemoveAt(int index)
        {
            if (this.InvokeRequired)
            {
                Del2 del = new Del2(RemoveAt);
                IAsyncResult result = this.BeginInvoke(del, new object[] { index });
                this.EndInvoke(result);
            }
            else
                Items.RemoveAt(index);
        }

        public int IndexOf(ToolStripItem item)
        {
            if (this.InvokeRequired)
            {
                Del3 del = new Del3(IndexOf);
                IAsyncResult result = this.BeginInvoke(del, new object[] { item });
                return (int)this.EndInvoke(result);
            }
            else
                return Items.IndexOf(item);
        }

        #endregion
    }
}
