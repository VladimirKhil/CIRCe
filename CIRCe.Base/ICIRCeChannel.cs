using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRC.Client.Base;

namespace CIRCe.Base
{
    /// <summary>
    /// Канал Цирцеи
    /// </summary>
    public interface ICIRCeChannel: IChannel, ICIRCeSession
    {
        IUsersList UsersList { get; }
    }
}
