using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using IRCConnection;
using IRCProviders;
using IRCWindow.Properties;
using Media;
//using ObjectEditors;
using System.Security.Policy;
using IRCWindow.ViewModel;
using CIRCe.Base;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Linq;
using IRC.Client;
using Microsoft.WindowsAPICodePack.Taskbar;
using System.Text.RegularExpressions;
using IRCWindow.View;
using IRCWindow.Data;
using IRCWindow.ViewModel.Common;

namespace IRCWindow
{
    /// <summary>
    /// Главное окно приложения
    /// </summary>
    internal partial class MDIParent : Form, IEnumerable<IRCForm>
    {
        private WindowNode servNode = null;

        private MediaPlayer[] player = new MediaPlayer[3];

        private const string debugLog = "log.txt";

        private List<IRCForm> children = new List<IRCForm>();
        private object childrenSync = new object();

        private WindowNode activeNode = null;

        private Dictionary<WindowNode, List<string>> connectionTasks = new Dictionary<WindowNode, List<string>>();

        bool firstTime = true;

        private Dictionary<string, WindowInfo> Windows = new Dictionary<string, WindowInfo>();

        private string[] args = null;

        /// <summary>
        /// Приложение Цирцеи (и главное окно)
        /// </summary>
        private CIRCeApplication application = null;

        public MDIParent() : this(new string[0]) { }

        public MDIParent(string[] args)
        {
            InitializeComponent();
            
            this.args = args;
            this.application = new CIRCeApplication(this);

            toolStrip.ImageList = imageList1;
            tsddbServers.ImageIndex = 1;
            tsddbServers.DropDown.ImageList = imageList1;

            this.tSMIConnect.DropDown = tsddbServers.DropDown;

            this.tsddbChannels.DropDown = this.tsmiChannels.DropDown;
            this.tsmiChannels.DropDown.Enabled = false;

            this.tsddbNick.DropDown = this.tsmiNick.DropDown;
            this.tsmiNick.DropDown.Enabled = false;

            this.tsmiUpdates.CheckStateChanged += (sender, e) => Settings.Default.SearchForUpdates = this.tsmiUpdates.CheckState;
            this.tsmiSearchAddons.CheckStateChanged += (sender, e) => Settings.Default.SearchForAddons = this.tsmiSearchAddons.CheckState;

            this.tSCBNickList.SelectedIndexChanged += new System.EventHandler(this.tSCBNickList_SelectedIndexChanged);

            this.tvWindows.DataBindings.Add("Font", UISettings.Default, "TreeFont");
            this.tvWindows.DataBindings.Add("BackColor", UISettings.Default, "TreeBackColor");
            this.tsbOpenPrivate.Visible = UISettings.Default.OpenPrivateVisible;

            this.tsmiCmdPlay.Checked = UISettings.Default.PlayMusicExt != PlayMode.None;
            this.tsmiCmdUrl.Checked = UISettings.Default.UrlExt != PlayMode.None;

            UISettings.Default.PropertyChanged += Default_PropertyChanged;

            this.tsddbServers.Visible = Settings.Default.ServersVisibility;
            this.tSCBNickList.Visible = Settings.Default.NickListVisibility;
            this.tsddbChannels.Visible = Settings.Default.ChannelsVisibility;
            this.tsddbNick.Visible = Settings.Default.NicksVisibility;
            this.tSBUser.Visible = Settings.Default.PersonVisibility;
            this.tsddbSettings.Visible = Settings.Default.SettingsVisibility;

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        void Default_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (String.Compare(e.PropertyName, "OpenPrivateVisible", false) == 0)
                this.tsbOpenPrivate.Visible = UISettings.Default.OpenPrivateVisible;
        }

        void item_Error(object sender, Media.ErrorEventArgs eea)
        {
            int errCode = (int)(eea.ErrNum % ((long)uint.MaxValue + 1));
            if (errCode != 0)
            {
                MessageBox.Show(new Win32Exception(errCode).Message);
            }
        }

        internal static void ApplyResource(ComponentResourceManager resources, object c)
        {
            var control = c as Control;
            if (control != null)
            {
                resources.ApplyResources(control, control.Name);

                foreach (var item in control.Controls)
                {
                    ApplyResource(resources, item);
                }
            }

            var toolStrip = c as ToolStrip;
            if (toolStrip != null)
                foreach (ToolStripItem item in toolStrip.Items)
                {
                    ApplyItem(resources, item);
                }
        }

        private static void ApplyItem(ComponentResourceManager resources, ToolStripItem item)
        {
            resources.ApplyResources(item, item.Name);
            var menuItem = item as ToolStripMenuItem;
            if (menuItem != null)
                foreach (ToolStripItem item2 in menuItem.DropDownItems)
                {
                    ApplyItem(resources, item2);
                }

            var dropDownItem = item as ToolStripDropDownItem;
            if (dropDownItem != null)
                foreach (ToolStripItem item2 in dropDownItem.DropDownItems)
                {
                    ApplyItem(resources, item2);
                }
        }

        private void ChangeVisibility(object sender, EventArgs e)
        {
            ((ToolStripItem)((ToolStripMenuItem)sender).Tag).Visible = ((ToolStripMenuItem)sender).Checked;
        }

        private void ConnectToServ(object sender, EventArgs e)
        {
            ConnectTo(UserOptions.Default.Servers[tsddbServers.DropDownItems.IndexOf((ToolStripItem)sender)]);
        }

        private void ConfigureServ(object sender, EventArgs e)
        {
            //var collDiag = new CollectionEditorDialog(UserOptions.Default.Servers, typeof(ExtendedServerInfo));
            //collDiag.Text = Resources.IRCServerListConfig;
            //collDiag.ShowDialog();

            EditCollection<ExtendedServerInfo>(UserOptions.Default.Servers, Resources.IRCServerListConfig, "Новый сервер", "Сервер", UpdateServList);
        }

        private void EditCollection<T>(List<T> list, string mainTitle, string newTitle, string editTitle, Action refresh) where T: ICloneable, IDataErrorInfo, INotifyPropertyChanged, new()
        {
            using (var collection = new EditableCollectionViewModel<T>(list, this.Handle, newTitle, editTitle))
            {
                var collDialog = new CollectionEditorView { DataContext = collection, Title = mainTitle };
                if (collDialog.ShowDialog() == true)
                {
                    list.Clear();
                    list.AddRange(collection.List);
                    refresh();
                }
            }
        }

        private void JoinChan(object sender, EventArgs e)
        {
            var child = ActiveIRCWindow;
            if (child != null)
            {
                child.ServerWindow.JoinChannel(((ToolStripItem)sender).Tag.ToString());
            }
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        private void NewServer(object sender, EventArgs e)
        {
            var newObject = new ExtendedServerInfo();

            if (ShowDialog(newObject, "Новый сервер"))
            {
                UserOptions.Default.Servers.Add(newObject);
                UpdateServList();
                ConnectTo(newObject);
            }
        }

        private bool ShowDialog(object data, string title)
        {
            //var button = new System.Windows.Controls.Button
            //{
            //    HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
            //    Padding = new System.Windows.Thickness(40, 0, 40, 0),
            //    Margin = new System.Windows.Thickness(5),
            //    Content = "OK",
            //    IsDefault = true
            //};

            //var panel = new System.Windows.Controls.StackPanel();
            //panel.Children.Add(view);
            //panel.Children.Add(button);

            //System.Windows.Media.Imaging.BitmapSource image;

            //var hBitmap = Resources.icon1.GetHbitmap();
            //try
            //{
            //    image = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, System.Windows.Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            //}
            //finally
            //{
            //    DeleteObject(hBitmap);
            //}

            //var diag = new System.Windows.Window
            //{
            //    FontSize = 16,
            //    Content = panel,
            //    DataContext = data,
            //    SizeToContent = System.Windows.SizeToContent.WidthAndHeight,
            //    ResizeMode = System.Windows.ResizeMode.NoResize,
            //    ShowInTaskbar = false,
            //    WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,
            //    Icon = image
            //};

            var diag = new ItemEditorView { DataContext = data };

            var helper = new System.Windows.Interop.WindowInteropHelper(diag);
            helper.Owner = this.Handle;

            //button.Click += (sender2, e2) => { diag.DialogResult = true; diag.Close(); };

            diag.Title = title;
            return diag.ShowDialog() == true;
        }

        internal void NewChannel(object sender, EventArgs e)
        {
            var newObject = new ExtendedChannelInfo();

            if (ShowDialog(newObject, "Новый канал"))
            {
                if (servNode == null)
                {
                    var child = GetActiveChild() as MDIChild;
                    if (child == null)
                        return;

                    servNode = child.MyNode;
                    if (servNode == null)
                        return;

                    while (servNode.Parent != null)
                        servNode = servNode.Parent as WindowNode;
                }

                var wnd = (MDIChildServer)servNode.Window;
                if (wnd.Server == null)
                    return;

                wnd.Server.Channels.Add(newObject);
                UpdateChannelsList();

                wnd.JoinChannel(newObject.Name);
                servNode = null;
            }
        }

        /// <summary>
        /// Добавление нового пользователя в список
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void NewNickName(object sender, EventArgs e)
        {
            var newObj = new NickInfo();

            if (ShowDialog(newObj, "Новый ник"))
            {
                UserOptions.Default.Nicks.Add(newObj.Name);
                UpdateNicksList();
                if (sender == tSCBNickList)
                {
                    tSCBNickList.Text = newObj.Name;
                }
                else
                {
                    if (servNode == null)
                    {
                        var child = GetActiveChild() as MDIChild;
                        if (child == null)
                            return;

                        servNode = child.MyNode;
                        while (servNode.Parent != null)
                            servNode = servNode.Parent as WindowNode;
                    }

                    ((MDIChildServer)servNode.Window).SetNick(newObj.Name);
                }
            }
            else if (sender == tSCBNickList)
            {
                tSCBNickList.SelectedIndex = 0;
            }
        }

        internal void ConfigureChan(object sender, EventArgs e)
        {
            var active = this.ActiveIRCWindow;
            if (active != null)
                EditCollection<ExtendedChannelInfo>(active.ServerWindow.Server.Channels, Resources.ChannelListConfig, "Новый канал", "Канал", UpdateChannelsList);
        }

        internal void ChangeNick(object sender, EventArgs e)
        {
            if (servNode == null)
            {
                var child = GetActiveChild() as MDIChild;
                if (child == null)
                    return;

                servNode = child.MyNode;
                if (servNode == null)
                    return;

                while (servNode.Parent != null)
                    servNode = servNode.Parent as WindowNode;
            }

            var server = (MDIChildServer)servNode.Window;
            if (server != null)
                server.SetNick(sender.ToString());
        }

        internal void ConfigureNickNames(object sender, EventArgs e)
        {
            var list = UserOptions.Default.Nicks.Select(n => new NickInfo { Name = n}).ToList();

            EditCollection<NickInfo>(list, Resources.NickListConfig, "Новый ник", "Ник", () =>
                {
                    UserOptions.Default.Nicks.Clear();
                    UserOptions.Default.Nicks.AddRange(list.Select(ni => ni.Name));

                    UpdateNicksList();
                });
        }

        internal void ConfigureAddons(object sender, EventArgs e)
        {
            try
            {
                var addonsList = Wrap(Settings.Default.AddonsNew);

                using (var collection = new EditableCollectionViewModel<AddonSettings>(addonsList, this.Handle, null, null))
                {
                    collection.CanAdd = false;
                    var editor = new CollectionEditorView { DataContext = collection, Title = "Настройка дополнений" };

                    if (editor.ShowDialog() == true)
                    {
                        foreach (var addon in addonsList.Except(collection.List))
                        {
                            this.application.AddonsManager.Uninstall(addon.Info);
                        }

                        Settings.Default.AddonsNew.Clear();
                        Settings.Default.AddonsNew.AddRange(collection.List.Select(item => item.Info));

                        UpdateAddonsList();
                    }
                }

                //var addonForm = new AddonForm(Wrap(Settings.Default.AddonsNew));
                //addonForm.ShowDialog();
                //UpdateAddonsList();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString(), System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<AddonSettings> Wrap(AddonInformationList addonInformationList)
        {
            return addonInformationList.Select(info => new AddonSettings(info)).ToList();
        }

        /// <summary>
        /// Установить аддоны с сайта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void InstallAddons(object sender, EventArgs e)
        {
            try
            {
                var webRequest = (HttpWebRequest)WebRequest.Create(String.Format("{0}addons.xml", Settings.Default.UpdatePath));
                webRequest.UserAgent = Program.UserAgentHeader;
                var stream = webRequest.GetResponse().GetResponseStream();
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(stream);

                var dialog = new InstallationDialog(xmlDocument)
                {
                    Text = Resources.AvailableAddons,
                    InstallationFolder = Program.DataStorage.GetDirectoryInfo("AddOns").FullName,
                    SuccessMessage = Resources.AddonInstalledSuccessfully,
                };

                dialog.Install += dialog_Install;
                dialog.Success += () => BrowseAddons(false);

                //dialog.ShowDialog();
                RegisterAsMDIChild(null, dialog, null);

                //BrowseAddons();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void BrowseAddons(bool install = false)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    this.application.AddonsManager.BrowseAddons();
                    UpdateAddonsList();
                    if (install)
                        this.application.AddonsManager.InstallPredefinedAddons();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(string.Format("Ошибка при запуске дополнений: {0}", exc.ToString()), System.Windows.Forms.Application.ProductName);
                }
            });
        }

        void dialog_Install(object sender, InstallEventArgs e)
        {
            if (e.Guid.Length > 0 && Settings.Default.AddonsNew.Exists(addonInfo => addonInfo.Guid.ToLower() == e.Guid.ToLower()))
            {
                e.Install = MessageBox.Show(Resources.AddonIsInstalled, System.Windows.Forms.Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
            }
            else
                e.Install = true;
        }

        private void UpdateAddonsList()
        {
            if (InvokeRequired || this.menuStrip.InvokeRequired  || this.tsmiAddons.DropDown.InvokeRequired)
            {
                this.Invoke(new Action(UpdateAddonsList));
                return;
            }

            while (this.tsmiAddons.DropDownItems.Count > 3)
                this.tsmiAddons.DropDownItems.RemoveAt(0);

            //var list = new AddonInformationList();
            //list.AddRange(Settings.Default.AddonsNew);
            //Settings.Default.AddonsNew.Clear();
            foreach (var info in Settings.Default.AddonsNew)
            {
                try
                {
                    //if (!info.Path.Contains(Program.DataStorage.RootPath) && !info.Path.Contains(Program.ProgramStorage.RootPath)
                    //    || !File.Exists(info.Path)
                    //    || Settings.Default.AddonsNew.Exists(ai => ai.Guid == info.Guid))
                    //    continue;

                    if (info.Info.VisibleInMenu)
                    {
                        var localizedInfo = AddonsManager.GetLocalizationInfoForAddon(info);
                        var addonItem = new ToolStripMenuItem(localizedInfo.Title);
                        addonItem.ToolTipText = string.Format(Resources.AddonDesription, localizedInfo.Description, localizedInfo.Author);
                        addonItem.Tag = info;
                        if (info.Info.StartMode == AddonStartMode.Manual)
                        {
                            addonItem.Click += delegate(object sender1, EventArgs evArgs)
                            {
                                try
                                {
                                    this.application.AddonsManager.RunAddon((sender1 as ToolStripMenuItem).Tag as AddonInformation);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(string.Format(String.Format("{0} {1} {2}", Resources.AddonError, ex.Message, ex.GetType()), Path.GetDirectoryName(info.Path)), Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                }
                            };
                        }
                        tsmiAddons.DropDownItems.Insert(this.tsmiAddons.DropDownItems.Count - 3, addonItem);
                    }                    

                    //if (!Settings.Default.AddonsNew.Exists(addonInfo => addonInfo.Path == info.Path))
                    //    Settings.Default.AddonsNew.Add(info);

                    if (info.Info.StartMode == AddonStartMode.Automatic && !this.application.AddonsManager.IsRunning(info))
                    {
                        this.application.AddonsManager.RunAddon(info);
                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.ToString());
                }
            }

            this.tssAddons.Visible = this.tsmiAddons.DropDownItems.Count > 3;
        }

        private void ConnectTo(ExtendedServerInfo serv)
        {
            try 
            {
                var server = OpenServerWindow(serv);
                if (server != null)
                    server.Connect();
            }
            catch (Exception exc) 
            {
                MessageBox.Show(exc.Message);
            }
        }

        private MDIChildServer OpenServerWindow(ExtendedServerInfo server)
        {
            // Create a new instance of the child form.
            if (UserOptions.Default.Nicks.Count == 0)
            {
                MessageBox.Show(Resources.SpecifyOneNickAtLeast, System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }

            var active = UserOptions.Default.Nicks[Settings.Default.DefUserIndex];

            var childForm = new MDIChildServer(this, new ExtendedConnectionInfo { Server = server, Nick = active, User = UserOptions.Default.User });
            childForm.DataContext = new CIRCeServer(childForm);

            childForm.OnConnected += childForm_OnConnected;
            childForm.NewWindow += childForm_NewWindow;
            childForm.PlayCommand += Play;
            childForm.PauseCommand += Pause;
            childForm.StopCommand += Stop;
            childForm.SeekCommand += Seek;
            childForm.ResumeCommand += Resume;

            var wNode = (WindowNode)RegisterAsMDIChild(null, childForm, null);

            if (wNode != null)
            {
                wNode.ImageIndex = 0;
                wNode.ForeColor = UISettings.Default.ServerDisconnectedColor;
                wNode.ToolTipText = Resources.SettingsAvailability;
                childForm.MyNode = wNode;
            }

            ((Changeable<ICIRCeServer>)this.application.Servers).Add(childForm.DataContext as ICIRCeServer);

            return childForm;
        }

        string Seek(int obj, ulong pos)
        {
            if (this.InvokeRequired)
            {
                var del = new Func<int, ulong, string>(Seek);
                var ar = this.BeginInvoke(del, obj, pos);
                return (string)this.EndInvoke(ar);
            }

            this.player[obj].Seek(pos);
            return string.Empty;
        }

        string Resume(int obj)
        {
            if (this.InvokeRequired)
            {
                var del = new Func<int, string>(Resume);
                var ar = this.BeginInvoke(del, obj);
                return (string)this.EndInvoke(ar);
            }

            this.player[obj].Play();
            return string.Empty;
        }

        string Stop(int obj)
        {
            if (this.InvokeRequired)
            {
                var del = new Func<int, string>(Stop);
                var ar = this.BeginInvoke(del, obj);
                return (string)this.EndInvoke(ar);
            }

            this.player[obj].Stop();
            return string.Empty;
        }

        string Pause(int obj)
        {
            if (this.InvokeRequired)
            {
                var del = new Func<int, string>(Pause);
                var ar = this.BeginInvoke(del, obj);
                return (string)this.EndInvoke(ar);
            }

            this.player[obj].Pause();
            return string.Empty;
        }

        /// <summary>
        /// Открылось новое окно
        /// </summary>
        /// <param name="sender">Окно сервера</param>
        /// <param name="e">Аргумент, содержащий новое окно</param>
        void childForm_NewWindow(object sender, JoinEventArgs e)
        {
            MDIChildServer parent = sender as MDIChildServer;
            MDIChildCommunication child = e.Window as MDIChildCommunication;

            var wNode = (WindowNode)RegisterAsMDIChild(parent.Self, child, null);
            /*WindowNode wNode = new WindowNode(child, child.WindowName, child.WindowName[0] == '#' ? 2 : 4);*/
            wNode.ImageIndex = String.Compare(child.WindowName[0].ToString(), '#'.ToString(), false) == 0 ? 2 : 4;
            wNode.SelectedImageIndex = wNode.ImageIndex;
            wNode.ForeColor = Color.MidnightBlue;
            child.MyNode = wNode;

            /*parent.MyNode.Nodes.Add(wNode);
            parent.MyNode.Expand();

            child.Show();*/
        }

        /// <summary>
        /// Подключение состоялось в полном объёме (с идентификацией)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void childForm_OnConnected(object sender, EventArgs e)
        {
            this.tsmiChannels.Enabled = true;
            this.tsmiChannels.DropDown.Enabled = true;
            this.tsddbChannels.Enabled = true;

            this.tsmiNick.Enabled = true;
            this.tsmiNick.DropDown.Enabled = true;
            this.tsddbNick.Enabled = true;
            this.tsbOpenPrivate.Enabled = true;

            var serverWindow = sender as MDIChildServer;
            if (this.connectionTasks.ContainsKey(serverWindow.MyNode))
            {
                this.connectionTasks[serverWindow.MyNode].ForEach(serverWindow.JoinChannel);
                this.connectionTasks.Remove(serverWindow.MyNode);
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //LayoutMdi(MdiLayout.Cascade);
            Win32.CascadeWindows(this.pMainLayout.Handle, Win32.CascadeMode.MDITILE_SKIPDISABLED | Win32.CascadeMode.MDITILE_ZORDER, IntPtr.Zero, 0, IntPtr.Zero);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //LayoutMdi(MdiLayout.TileVertical);
            Win32.TileWindows(this.pMainLayout.Handle, Win32.TileMode.MDITILE_VERTICAL, IntPtr.Zero, 0, IntPtr.Zero);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //LayoutMdi(MdiLayout.TileHorizontal);
            Win32.TileWindows(this.pMainLayout.Handle, Win32.TileMode.MDITILE_HORIZONTAL, IntPtr.Zero, 0, IntPtr.Zero);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IRCForm[] toDelete;
            lock (childrenSync)
            {
                toDelete = new IRCForm[this.children.Count];
                this.children.CopyTo(toDelete);
            }

            Array.ForEach(toDelete, child => child.Close());
        }

        private void MDIParent_FormClosed(object sender, FormClosedEventArgs e)
        {
            UISettings.Default.PropertyChanged -= Default_PropertyChanged;

            if (this.application != null)
                this.application.Dispose();

            if (this.player != null)
            {
                this.player[0].Stop();
                this.player[1].Stop();
                this.player[2].Stop();
            }
        }

        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var asms = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < asms.Length; ++i)
            {
                if (asms[i].FullName == args.Name)
                    return asms[i];
            }
            return null;
        }

        private void UpdateServList()
        {
            this.tsddbServers.DropDownItems.Clear();

            foreach (var serv in UserOptions.Default.Servers)
            {
                var item = new ToolStripMenuItem(serv.ToString(), null, ConnectToServ);
                item.ImageIndex = 0;
                item.ToolTipText = serv.Name + ": " + serv.Port;
                this.tsddbServers.DropDownItems.Add(item);
            }
            
            this.tsddbServers.DropDownItems.Add(new ToolStripSeparator());
            this.tsddbServers.DropDownItems.Add(String.Format("{0}...", Resources.New), null, NewServer);
            this.tsddbServers.DropDownItems.Add(String.Format("{0}...", Resources.Configure), null, ConfigureServ);
        }

        private void UpdateNicksList()
        {
            this.tSCBNickList.Items.Clear();
            this.tSCBNickList.Items.AddRange(UserOptions.Default.Nicks.ToArray());
            this.tSCBNickList.Items.Add(String.Format("{0}...", Resources.New));
            this.tSCBNickList.Items.Add(String.Format("{0}...", Resources.Configure));

            if (tSCBNickList.Items.Count - 2 <= Settings.Default.DefUserIndex)
                Settings.Default.DefUserIndex = 0;

            tSCBNickList.Text = tSCBNickList.Items[Settings.Default.DefUserIndex].ToString();

            this.tsmiNick.DropDownItems.Clear();
            foreach (var nick in UserOptions.Default.Nicks)
            {
                var item = new ToolStripMenuItem(nick, Resources.selectUser, ChangeNick);
                item.Tag = nick;
                this.tsmiNick.DropDownItems.Add(item);
            }

            this.tsmiNick.DropDownItems.Add(new ToolStripSeparator());
            this.tsmiNick.DropDownItems.Add(String.Format("{0}...", Resources.New), null, NewNickName);
            this.tsmiNick.DropDownItems.Add(String.Format("{0}...", Resources.Configure), null, ConfigureNickNames);

            if (this.tSCBNickList.Items.Count > Settings.Default.DefUserIndex)
                this.tSCBNickList.SelectedIndex = Settings.Default.DefUserIndex;
        }

        private void UpdateChannelsList()
        {
            this.tsmiChannels.DropDownItems.Clear();

            if (this.ActiveIRCWindow != null)
            {
                var list = this.ActiveIRCWindow.ServerWindow.Server.Channels;

                list.Sort((ch1, ch2) =>
                {
                    return ch1.Name.CompareTo(ch2.Name);
                });

                foreach (var channel in list)
                {
                    var item = new ToolStripMenuItem(channel.Name, Resources.channel, JoinChan) { Tag = channel.Name };
                    this.tsmiChannels.DropDownItems.Add(item);
                }
                this.tsmiChannels.DropDownItems.Add(new ToolStripSeparator());
            }

            this.tsmiChannels.DropDownItems.Add(String.Format("{0}...", Resources.New), null, NewChannel);
            this.tsmiChannels.DropDownItems.Add(String.Format("{0}...", Resources.Configure), null, ConfigureChan);
        }

        private void tSCBNickList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tSCBNickList.SelectedIndex == tSCBNickList.Items.Count - 1)
            {
                ConfigureNickNames(sender, e);
            }
            else if (tSCBNickList.SelectedIndex == tSCBNickList.Items.Count - 2)
            {
                NewNickName(tSCBNickList, e);
            }
            else
                Settings.Default.DefUserIndex = tSCBNickList.SelectedIndex;
        }

        private void горячиеКлавишиToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            new HotKeysSettingDialog().ShowDialog();
        }

        private void цветаToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            new ColorChooseDialog(Resources.IRCColors) { Location = MousePosition, WorkMode = ColorChooseDialog.Mode.Edit }.ShowDialog();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByKeyboard)
            {
                ActivateChild(((WindowNode)e.Node).Window);
                tvWindows.Focus();
            }
            else
            {
                this.activeNode = (WindowNode)e.Node;
                if (this.activeNode.Window != null)
                {
                    lock (sync)
                    {
                        ActivateChild(this.activeNode.Window);
                    }
                }
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {            
            if (e.Button == MouseButtons.Right && e.Node.Level == 0)
            {
                var child = ((WindowNode)e.Node).Window as MDIChildServer;
                if (child != null)
                {
                    servNode = e.Node as WindowNode;
                    servNode.TreeView.SelectedNode = servNode;
                }
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            servNode.Window.Close();
        }

        private void tSMIReconnect_Click(object sender, EventArgs e)
        {
            ((MDIChildServer)servNode.Window).Connect();
        }

        private void tSMIAbout_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SettingsDialog().ShowDialog();
        }

        private void ConfigureUser(object sender, EventArgs e)
        {
            ShowDialog(UserOptions.Default.User, "Личные данные");
        }

        #region IMainWindow Members

        public MDIChild ActiveIRCWindow
        {
            get
            {
                return GetActiveIRCChild();
            }
        }

        private delegate MDIChild GetActiveIRCChildDel();

        private MDIChild GetActiveIRCChild()
        {
            if (this.InvokeRequired)
            {
                GetActiveIRCChildDel showDialogDel = GetActiveIRCChild;
                var result = this.BeginInvoke(showDialogDel, null);
                return (MDIChild)this.EndInvoke(result);
            }
            else
            {
                object activeForm = GetActiveChild();
                while (activeForm != null && !(activeForm is MDIChild))
                {
                    activeForm = GetWindowOwner((Form)activeForm);
                }
                return (MDIChild)activeForm;
            }
        }

        private delegate DialogResult ShowDialogDel(Form form);

        public DialogResult ShowDialog(Form dialogForm)
        {
            if (this.InvokeRequired)
            {
                ShowDialogDel showDialogDel = new ShowDialogDel(ShowDialog);
                IAsyncResult result = this.BeginInvoke(showDialogDel, new object[] { dialogForm });
                return (DialogResult)this.EndInvoke(result);
            }
            else
            {
                return dialogForm.ShowDialog();
            }
        }

        internal IEnumerable<MDIChildServer> GetServerWindows()
        {
            if (this.InvokeRequired)
            {
                return (IEnumerable<MDIChildServer>)this.Invoke((Func<IEnumerable<MDIChildServer>>)GetServerWindows);
            }

            return this.tvWindows.Nodes.Cast<WindowNode>().Select(wn => wn.Window).OfType<MDIChildServer>();
        }

        private delegate MDIChildServer OpenConnectionDel(ExtendedConnectionInfo info);

        /// <summary>
        /// Открыть соединение с сервером
        /// </summary>
        /// <param name="server">Сервер</param>
        /// <param name="user">Пользователь, соединяющийся с сервером</param>
        /// <returns></returns>
        public MDIChildServer OpenConnection(ExtendedConnectionInfo info)
        {
            if (this.InvokeRequired)
            {
                OpenConnectionDel openConnectionDel = OpenConnection;
                var result = this.BeginInvoke(openConnectionDel, info);
                return (MDIChildServer)this.EndInvoke(result);
            }
            else
            {
                if (info.Nick != null)
                {
                    Settings.Default.DefUserIndex = UserOptions.Default.Nicks.IndexOf(info.Nick);
                    if (Settings.Default.DefUserIndex == -1)
                    {
                        UserOptions.Default.Nicks.Add(info.Nick);
                        Settings.Default.DefUserIndex = UserOptions.Default.Nicks.Count - 1;

                        UpdateNicksList();
                    }                    
                }
                else if (UserOptions.Default.Nicks.Count > 0)
                {
                    info.Nick = UserOptions.Default.Nicks[Settings.Default.DefUserIndex];
                }
                else
                    return null;

                var server = UserOptions.Default.Servers.Find(serv => serv.Equals(info.Server));
                if (server != null)
                {
                    info.Server.Description = server.Description;
                    info.Server.Passwords = server.Passwords;
                }

                var serverWindows = this.GetServerWindows();
                foreach (var window in serverWindows)
                {
                    if (window.Server.Equals(info.Server) && window.Nick.Equals(info.Nick))
                        return window;
                }

                return OpenServerWindow(info.Server);
            }
        }

        private delegate void StatusDel(string statusString);

        /// <summary>
        /// Установить статусное сообщение
        /// </summary>
        /// <param name="statusString">Статусное сообщение</param>
        public void Status(string statusString)
        {
            if (this.InvokeRequired)
            {
                StatusDel statusDel = new StatusDel(Status);
                this.BeginInvoke(statusDel, new object[] { statusString });
            }
            else
            {
                this.tSSLabel.Text = statusString;
            }
        }

        private delegate DialogResult MessageBoxDel(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon);
        
        public DialogResult ShowMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            if (this.InvokeRequired)
            {
                MessageBoxDel mbDel = new MessageBoxDel(ShowMessageBox);
                IAsyncResult result = this.BeginInvoke(mbDel, new object[] { text, caption, buttons, icon });
                return (DialogResult)this.EndInvoke(result);
            }
            else
            {
                return MessageBox.Show(text, caption, buttons, icon);
            }
        }

        private MediaView mediaView = null;
        private static string[] ImageExtensions = new string[] { ".bmp", ".png", ".jpg", ".jpeg", ".gif", ".tif", ".tiff", ".pic" };

        public string Play(string multimediaFile, int numOfPlayer, bool loop = false, ulong initialPosition = 0, MDIChild executor = null)
        {
            if (this.InvokeRequired)
            {
                return (string)this.EndInvoke(this.BeginInvoke(new Func<string, int, bool, ulong, MDIChild, string>(Play), multimediaFile, numOfPlayer, loop, initialPosition, executor));
            }

            try
            {
                if (multimediaFile.Length == 0)
                {
                    player[numOfPlayer].Close();
                }
                else
                {
                    if (!Path.IsPathRooted(multimediaFile))
                    {
                        var info = Program.DataStorage.GetDirectoryInfo("Media");//new DirectoryInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Media"));
                        var res = info.GetFiles(multimediaFile, SearchOption.AllDirectories);
                        if (res.Length == 0)
                            return UISettings.Default.NotifyOnNoMedia ? String.Format("{0}: {1}", multimediaFile, "Файл не найден") : "";

                        multimediaFile = res[0].FullName;
                    }

                    var ext = Path.GetExtension(multimediaFile);
                    if (ImageExtensions.Contains(ext.ToLower()))
                    {
                        // Новый (и в то же время переходный) код
                        if (this.mediaView == null)
                        {
                            this.mediaView = new MediaView { DataContext = multimediaFile };
                            var host = new System.Windows.Forms.Integration.ElementHost { Child = this.mediaView, Dock = DockStyle.Fill };
                            var hostForm = new IRCProviders.IRCForm { Text = "Изображение", ShowIcon = false };

                            hostForm.Controls.Add(host);
                            hostForm.FormClosed += (sender, e) =>
                                {
                                    this.mediaView = null;
                                };

                            RegisterAsMDIChild((IRCForm)executor, hostForm, null);                            
                        }
                        else
                            this.mediaView.DataContext = multimediaFile;

                        return "";
                    }

                    var shortPath = ArchiveManager.GetShortPath(multimediaFile);
                    var currentPlayer = player[numOfPlayer];
                    
                    currentPlayer.Open(shortPath);
                    currentPlayer.Looping = loop;
                    currentPlayer.Play();
                    if (initialPosition > 0)
                        currentPlayer.Seek(initialPosition);
                }
                return string.Empty;
            }
            catch (FileNotFoundException exc)
            {
                return UISettings.Default.NotifyOnNoMedia ? String.Format("{0}: {1}", multimediaFile, exc.Message) : "";
            }
            catch (Exception exc)
            {
                return String.Format("{0}: {1}", multimediaFile, exc.Message);
            }    
        }
        
        /// <summary>
        /// Помигать
        /// </summary>
        public void Flash()
        {
            if (InvokeRequired)
            {
                Action act = Flash;
                this.BeginInvoke(act, null);
            }
            else
            {
                Flasher.Flash(this);
            }
        }

        #endregion

        private void tSMINotice_Click(object sender, EventArgs e)
        {
            Process.Start(Uri.EscapeUriString(Resources.AuthorSiteUrl));
        }

        #region IMainWindow Members

        private static bool JoinToNode(TreeNodeCollection treeNodeCollection, IRCForm owner, WindowNode wNode)
        {
            foreach (WindowNode node in treeNodeCollection)
            {
                if (node.Window == owner)
                {
                    node.Nodes.Add(wNode);
                    node.Expand();
                    return true;
                }
                if (JoinToNode(node.Nodes, owner, wNode))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Ник по умолчанию
        /// </summary>
        public string DefaultNick
        {
            get 
            {
                if (UserOptions.Default.Nicks.Count == 0)
                    return null;

                return UserOptions.Default.Nicks[Settings.Default.DefUserIndex]; 
            }
        }

        public IRCForm GetWindowOwner(Form form)
        {
            if (this.InvokeRequired)
            {
                var del = new Func<Form, object>(GetWindowOwner);
                IAsyncResult ar = this.BeginInvoke(del, new object[] { form });
                return (IRCForm)this.EndInvoke(ar);
            }
            else
            {
                WindowNode node = FindNode(form);
                if (node == null || node.Parent == null)
                    return null;
                return ((WindowNode)node.Parent).Window;
            }
        }

        private WindowNode FindNode(Form form)
        {
            return FindNode(tvWindows.Nodes, form);//treeViewWindows.Nodes, form);
        }

        private static WindowNode FindNode(TreeNodeCollection treeNodeCollection, Form form)
        {
            foreach (WindowNode node in treeNodeCollection)
            {
                if (node.Window == form)
                    return node;
                WindowNode nodeRes = FindNode(node.Nodes, form);
                if (nodeRes != null)
                    return nodeRes;
            }
            return null;
        }

        #endregion

        private void tsmiCustomize_Click(object sender, EventArgs e)
        {
            try
            {
                Settings.Default.Save();
                UISettings.Default.Save();
                UserOptions.Default.Save();
                using (var dialog = new CustomizationDialog())
                {
                    var result = dialog.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        Settings.Default.Save();
                        UISettings.Default.Save();
                        UserOptions.Default.Save();
                    }
                    else
                    {
                        Settings.Default.Reload();
                        UISettings.Default.Reload();
#if DEBUG
                        UserOptions.Default.Nicks.Clear();
                        UserOptions.Default.Nicks.Add("TestUser1");
                        UserOptions.Default.Nicks.Add("TestUser2");
                        UISettings.Default.ShowUrlOnCmd = true;
#endif
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MDIParent_Activated(object sender, EventArgs e)
        {
            var active = this.GetActiveChild();
            
            if (active != null)
            {
                Win32.SendMessage(active.Handle, (uint)Win32.WM_NCACTIVATE, (IntPtr)1, IntPtr.Zero);
                Win32.SetFocus(active.Handle);

                var child = active as MDIChild;
                if (child != null)
                    child.BeginRead();
            }
        }

        private void tsmiViewTreeView_CheckedChanged(object sender, EventArgs e)
        {
            this.splitter1.Visible = this.panel1.Visible = this.tsmiViewTreeView.Checked;
        }

        private void tsmiAlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = this.tsmiAlwaysOnTop.Checked;
        }

        private void tsmiHelp_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "help.chm");
        }

        private void MDIParent_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                CloseAllToolStripMenuItem_Click(sender, e);
            }
            catch (Exception) { }

            Settings.Default.ServersVisibility = this.tsddbServers.Visible;
            Settings.Default.NickListVisibility = this.tSCBNickList.Visible;
            Settings.Default.ChannelsVisibility = this.tsddbChannels.Visible;
            Settings.Default.NicksVisibility = this.tsddbNick.Visible;
            Settings.Default.PersonVisibility = this.tSBUser.Visible;
            Settings.Default.SettingsVisibility = this.tsddbSettings.Visible;
        }

        private void tsmiCompact_CheckedChanged(object sender, EventArgs e)
        {
            this.tsmiToolBar.Checked = !this.tsmiCompact.Checked;
            this.tsmiStatusBar.Checked = !this.tsmiCompact.Checked;
            this.tsmiViewTreeView.Checked = !this.tsmiCompact.Checked;
            this.tsmiAlwaysOnTop.Checked = this.tsmiCompact.Checked;
            this.Opacity = this.tsmiCompact.Checked ? 0.9 : 1;

            if (this.tsmiCompact.Checked)
            {
                this.WindowState = FormWindowState.Normal;
                this.Size = new Size(500, 250);
                this.Location = new Point(Screen.PrimaryScreen.Bounds.Width - 500, Screen.PrimaryScreen.Bounds.Height - 280);
                var active = this.GetActiveChild();
                if (active != null && active.MaximizeBox)
                    active.WindowState = FormWindowState.Maximized;
                if (active != null && active == this.ActiveIRCWindow)
                    (active as MDIChild).HideInputEditor();
            }
            else
                this.WindowState = FormWindowState.Maximized;
        }

        private void tsmiStatusBar_CheckedChanged(object sender, EventArgs e)
        {
            statusStrip.Visible = tsmiStatusBar.Checked;
        }

        private void tsmiToolBar_CheckedChanged(object sender, EventArgs e)
        {
            toolStrip.Visible = tsmiToolBar.Checked;
        }

        #region IMainWindow Members

        private delegate object CreateObjectDelegate(AppDomain domain, Type formType, params object[] args);

        public object CreateObject(AppDomain domain, Type objectType, params object[] args)
        {
            if (this.InvokeRequired)
            {
                CreateObjectDelegate del = new CreateObjectDelegate(CreateObject);
                IAsyncResult ar = this.BeginInvoke(del, domain, objectType, args);
                return this.EndInvoke(ar);
            }
            else
            {
                try
                {
                    if (domain == null)
                        return Activator.CreateInstance(objectType, args);
                    return domain.CreateInstanceAndUnwrap(objectType.Assembly.FullName, objectType.FullName, true, BindingFlags.Default, null, args, CultureInfo.CurrentCulture, null, null);
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.ToString());
                    return null;
                }
            }
        }

        public void DestroyObject(object obj)
        {
            if (this.InvokeRequired)
            {
                var ar = this.BeginInvoke(new Action<object>(DestroyObject), obj);
                this.EndInvoke(ar);
                return;
            }

            var disposable = obj as IDisposable;
            if (disposable != null)
                disposable.Dispose();
        }

        private delegate void RunCallbackDel(Action<object> func, object param);

        public void RunCallback(Action<object> func, object param)
        {
            if (this.InvokeRequired)
            {
                RunCallbackDel del = new RunCallbackDel(RunCallback);
                IAsyncResult ar = this.BeginInvoke(del, func, param);
                this.EndInvoke(ar);
            }
            else
            {
                func(param);
            }
        }

        public void RunCallback(Action func)
        {
            if (this.InvokeRequired)
            {
                var del = new Action<Action>(RunCallback);
                this.Invoke(del, func);
            }
            else
            {
                func();
            }
        }

        #endregion

        private void tsmiLogs_Click(object sender, EventArgs e)
        {
            Process.Start(Program.DataStorage.GetDirectoryInfo("Logs").FullName);
        }

        private void tsmiMedia_Click(object sender, EventArgs e)
        {
            Process.Start(Program.DataStorage.GetDirectoryInfo("Media").FullName);
        }

        private void tsmiInstallFormDisk_Click(object sender, EventArgs e)
        {
            //var openFileDialog = new OpenFileDialog { Filter = String.Format("{0}|*.exe", Resources.SFXAddon), DefaultExt = ".exe" };
            //if (openFileDialog.ShowDialog() == DialogResult.OK)
            //{
            //    var destination = Program.DataStorage.GetDirectoryInfo("AddOns").FullName;
            //    Directory.SetCurrentDirectory(destination);
            //    destination = Path.Combine(destination, Path.GetFileName(openFileDialog.FileName));
            //    Program.DataStorage.InsertFile(openFileDialog.FileName, destination, true);

            //    Process process = new Process();
            //    process.StartInfo.FileName = Program.DataStorage.RealPath(destination);
            //    process.Start();
            //    process.WaitForExit();
            //    Program.DataStorage.DeleteFile(destination);

            //    MessageBox.Show(Resources.AddonInstalledSuccessfully);

            //    BrowseAddons();
            //    Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            //}
            var openFileDialog = new OpenFileDialog { Filter = String.Format("{0}|*.zip", "Архив с дополнением"), DefaultExt = ".zip" };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var destination = Program.DataStorage.GetDirectoryInfo("AddOns").FullName;
                
                destination = Path.Combine(destination, Path.GetFileName(openFileDialog.FileName));
                Program.DataStorage.InsertFile(openFileDialog.FileName, destination, true);

                if (ArchiveManager.ExtractArchive(destination))
                {
                    MessageBox.Show(Resources.AddonInstalledSuccessfully);
                    BrowseAddons();
                }
            }
        }

        #region IMainWindow Members

        /// <summary>
        /// Создать дочернее окно
        /// </summary>
        /// <param name="child"></param>
        private void RegisterBase(IRCForm child)
        {
            var item = new ToolStripMenuItem(child.Text);
            item.Click += (sender2, e2) => ActivateChild(child);
            this.windowsMenu.DropDownItems.Add(item);

            child.TextChanged += (sender2, e2) => item.Text = child.Text;
            child.FormClosed += (sender2, e2) =>
                {
                    if (child.WindowState != FormWindowState.Normal)
                        return;

                    WindowInfo wInfo = null;
                    if (this.Windows.TryGetValue(child.Id, out wInfo))
                    {
                        wInfo.Location = child.Location;
                        wInfo.Size = child.Size;
                    }
                    else
                    {
                        wInfo = new WindowInfo() { Id = child.Id, Size = child.Size, Location = child.Location };
                        Settings.Default.Windows.Add(wInfo);
                        this.Windows[child.Id] = wInfo;
                    }
                };

            child.Disposed += (sender2, e2) =>
            {
                this.windowsMenu.DropDownItems.Remove(item);
                UpdateView();
                lock (this.childrenSync)
                {
                    children.Remove(child);
                    this.tssWindows.Visible = this.children.Count > 0;
                    this.tsbOpenPrivate.Enabled = this.tsddbChannels.Enabled = this.tsddbNick.Enabled = this.children.Count > 0;
                }
            };

            this.tssWindows.Visible = true;

            var active = GetActiveChild();

            this.pMainLayout.Controls.Add(child as Control);

            if (active != null && child.MaximizeBox == true && active.WindowState != FormWindowState.Minimized)
            {
                child.WindowState = active.WindowState;
                if (active.WindowState == FormWindowState.Maximized)
                {
                    var style = Win32.GetWindowLongFunc(child.Handle, Win32.GWL_STYLE);
                    Win32.SetWindowLongFunc(child.Handle, Win32.GWL_STYLE, style & ~Win32.WS_CAPTION);
                    //child.WindowState = FormWindowState.Maximized;
                }
            }

            child.Activated += child_Activated;
            child.TextChanged += child_TextChanged;
            child.Maximized += child_Maximized;
            child.Restored += child_Restored;
            child.Minimized += child_Minimized;

            lock (this.childrenSync)
            {
                WindowInfo wInfo = null;
                if (this.Windows.TryGetValue(child.Id, out wInfo))
                {
                    child.Location = wInfo.Location;
                    child.Size = wInfo.Size;
                }
                else
                {
                    if (this.children.Count == 0)
                        child.Location = new Point(1, 1);
                    else
                        child.Location = new Point(active.Left + 25, active.Top + 25);
                }

                this.children.Add(child);
            }
        }

        void child_TextChanged(object sender, EventArgs e)
        {
            UpdateView();
        }

        void child_Minimized(object sender, EventArgs e)
        {
            ActivateChild(this.GetActiveChild());
            UpdateView();
        }

        void child_Restored(object sender, EventArgs e)
        {
            UpdateView();
        }

        void child_Maximized(object sender, EventArgs e)
        {
            var form = sender as IRCForm;

            if (firstTime)
            {
                firstTime = false;
                blockActivates = true;
                lock (childrenSync)
                    this.children.ForEach(child =>
                        {
                            if (child.WindowState == FormWindowState.Normal && child.MaximizeBox && child != sender)
                            {
                                child.WindowState = FormWindowState.Maximized;
                            }
                        });

                form.BringToFront();
                blockActivates = false;
                firstTime = true;

                UpdateView();
            }
        }

        #endregion

        public IRCForm GetActiveChild()
        {
            if (this.pMainLayout.InvokeRequired)
            {
                var del = new Func<IRCForm>(GetActiveChild);
                var ar = this.BeginInvoke(del);
                return (IRCForm)this.EndInvoke(ar);
            }
            else
            {
                if (this.pMainLayout.IsDisposed || this.pMainLayout.Controls.Count == 0) return null;
                return this.pMainLayout.Controls[0] as IRCForm;
            }
        }
        
        private FormWindowState ActiveChildState
        {
            get
            {
                var active = GetActiveChild();
                if (active == null)
                    return FormWindowState.Normal;
                return active.WindowState;
            }
        }

        private void UpdateView()
        {
            this.ControlBoxVisible = this.ShowChildControlBox;
            var active = this.GetActiveChild();
            this.Text = System.Windows.Forms.Application.ProductName + (active != null && active.WindowState == FormWindowState.Maximized ? string.Format(" - [{0}]", active.Text) : "");
        }

        public bool ShowChildControlBox 
        {
            get 
            {
                return ActiveChildState == FormWindowState.Maximized;
            }
            set
            {

            }
        }

        [Bindable(true)]
        public bool ControlBoxVisible
        {
            get
            {
                return this.minimizeBox.Visible;
            }
            set
            {
                this.minimizeBox.Visible = this.maximizeBox.Visible = this.closeBox.Visible = value;
            }
        }

        private IRCForm oldActiveChild = null;
        private static bool blockActivates = false;
        public static bool BlockActivation { get { return blockActivates; } }

        private Dictionary<IntPtr, bool> visualThreads = new Dictionary<IntPtr, bool>();
        private object sync = new object();

        void child_Activated(object sender, EventArgs e)
        {
            var visual = (IRCForm)sender;

            if (blockActivates)
                return;

            if (visual == oldActiveChild)
                return;

            UpdateView();
            UpdateChannelsList();

            if (oldActiveChild != null && !oldActiveChild.Disposing && !oldActiveChild.IsDisposed)
            {
                lock (sync)
                {
                    Win32.SendMessage(oldActiveChild.Handle, (uint)Win32.WM_NCACTIVATE, (IntPtr)0, IntPtr.Zero);
                    oldActiveChild.DeactivateMe();

                    foreach (var item in this.windowsMenu.DropDownItems)
                    {
                        var menuItem = item as ToolStripMenuItem;
                        if (menuItem != null && menuItem.Text == oldActiveChild.Text)
                        {
                            menuItem.Checked = false;
                            break;
                        }
                    }
                }
            }

            Win32.SendMessage(visual.Handle, (uint)Win32.WM_NCACTIVATE, (IntPtr)1, IntPtr.Zero);
            
            oldActiveChild = visual;

            foreach (var item in this.windowsMenu.DropDownItems)
            {
                var menuItem = item as ToolStripMenuItem;
                if (menuItem != null && menuItem.Text == visual.Text)
                {
                    menuItem.Checked = true;
                    break;
                }
            }
        }

        public void ActivateMe(object arg)
        {
            if (this.InvokeRequired && !this.Disposing && !this.IsDisposed)
            {
                Action<object> func = ActivateMe;
                try
                {
                    var ar = this.BeginInvoke(func, arg);
                    this.EndInvoke(ar);
                }
                catch (Exception)
                {
                }
            }
            else
            {
                lock (sync)
                {
                    var visualHandle = (IntPtr)arg;
                    if (visualThreads[visualHandle])
                    {
                        var style = Win32.GetWindowLongFunc(visualHandle, Win32.GWL_STYLE);
                        Win32.SetWindowLongFunc(visualHandle, Win32.GWL_STYLE, (style | Win32.WS_CHILD) & ~Win32.WS_POPUP);
                        this.Activate();
                        try
                        {
                            Win32.SetFocus(visualHandle);
                        }
                        catch (Exception exc)
                        {
                            MessageBox.Show(exc.Message);
                        }
                    }
                    visualThreads.Remove(visualHandle);
                }
            }
        }

        public static void ActivateChild(IRCForm child)
        {
            if (child == null)
                return;
            
            if (!child.Visible)
                child.Show();

            Win32.SendMessage(child.Handle, (uint)Win32.WM_MOUSEACTIVATE, IntPtr.Zero, IntPtr.Zero);
        }

        private void minimizeBox_Click(object sender, EventArgs e)
        {
            var active = GetActiveChild();
            if (active != null)
                active.WindowState = FormWindowState.Minimized;
        }

        private void maximizeBox_Click(object sender, EventArgs e)
        {
            lock (this.childrenSync)
            {
                var active = this.GetActiveChild();

                blockActivates = true;
                this.children.ForEach(child =>
                {
                    if (child.WindowState == FormWindowState.Maximized)
                    {
                        //if (child != active)
                        //    child.Visible = false;

                        child.WindowState = FormWindowState.Normal;
                        Win32.ShowWindow(child.Handle, Win32.SW_SHOWNORMAL);
                        //if (child != active)
                        //    child.Visible = true;
                    }
                });
                blockActivates = false;
                
                active.BringToFront();
                //form_Activated(active, EventArgs.Empty);
            }
        }

        private void closeBox_Click(object sender, EventArgs e)
        {
            var active = GetActiveChild();
            if (active != null)
            {
                try
                {
                    active.Close();
                }
                catch (Exception exc) { MessageBox.Show(exc.Message); }
            }
        }

        private void MDIParent_MouseClick(object sender, MouseEventArgs e)
        {
            if (!this.pMainLayout.ClientRectangle.Contains(e.Location))
                this.Focus();
        }

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            IntPtr p = IntPtr.Zero;
            do
            {
                p = Win32.FindWindowEx(this.pMainLayout.Handle, p, null, null);
                if (p != IntPtr.Zero)
                {
                    lock (this.childrenSync)
                        foreach (var child in this.children)
                        {
                            if (child.Handle == p)
                                yield return child;
                        }
                }
            } while (p != IntPtr.Zero); 
        }

        #endregion

        #region IEnumerable<CIRCeWindow> Members

        IEnumerator<IRCForm> IEnumerable<IRCForm>.GetEnumerator()
        {
            IntPtr p = IntPtr.Zero;
            do
            {
                p = Win32.FindWindowEx(this.pMainLayout.Handle, p, null, null);
                if (p != IntPtr.Zero)
                {
                    lock (this.childrenSync)
                    {
                        foreach (var child in this.children)
                        {
                            if (child.Handle == p)
                                yield return child;
                        }
                    }
                }
            } while (p != IntPtr.Zero);
        }

        #endregion

        #region IMainWindow Members

        public IFormNode RegisterAsMDIChild(IRCForm owner, IRCForm child, IWin32Window ownerForm)
        {
            if (this.Disposing)
                return null;

            if (this.InvokeRequired)
            {
                var showDialogDel = new Func<IRCForm, IRCForm, IWin32Window, IFormNode>(RegisterAsMDIChild);
                IAsyncResult ar = this.BeginInvoke(showDialogDel, owner, child, ownerForm);
                return (IFormNode)this.EndInvoke(ar);
            }
            else
            {
                RegisterBase(child);
                
                WindowNode wNode = null;

                if (owner == null)
                {
                    bool found = false;
                    if (child is MDIChildServer)
                        foreach (WindowNode item in this.tvWindows.Nodes)
                        {
                            if (item.Sticked && item.Window == null && item.Tag == ((MDIChildServer)child).Server)
                            {
                                wNode = item;
                                wNode.Window = child;
                                this.tvWindows.SelectedNode = null;
                                found = true;
                                break;
                            }
                        }
                    
                    if (!found)
                    {
                        wNode = new WindowNode(child, child.Text, -1);

                        tvWindows.Nodes.Add(wNode);
                        tvWindows.Sort();
                    }
                }
                else
                {
                    bool found = false;
                    if (child is MDIChildChannel)
                        foreach (WindowNode sItem in this.tvWindows.Nodes)
                        {
                            foreach (WindowNode item in sItem.Nodes)
                            {
                                if (item.Sticked && item.Window == null && item.Tag == ((MDIChildChannel)child).Channel)
                                {
                                    wNode = item;
                                    wNode.Window = child;
                                    this.tvWindows.SelectedNode = null;
                                    found = true;
                                    break;
                                }
                            }
                            if (found)
                                break;
                        }

                    if (!found)
                    {
                        wNode = new WindowNode(child, child.Text);
                        JoinToNode(tvWindows.Nodes, owner, wNode);
                    }
                }

                try
                {
                    if (child.ShowIcon)
                        wNode.Image = child.Icon.ToBitmap();
                }
                catch (Exception) { wNode.ImageIndex = 0; }

                //if (TaskbarManager.IsPlatformSupported)
                //{
                //    child.Load += child_Load;
                //    child.FormClosed += child_FormClosed;
                //}
                
                child.Show();

                this.tvWindows.SelectedNode = wNode;
                return wNode;
            }
        }

        private bool closingFromThumbnail = false;

        void child_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (TaskbarManager.IsPlatformSupported && !this.closingFromThumbnail)
            {
                TaskbarManager.Instance.TabbedThumbnail.RemoveThumbnailPreview(((IRCForm)sender).Handle);
            }
        }

        void child_Load(object sender, EventArgs e)
        {
            var child = (IRCForm)sender;
            child.Load -= child_Load;

            var preview = new TabbedThumbnail(this.Handle, child.Handle) { Title = child.Text };
            child.TextChanged += (sender2, e2) => preview.Title = child.Text;
            preview.SetWindowIcon(child.Icon);
            this.thumbnailTable[preview] = child;

            //preview.TabbedThumbnailMaximized += new EventHandler<TabbedThumbnailEventArgs>(preview_TabbedThumbnailMaximized);
            preview.TabbedThumbnailClosed += preview_TabbedThumbnailClosed;
            preview.TabbedThumbnailActivated += preview_TabbedThumbnailActivated;
            //preview.TabbedThumbnailBitmapRequested += preview_TabbedThumbnailBitmapRequested;


            TaskbarManager.Instance.TabbedThumbnail.AddThumbnailPreview(preview);
        }

        void preview_TabbedThumbnailActivated(object sender, TabbedThumbnailEventArgs e)
        {
            this.Activate();
            var preview = (TabbedThumbnail)sender;
            this.thumbnailTable[preview].BringToFront();
        }

        void preview_TabbedThumbnailBitmapRequested(object sender, TabbedThumbnailBitmapRequestedEventArgs e)
        {
            e.Handled = UpdateCurrentPreview((TabbedThumbnail)sender);
        }

        private bool UpdateCurrentPreview(TabbedThumbnail thumbnail = null)
        {
            if (!TaskbarManager.IsPlatformSupported)
                return false;

            var form = this.thumbnailTable[thumbnail];
            if (form != null)
            {
                var preview = TaskbarManager.Instance.TabbedThumbnail.GetThumbnailPreview(form.Handle);

                if (preview != null && (preview == thumbnail || thumbnail == null))
                {
                    if (form.Width < 1 || form.Height < 1)
                        return false;

                    var bitmap = new Bitmap(form.Width, form.Height);
                    form.DrawToBitmap(bitmap, new Rectangle(0, 0, this.Width, this.Height));
                    preview.SetImage(bitmap);
                    
                    return true;
                }
            }
            return false;
        }
        
        void preview_TabbedThumbnailMaximized(object sender, TabbedThumbnailEventArgs e)
        {
            var preview = (TabbedThumbnail)sender;
            this.thumbnailTable[preview].WindowState = FormWindowState.Maximized;
        }

        private Dictionary<TabbedThumbnail, IRCForm> thumbnailTable = new Dictionary<TabbedThumbnail, IRCForm>();

        void preview_TabbedThumbnailClosed(object sender, TabbedThumbnailClosedEventArgs e)
        {
            var preview = (TabbedThumbnail)sender;
            this.closingFromThumbnail = true;
            this.thumbnailTable[preview].Close();
            this.closingFromThumbnail = false;

            preview.TabbedThumbnailClosed -= preview_TabbedThumbnailClosed;
        }

        #endregion

        /// <summary>
        /// Установить мультимедиа с сайта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiMediaDownload_Click(object sender, EventArgs e)
        {
            try
            {
                var webRequest = (HttpWebRequest)WebRequest.Create(String.Format("{0}media.xml", Settings.Default.UpdatePath));
                webRequest.UserAgent = Program.UserAgentHeader;
                var stream = webRequest.GetResponse().GetResponseStream();
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(stream);
                
                var dialog = new InstallationDialog(xmlDocument)
                {
                    Text = Resources.AvailableMultimedia,
                    InstallationFolder = Program.DataStorage.GetDirectoryInfo("Media").FullName,
                    SuccessMessage = Resources.MediaInstalledSuccessfully
                };

                //dialog.ShowDialog();
                RegisterAsMDIChild(null, dialog, null);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        #region IMainWindow Members

        public string DataFolderPath
        {
            get { return Program.ProgramOptions.AppDataFolder; }
        }

        #endregion

        private void pMainLayout_SizeChanged(object sender, EventArgs e)
        {
            if (blockActivates)
                return;

            blockActivates = true;
            var active = this.GetActiveChild();

            this.pMainLayout.UpdateSize();

            ActivateChild(active);

            blockActivates = false;
        }

        /// <summary>
        /// Открыть место хранения кэша
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiCachePlace_Click(object sender, EventArgs e)
        {
            Process.Start(Program.LocalDataStorage.GetDirectoryInfo("Cache").FullName);
        }

        /// <summary>
        /// Очистить кэш
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiCleanCache_Click(object sender, EventArgs e)
        {
            try
            {
                var cursor = this.Cursor;
                this.Cursor = Cursors.WaitCursor;

                var dir = Program.LocalDataStorage.GetDirectoryInfo("Cache");
                foreach (var item in dir.GetFiles())
                {
                    item.Delete();
                }
                foreach (var item in dir.GetDirectories())
                {
                    item.Delete(true);
                }

                this.Cursor = cursor;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void tsmiMediaPanel_Click(object sender, EventArgs e)
        {
            var panel = new MultimediaForm(this);
            RegisterAsMDIChild(null, panel, null);
        }

        private void tsbOpenPrivate_Click(object sender, EventArgs e)
        {
            var dialog = new StringEnterDialog(Resources.EnterNick);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                MDIChild child = ActiveIRCWindow;
                if (child != null)
                {
                    child.ServerWindow.OpenWindow(dialog.PrintedText);
                }
            }
        }

        private void MDIParent_Load(object sender, EventArgs e)
        {
            try
            {
#if DEBUG
                UserOptions.Default.Nicks.Clear();
                UserOptions.Default.Nicks.Add("TestUser1");
                UserOptions.Default.Nicks.Add("TestUser2");
                UISettings.Default.ShowUrlOnCmd = true;
#else
            if (Settings.Default.FirstRun)
            {
                Settings.Default.FirstRun = false;

                if (Settings.Default.Nicks.Count == 0)
                {
                    var dialog = new FirstDialog();
                    dialog.ShowDialog();
                    var name = dialog.Nick;
                    if (name.Length > 0)
                        Settings.Default.Nicks.Insert(0, new NickName(name));
                }
            }
#endif

                Program.DataStorage.CreateDirectory("AddOns");
                Program.DataStorage.CreateDirectory("Logs");
                Program.DataStorage.CreateDirectory("Media");

                Program.LocalDataStorage.CreateDirectory("Cache");

                this.player[0] = new MediaPlayer("First");
                this.player[1] = new MediaPlayer("Second");
                this.player[2] = new MediaPlayer("Url");

                foreach (var item in this.player)
                {
                    item.Error += new MediaPlayer.ErrorEventHandler(item_Error);
                }

                foreach (var item in Settings.Default.Windows)
                {
                    this.Windows[item.Id] = item;
                }

                Status(string.Empty);
                this.statusStrip.Select();

                BrowseAddons(true);

                UpdateServList();
                UpdateChannelsList();
                UpdateNicksList();

                UserOptions.Default.Servers.FindAll(server => server.Sticked).ForEach(server =>
                    {
                        var node = new WindowNode(null, string.IsNullOrEmpty(server.Description) ? server.Name : server.Description)
                        {
                            ForeColor = Color.PaleVioletRed,
                            Tag = server,
                            Sticked = true
                        };

                        this.tvWindows.Nodes.Add(node);

                        server.Channels.FindAll(channel => channel.Sticked).ForEach(channel =>
                            {
                                var node2 = new WindowNode(null, channel.Name)
                                {
                                    ForeColor = Color.PaleVioletRed,
                                    Tag = channel,
                                    Sticked = true
                                };
                                node.Nodes.Add(node2);
                            }
                        );
                    }
                );

                this.tvWindows.ExpandAll();

                UserOptions.Default.Servers.FindAll(server => server.AutoOpen).ForEach(server => ConnectTo(server));

                this.tvWindows.Sort();

                foreach (ToolStripItem item in this.toolStrip.Items)
                {
                    var tsItem = new ToolStripMenuItem(item.Text, null, ChangeVisibility) { CheckOnClick = true, Checked = item.Visible, Tag = item };
                    this.cmsTools.Items.Add(tsItem);
                }

                var ircRegex = new Regex(@"irc://(?<server>[^:/]+)(\:(?<port>\d+))?(/(?<channel>.+))?$");
                foreach (var link in this.args)
                {
                    var match = ircRegex.Match(link);
                    if (match.Success)
                    {
                        var name = match.Groups["server"].Value;
                        var portStr = match.Groups["port"].Value;
                        var channel = match.Groups["channel"].Value;

                        int port;
                        if (!int.TryParse(portStr, out port))
                            port = 6667;

                        ExtendedServerInfo server;
                        if ((server = UserOptions.Default.Servers.Find(s => s.Name == name && s.Port == port)) == null)
                            server = new ExtendedServerInfo(name, name, port);

                        var serverWindow = OpenServerWindow(server);

                        if (!string.IsNullOrEmpty(channel))
                        {
                            EventHandler handler = null;
                            handler = (sender2, e2) =>
                                {
                                    serverWindow.OnConnected -= handler;
                                    serverWindow.JoinChannel(channel);
                                };
                            serverWindow.OnConnected += handler;
                        }

                        if (serverWindow != null)
                            serverWindow.Connect();
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString(), System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MDIParent_Deactivate(object sender, EventArgs e)
        {
            this.statusStrip.Focus();
            this.application.OnDeactivated();
        }

        private void tsmiCmdPlay_CheckedChanged(object sender, EventArgs e)
        {
            UISettings.Default.PlayMusicExt = this.tsmiCmdPlay.Checked ? PlayMode.OpOnly : PlayMode.None;
        }

        private void tsmiCmdUrl_Click(object sender, EventArgs e)
        {
            UISettings.Default.UrlExt = this.tsmiCmdUrl.Checked ? PlayMode.OpOnly : PlayMode.None;
        }

        private void tvWindows_ClickedByMouseLeftButton(object sender, TreeNodeMouseClickEventArgs e)
        {
            //this.tvWindows.SelectedNode = e.Node;

            var windowNode = (WindowNode)e.Node;
            if (windowNode != null && windowNode.Window == null)
            {
                var serv = windowNode.Tag as ExtendedServerInfo;

                if (serv == null)
                {
                    var parent = windowNode.Parent as WindowNode;
                    serv = parent.Tag as ExtendedServerInfo;
                    var servWindow = (MDIChildServer)parent.Window;
                    var chan = windowNode.Tag as Channel;
                    if (servWindow != null)
                    {
                        servWindow.JoinChannel(chan.Name);
                        return;
                    }

                    if (this.connectionTasks.ContainsKey(parent))
                        this.connectionTasks[parent].Add(chan.Name);
                    else
                        this.connectionTasks.Add(parent, new List<string>(new string[] { chan.Name }));
                }

                ConnectTo(serv);

                return;
            }
        }

        private void tsmiIRCEditor_Click(object sender, EventArgs e)
        {
            var diag = new IRCEditor();
            RegisterAsMDIChild(null, diag, null);
        }


        public string Play(string multimediaFile, int numOfPlayer)
        {
            return Play(multimediaFile, numOfPlayer, false, 0);
        }
    }
}
