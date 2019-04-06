using System;
using System.Collections.Generic;
using System.Text;

namespace IRCConnection
{
    /// <summary>
    /// Список каналов
    /// </summary>
    public class ChannelList: List<Channel>
    {
        public new void Add(Channel item)
        {
            if (item.Name.Length == 0)
                item.Name = "#unknown";
            base.Add(item);
        }
    }
}
