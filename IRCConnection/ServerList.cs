using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Drawing.Design;
using System.Collections;
using System.Runtime.InteropServices;

namespace IRCConnection
{
    /// <summary>
    /// Спсиок серверов
    /// </summary>
    public class ServerList: List<Server>
    {
        public new void Add(Server item)
        {
            if (item.Name.Length == 0)
                item.Name = "unknown";
            base.Add(item);
        }
    }
}
