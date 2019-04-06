using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using IRCWindow.Properties;
using IRCProviders;
using System.Globalization;
using System.Security;
using System.Configuration;
using System.Text.RegularExpressions;
using IRCWindow.ViewModel;
using System.Threading.Tasks;
using IRCWindow.Data;
using System.Linq;
using CIRCe.Base;

namespace IRCWindow
{
    static class Program
    {
        // TODO: Логи скрыть от пользователя
        // TODO: Избавиться от многократного запроса WHOIS
        // TODO: вывести информацию о UserInfo.Auth
        // TODO: отдельные пути для логов, медиа, аддонов, кэша
        // TODO

        /// <summary>
        /// Настройки программы
        /// </summary>
        public static ProgramOptions ProgramOptions = new ProgramOptions();

        public static string DefaultDataFolder()
        {
            return Path.Combine(
                       Path.Combine(
                           Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                           Application.CompanyName),
                       Application.ProductName);
        }

        public static string DefaultLocalDataFolder()
        {
            return Path.Combine(
                       Path.Combine(
                           Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                           Application.CompanyName),
                       Application.ProductName);
        }

        // Виртуальная файловая директория
        internal static StorageProvider DataStorage = new FolderStorage();
        internal static StorageProvider LocalDataStorage = new FolderStorage();
        internal static StorageProvider ProgramStorage = new FolderStorage();

        internal static Random Rand = new Random();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Directory.SetCurrentDirectory(Application.StartupPath);
            
            string configFile = null;
            // Защита от повреждения файла настроек
            try
            {
                var conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
                configFile = conf.FilePath;
            }
            catch (ConfigurationErrorsException exc)
            {
                MessageBox.Show("Ошибка загрузки настроек из файла конфигурации. Настройки восстановлены в значения по умолчанию.");
                File.Delete(exc.Filename);
                Settings.Default.Upgrade();
                Settings.Default.Save();
            }

            if (Settings.Default.FirstRun)
            {
                Settings.Default.Upgrade();
                UISettings.Default.Upgrade();

                UISettings.Default.WaitServer = 150;
                UISettings.Default.PingServer = 300;
                UISettings.Default.Language = Thread.CurrentThread.CurrentCulture;
                var proxyWrapper = WebRequest.DefaultWebProxy;
                if (proxyWrapper != null)
                {
                    Settings.Default.Proxy = proxyWrapper.GetProxy(new Uri(Settings.Default.UpdatePath));
                    Settings.Default.ProxyCredentials = proxyWrapper.Credentials as NetworkCredential;
                }

                Settings.Default.UseProxy = false;
                Settings.Default.IsFirstRun = false;
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = UISettings.Default.Language;
                if (Settings.Default.UseProxy)
                    WebRequest.DefaultWebProxy = new WebProxy(Settings.Default.Proxy) { Credentials = Settings.Default.ProxyCredentials };
            }

            //if (Settings.Default.SearchForUpdates == CheckState.Checked)
            //    Task.Factory.StartNew(SearchForUpdatesAsync);

            var dataPath = string.Empty;
            var localDataPath = string.Empty;
            try
            {
                if (Settings.Default.UseAppDataFolder)
                {
                    dataPath = Program.DefaultDataFolder();
                    localDataPath = Program.DefaultLocalDataFolder();
                }
                else
                    localDataPath = dataPath = Settings.Default.DataFolder;

                if (string.IsNullOrWhiteSpace(dataPath) || string.IsNullOrWhiteSpace(localDataPath))
                {
                    MessageBox.Show("Не задан путь в папке хранения данных!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Directory.CreateDirectory(dataPath);
                DataStorage.RootPath = dataPath;

                Directory.CreateDirectory(localDataPath);
                LocalDataStorage.RootPath = localDataPath;

                Application.ThreadException += (sender, e) =>
                    {
                        if (!(e.Exception is AppDomainUnloadedException))
                        {
                            SendErrorReport(e.Exception, false);
                        }
                    };
                
                Application.Run(new MDIParent(args));
            }
            catch (Exception e)
            {
                SendErrorReport(e);
            }
            finally
            {
                try
                {
                    Settings.Default.Save();
                    UISettings.Default.Save();
                    UserOptions.Default.Save();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message, System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private static bool? Approved = null;

        internal static void SendErrorReport(Exception exception, bool exit = true)
        {
            try
            {
                if (!Approved.HasValue)
                    Approved = MessageBox.Show(string.Format("Во время выполнения приложения произошла непредвиденная ошибка. {0}Отправить отчёт разработчику?", exit ? "Приложение будет завершено. " : ""), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

                if (Approved.Value)
                {
                    var errorText = new StringBuilder();
                    while (exception != null)
                    {
                        errorText.AppendLine(exception.ToString()).AppendLine();
                        exception = exception.InnerException;
                    }

                    var service = new UpdateService.UpdateServiceClient();
                    service.SendErrorReport("CIRCe", Assembly.GetExecutingAssembly().GetName().Version, DateTime.Now, errorText.ToString());
                }
            }
            catch
            {
            }
            finally
            {
                if (exit)
                    Application.Exit();
            }
        }

        private static void SearchForUpdatesAsync()
        {
            if (SearchForUpdatesNew())
                Application.Exit();
        }

        /// <summary>
        /// Переместить директорию
        /// </summary>
        /// <param name="source">Исходное расположение директории</param>
        /// <param name="target">Целевое расположение директории</param>
        public static void MoveAll(string source, string target)
        {
            try
            {
                if (!Directory.Exists(source))
                    return;
                
                CopyAll(new DirectoryInfo(source), new DirectoryInfo(target));
                Directory.Delete(source, true);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        public static void CopyAll(string source, string target)
        {
            CopyAll(new DirectoryInfo(source), new DirectoryInfo(target));
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            // Check if the target directory exists, if not, create it.
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into it’s new directory.
            Array.ForEach(source.GetFiles(), fileInfo => fileInfo.CopyTo(Path.Combine(target.ToString(), fileInfo.Name), true));

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        /// <summary>
        /// Необходимый заголовок для WebRequest'ов и WebClient'ов
        /// </summary>
        public static string UserAgentHeader
        {
            get
            {
                return string.Format("{0} {1} ({2})", Application.ProductName, Assembly.GetExecutingAssembly().GetName().Version.ToString(), Environment.OSVersion.VersionString);
            }
        }

        /// <summary>
        /// Произвести поиск обновлений
        /// </summary>
        /// <returns>Нужно ли завершить приложение для выполнения обновления</returns>
        private static bool SearchForUpdatesNew()
        {
            var updateService = new UpdateService.UpdateServiceClient();
            try
            {
                updateService.Open();

                var currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
                var actualVersion = updateService.GetProductVersionByOS("CIRCe", Environment.OSVersion.Version);

                if (actualVersion > currentVersion)
                {
                    if (MessageBox.Show(string.Format(Resources.FullUpdates, actualVersion.ToString(3)), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        var updateUri = updateService.GetProductUpdate("CIRCe");
                        if (!updateUri.IsAbsoluteUri)
                            updateUri = new Uri("http://vladimirkhil.com/" + updateUri.OriginalString);
                        
                        var localFile = Path.Combine(Path.GetTempPath(), "setup.exe");

                        using (var webClient = new WebClient())
                        {
                            webClient.Headers.Add(HttpRequestHeader.UserAgent, Program.UserAgentHeader);
                            webClient.DownloadFile(updateUri, localFile);
                        }

                        Process.Start(localFile);
                        return true;
                    }
                }

            }
            catch (Exception exc)
            {
                MessageBox.Show(string.Format(Resources.UpdateException, exc.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                updateService.Close();
            }

            return false;
        }
    }
}