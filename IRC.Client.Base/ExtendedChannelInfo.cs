using System;
using System.Collections.Generic;
using System.Text;

namespace IRC.Client.Base
{
    /// <summary>
    /// Расширенная информация о канале (используется при выводе списка каналов)
    /// </summary>
    [Serializable]
    public sealed class ExtendedChannelInfo: ChannelInfo
    {
        /// <summary>
        /// Число пользователей
        /// </summary>
        public int UserNum { get; set; }
        /// <summary>
        /// Тема
        /// </summary>
        public string Topic { get; set; }
    }
}
