using System;
using System.Collections.Generic;
using System.Text;

namespace IRCConnection
{
    /// <summary>
    /// Список ников
    /// </summary>
    public class NickList: List<NickName>
    {
        public new void Add(NickName item)
        {
            if (item.Nick.Length == 0)
                item.Nick = "Unknown";
            base.Add(item);
        }
    }
}
