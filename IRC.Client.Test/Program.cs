using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRC.Client.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var application = new IRC.Client.Application();
            var server = application.CreateConnection(new IRC.Client.Base.ConnectionInfo { Nick = "test", Server = new Base.ServerInfo { Name = "irc.forestnet.org", Port = 6667 } });

            if (!server.Connect())
                return;

            var channel = server.JoinChannel("#test");
            channel.SendMessage("Hello, world!");

            server.Dispose();
        }
    }
}
