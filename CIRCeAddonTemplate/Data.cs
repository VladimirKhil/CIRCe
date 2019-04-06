using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.IO.IsolatedStorage;
using System.Xaml;
using IRC.Client.Base;

namespace CIRCeAddonTemplate
{
    /// <summary>
    /// Данные аддона
    /// </summary>
    [Serializable]
    public sealed class Data
    {
        private const string ConfigFile = "Config.xaml";

        /// <summary>
        /// Информация о подключении
        /// </summary>
        public ConnectionInfo Info { get; set; }
        /// <summary>
        /// Канал
        /// </summary>
        public string Channel { get; set; }

        // Ниже можно описать дополнительные данные для дополнения, которые необходимо сохранять между подключениями
        #region MyData

        // TODO: Опишите здесь свои данные

        #endregion

        /// <summary>
        /// Создание данных без параметров
        /// </summary>
        public Data()
        {
            this.Info = new ConnectionInfo
            {
                Server = new ServerInfo()
            };
            this.Info.Server.Port = 6667;
            this.Info.Nick = "";
            this.Channel = "";
        }

        /// <summary>
        /// Создание данных с параметрами
        /// </summary>
        /// <param name="info">Информация о подключении</param>
        /// <param name="channel">Канал</param>
        public Data(ConnectionInfo info, string channel)
        {
            this.Info = info;
            this.Channel = channel;
        }

        #region HelpMethods

        /// <summary>
        /// Сохранить данные
        /// </summary>
        internal void Save()
        {
            try
            {
                using (var file = IsolatedStorageFile.GetUserStoreForAssembly())
                {
                    using (var stream = new IsolatedStorageFileStream(ConfigFile, FileMode.Create, FileAccess.Write, file))
                    {
                        XamlServices.Save(stream, this);
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        /// <summary>
        /// Загрузить данные
        /// </summary>
        /// <returns>Загруженные данные</returns>
        internal static Data Load()
        {
            try
            {
                using (var file = IsolatedStorageFile.GetUserStoreForAssembly())
                {
                    if (file.FileExists(ConfigFile))
                    {
                        using (var stream = new IsolatedStorageFileStream(ConfigFile, FileMode.Open, FileAccess.Read, file))
                        {
                            return (Data)XamlServices.Load(stream);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }

            return new Data();
        }

        #endregion
    }
}
