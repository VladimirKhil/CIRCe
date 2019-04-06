using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using IRCWindow.Properties;
using System.IO;
using IRCProviders;
using IRCConnection;
using System.Reflection;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using CIRCe.Base;
using IRCWindow.ViewModel;
using IRCWindow.Data;

namespace IRCWindow
{
    /// <summary>
    /// Базовый класс внутреннего окна клиента
    /// </summary>
    internal partial class MDIChild : IRCForm
    {
        #region Fields

        protected MDIParent main = null;
        internal static Settings settings = Settings.Default;
        protected WindowNode myNode = null;
        protected bool reading = true;
        protected LogMode logMode = LogMode.Txt;

        internal LogMode Logging { get { return this.logMode; } set { this.logMode = value; } }

        /// <summary>
        /// Словарь, содержащий информацию о пользователях сервера
        /// </summary>
        protected Dictionary<string, UserInfo> whois;
        protected object whoisLock = new object();

        #endregion

        /// <summary>
        /// Добавление сообщения в окно сообщений
        /// </summary>
        /// <param name="text">Текст сообщения</param>
        /// <param name="color">Цвет по умолчанию</param>
        /// <param name="time">Указать ли время добалвения сообщения</param>
        internal delegate void PutMsgDel(string text, Color color, bool time);

        internal MDIParent Main { get { return main; } }

        /// <summary>
        /// Узел в дереве, соответстующий данному окну
        /// </summary>
        internal WindowNode MyNode
        {
            get { return myNode; }
            set { myNode = value; }
        }

        /// <summary>
        /// Активное IRC-окно
        /// </summary>
        protected MDIChild ActiveIRCWindow
        {
            get { return this.Main.ActiveIRCWindow ?? this; }
        }

        public virtual MDIChildServer ServerWindow { get { return null; } }

        public ICIRCeItem DataContext { get; set; }

        public MDIChild() { InitializeComponent(); }

        internal MDIChild(MDIParent main)
        {
            InitializeComponent();

            this.main = main;
            this.Size = UISettings.Default.IRCWindowSize;
            if (!DesignMode)
                this.logMode = UISettings.Default.LogMode;

            this.GotFocus += (sender, e) =>
                {
                    this.irtbPrintMessage.SetFocus();
                };
            
            var column = new DataGridViewTextBoxColumn { DataPropertyName = "VisibleNick", Name = "VisibleNick", HeaderText = "VisibleNick", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill };
            this.dgvUsers.Columns.Add(column);

            this.irtbPrintMessage.ForeColor = Settings.Default.Colors == null ? Color.Black : Settings.Default.Colors[Program.ProgramOptions.PrintForeColor];

            UISettings.Default.PropertyChanged += Settings_PropertyChanged;

            this.irtbPrintMessage.RichTextBox.Font = Program.ProgramOptions.PrintFont;
            this.irtbPrintMessage.Font = Program.ProgramOptions.PrintFont;
            this.chatRTB.Font = Program.ProgramOptions.ChatFont;

            this.chatRTB.BackColor = UISettings.Default.ChatBackColor;
            this.chatRTB.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", UISettings.Default, "ChatBackColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.dgvUsers.BackgroundColor = UISettings.Default.UsersBackColor;
            this.dgvUsers.CellsBackgroundColor = UISettings.Default.UsersBackColor;
            this.dgvUsers.DataBindings.Add(new System.Windows.Forms.Binding("Font", UISettings.Default, "UsersFont", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.dgvUsers.DataBindings.Add(new System.Windows.Forms.Binding("BackgroundColor", UISettings.Default, "UsersBackColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.dgvUsers.DataBindings.Add(new System.Windows.Forms.Binding("CellsBackgroundColor", UISettings.Default, "UsersBackColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.dgvUsers.Font = UISettings.Default.UsersFont;
            this.irtbPrintMessage.BackColor = UISettings.Default.PrintBackColor;
            this.irtbPrintMessage.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", UISettings.Default, "PrintBackColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.irtbPrintMessage.DataBindings.Add(new System.Windows.Forms.Binding("Font", UISettings.Default, "PrintFont", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.irtbPrintMessage.Font = UISettings.Default.PrintFont;
            
        }

        internal virtual void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "MessagesColor")
                this.irtbPrintMessage.ForeColor = Settings.Default.Colors[Program.ProgramOptions.PrintForeColor];
        }

        #region PutMessage

        /// <summary>
        /// Добавить текст сообщения в окно сообщений
        /// </summary>
        /// <param name="msgText">Текст сообщения</param>
        /// <param name="defColor">Цвет по умолчанию</param>
        /// <param name="putTime">Указать ли время добавления сообщения</param>
        /// <param name="messageType">Тип публикуемого сообщения</param>
        public virtual void PutMessage(string msgText, Color defColor, bool putTime, MessageType messageType)
        {
            if (InvokeRequired)
            {
                var del = new Action<string, Color, bool, MessageType>(PutMessage);
                BeginInvoke(del, msgText, defColor, putTime, messageType);
            }
            else
            {
                if (this.IsDisposed) return;
                if ((Form.ActiveForm == null || (this.Main as MDIParent).GetActiveChild() != this) && messageType == MessageType.Replic)
                {
                    if (reading)
                    {
                        reading = false;
                        if (this.chatRTB.TextLength > 0 && UISettings.Default.ShowReadSplitters)
                        {
                            LogMode oldMode = this.logMode;
                            this.logMode = LogMode.None;
                            Echo(MDIChildPrivate.ReadSplitterString);
                            this.logMode = oldMode;
                        }
                    }
                    try
                    {
                        var flashing = UISettings.Default.FlashingParams;
                        switch (flashing.FlashingMode)
                        {
                            case FlashMode.Full:
                                bool found = false;
                                if (flashing.UseBlackList)
                                {
                                    string[] black = flashing.BlackList.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                                    foreach (var item in black)
                                    {
                                        if (msgText.Contains(item))
                                        {
                                            found = true;
                                            break;
                                        }
                                    }
                                }
                                if (!found)
                                    Main.Flash();
                                break;

                            case FlashMode.Meduim:
                                if (flashing.FlashOnNick)
                                    if (msgText.Length > 2 && msgText.IndexOf(ServerWindow.Nick, 2) > -1)
                                    {
                                        Main.Flash();
                                        break;
                                    }
                                if (!flashing.UseWhiteList)
                                    break;
                                string[] white = flashing.WhiteList.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                                found = false;
                                foreach (var item in white)
                                {
                                    if (msgText.Contains(item))
                                    {
                                        found = true;
                                        break;
                                    }
                                }
                                if (found)
                                    Main.Flash();
                                break;
                        }
                    }
                    catch (ArgumentException) { }
                }                

                string add = putTime && UISettings.Default.DateTimeFormat.Length > 0 ? DateTime.Now.ToString(UISettings.Default.DateTimeFormat) + " " : string.Empty;
                string msg = add + msgText + Environment.NewLine;
                try
                {
                    if (chatRTB != null)
                        IRCRichTextBox.SetText(chatRTB, msg, defColor, UISettings.Default.ChatFont);
                    if (this.Main != null && this != (this.Main as MDIParent).GetActiveChild())
                        myNode.ForeColor = Color.Violet;
                    Log(add + msgText, defColor);
                }
                catch (Exception e)
                {
                    MessageBox.Show(msgText + " " + e.ToString());
                }
            }
        }

        /// <summary>
        /// Добавить текст сообщения в окно сообщений
        /// </summary>
        /// <param name="msgText">Текст сообщения</param>
        /// <param name="defColor">Цвет по умолчанию</param>
        /// <param name="putTime">Указать ли время добалвения сообщения</param>
        public virtual void PutMessage(string msgText, Color defColor, bool putTime)
        {
            PutMessage(msgText, defColor, putTime, MessageType.Replic);
        }

        /// <summary>
        /// Добавить текст сообщения в окно сообщений
        /// </summary>
        /// <param name="msgText">Текст сообщения</param>
        /// <param name="defColor">Цвет по умолчанию</param>
        public virtual void PutMessage(string msgText, Color defColor)
        {
            PutMessage(msgText, defColor, true, MessageType.Replic);
        }

        /// <summary>
        /// Добавить текст сообщения в окно сообщений
        /// </summary>
        /// <param name="msgText">Текст сообщения</param>
        /// <param name="defColorIndex">Код цвета по умолчанию</param>
        /// <param name="putTime">Указать ли время добалвения сообщения</param>
        public virtual void PutMessage(string msgText, int defColorIndex, bool putTime)
        {
            PutMessage(msgText, Settings.Default.Colors[defColorIndex], putTime, MessageType.Replic);
        }

        /// <summary>
        /// Добавить текст сообщения в окно сообщений
        /// </summary>
        /// <param name="msgText">Текст сообщения</param>
        /// <param name="defColorIndex">Код цвета по умолчанию</param>
        public virtual void PutMessage(string msgText, int defColorIndex)
        {
            PutMessage(msgText, Settings.Default.Colors[defColorIndex], true, MessageType.Replic);
        }

        /// <summary>
        /// Добавить текст сообщения в окно сообщений
        /// </summary>
        /// <param name="msgText">Текст сообщения</param>
        public virtual void PutMessage(string msgText)
        {
            PutMessage(msgText, settings.Colors[1], true, MessageType.Replic);
        }

        protected virtual void Log(string text) { Log(text, Settings.Default.Colors.DefForeColor); }

        protected virtual void Log(string text, Color defColor) { }

        #endregion

        /// <summary>
        /// Форма активирована
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void MDIChild_Activated(object sender, EventArgs e)
        {
            this.reading = true;
            this.irtbPrintMessage.SetFocus();
        }

        /// <summary>
        /// Вывести текст на экран
        /// </summary>
        /// <param name="text"></param>
        public void Echo(string text)
        {
            PutMessage(text, Settings.Default.Colors.DefForeColor, false);
        }

        #region IBaseWindow Members

        public IRCForm Self
        {
            get { return GetSelf(); }
        }

        private delegate Form GetSelfDel();

        /// <summary>
        /// Получить собственно форму
        /// </summary>
        /// <returns></returns>
        private IRCForm GetSelf()
        {
            if (this.InvokeRequired)
            {
                GetSelfDel getSelfDel = new GetSelfDel(GetSelf);
                IAsyncResult result = this.BeginInvoke(getSelfDel, null);
                return (IRCForm)this.EndInvoke(result);
            }
            else
            {
                return this;
            }
        }

        public IRCProviders.IPanel ChatPanel
        {
            get { return GetChatPanel(); }
        }

        private delegate IRCProviders.IPanel GetPanelDel();

        /// <summary>
        /// Получить панель чата
        /// </summary>
        /// <returns></returns>
        private IRCProviders.IPanel GetChatPanel()
        {
            if (this.InvokeRequired)
            {
                GetPanelDel getPanelDel = new GetPanelDel(GetChatPanel);
                IAsyncResult result = this.BeginInvoke(getPanelDel);
                return (IRCProviders.IPanel)this.EndInvoke(result);
            }
            else
            {
                return this.chatPanel;
            }
        }

        public IRCProviders.IPanel InputPanel
        {
            get { return GetInputPanel(); }
        }

        /// <summary>
        /// Получить панель ввода сообщения
        /// </summary>
        /// <returns></returns>
        private IRCProviders.IPanel GetInputPanel()
        {
            if (this.InvokeRequired)
            {
                GetPanelDel getPanelDel = new GetPanelDel(GetInputPanel);
                IAsyncResult result = this.BeginInvoke(getPanelDel, null);
                return (IRCProviders.IPanel)this.EndInvoke(result);
            }
            else
            {
                return this.printMessagePanel;
            }
        }

        #endregion

        private void tSMIChannels_DropDownOpening(object sender, EventArgs e)
        {
            tSMIChannels.DropDownItems.Clear();
            foreach (var item in this.ServerWindow.Server.Channels)
            {
                tSMIChannels.DropDownItems.Add(item.Name, Resources.channel, JoinChannel);
            }

            tSMIChannels.DropDownItems.Add(new ToolStripSeparator());
            tSMIChannels.DropDownItems.Add(Resources.New + "...", null, (Main as MDIParent).NewChannel);
            tSMIChannels.DropDownItems.Add(Resources.Configure + "...", null, (Main as MDIParent).ConfigureChan);
        }

        private void tSMINick_DropDownOpening(object sender, EventArgs e)
        {
            tSMINick.DropDownItems.Clear();
            foreach (var item in UserOptions.Default.Nicks)
            {
                tSMINick.DropDownItems.Add(item, Resources.selectUser, ChangeNick);
            }
            tSMINick.DropDownItems.Add(new ToolStripSeparator());
            tSMINick.DropDownItems.Add(Resources.New + "...", null, (Main as MDIParent).NewNickName);
            tSMINick.DropDownItems.Add(Resources.Configure + "...", null, (Main as MDIParent).ConfigureNickNames);
        }

        protected virtual void JoinChannel(object sender, EventArgs e) { }
        protected virtual void ChangeNick(object sender, EventArgs e) { }

        private void usersListView_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
        }

        private void MDIChild_Load(object sender, EventArgs e)
        {
            
        }

        protected enum Direction { Left, Right };
        protected delegate bool MatchTokenDelegate(char c, Direction direction);
        protected delegate bool ProoveTokenDelegate(string s);

        protected bool GetToken(RichTextBox chatRichTextBox, Point p, out int left, out int right, MatchTokenDelegate matcher, ProoveTokenDelegate proover)
        {
            int index = chatRichTextBox.GetCharIndexFromPosition(p);
            int lindex = chatRichTextBox.GetLineFromCharIndex(index);
            int start = chatRichTextBox.GetFirstCharIndexFromLine(lindex);
            left = index;
            while (left > 0 && left < chatRichTextBox.Text.Length && matcher(chatRichTextBox.Text[left], Direction.Left))
                left--;
            right = index;
            while (right + 1 < chatRichTextBox.Text.Length && matcher(chatRichTextBox.Text[right], Direction.Right))
                right++;
            return left + 1 < right && proover(chatRichTextBox.Text.Substring(left, right - left + 1)); // Выделено имя канала
        }

        private TextStyleSaver saver = new TextStyleSaver();

        public static void HighLight(RichTextBox chatRichTextBox, int left, int right, TextStyleSaver saver)
        {
            saver.CopyFrom(chatRichTextBox, left, right);

            chatRichTextBox.SelectionFont = new Font(chatRichTextBox.SelectionFont ?? chatRichTextBox.Font, /*FontStyle.Bold | */FontStyle.Underline);
            chatRichTextBox.SelectionLength = 0;
        }

        public virtual void JoinChannel(string channel)
        {
            
        }

        private bool IsChannelChar(char c, Direction direction)
        {
            return c != ' ' && c != ',' && c != '\u0007' && c != '\n'
                && (direction == Direction.Right || c != '#' && c != '&');
        }

        private bool IsChannel(string name)
        {
            return name[0] == '#' || name[0] == '&';
        }

        #region IBaseWindow Members

        /// <summary>
        /// Пользователь нажал клавишу
        /// </summary>
        public event EventHandler<IRCProviders.SerializableKeyEventArgs> InputKeyDown;

        /// <summary>
        /// Пользователь нажал символьную клавишу
        /// </summary>
        public event EventHandler<IRCProviders.SerializableKeyPressedEventArgs> InputKeyPress;

        #endregion

        private void printMessageIrcRichTextBox_InputKeyDown(object sender, KeyEventArgs e)
        {
            if (InputKeyDown != null)
            {
                var eventArgs = new IRCProviders.SerializableKeyEventArgs() { Alt = e.Alt, Control = e.Control, Handled = e.Handled, KeyCode = e.KeyCode, KeyData = e.KeyData, KeyValue = e.KeyValue, Modifiers = e.Modifiers, Shift = e.Shift, SuppressKeyPress = e.SuppressKeyPress };
                InputKeyDown(this, eventArgs);
                e.Handled = eventArgs.Handled;
            }

            if (e.Control && e.KeyCode == System.Windows.Forms.Keys.F)
                Search();
        }

        private void Search()
        {
            var form = new SearchForm(this.chatRTB);
            this.Main.RegisterAsMDIChild(this.Self, form, this);
        }

        private void printMessageIrcRichTextBox_InputKeyPress(object sender, KeyPressEventArgs e)
        {
            if (InputKeyPress != null)
            {
                var eventArgs = new IRCProviders.SerializableKeyPressedEventArgs() { Handled = e.Handled, KeyChar = e.KeyChar };
                InputKeyPress(this, eventArgs);
                e.Handled = eventArgs.Handled;
            }
        }

        private void chatRTB_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                Process.Start(e.LinkText);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void chatRTB_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                return;
            int left = 0, right = 0;
            bool b = GetToken(chatRTB, e.Location, out left, out right, IsChannelChar, IsChannel);

            if (saver.Left != -1 && (!b || left != saver.Left))
            {
                chatRTB.SelectionStart = saver.Left;
                chatRTB.SelectionLength = saver.Right - saver.Left;

                saver.Paste();
                chatRTB.SelectionLength = 0;

                saver.Left = -1;
            }

            if (b && (saver.Left == -1 || saver.Left != left))
            {
                HighLight(chatRTB, left, right, saver);
            }
        }

        private void chatRTB_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                this.irtbPrintMessage.SetFocus();
                if (chatRTB.SelectionLength > 0)
                {
                    var viewModel = (CIRCeAppItem)this.DataContext;
                    viewModel.OnChatSelected(this.chatRTB.SelectedText);
                    chatRTB.Copy();
                    chatRTB.DeselectAll();
                }
            }
            catch (Exception) { }
        }

        private void chatRTB_MouseClick(object sender, MouseEventArgs e)
        {
            int left = 0, right = 0;
            bool b = GetToken(chatRTB, e.Location, out left, out right, IsChannelChar, IsChannel);
            if (b) // Выделено имя канала
            {
                string channel = chatRTB.Text.Substring(left, right - left);
                JoinChannel(channel);
                return;
            }
        }

        private delegate void Action();

        public void ActivateWindow()
        {
            if (this.InvokeRequired)
            {
                Debug.WriteLine("Activate!");
                Action del = new Action(ActivateWindow);
                IAsyncResult ar = this.BeginInvoke(del);
                this.EndInvoke(ar);
            }
            else
            {
                this.Activate();
                Debug.WriteLine("Activated!");
            }
        }

        internal void BeginRead()
        {
            this.reading = true;
        }

        private void MDIChild_Deactivate(object sender, EventArgs e)
        {
            //this.reading = false;
        }

        private void irtbPrintMessage_InnerMouseWheel(object sender, MouseEventArgs e)
        {
            int pos = IRCRichTextBox.GetVScrollPos(this.chatRTB);
            IRCRichTextBox.SetVScrollPos(this.chatRTB,  pos - e.Delta / 5);
        }

        internal void HideInputEditor()
        {
            this.irtbPrintMessage.HideEditor();
        }

        #region IVisual Members

        public void Deactivated()
        {
            
        }

        #endregion

        private void MDIChild_FormClosed(object sender, FormClosedEventArgs e)
        {
            UISettings.Default.PropertyChanged -= Settings_PropertyChanged;
        }

        private void dgvUsers_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Program.SendErrorReport(e.Exception, false);
        }
    }
}