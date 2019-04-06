using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using IRC.Client.Base;
using CIRCe.Base;
using System.Linq;
using System.Threading.Tasks;
using SIIRC.Properties;
using System.ServiceModel;

namespace SIIRC
{
    /// <summary>
    /// Аддон для ведения СИ
    /// </summary>
    public sealed class SIAddon : Addon
    {
        private ICIRCeApplication application = null;
        private ICIRCeServer server = null;
        private ICIRCeChannel channel = null;
        
        private CommandPanel commandPanel = null;
        private GameConfiguration gameConfig = null;
        private MainForm mainForm = null;
        private ICIRCeItem mainFormHost = null;
        private StartUpForm startUpForm = null;
        private ICommand separator = null;
        private ICommand tSMISI = null;
        private ICommand tSMISIAnswer = null;
        private ICommand tSMISIChooser = null;
        private ICommand tSMISICall = null;

        private object sync = new object();
        private bool isDisposed = false;
        public static volatile string TempFile = null;

        public SIAddon()
        {
        }        

        void AddPerson(IEnumerable<ChannelUserInfo> users)
        {
            foreach (var item in users)
            {
                if (mainForm != null)
                    mainForm.AddPlayer(new Player(item.NickName));
            }
        }

        void SetAnswerer(IEnumerable<ChannelUserInfo> users)
        {
            ChannelUserInfo user = null;
            if (mainForm != null && (user = users.FirstOrDefault()) != null)
            {
                mainForm.SetAnswerer(user.NickName);
            }
        }

        void SetChooser(IEnumerable<ChannelUserInfo> users)
        {
            ChannelUserInfo user = null;
            if (mainForm != null && (user = users.FirstOrDefault()) != null)
            {
                if (this.mainForm.InvokeRequired)
                {
                    this.mainForm.BeginInvoke(new Action<string>(mainForm.SetChooser), user.NickName);
                }
                else
                    mainForm.SetChooser(user.NickName);
            }
        }

        void Call(IEnumerable<ChannelUserInfo> users)
        {
            foreach (var item in users)
            {
                if (mainForm != null)
                    mainForm.Call(item.NickName);
            }
        }

        void tSMINewGame_Click(object sender, EventArgs e)
        {
            this.Dispose();
            lock (this.sync)
            {
                this.isDisposed = false;
            }
            NewGame(false);
        }

        void tSBShow_Click(object sender, EventArgs e)
        {
            this.mainForm = new MainForm(gameConfig, startUpForm.PackageDoc, this.application, this.server, this.channel, commandPanel, this);

            this.mainFormHost = this.application.AddItem(this.channel, mainForm.Handle, "SIGame", Resources.logo.Handle/*icon != null ? icon.Handle : IntPtr.Zero*/);            
            this.mainFormHost.Closed += Wrap(mainFormHost_Closed);
            formSync.Reset();

            commandPanel.ShowVisible = false;
        }

        void tSBEnd_Click(object sender, EventArgs e)
        {
            Stop();
        }

        public override void Stop()
        {
            if (this.commandPanel != null && this.commandPanel.InvokeRequired)
            {
                this.commandPanel.BeginInvoke(new Action(Stop));
                return;
            }

            if (Application.MessageLoop)
                Application.ExitThread();
        }

        public override bool IsUpdateNeeded()
        {
            return false;
        }

        public override string GetUpdateUri()
        {
            return null;
        }

        #region IDisposable Members

        /// <summary>
        /// Закрыть аддон
        /// </summary>
        public override void Dispose()
        {
            var invokeNeeded = this.commandPanel != null && this.commandPanel.InvokeRequired;

            if (invokeNeeded)
            {
                this.commandPanel.EndInvoke(this.commandPanel.BeginInvoke(new Action(Dispose)));
                return;
            }

            try
            {
                lock (this.sync)
                {
                    this.isDisposed = true;

                    if (this.startUpForm != null && this.startUpForm.PackageDoc != null)
                        this.startUpForm.PackageDoc.Dispose();

                    ClearTempFile();

                    if (mainFormHost != null)
                    {
                        this.mainFormHost.Close();                        
                    }

                    if (gameConfig != null)
                        gameConfig.Save();

                    if (this.application != null)
                        this.application.Status("");

                    if (this.channel != null)
                    {
                        this.channel.Closed -= Wrap(Stop);
                        var commands = this.channel.UsersList.Commands;
                        if (commands != null && this.tSMISI != null)
                        {
                            commands.Remove(this.separator);
                            commands.Remove(this.tSMISI);
                            commands.Remove(this.tSMISIAnswer);
                            commands.Remove(this.tSMISICall);
                            if (this.tSMISIChooser != null)
                                commands.Remove(this.tSMISIChooser);
                        }
                    }

                    if (this.commandPanel != null)
                    {
                        this.channel.ChatPanel.RemoveBottom(commandPanel.Handle);
                    }

                    if (mainFormHost != null)
                    {
                        formSync.WaitOne();
                        mainFormHost = null;
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
        }

        #endregion

        internal static void ClearTempFile()
        {
            if (TempFile != null && File.Exists(TempFile))
                File.Delete(TempFile);
        }

        #region Члены IAddon

        /// <summary>
        /// Запустить аддон
        /// </summary>
        /// <param name="application">Запускающее приложение</param>
        public override void Run(ICIRCeApplication application)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            this.application = application;
            this.gameConfig = GameConfiguration.Load();

            var activeItem = this.application.ActiveItem;
            var activeServer = activeItem as IServer;
            if (activeServer != null)
            {
                gameConfig.DefServerName = activeServer.Info.Server.Name;
                gameConfig.DefUserName = activeServer.Info.Nick;
            }
            else
            {
                var activeChannel = activeItem as IChannel;

                if (activeChannel != null)
                {
                    gameConfig.DefServerName = activeChannel.OwnerServer.Info.Server.Name;
                    gameConfig.DefChannelName = activeChannel.Name;
                    gameConfig.DefUserName = activeChannel.OwnerServer.Info.Nick;
                }
                else
                {
                    var activePrivate = activeItem as IPrivateSession;

                    if (activePrivate != null)
                    {
                        gameConfig.DefServerName = activePrivate.OwnerServer.Info.Server.Name;
                        gameConfig.DefUserName = activePrivate.OwnerServer.Info.Nick;
                    }
                    else
                    {
                        var nick = this.application.DefaultNickName;
                        if (nick != null)
                            gameConfig.DefUserName = nick;
                    }
                }
            }
            
            this.startUpForm = new StartUpForm(this.gameConfig);
            this.application.AddOwnedWindow(this.startUpForm.Handle);

            NewGame();
        }

        #endregion

        private void NewGame(bool first = true)
        {
            if (this.startUpForm.ShowDialog() == DialogResult.OK)
            {
                gameConfig.DefServerPort = startUpForm.Port;
                gameConfig.GameType = startUpForm.GameType;
                if (gameConfig.DefPackagePath.Length == 0)
                {
                    MessageBox.Show("Вы должны выбрать пакет для игры!");
                    Stop();
                    return;
                }

                this.server = this.application.CreateConnection(new ConnectionInfo
                {
                    Server = new ServerInfo { Name = gameConfig.DefServerName, Port = (int)gameConfig.DefServerPort },
                    Nick = gameConfig.DefUserName
                });

                if (!this.server.IsConnected && !this.server.Connect())
                {
                    Stop();
                    return;
                }

                Futher(first);
            }
            else
                Stop();
        }

        private void Futher(bool first = true)
        {
            this.channel = this.server.JoinChannel(gameConfig.DefChannelName);

            if (this.channel == null)
            {
                Stop();
                return;
            }

            this.channel.Closed += Wrap(Stop);

            lock (this.sync)
            {
                if (this.isDisposed)
                    return;

                this.commandPanel = new CommandPanel();

                commandPanel.Dock = DockStyle.Bottom;

                commandPanel.End += tSBEnd_Click;
                commandPanel.ShowMain += tSBShow_Click;
                commandPanel.NewGame += tSMINewGame_Click;

                this.channel.Activate();
                this.channel.ChatPanel.AddBottom(commandPanel.Handle);

                gameConfig.CurrentStage = GameConfiguration.SIStage.Begin;
                gameConfig.RoundNum = gameConfig.ThemeNum = gameConfig.QuestNum = 0;

                this.mainForm = new MainForm(gameConfig, startUpForm.PackageDoc, this.application, this.server, this.channel, commandPanel, this);

                this.mainFormHost = this.application.AddItem(this.channel, mainForm.Handle, "SIGame", Resources.logo.Handle);
                this.mainFormHost.Closed += Wrap(mainFormHost_Closed);
                formSync.Reset();

                var commands = this.channel.UsersList.Commands;
                this.separator = commands.AddSeparator();
                this.tSMISI = commands.AddCommand("Добавить в список игроков", Wrap<IEnumerable<ChannelUserInfo>>(AddPerson));
                this.tSMISIAnswer = commands.AddCommand("Назначить отвечающим", Wrap<IEnumerable<ChannelUserInfo>>(SetAnswerer));
                if (gameConfig.GameType == GameConfiguration.GameTypes.TeleSI)
                {
                    this.tSMISIChooser = commands.AddCommand("Назначить выбирающим", Wrap<IEnumerable<ChannelUserInfo>>(SetChooser));
                }
                this.tSMISICall = commands.AddCommand("Обратиться", Wrap<IEnumerable<ChannelUserInfo>>(Call));
            }

            if (first)
                Application.Run();
        }

        private AutoResetEvent formSync = new AutoResetEvent(false);

        void mainFormHost_Closed()
        {
            this.commandPanel.Next -= this.mainForm.tSBNext_Click;
            this.commandPanel.Ready -= this.mainForm.tSBReady_Click;

            this.commandPanel.ShowScore -= this.mainForm.tSMUScore_Click;
            this.commandPanel.ShowStats -= this.mainForm.tSMUStat_Click;

            this.commandPanel.Yes -= this.mainForm.tSBYes_Click;
            this.commandPanel.No -= this.mainForm.tSBNo_Click;
            this.commandPanel.Cancel -= this.mainForm.commandPanel_Cancel;

            this.commandPanel.Closed -= this.mainForm.tSMIClosed_Click;
            this.commandPanel.Select -= this.mainForm.tSMIScroll_Click;

            this.channel.MessageReceived -= Wrap<SessionMessageEventArgs>(this.mainForm.window_MessageReceived);
            this.commandPanel.ConfigureTimer -= this.mainForm.tSMIConfigureTimer_Click;

            MakeShowVisible();

            this.mainFormHost.Closed -= Wrap(mainFormHost_Closed);
            formSync.Set();
        }

        private void MakeShowVisible()
        {
            if (this.commandPanel.InvokeRequired)
            {
                this.commandPanel.BeginInvoke(new Action(MakeShowVisible));
                return;
            }

            this.commandPanel.ShowVisible = true;
        }
    }
}