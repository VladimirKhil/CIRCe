using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRC.Client.Base;

namespace CIRCe.Base
{
    /// <summary>
    /// Интерфейс списка пользователей
    /// </summary>
    public interface IUsersList
    {
        IUsersCommandsList Commands { get; }
        ICIRCeChannelUserItem GetItem(ChannelUserInfo userInfo);
    }
}
