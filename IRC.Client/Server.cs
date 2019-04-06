using System;
using System.Collections.Generic;
using System.Text;
using IRC.Client.Base;
using System.Threading;
using System.ComponentModel;
using System.Diagnostics;

namespace IRC.Client
{
    /// <summary>
    /// IRC-сервер
    /// </summary>
    internal sealed class Server : IServer
    {
        private ConnectionInfo info = null;
        private Connection connection = null;
        private bool isConnected = false;
        private object connectionSync = new object();

        private DateTime lastMessageReceived = DateTime.Now;
        private StringBuilder cache = new StringBuilder();

        private Changeable<IChannel> channels = new Changeable<IChannel>();
        private Changeable<IPrivateSession> privates = new Changeable<IPrivateSession>();

        internal Server(ConnectionInfo info)
        {
            this.info = info;
            this.connection = new Connection(info);
            //this.connection.MessageReceived += connection_MessageReceived;
        }

        private bool IsMyNick(string nick)
        {
            return this.info.Nick == nick;
        }

        //void connection_MessageReceived(string m)
        //{
        //    this.lastMessageReceived = DateTime.Now;

        //    // Кэширование сообщений
        //    if (cache.Length > 0)
        //    {
        //        m = cache + m;
        //        cache = new StringBuilder();
        //    }

        //    var lines = m.Split(new string[] { Symbols.StringSeparator }, StringSplitOptions.RemoveEmptyEntries);

        //    if (!m.EndsWith(Symbols.StringSeparator))
        //    {
        //        cache = new StringBuilder(lines[lines.Length - 1]);
        //        lines[lines.Length - 1] = string.Empty;
        //    }

        //    // Здесь обрабатываем спецсообщения
        //    // http://irc.tm-net.ru/files/rfc1459-rus.php
        //    // http://svn.hydrairc.com/hydrairc/trunk/Reference%20Docs/irc-numerics-conflicts.htm
        //    foreach (string line in lines)
        //    {
        //        if (line.Length == 0)
        //            continue;

        //        var message = Message.Parse(line);
        //        if (MessageReceived != null)
        //        {
        //            var args = new MessageEventArgs(message);
        //            MessageReceived(this, args);
        //            if (args.Handled)
        //                continue;
        //        }

        //        var tail = message.Tail;
        //        var param = message.Params.ToArray();
        //        var prefix = message.Prefix;
        //        var name = message.Name;
        //        try
        //        {
        //            switch (message.Command)
        //            {
        //                case "ERROR":
        //                    OnError(message.Tail);
        //                    break;

        //                case "INVITE":
        //                    OnInvite(message.Name, message.Tail);
        //                    break;

        //                case "JOIN":
        //                    #region JOIN
        //                    if (IsMyNick(message.Name))
        //                    {
        //                        var channelList = string.IsNullOrEmpty(message.Tail) ? message.Params.ToArray() : message.Tail.Split(',');
        //                        Array.ForEach(channelList, channel => JoinChannel(channel));
        //                    }
        //                    else
        //                    {
        //                        var channel = (Channel)this[tail];
        //                        channel.AddPerson(message.Name);
        //                    }
        //                    break;
        //                    #endregion

        //                case "KICK":
        //                    #region KICK
        //                    var child = this[param[0]];

        //                    if (IsMyNick(param[1]))
        //                    {
        //                        if (name == tail)
        //                            OnSelfKicked(param[0], message.Name);
        //                        else
        //                            OnSelfKicked(param[0], message.Name, tail);
        //                    }
        //                    else
        //                    {
        //                        ((Channel)child).KickPerson(param[1], message.Name, tail);
        //                    }
        //                    break;
        //                    #endregion

        //                case "MODE":
        //                    #region MODE
        //                    if (param[0][0] == '#')
        //                    {
        //                        if (param.Length == 2)
        //                            ((Channel)this[param[0]]).SetMode(message.Name, param[1]);
        //                        else
        //                        {
        //                            var persons = new string[param.Length - 2];
        //                            for (int i = 2; i < param.Length; i++)
        //                            {
        //                                persons[i - 2] = param[i];
        //                            }
        //                            ((Channel)this[param[0]]).SetMode(message.Name, param[1], persons);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        var mode = tail;
        //                        if (param.Length > 1)
        //                            mode = param[1];

        //                        OnSetMode(name, mode);
        //                    }

        //                    break;
        //                    #endregion

        //                case "NICK":
        //                    ChangeNick(message.Name, tail);
        //                    break;

        //                case "NOTICE":
        //                    #region NOTICE
        //                    if (tail[0] == Special.Ctcp)
        //                    {
        //                        int next = tail.IndexOf(Special.Ctcp, 1);
        //                        string ctcp = next > 1 ? tail.Substring(1, next - 1) : tail.Substring(1);
        //                        string[] ctcpParams = ctcp.Split(' ');

        //                        MDIChild childForm = (MDIChild)this.ActiveIRCWindow;
        //                        LogMode mode = childForm.Logging;
        //                        childForm.Logging = LogMode.None;
        //                        switch (ctcpParams[0])
        //                        {
        //                            case "PING":
        //                                var pingInfo = ctcpParams.Length > 1 ?
        //                                    pingTimeTable.Find(info => info.Server == name && info.Pid.ToString() == ctcpParams[1])
        //                                    : pingTimeTable.Find(info => info.Server == name);
        //                                if (pingInfo != null)
        //                                {
        //                                    var ping = DateTime.Now - pingInfo.SentTime;
        //                                    childForm.PutMessage(string.Format(Resources.PingAnswer, name, ping), settings.Colors[6], false);
        //                                    pingTimeTable.Remove(pingInfo);
        //                                }
        //                                break;

        //                            case "VERSION":
        //                                childForm.PutMessage(string.Format(Resources.VersionAnswer, name, ctcp.Substring(8)), settings.Colors[6], false);
        //                                break;

        //                            case "USERINFO":
        //                                childForm.PutMessage(string.Format(Resources.UserInfoAnswer, name, ctcp.Substring(9)), settings.Colors[6], false);
        //                                break;

        //                            //case "CLIENTINFO":
        //                            //    break;

        //                            //case "FINGER":                                        
        //                            //    break;

        //                            //case "SOURCE":
        //                            //    break;

        //                            case "TIME":
        //                                childForm.PutMessage(string.Format(Resources.TimeAnswer, name, ctcp.Substring(5)), settings.Colors[6], false);
        //                                break;

        //                            default:
        //                                childForm.PutMessage(string.Format("-{0}- {1}", prefix, tail), settings.Colors[6], false);
        //                                break;
        //                        }
        //                        childForm.Logging = mode;
        //                    }
        //                    else
        //                    {
        //                        PutMessage(tail, settings.Colors[5], false);
        //                        if (tail.Contains("/msg NickServ IDENTIFY"))
        //                        {
        //                            var password = this.Server.Passwords.Find(pass => pass.Nick == this.Nick.Nick);
        //                            if (password != null)
        //                            {
        //                                connection.RunCmd("PRIVMSG", "NickServ", ":IDENTIFY", password.Pass);
        //                            }
        //                            else if (UISettings.Default.PutRegisterMessage)
        //                            {
        //                                this.irtbPrintMessage.Clear();
        //                                this.irtbPrintMessage.SetText("/msg NickServ IDENTIFY ");
        //                                this.irtbPrintMessage.Select(this.irtbPrintMessage.Text.Length, 0);
        //                                this.irtbPrintMessage.SelectionColor = this.irtbPrintMessage.BackColor;
        //                            }
        //                        }
        //                    }
        //                    break;
        //                    #endregion

        //                case "PART":
        //                    #region PART
        //                    if (name != connection.Nick.Nick)
        //                    {
        //                        child = ChildByName(param[0]);
        //                        child.RemovePerson(name, true);
        //                        child.PutMessage(string.Format("* {0} {1}", name, Resources.LeavesChannel), settings.Colors[3], true, MessageType.UserEvent);
        //                    }
        //                    else
        //                    {
        //                        if (channelForms.ContainsKey(param[0]))
        //                            ChildByName(param[0]).Close();
        //                    }
        //                    break;
        //                    #endregion

        //                case "PING":
        //                    connection.RunCmd(line.Replace("PING", "PONG"));
        //                    break;

        //                case "PONG":
        //                    {
        //                        var pingInfo = tail.Length > 0 ?
        //                                        pingTimeTable.Find(info => info.Server == name && info.Pid.ToString() == tail)
        //                                        : pingTimeTable.Find(info => info.Server == name);
        //                        if (pingInfo != null)
        //                        {
        //                            var ping = DateTime.Now - pingInfo.SentTime;
        //                            PutMessage(string.Format(Resources.PingAnswer, name, ping), settings.Colors[6], false);
        //                            pingTimeTable.Remove(pingInfo);
        //                        }
        //                    }
        //                    break;

        //                case "PRIVMSG":
        //                    #region PRIVMSG
        //                    if (tail[0] == Special.Ctcp)
        //                    {
        //                        int next = tail.IndexOf(Special.Ctcp, 1);
        //                        string ctcp = next > 1 ? tail.Substring(1, next - 1) : tail.Substring(1);
        //                        string[] ctcpParams = ctcp.Split(' ');

        //                        switch (ctcpParams[0])
        //                        {
        //                            case "PING":
        //                                Notice(name, Special.Ctcp + ctcp + Special.Ctcp);
        //                                break;

        //                            case "VERSION":
        //                                AssemblyName program = Assembly.GetExecutingAssembly().GetName();
        //                                Notice(name, Special.Ctcp + "VERSION " + program.Name + " " + program.Version + " by Vladimir Khil" + Special.Ctcp);
        //                                break;

        //                            case "USERINFO":
        //                                string info = Settings.Default.UserInfoString;
        //                                Notice(name, Special.Ctcp + "USERINFO " + (info.Length > 0 ? info : this.connection.Nick.User.ToString()) + Special.Ctcp);
        //                                break;

        //                            case "CLIENTINFO":
        //                                Notice(name, Special.Ctcp + "CLIENTINFO " + Assembly.GetExecutingAssembly().GetName().Name + " supplies CTCP: PING, VERSION, USERINFO, CLIENTINFO, FINGER, SOURCE, TIME, ACTION"/*, AVATAR, MULTIMEDIA, DCC"*/ + Special.Ctcp);
        //                                break;

        //                            case "FINGER":
        //                                Notice(name, Special.Ctcp + "FINGER " +
        //                                    Resources.RealName + " " + this.connection.Nick.User.Name +
        //                                    " " + Resources.Email + " " + this.connection.Nick.User.EMail +
        //                                    " " + Resources.IdleTime + " " + (DateTime.Now - lastCmdTime).ToString() +
        //                                    Special.Ctcp);
        //                                break;

        //                            case "SOURCE":
        //                                Notice(name, Special.Ctcp + "SOURCE http://ur-quan1986.narod.ru" + Special.Ctcp);
        //                                break;

        //                            case "TIME":
        //                                Notice(name, Special.Ctcp + "TIME " + DateTime.Now.ToString() + Special.Ctcp);
        //                                break;

        //                            case "ACTION": // by NOTICE only
        //                                ChildByName(param[0] != connection.Nick.Nick ? param[0] : name).PutMessage(String.Format("* {0} {1}", name, ctcp.Substring(7)), Settings.Default.Colors[6]);
        //                                return;

        //                            /*case "AVATAR":
        //                                break;

        //                            case "MULTIMEDIA":
        //                                break;

        //                            case "DCC":
        //                                break;*/

        //                            default:
        //                                break;
        //                        }

        //                        ActiveIRCWindow.PutMessage(String.Format("{0} [CTCP: {1}]", name, ctcp), settings.Colors[4]);
        //                    }
        //                    else
        //                    {
        //                        if (param[0] != connection.Nick.Nick)
        //                        {
        //                            if (!channelForms.TryGetValue(param[0].ToLower(), out child))
        //                                break;
        //                        }
        //                        else
        //                            child = ChildByName(name);
        //                        child.PutTextMessage(name, tail);
        //                    }
        //                    break;
        //                    #endregion

        //                case "QUIT":
        //                    foreach (var item in channelForms.Values)
        //                    {
        //                        if (item.HasPerson(name))
        //                        {
        //                            item.RemovePerson(name, true);
        //                            item.PutMessage(String.Format("* {0} {1}{2}", name, Resources.QuitsIRC, (tail.Length > 0 ? " (" + tail + ")" : string.Empty)), settings.Colors[2], true, MessageType.UserEvent);
        //                        }
        //                    }
        //                    break;

        //                case "TOPIC": // Ср. case "332"
        //                    ChildByName(param[0]).PutMessage(string.Format("{0} {1}", name, Resources.SetsNewTopic), Settings.Default.Colors[3]);
        //                    (ChildByName(param[0]) as MDIChildChannel).Topic = tail;
        //                    break;

        //                case "001": // RPL_WELCOME
        //                    ChangeNick(connection.Nick.Nick, param[0]);
        //                    PutMessage(tail, settings.Colors.DefForeColor);
        //                    break;

        //                case "002": // RPL_YOURHOST
        //                    PutMessage(tail, settings.Colors.DefForeColor);
        //                    break;

        //                case "003": // RPL_CREATED
        //                    PutMessage(tail, settings.Colors.DefForeColor);
        //                    break;

        //                case "004": // RPL_MYINFO
        //                    var text = new StringBuilder();
        //                    for (int i = 1; i < param.Length; i++)
        //                    {
        //                        text.Append(param[i]);
        //                        text.Append(' ');
        //                    }
        //                    PutMessage(text.ToString(), settings.Colors.DefForeColor);
        //                    break;

        //                case "005": // RPL_BOUNCE // RPL_ISUPPORT
        //                    text = new StringBuilder();
        //                    for (int i = 1; i < param.Length; i++)
        //                    {
        //                        text.Append(param[i]);
        //                        text.Append(' ');
        //                    }
        //                    PutMessage(text.ToString(), settings.Colors.DefForeColor);
        //                    break;

        //                //case "200": // RPL_TRACELINK

        //                //case "201": // RPL_TRACECONNECTING

        //                //case "202": // RPL_TRACEHANDSHAKE

        //                //case "203": // RPL_TRACEUNKNOWN

        //                //case "204": // RPL_TRACEOPERATOR

        //                //case "205": // RPL_TRACEUSER

        //                //case "206": // RPL_TRACESERVER

        //                //case "208": // RPL_TRACENEWTYPE

        //                //case "211": // RPL_STATSLINKINFO

        //                case "212": // RPL_STATSCOMMANDS
        //                    StringBuilder result = new StringBuilder();
        //                    for (int i = 1; i < param.Length; i++)
        //                    {
        //                        result.Append(param[i]);
        //                        result.Append(' ');
        //                    }
        //                    result.AppendFormat("({0})", message.Tail);
        //                    ActiveIRCWindow.PutMessage(result.ToString(), settings.Colors[5], false);
        //                    break;

        //                //case "213": // RPL_STATSCLINE

        //                //case "214": // RPL_STATSNLINE

        //                //case "215": // RPL_STATSILINE

        //                //case "216": // RPL_STATSKLINE

        //                //case "218": // RPL_STATSYLINE

        //                case "219": // RPL_ENDOFSTATS
        //                    ActiveIRCWindow.PutMessage(Resources.EndOfStats, settings.Colors[5], false);
        //                    break;

        //                case "221": // RPL_UMODEIS
        //                    PutMessage(string.Format("{0}: {1}", Resources.YourModeIs, param[1]), settings.Colors[3]);
        //                    break;

        //                //case "225": // RPL_STATSCLONE

        //                //case "226": // RPL_STATSCOUNT

        //                //case "227": // RPL_STATSGLINE

        //                //case "241": // RPL_STATSLLINE

        //                case "242": // RPL_STATSUPTIME
        //                    ActiveIRCWindow.PutMessage(tail, settings.Colors[5], false);
        //                    break;

        //                //case "243": // RPL_STATSOLINE

        //                //case "244": // RPL_STATSHLINE

        //                //case "245": // RPL_STATSSLINE

        //                //case "246": // RPL_STATSXLINE

        //                case "250": // RPL_STATSDLINE
        //                    PutMessage(tail, settings.Colors[6]);
        //                    break;

        //                case "251": // RPL_LUSERCLIENT
        //                    #region RPL_LUSERCLIENT
        //                    // Это сообщение является при любом подключении обязательным
        //                    // Оно обозначает успешность подключения
        //                    connection.UsingServer = message.Name;
        //                    PutMessage("***", settings.Colors[6]);
        //                    PutMessage(tail, settings.Colors[6]);
        //                    if (OnConnected != null)
        //                        OnConnected(this, new EventArgs());
        //                    this.MyNode.ForeColor = Color.Red;
        //                    fullConnect = true;
        //                    numOfTries = 0;

        //                    foreach (ToolStripItem item in this.contextMenuStripChat.Items)
        //                    {
        //                        item.Visible = true;
        //                    }

        //                    if (!string.Equals(this.ghostNick, null))
        //                    {
        //                        var password = this.Server.Passwords.Find(pass => pass.Nick == this.ghostNick);
        //                        if (password != null)
        //                        {
        //                            connection.RunCmd("ns", "GHOST", password.Nick, password.Pass);
        //                            new Thread(this.DelayedSetNick).Start(password.Nick);
        //                        }
        //                    }

        //                    // Здесь выполняем повторный вход на все ранее окрытые каналы
        //                    foreach (var item in channelForms.Values)
        //                    {
        //                        var channel = item as MDIChildChannel;
        //                        if (channel != null)
        //                        {
        //                            channel.ClearModes();
        //                            JoinChannel(channel.WindowName);
        //                        }
        //                    }

        //                    this.Server.Channels.FindAll(ch => ch.AutoOpen).ForEach(ch => { if (!channelForms.ContainsKey(ch.Name)) JoinChannel(ch.Name); });

        //                    break;
        //                    #endregion

        //                case "252": // RPL_LUSEROP
        //                    PutMessage(String.Format("{0}: {1}", Resources.OpsOnline, param[1]), settings.Colors[6]);
        //                    break;

        //                case "253": // RPL_LUSERUNKNOWN
        //                    PutMessage(String.Format("{0}: {1}", Resources.TotalUsers, param[1]), settings.Colors[6]);
        //                    break;

        //                case "254": // RPL_LUSERCHANNELS
        //                    PutMessage(String.Format("{0}: {1}", Resources.TotalChannels, param[1]), settings.Colors[6]);
        //                    break;

        //                case "255": // RPL_LUSERME
        //                    PutMessage(tail, settings.Colors[6]);
        //                    PutMessage("***", settings.Colors[6]);
        //                    break;

        //                case "256": // RPL_ADMINME
        //                    ActiveIRCWindow.PutMessage(String.Format("{0}:", tail), settings.Colors[6], false);
        //                    break;

        //                case "257": // RPL_ADMINLOC1
        //                    ActiveIRCWindow.PutMessage(tail, settings.Colors[6], false);
        //                    break;

        //                case "258": // RPL_ADMINLOC2
        //                    ActiveIRCWindow.PutMessage(tail, settings.Colors[6], false);
        //                    break;

        //                case "259": // RPL_ADMINEMAIL
        //                    ActiveIRCWindow.PutMessage(string.Format("{0}: {1}", Resources.AdminMailMessage, tail), settings.Colors[6], false);
        //                    break;

        //                //case "261": // RPL_TRACELOG

        //                case "262":
        //                    ActiveIRCWindow.PutMessage(Resources.EndOfTrace, settings.Colors[5], false);
        //                    break;

        //                case "263":
        //                    ActiveIRCWindow.PutMessage(Resources.ServerOverload, settings.Colors[4], false);
        //                    break;

        //                case "265": // RPL_LOCALUSERS
        //                    PutMessage(tail, settings.Colors[6]);
        //                    break;

        //                case "266": // RPL_GLOBALUSERS
        //                    PutMessage(tail, settings.Colors[6]);
        //                    break;

        //                //case "271": // RPL_SILELIST

        //                //case "272": // RPL_ENDOFSILELIST

        //                case "275":
        //                    lock (whois)
        //                    {
        //                        if (!whois.ContainsKey(param[1]))
        //                            whois[param[1]] = new UserInfo();
        //                        whois[param[1]].Ssl = tail;
        //                        if (!notPrintWhois.Contains(param[1]))
        //                        {
        //                            ActiveIRCWindow.PutMessage(String.Format("{0}: {1}", param[1], tail), 6);
        //                        }
        //                    }
        //                    break;

        //                case "281": // RPL_ACCEPT
        //                    ActiveIRCWindow.PutMessage(string.Format(Resources.AcceptList, message.Tail), settings.Colors[6], true);
        //                    break;

        //                case "282": // RPL_ENDOFACCEPTLIST
        //                    ActiveIRCWindow.PutMessage(Resources.EndOfAcceptList, settings.Colors[6], true);
        //                    break;

        //                case "301": // RPL_AWAY
        //                    foreach (MDIChildCommunication childForm in channelForms.Values)
        //                    {
        //                        // TODO: перенести в whois
        //                        PutMessage(string.Format(Resources.AwayWithWords, param[1], tail), settings.Colors[3]);
        //                    }
        //                    break;

        //                case "302": // RPL_USERHOST
        //                    #region RPL_USERHOST
        //                    MatchCollection matchCollection = Regex.Matches(message.Tail, Settings.Default.RplUserHostRegexPattern);
        //                    MDIChild form = (MDIChild)this.ActiveIRCWindow;
        //                    form.PutMessage(Resources.RplUserInfoMessage, Settings.Default.Colors[6], true);
        //                    foreach (Match match in matchCollection)
        //                    {
        //                        StringBuilder msg = new StringBuilder(match.Groups["nick"].Value);
        //                        if (match.Groups["op"].Value.Length > 0)
        //                            msg.AppendFormat(" ({0})", Resources.IRCOp);
        //                        msg.AppendFormat(" ({0}). ", match.Groups["plus"].Value.Equals("+") ? Resources.IsOnline : Resources.IsOffline);
        //                        msg.AppendFormat("{0}: {1}", Resources.Hostname, match.Groups["host"].Value);
        //                        form.PutMessage(msg.ToString(), Settings.Default.Colors[6], true);
        //                    }
        //                    break;
        //                    #endregion

        //                case "303": // RPL_ISON
        //                    ActiveIRCWindow.PutMessage(string.Format(Resources.IsOnReply, message.Tail), settings.Colors[5], true);
        //                    break;

        //                case "305": // RPL_UNAWAY
        //                    this.ActiveIRCWindow.PutMessage(Resources.MarkedAsNotBeingAway, settings.Colors[3]);
        //                    break;

        //                case "306": // RPL_NOWAWAY
        //                    this.ActiveIRCWindow.PutMessage(Resources.MarkedAsBeingAway, settings.Colors[3]);
        //                    break;

        //                case "307":
        //                    lock (whois)
        //                    {
        //                        if (!whois.ContainsKey(param[1]))
        //                            whois[param[1]] = new UserInfo();
        //                        whois[param[1]].IsRegistered = true;
        //                        if (!notPrintWhois.Contains(param[1]))
        //                        {
        //                            ActiveIRCWindow.PutMessage(string.Format(Resources.IsRegisteredNick, param[1]), 6);
        //                        }
        //                    }
        //                    break;

        //                //case "308": // RPL_WHOISADMIN

        //                //case "309": // RPL_WHOISSADMIN

        //                case "310":
        //                    // TODO: перенести в whois
        //                    PutMessage(String.Format("{0} {1}", message.Params[1], Resources.IsAvailableForHelp), Settings.Default.Colors[6]);
        //                    break;

        //                case "311": // RPL_WHOISUSER
        //                    #region RPL_WHOISUSER
        //                    lock (whois)
        //                    {
        //                        if (!whois.ContainsKey(param[1]))
        //                            whois[param[1]] = new UserInfo();
        //                        else if (whois[param[1]].Obsolete == -1)
        //                        {
        //                            //whois[param[1]] = new UserInfo();
        //                            notPrintWhois.Add(param[1]);
        //                        }
        //                        whois[param[1]].Nick = param[1];
        //                        ((User)whois[param[1]].User).UserName = param[2];
        //                        ((User)whois[param[1]].User).Host = param[3];
        //                        ((User)whois[param[1]].User).Name = tail;
        //                        whois[param[1]].Obsolete = Settings.Default.ObsoleteTime;
        //                        if (!notPrintWhois.Contains(param[1]))
        //                        {
        //                            this.ActiveIRCWindow.PutMessage(String.Format("{0}{1}:", Resources.InformationAbout, param[1]), settings.Colors[6]);
        //                            this.ActiveIRCWindow.PutMessage(String.Format("{0}: {1}@{2}", Resources.User, param[2], param[3]), settings.Colors[6]);
        //                            this.ActiveIRCWindow.PutMessage(String.Format("{0}: {1}", Resources.RealName, tail), settings.Colors[6]);
        //                        }
        //                    }
        //                    break;
        //                    #endregion

        //                case "312": // RPL_WHOISSERVER
        //                    lock (whois)
        //                    {
        //                        if (!whois.ContainsKey(param[1]))
        //                            whois[param[1]] = new UserInfo();
        //                        whois[param[1]].Server = param[2];
        //                        whois[param[1]].ServerInfo = tail;
        //                        if (!notPrintWhois.Contains(param[1]))
        //                        {
        //                            this.ActiveIRCWindow.PutMessage(string.Format("{0}: {1} ({2})", Resources.Server, param[2], tail), settings.Colors[6]);
        //                        }
        //                    }
        //                    break;

        //                case "313": // RPL_WHOISOPERATOR
        //                    lock (whois)
        //                    {
        //                        if (!whois.ContainsKey(param[1]))
        //                            whois[param[1]] = new UserInfo();
        //                        whois[param[1]].IsIRCOp = true;
        //                        if (!notPrintWhois.Contains(param[1]))
        //                        {
        //                            this.ActiveIRCWindow.PutMessage(String.Format("{0} {1}", param[1], Resources.IsIRCOp), settings.Colors[6]);
        //                        }
        //                    }
        //                    break;

        //                case "314": // RPL_WHOWASUSER
        //                    ActiveIRCWindow.PutMessage(string.Format(Resources.WhoWasUser, param[1], param[2], param[3], tail), settings.Colors[6]);
        //                    break;

        //                case "315": // RPL_ENDOFWHO
        //                    ActiveIRCWindow.PutMessage(string.Format(Resources.EndOfWhoList, param[1]), settings.Colors[6]);
        //                    break;

        //                //case "316": // RPL_WHOISCHANOP

        //                case "317": // RPL_WHOISIDLE
        //                    lock (whois)
        //                    {
        //                        if (!whois.ContainsKey(param[1]))
        //                            whois[param[1]] = new UserInfo();
        //                        whois[param[1]].Idle = int.Parse(param[2]);
        //                        if (!notPrintWhois.Contains(param[1]))
        //                        {
        //                            this.ActiveIRCWindow.PutMessage(String.Format(Resources.UserIdle, param[1], param[2]), settings.Colors[6]);
        //                        }
        //                    }
        //                    break;

        //                case "318": // RPL_WHOISSERVER // RPL_ENDOFWHOIS
        //                    if (!notPrintWhois.Contains(param[1]))
        //                        this.ActiveIRCWindow.PutMessage(Resources.EndOfData, settings.Colors[6]);
        //                    else
        //                    {
        //                        if (notPrintWhois.Contains(param[1]))
        //                            notPrintWhois.Remove(param[1]);
        //                        if (WhoisUpdated != null)
        //                            WhoisUpdated(this, new PersonEventArgs(param[1]));
        //                    }
        //                    break;

        //                case "319": // RPL_WHOISCHANNELS
        //                    #region RPL_WHOISCHANNELS
        //                    lock (whois)
        //                    {
        //                        try
        //                        {
        //                            if (!whois.ContainsKey(param[1]))
        //                                whois[param[1]] = new UserInfo();
        //                            string[] chans = tail.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        //                            Log(String.Format("{0} {1}", string.Join(" ", chans), param[1]));
        //                            Array.ForEach(chans, chan =>
        //                            {
        //                                if (Settings.Default.ModesTable.ContainsValue(chan[0]))
        //                                    whois[param[1]].Modes[chan.Substring(1)] = chan[0];
        //                                else
        //                                    whois[param[1]].Modes[chan] = ' ';
        //                            });
        //                            if (!notPrintWhois.Contains(param[1]))
        //                                this.ActiveIRCWindow.PutMessage(String.Format("{0} {1}", Resources.IsOnChannels, tail), settings.Colors[6]);
        //                        }
        //                        catch (Exception exc) { MessageBox.Show(String.Format("{0} {1}", line, exc)); }
        //                    }
        //                    break;
        //                    #endregion

        //                case "320":
        //                    lock (whois)
        //                    {
        //                        if (!whois.ContainsKey(param[1]))
        //                            whois[param[1]] = new UserInfo();
        //                        whois[param[1]].CodePage = tail;
        //                    }
        //                    if (!notPrintWhois.Contains(param[1]))
        //                        this.ActiveIRCWindow.PutMessage(String.Format("{0}: {1}", param[1], tail), settings.Colors[6]);
        //                    break;

        //                case "321": // RPL_LISTSTART
        //                    channelInfo.Clear();
        //                    break;

        //                case "322": // RPL_LIST
        //                    channelInfo.Add(new ChannelInfo(message.Params[1], int.Parse(message.Params[2]), message.Tail));
        //                    break;

        //                case "323": // RPL_LISTEND
        //                    this.channelInfo.Sort(delegate(ChannelInfo ci1, ChannelInfo ci2) { return ci1.Name.CompareTo(ci2.Name); });
        //                    var view = new ChannelViewForm(this.channelInfo);
        //                    view.NeedJoin += (channel) => JoinChannel(channel);
        //                    this.Main.RegisterAsMDIChild(this, view, null);
        //                    break;

        //                case "324": // RPL_CHANNELMODEIS
        //                    if (this.channelForms.ContainsKey(param[1].ToLower()))
        //                    {
        //                        string[] modPar = new string[param.Length - 3];
        //                        for (int i = 3; i < param.Length; i++)
        //                        {
        //                            modPar[i - 3] = param[i];
        //                        }
        //                        (ChildByName(param[1]) as MDIChildChannel).Mode(name, param[2], modPar);
        //                    }
        //                    break;

        //                case "329": // RPL_CREATIONTIME
        //                    if (this.channelForms.ContainsKey(param[1].ToLower()))
        //                        (ChildByName(param[1]) as MDIChildChannel).PutMessage(String.Format("{1} {0}", DateTime.Parse("01.01.1970 00:00:00").AddSeconds(double.Parse(param[2])).ToLocalTime(), Resources.TopicCreated), settings.Colors[3]);
        //                    break;

        //                case "330":
        //                    lock (whois)
        //                    {
        //                        if (!whois.ContainsKey(param[1]))
        //                            whois[param[1]] = new UserInfo();
        //                        whois[param[1]].Auth = param[2];
        //                        if (!notPrintWhois.Contains(param[1]))
        //                        {
        //                            ActiveIRCWindow.PutMessage(String.Format("{0}: {1} - {2}", param[1], Resources.RealName, param[2]), 6);
        //                        }
        //                    }
        //                    break;

        //                case "331": // RPL_NOTOPIC
        //                    (ChildByName(param[1]) as MDIChildChannel).Topic = string.Empty;
        //                    break;

        //                case "332": // RPL_TOPIC
        //                    (ChildByName(param[1]) as MDIChildChannel).Topic = tail;
        //                    break;

        //                case "333": // RPL_TOPICWHOTIME
        //                    (ChildByName(param[1]) as MDIChildChannel).PutMessage(String.Format("{0} {1}", Resources.TopicSetBy, param[2]), settings.Colors[3]);
        //                    break;

        //                case "334": // RPL_COMMANDSYNTAX
        //                    ActiveIRCWindow.PutMessage(tail, settings.Colors[14], false);
        //                    break;

        //                case "335":
        //                    lock (whois)
        //                    {
        //                        if (!whois.ContainsKey(param[1]))
        //                            whois[param[1]] = new UserInfo();
        //                        Log(line);
        //                        whois[param[1]].IsBot = true;
        //                        if (!notPrintWhois.Contains(param[1]))
        //                            this.ActiveIRCWindow.PutMessage(String.Format("{0}: {1}", param[1], tail), settings.Colors[6]);
        //                    }
        //                    break;

        //                //case "337": // RPL_WHOISTEXT

        //                case "338": // RPL_WHOISACTUALLY
        //                    lock (whois)
        //                    {
        //                        if (!whois.ContainsKey(param[1]))
        //                            whois[param[1]] = new UserInfo();
        //                        Log(line);
        //                        whois[param[1]].WhoisActially = tail;
        //                        if (!notPrintWhois.Contains(param[1]))
        //                            ActiveIRCWindow.PutMessage(String.Format("{0}: {1}", param[1], tail), settings.Colors[6]);
        //                    }
        //                    break;

        //                case "339": // RPL_WHOISACTUALLY
        //                    goto case "338";

        //                case "341": // RPL_INVITING
        //                    ActiveIRCWindow.PutMessage(string.Format(Resources.RplInvitingMessage, param[1], param[2]), settings.Colors[6]);
        //                    break;

        //                //case "342": // RPL_SUMMONING

        //                case "346": // RPL_INVITELIST
        //                    (ChildByName(param[1]) as MDIChildChannel).AddInvite(new ChannelListItemInfo(param[2], param[3], DateTime.Parse("01.01.1970 00:00:00").AddSeconds(double.Parse(param[4])).ToLocalTime()));
        //                    break;

        //                case "347": // RPL_ENDOFINVITELIST
        //                    (ChildByName(param[1]) as MDIChildChannel).InvitesFinished();
        //                    break;

        //                case "348": // RPL_EXEMPTLIST
        //                    (ChildByName(param[1]) as MDIChildChannel).AddExempt(new ChannelListItemInfo(param[2], param[3], DateTime.Parse("01.01.1970 00:00:00").AddSeconds(double.Parse(param[4])).ToLocalTime()));
        //                    break;

        //                case "349": // RPL_ENDOFEXEMPTLIST
        //                    (ChildByName(param[1]) as MDIChildChannel).ExemptsFinished();
        //                    break;

        //                case "351": // RPL_VERSION
        //                    string[] ver = param[1].Split('.');
        //                    ActiveIRCWindow.PutMessage(string.Format(Resources.VersionMessage, param[2], param[1] + (ver[ver.Length - 1].Length > 0 ? string.Format("({0})", Resources.ServerDebugMode) : ""), tail), settings.Colors[6]);
        //                    break;

        //                case "352": // RPL_WHOREPLY
        //                    int ind = tail.IndexOf(' ');
        //                    string hops = tail.Substring(0, ind);
        //                    string realName = tail.Substring(ind + 1);
        //                    string flags = param[6];
        //                    List<string> userData = new List<string>();
        //                    userData.Add(flags.Contains("H") ? Resources.NotAway : Resources.Away);
        //                    if (flags.Contains("*"))
        //                        userData.Add(Resources.IRCOp);
        //                    if (flags.Contains("@"))
        //                        userData.Add(Resources.WithOp);
        //                    else if (flags.Contains("+"))
        //                        userData.Add(Resources.WithVoice);
        //                    string whoInfo = string.Join(", ", userData.ToArray());
        //                    ActiveIRCWindow.PutMessage(string.Format(Resources.WhoReply, param[2], param[3], param[1], param[4], hops, whoInfo, realName, param[5]), settings.Colors[6]);
        //                    break;

        //                case "353": // RPL_NAMREPLY
        //                    string[] pers = tail.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        //                    if (channelForms.TryGetValue(param[2].ToLower(), out child))
        //                    {
        //                        Array.ForEach<string>(pers, person => { child.AddPerson(person, false); });
        //                    }
        //                    else
        //                    {
        //                        ActiveIRCWindow.PutMessage(string.Format(Resources.UsersOnChannel, param[2], tail), settings.Colors[6]);
        //                    }
        //                    break;

        //                //case "354": // RPL_RWHOREPLY

        //                //case "362": // RPL_CLOSING

        //                //case "363": // RPL_CLOSEEND

        //                case "364": // RPL_LINKS
        //                    ind = tail.IndexOf(' ');
        //                    linksData.Rows.Add(param[1], param[2], tail.Substring(0, ind), tail.Substring(ind + 1));
        //                    break;

        //                case "365": // RPL_ENDOFLINKS
        //                    using (var linksViewForm = new LinksViewForm(linksData))
        //                    {
        //                        this.Main.RegisterAsMDIChild(this, linksViewForm, null);
        //                    }
        //                    linksData.Clear();
        //                    break;

        //                case "366": // RPL_ENDOFNAMES
        //                    //ActiveIRCWindow.PutMessage(Resources.EndOfList, settings.Colors[6]);
        //                    break;

        //                case "367": // RPL_BANLIST
        //                    (ChildByName(param[1]) as MDIChildChannel).AddBan(new ChannelListItemInfo(param[2], param[3], DateTime.Parse("01.01.1970 00:00:00").AddSeconds(double.Parse(param[4])).ToLocalTime()));
        //                    break;

        //                case "368": // RPL_ENDOFBANLIST
        //                    (ChildByName(param[1]) as MDIChildChannel).BansFinished();
        //                    break;

        //                case "369": // RPL_ENDOFWHOWAS
        //                    ActiveIRCWindow.PutMessage(Resources.EndOfWhoWas, settings.Colors[6]);
        //                    break;

        //                case "371": // RPL_INFO
        //                    PutMessage(tail, settings.Colors[5], false);
        //                    break;

        //                case "372": // RPL_MOTD
        //                    if (motd)
        //                        PutMessage(tail, settings.Colors.DefForeColor, false);
        //                    break;

        //                //case "373": // RPL_INFOSTART

        //                case "374": // RPL_ENDOFINFO
        //                    PutMessage(Resources.EndOfInfo, settings.Colors[5], false);
        //                    break;

        //                case "375": // RPL_MOTDSTART
        //                    motd = true;
        //                    PutMessage(Resources.MOTD, settings.Colors[5], false);
        //                    PutMessage(tail, settings.Colors.DefForeColor, false);
        //                    break;

        //                case "376": // RPL_ENDOFMOTD
        //                    motd = false;
        //                    PutMessage(Resources.EndOfMOTD, settings.Colors[5], false);
        //                    // PutMessage(tail, settings.Colors.DefForeColor, false);
        //                    break;

        //                case "378": // RPL_WHOISHOST
        //                    lock (whois)
        //                    {
        //                        if (!whois.ContainsKey(param[1]))
        //                            whois[param[1]] = new UserInfo();
        //                        whois[param[1]].WhoisHost = tail;
        //                        if (!notPrintWhois.Contains(param[1]))
        //                            this.ActiveIRCWindow.PutMessage(String.Format("{0}: {1}", param[1], tail), settings.Colors[6]);
        //                    }
        //                    break;

        //                case "381": // RPL_YOUREOPER
        //                    PutMessage(Resources.YouAreAnIRCOp, settings.Colors[12], true);
        //                    break;

        //                //case "382": // RPL_REHASHING

        //                //case "384": // RPL_MYPORTIS

        //                case "391": // RPL_TIME
        //                    ActiveIRCWindow.PutMessage(string.Format(Resources.ServerTime, param[1], tail), settings.Colors[5]);
        //                    break;

        //                case "401": // ERR_NOSUCHNICK
        //                    ActiveIRCWindow.PutMessage(String.Format("{0}: {1}", Resources.ChannelNickDoesNotExistst, param[1]), settings.Colors[4], false);
        //                    break;

        //                case "402": // ERR_NOSUCHSERVER
        //                    PutMessage(string.Format(Resources.NoSuchServerMessage, param[1]), settings.Colors[4], false);
        //                    break;

        //                case "403": // ERR_NOSUCHCHANNEL
        //                    PutMessage(string.Format(Resources.NoSuchChannelMessage, param[1]), settings.Colors[4], false);
        //                    break;

        //                case "404": // ERR_CANNOTSENDTOCHAN
        //                    ChildByName(message.Params[1]).PutMessage(string.Format("{0}: {1}", Resources.CannotSendToChannel, message.Tail), settings.Colors[4], false);
        //                    break;

        //                case "405": // ERR_TOOMANYCHANNELS
        //                    ActiveIRCWindow.PutMessage(Resources.TooManyChannelsMessage, settings.Colors[4], false);
        //                    break;

        //                case "406": // ERR_WASNOSUCHNICK
        //                    ActiveIRCWindow.PutMessage(string.Format(Resources.WasNoSuchNick, param[1]), settings.Colors[4]);
        //                    break;

        //                case "407": // ERR_TOOMANYTARGETS
        //                    ActiveIRCWindow.PutMessage(string.Format("{0}: {1}", message.Params[1], Resources.TooManyTargetsMessage), settings.Colors[4], false);
        //                    break;

        //                //case "408": // ERR_NOCTRLSONCHAN

        //                case "409": // ERR_NOORIGIN
        //                    PutMessage(Resources.NoOriginMessage, settings.Colors[4], false);
        //                    break;

        //                //case "411": // ERR_NORECIPIENT

        //                case "412": // ERR_NOTEXTTOSEND
        //                    PutMessage(Resources.NoTextToSendMessage, settings.Colors[4], false);
        //                    break;

        //                case "413": // ERR_NOTOPLEVEL
        //                    ActiveIRCWindow.PutMessage(string.Format("{0}: {1}", message.Params[1], Resources.NoTopLevelDomainMessage), settings.Colors[4], false);
        //                    break;

        //                case "414": // ERR_WILDTOPLEVEL
        //                    ActiveIRCWindow.PutMessage(string.Format("{0}: {1}", message.Params[1], Resources.WildTopLevelMessage), settings.Colors[4], false);
        //                    break;

        //                case "421": // ERR_UNKNOWNCOMMAND
        //                    ActiveIRCWindow.PutMessage(string.Format(Resources.UnknownCommandMessage, param[1]), settings.Colors[4]);
        //                    break;

        //                case "422": // ERR_NOMOTD
        //                    PutMessage(Resources.NoMOTDMessage, settings.Colors[4], false);
        //                    break;

        //                case "423": // ERR_NOADMININFO
        //                    ActiveIRCWindow.PutMessage(string.Format(Resources.NoAdminInfoMessage, message.Params[1]), settings.Colors[4], false);
        //                    break;

        //                case "424": // ERR_FILEERROR
        //                    ActiveIRCWindow.PutMessage(tail, settings.Colors[4], false);
        //                    break;

        //                //case "429": // ERR_TOOMANYAWAY

        //                case "431": // ERR_NONICKNAMEGIVEN
        //                    ActiveIRCWindow.PutMessage(Resources.NoNicknameGivenMessage, settings.Colors[4], false);
        //                    break;

        //                case "432": // ERR_ERRONEUSNICKNAME
        //                    ActiveIRCWindow.PutMessage(String.Format("{0} {1}", Resources.BadNick, param[1]), settings.Colors[4]);
        //                    break;

        //                case "433": // ERR_NICKNAMEINUSE
        //                    {
        //                        ActiveIRCWindow.PutMessage(string.Format(Resources.NicknameInUseMessage, param[1]), settings.Colors[4]);
        //                        var password = this.Server.Passwords.Find(pass => pass.Nick == param[1]);
        //                        if (password != null)
        //                        {
        //                            this.ghostNick = param[1];
        //                        }
        //                        this.SetNick(string.Format("|{0}|", param[1]));
        //                    }
        //                    break;

        //                //case "435": // ERR_BANONCHAN

        //                //case "436": // ERR_NICKCOLLISION

        //                case "437": // ERR_BANNICKCHANGE
        //                    ActiveIRCWindow.PutMessage(Resources.BanNickChangeMessage, settings.Colors[4], false);
        //                    break;

        //                //case "438":

        //                case "439": // ERR_TARGETTOOFAST
        //                    ActiveIRCWindow.PutMessage(String.Format("{0}: {1}", param[1], tail), settings.Colors[6], false);
        //                    break;

        //                case "440": // ERR_SERVICESDOWN
        //                    PutMessage(Resources.ServiceDownMessage, settings.Colors[4]);
        //                    break;

        //                //case "441": // ERR_USERNOTINCHANNEL

        //                case "442": // ERR_NOTONCHANNEL
        //                    PutMessage(string.Format(Resources.YouAreNotOnAChannel, param[1]), settings.Colors[4]);
        //                    break;

        //                case "443": // ERR_USERONCHANNEL
        //                    ActiveIRCWindow.PutMessage(string.Format(Resources.UserOnChannelMessage, param[1], param[2]), Settings.Default.Colors[4]);
        //                    break;

        //                //case "444": // ERR_NOLOGIN

        //                //case "445": // ERR_SUMMONDISABLED

        //                case "446": // ERR_USERSDISABLED
        //                    PutMessage(Resources.UsersDisabledMessage, Settings.Default.Colors[4]);
        //                    break;

        //                case "451": // ERR_NOTREGISTERED
        //                    PutMessage(param[0] + Resources.NotRegisteredMessage, Settings.Default.Colors[4]);
        //                    break;

        //                case "458":
        //                    ActiveIRCWindow.PutMessage(string.Format(Resources.NotInAcceptList, param[1]), settings.Colors[4]);
        //                    break;

        //                case "461": // ERR_NEEDMOREPARAMS
        //                    PutMessage(string.Format(Resources.NeedMoreParamsMessage, param[1]), settings.Colors[4]);
        //                    break;

        //                case "462": // ERR_ALREADYREGISTRED
        //                    PutMessage(Resources.AlreadyRegisteredMessage, settings.Colors[4]);
        //                    break;

        //                //case "463": // ERR_NOPERMFORHOST

        //                //case "464": // ERR_PASSWDMISMATCH

        //                case "465": // ERR_YOUREBANNEDCREEP
        //                    PutMessage(string.Format("{0}: {1}", Resources.CannotJoin, tail), settings.Colors[4]);
        //                    break;

        //                //case "467": // ERR_KEYSET

        //                case "468": // ERR_ONLYSERVERSCANCHANGE
        //                    ActiveIRCWindow.PutMessage(string.Format(Resources.Message468, param[1]), settings.Colors[4]);
        //                    break;

        //                //case "471": // ERR_CHANNELISFULL

        //                case "472": // ERR_UNKNOWNMODE
        //                    ActiveIRCWindow.PutMessage(string.Format(Resources.UnknownModeMessage, param[1]), settings.Colors[4]);
        //                    break;

        //                case "473": // ERR_INVITEONLYCHAN
        //                    ActiveIRCWindow.PutMessage(string.Format(Resources.InviteOnly, param[1]), Settings.Default.Colors[4]);
        //                    break;

        //                case "474": // ERR_BANNEDFROMCHAN
        //                    ActiveIRCWindow.PutMessage(string.Format(Resources.BannedFormChannelMessage, param[1]), Settings.Default.Colors[4]);
        //                    break;

        //                case "475": // ERR_BADCHANNELKEY
        //                    using (var diag = new StringEnterDialog(Resources.PasswordNeeded))
        //                    {
        //                        if (diag.ShowDialog() == DialogResult.OK && diag.PrintedText.Length > 0)
        //                            JoinChannel(param[1], diag.PrintedText);
        //                    }
        //                    break;

        //                case "476": // ERR_BADCHANMASK
        //                    PutMessage(string.Format(Resources.BadChanMaskMessage, param[1]), settings.Colors[4]);
        //                    break;

        //                case "477": // ERR_NEEDREGGEDNICK
        //                    ActiveIRCWindow.PutMessage(message.Tail, Settings.Default.Colors[5], false);
        //                    break;

        //                //case "478": // ERR_BANLISTFULL

        //                case "479": // ERR_BADCHANNAME
        //                    ActiveIRCWindow.PutMessage(Resources.IncorrectCharactersInChannelName, Settings.Default.Colors[4]);
        //                    break;

        //                case "481": // ERR_NOPRIVILEGES
        //                    ActiveIRCWindow.PutMessage(Resources.NoPrivileges, Settings.Default.Colors[4]);
        //                    break;

        //                case "482": // ERR_CHANOPRIVSNEEDED
        //                    ActiveIRCWindow.PutMessage(Resources.ChannelOpPrivsNeeded, settings.Colors[4]);
        //                    break;

        //                case "483": // ERR_CANTKILLSERVER
        //                    PutMessage(Resources.CannotKillServer, settings.Colors[4]);
        //                    break;

        //                //case "485": // ERR_CHANBANREASON

        //                //case "486": // ERR_NONONREG

        //                //case "487": // ERR_MSGSERVICES

        //                case "491": // ERR_NOOPERHOST
        //                    ActiveIRCWindow.PutMessage(message.Tail, settings.Colors[4], true);
        //                    break;

        //                //case "494": // ERR_OWNMODE

        //                //case "501": // ERR_UMODEUNKNOWNFLAG

        //                case "502": // ERR_USERSDONTMATCH
        //                    PutMessage(Resources.UserDontMatchMessage, settings.Colors[4]);
        //                    break;

        //                //case "511": // ERR_SILELISTFULL

        //                //case "512": // ERR_TOOMANYWATCH

        //                //case "514": // ERR_TOOMANYDCC

        //                //case "521": // ERR_LISTSYNTAX

        //                //case "522": // ERR_WHOSYNTAX

        //                //case "523": // ERR_WHOLIMEXCEED

        //                case "524":
        //                    PutMessage(String.Format("{0}: {1}", param[1], Resources.HelpNotFoundMessage), settings.Colors[4]);
        //                    break;

        //                //case "600": // RPL_LOGON

        //                //case "601": // RPL_LOGOFF

        //                //case "602": // RPL_WATCHOFF

        //                //case "603": // RPL_WATCHSTAT

        //                //case "604": // RPL_NOWON

        //                //case "605": // RPL_NOWOFF

        //                //case "606": // RPL_WATCHLIST

        //                //case "607": // RPL_ENDOFWATCHLIST

        //                //case "617": // RPL_DCCSTATUS

        //                //case "618": // RPL_DCCLIST

        //                //case "619": // RPL_ENDOFDCCLIST

        //                //case "620": // RPL_DCCINFO

        //                case "634":
        //                    PutMessage(tail, Settings.Default.Colors[5], false);
        //                    break;

        //                case "635":
        //                    PutMessage(tail, Settings.Default.Colors[5], false);
        //                    break;

        //                case "671":
        //                    // TODO: -> WHOIS
        //                    // [14:47:50] :2777.ru 671 Ur-Quan Dmitry :is using a Secure Connection
        //                    PutMessage(string.Format("{0} {1}", param[1], tail), 6, false);
        //                    break;

        //                case "700":
        //                    if (message.Params.Count > 1)
        //                        PutMessage(string.Format(Resources.TranslationSchemeMessage, param[1]), settings.Colors[6], false);
        //                    else
        //                        PutMessage(tail, Settings.Default.Colors[6], false);
        //                    break;

        //                case "703":
        //                    goto case "320";

        //                case "704": // RPL_HELPSTART
        //                    PutMessage(tail, settings.Colors[6], false);
        //                    break;

        //                case "705": // RPL_HELPTXT
        //                    PutMessage(tail, settings.Colors[6], false);
        //                    break;

        //                case "706": // RPL_ENDOFHELP
        //                    PutMessage(Resources.EndOfHelp, settings.Colors[6], false);
        //                    break;

        //                case "713":
        //                    ActiveIRCWindow.PutMessage(string.Format(Resources.KnockOpenMessage, param[1]), settings.Colors[4]);
        //                    break;

        //                case "716":
        //                    ActiveIRCWindow.PutMessage(Resources.InGMode, settings.Colors[6]);
        //                    break;

        //                case "718":
        //                    ActiveIRCWindow.PutMessage(string.Format(Resources.SendingToG, param[1]), settings.Colors[4]);
        //                    break;

        //                default:
        //                    PutMessage(line);
        //                    break;
        //            }
        //        }
        //        catch (Exception exc)
        //        {
        //            PutMessage(string.Format(Resources.ErrorInMessage, String.Format("{0}: {1}", line, exc.Message)), Settings.Default.Colors[4], true);
        //        }
        //    }
        //}

        //private void ChangeNick(string oldNick, string newNick)
        //{
        //    if (IsMyNick(oldNick))
        //    {
        //        PutMessage(String.Format("{0} {1}", Resources.NowYourNickIs, newNick), settings.Colors[3]);
        //        connection.Nick.Nick = newNick;
        //        /*myNode*/
        //        this.Text = String.Format("{0} [{1}]", string.IsNullOrEmpty(connection.Server.Description) ? connection.Server.Name : connection.Server.Description, connection.Nick.Nick);
        //    }
        //    foreach (var item in channelForms.Values)
        //    {
        //        if (item.HasPerson(oldNick))
        //        {
        //            item.RenamePerson(oldNick, newNick);
        //            item.PutMessage(string.Format(Resources.ChangeNickTo, oldNick, newNick), 3, true);
        //        }
        //    }

        //    if (channelForms.ContainsKey(oldNick.ToLower()))
        //    {
        //        channelForms[newNick.ToLower()] = channelForms[oldNick.ToLower()];
        //        channelForms.Remove(oldNick.ToLower());
        //    }
        //    lock (whois)
        //    {
        //        if (whois.ContainsKey(oldNick))
        //        {
        //            whois[newNick] = whois[oldNick];
        //            whois.Remove(oldNick);
        //        }
        //    }
        //    if (PersonRenames != null)
        //        PersonRenames(this, new TwoPersonsEventArgs(oldNick, newNick));
        //}

        private void OnSetMode(string name, string mode)
        {
            throw new NotImplementedException();
        }

        private void OnSelfKicked(string channel, string kicker, string words = null)
        {
            throw new NotImplementedException();
        }

        private void OnInvite(string inviter, string channel)
        {
            throw new NotImplementedException();
        }

        private void OnError(string error)
        {
            throw new NotImplementedException();
        }

        #region Члены IServer

        public ConnectionInfo Info
        {
            get { return this.info; }
        }

        public IChangeable<IChannel> Channels
        {
            get { return this.channels; }
        }

        public IChangeable<IPrivateSession> Privates
        {
            get { return this.privates; }
        }

        public bool IsConnected
        {
            get { return this.isConnected; }
        }

        public ISession this[string name]
        {
            get 
            {
                foreach (var item in this.channels)
                {
                    if (item.Name == name)
                        return item;
                }

                foreach (var item in this.privates)
                {
                    if (item.Name == name)
                        return item;
                }

                return null;
            }
        }

        public event Func<MessageEventArgs, bool> MessageReceived;

        public bool Connect()
        {
            this.connection.Connect();
            lock (this.connectionSync)
            {
                Monitor.Wait(this.connectionSync, TimeSpan.FromMinutes(1.0));
                return this.isConnected;
            }
        }

        public void Send(string name, string message)
        {
            throw new NotImplementedException();
        }

        public IChannel JoinChannel(string channel)
        {
            return null;
            //if (!channel.StartsWith('#') && !channel.StartsWith('&'))
            //    channel = "#" + channel;

            //foreach (var item in this.channels)
            //{
            //    if (item.Name == channel)
            //        return;
            //}

            //var channel = new Channel(channel);
            //this.channels.Add(channel);
            //return channel;
        }

        public IPrivateSession OpenPrivate(string name)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Члены IDisposable

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
        
        public void SendMessage(string receiver, string message)
        {
            throw new NotImplementedException();
        }
    }
}
