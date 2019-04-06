using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRC.Client.Base;
using CIRCe.Base;

namespace IRCWindow.ViewModel
{
    /// <summary>
    /// Класс, обеспечивающий совместимость старого и нового интерфейсов Цирцеи
    /// </summary>
    internal static class InteropManager
    {
        internal static IRCConnection.User ToOld(this UserInfo userInfo)
        {
            return new IRCConnection.User { Name = userInfo.RealName, UserName = userInfo.UserName, Info = userInfo.Info, EMail = userInfo.EMail, Host = "" };
        }

        internal static UserInfo ToNew(this IRCConnection.IUser user)
        {
            return new UserInfo { RealName = user.Name, UserName = user.UserName, EMail = user.EMail };
        }

        internal static IRCConnection.Server ToOld(this ServerInfo serverInfo)
        {
            return new IRCConnection.Server { Name = serverInfo.Name, Port = serverInfo.Port };
        }

        internal static ServerInfo ToNew(this IRCConnection.IServer server)
        {
            return new ServerInfo { Name = server.Name, Port = server.Port };
        }

        internal static IRCConnection.ConnectionInfo ToOld(this ConnectionInfo connectionInfo)
        {
            return new IRCConnection.ConnectionInfo { Nick = new IRCConnection.NickName { Nick = connectionInfo.Nick }, Server = connectionInfo.Server.ToOld() };
        }

        internal static ChannelUserInfo ToNew(this IRCProviders.IChannelUser user)
        {
            var channelUserInfo = new ChannelUserInfo { NickName = user.NickName };

            foreach (var mode in IRC.Client.Application.ChannelUserModesTable)
            {
                if (user.Modes.ContainsKey(mode.Value) && user.Modes[mode.Value])
                    channelUserInfo.Modes |= mode.Key;
            }

            return channelUserInfo;
        }

        internal static SerializableKeyEventArgs ToNew(this IRCProviders.SerializableKeyEventArgs args)
        {
            return new SerializableKeyEventArgs
            {
                Alt = args.Alt,
                Control = args.Control,
                Handled = args.Handled,
                KeyCode = (Keys)args.KeyCode,
                KeyData = (Keys)args.KeyData,
                KeyValue = args.KeyValue,
                Modifiers = (Keys)args.Modifiers,
                Shift = args.Shift,
                SuppressKeyPress = args.SuppressKeyPress
            };
        }

        internal static SerializableKeyPressedEventArgs ToNew(this IRCProviders.SerializableKeyPressedEventArgs args)
        {
            return new SerializableKeyPressedEventArgs
            {
                Handled =args.Handled, KeyChar = args.KeyChar
            };
        }
    }
}
