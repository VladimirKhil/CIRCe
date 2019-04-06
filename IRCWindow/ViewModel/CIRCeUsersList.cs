using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIRCe.Base;
using IRC.Client.Base;

namespace IRCWindow.ViewModel
{
    internal sealed class CIRCeUsersList : InfiniteMarshalByRefObject, IUsersList
    {
        private IRCProviders.IListView list = null;
        private CIRCeUsersCommandsList commands = null;

        internal IRCProviders.IListView List
        {
            set { this.list = value; }
        }

        public IUsersCommandsList Commands
        {
            get
            {
                if (this.commands == null)
                {
                    this.commands = new CIRCeUsersCommandsList { Menu = this.list.LVContextMenu, List = this.list };
                }

                return this.commands;
            }
        }

        public ICIRCeChannelUserItem GetItem(ChannelUserInfo userInfo)
        {
            return new CIRCeChannelUserItem { DataRow = this.list[userInfo.NickName] };
        }
    }
}
