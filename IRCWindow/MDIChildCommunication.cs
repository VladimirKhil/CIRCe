using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IRCConnection;
using IRCWindow.Properties;
using IRCProviders;
using System.IO;
using System.Xml;
using System.Web;
using System.Text.RegularExpressions;
using System.Net;
using CIRCe.Base;
using IRCWindow.ViewModel;
using IRC.Client.Base;
using System.Linq;
using IRC.Client;
using IRCWindow.Data;

namespace IRCWindow
{
    /// <summary>
    /// Окно чата (на канале или в привате)
    /// </summary>
    internal partial class MDIChildCommunication : MDIChild
    {
        protected MDIChildServer mainWindow = null;
        private TextWriter log = null;
        protected Changeable<ChannelUser> channelUsers = new Changeable<ChannelUser>();
        private int logLength = 0;

        public override MDIChildServer ServerWindow
        {
            get
            {
                return mainWindow;
            }
        }

        /// <summary>
        /// Имя получателя сообщений
        /// </summary>
        public virtual string ReceiverName
        {
            get { return this.Text; }
        }

        /// <summary>
        /// Введена команда
        /// </summary>
        /// <param name="child">Окно, в котором была ведена команда</param>
        /// <param name="cmd">Текст команды</param>
        public delegate void CmdDel(MDIChildCommunication child, string cmd);

        public virtual event CmdDel Cmd;

        public MDIChildCommunication() { InitializeComponent(); }

        public MDIChildCommunication(MDIParent main)
            : base(main)
        {
            InitializeComponent();
        }

        public MDIChildCommunication(MDIParent main, MDIChildServer mainWindow, string name, Dictionary<string, IRCConnection.UserInfo> whois)
            :base(main)
        {
            InitializeComponent();

            this.mainWindow = mainWindow;
            this.whois = whois;
            this.bsUsers.DataSource = this.channelUsers;
            this.bsUsers.ResetBindings(true);
        }

        protected void RaiseCmd(string cmd)
        {
            if (Cmd != null)
                Cmd(this, cmd);
        }

        protected virtual void UpdateTitle() { }

        #region Persons

        /// <summary>
        /// Добавить пользователя в список
        /// </summary>
        /// <param name="name">Имя пользователя</param>
        /// <param name="raiseEvent">Нужно ли создать событие, информирующее о добавлении</param>
        public virtual void AddPerson(string name, bool raiseEvent)
        {
            lock (this.channelUsers.SyncRoot)
            {
                if (this.channelUsers.Any(user => user.NickName == name))
                    return;

                var channelUser = new ChannelUser(name);
                this.channelUsers.Add(channelUser);

                var channel = this.DataContext as CIRCeChannel;
                if (channel != null)
                    ((Changeable<ChannelUserInfo>)channel.Users).Add(channelUser.ToNew());

                SortUsersList();
                UpdateTitle();

                if (raiseEvent && PersonJoined != null)
                    PersonJoined(this, new PersonEventArgs(name));
            }
        }

        protected void SortUsersList()
        {
            lock (this.channelUsers.SyncRoot)
            {
                this.channelUsers.Sort((cu1, cu2) => cu1.CompareTo(cu2));
            }
            MyResetBindings();
        }

        protected virtual void MyResetBindings()
        {
            lock (this.dgvUsers.SyncRoot)
            {
                this.bsUsers.ResetBindings(false);
                this.dgvUsers.CurrentCell = null;

                int me = -1;
                lock (this.channelUsers.SyncRoot)
                {
                    me = this.channelUsers.FindIndex(cu => cu.NickName == this.OwnerServerWindow.Nick);
                }

                if (me > -1)
                {
                    var selfStyle = this.dgvUsers.Rows[me].Cells[0].Style;
                    selfStyle.ForeColor = UISettings.Default.SelfOnChannelForeColor;
                    selfStyle.BackColor = UISettings.Default.SelfOnChannelBackColor;
                }
            }
        }

        /// <summary>
        /// Содержит ли имя пользователя
        /// </summary>
        /// <param name="name">Имя</param>
        /// <returns></returns>
        public bool HasPerson(string name)
        {
            lock (this.channelUsers.SyncRoot)
            {
                return this.channelUsers.Any(cu => cu.NickName == name);
            }
        }

        /// <summary>
        /// Поменять имя пользователя
        /// </summary>
        /// <param name="oldName">Старый ник</param>
        /// <param name="newName">Новый ник</param>
        public virtual void RenamePerson(string oldName, string newName)
        {
            lock (this.channelUsers.SyncRoot)
            {
                var channelUser = this.channelUsers.FirstOrDefault(cu => cu.NickName == oldName);
                if (channelUser != null)
                {
                    channelUser.NickName = newName;

                    var channel = this.DataContext as CIRCeChannel;
                    if (channel != null)
                        channel.Users.First(cui => cui.NickName == oldName).NickName = newName;

                    SortUsersList();
                }
            }
        }

        /// <summary>
        /// Удалить пользователя
        /// </summary>
        /// <param name="name">Ник пользователя</param>
        public void RemovePerson(string name, bool raiseEvent)
        {
            lock (this.channelUsers.SyncRoot)
            {
                var channelUser = this.channelUsers.FirstOrDefault(cu => cu.NickName == name);
                if (channelUser != null)
                {
                    this.channelUsers.Remove(channelUser);

                    var channel = this.DataContext as CIRCeChannel;
                    if (channel != null)
                        ((Changeable<ChannelUserInfo>)channel.Users).Remove(channel.Users.First(cui => cui.NickName == name));

                    UpdateTitle();
                    MyResetBindings();
                }
            }

            if (raiseEvent && PersonLeaved != null)
                PersonLeaved(this, new PersonEventArgs(name));
        }

        /// <summary>
        /// Удалить всех
        /// </summary>
        public void ClearPersons()
        {
            lock (this.channelUsers.SyncRoot)
            {
                this.channelUsers.Clear();
            }
            MyResetBindings();
        }

        #endregion

        protected override void JoinChannel(object sender, EventArgs e)
        {
            mainWindow.JoinChannel(this.ServerWindow.Server.Channels[this.tSMIChannels.DropDownItems.IndexOf((ToolStripItem)sender)].Name);
        }

        public override void JoinChannel(string channel)
        {
            mainWindow.JoinChannel(channel);
        }

        private TextStyleSaver nickSaver = new TextStyleSaver();

        private bool IsNickChar(char c, Direction direction)
        {
            return Char.IsLetterOrDigit(c) || c == '-' || c == '[' || c == ']' || c == '\\' || c == '\'' || c == '^' || c == '{' || c == '}' || c == '_' || c == '|' || c == '`' || c == '\u00A0';
        }

        private bool IsNick(string s)
        {
            return true;
        }

        protected override void Log(string text)
        {
            Log(text, true, true, Settings.Default.Colors.DefForeColor);
        }

        protected override void Log(string text, Color defColor)
        {
            Log(text, true, true, defColor);
        }

        protected void Log(string text, bool newLine, bool enCode)
        {
            Log(text, newLine, enCode, Settings.Default.Colors.DefForeColor);
        }

        /// <summary>
        /// Записать текст в лог
        /// </summary>
        /// <param name="text">Записываемый текст</param>
        protected void Log(string text, bool newLine, bool enCode, Color defColor)
        {
            if (this.log == null)
                return;

            try
            {
                switch (this.logMode)
                {
                    case LogMode.None:
                        return;

                    case LogMode.Txt:
                        var logText = new FormattedText(text, 1, 0);
                        if (newLine)
                            log.WriteLine(logText.Text);
                        else
                            log.Write(logText.Text);
                        break;

                    case LogMode.Rtf:
                        StringBuilder res = new StringBuilder();
                        bool bold = false, underlined = false, colored = false;
                        string html = text;
                        int ind = Settings.Default.Colors.IndexOf(defColor);
                        int yind = 0;
                        if (ind == -1) ind = 1;
                        if (enCode) res.AppendFormat(@"\cf{0}\chshdng0\chcbpat0 ", ind);
                        logText = new FormattedText(html, ind, 0);
                        for (int i = 0; i < logText.Text.Length; i++)
                        {
                            bool addSpace = false;
                            if (logText.Bold.Contains(i))
                            {
                                bold = !bold;
                                res.Append(bold ? @"\b" : @"\b0");
                                addSpace = true;
                            }
                            if (logText.Under.Contains(i))
                            {
                                underlined = !underlined;
                                res.Append(underlined ? @"\ul" : @"\ul0");
                                addSpace = true;
                            }
                            if (logText.Colors.ContainsKey(i) && (logText.Colors[i].ForegroundColorCode != ind || logText.Colors[i].BackgroundColorCode != yind && logText.Colors[i].BackgroundColorCode != -1))
                            {
                                colored = true;
                                var x = logText.Colors[i].ForegroundColorCode;
                                int y = logText.Colors[i].BackgroundColorCode;
                                if (x != -1)
                                    res.AppendFormat(@"\cf{0}", x);
                                if (y != -1)
                                    res.AppendFormat(@"\chshdng0\chcbpat{0}", y);
                                addSpace = true;
                                ind = x;
                                yind = y;
                            }
                            else if (i == 0 && ind != 1)
                            {
                                colored = true;
                                res.AppendFormat(@"\cf{0}", ind);
                                addSpace = true;
                            }
                            if (addSpace)
                                res.Append(' ');
                            if (enCode && (logText.Text[i] == '{' || logText.Text[i] == '}' || logText.Text[i] == '\\'))
                            {
                                res.Append('\\');
                            }
                            res.Append(logText.Text[i]);
                        }
                        if (bold) res.Append(@"\b0");
                        if (underlined) res.Append(@"\ul0");
                        this.log.Write(res);
                        if (newLine)
                            log.Write(@"\par");
                        break;

                    case LogMode.Html:
                    case LogMode.HtmlForum:
                        res = new StringBuilder();
                        bold = false; underlined = false; colored = false;
                        html = (this.logMode == LogMode.Html && enCode) ? WebUtility.HtmlEncode(text) : text;
                        ind = Settings.Default.Colors.IndexOf(defColor);
                        yind = 0;
                        if (ind == -1) ind = 1;
                        logText = new FormattedText(html, ind, 99);
                        for (int i = 0; i < logText.Text.Length; i++)
                        {
                            if (logText.Bold.Contains(i))
                            {
                                bold = !bold;
                                res.Append(bold ? "<b>" : "</b>");
                            }
                            if (logText.Under.Contains(i))
                            {
                                underlined = !underlined;
                                res.Append(underlined ? "<u>" : "</u>");
                            }
                            if (logText.Colors.ContainsKey(i) && (logText.Colors[i].ForegroundColorCode != ind || logText.Colors[i].BackgroundColorCode != yind && logText.Colors[i].BackgroundColorCode != -1))
                            {
                                if (colored) res.Append("</span>");
                                colored = true;
                                var x = logText.Colors[i].ForegroundColorCode;
                                int y = logText.Colors[i].BackgroundColorCode;
                                if (x != -1 && x != 1 && y != -1 && y != 99)
                                    res.AppendFormat("<span class=\"fc{0} bc{1}\">", x, y);
                                else if (x != -1 && x != 1)
                                    res.AppendFormat("<span class=\"fc{0}\">", x);
                                else if (y != -1 && y != 99)
                                    res.AppendFormat("<span class=\"bc{0}\">", y);
                                else
                                    colored = false;
                                ind = x;
                                yind = y;
                            }
                            else if (i == 0 && ind != 1)
                            {
                                colored = true;
                                res.AppendFormat("<span class=\"fc{0}\">", ind);
                            }

                            res.Append(logText.Text[i]);
                        }
                        if (bold) res.Append("</b>");
                        if (underlined) res.Append("</u>");
                        if (colored) res.Append("</span>");
                        this.log.Write(res);
                        if (newLine)
                            log.Write(this.logMode == LogMode.Html ? "<br/>" : Environment.NewLine);

                        logLength += res.Length;
                        if (logLength > 50000)
                        {
                            log.WriteLine(); log.WriteLine(); log.WriteLine();
                            this.logLength = 0;
                        }

                        break;

                    case LogMode.Forum:
                    case LogMode.Forum2:
                        {
                            res = new StringBuilder();
                            bold = false; underlined = false; colored = false;
                            bool back = false;
                            html = text;
                            ind = Settings.Default.Colors.IndexOf(defColor);
                            yind = 0;
                            if (ind == -1) ind = 1;
                            logText = new FormattedText(html, ind, 0);
                            for (int i = 0; i < logText.Text.Length; i++)
                            {
                                if (logText.Bold.Contains(i))
                                {
                                    bold = !bold;
                                    res.Append(bold ? "[b]" : "[/b]");
                                }
                                if (logText.Under.Contains(i))
                                {
                                    underlined = !underlined;
                                    res.Append(underlined ? "[u]" : "[/u]");
                                }

                                if (this.logMode == LogMode.Forum2)
                                {
                                    if (logText.Colors.ContainsKey(i) && (logText.Colors[i].ForegroundColorCode != ind || logText.Colors[i].BackgroundColorCode != yind && logText.Colors[i].BackgroundColorCode != -1))
                                    {
                                        if (back) res.Append("</span>");
                                        if (colored) res.Append("[/color]");                                        
                                        colored = true;

                                        var x = logText.Colors[i].ForegroundColorCode;
                                        if (x != -1)
                                            res.AppendFormat("[color={0}]",
                                                ColorToString(Settings.Default.Colors[x]));
                                        var y = logText.Colors[i].BackgroundColorCode;
                                        if (y != -1)
                                        {
                                            back = true;
                                            res.AppendFormat("<span style=\"background-color:{0}\">",
                                                ColorToString(Settings.Default.Colors[y]));
                                        }
                                        ind = x;
                                        yind = y;
                                    }
                                    else if (i == 0 && ind != 1)
                                    {
                                        colored = true;
                                        res.AppendFormat("[color={0}]", ColorToString(Settings.Default.Colors[ind]));
                                    }
                                }
                                else
                                {
                                    if (logText.Colors.ContainsKey(i) && logText.Colors[i].ForegroundColorCode != ind)
                                    {
                                        if (colored) res.Append("[/color]");
                                        colored = true;
                                        var x = logText.Colors[i].ForegroundColorCode;
                                        if (x != -1)
                                            res.AppendFormat("[color={0}]",
                                                ColorToString(Settings.Default.Colors[x]));
                                    }
                                    else if (i == 0 && ind != 1)
                                    {
                                        colored = true;
                                        res.AppendFormat("[color={0}]", ColorToString(Settings.Default.Colors[ind]));
                                    }
                                }

                                res.Append(logText.Text[i]);
                            }
                            if (bold) res.Append("[/b]");
                            if (underlined) res.Append("[/u]");
                            if (colored) res.Append("[/color]");
                            if (back) res.Append("</span>");
                            this.log.Write(res);
                            if (newLine)
                                log.WriteLine();
                            logLength += res.Length;
                            if (logLength > 50000)
                            {
                                log.WriteLine(); log.WriteLine(); log.WriteLine();
                                this.logLength = 0;
                            }
                        }
                        break;
                }

                log.Flush();
            }
            catch (Exception) { }
        }

        private static string ColorToString(Color c)
        {
            var res = new StringBuilder("#");
            res.Append(c.R.ToString("X2"));
            res.Append(c.G.ToString("X2"));
            res.Append(c.B.ToString("X2"));
            return res.ToString();
        }

        /// <summary>
        /// Пользователь ввёл текст и нажал Enter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void printMessageIrcRichTextBox_EditFinished(object sender, EnterPushedEventArgs e)
        {
            if (this.irtbPrintMessage == null)
                return;

            if (this.irtbPrintMessage.InvokeRequired)
            {
                var eventHandler = new EventHandler<EnterPushedEventArgs>(printMessageIrcRichTextBox_EditFinished);
                this.BeginInvoke(eventHandler, sender, e);
            }
            else
            {
                if (Cmd != null)
                {
                    if (string.IsNullOrWhiteSpace(e.Message))
                        return;

                    var msg = e.Message.Trim();

                    if (UISettings.Default.MessagesColor != 1 && msg[0] != Special.Color && msg[0] != Special.CmdStarter)
                    {
                        msg = string.Format("{1}{0:D2}{2}", UISettings.Default.MessagesColor, Special.Color, msg);
                    }

                    Cmd(this, msg);
                }

                this.irtbPrintMessage.Clear();
            }
        }

        private void MDIChildCommunication_KeyDown(object sender, KeyEventArgs e)
        {
            string message;
            if (UserOptions.Default.HotKeys.TryGetValue(e.KeyCode, out message))
                printMessageIrcRichTextBox_EditFinished(irtbPrintMessage, new EnterPushedEventArgs(message));
        }

        protected override void ChangeNick(object sender, EventArgs e)
        {
            mainWindow.SetNick(sender.ToString());
        }

        #region ICommunicationWindow Members

        /// <summary>
        /// Окно-владелец сервера
        /// </summary>
        public MDIChildServer OwnerServerWindow
        {
            get { return mainWindow; }
        }

        public virtual string WindowName
        {
            get { return this.Text; }
        }

        public void Action(string actionText)
        {
            ;
        }

        public event EventHandler<IRCProviders.MessageEventArgs> MessageReceived;

        #endregion

        private static readonly Regex PlayRegex = new Regex(Special.Color + @"\d\d?(\,\d\d?)?\s*play\s+(((?<file>[^\.]*\.(mp3|wav))(\:(?<arg>.+))?\s*((?<file2>[^\.]*\.(mp3|wav))(\:(?<arg2>.+))?)?.*)|((?<command>[^:]+)(\:(?<arg>.+))?))\s*$", RegexOptions.Compiled);
        private static readonly Regex UrlRegex = new Regex(Special.Color + @"\d\d?(\,\d\d?)?\s*url\s+(?<url>.*)", RegexOptions.Compiled);

        /// <summary>
        /// Записать в чат обычное сообщение
        /// </summary>
        /// <param name="name">Отправитель</param>
        /// <param name="text">Текст сообщения</param>
        public void PutTextMessage(string name, string text)
        {
            PutMessage(string.Format("<{0}> {1}", name, text));

            bool good;
            lock (this.channelUsers.SyncRoot)
            {
                good = this.channelUsers.Any(cu => cu.NickName == name && cu.Modes.ContainsKey('@') && cu.Modes['@']);
            }

            // Подключение саундов по временной схеме
            if (UISettings.Default.PlayMusicExt == PlayMode.All
                || UISettings.Default.PlayMusicExt == PlayMode.OpOnly
                && good)
            {
                var match = PlayRegex.Match(text.ToLower());
                if (match.Success)
                {
                    var command = match.Groups["command"].Value;
                    if (command.Length > 0)
                    {
                        var arg = match.Groups["arg"].Value;
                        switch (command)
                        {
                            case "pause":
                                this.ServerWindow.Execute(null, "media", "pause");
                                break;

                            case "resume":
                                this.ServerWindow.Execute(null, "media", "resume");
                                break;

                            case "seek":
                                this.ServerWindow.Execute(null, "media", "seek", arg);
                                break;

                            case "stop":
                                this.ServerWindow.Execute(null, "media", "stop", "0");
                                this.ServerWindow.Execute(null, "media", "stop", "1");
                                break;

                            case "mp3":
                                if (arg == "stop")
                                    this.ServerWindow.Execute(null, "media", "stop", "0");
                                break;

                            case "wav":
                                if (arg == "stop")
                                    this.ServerWindow.Execute(null, "media", "stop", "1");
                                break;

                            default:
                                switch (arg)
                                {
                                    case "loop":
                                        this.ServerWindow.Execute(null, "media", "loop", command);
                                        break;

                                    default:
                                        ulong pos = 0;
                                        if (ulong.TryParse(arg, out pos))
                                            this.ServerWindow.Execute(null, "media", "pos", command, arg);
                                        break;
                                }
                                break;
                        }
                    }
                    else
                    {
                        var arg = match.Groups["arg"].Value;
                        if (arg.Length > 0)
                        {
                            switch (arg)
                            {
                                case "loop":
                                    this.ServerWindow.Execute(null, "media", "loop", match.Groups["file"].Value);
                                    break;

                                default:
                                    ulong pos = 0;
                                    if (ulong.TryParse(arg, out pos))
                                        this.ServerWindow.Execute(null, "media", "pos", match.Groups["file"].Value, arg);
                                    break;
                            }
                        }
                        else
                        {
                            if (match.Groups["file2"].Value.Length > 0)
                                this.ServerWindow.Execute(this.WindowName, "splay", match.Groups["file"].Value, match.Groups["file2"].Value);
                            else
                                this.ServerWindow.Execute(this.WindowName, "splay", match.Groups["file"].Value);
                        }
                    }
                }
            }

            bool good2;
            lock (this.channelUsers.SyncRoot)
            {
                good2 = this.channelUsers.Any(cu => cu.NickName == name && cu.Modes.ContainsKey('@') && cu.Modes['@']);
            }

            // Скачивать и показывать мультимедиа
            if (UISettings.Default.UrlExt == PlayMode.All
                || UISettings.Default.UrlExt == PlayMode.OpOnly
                && good2)
            {
                var match = UrlRegex.Match(text);
                if (match.Success)
                {
                    this.ServerWindow.Execute(this.WindowName, "url", match.Groups["url"].Value.Trim());
                }
            }

            if (MessageReceived != null)
                MessageReceived(this, new IRCProviders.MessageEventArgs(text, name));
        }

        private void MDIChildCommunication_Load(object sender, EventArgs e)
        {
            if (mainWindow != null)
            {
                //this.dgvUsers.Columns["VisibleNick"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                //this.dgvUsers.Columns["NickName"].Visible = false;
                //this.dgvUsers.Columns["Modes"].Visible = false;

                string logDir = Path.Combine("Logs", mainWindow.Server.Name);
                Program.DataStorage.CreateDirectory(logDir);

                if (this.logMode != LogMode.None)
                {
                    try
                    {
                        string ext = string.Empty;
                        var invalid = Path.GetInvalidFileNameChars();
                        var fileName = WindowName;
                        foreach (var item in invalid)
                        {
                            fileName = fileName.Replace(item, '_');
                        }

                        switch (this.logMode)
                        {
                            case LogMode.Txt:
                            case LogMode.Forum:
                            case LogMode.Forum2:
                            case LogMode.HtmlForum:
                                var logFile = String.Format("{0}.log", fileName);                                
                                string path = Path.Combine(logDir, logFile);
                                this.log = Program.DataStorage.AppendText(path, Encoding.GetEncoding(1251));

                                Log("");
                                Log(string.Format(Resources.LogStarted, DateTime.Now));
                                log.Flush();
                                break;

                            case LogMode.Rtf:
                                path = Path.Combine(logDir, String.Format("{0}.{1:dd.MM.yyyy_hh.mm.ss}.rtf", fileName, DateTime.Now));
                                this.log = Program.DataStorage.AppendText(path, Encoding.GetEncoding(1251));

                                Log("{\\rtf1\\ansi\\ansicpg1251\\deff0\\deflang1049", false, false);
                                Log("{\\fonttbl{\\f0\\fnil\\fcharset204{\\*\\fname Courier New;}Courier New CYR;}}", false, false);
                                Log(Settings.Default.Colors.CreateRtfColorTable(), false, false);
                                Log("\r\n\\viewkind4\\uc1\\pard\\f0\\fs20", false, false);
                                Log(string.Format(Resources.LogStarted, DateTime.Now));
                                log.Flush();
                                break;

                            case LogMode.Html:
                                path = Path.Combine(logDir, String.Format("{0}.{1:dd.MM.yyyy_hh.mm.ss}.html", fileName, DateTime.Now));
                                this.log = Program.DataStorage.AppendText(path, Encoding.GetEncoding(1251));

                                var style = new StringBuilder();
                                for (int i = 0; i < Settings.Default.Colors.Length; i++)
                                {
                                    var colorString = ColorToString(Settings.Default.Colors[i]);
                                    style.AppendFormat(".fc{0} {{color:{1};}} ", i, colorString)
                                        .AppendFormat(".bc{0} {{background-color:{1};}} ", i, colorString);
                                }

                                Log(String.Format("<html><head><title>{0}</title><style type=\"text/css\">{1}</style></head><body>", this.WindowName, style), false, false);
                                Log(string.Format(Resources.LogStarted, DateTime.Now));
                                Log("\r\n\r\n\r\n");
                                log.Flush();
                                break;

                            default:
                                break;
                        }
                    }
                    catch (IOException)
                    {
                    }
                }
            }
        }

        private void MDIChildCommunication_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (log != null)
            {
                if (logMode == LogMode.Html)
                    Log("\r\n\r\n\r\n");
                Log(string.Format(Resources.LogFinished, DateTime.Now));
                switch (this.logMode)
                {
                    case LogMode.Rtf:
                        Log("\r\n}\r\n", false, false);
                        break;

                    case LogMode.Html:
                        Log("</body></html>", false, false);
                        break;

                    default:
                        break;
                }
                log.Close();
            }
        }

        /// <summary>
        /// Записать полученное сообщение от ника в окно как если бы оно на самом деле было получено
        /// </summary>
        /// <param name="nick"></param>
        /// <param name="msg"></param>
        public void InvokeReceive(string nick, string msg)
        {
            PutTextMessage(nick, msg);
            if (MessageReceived != null)
                MessageReceived(this, new IRCProviders.MessageEventArgs(msg, nick));
        }

        #region ICommunicationWindow Members

        public event EventHandler<PersonEventArgs> PersonJoined;

        public event EventHandler<PersonEventArgs> PersonLeaved;

        #endregion

        private void MDIChildCommunication_Activated(object sender, EventArgs e)
        {
            if (this.myNode != null) this.MyNode.ForeColor = Color.MidnightBlue;
        }

        private void chatRTB_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                return;

            int left = 0, right = 0;
            bool b = GetToken(chatRTB, e.Location, out left, out right, IsNickChar, IsNick);

            if (nickSaver.Left != -1 && (!b || left + 1 != nickSaver.Left))
            {
                chatRTB.SelectionStart = nickSaver.Left;
                chatRTB.SelectionLength = nickSaver.Right - nickSaver.Left;

                nickSaver.Paste();
                chatRTB.SelectionLength = nickSaver.Right - nickSaver.Left;
                chatRTB.SelectionStart = nickSaver.Left;
                chatRTB.SelectedText = chatRTB.SelectedText.Replace(' ', '\u00A0');
                chatRTB.SelectionLength = 0;

                nickSaver.Left = -1;
            }

            if (b && (nickSaver.Left == -1 || nickSaver.Left != left + 1))
            {
                string name = chatRTB.Text.Substring(left + 1, right - left - 1);
                if (HasPerson(name))
                {
                    HighLight(chatRTB, left + 1, right, nickSaver);
                }
            } 
        }

        private void chatRTB_MouseClick(object sender, MouseEventArgs e)
        {
            int left = 0, right = 0;
            bool b = GetToken(chatRTB, e.Location, out left, out right, IsNickChar, IsNick);
            if (b) // Выделено имя (юзера?)
            {
                string name = chatRTB.Text.Substring(left + 1, right - left - 1);
                if (HasPerson(name))
                {
                    this.irtbPrintMessage.RichTextBox.SelectedText = String.Format("{0}, ", name);
                    //this.irtbPrintMessage.Text += name + ", ";
                    this.irtbPrintMessage.SelectionStart += this.irtbPrintMessage.SelectionLength;//this.irtbPrintMessage.Text.Length;
                    this.irtbPrintMessage.SelectionLength = 0;
                    this.irtbPrintMessage.SetFocus();

                    return;
                }
            }

            var viewModel = (CIRCeSession)this.DataContext;
            if (viewModel.HasChatListeners())
            {
                int index = chatRTB.GetCharIndexFromPosition(e.Location);
                //int lindex = chatRTB.GetLineFromCharIndex(index);
                int start = index;//chatRTB.GetFirstCharIndexFromLine(lindex);
                while (start > 0 && chatRTB.Text[start] != '\n')
                    start--;
                int end = index;// chatRTB.GetFirstCharIndexFromLine(lindex + 1);
                while (end < chatRTB.TextLength && chatRTB.Text[end] != '\n')
                    end++;
                var relIndex = index;
                var line = IRCRichTextBox.GenerateCodes(chatRTB, ref relIndex, start, end + 1, false)[0];
                viewModel.OnChatClicked(line, relIndex);
            }
        }

        public void Quit()
        {
            var msg = UISettings.Default.QuitMessage;
            RaiseCmd(string.Format("/QUIT{0}", !string.IsNullOrWhiteSpace(msg) ? " :" + msg : ""));
        }
    }
}
