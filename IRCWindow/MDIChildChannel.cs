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
//using ObjectEditors;
using System.Runtime.InteropServices;
using CIRCe.Base;
using IRCWindow.ViewModel;
using System.Linq;
using IRCWindow.View;
using IRCWindow.ViewModel.Common;

namespace IRCWindow
{
    internal partial class MDIChildChannel : MDIChildCommunication
    {
        // Название канала/Имя юзера в привате
        protected string winName = string.Empty;

        private Dictionary<char, ChannelMode> modes = new Dictionary<char, ChannelMode>();
        private string password = string.Empty;
        private int limit = -1;
        private List<MaskInfo> bans = new List<MaskInfo>();
        private List<MaskInfo> exempts = new List<MaskInfo>();
        private List<MaskInfo> invite = new List<MaskInfo>();

        private event CmdDel MyCmd;

        public delegate void OpenPrivDel(string name);
        public event OpenPrivDel OpenPrivate;

        public override string ReceiverName
        {
            get
            {
                return this.winName;
            }
        }

        /// <summary>
        /// Заголовок канала
        /// </summary>
        public string Topic
        {
            get
            {
                return irtbTopic.Text;
            }
            set
            {
                irtbTopic.Text = value;
                irtbTopic.DefaultColor = settings.Colors.DefForeColor;
                PutMessage(String.Format("{0}: {1}", Resources.ChannelTopic, value), settings.Colors[3]);
                if (ChannelTopicChanged != null)
                    ChannelTopicChanged(this, new EventArgs());
            }
        }

        private Channel channel = null;

        public IChannel Channel { get { return this.channel; } }

        ToolStripMenuItem tSMIchannelModes = null;

        private bool setWhoisHandler = false;

        public override string Id
        {
            get
            {
                return string.Format("{0}_{1}", this.ServerWindow.Server.Name, this.WindowName);
            }
        }

        public string FullChannelName
        {
            get
            {
                return string.Format("{0} [{1}] / {2}", this.ServerWindow.Server.Name, this.ServerWindow.Nick, this.WindowName);
            }
        }

        private bool NeedUpdateWhois
        {
            set
            {
                if (value && !this.setWhoisHandler)
                {
                    this.ServerWindow.WhoisUpdated += new EventHandler<PersonEventArgs>(ServerWindow_WhoisUpdated);
                    this.setWhoisHandler = true;
                }
                else if (this.setWhoisHandler)
                {
                    this.ServerWindow.WhoisUpdated -= new EventHandler<PersonEventArgs>(ServerWindow_WhoisUpdated);
                    this.setWhoisHandler = false;
                }
            }
        }

        public MDIChildChannel()
        {
            InitializeComponent();
        }

        public MDIChildChannel(MDIParent main)
            : base(main)
        {
            InitializeComponent();
        }

        public MDIChildChannel(MDIParent main, MDIChildServer mainWindow, Channel channel, Dictionary<string, UserInfo> whois)
            :base(main, mainWindow, channel.Name, whois)
        {
            InitializeComponent();

            this.channel = channel;

            this.Size = UISettings.Default.IRCWindowSize;

            this.dgvUsers.ContextMenuStrip = new IRCContextMenuStrip();

            this.dgvUsers.ContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.tSMICtcp,
                this.toolStripMenuItem6,
                this.tsmiHalfOp,
                this.toolStripMenuItem9,
                this.toolStripSeparator2,
                this.tsmiCall,
                this.tsmiInvite,
                this.toolStripSeparator3,
                this.toolStripMenuItem13,
                this.tSMIBan,
                this.tsmiKickAndBan
            });

            this.dgvUsers.ContextMenuStrip.Opened += (sender, e) => NeedUpdateWhois = false;

            this.winName = this.Channel.Name;
            this.Text = this.Channel.Name + " [0] []";

            this.MyCmd += new CmdDel(MDIChildChannel_MyCmd);
            
            this.contextMenuStripChat.Items.Add(new ToolStripSeparator());
            this.tSMIchannelModes = new ToolStripMenuItem(Resources.ChannelModes);
            this.contextMenuStripChat.Items.Add(tSMIchannelModes);

            modes['m'] = new ChannelMode('m', Resources.ModeM, ChannelModeHandler, tSMIchannelModes);
            tSMIchannelModes.DropDownItems.Add(new ToolStripSeparator());
            modes['i'] = new ChannelMode('i', Resources.ModeI, ChannelModeHandler, tSMIchannelModes);
            modes['k'] = new ChannelMode('k', Resources.ModeK, ChannelModeHandler, tSMIchannelModes);
            modes['l'] = new ChannelMode('l', Resources.ModeL, ChannelModeHandler, tSMIchannelModes);
            tSMIchannelModes.DropDownItems.Add(new ToolStripSeparator());
            modes['b'] = new ChannelMode('b', Resources.ModeB, ChannelModeHandler, tSMIchannelModes);
            modes['e'] = new ChannelMode('e', Resources.ModeE, ChannelModeHandler, tSMIchannelModes);
            modes['I'] = new ChannelMode('I', Resources.ModeBigI, ChannelModeHandler, tSMIchannelModes);
            tSMIchannelModes.DropDownItems.Add(new ToolStripSeparator());
            modes['t'] = new ChannelMode('t', Resources.ModeT, ChannelModeHandler, tSMIchannelModes);
            modes['n'] = new ChannelMode('n', Resources.ModeN, ChannelModeHandler, tSMIchannelModes);
            modes['c'] = new ChannelMode('c', Resources.ModeC, ChannelModeHandler, tSMIchannelModes);
            modes['c'] = new ChannelMode('S', Resources.ModeBigS, ChannelModeHandler, tSMIchannelModes);
            tSMIchannelModes.DropDownItems.Add(new ToolStripSeparator());
            modes['p'] = new ChannelMode('p', Resources.ModeP, ChannelModeHandler, tSMIchannelModes);
            modes['s'] = new ChannelMode('s', Resources.ModeS, ChannelModeHandler, tSMIchannelModes);
            modes['a'] = new ChannelMode('a', Resources.ModeA, ChannelModeHandler, tSMIchannelModes);

            this.dgvUsers.CellMouseDoubleClick += dgvUsers_CellMouseDoubleClick;
            this.dgvUsers.CellMouseEnter += dgvUsers_CellMouseEnter;
            this.dgvUsers.CellMouseLeave += (sender, e) => { currentRow = -1; NeedUpdateWhois = false; };
            this.dgvUsers.MouseLeave += (sender, e) => { currentRow = -1; NeedUpdateWhois = false; this.userInfoToolTip1.Hide(this.dgvUsers); };

            this.tsmiStick.Checked = this.Channel.Sticked;
            this.tsmiAutoOpen.Checked = this.Channel.AutoOpen;

            this.tsmiStick.CheckedChanged += (sender, e) =>
            {
                this.Channel.Sticked = this.tsmiStick.Checked;
                if (this.Channel.Sticked)
                    this.ServerWindow.Server.Sticked = true;
            };

            this.tsmiAutoOpen.CheckedChanged += (sender, e) => this.Channel.AutoOpen = this.tsmiAutoOpen.Checked;

            this.Channel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "Sticked")
                {
                    this.tsmiStick.Checked = this.Channel.Sticked;
                    this.myNode.Sticked = this.Channel.Sticked;
                    if (this.Channel.Sticked)
                        this.MyNode.Tag = this.Channel;
                }
            };
        }

        internal override void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.Settings_PropertyChanged(sender, e);
            if (e.PropertyName == "TopicBackColor" && this.irtbTopic.BackColor != UISettings.Default.TopicBackColor)
                this.irtbTopic.BackColor = UISettings.Default.TopicBackColor;
            else if (e.PropertyName == "TopicFont" && this.irtbTopic.InnerFont != UISettings.Default.TopicFont)
                this.irtbTopic.InnerFont = UISettings.Default.TopicFont;
            /*else if (e.PropertyName == "Language")
            {
                var resources = new System.ComponentModel.ComponentResourceManager(this.GetType());
                MDIParent.ApplyResource(resources, this);
            }*/
        }

        int currentRow = -1;

        void dgvUsers_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            lock (whoisLock)
            {
                string nick;
                lock (this.channelUsers.SyncRoot)
                {
                    if (e.RowIndex >= this.channelUsers.Count)
                        return;

                    nick = this.channelUsers[e.RowIndex].NickName;
                }

                var has = whois.ContainsKey(nick) && whois[nick] != null;
                currentRow = e.RowIndex;
                var x = -this.userInfoToolTip1.Size.Width - 10;

                int y;
                lock (this.dgvUsers.SyncRoot)
                {
                    if (this.dgvUsers.RowCount == 0)
                        return;

                    y = Math.Min((e.RowIndex - this.dgvUsers.FirstDisplayedScrollingRowIndex) * this.dgvUsers.Rows[0].Height,
                        Screen.PrimaryScreen.Bounds.Height - this.userInfoToolTip1.Size.Height - this.dgvUsers.PointToScreen(Point.Empty).Y);
                }

                if (has && !string.IsNullOrEmpty(whois[nick].Nick))
                {
                    userInfoToolTip1.UseObject(whois[nick]);
                    userInfoToolTip1.Show("show", this.dgvUsers, x, y, 3000);
                }
                else
                {
                    userInfoToolTip1.Show("wait", this.dgvUsers, x, y, 3000);
                }

                if (!has || whois[nick].Obsolete == 0)
                {
                    whois[nick] = new UserInfo();
                    whois[nick].Obsolete = -1;
                    MyCmd(this, String.Format("/WHOIS {0}", nick));
                    NeedUpdateWhois = true;
                }
            }
        }

        void ServerWindow_WhoisUpdated(object sender, PersonEventArgs e)
        {
            if (currentRow == -1)
            {
                NeedUpdateWhois = false;
                return;
            }

            string nick;
            lock (this.channelUsers.SyncRoot)
            {
                if (currentRow >= this.channelUsers.Count)
                    return;

                nick = this.channelUsers[currentRow].NickName;
            }

            if (nick == e.NickName)
            {
                this.userInfoToolTip1.UseObject(whois[nick]);

                var y = Math.Min((currentRow - this.dgvUsers.FirstDisplayedScrollingRowIndex) * this.dgvUsers.Rows[0].Height,
                    Screen.PrimaryScreen.Bounds.Height - this.userInfoToolTip1.Size.Height - this.dgvUsers.PointToScreen(Point.Empty).Y);
                userInfoToolTip1.Show("show", this.dgvUsers, -300, y, 3000);
                NeedUpdateWhois = false;
            }
        }

        void dgvUsers_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            lock (this.channelUsers.SyncRoot)
            {
                if (OpenPrivate != null && e.RowIndex < this.channelUsers.Count)
                {
                    var name = this.channelUsers[e.RowIndex].NickName;
                    if (name != ((MDIChildServer)((WindowNode)myNode.Parent).Window).MyUser)
                        OpenPrivate(name);
                }
            }
        }

        /// <summary>
        /// Уставновить режим канала
        /// </summary>
        /// <param name="mode">Режим</param>
        /// <param name="param">Дополнительные параметры</param>
        private void ChannelMode(string mode, params string[] param)
        {
            if (MyCmd != null)
                MyCmd(this, String.Format("/MODE {0} {1}{2}", this.winName, mode, (param.Length > 0 ? String.Format(" {0}", string.Join(" ", param)) : string.Empty)));
        }

        private void ChannelModeHandler(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            ChannelMode mode = item.Tag as ChannelMode;

            switch (mode.Letter)
            {
                case 'b':
                    bans.Clear();
                    ChannelMode("+b");
                    break;

                case 'e':
                    exempts.Clear();
                    ChannelMode("+e");
                    break;

                case 'I':
                    invite.Clear();
                    ChannelMode("+I");
                    break;

                case 'k':
                    if (!item.Checked)
                    {
                        StringEnterDialog diag = new StringEnterDialog(Resources.SetPassword);
                        if (diag.ShowDialog() == DialogResult.OK && diag.PrintedText.Length > 0)
                            ChannelMode("+k", diag.PrintedText);
                    }
                    else
                        ChannelMode("-k", this.password);
                    break;

                case 'l':
                    if (!item.Checked)
                    {
                        InputNumDialog diag = new InputNumDialog(Resources.EnterLimit);
                        if (diag.ShowDialog() == DialogResult.OK && diag.Value > 0)
                            ChannelMode("+l", diag.Value.ToString());
                    }
                    else
                        ChannelMode("-l");
                    break;

                default:
                    ChannelMode((!item.Checked ? "+" : "-") + mode.Letter);
                    break;
            }
        }

        void MDIChildChannel_MyCmd(MDIChildCommunication child, string cmd)
        {
            RaiseCmd(cmd);
        }

        void irtbTopic_EditFinished(object sender, EnterPushedEventArgs e)
        {
            irtbTopic.ReadOnly = true;
            irtbTopic.Rollback();
            SetTopic(e.Message.Trim());
            this.irtbPrintMessage.Focus();
        }

        void MDIChildChannel_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            try
            {
                this.userInfoToolTip1.Hide(this.dgvUsers);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                throw;
            }
        }

        void MDIChildChannel_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.userInfoToolTip1.Dispose();
            this.NeedUpdateWhois = false;
            if (MyCmd != null)
                MyCmd(this, String.Format("/PART {0}", this.WindowName));

            if (this.MyNode.Sticked)
            {
                this.MyNode.Text = this.Channel.Name;
                this.MyNode.ForeColor = Color.PaleVioletRed;
            }
        }

        private void pingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lock (this.dgvUsers.SyncRoot)
            {
                lock (this.channelUsers.SyncRoot)
                {
                    foreach (DataGridViewRow item in this.dgvUsers.SelectedRows)
                    {
                        int pid = Program.Rand.Next();
                        (mainWindow as MDIChildServer).SaveTimeForPing(this.channelUsers[item.Index].NickName, pid);
                        mainWindow.Send(this.channelUsers[item.Index].NickName, Special.Ctcp + "PING " + pid.ToString() + Special.Ctcp, true);
                    }
                }
            }
        }

        private void timeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ctcp("TIME");
        }

        private void versionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ctcp("VERSION");
        }

        private void tSMIUserInfo_Click(object sender, EventArgs e)
        {
            Ctcp("USERINFO");
        }

        private void Ctcp(string message)
        {
            lock (this.dgvUsers.SyncRoot)
            {
                lock (this.channelUsers.SyncRoot)
                {
                    foreach (DataGridViewRow item in this.dgvUsers.SelectedRows)
                    {
                        mainWindow.Send(this.channelUsers[item.Index].NickName, Special.Ctcp + message + Special.Ctcp, true);
                    }
                }
            }
        }

        private void callToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lock (this.dgvUsers.SyncRoot)
            {
                lock (this.channelUsers.SyncRoot)
                {
                    foreach (DataGridViewRow item in this.dgvUsers.SelectedRows)
                    {
                        this.irtbPrintMessage.Text += String.Format("{0}, ", this.channelUsers[item.Index].NickName);
                        this.irtbPrintMessage.SelectionStart = this.irtbPrintMessage.Text.Length;
                        this.irtbPrintMessage.SetFocus();
                    }
                }
            }
        }

        /// <summary>
        /// Обновить заголовок окна канала
        /// </summary>
        protected override void UpdateTitle()
        {
            var add = new StringBuilder(winName);
            add.Append(" [");
            add.Append(this.channelUsers.Count);
            add.Append("] [");
            var param = new StringBuilder();
            foreach (var item in modes)
                if (item.Value.Set)
                {
                    if (add.Length == 0)
                        add.Append('+');
                    add.Append(item.Key);
                    switch (item.Key)
                    {
                        case 'k':
                            param.Append(' ');
                            param.Append(this.password);
                            break;

                        case 'l':
                            param.Append(' ');
                            param.Append(this.limit);
                            break;

                        default:
                            break;
                    }
                }
            add.Append(param.ToString());
            add.Append(']');
            this.Text = add.ToString();
        }

        /// <summary>
        /// Установлен режим канала
        /// </summary>
        /// <param name="p">Режимы</param>
        protected internal void Mode(string name, string p, params string[] param)
        {
            if (p.Length < 2)
                return;
            bool add = p[0] == '+';
            int paramInd = 0;
            for (int i = 1; i < p.Length; i++)
            {
                if (p[i] == '+')
                {
                    add = true;
                    continue;
                }
                else if (p[i] == '-')
                {
                    add = false;
                    continue;
                }

                switch (p[i])
                {
                    case 'b':
                        break;

                    case 'e':
                        break;

                    case 'I':
                        break;

                    case 'h':
                        goto case 'v';

                    case 'k':
                        if (paramInd < param.Length)
                            password = param[paramInd++];
                        goto default;

                    case 'l':
                        if (add && paramInd < param.Length)
                            limit = int.Parse(param[paramInd++]);
                        goto default;

                    case 'o':
                        goto case 'v';

                    case 'v':
                        string person = param[paramInd++];
                        lock (this.channelUsers.SyncRoot)
                        {
                            int ind = this.channelUsers.FindIndex(cu => cu.NickName == person);
                            if (ind > -1)
                            {
                                this.channelUsers[ind].SetMode(MDIChildServer.ModesTable2[p[i]], add);
                                if (person == this.ServerWindow.Nick)
                                {
                                    this.tSMIchannelModes.Enabled = this.channelUsers[ind].Modes.ContainsKey('@') && this.channelUsers[ind].Modes['@'];
                                    this.tSMIchannelModes.DropDown.Enabled = this.tSMIchannelModes.Enabled;
                                }
                                SortUsersList();
                            }
                        }
                        break;

                    default:
                        if (!modes.ContainsKey(p[i]))
                            modes[p[i]] = new ChannelMode(p[i], string.Empty, null, null);
                        modes[p[i]].Set = add;
                        break;
                }
            }
            UpdateTitle();
            PutMessage(String.Format("{0} {1} {2} {3}", name, Resources.SetsMode, p, string.Join(" ", param)), settings.Colors[3]);
            if (ChannelModesChanged != null)
                ChannelModesChanged(this, new EventArgs());
        }

        /// <summary>
        /// Утсановлен режим ника на канале
        /// </summary>
        /// <param name="p">Режим</param>
        /// <param name="persons">Для кого установлен</param>
        protected internal void UserMode(string name, string p, string[] persons)
        {
            PutMessage(String.Format("{0} {1} {2} {3} {4}", name, Resources.SetsMode, p, Resources.For, string.Join(" ", persons)), settings.Colors[3]);

            foreach (string person in persons)
            {
                lock (this.dgvUsers.SyncRoot)
                {
                    lock (this.channelUsers.SyncRoot)
                    {
                        foreach (DataGridViewRow item in this.dgvUsers.SelectedRows)
                            if (this.channelUsers[item.Index].NickName == person)
                            {
                                this.channelUsers[item.Index].SetMode(MDIChildServer.ModesTable2[p[1]], p[0] == '+');
                                if (PersonModesChanged != null)
                                    PersonModesChanged(this, new PersonEventArgs(person));
                                break;
                            }
                    }
                }
            }
            SortUsersList();
        }

        public override void AddPerson(string name, bool raiseEvent)
        {
            base.AddPerson(name, raiseEvent);

            lock (this.dgvUsers.SyncRoot)
            {
                lock (this.channelUsers.SyncRoot)
                {
                    foreach (DataGridViewRow item in this.dgvUsers.SelectedRows)
                    {
                        if (this.channelUsers[item.Index].NickName == ServerWindow.Nick)
                        {
                            this.tSMIchannelModes.Enabled = this.channelUsers[item.Index].Modes.ContainsKey('@') && this.channelUsers[item.Index].Modes['@'];
                            this.tSMIchannelModes.DropDown.Enabled = this.tSMIchannelModes.Enabled;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Установить режим для выделенных пользователей
        /// </summary>
        /// <param name="mode"></param>
        private void Mode(string mode)
        {
            lock (this.dgvUsers.SyncRoot)
            {
                lock (this.channelUsers.SyncRoot)
                {
                    for (int i = 0; i < this.dgvUsers.SelectedRows.Count; i += 3)
                    {
                        int current = Math.Min(3, this.dgvUsers.SelectedRows.Count - i);
                        var sayMode = new StringBuilder();
                        sayMode.Append(mode[0]);
                        string[] nicks = new string[current];
                        for (int j = 0; j < current; j++)
                        {
                            sayMode.Append(mode[1]);
                            nicks[j] = this.channelUsers[this.dgvUsers.SelectedRows[i + j].Index].NickName;
                        }
                        if (MyCmd != null)
                            MyCmd(this, String.Format("/MODE {0} {1} {2}", this.winName, sayMode, string.Join(" ", nicks)));
                    }
                }
            }
        }

        private void OpPlus(object sender, EventArgs e)
        {
            Mode("+o");
        }

        private void OpMinus(object sender, EventArgs e)
        {
            Mode("-o");
        }

        private void VoicePlus(object sender, EventArgs e)
        {
            Mode("+v");
        }

        private void VoiceMinus(object sender, EventArgs e)
        {
            Mode("-v");
        }

        /// <summary>
        /// Отправить новый заголовок канала
        /// </summary>
        /// <param name="topic"></param>
        private void SetTopic(string topic)
        {
            MyCmd(this, String.Format("/TOPIC {0} :{1}", this.winName, topic));
        }

        #region IChannelWindow Members

        /// <summary>
        /// Имя канала
        /// </summary>
        public override string WindowName
        {
            get
            {
                return winName;
            }
        }

        #endregion

        #region IChannelWindow Members

        /// <summary>
        /// Список юзеров
        /// </summary>
        public IListView UsersList
        {
            get { return GetUsersList(); }
        }

        public IListView GetUsersList()
        {
            if (this.InvokeRequired)
            {
                var del = new Func<IListView>(GetUsersList);
                IAsyncResult ar = this.BeginInvoke(del);
                return (IListView)this.EndInvoke(ar);
            }
            else
            {
                return this.dgvUsers;
            }
        }

        #endregion

        private void HalfPlus(object sender, EventArgs e)
        {
            Mode("+h");
        }

        private void HalfMinus(object sender, EventArgs e)
        {
            Mode("-h");
        }

        private void Kick(object sender, EventArgs e)
        {
            lock (this.dgvUsers.SyncRoot)
            {
                lock (this.channelUsers.SyncRoot)
                {
                    lock (this.channelUsers.SyncRoot)
                    {
                        foreach (DataGridViewRow item in this.dgvUsers.SelectedRows)
                        {
                            MyCmd(this, String.Format("/KICK {0} {1}", this.winName, this.channelUsers[item.Index].NickName));
                        }
                    }
                }
            }
        }

        private void KickWords(object sender, EventArgs e)
        {
            IChannelUser[] toKick;
            lock (this.dgvUsers.SyncRoot)
            {
                toKick = this.dgvUsers.LVSelectedItems;
            }
            var diag = new StringEnterDialog(Resources.EnterWords);
            if (diag.ShowDialog() == DialogResult.OK)
            {
                string comment = diag.PrintedText;
                foreach (var user in toKick)
                {
                    MyCmd(this, String.Format("/KICK {0} {1} :{2}", this.winName, user.NickName, comment));
                }
            }
        }

        private void Ban(object sender, EventArgs e)
        {
            lock (this.dgvUsers.SyncRoot)
            {
                lock (this.channelUsers.SyncRoot)
                {
                    foreach (DataGridViewRow item in this.dgvUsers.SelectedRows)
                    {
                        MyCmd(this, String.Format("/MODE {0} +b {1}", this.winName, this.channelUsers[item.Index].NickName));
                    }
                }
            }
        }

        private void BanWords(object sender, EventArgs e)
        {
            IChannelUser[] toBan;
            lock (this.dgvUsers.SyncRoot)
            {
                toBan = this.dgvUsers.LVSelectedItems;
            }
            var diag = new StringEnterDialog(Resources.EnterWords);
            if (diag.ShowDialog() == DialogResult.OK)
            {
                string comment = diag.PrintedText;
                foreach (var user in toBan)
                {
                    MyCmd(this, String.Format("/MODE {0} +b {1} :{2}", this.winName, user.NickName, comment));
                }
            }
        }

        private void KickBan(object sender, EventArgs e)
        {
            lock (this.dgvUsers.SyncRoot)
            {
                lock (this.channelUsers.SyncRoot)
                {
                    foreach (DataGridViewRow item in this.dgvUsers.SelectedRows)
                    {
                        MyCmd(this, String.Format("/KICK {0} {1}", this.winName, this.channelUsers[item.Index].NickName));
                        MyCmd(this, String.Format("/MODE {0} +b {1}", this.winName, this.channelUsers[item.Index].NickName));
                    }
                }
            }
        }

        private void KickBanWords(object sender, EventArgs e)
        {
            if (MyCmd == null)
                return;

            var toBan = this.dgvUsers.LVSelectedItems;
            using (var diag = new StringEnterDialog(Resources.EnterWords))
            {
                if (diag.ShowDialog() == DialogResult.OK)
                {
                    string comment = diag.PrintedText;
                    foreach (var user in toBan)
                    {
                        MyCmd(this, String.Format("/KICK {0} {1} :{2}", this.winName, user.NickName, comment));
                        MyCmd(this, String.Format("/MODE {0} +b {1} :{2}", this.winName, user.NickName, comment));
                    }
                }
            }
        }

        /// <summary>
        /// Очистить все режимы канала (при перезаходе)
        /// </summary>
        internal void ClearModes()
        {
            foreach (var mode in modes)
            {
                mode.Value.Set = false;
            }
        }

        #region IChannelWindow Members

        public event EventHandler<PersonEventArgs> PersonModesChanged;

        public event EventHandler ChannelTopicChanged;

        public event EventHandler ChannelModesChanged;

        public event EventHandler<PersonEventArgs> PersonKicked;

        internal void KickPerson(string p)
        {
            RemovePerson(p, false);
            if (PersonKicked != null)
                PersonKicked(this, new PersonEventArgs(p));
        }

        #endregion

        private void tsmiInvite_DropDownOpening(object sender, EventArgs e)
        {
            tsmiInvite.DropDownItems.Clear();
            foreach (var channel in this.mainWindow.Channels)
            {
                bool good = false;
                foreach (var user in channel.UsersList.LVItems)
                {
                    if (user.NickName == this.mainWindow.Nick)
                    {
                        if (user.Modes.ContainsKey('@') && user.Modes['@'])
                            good = true;
                        else
                            break;
                    }
                }
                if (!good)
                    continue;

                good = false;
                lock (this.dgvUsers.SyncRoot)
                {
                    lock (this.channelUsers.SyncRoot)
                    {
                        foreach (DataGridViewRow item in this.dgvUsers.SelectedRows)
                        {
                            bool found = false;
                            foreach (var user in channel.UsersList.LVItems)
                                if (user.NickName == this.channelUsers[item.Index].NickName)
                                {
                                    found = true;
                                    break;
                                }
                            if (!found)
                            {
                                good = true;
                                break;
                            }
                        }
                    }
                }
                if (good)
                    tsmiInvite.DropDownItems.Add(channel.WindowName, Resources.channel, InvitePerson);
            }
        }

        /// <summary>
        /// Пригласить человека на канал
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InvitePerson(object sender, EventArgs e)
        {
            if (MyCmd != null)
            {
                string channelName = ((ToolStripItem)sender).Text;
                lock (this.dgvUsers.SyncRoot)
                {
                    foreach (DataGridViewRow item in this.dgvUsers.SelectedRows)
                    {
                        MyCmd(this, string.Format("/INVITE {0} {1}", this.channelUsers[item.Index].NickName, channelName));
                    }
                }
            }
        }

        /// <summary>
        /// Отредактировтаь набор масок
        /// </summary>
        /// <param name="list"></param>
        /// <param name="mode"></param>
        /// <param name="title"></param>
        private void EditChannelList(List<MaskInfo> list, char mode, string title)
        {
            //var old = new List<ChannelListItemInfo>();
            //list.ForEach(item => old.Add((ChannelListItemInfo)item.Clone()));

            //var collDialog = new CollectionEditorView { DataContext = list, Title = title };
            //collDialog.ShowDialog();

            using (var collection = new EditableCollectionViewModel<MaskInfo>(list, this.Handle, title, title,
                maskInfo =>
                {
                    maskInfo.When = DateTime.Now;
                    maskInfo.WhoSet = mainWindow.Nick;
                }))
            {
                var collDialog = new CollectionEditorView { DataContext = collection, Title = title };

                if (collDialog.ShowDialog() == true)
                {
                    // Нужно определить разницу между collection.List и list и внести соответствующие изменения
                    // Удалённые
                    foreach (var item in list.Except(collection.List))
                    {
                        ChannelMode("-" + mode, item.Mask);
                    }

                    // Добавленые
                    foreach (var item in collection.List.Except(list))
                    {
                        ChannelMode("+" + mode, item.Mask);
                    }
                }
            }

            //var collDiag = new CollectionEditorDialog(list, typeof(ChannelListItemInfo));
            //collDiag.Text = title;
            //collDiag.Width += 200;
            //collDiag.Adding += (sender, e) =>
            //{
            //    ChannelMode("+" + mode, ((ChannelListItemInfo)e.Item).Mask);
            //    ((ChannelListItemInfo)e.Item).WhoSet = mainWindow.User.UserName;
            //};
            //collDiag.AfterEdit += (sender, e) => 
            //{
            //    ChannelMode("-" + mode, ((ChannelListItemInfo)e.ItemOld).Mask);
            //    ChannelMode("+" + mode, ((ChannelListItemInfo)e.ItemNew).Mask);
            //    ((ChannelListItemInfo)e.ItemNew).WhoSet = mainWindow.User.UserName;
            //};
            //collDiag.Deleting += (sender, e) =>
            //{
            //    ChannelMode("-" + mode, (e.Item as ChannelListItemInfo).Mask);
            //};
            //collDiag.ShowDialog();
        }

        internal void AddBan(MaskInfo banInfo)
        {
            bans.Add(banInfo);
        }

        internal void BansFinished()
        {
            EditChannelList(bans, 'b', Resources.BansEdit);
        }

        internal void AddInvite(MaskInfo channelListItemInfo)
        {
            invite.Add(channelListItemInfo);
        }

        internal void InvitesFinished()
        {
            EditChannelList(invite, 'I', Resources.InviteEdit);
        }

        internal void AddExempt(MaskInfo channelListItemInfo)
        {
            exempts.Add(channelListItemInfo);
        }

        internal void ExemptsFinished()
        {
            EditChannelList(exempts, 'e', Resources.ExemptsEdit);
        }

        protected override void MDIChild_Activated(object sender, EventArgs e)
        {
            this.reading = true;
            if (!this.irtbTopic.IsEditing && !this.dgvUsers.Focused) this.irtbPrintMessage.SetFocus();
        }
    }
}
