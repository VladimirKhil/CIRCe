using System;
using System.Collections.Generic;
using System.Text;
using IRCProviders;

namespace IRCWindow
{
    internal sealed class IRCDataRow: IDataRow
    {
        private ChannelUsersList list = null;
        private string nick = null;

        public IRCDataRow(ChannelUsersList list, string nick)
        {
            this.list = list;
            this.nick = nick;
        }

        #region IDataRow Members

        public System.Drawing.Color BackColor
        {
            get
            {
                return list.GetBackColor(nick);
            }
            set
            {
                list.SetBackColor(nick, value);
            }
        }

        #endregion
    }
}
