using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using IRCWindow.Properties;
using System.Xaml;
using CIRCe.Base;
using IRC.Client.Base;
using IRCWindow.ViewModel;

namespace IRCWindow.Data
{
    /// <summary>
    /// Пользовательские настройки программы
    /// </summary>
    public sealed class UserOptions : INotifyPropertyChanged
    {
        internal static string ConfigFileName = "user.config";

        public static UserOptions Default;

        static UserOptions()
        {
            Default = Load();
        }

        public UserOptions()
        {
            
        }

        public static UserOptions Load()
        {
            using (var file = IsolatedStorageFile.GetUserStoreForAssembly())
            {
                if (file.FileExists(ConfigFileName) && Monitor.TryEnter(ConfigFileName, 2000))
                {
                    try
                    {
                        using (var stream = file.OpenFile(ConfigFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            return Load(stream);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Не удалось загрузить настройки пользователя! Будут использованы настройки по умолчанию.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        Monitor.Exit(ConfigFileName);
                    }
                }
            }

            var newSettings = new UserOptions();
            newSettings.Initialize();
            return newSettings;
        }

        private static UserOptions Load(Stream stream)
        {
            return (UserOptions)XamlServices.Load(stream);
        }

        public void Save()
        {
            if (Monitor.TryEnter(ConfigFileName, 2000))
            {
                try
                {
                    using (var file = IsolatedStorageFile.GetUserStoreForAssembly())
                    {
                        using (var stream = new IsolatedStorageFileStream(ConfigFileName, FileMode.Create, file))
                        {
                            Save(stream);
                        }
                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show(string.Format("Не удалось сохранить настройки пользователя: {0}", exc.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Monitor.Exit(ConfigFileName);
                }
            }
        }

        private void Save(Stream stream)
        {
            XamlServices.Save(stream, this);
        }

        private void Initialize()
        {
            this.hotKeys[System.Windows.Forms.Keys.F2] = "й";
            this.hotKeys[System.Windows.Forms.Keys.F3] = "";
            this.hotKeys[System.Windows.Forms.Keys.F4] = "";
            this.hotKeys[System.Windows.Forms.Keys.F5] = "";
            this.hotKeys[System.Windows.Forms.Keys.F6] = "";
            this.hotKeys[System.Windows.Forms.Keys.F7] = "";
            this.hotKeys[System.Windows.Forms.Keys.F8] = "";
            this.hotKeys[System.Windows.Forms.Keys.F9] = "";
            this.hotKeys[System.Windows.Forms.Keys.F10] = "";
            this.hotKeys[System.Windows.Forms.Keys.F11] = "";
            this.hotKeys[System.Windows.Forms.Keys.F12] = "";

            var server = new ExtendedServerInfo("GameShows", "irc.gameshows.ru");
            this.servers.Add(server);

            server = new ExtendedServerInfo("ForestNet", "irc.forestnet.org");
            this.servers.Add(server);

            server = new ExtendedServerInfo("ChgkInfo", "irc.chgk.info");
            server.Channels.Add(new ExtendedChannelInfo("#vdi"));
            server.Channels.Add(new ExtendedChannelInfo("#vdi-red"));
            this.servers.Add(server);

            server = new ExtendedServerInfo("MGTS", "irc.mgts.by");
            this.servers.Add(server);

            this.user.UserName = "Пользователь";
        }

        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int Version { get; set; }

        private Dictionary<System.Windows.Forms.Keys, string> hotKeys = new Dictionary<System.Windows.Forms.Keys, string>();

        public Dictionary<System.Windows.Forms.Keys, string> HotKeys
        {
            get { return this.hotKeys; }
            set
            {
                if (value != null)
                {
                    this.hotKeys = value;
                    OnPropertyChanged("HotKeys");
                }
            }
        }

        private ExtendedServerInfoList servers = new ExtendedServerInfoList();

        public ExtendedServerInfoList Servers
        {
            get { return servers; }
            set
            {
                if (value != null)
                {
                    servers = value;
                    OnPropertyChanged("Servers");
                }
            }
        }

        private List<string> nicks = new List<string>();

        public List<string> Nicks
        {
            get { return nicks; }
            set
            {
                if (value != null)
                {
                    nicks = value;
                    OnPropertyChanged("Nicks");
                }
            }
        }

        private UserInfo user = new UserInfo();

        public UserInfo User
        {
            get { return user; }
            set
            {
                if (value != null)
                {
                    user = value;
                    OnPropertyChanged("User");
                }
            }
        }
    }
}
