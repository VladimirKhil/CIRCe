using System;
using System.Collections.Generic;
using System.Text;
using CIRCe.Base;
using System.Threading;
using IRCWindow.Properties;
using System.Windows.Forms;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using System.ComponentModel;
using System.Diagnostics;
using IRCProviders;
using System.Reflection;
using System.Runtime.InteropServices;
using IRC.Client.Base;
using System.Linq;
using Microsoft.Win32;
using System.Xml;
using System.Xml.Linq;

namespace IRCWindow.ViewModel
{
    /// <summary>
    /// Класс, отвечающий за работу с дополнениями
    /// </summary>
    internal sealed class AddonsManager: InfiniteMarshalByRefObject, IDisposable
    {
        /// <summary>
        /// Активные дополнения
        /// </summary>
        private Dictionary<Addon, RunningAddonInfo> runningAddons = new Dictionary<Addon, RunningAddonInfo>();
        private CIRCeApplication application = null;

        private SynchronizationContext context = SynchronizationContext.Current;

        /// <summary>
        /// Создать менеджер дополнений
        /// </summary>
        /// <param name="application">Ссылка на приложение Цирцеи</param>
        public AddonsManager(CIRCeApplication application)
        {
            this.application = application;
        }

        /// <summary>
        /// Получить информацию о дополнении на текущем языке приложения
        /// </summary>
        /// <param name="info">Запрашиваемое дополнение</param>
        /// <returns>Инорфмация о дополнении на текущем языке</returns>
        internal static AddonLocalizationInfoAttribute GetLocalizationInfoForAddon(AddonInformation info)
        {
            AddonLocalizationInfoAttribute neutral = null;
            var culture = CultureInfo.CurrentUICulture;
            foreach (var item in info.LocalizedInfos)
            {
                if (string.IsNullOrEmpty(item.Culture))
                    neutral = item;
                else if (culture.Name == item.Culture)
                    return item;
            }

            return neutral;
        }

        /// <summary>
        /// Выгрузить работающее дополнение
        /// </summary>
        /// <param name="info">Выгружаемое дополнение</param>
        private void UnloadAddon(RunningAddonInfo info, bool showError = true)
        {
            try
            {
                lock (this.runningAddons)
                {
                    this.runningAddons.Remove(info.Instanse);
                }

                info.Instanse.Dispose();
                AppDomain.Unload(info.Domain);
            }
            catch (ThreadAbortException)
            {
            }
            catch (AppDomainUnloadedException)
            {
            }
            catch (Exception exc)
            {
                if (showError)
                    MessageBox.Show(string.Format(String.Format("{0} {1} {2}", Resources.AddonError, exc.Message, exc.GetType()), GetLocalizationInfoForAddon(info.Info).Title), Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Поиск установленных дополнений
        /// </summary>
        internal void BrowseAddons()
        {
            var dirInfo = Program.DataStorage.GetDirectoryInfo("AddOns");
            var dirInfo2 = Program.ProgramStorage.GetDirectoryInfo("AddOns");
            var dirInfoList = new List<DirectoryInfo>();
            if (dirInfo.Exists)
                dirInfoList.AddRange(dirInfo.GetDirectories());
            if (dirInfo2.Exists)
                dirInfoList.AddRange(dirInfo2.GetDirectories());

            // Удалим несуществующие более дополнения
            Settings.Default.AddonsNew.RemoveAll(ai => !File.Exists(ai.Path) || (!ai.Path.Contains(Program.DataStorage.RootPath) && !ai.Path.Contains(Program.ProgramStorage.RootPath)));
            
            foreach (var addonDir in dirInfoList)
            {
                if (Settings.Default.AddonsNew.Exists(info => Path.GetDirectoryName(info.Path) == addonDir.FullName))
                    continue;
                
                foreach (var fileInfo in addonDir.GetFiles("*.dll"))
                {
                    AppDomain domain = null;
                    try
                    {
                        Guid guid;
                        AddonInfoAttribute info;
                        AddonLocalizationInfoAttribute[] localizationInfos;
                        
                        domain = AppDomain.CreateDomain("AddonsChecker", AppDomain.CurrentDomain.Evidence, new AppDomainSetup { ApplicationBase = Application.StartupPath });
                        var checker = (AddonsChecker.AddonsChecker)domain.CreateInstanceFromAndUnwrap(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AddonsChecker.dll"), "AddonsChecker.AddonsChecker");
                        
                        var ok = checker.CheckAssembly(fileInfo.FullName, out guid, out info, out localizationInfos);
                        
                        if (!ok || guid == null || info == null || string.IsNullOrEmpty(info.AddonType) || localizationInfos == null || localizationInfos.Length == 0)
                            continue;

                        if (Settings.Default.AddonsNew.Exists(ai => ai.Guid == guid.ToString()))
                            continue;

                        Settings.Default.AddonsNew.Add(new AddonInformation
                        {
                            Guid = guid.ToString(),
                            Info = info,
                            IsAssembly = true,
                            LocalizedInfos = localizationInfos,
                            Path = fileInfo.FullName
                        });
                    }
                    catch (BadImageFormatException)
                    {
                        // Это не сборка .NET; попробуем извлечь информацию об аддоне, созданном с помощью нативного кода
                        // TODO: пока не реализовано
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(string.Format(String.Format("{0} {1} {2}", Resources.AddonError, ex.Message, ex.GetType()), addonDir.Name), Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    finally
                    {
                        if (domain != null)
                            AppDomain.Unload(domain);
                    }
                }
            }
        }

        /// <summary>
        /// Запустить дополнение
        /// </summary>
        /// <param name="info">Запускаемое дополнение</param>
        internal void RunAddon(AddonInformation info)
        {
            var thread = new Thread(arg =>
            {
                AppDomain domain = null;
                Addon addon;
                try
                {
                    domain = AppDomain.CreateDomain("Addon", AppDomain.CurrentDomain.Evidence, new AppDomainSetup { ApplicationBase = Application.StartupPath, PrivateBinPath = Path.GetDirectoryName(info.Path) });
                    var loader = (ObjectLoader)domain.CreateInstanceFromAndUnwrap(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CIRCe.Base.dll"), "CIRCe.Base.ObjectLoader");
                    addon = (Addon)loader.LoadObject(info.Path, info.Info.AddonType);//domain.CreateInstanceFromAndUnwrap(info.Path, info.Info.AddonType);

                    if (Settings.Default.SearchForAddons == CheckState.Checked) // Проверяем наличие обновления
                    {
                        if (addon.IsUpdateNeeded() && MessageBox.Show(string.Format("Дополнение \"{0}\" сообщает, что оно нуждается в обновлении. Обновить его?", GetLocalizationInfoForAddon(info).Title), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                        {
                            var updatePath = addon.GetUpdateUri();
                            addon.Dispose();
                            AppDomain.Unload(domain);

                            UpdateAddon(info, updatePath);
                            return;
                        }
                    }

                    domain.UnhandledException += domain_UnhandledException;
                }
                catch (Exception e)
                {
                    MessageBox.Show(string.Format("Ошибка запуска дополнения {0}: {1}", GetLocalizationInfoForAddon(info).Title, e.Message), Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (domain != null)
                    {
                        AppDomain.Unload(domain);
                    }

                    return;
                }

                //SecurityPermission perm = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
                //perm.Deny();

                var addonInfo = new RunningAddonInfo { Domain = domain, MainThread = Thread.CurrentThread, Info = info, Instanse = addon };
                lock (this.runningAddons)
                {
                    this.runningAddons[addon] = addonInfo;
                }

                var showError = true;

                try
                {
                    addon.Run(this.application);
                }
                catch (TargetInvocationException)
                {
                    MessageBox.Show(string.Format(String.Format("{0} {1}", Resources.AddonError, "Нельзя подписываться на события Цирцеи в дополнении напрямую. Используйте метод Wrap или класс EventWrapper"), info.LocalizedInfos[0].Title), Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    showError = false;
                }
                catch (Exception exc)
                {
                    MessageBox.Show(string.Format(String.Format("{0} {1} {2}", Resources.AddonError, exc.Message, exc.GetType()), GetLocalizationInfoForAddon(info).Title), Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    showError = false;
                }
                finally
                {
                    UnloadAddon(addonInfo, showError: showError);
                }
            }) { IsBackground = true };

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void UpdateAddon(AddonInformation info, string updatePath)
        {
            var addonFolder = Path.GetDirectoryName(Path.GetDirectoryName(info.Path));
            var updateLocalPath = Path.Combine(addonFolder, Path.GetFileName(updatePath));

            Task.Factory.StartNew(() =>
            {
                var webClient = new WebClient();

                var progressDialog = new MyProgressDialog() { Text = Resources.FileDownloading };
                progressDialog.FormClosed += (sender2, e2) =>
                {
                    if (webClient != null)
                    {
                        webClient.CancelAsync();
                    }
                };

                webClient.Headers.Add(HttpRequestHeader.UserAgent, Program.UserAgentHeader);

                DownloadProgressChangedEventHandler downlDel = null;
                downlDel = (sender, e) =>
                {
                    progressDialog.Value = e.ProgressPercentage;
                };

                webClient.DownloadProgressChanged += downlDel;
                webClient.DownloadFileCompleted += webClient_DownloadFileCompleted;

                webClient.DownloadFileAsync(new Uri(updatePath), updateLocalPath, new object[] { updateLocalPath, progressDialog, info });

                Application.Run(progressDialog);
            });
        }

        private void webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            var state = (object[])e.UserState;
            var destination = (string)state[0];
            var progress = (MyProgressDialog)state[1];
            var info = (AddonInformation)state[2];
            progress.Close();

            try
            {
                if (e.Error != null)
                {
                    MessageBox.Show(e.Error.Message, Application.ProductName);
                    return;
                }

                if (e.Cancelled)
                {
                    if (File.Exists(destination))
                        File.Delete(destination);
                    return;
                }

                if (ArchiveManager.ExtractArchive(destination, false))
                {
                    context.Post(AddonUpdated, info);
                }
            }
            finally
            {
                Application.ExitThread();
            }
        }

        private void AddonUpdated(object state)
        {
            var info = (AddonInformation)state;
            MessageBox.Show("Дополнение успешно обновлено!");

            if (info.Info.StartMode == AddonStartMode.Automatic)
                RunAddon(info);
        }

        private void domain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            lock (this.runningAddons)
            {
                foreach (var item in runningAddons)
                {
                    if (item.Value.Domain == sender)
                    {
                        MessageBox.Show(string.Format(string.Format("{0} {1}", Resources.AddonError, (e.ExceptionObject as Exception).Message), GetLocalizationInfoForAddon(item.Value.Info).Title), Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        UnloadAddon(item.Value, false);
                        break;
                    }
                }
            }
        }

        #region Члены IDisposable

        public void Dispose()
        {
            List<Thread> threads = new List<Thread>();
            lock (this.runningAddons)
            {
                foreach (var item in this.runningAddons)
                {
                    item.Value.Instanse.Stop();
                    threads.Add(item.Value.MainThread);
                }
            }

            new Thread(() => Parallel.ForEach(threads, thread => thread.Join(TimeSpan.FromSeconds(10.0)))).Start();
        }

        #endregion

        /// <summary>
        /// Запущено ли дополнение
        /// </summary>
        /// <param name="info">Информация о дополнении</param>
        /// <returns>Запущено ли оно</returns>
        internal bool IsRunning(AddonInformation info)
        {
            lock (this.runningAddons)
            {
                return this.runningAddons.Values.Any(i => i.Info.Guid == info.Guid);
            }
        }

        internal void InstallPredefinedAddons()
        {
            try
            {
                var key = string.Format(@"Software{0}\\Svoyak-soft\\CIRCe", Environment.Is64BitProcess ? @"\\Wow6432Node" : "");
                var reg = Registry.LocalMachine.OpenSubKey(key);
                if (reg == null)
                    reg = Registry.CurrentUser.OpenSubKey(key);
                
                if (reg != null)
                {
                    var addonsString = (string)reg.GetValue("Addons");
                    if (addonsString != null)
                    {
                        var addons = addonsString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Where(guid => !Settings.Default.AddonsNew.Any(ai => ai.Guid.ToUpper() == guid.ToUpper())).ToArray();

                        if (addons.Length == 0)
                        {
                            reg.Close();
                            return;
                        }

                        var webRequest = (HttpWebRequest)WebRequest.Create(String.Format("{0}addons.xml", Settings.Default.UpdatePath));
                        webRequest.UserAgent = Program.UserAgentHeader;
                        var stream = webRequest.GetResponse().GetResponseStream();
                        var xmlDocument = XDocument.Load(stream);
                        
                        foreach (var addon in addons)
                        {
                            var url = xmlDocument.Descendants(XName.Get("addon")).Where(el => el.Element(XName.Get("guid")).Value.ToLower() == addon.ToLower()).Select(el => el.Element(XName.Get("url")).Value).FirstOrDefault();
                            if (url != null)
                            {
                                InstallFromUrl(url);
                            }
                        }
                    }
                    reg.Close();                    
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Ошибка установки дополнений", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InstallFromUrl(string url)
        {
            var uri = new Uri(url);
            var fileName = Path.GetFileName(uri.AbsolutePath);
            var destination = Path.Combine(Program.DataStorage.GetDirectoryInfo("AddOns").FullName, fileName);

            var webClient = new WebClient();
            webClient.Headers.Add(HttpRequestHeader.UserAgent, Program.UserAgentHeader);
            //webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(webClient_DownloadProgressChanged);
            webClient.DownloadFileCompleted += webClient_DownloadFileCompleted2;

            //var progressDialog = new MyProgressDialog() { Text = Resources.FileDownloading };
            //progressDialog.FormClosed += (sender2, e2) =>
            //{
            //    if (webClient != null)
            //    {
            //        webClient.CancelAsync();
            //    }
            //};

            //webClient.DownloadFileCompleted += (sender2, e2) => { if (progressDialog.Visible) progressDialog.Close(); };
            webClient.DownloadFileAsync(uri, destination, new object[] { destination, /*progressDialog*/null });
            
            //progressDialog.Show();
        }

        private void webClient_DownloadFileCompleted2(object sender, AsyncCompletedEventArgs e)
        {
            var state = (object[])e.UserState;
            var destination = (string)state[0];

            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, Application.ProductName);
                return;
            }

            if (e.Cancelled)
            {
                if (File.Exists(destination))
                    File.Delete(destination);
                //this.webClient = null;

                return;
            }

            Thread.Sleep(500);
            //this.webClient = null;

            ArchiveManager.ExtractArchive(destination);
            BrowseAddons();
        }

        void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            var state = (object[])e.UserState;
            var progressDialog = (MyProgressDialog)state[1];
            if (progressDialog.InvokeRequired)
            {
                var func = new DownloadProgressChangedEventHandler(webClient_DownloadProgressChanged);
                progressDialog.BeginInvoke(func, sender, e);
            }
            else
                progressDialog.Value = e.ProgressPercentage;
        }

        /// <summary>
        /// Удалить дополнение
        /// </summary>
        /// <param name="addonInformation"></param>
        internal void Uninstall(AddonInformation addonInformation)
        {
            lock (this.runningAddons)
            {
                var runningInfo = this.runningAddons.Values.FirstOrDefault(i => i.Info.Guid == addonInformation.Guid);
                if (runningInfo != null)
                    UnloadAddon(runningInfo, false);
            }

            var folder = Path.GetDirectoryName(addonInformation.Path);
            Directory.Delete(folder, true);
        }
    }
}
