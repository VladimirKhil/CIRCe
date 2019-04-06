using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace IRCWindow
{
    /// <summary>
    /// Класс, обеспечивающий хранение данных по абсолютным путям
    /// </summary>
    internal class FolderStorage: StorageProvider
    {
        internal FolderStorage()
        {
            this.RootPath = Application.StartupPath;
        }

        internal override string RealPath(string name)
        {
            return Path.Combine(this.RootPath, name);
        }

        internal override Stream CreateFile(string name)
        {
            return File.Create(RealPath(name));
        }

        internal override void CreateDirectory(string name)
        {
            Directory.CreateDirectory(RealPath(name));
        }

        internal override bool FileExists(string name)
        {
            return File.Exists(RealPath(name));
        }

        internal override Stream OpenFile(string name, FileMode mode)
        {
            return File.Open(RealPath(name), mode);
        }

        internal override StreamWriter AppendText(string name, Encoding encoding)
        {
            return new StreamWriter(RealPath(name), true, encoding);
        }

        internal override DirectoryInfo GetDirectoryInfo(string name)
        {
            return new DirectoryInfo(RealPath(name));
        }

        internal override void DeleteFile(string name)
        {
            File.Delete(RealPath(name));
        }

        internal override void InsertFile(string sourceName, string innerName, bool overwrite)
        {
            File.Copy(sourceName, RealPath(innerName), true);
        }
    }
}
