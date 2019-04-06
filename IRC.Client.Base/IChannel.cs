using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace IRC.Client.Base
{
    /// <summary>
    /// IRC-канал
    /// </summary>
    public interface IChannel: ISession
    {
        /// <summary>
        /// Пользователи на канале
        /// </summary>
        IChangeable<ChannelUserInfo> Users { get; }
        /// <summary>
        /// Режимы канала
        /// </summary>
        ChannelModes Modes { get; }
    }
}
