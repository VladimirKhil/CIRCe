using System;
using System.Collections.Generic;
using System.Text;

namespace IRCWindow
{
    internal sealed class PingInfo
    {
        internal string Server { get; set; }
        internal DateTime SentTime { get; set; }
        internal int Pid { get; set; }
    }
}
