using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using IRCProviders;
using System.Drawing;
using System.Linq;

namespace IRCWindow
{
    public class ChannelUsersList: DataGridView, IListView
    {
        private object syncRoot = new object();

        public object SyncRoot { get { return this.syncRoot; } }

        public override System.Drawing.Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                this.DefaultCellStyle.Font = value;
            }
        }

        public Color CellsBackgroundColor
        {
            get { return this.DefaultCellStyle.BackColor; }
            set { this.DefaultCellStyle.BackColor = value; }
        }

        public ChannelUsersList()
        {
            this.CurrentCell = null;
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            this.CurrentCell = null;
        }
        
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (this.HitTest(e.X, e.Y).Type != DataGridViewHitTestType.Cell)
                this.CurrentCell = null;
        }

        protected override void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
        {
            base.OnCellMouseDown(e);

            if (e.Button == MouseButtons.Right && this.CurrentCell == null)
            {
                this.Focus();
                this.CurrentCell = this.Rows[e.RowIndex].Cells[e.ColumnIndex];
            }
        }

        #region IListView Members

        public IChannelUser[] LVItems
        {
            get
            {
                return ((IEnumerable<ChannelUser>)((BindingSource)this.DataSource).DataSource).ToArray();
            }
        }

        public IChannelUser[] LVSelectedItems
        {
            get
            {
                var list = ((BindingSource)this.DataSource).DataSource as IList<ChannelUser>;
                var result = new List<ChannelUser>();

                lock (this.syncRoot)
                {
                    foreach (DataGridViewRow item in this.SelectedRows)
                    {
                        result.Add(list[item.Index]);
                    }
                }
                return result.ToArray();
            }
        }

        public IContextMenuStrip LVContextMenu
        {
            get { return GetContextMenu(); }
        }

        delegate IContextMenuStrip GetContextMenuDel();
        private IContextMenuStrip GetContextMenu()
        {
            if (this.InvokeRequired)
            {
                GetContextMenuDel del = new GetContextMenuDel(GetContextMenu);
                IAsyncResult result = this.BeginInvoke(del, null);
                return (IContextMenuStrip)this.EndInvoke(result);
            }
            else
            {
                return this.ContextMenuStrip as IContextMenuStrip;
            }
        }

        #endregion

        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // ChannelUsersList
            // 
            this.ShowCellToolTips = false;
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        internal Color GetBackColor(string nick)
        {
            if (this.InvokeRequired)
            {
                var del = new Func<string, Color>(GetBackColor);
                IAsyncResult result = this.BeginInvoke(del, nick);
                return (Color)this.EndInvoke(result);
            }
            else
            {
                var items = this.LVItems;
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i].NickName == nick)
                    {
                        return this.Rows[i].Cells[0].Style.BackColor;
                    }
                }
                return Color.Empty;
            }
        }

        internal void SetBackColor(string nick, Color value)
        {
            if (this.InvokeRequired)
            {
                var del = new Action<string, Color>(SetBackColor);
                IAsyncResult result = this.BeginInvoke(del, nick, value);
                this.EndInvoke(result);
            }
            else
            {
                var items = this.LVItems;
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i].NickName == nick)
                    {
                        this.Rows[i].Cells[0].Style.BackColor = value;
                        break;
                    }
                }
            }
        }

        #region IListView Members


        public IDataRow this[string nickName]
        {
            get { return new IRCDataRow(this, nickName); }
        }

        #endregion
    }
}
