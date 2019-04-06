using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIRCe.Base;
using System.ComponentModel;
using IRC.Client.Base;
using IRC.Client;

namespace IRCWindow.ViewModel
{
    internal sealed class CIRCeChannel: CIRCeSession, ICIRCeChannel
    {
        private MDIChildChannel channel = null;
        private CIRCeUsersList usersList = null;

        public CIRCeChannel(MDIChildChannel channel)
            : base(channel)
        {
            this.channel = channel;
            this.users = new Changeable<IRC.Client.Base.ChannelUserInfo>(this.channel.UsersList.LVItems.Select(cu => cu.ToNew()).ToList());
        }

        public IUsersList UsersList
        {
            get 
            {
                if (this.usersList == null)
                {
                    this.usersList = new CIRCeUsersList { List = this.channel.UsersList }; 
                }

                return this.usersList;
            }
        }

        private Changeable<IRC.Client.Base.ChannelUserInfo> users = null;

        public IChangeable<IRC.Client.Base.ChannelUserInfo> Users
        {
            get 
            {
                return this.users;
            }
        }

        public IRC.Client.Base.ChannelModes Modes
        {
            get { return IRC.Client.Base.ChannelModes.None; }
        }
    }
}
