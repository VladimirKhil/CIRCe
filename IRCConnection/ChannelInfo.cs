using System;
using System.Collections.Generic;
using System.Text;

namespace IRCConnection
{
    /// <summary>
    /// Информация о канале
    /// </summary>
    public class ChannelInfo : Channel
    {
        int userNum = 0;
        //string mode = string.Empty;
        string topic = string.Empty;

        public int UserNum { get { return userNum; } }
        //public string Mode { get { return mode; } }
        public string Topic { get { return topic; } }

        public ChannelInfo(string name, int userNum/*, string mode*/, string topic):base(name)
        {
            this.userNum = userNum;
            //this.mode = mode;
            this.topic = topic;
        }
    }
}
