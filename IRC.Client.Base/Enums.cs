using System;
using System.Collections.Generic;
using System.Text;

namespace IRC.Client.Base
{
    /// <summary>
    /// Режимы канала
    /// </summary>
    [Flags]
    public enum ChannelModes
    {
        None = 0
    }

    /// <summary>
    /// Режимы пользователя на канале
    /// </summary>
    [Flags]
    public enum ChannelUserModes
    {
        /// <summary>
        /// Обычный режим
        /// </summary>
        None = 0,
        /// <summary>
        /// Оператор
        /// </summary>
        Op = 1,
        /// <summary>
        /// Полуоператор
        /// </summary>
        Halfop = 2,
        /// <summary>
        /// Обладающий голосом
        /// </summary>
        Voice = 4
    }
}
