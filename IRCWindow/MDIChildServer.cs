using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IRCProviders;
using IRCConnection;
using IRCWindow.Properties;
using System.Reflection;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.Diagnostics;
using CIRCe.Base;
using IRCWindow.ViewModel;
using System.Threading.Tasks;
using IRC.Client;
using System.Security.Cryptography;
using IRCWindow.Data;

namespace IRCWindow
{
    /// <summary>
    /// Базовое окно для взаимодействия с сервером
    /// </summary>
    internal partial class MDIChildServer : MDIChild
    {
        private IRC.Client.Connection connection = null;
        private bool fullConnect = false;
        /// <summary>
        /// Дочерние окна для данного
        /// </summary>
        private Dictionary<string, MDIChildCommunication> channelForms = new Dictionary<string, MDIChildCommunication>();
        private string cache = string.Empty;
        private List<string> notPrintWhois = new List<string>();
        private bool motd = false;
        private DateTime lastCmdTime = DateTime.Now;

        private string ghostNick = null;
        private DateTime lastMessageReceived = DateTime.Now;
        private int numOfTries = 0;

        private List<ChannelInfo> channelInfo = new List<ChannelInfo>();
        /// <summary>
        /// Таблица, в которой хранятся времена отправки пингов другим сторонам.
        /// Впоследствии эти времена используются для подсчёта общего времени возврата сообщения
        /// </summary>
        private List<PingInfo> pingTimeTable = new List<PingInfo>();
        private DataTable linksData = new DataTable();

        public override MDIChildServer ServerWindow
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Состоялось ли подключение в полном объёме
        /// </summary>
        public bool FullConnect
        {
            get { return fullConnect; }
        }

        /// <summary>
        /// Сервер, с которым происходит взаимодействие
        /// </summary>
        public ExtendedServerInfo MyServer
        {
            get
            {
                return this.connectionInfo.Server;
            }
        }

        /// <summary>
        /// Пользователь, подключившийся к серверу
        /// </summary>
        public string MyUser
        {
            get
            {
                return this.connectionInfo.Nick;
            }
        }

        public IRC.Client.Base.UserInfo User
        {
            get { return this.connectionInfo.User; }
        }

        public ExtendedConnectionInfo ConnectionInfo
        {
            get { return this.connectionInfo; }
        }

        public event Action<MDIChild> Break;

        public delegate void String2Del(string text, out string result);
        public delegate string PlayCommandDelegate(params string[] args);

        public event Func<string, int, bool, ulong, MDIChild, string> PlayCommand;
        public event Func<int, ulong, string> SeekCommand;
        public event Func<int, string> StopCommand;
        public event Func<int, string> PauseCommand;
        public event Func<int, string> ResumeCommand;

        public event EventHandler<PersonEventArgs> WhoisUpdated;

        internal static Dictionary<char, char> ModesTable2 = new Dictionary<char, char>();

        static MDIChildServer()
        {
            ModesTable2['o'] = '@';
            ModesTable2['v'] = '+';
            ModesTable2['h'] = '%';
            ModesTable2['a'] = '&';
            ModesTable2['b'] = '~';
        }

        private ExtendedConnectionInfo connectionInfo;

        /// <summary>
        /// Создание окна для взаимодествия с IRC-сервером
        /// </summary>
        /// <param name="user"></param>
        /// <param name="server"></param>
        internal MDIChildServer(MDIParent main, ExtendedConnectionInfo connectionInfo)
            : base(main)
        {
            InitializeComponent();

            var server = connectionInfo.Server;

            this.Size = UISettings.Default.IRCWindowSize;
            this.Text = string.Format("{0} [{1}]", string.IsNullOrEmpty(server.Description) ? server.Name : server.Description, connectionInfo.Nick);

            this.connectionInfo = connectionInfo;
            this.connection = new Connection(connectionInfo.Data, Settings.Default.UseProxy);
                //new Connection(new ConnectionInfo(server, new NickName(nick)), UISettings.Default.QuitMessage, Settings.Default.UseProxy); 
            connection.MessageReceived += connection_MessageReceived;

            mainAreaSplitContainer.SplitterDistance = mainAreaSplitContainer.Width;
            mainAreaSplitContainer.Panel2Collapsed = true;
            topicPanel.Visible = false;

            this.irtbPrintMessage.Font = new Font("Calibri", 11.25f);

            while (this.cmsServer.Items.Count > 0)
            {
                this.cmsServer.Items[0].Visible = false;
                this.ContextMenuStrip.Items.Add(this.cmsServer.Items[0]);
            }

            this.tsmiReconnect.Visible = true;
            this.chatRTB.ContextMenuStrip = this.ContextMenuStrip;

            whois = new Dictionary<string, UserInfo>();

            this.tSMIChannels.Visible = false;

            this.linksData.Columns.Add("server");
            this.linksData.Columns.Add("mainServer");
            this.linksData.Columns.Add("hops");
            this.linksData.Columns.Add("title");

            this.buttonPanel.Dock = DockStyle.Fill;

            this.tsmiStick.Checked = this.Server.Sticked;
            this.tsmiAutoOpen.Checked = this.Server.AutoOpen;

            this.tsmiStick.CheckedChanged += (sender, e) =>
                {
                    this.Server.Sticked = this.tsmiStick.Checked;
                    if (!this.Server.Sticked)
                        this.Server.Channels.ForEach(ch => ch.Sticked = false);
                };

            this.tsmiAutoOpen.CheckedChanged += (sender, e) => this.Server.AutoOpen = this.tsmiAutoOpen.Checked;

            this.Server.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "Sticked")
                    {
                        this.tsmiStick.Checked = this.Server.Sticked;
                        this.myNode.Sticked = this.Server.Sticked;
                        if (this.Server.Sticked)
                            this.myNode.Tag = this.connectionInfo.Server;
                    }
                };
        }

        /// <summary>
        /// Подключение состоялось
        /// </summary>
        /// <param name="result"></param>
        void ConnectionFinished(IAsyncResult result)
        {
            if (connection.Connected)
                connection.Disposed += connection_Disposed;
            else
            {
                Thread.Sleep(5000);
                // Попытаемся восстановить соединение
                if (numOfTries < 10)
                {
                    numOfTries++;
                    this.Connect();
                }
            }
        }

        /// <summary>
        /// Закрытие серверного окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MDIChildServer_FormClosed(object sender, FormClosedEventArgs e)
        {
            var children = new MDIChildCommunication[channelForms.Values.Count];

            int i = 0;

            foreach (var item in channelForms.Values)
            {
                children[i++] = item;
            }

            Array.ForEach(children, item => { item.Quit(); item.Close(); });

            try
            {
                connection.RunCmd("QUIT", ":" + UISettings.Default.QuitMessage);
            }
            catch (System.Net.Sockets.SocketException)
            {
                // Соединение разорвано, ничего не поделаешь
            }
            catch (IOException) { }
            catch (InvalidOperationException) { }

            connection.Disposed -= connection_Disposed;
            connection.Dispose();

            if (this.MyNode.Sticked)
            {
                this.MyNode.Text = string.IsNullOrEmpty(this.Server.Description) ? this.Server.Name : this.Server.Description;
                this.MyNode.ForeColor = Color.PaleVioletRed;
            }

            if (Break != null)
                Break(this);
        }

        /// <summary>
        /// Получить окно по имени
        /// </summary>
        /// <param name="name">Имя окна</param>
        /// <returns>Найденное или созданное окно</returns>
        private MDIChildCommunication ChildByName(string name)
        {
            MDIChildCommunication result = null;
            if (channelForms.TryGetValue(name.ToLower(), out result))
                return result;

            return (MDIChildCommunication)OpenWindow(name);
        }

        /// <summary>
        /// Открыть новое окно
        /// </summary>
        /// <param name="name">Имя окна</param>
        /// <returns>Созданное окно</returns>
        public MDIChildCommunication OpenWindow(string name)
        {
            bool isChannel = name.Length > 0 && (name[0] == '#' || name[0] == '&');
            MDIChildCommunication childForm = null;

            if (isChannel)
            {
                var chan = this.Server.Channels.Find(channel => channel.Name == name);
                if (chan == null)
                {
                    chan = new ExtendedChannelInfo { Name = name };
                    this.Server.Channels.Add(chan);
                }

                childForm = new MDIChildChannel(main, this, new Channel(chan.Name), whois);
                var circeChannel = new CIRCeChannel((MDIChildChannel)childForm);
                childForm.DataContext = circeChannel;
                ((MDIChildChannel)childForm).OpenPrivate += childForm_OpenPrivate;

                ((Changeable<ICIRCeChannel>)((CIRCeServer)this.DataContext).Channels).Add(circeChannel);

                var link = string.Format("irc://{0}:{1}/{2}", this.Server.Name, this.Server.Port, name);
                var jumpTask = new System.Windows.Shell.JumpTask
                {
                    Arguments = link,
                    Title = name,
                    Description = link
                };
                System.Windows.Shell.JumpList.AddToRecentCategory(jumpTask);
            }
            else if (name.Length > 0)
            {
                childForm = new MDIChildPrivate(main, this, name, whois);
                childForm.DataContext = new CIRCePrivateSession((MDIChildPrivate)childForm);
            }
            else
                return null;

            childForm.Cmd += childForm_Cmd;
            childForm.Disposed += childForm_Disposed;
            channelForms[name.ToLower()] = childForm;

            if (this.NewWindow != null)
                this.NewWindow(this, new JoinEventArgs(childForm));

            return childForm;
        }

        public void Say(string person, string text, bool silent = false)
        {
            var msgColl = text.Split(new string[] { Special.StringSeparator }, StringSplitOptions.RemoveEmptyEntries);

            Array.ForEach(msgColl, msg =>
            {
                connection.RunCmd("PRIVMSG", person, String.Format(":{0}", msg));

                if (!silent)
                    ChildByName(person).InvokeReceive(this.connectionInfo.Nick, msg);
            });
        }

        public void Notice(string person, string text)
        {
            connection.RunCmd("NOTICE", person, String.Format(":{0}", text));
        }

        /// <summary>
        /// Вызвать Away-сообщение
        /// </summary>
        /// <param name="message"></param>
        public void Away(string message)
        {
            if (message.Length > 0)
                connection.RunCmd("AWAY", String.Format(":{0}", message));
            else
                connection.RunCmd("AWAY");
        }

        void timer1_Tick(object sender, EventArgs e)
        {
            lock (whoisLock)
            {
                foreach (var item in whois.Values)
                {
                    if (item != null && item.Obsolete > 0)
                        item.Obsolete--;
                }
            }

            if (UISettings.Default.WaitServer > 0 && UISettings.Default.PingServer > 0)
            {
                var delta = (DateTime.Now - this.lastMessageReceived).Seconds;
                if (delta > UISettings.Default.WaitServer + UISettings.Default.PingServer)
                    connection.Dispose();
                else if (delta > UISettings.Default.WaitServer)
                    connection.RunCmd("PING", this.connectionInfo.Server.Name);
            }
        }

        /// <summary>
        /// Пользователь ввёл сообщение и нажал Enter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void printMessageIrcRichTextBox_EditFinished(object sender, EnterPushedEventArgs e)
        {
            string msg = e.Message.Trim();
            if (msg.Length == 0)
                return;

            if (msg[0] == Special.CmdStarter)
                Execute(null, msg.Substring(1).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            else
                PutMessage(String.Format("{0}!", Resources.YouAreNotOnAChannel), settings.Colors[2]);

            this.irtbPrintMessage.Clear();
            lastCmdTime = DateTime.Now;
        }

        internal event Action<string> DownloadFinished;

        private void ExecuteBack(string windowName, string[] cmd)
        {
            MDIChild executor = null;
            if (windowName != null)
            {
                if (!this.channelForms.ContainsKey(windowName))
                    return;

                executor = ChildByName(windowName);
            }
            else
                executor = this;

            bool url = false;

            switch (cmd[0].ToLower())
            {
                case "echo":
                    string msg = string.Join(" ", cmd);
                    executor.Echo(msg.Substring(5));
                    return;

                case "url":
                    {
                        if (cmd.Length < 2)
                        {
                            break;
                        }
                        try
                        {
                            var text = cmd[1];
                            var formatted = new IRC.Client.Base.FormattedText(text, 1, 0);
                            var str = formatted.Text.Trim();
                            if (str.Length == 0)
                                break;

                            var uri = new Uri(str.Split(' ')[0], UriKind.Absolute);

                            using (var client = new WebClient())
                            {
                                client.Headers.Add(HttpRequestHeader.UserAgent, Program.UserAgentHeader);
                                // Important for some sites
                                var fileName = Program.LocalDataStorage.GetDirectoryInfo("Cache").FullName;
                                var md5 = MD5.Create();
                                var invalid = Path.GetInvalidFileNameChars();
                                
                                var hash = Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(Path.GetDirectoryName(uri.AbsoluteUri)))).Trim(invalid);
                                var goodName = Path.GetFileName(uri.AbsolutePath);

                                foreach (var item in invalid)
                                {
                                    goodName = goodName.Replace(item.ToString(), "_");
                                    hash = hash.Replace(item.ToString(), "_");
                                }

                                fileName = Path.Combine(fileName, hash);
                                Directory.CreateDirectory(fileName);
                                fileName = Path.Combine(fileName, goodName);

                                //if (File.Exists(fileName))
                                //{
                                //    if (StopCommand != null)
                                //    {
                                //        StopCommand(2);
                                //        client.DownloadFile(uri, fileName);
                                //    }
                                //}
                                //else
                                if (!File.Exists(fileName))
                                {
                                    client.DownloadFile(uri, fileName);
                                }
                                cmd[1] = fileName;
                            }

                            if (DownloadFinished != null)
                                DownloadFinished(uri.AbsoluteUri);

                            url = true;
                            goto case "splay";
                        }
                        catch (Exception exc)
                        {
                            PutMessage(exc.Message, 4, true);
                        }
                        return;
                    }

                case "list":
                    channelInfo.Clear();
                    break;

                case "me":
                    var newCmd = new List<string>();
                    newCmd.Add("PRIVMSG");
                    newCmd.Add(windowName);
                    var command = new StringBuilder();
                    command.Append(":" + Special.Ctcp + "ACTION ");
                    var content = new StringBuilder();
                    for (int i = 1; i < cmd.Length; i++)
                    {
                        if (content.Length > 2)
                            content.Append(' ');
                        content.Append(cmd[i]);
                    }
                    command.Append(content.ToString());
                    command.Append(Special.Ctcp);
                    newCmd.Add(command.ToString());
                    connection.RunCmd(newCmd.ToArray());
                    ChildByName(windowName).PutMessage(String.Format("* {0} {1}", this.connectionInfo.Nick, content), Settings.Default.Colors[6]);
                    return;

                case "msg":
                    if (cmd.Length < 2)
                    {
                        Echo(String.Format("msg: {0}", Resources.UnsufficientParams));
                    }
                    else
                    {
                        cmd[0] = "PRIVMSG";
                        cmd[2] = String.Format(":{0}", cmd[2]);
                    }
                    break;

                case "splay":
                    if (PlayCommand != null)
                    {
                        try
                        {
                            var fileNames = new List<StringBuilder>();
                            int j = 0;
                            fileNames.Add(new StringBuilder());

                            // В именах файлов могут быть пробелы однако
                            for (int i = 1; i < cmd.Length; i++)
                            {
                                if (fileNames[j].Length > 0)
                                    fileNames[j].Append(' ');

                                fileNames[j].Append(cmd[i]);

                                if (cmd[i].Contains("."))
                                {
                                    j++;
                                    fileNames.Add(new StringBuilder());
                                }
                            }

                            if (fileNames[j].Length == 0)
                                fileNames.RemoveAt(j);

                            int numOfMediaPlayer = 0;
                            string result = string.Empty;
                            if (fileNames.Count == 0 && StopCommand != null)
                                result = StopCommand(numOfMediaPlayer);

                            foreach (StringBuilder fileName in fileNames)
                            {
                                if (fileName.Length == 0)
                                    continue;
                                
                                result = string.Empty;

                                if (url)
                                    numOfMediaPlayer = 2;
                                else if (Path.GetExtension(fileName.ToString()) == ".wav")
                                    numOfMediaPlayer = 1;
                                else
                                    numOfMediaPlayer = 0;

                                result = PlayCommand(fileName.ToString(), numOfMediaPlayer, false, 0, executor);
                                if (result.Length > 0)
                                    PutMessage(result, settings.Colors[4], false);
                            }
                        }
                        catch (Exception exc)
                        {
                            PutMessage(exc.Message, 4, false);
                        }
                    }
                    return;

                case "media": // Управление мультимедиа
                    if (cmd.Length < 2)
                        break;

                    switch (cmd[1].ToLower())
                    {
                        case "loop":
                            if (PlayCommand != null && cmd.Length > 2)
                            {
                                var result = PlayCommand(cmd[2], Path.GetExtension(cmd[2]) == ".wav" ? 1 : 0, true, 0, executor);
                                if (result.Length > 0)
                                    PutMessage(result, settings.Colors[4], false);
                            }
                            break;

                        case "pause":
                            if (PauseCommand != null)
                            {
                                var result = PauseCommand(0);
                                if (result.Length > 0)
                                    PutMessage(result, settings.Colors[4], false);
                            }
                            break;

                        case "pos":
                            if (PlayCommand != null && cmd.Length > 3)
                            {
                                ulong pos = 0;
                                ulong.TryParse(cmd[3], out pos);

                                var result = PlayCommand(cmd[2], Path.GetExtension(cmd[2]) == ".wav" ? 1 : 0, false, pos, executor);
                                if (result.Length > 0)
                                    PutMessage(result, settings.Colors[4], false);
                            }
                            break;

                        case "resume":
                            if (ResumeCommand != null)
                            {
                                var result = ResumeCommand(0);
                                if (result.Length > 0)
                                    PutMessage(result, settings.Colors[4], false);
                            }
                            break;

                        case "seek":
                            if (cmd.Length > 2 && SeekCommand != null)
                            {
                                ulong pos = 0;
                                ulong.TryParse(cmd[2], out pos);

                                var result = SeekCommand(0, pos);
                                if (result.Length > 0)
                                    PutMessage(result, settings.Colors[4], false);
                            }
                            break;

                        case "stop":
                            if (cmd.Length > 2 && StopCommand != null)
                            {
                                int numOfPlayer = 0;
                                int.TryParse(cmd[2], out numOfPlayer);

                                var result = StopCommand(numOfPlayer);
                                if (result.Length > 0)
                                    PutMessage(result, settings.Colors[4], false);
                            }
                            break;

                        default:
                            break;
                    }

                    return;

                default:
                    break;
            }

            if (UISettings.Default.SavePasswords && cmd.Length == 4 && cmd[0] == "PRIVMSG" && cmd[1] == "NickServ" && cmd[2] == ":IDENTIFY")
            {
                var pass = cmd[3];
                this.Server.Passwords.Data[this.Nick] = pass;
            }

            connection.RunCmd(cmd);
        }

        /// <summary>
        /// Выполнить команду
        /// </summary>
        /// <param name="cmd">Команда</param>
        public void Execute(string windowName, params string[] cmd)
        {
            Task.Factory.StartNew(() => ExecuteBack(windowName, cmd));
        }

        #region Connection event handlers

        void connection_MessageReceived(string m)
        {
            if (InvokeRequired)
            {
                Action<string> r = connection_MessageReceived;
                Invoke(r, m);
            }
            else
            {
                this.lastMessageReceived = DateTime.Now;
                MDIChildCommunication child = null;

                // Кэширование сообщений
                if (cache.Length > 0)
                {
                    m = cache + m;
                    cache = "";
                }

                var lines = m.Split(new string[] { Special.StringSeparator }, StringSplitOptions.RemoveEmptyEntries);

                if (!m.EndsWith(Special.StringSeparator))
                {
                    cache = lines[lines.Length - 1];
                    lines[lines.Length - 1] = "";
                }

                // Здесь обрабатываем спецсообщения
                // http://irc.tm-net.ru/files/rfc1459-rus.php
                // http://svn.hydrairc.com/hydrairc/trunk/Reference%20Docs/irc-numerics-conflicts.htm
                foreach (var line in lines)
                {
                    if (line.Length == 0)
                        continue;

                    var message = new IRCMessage(line);
                    if (IRCMessageReceived != null)
                    {
                        var args = new IRCMessageEventArgs(message);
                        //try
                        //{
                            IRCMessageReceived(this, args);
                        //}
                        //catch (AppDomainUnloadedException)
                        //{
                        //}
                        if (args.Cancel)
                            continue;
                    }

                    var tail = message.Tail;
                    var param = message.Param.ToArray();
                    var prefix = message.Prefix;
                    var name = message.Name;
                    try
                    {
                        switch (message.Command)
                        {
                            case "ERROR":
                                PutMessage(string.Format(Resources.ErrorMessage, message.Tail), Settings.Default.Colors[4], true);
                                break;

                            case "INVITE":
                                PutMessage(string.Format(Resources.InviteMessage, message.Name, tail), settings.Colors[2]);
                                break;

                            case "JOIN":
                                #region JOIN
                                if (IsMyNick(message.Name))
                                {
                                    var channelList = string.IsNullOrEmpty(message.Tail) ? message.Param.ToArray() : message.Tail.Split(',');
                                    Array.ForEach(channelList, channel => ChildByName(channel));
                                }
                                else
                                {
                                    var channelName = tail.Length > 0 ? tail : param[0];
                                    child = ChildByName(channelName);
                                    ((MDIChildChannel)child).AddPerson(message.Name, true);
                                    child.PutMessage(string.Format(Resources.JoinMessage, message.Name), settings.Colors[3], true, MessageType.UserEvent);
                                }
                                break;
                                #endregion

                            case "KICK":
                                #region KICK
                                child = ChildByName(param[0]);

                                if (IsMyNick(param[1]))
                                {
                                    child.Cmd -= childForm_Cmd;
                                    child.Close();

                                    var reply = new StringBuilder();
                                    reply.AppendFormat(Resources.YouWasKicked, param[0], name);

                                    if (name != tail)
                                        reply.Append(' ').AppendFormat(Resources.WithWords, tail);

                                    PutMessage(reply.ToString(), Settings.Default.Colors[4]);
                                }
                                else
                                {
                                    child.PutMessage(String.Format("{0} {1} {2} ({3})", message.Name, Resources.KicksUserFormChannel, param[1], tail), settings.Colors[3]);
                                    ((MDIChildChannel)child).KickPerson(param[1]);
                                }
                                break;
                                #endregion

                            case "MODE":
                                #region MODE
                                if (param[0][0] == '#')
                                {
                                    if (param.Length == 2)
                                        ((MDIChildChannel)ChildByName(param[0])).Mode(message.Name, param[1]);
                                    else
                                    {
                                        string[] persons = new string[param.Length - 2];
                                        for (int i = 2; i < param.Length; i++)
                                        {
                                            persons[i - 2] = param[i];
                                        }
                                        
                                        ((MDIChildChannel)ChildByName(param[0])).Mode(message.Name, param[1], persons);
                                    }
                                }
                                else
                                {
                                    var mode = tail;
                                    if (param.Length > 1)
                                        mode = param[1];
                                    PutMessage(String.Format("{0}: {1} {2}", name, Resources.ModeMessage, mode), settings.Colors[3]);
                                }

                                break;
                                #endregion

                            case "NICK":
                                ChangeNick(message.Name, tail);
                                break;

                            case "NOTICE":
                                #region NOTICE
                                if (tail[0] == Special.Ctcp)
                                {
                                    int next = tail.IndexOf(Special.Ctcp, 1);
                                    string ctcp = next > 1 ? tail.Substring(1, next - 1) : tail.Substring(1);
                                    string[] ctcpParams = ctcp.Split(' ');

                                    var childForm = (MDIChild)this.ActiveIRCWindow;
                                    var mode = childForm.Logging;
                                    childForm.Logging = LogMode.None;
                                    switch (ctcpParams[0])
                                    {
                                        case "PING":
                                            var pingInfo = ctcpParams.Length > 1 ?
                                                pingTimeTable.Find(info => info.Server == name && info.Pid.ToString() == ctcpParams[1])
                                                : pingTimeTable.Find(info => info.Server == name);
                                            if (pingInfo != null)
                                            {
                                                var ping = DateTime.Now - pingInfo.SentTime;
                                                childForm.PutMessage(string.Format(Resources.PingAnswer, name, ping), settings.Colors[6], false);
                                                pingTimeTable.Remove(pingInfo);
                                            }
                                            break;

                                        case "VERSION":
                                            childForm.PutMessage(string.Format(Resources.VersionAnswer, name, ctcp.Substring(8)), settings.Colors[6], false);
                                            break;

                                        case "USERINFO":
                                            childForm.PutMessage(string.Format(Resources.UserInfoAnswer, name, ctcp.Substring(9)), settings.Colors[6], false);
                                            break;

                                        //case "CLIENTINFO":
                                        //    break;

                                        //case "FINGER":                                        
                                        //    break;

                                        //case "SOURCE":
                                        //    break;

                                        case "TIME":
                                            childForm.PutMessage(string.Format(Resources.TimeAnswer, name, ctcp.Substring(5)), settings.Colors[6], false);
                                            break;

                                        default:
                                            childForm.PutMessage(string.Format("-{0}- {1}", prefix, tail), settings.Colors[6], false);
                                            break;
                                    }
                                    childForm.Logging = mode;
                                }
                                else
                                {
                                    if (message.Host.Length > 0)
                                    {
                                        var active = this.ActiveIRCWindow;
                                        active.PutMessage(string.Format("<{0}> {1}", name, tail), settings.Colors[5], true);
                                    }
                                    else
                                        PutMessage(tail, settings.Colors[5], false);

                                    if (tail.Contains("/msg NickServ IDENTIFY"))
                                    {
                                        string password;
                                        if (this.Server.Passwords.Data.TryGetValue(this.Nick, out password))
                                        {
                                            connection.RunCmd("PRIVMSG", "NickServ", ":IDENTIFY", password);
                                        }
                                        else if (UISettings.Default.PutRegisterMessage)
                                        {
                                            this.irtbPrintMessage.Clear();
                                            this.irtbPrintMessage.SetText("/msg NickServ IDENTIFY ");
                                            this.irtbPrintMessage.Select(this.irtbPrintMessage.Text.Length, 0);
                                            this.irtbPrintMessage.SelectionColor = this.irtbPrintMessage.BackColor;
                                        }
                                    }
                                }
                                break;
                                #endregion

                            case "PART":
                                #region PART
                                if (name != this.connectionInfo.Nick)
                                {
                                    child = ChildByName(param[0]);
                                    child.RemovePerson(name, true);
                                    child.PutMessage(string.Format("* {0} {1}", name, Resources.LeavesChannel), settings.Colors[3], true, MessageType.UserEvent);
                                }
                                else
                                {
                                    if (channelForms.ContainsKey(param[0]))
                                        ChildByName(param[0]).Close();
                                }
                                break;
                                #endregion

                            case "PING":
                                connection.RunCmd(line.Replace("PING", "PONG"));
                                break;

                            case "PONG":
                                {
                                    var pingInfo = tail.Length > 0 ?
                                                    pingTimeTable.Find(info => info.Server == name && info.Pid.ToString() == tail)
                                                    : pingTimeTable.Find(info => info.Server == name);

                                    if (pingInfo != null)
                                    {
                                        var ping = DateTime.Now - pingInfo.SentTime;
                                        PutMessage(string.Format(Resources.PingAnswer, name, ping), settings.Colors[6], false);
                                        pingTimeTable.Remove(pingInfo);
                                    }
                                }
                                break;

                            case "PRIVMSG":
                                #region PRIVMSG
                                if (tail[0] == Special.Ctcp)
                                {
                                    int next = tail.IndexOf(Special.Ctcp, 1);
                                    string ctcp = next > 1 ? tail.Substring(1, next - 1) : tail.Substring(1);
                                    string[] ctcpParams = ctcp.Split(' ');

                                    switch (ctcpParams[0])
                                    {
                                        case "PING":
                                            Notice(name, Special.Ctcp + ctcp + Special.Ctcp);
                                            break;

                                        case "VERSION":
                                            AssemblyName program = Assembly.GetExecutingAssembly().GetName();
                                            Notice(name, Special.Ctcp + "VERSION " + program.Name + " " + program.Version + " by Vladimir Khil" + Special.Ctcp);
                                            break;

                                        case "USERINFO":
                                            string info = Settings.Default.UserInfoString;
                                            Notice(name, Special.Ctcp + "USERINFO " + (info.Length > 0 ? info : this.connectionInfo.User.ToString()) + Special.Ctcp);
                                            break;

                                        case "CLIENTINFO":
                                            Notice(name, Special.Ctcp + "CLIENTINFO " + Assembly.GetExecutingAssembly().GetName().Name + " supplies CTCP: PING, VERSION, USERINFO, CLIENTINFO, FINGER, SOURCE, TIME, ACTION"/*, AVATAR, MULTIMEDIA, DCC"*/ + Special.Ctcp);
                                            break;

                                        case "FINGER":
                                            Notice(name, Special.Ctcp + "FINGER " +
                                                Resources.RealName + " " + this.connectionInfo.User.RealName +
                                                " " + Resources.Email + " " + this.connectionInfo.User.EMail +
                                                " " + Resources.IdleTime + " " + (DateTime.Now - lastCmdTime).ToString() +
                                                Special.Ctcp);
                                            break;

                                        case "SOURCE":
                                            Notice(name, Special.Ctcp + "SOURCE http://ur-quan1986.narod.ru" + Special.Ctcp);
                                            break;

                                        case "TIME":
                                            Notice(name, Special.Ctcp + "TIME " + DateTime.Now.ToString() + Special.Ctcp);
                                            break;

                                        case "ACTION": // by NOTICE only
                                            ChildByName(param[0] != this.connectionInfo.Nick ? param[0] : name).PutMessage(String.Format("* {0} {1}", name, ctcp.Substring(7)), Settings.Default.Colors[6]);
                                            return;

                                        /*case "AVATAR":
                                            break;

                                        case "MULTIMEDIA":
                                            break;

                                        case "DCC":
                                            break;*/

                                        default:
                                            break;
                                    }

                                    if (UISettings.Default.NotifyOnCtcp)
                                        ActiveIRCWindow.PutMessage(String.Format("{0} [CTCP: {1}]", name, ctcp), settings.Colors[4]);
                                }
                                else
                                {
                                    if (param[0] != this.connectionInfo.Nick)
                                    {
                                        if (!channelForms.TryGetValue(param[0].ToLower(), out child))
                                            break;
                                    }
                                    else
                                        child = ChildByName(name);
                                    child.PutTextMessage(name, tail);
                                }
                                break;
                                #endregion

                            case "QUIT":
                                foreach (var item in channelForms.Values)
                                {
                                    if (item.HasPerson(name))
                                    {
                                        item.RemovePerson(name, true);
                                        item.PutMessage(String.Format("* {0} {1}{2}", name, Resources.QuitsIRC, (tail.Length > 0 ? " (" + tail + ")" : string.Empty)), settings.Colors[2], true, MessageType.UserEvent);
                                    }
                                }
                                break;

                            case "TOPIC": // Ср. case "332"
                                ChildByName(param[0]).PutMessage(string.Format("{0} {1}", name, Resources.SetsNewTopic), Settings.Default.Colors[3]);
                                ((MDIChildChannel)ChildByName(param[0])).Topic = tail;
                                break;

                            case "001": // RPL_WELCOME
                                ChangeNick(this.connectionInfo.Nick, param[0]);
                                PutMessage(tail, settings.Colors.DefForeColor);
                                break;

                            case "002": // RPL_YOURHOST
                                PutMessage(tail, settings.Colors.DefForeColor);
                                break;

                            case "003": // RPL_CREATED
                                PutMessage(tail, settings.Colors.DefForeColor);
                                break;

                            case "004": // RPL_MYINFO
                                var text = new StringBuilder();
                                for (int i = 1; i < param.Length; i++)
                                {
                                    text.Append(param[i]);
                                    text.Append(' ');
                                }
                                PutMessage(text.ToString(), settings.Colors.DefForeColor);
                                break;

                            case "005": // RPL_BOUNCE // RPL_ISUPPORT
                                text = new StringBuilder();
                                for (int i = 1; i < param.Length; i++)
                                {
                                    text.Append(param[i]);
                                    text.Append(' ');
                                }
                                PutMessage(text.ToString(), settings.Colors.DefForeColor);
                                break;

                            //case "200": // RPL_TRACELINK

                            //case "201": // RPL_TRACECONNECTING

                            //case "202": // RPL_TRACEHANDSHAKE

                            //case "203": // RPL_TRACEUNKNOWN

                            //case "204": // RPL_TRACEOPERATOR

                            //case "205": // RPL_TRACEUSER

                            //case "206": // RPL_TRACESERVER

                            //case "208": // RPL_TRACENEWTYPE

                            //case "211": // RPL_STATSLINKINFO

                            case "212": // RPL_STATSCOMMANDS
                                var result = new StringBuilder();
                                for (int i = 1; i < param.Length; i++)
                                {
                                    result.Append(param[i]);
                                    result.Append(' ');
                                }

                                result.AppendFormat("({0})", message.Tail);
                                ActiveIRCWindow.PutMessage(result.ToString(), settings.Colors[5], false);
                                break;

                            //case "213": // RPL_STATSCLINE

                            //case "214": // RPL_STATSNLINE

                            //case "215": // RPL_STATSILINE

                            //case "216": // RPL_STATSKLINE

                            //case "218": // RPL_STATSYLINE

                            case "219": // RPL_ENDOFSTATS
                                ActiveIRCWindow.PutMessage(Resources.EndOfStats, settings.Colors[5], false);
                                break;

                            case "221": // RPL_UMODEIS
                                PutMessage(string.Format("{0}: {1}", Resources.YourModeIs, param[1]), settings.Colors[3]);
                                break;

                            //case "225": // RPL_STATSCLONE

                            //case "226": // RPL_STATSCOUNT

                            //case "227": // RPL_STATSGLINE

                            //case "241": // RPL_STATSLLINE

                            case "242": // RPL_STATSUPTIME
                                ActiveIRCWindow.PutMessage(tail, settings.Colors[5], false);
                                break;

                            //case "243": // RPL_STATSOLINE

                            //case "244": // RPL_STATSHLINE

                            //case "245": // RPL_STATSSLINE

                            //case "246": // RPL_STATSXLINE

                            case "250": // RPL_STATSDLINE
                                PutMessage(tail, settings.Colors[6]);
                                break;

                            case "251": // RPL_LUSERCLIENT
                                #region RPL_LUSERCLIENT
                                // Это сообщение является при любом подключении обязательным
                                // Оно обозначает успешность подключения
                                connection.UsingServer = message.Name;
                                PutMessage("***", settings.Colors[6]);
                                PutMessage(tail, settings.Colors[6]);
                                fullConnect = true;

                                if (OnConnected != null)
                                    OnConnected(this, EventArgs.Empty);

                                this.MyNode.ForeColor = Color.Red;
                                
                                numOfTries = 0;

                                foreach (ToolStripItem item in this.contextMenuStripChat.Items)
                                {
                                    item.Visible = true;
                                }

                                if (!string.Equals(this.ghostNick, null))
                                {
                                    string password;
                                    if (this.Server.Passwords.Data.TryGetValue(this.ghostNick, out password))
                                    {
                                        connection.RunCmd("ns", "GHOST", this.ghostNick, password);
                                        Task.Factory.StartNew(() => DelayedSetNick(this.ghostNick));
                                    }
                                }

                                // Здесь выполняем повторный вход на все ранее окрытые каналы
                                foreach (var item in channelForms.Values)
                                {
                                    var channel = item as MDIChildChannel;
                                    if (channel != null)
                                    {
                                        channel.ClearModes();
                                        JoinChannel(channel.WindowName);
                                    }
                                }

                                this.Server.Channels.FindAll(ch => ch.AutoOpen).ForEach(ch => { if (!channelForms.ContainsKey(ch.Name)) JoinChannel(ch.Name); });

                                break;
                                #endregion

                            case "252": // RPL_LUSEROP
                                PutMessage(String.Format("{0}: {1}", Resources.OpsOnline, param[1]), settings.Colors[6]);
                                break;

                            case "253": // RPL_LUSERUNKNOWN
                                PutMessage(String.Format("{0}: {1}", Resources.TotalUsers, param[1]), settings.Colors[6]);
                                break;

                            case "254": // RPL_LUSERCHANNELS
                                PutMessage(String.Format("{0}: {1}", Resources.TotalChannels, param[1]), settings.Colors[6]);
                                break;

                            case "255": // RPL_LUSERME
                                PutMessage(tail, settings.Colors[6]);
                                PutMessage("***", settings.Colors[6]);
                                break;

                            case "256": // RPL_ADMINME
                                ActiveIRCWindow.PutMessage(String.Format("{0}:", tail), settings.Colors[6], false);
                                break;

                            case "257": // RPL_ADMINLOC1
                                ActiveIRCWindow.PutMessage(tail, settings.Colors[6], false);
                                break;

                            case "258": // RPL_ADMINLOC2
                                ActiveIRCWindow.PutMessage(tail, settings.Colors[6], false);
                                break;

                            case "259": // RPL_ADMINEMAIL
                                ActiveIRCWindow.PutMessage(string.Format("{0}: {1}", Resources.AdminMailMessage, tail), settings.Colors[6], false);
                                break;

                            //case "261": // RPL_TRACELOG

                            case "262":
                                ActiveIRCWindow.PutMessage(Resources.EndOfTrace, settings.Colors[5], false);
                                break;

                            case "263":
                                ActiveIRCWindow.PutMessage(Resources.ServerOverload, settings.Colors[4], false);
                                break;

                            case "265": // RPL_LOCALUSERS
                                PutMessage(tail, settings.Colors[6]);
                                break;

                            case "266": // RPL_GLOBALUSERS
                                PutMessage(tail, settings.Colors[6]);
                                break;

                            //case "271": // RPL_SILELIST

                            //case "272": // RPL_ENDOFSILELIST

                            case "275":
                                lock (whoisLock)
                                {
                                    if (!whois.ContainsKey(param[1]))
                                        whois[param[1]] = new UserInfo();

                                    whois[param[1]].Ssl = tail;
                                    if (!notPrintWhois.Contains(param[1]))
                                    {
                                        ActiveIRCWindow.PutMessage(String.Format("{0}: {1}", param[1], tail), 6);
                                    }
                                }
                                break;

                            case "281": // RPL_ACCEPT
                                ActiveIRCWindow.PutMessage(string.Format(Resources.AcceptList, message.Tail), settings.Colors[6], true);
                                break;

                            case "282": // RPL_ENDOFACCEPTLIST
                                ActiveIRCWindow.PutMessage(Resources.EndOfAcceptList, settings.Colors[6], true);
                                break;

                            case "301": // RPL_AWAY
                                foreach (MDIChildCommunication childForm in channelForms.Values)
                                {
                                    // TODO: перенести в whois
                                    PutMessage(string.Format(Resources.AwayWithWords, param[1], tail), settings.Colors[3]);
                                }
                                break;

                            case "302": // RPL_USERHOST
                                #region RPL_USERHOST
                                MatchCollection matchCollection = Regex.Matches(message.Tail, Settings.Default.RplUserHostRegexPattern);
                                MDIChild form = (MDIChild)this.ActiveIRCWindow;
                                form.PutMessage(Resources.RplUserInfoMessage, Settings.Default.Colors[6], true);
                                foreach (Match match in matchCollection)
                                {
                                    StringBuilder msg = new StringBuilder(match.Groups["nick"].Value);
                                    if (match.Groups["op"].Value.Length > 0)
                                        msg.AppendFormat(" ({0})", Resources.IRCOp);
                                    msg.AppendFormat(" ({0}). ", match.Groups["plus"].Value.Equals("+") ? Resources.IsOnline : Resources.IsOffline);
                                    msg.AppendFormat("{0}: {1}", Resources.Hostname, match.Groups["host"].Value);
                                    form.PutMessage(msg.ToString(), Settings.Default.Colors[6], true);
                                }
                                break;
                                #endregion

                            case "303": // RPL_ISON
                                ActiveIRCWindow.PutMessage(string.Format(Resources.IsOnReply, message.Tail), settings.Colors[5], true);
                                break;

                            case "305": // RPL_UNAWAY
                                this.ActiveIRCWindow.PutMessage(Resources.MarkedAsNotBeingAway, settings.Colors[3]);
                                break;

                            case "306": // RPL_NOWAWAY
                                this.ActiveIRCWindow.PutMessage(Resources.MarkedAsBeingAway, settings.Colors[3]);
                                break;

                            case "307":
                                lock (whoisLock)
                                {
                                    if (!whois.ContainsKey(param[1]))
                                        whois[param[1]] = new UserInfo();

                                    whois[param[1]].IsRegistered = true;
                                    if (!notPrintWhois.Contains(param[1]))
                                    {
                                        ActiveIRCWindow.PutMessage(string.Format(Resources.IsRegisteredNick, param[1]), 6);
                                    }
                                }
                                break;

                            //case "308": // RPL_WHOISADMIN

                            //case "309": // RPL_WHOISSADMIN

                            case "310":
                                // TODO: перенести в whois
                                PutMessage(String.Format("{0} {1}", message.Param[1], Resources.IsAvailableForHelp), Settings.Default.Colors[6]);
                                break;

                            case "311": // RPL_WHOISUSER
                                #region RPL_WHOISUSER
                                lock (whoisLock)
                                {
                                    if (!whois.ContainsKey(param[1]))
                                        whois[param[1]] = new UserInfo();
                                    else if (whois[param[1]].Obsolete == -1)
                                    {
                                        //whois[param[1]] = new UserInfo();
                                        notPrintWhois.Add(param[1]);
                                    }
                                    whois[param[1]].Nick = param[1];
                                    ((User)whois[param[1]].User).UserName = param[2];
                                    ((User)whois[param[1]].User).Host = param[3];
                                    ((User)whois[param[1]].User).Name = tail;
                                    whois[param[1]].Obsolete = Settings.Default.ObsoleteTime;
                                    if (!notPrintWhois.Contains(param[1]))
                                    {
                                        this.ActiveIRCWindow.PutMessage(String.Format("{0}{1}:", Resources.InformationAbout, param[1]), settings.Colors[6]);
                                        this.ActiveIRCWindow.PutMessage(String.Format("{0}: {1}@{2}", Resources.User, param[2], param[3]), settings.Colors[6]);
                                        this.ActiveIRCWindow.PutMessage(String.Format("{0}: {1}", Resources.RealName, tail), settings.Colors[6]);
                                    }
                                }
                                break;
                                #endregion

                            case "312": // RPL_WHOISSERVER
                                lock (whoisLock)
                                {
                                    if (!whois.ContainsKey(param[1]))
                                        whois[param[1]] = new UserInfo();
                                    whois[param[1]].Server = param[2];
                                    whois[param[1]].ServerInfo = tail;
                                    if (!notPrintWhois.Contains(param[1]))
                                    {
                                        this.ActiveIRCWindow.PutMessage(string.Format("{0}: {1} ({2})", Resources.Server, param[2], tail), settings.Colors[6]);
                                    }
                                }
                                break;

                            case "313": // RPL_WHOISOPERATOR
                                lock (whoisLock)
                                {
                                    if (!whois.ContainsKey(param[1]))
                                        whois[param[1]] = new UserInfo();
                                    whois[param[1]].IsIRCOp = true;
                                    if (!notPrintWhois.Contains(param[1]))
                                    {
                                        this.ActiveIRCWindow.PutMessage(String.Format("{0} {1}", param[1], Resources.IsIRCOp), settings.Colors[6]);
                                    }
                                }
                                break;

                            case "314": // RPL_WHOWASUSER
                                ActiveIRCWindow.PutMessage(string.Format(Resources.WhoWasUser, param[1], param[2], param[3], tail), settings.Colors[6]);
                                break;

                            case "315": // RPL_ENDOFWHO
                                ActiveIRCWindow.PutMessage(string.Format(Resources.EndOfWhoList, param[1]), settings.Colors[6]);
                                break;

                            //case "316": // RPL_WHOISCHANOP

                            case "317": // RPL_WHOISIDLE
                                lock (whoisLock)
                                {
                                    if (!whois.ContainsKey(param[1]))
                                        whois[param[1]] = new UserInfo();
                                    whois[param[1]].Idle = int.Parse(param[2]);
                                    if (!notPrintWhois.Contains(param[1]))
                                    {
                                        this.ActiveIRCWindow.PutMessage(String.Format(Resources.UserIdle, param[1], param[2]), settings.Colors[6]);
                                    }
                                }
                                break;

                            case "318": // RPL_WHOISSERVER // RPL_ENDOFWHOIS
                                if (!notPrintWhois.Contains(param[1]))
                                    this.ActiveIRCWindow.PutMessage(Resources.EndOfData, settings.Colors[6]);
                                else
                                {
                                    if (notPrintWhois.Contains(param[1]))
                                        notPrintWhois.Remove(param[1]);
                                    if (WhoisUpdated != null)
                                        WhoisUpdated(this, new PersonEventArgs(param[1]));
                                }
                                break;

                            case "319": // RPL_WHOISCHANNELS
                                #region RPL_WHOISCHANNELS
                                lock (whoisLock)
                                {
                                    try
                                    {
                                        if (!whois.ContainsKey(param[1]))
                                            whois[param[1]] = new UserInfo();
                                        string[] chans = tail.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                        Log(String.Format("{0} {1}", string.Join(" ", chans), param[1]));
                                        Array.ForEach(chans, chan =>
                                        {
                                            if (MDIChildServer.ModesTable2.ContainsValue(chan[0]))
                                                whois[param[1]].Modes[chan.Substring(1)] = chan[0];
                                            else
                                                whois[param[1]].Modes[chan] = ' ';
                                        });
                                        if (!notPrintWhois.Contains(param[1]))
                                            this.ActiveIRCWindow.PutMessage(String.Format("{0} {1}", Resources.IsOnChannels, tail), settings.Colors[6]);
                                    }
                                    catch (Exception exc) { MessageBox.Show(String.Format("{0} {1}", line, exc)); }
                                }
                                break;
                                #endregion

                            case "320":
                                lock (whoisLock)
                                {
                                    if (!whois.ContainsKey(param[1]))
                                        whois[param[1]] = new UserInfo();
                                    whois[param[1]].CodePage = tail;
                                }

                                if (!notPrintWhois.Contains(param[1]))
                                    this.ActiveIRCWindow.PutMessage(String.Format("{0}: {1}", param[1], tail), settings.Colors[6]);
                                break;

                            case "321": // RPL_LISTSTART
                                channelInfo.Clear();
                                break;

                            case "322": // RPL_LIST
                                channelInfo.Add(new ChannelInfo(message.Param[1], int.Parse(message.Param[2]), message.Tail));
                                break;

                            case "323": // RPL_LISTEND
                                this.channelInfo.Sort(delegate(ChannelInfo ci1, ChannelInfo ci2) { return ci1.Name.CompareTo(ci2.Name); });
                                var view = new ChannelViewForm(this.channelInfo);
                                view.NeedJoin += (channel) => JoinChannel(channel);
                                this.Main.RegisterAsMDIChild(this, view, null);
                                break;

                            case "324": // RPL_CHANNELMODEIS
                                if (this.channelForms.ContainsKey(param[1].ToLower()))
                                {
                                    string[] modPar = new string[param.Length - 3];
                                    for (int i = 3; i < param.Length; i++)
                                    {
                                        modPar[i - 3] = param[i];
                                    }
                                    ((MDIChildChannel)ChildByName(param[1])).Mode(name, param[2], modPar);
                                }
                                break;

                            case "329": // RPL_CREATIONTIME
                                if (this.channelForms.ContainsKey(param[1].ToLower()))
                                    ((MDIChildChannel)ChildByName(param[1])).PutMessage(String.Format("{1} {0}", DateTime.Parse("01.01.1970 00:00:00").AddSeconds(double.Parse(param[2])).ToLocalTime(), Resources.TopicCreated), settings.Colors[3]);
                                break;

                            case "330":
                                lock (whoisLock)
                                {
                                    if (!whois.ContainsKey(param[1]))
                                        whois[param[1]] = new UserInfo();
                                    whois[param[1]].Auth = param[2];
                                    if (!notPrintWhois.Contains(param[1]))
                                    {
                                        ActiveIRCWindow.PutMessage(String.Format("{0}: {1} - {2}", param[1], Resources.RealName, param[2]), 6);
                                    }
                                }
                                break;

                            case "331": // RPL_NOTOPIC
                                ((MDIChildChannel)ChildByName(param[1])).Topic = string.Empty;
                                break;

                            case "332": // RPL_TOPIC
                                ((MDIChildChannel)ChildByName(param[1])).Topic = tail;
                                break;

                            case "333": // RPL_TOPICWHOTIME
                                ((MDIChildChannel)ChildByName(param[1])).PutMessage(String.Format("{0} {1}", Resources.TopicSetBy, param[2]), settings.Colors[3]);
                                break;

                            case "334": // RPL_COMMANDSYNTAX
                                ActiveIRCWindow.PutMessage(tail, settings.Colors[14], false);
                                break;

                            case "335":
                                lock (whoisLock)
                                {
                                    if (!whois.ContainsKey(param[1]))
                                        whois[param[1]] = new UserInfo();

                                    Log(line);
                                    whois[param[1]].IsBot = true;

                                    if (!notPrintWhois.Contains(param[1]))
                                        this.ActiveIRCWindow.PutMessage(String.Format("{0}: {1}", param[1], tail), settings.Colors[6]);
                                }
                                break;

                            //case "337": // RPL_WHOISTEXT

                            case "338": // RPL_WHOISACTUALLY
                                lock (whoisLock)
                                {
                                    if (!whois.ContainsKey(param[1]))
                                        whois[param[1]] = new UserInfo();

                                    Log(line);
                                    whois[param[1]].WhoisActially = tail;

                                    if (!notPrintWhois.Contains(param[1]))
                                        ActiveIRCWindow.PutMessage(String.Format("{0}: {1}", param[1], tail), settings.Colors[6]);
                                }
                                break;

                            case "339": // RPL_WHOISACTUALLY
                                goto case "338";

                            case "341": // RPL_INVITING
                                ActiveIRCWindow.PutMessage(string.Format(Resources.RplInvitingMessage, param[1], param[2]), settings.Colors[6]);
                                break;

                            //case "342": // RPL_SUMMONING

                            case "346": // RPL_INVITELIST
                                ((MDIChildChannel)ChildByName(param[1])).AddInvite(new MaskInfo(param[2], param[3], DateTime.Parse("01.01.1970 00:00:00").AddSeconds(double.Parse(param[4])).ToLocalTime()));
                                break;

                            case "347": // RPL_ENDOFINVITELIST
                                ((MDIChildChannel)ChildByName(param[1])).InvitesFinished();
                                break;

                            case "348": // RPL_EXEMPTLIST
                                ((MDIChildChannel)ChildByName(param[1])).AddExempt(new MaskInfo(param[2], param[3], DateTime.Parse("01.01.1970 00:00:00").AddSeconds(double.Parse(param[4])).ToLocalTime()));
                                break;

                            case "349": // RPL_ENDOFEXEMPTLIST
                                ((MDIChildChannel)ChildByName(param[1])).ExemptsFinished();
                                break;

                            case "351": // RPL_VERSION
                                var ver = param[1].Split('.');
                                ActiveIRCWindow.PutMessage(string.Format(Resources.VersionMessage, param[2], param[1] + (ver[ver.Length - 1].Length > 0 ? string.Format("({0})", Resources.ServerDebugMode) : ""), tail), settings.Colors[6]);
                                break;

                            case "352": // RPL_WHOREPLY
                                int ind = tail.IndexOf(' ');
                                string hops = tail.Substring(0, ind);
                                string realName = tail.Substring(ind + 1);
                                string flags = param[6];
                                var userData = new List<string>();
                                userData.Add(flags.Contains("H") ? Resources.NotAway : Resources.Away);
                                if (flags.Contains("*"))
                                    userData.Add(Resources.IRCOp);
                                if (flags.Contains("@"))
                                    userData.Add(Resources.WithOp);
                                else if (flags.Contains("+"))
                                    userData.Add(Resources.WithVoice);

                                var whoInfo = string.Join(", ", userData.ToArray());
                                ActiveIRCWindow.PutMessage(string.Format(Resources.WhoReply, param[2], param[3], param[1], param[4], hops, whoInfo, realName, param[5]), settings.Colors[6]);
                                break;

                            case "353": // RPL_NAMREPLY
                                var pers = tail.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                if (channelForms.TryGetValue(param[2].ToLower(), out child))
                                {
                                    Array.ForEach<string>(pers, person => { child.AddPerson(person, false); });
                                }
                                else
                                {
                                    ActiveIRCWindow.PutMessage(string.Format(Resources.UsersOnChannel, param[2], tail), settings.Colors[6]);
                                }
                                break;

                            //case "354": // RPL_RWHOREPLY

                            //case "362": // RPL_CLOSING

                            //case "363": // RPL_CLOSEEND

                            case "364": // RPL_LINKS
                                ind = tail.IndexOf(' ');
                                linksData.Rows.Add(param[1], param[2], tail.Substring(0, ind), tail.Substring(ind + 1));
                                break;

                            case "365": // RPL_ENDOFLINKS
                                using (var linksViewForm = new LinksViewForm(linksData))
                                {
                                    this.Main.RegisterAsMDIChild(this, linksViewForm, null);
                                }
                                linksData.Clear();
                                break;

                            case "366": // RPL_ENDOFNAMES
                                //ActiveIRCWindow.PutMessage(Resources.EndOfList, settings.Colors[6]);
                                break;

                            case "367": // RPL_BANLIST
                                ((MDIChildChannel)ChildByName(param[1])).AddBan(new MaskInfo(param[2], param[3], DateTime.Parse("01.01.1970 00:00:00").AddSeconds(double.Parse(param[4])).ToLocalTime()));
                                break;

                            case "368": // RPL_ENDOFBANLIST
                                ((MDIChildChannel)ChildByName(param[1])).BansFinished();
                                break;

                            case "369": // RPL_ENDOFWHOWAS
                                ActiveIRCWindow.PutMessage(Resources.EndOfWhoWas, settings.Colors[6]);
                                break;

                            case "371": // RPL_INFO
                                PutMessage(tail, settings.Colors[5], false);
                                break;

                            case "372": // RPL_MOTD
                                if (motd)
                                    PutMessage(tail, settings.Colors.DefForeColor, false);
                                break;

                            //case "373": // RPL_INFOSTART

                            case "374": // RPL_ENDOFINFO
                                PutMessage(Resources.EndOfInfo, settings.Colors[5], false);
                                break;

                            case "375": // RPL_MOTDSTART
                                motd = true;
                                PutMessage(Resources.MOTD, settings.Colors[5], false);
                                PutMessage(tail, settings.Colors.DefForeColor, false);
                                break;

                            case "376": // RPL_ENDOFMOTD
                                motd = false;
                                PutMessage(Resources.EndOfMOTD, settings.Colors[5], false);
                                // PutMessage(tail, settings.Colors.DefForeColor, false);
                                break;

                            case "378": // RPL_WHOISHOST
                                lock (whoisLock)
                                {
                                    if (!whois.ContainsKey(param[1]))
                                        whois[param[1]] = new UserInfo();
                                    whois[param[1]].WhoisHost = tail;
                                    if (!notPrintWhois.Contains(param[1]))
                                        this.ActiveIRCWindow.PutMessage(String.Format("{0}: {1}", param[1], tail), settings.Colors[6]);
                                }
                                break;

                            case "381": // RPL_YOUREOPER
                                PutMessage(Resources.YouAreAnIRCOp, settings.Colors[12], true);
                                break;

                            //case "382": // RPL_REHASHING

                            //case "384": // RPL_MYPORTIS

                            case "391": // RPL_TIME
                                ActiveIRCWindow.PutMessage(string.Format(Resources.ServerTime, param[1], tail), settings.Colors[5]);
                                break;

                            case "401": // ERR_NOSUCHNICK
                                ActiveIRCWindow.PutMessage(String.Format("{0}: {1}", Resources.ChannelNickDoesNotExistst, param[1]), settings.Colors[4], false);
                                break;

                            case "402": // ERR_NOSUCHSERVER
                                PutMessage(string.Format(Resources.NoSuchServerMessage, param[1]), settings.Colors[4], false);
                                break;

                            case "403": // ERR_NOSUCHCHANNEL
                                PutMessage(string.Format(Resources.NoSuchChannelMessage, param[1]), settings.Colors[4], false);
                                break;

                            case "404": // ERR_CANNOTSENDTOCHAN
                                ChildByName(message.Param[1]).PutMessage(string.Format("{0}: {1}", Resources.CannotSendToChannel, message.Tail), settings.Colors[4], false);
                                break;

                            case "405": // ERR_TOOMANYCHANNELS
                                ActiveIRCWindow.PutMessage(Resources.TooManyChannelsMessage, settings.Colors[4], false);
                                break;

                            case "406": // ERR_WASNOSUCHNICK
                                ActiveIRCWindow.PutMessage(string.Format(Resources.WasNoSuchNick, param[1]), settings.Colors[4]);
                                break;

                            case "407": // ERR_TOOMANYTARGETS
                                ActiveIRCWindow.PutMessage(string.Format("{0}: {1}", message.Param[1], Resources.TooManyTargetsMessage), settings.Colors[4], false);
                                break;

                            //case "408": // ERR_NOCTRLSONCHAN

                            case "409": // ERR_NOORIGIN
                                PutMessage(Resources.NoOriginMessage, settings.Colors[4], false);
                                break;

                            //case "411": // ERR_NORECIPIENT

                            case "412": // ERR_NOTEXTTOSEND
                                PutMessage(Resources.NoTextToSendMessage, settings.Colors[4], false);
                                break;

                            case "413": // ERR_NOTOPLEVEL
                                ActiveIRCWindow.PutMessage(string.Format("{0}: {1}", message.Param[1], Resources.NoTopLevelDomainMessage), settings.Colors[4], false);
                                break;

                            case "414": // ERR_WILDTOPLEVEL
                                ActiveIRCWindow.PutMessage(string.Format("{0}: {1}", message.Param[1], Resources.WildTopLevelMessage), settings.Colors[4], false);
                                break;

                            case "421": // ERR_UNKNOWNCOMMAND
                                ActiveIRCWindow.PutMessage(string.Format(Resources.UnknownCommandMessage, param[1]), settings.Colors[4]);
                                break;

                            case "422": // ERR_NOMOTD
                                PutMessage(Resources.NoMOTDMessage, settings.Colors[4], false);
                                break;

                            case "423": // ERR_NOADMININFO
                                ActiveIRCWindow.PutMessage(string.Format(Resources.NoAdminInfoMessage, message.Param[1]), settings.Colors[4], false);
                                break;

                            case "424": // ERR_FILEERROR
                                ActiveIRCWindow.PutMessage(tail, settings.Colors[4], false);
                                break;

                            //case "429": // ERR_TOOMANYAWAY

                            case "431": // ERR_NONICKNAMEGIVEN
                                ActiveIRCWindow.PutMessage(Resources.NoNicknameGivenMessage, settings.Colors[4], false);
                                break;

                            case "432": // ERR_ERRONEUSNICKNAME
                                ActiveIRCWindow.PutMessage(String.Format("{0} {1}", Resources.BadNick, param[1]), settings.Colors[4]);
                                break;

                            case "433": // ERR_NICKNAMEINUSE
                                {
                                    ActiveIRCWindow.PutMessage(string.Format(Resources.NicknameInUseMessage, param[1]), settings.Colors[4]);
                                    string password;
                                    if (!this.Server.Passwords.Data.TryGetValue(param[1], out password))
                                    {
                                        this.ghostNick = param[1];                                        
                                    }
                                    this.SetNick(string.Format("|{0}|", param[1]));
                                }
                                break;

                            //case "435": // ERR_BANONCHAN

                            //case "436": // ERR_NICKCOLLISION

                            case "437": // ERR_BANNICKCHANGE
                                ActiveIRCWindow.PutMessage(Resources.BanNickChangeMessage, settings.Colors[4], false);
                                break;

                            //case "438":

                            case "439": // ERR_TARGETTOOFAST
                                ActiveIRCWindow.PutMessage(String.Format("{0}: {1}", param[1], tail), settings.Colors[6], false);
                                break;

                            case "440": // ERR_SERVICESDOWN
                                PutMessage(Resources.ServiceDownMessage, settings.Colors[4]);
                                break;

                            //case "441": // ERR_USERNOTINCHANNEL

                            case "442": // ERR_NOTONCHANNEL
                                PutMessage(string.Format(Resources.YouAreNotOnAChannel, param[1]), settings.Colors[4]);
                                break;

                            case "443": // ERR_USERONCHANNEL
                                ActiveIRCWindow.PutMessage(string.Format(Resources.UserOnChannelMessage, param[1], param[2]), Settings.Default.Colors[4]);
                                break;

                            //case "444": // ERR_NOLOGIN

                            //case "445": // ERR_SUMMONDISABLED

                            case "446": // ERR_USERSDISABLED
                                PutMessage(Resources.UsersDisabledMessage, Settings.Default.Colors[4]);
                                break;

                            case "451": // ERR_NOTREGISTERED
                                PutMessage(param[0] + Resources.NotRegisteredMessage, Settings.Default.Colors[4]);
                                break;

                            case "458":
                                ActiveIRCWindow.PutMessage(string.Format(Resources.NotInAcceptList, param[1]), settings.Colors[4]);
                                break;

                            case "461": // ERR_NEEDMOREPARAMS
                                PutMessage(string.Format(Resources.NeedMoreParamsMessage, param[1]), settings.Colors[4]);
                                break;

                            case "462": // ERR_ALREADYREGISTRED
                                PutMessage(Resources.AlreadyRegisteredMessage, settings.Colors[4]);
                                break;

                            //case "463": // ERR_NOPERMFORHOST

                            //case "464": // ERR_PASSWDMISMATCH

                            case "465": // ERR_YOUREBANNEDCREEP
                                PutMessage(string.Format("{0}: {1}", Resources.CannotJoin, tail), settings.Colors[4]);
                                break;

                            //case "467": // ERR_KEYSET

                            case "468": // ERR_ONLYSERVERSCANCHANGE
                                ActiveIRCWindow.PutMessage(string.Format(Resources.Message468, param[1]), settings.Colors[4]);
                                break;

                            //case "471": // ERR_CHANNELISFULL

                            case "472": // ERR_UNKNOWNMODE
                                ActiveIRCWindow.PutMessage(string.Format(Resources.UnknownModeMessage, param[1]), settings.Colors[4]);
                                break;

                            case "473": // ERR_INVITEONLYCHAN
                                ActiveIRCWindow.PutMessage(string.Format(Resources.InviteOnly, param[1]), Settings.Default.Colors[4]);
                                break;

                            case "474": // ERR_BANNEDFROMCHAN
                                ActiveIRCWindow.PutMessage(string.Format(Resources.BannedFormChannelMessage, param[1]), Settings.Default.Colors[4]);
                                break;

                            case "475": // ERR_BADCHANNELKEY
                                using (var diag = new StringEnterDialog(Resources.PasswordNeeded))
                                {
                                    if (diag.ShowDialog() == DialogResult.OK && diag.PrintedText.Length > 0)
                                        JoinChannel(param[1], diag.PrintedText);
                                }
                                break;

                            case "476": // ERR_BADCHANMASK
                                PutMessage(string.Format(Resources.BadChanMaskMessage, param[1]), settings.Colors[4]);
                                break;

                            case "477": // ERR_NEEDREGGEDNICK
                                ActiveIRCWindow.PutMessage(message.Tail, Settings.Default.Colors[5], false);
                                break;

                            //case "478": // ERR_BANLISTFULL

                            case "479": // ERR_BADCHANNAME
                                ActiveIRCWindow.PutMessage(Resources.IncorrectCharactersInChannelName, Settings.Default.Colors[4]);
                                break;

                            case "481": // ERR_NOPRIVILEGES
                                ActiveIRCWindow.PutMessage(Resources.NoPrivileges, Settings.Default.Colors[4]);
                                break;

                            case "482": // ERR_CHANOPRIVSNEEDED
                                ActiveIRCWindow.PutMessage(Resources.ChannelOpPrivsNeeded, settings.Colors[4]);
                                break;

                            case "483": // ERR_CANTKILLSERVER
                                PutMessage(Resources.CannotKillServer, settings.Colors[4]);
                                break;

                            //case "485": // ERR_CHANBANREASON

                            //case "486": // ERR_NONONREG

                            //case "487": // ERR_MSGSERVICES

                            case "491": // ERR_NOOPERHOST
                                ActiveIRCWindow.PutMessage(message.Tail, settings.Colors[4], true);
                                break;

                            //case "494": // ERR_OWNMODE

                            //case "501": // ERR_UMODEUNKNOWNFLAG

                            case "502": // ERR_USERSDONTMATCH
                                PutMessage(Resources.UserDontMatchMessage, settings.Colors[4]);
                                break;

                            //case "511": // ERR_SILELISTFULL

                            //case "512": // ERR_TOOMANYWATCH

                            //case "514": // ERR_TOOMANYDCC

                            //case "521": // ERR_LISTSYNTAX

                            //case "522": // ERR_WHOSYNTAX

                            //case "523": // ERR_WHOLIMEXCEED

                            case "524":
                                PutMessage(String.Format("{0}: {1}", param[1], Resources.HelpNotFoundMessage), settings.Colors[4]);
                                break;

                            //case "600": // RPL_LOGON

                            //case "601": // RPL_LOGOFF

                            //case "602": // RPL_WATCHOFF

                            //case "603": // RPL_WATCHSTAT

                            //case "604": // RPL_NOWON

                            //case "605": // RPL_NOWOFF

                            //case "606": // RPL_WATCHLIST

                            //case "607": // RPL_ENDOFWATCHLIST

                            //case "617": // RPL_DCCSTATUS

                            //case "618": // RPL_DCCLIST

                            //case "619": // RPL_ENDOFDCCLIST

                            //case "620": // RPL_DCCINFO

                            case "634":
                                PutMessage(tail, Settings.Default.Colors[5], false);
                                break;

                            case "635":
                                PutMessage(tail, Settings.Default.Colors[5], false);
                                break;

                            case "671":
                                // TODO: -> WHOIS
                                // [14:47:50] :2777.ru 671 Ur-Quan Dmitry :is using a Secure Connection
                                PutMessage(string.Format("{0} {1}", param[1], tail), 6, false);
                                break;

                            case "700":
                                if (message.Param.Count > 1)
                                    PutMessage(string.Format(Resources.TranslationSchemeMessage, param[1]), settings.Colors[6], false);
                                else
                                    PutMessage(tail, Settings.Default.Colors[6], false);
                                break;

                            case "703":
                                goto case "320";

                            case "704": // RPL_HELPSTART
                                PutMessage(tail, settings.Colors[6], false);
                                break;

                            case "705": // RPL_HELPTXT
                                PutMessage(tail, settings.Colors[6], false);
                                break;

                            case "706": // RPL_ENDOFHELP
                                PutMessage(Resources.EndOfHelp, settings.Colors[6], false);
                                break;

                            case "713":
                                ActiveIRCWindow.PutMessage(string.Format(Resources.KnockOpenMessage, param[1]), settings.Colors[4]);
                                break;

                            case "716":
                                ActiveIRCWindow.PutMessage(Resources.InGMode, settings.Colors[6]);
                                break;

                            case "718":
                                ActiveIRCWindow.PutMessage(string.Format(Resources.SendingToG, param[1]), settings.Colors[4]);
                                break;

                            default:
                                PutMessage(line);
                                break;
                        }
                    }
                    catch (Exception exc)
                    {
                        PutMessage(string.Format(Resources.ErrorInMessage, String.Format("{0}: {1}", line, exc.Message)), Settings.Default.Colors[4], true);
                    }
                }
            }
        }

        private void ChangeNick(string name, string tail)
        {
            if (IsMyNick(name))
            {
                PutMessage(String.Format("{0} {1}", Resources.NowYourNickIs, tail), settings.Colors[3]);
                this.connectionInfo.Nick = tail;
                /*myNode*/
                this.Text = String.Format("{0} [{1}]", string.IsNullOrEmpty(this.connectionInfo.Server.Description) ? this.connectionInfo.Server.Name : this.connectionInfo.Server.Description, this.connectionInfo.Nick);
            }

            foreach (var item in channelForms.Values)
            {
                if (item.HasPerson(name))
                {
                    item.RenamePerson(name, tail);
                    item.PutMessage(string.Format(Resources.ChangeNickTo, name, tail), 3, true);
                }
            }

            if (channelForms.ContainsKey(name.ToLower()))
            {
                channelForms[tail.ToLower()] = channelForms[name.ToLower()];
                channelForms.Remove(name.ToLower());
            }

            lock (whoisLock)
            {
                if (whois.ContainsKey(name))
                {
                    whois[tail] = whois[name];
                    whois.Remove(name);
                }
            }

            if (PersonRenames != null)
                PersonRenames(this, new TwoPersonsEventArgs(name, tail));
        }

        private bool IsMyNick(string nick)
        {
            return this.connectionInfo.Nick == nick;
        }

        /// <summary>
        /// Разорвано соединение с сервером
        /// </summary>
        void connection_Disposed()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((Action)connection_Disposed);
                return;
            }

            connection.Disposed -= connection_Disposed;
            this.myNode.ForeColor = UISettings.Default.ServerDisconnectedColor;

            PutMessage(Resources.ServerLinkClosed, Settings.Default.Colors[2]);

            foreach (var item in channelForms.Values)
            {
                item.PutMessage(Resources.ServerLinkClosed, settings.Colors[2]);
                item.ClearPersons();
            }

            // Попытаемся восстановить соединение
            if (!reconnecting && numOfTries < 10)
            {
                numOfTries++;
                this.Connect();
            }
        }

        #endregion

        #region ChildForms event handlers

        void childForm_OpenPrivate(string name)
        {
            ChildByName(name).Focus();
        }

        void childForm_Cmd(MDIChildCommunication child, string cmd)
        {
            Send(child.ReceiverName, cmd, false);
        }

        private delegate void SendDel(string person, string text, bool silent);

        public void Send(string person, string text, bool silent)
        {
            if (text.Length == 0)
                return;

            if (text[0] == Special.CmdStarter)
                Execute(person, text.Substring(1).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));
            else
                Say(person, text, silent);

            lastCmdTime = DateTime.Now;
        }

        /// <summary>
        /// Закрыта дочерняя форма
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void childForm_Disposed(object sender, EventArgs e)
        {
            ((MDIChildCommunication)sender).Cmd -= childForm_Cmd;
            foreach (var child in channelForms)
                if (child.Value == sender)
                {
                    channelForms.Remove(child.Key);
                    var channel = child.Value as MDIChildChannel;
                    if (channel != null)
                    {
                        var channels = ((CIRCeServer)this.DataContext).Channels;
                        ((Changeable<ICIRCeChannel>)channels).Remove((CIRCeChannel)channel.DataContext);
                    }
                    return;
                }
        }

        #endregion

        private void MDIChildServer_KeyDown(object sender, KeyEventArgs e)
        {
            foreach (var item in UserOptions.Default.HotKeys)
            {
                if (item.Key == (e.KeyCode & item.Key))
                {
                    printMessageIrcRichTextBox_EditFinished(irtbPrintMessage, new EnterPushedEventArgs(item.Value));
                    break;
                }
            }
        }

        #region IServerWindow Members

        /// <summary>
        /// Сервер
        /// </summary>
        public ExtendedServerInfo Server
        {
            get { return this.connectionInfo.Server; }
        }

        /// <summary>
        /// Пользователь
        /// </summary>
        public string Nick
        {
            get { return this.connectionInfo.Nick; }
        }

        /// <summary>
        /// Открытые каналы
        /// </summary>
        internal MDIChildChannel[] Channels
        {
            get
            {
                var result = new List<MDIChildChannel>();
                foreach (var window in channelForms.Values)
                {
                    var channel = window as MDIChildChannel;
                    if (channel != null)
                    {
                        result.Add(channel);
                    }
                }
                return result.ToArray();
            }
        }

        /// <summary>
        /// Подключение состоялось
        /// </summary>
        public event EventHandler OnConnected;

        /// <summary>
        /// Состоялось присоединение к новому окну
        /// </summary>
        internal event EventHandler<JoinEventArgs> NewWindow;

        private bool reconnecting = false;

        /// <summary>
        /// Организовать подключение
        /// </summary>
        public void Connect()
        {
            if (connection.Connected)
            {
                this.reconnecting = true;
                connection.Dispose();
                this.reconnecting = false;
            }

            PutMessage(String.Format("{0} {1}", Resources.ConnectingTo, this.connectionInfo.Server), settings.Colors[2]);

            Task.Factory.StartNew(() =>
                {
                    try
                    {
                        connection.Connect();
                        ConnectionFinished(null);
                    }
                    catch (Exception exc)
                    {
                        this.PutMessage(exc.Message, settings.Colors[2]);
                    }
                });

            //ConnectDel connectDel = BeginConnect;
            //AsyncCallback callback = ConnectionFinished;
            //connectDel.BeginInvoke(callback, null);
        }

        private void BeginConnect()
        {
            try
            {
                connection.Connect();
            }
            catch (Exception exc)
            {
                this.PutMessage(exc.Message, settings.Colors[2]);
            }
        }

        /// <summary>
        /// Подключено ли
        /// </summary>
        public bool IsConnected
        {
            get { return fullConnect; }
        }
        
        /// <summary>
        /// Присоединиться к каналу
        /// </summary>
        /// <param name="channel">Имя канала</param>
        public override void JoinChannel(string channel)
        {
            connection.RunCmd("JOIN", channel);
            connection.RunCmd("MODE", channel);
        }

        /// <summary>
        /// Присоединиться к каналу
        /// </summary>
        /// <param name="channel">Имя канала</param>
        /// <param name="key">Пароль</param>
        public void JoinChannel(string channel, string key)
        {
            connection.RunCmd("JOIN", channel, key);
            connection.RunCmd("MODE", channel);
        }

        private void DelayedSetNick(string nick)
        {
            Thread.Sleep(1000);
            SetNick(nick);
        }

        /// <summary>
        /// Установить ник
        /// </summary>
        /// <param name="p">Ник</param>
        public void SetNick(string p)
        {
            connection.RunCmd("NICK", p);
        }

        protected override void JoinChannel(object sender, EventArgs e)
        {
            JoinChannel(this.Server.Channels[this.tSMIChannels.DropDownItems.IndexOf((ToolStripItem)sender)].Name);
        }

        /// <summary>
        /// Открыто ли окно
        /// </summary>
        /// <param name="name">Имя окна</param>
        /// <returns></returns>
        public bool ContainsWindow(string name)
        {
            return this[name] != null;
        }

        internal MDIChildCommunication this[string name]
        {
            get
            {
                foreach (MDIChildCommunication window in channelForms.Values)
                {
                    if (window.WindowName == name)
                    {
                        return window;
                    }
                }
                return null;
            }
        }

        #endregion

        protected override void ChangeNick(object sender, EventArgs e)
        {
            this.SetNick(sender.ToString());
        }

        #region IServerWindow Members
        
        public MDIChildPrivate[] Privates
        {
            get 
            {
                var result = new List<MDIChildPrivate>();
                foreach (MDIChildCommunication window in channelForms.Values)
                {
                    var personWindow = window as MDIChildPrivate;
                    if (personWindow != null)
                    {
                        result.Add(personWindow);
                    }
                }
                return result.ToArray();
            } 
        }

        public event EventHandler<TwoPersonsEventArgs> PersonRenames;

        public event EventHandler<IRCMessageEventArgs> IRCMessageReceived;

        #endregion

        private void MDIChildServer_Activated(object sender, EventArgs e)
        {
            if (this.myNode != null)
                this.myNode.ForeColor = fullConnect ? UISettings.Default.ServerConnectedColor : UISettings.Default.ServerDisconnectedColor;
        }

        private void tsmiReconnect_Click(object sender, EventArgs e)
        {
            this.Connect();
        }

        private void tsmiAway_Click(object sender, EventArgs e)
        {
            string message = string.Empty;
            if (!tsmiAway.Checked)
            {
                var stringEnterDialog = new StringEnterDialog("Введите сообщение, информирующее о вашем отсутствии");
                if (stringEnterDialog.ShowDialog() == DialogResult.OK)
                    message = stringEnterDialog.PrintedText;
                if (message.Length == 0)
                    message = "AWAY";
            }

            Away(message);
            tsmiAway.Checked = !tsmiAway.Checked;
        }

        internal void SaveTimeForPing(string name, int pid)
        {
            pingTimeTable.Add(new PingInfo() { Server = name, SentTime = DateTime.Now, Pid = pid });
        }

        private void tsmiList_Click(object sender, EventArgs e)
        {
            connection.RunCmd("LIST");
        }

        private void tsmiAdmin_Click(object sender, EventArgs e)
        {
            connection.RunCmd("ADMIN");
        }

        private void tsmiTime_Click(object sender, EventArgs e)
        {
            connection.RunCmd("TIME");
        }

        private void tsmiInfo_Click(object sender, EventArgs e)
        {
            connection.RunCmd("INFO");
        }

        private void tsmiPing_Click(object sender, EventArgs e)
        {
            int pid = Program.Rand.Next();
            SaveTimeForPing(connection.UsingServer, pid);
            connection.RunCmd("PING", /*pid.ToString(),*/ this.connectionInfo.Server.Name);
        }

        private void tsmiLiveTime_Click(object sender, EventArgs e)
        {
            connection.RunCmd("STATS", "u");
        }

        private void tsmiCommands_Click(object sender, EventArgs e)
        {
            connection.RunCmd("STATS", "m");
        }
    }
}