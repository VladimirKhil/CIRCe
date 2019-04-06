using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using IRCWindow.Properties;


namespace IRCWindow
{
    /// <summary>
    /// Based on http://www.codeproject.com/KB/install/shortcut_installer.aspx#Setup
    /// Класс для установки и удаления приложения
    /// </summary>
    [RunInstaller(true)]
    public partial class MyInstaller : Installer
    {
        public MyInstaller()
        {
            InitializeComponent();
        }

        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);

            DeleteData();
        }

        private void DeleteData()
        {
            var dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Свояк-софт\CIRCe");
            if (!Directory.Exists(dataPath))
                return;
            
            var dialog = new UninstallDialog();
            if (dialog.ShowDialog() != DialogResult.OK)
                return;
            
            if (dialog.DeleteLogs) Clean(Path.Combine(dataPath, "Logs"));
            if (dialog.DeleteAddons) Clean(Path.Combine(dataPath, "AddOns"));
            if (dialog.DeleteMedia) Clean(Path.Combine(dataPath, "Media"));

            var info = new DirectoryInfo(dataPath);
            if (info.GetDirectories().Length == 0 && info.GetFiles().Length == 0)
                Directory.Delete(dataPath);
        }

        private void Clean(string path)
        {
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }
    }
}
