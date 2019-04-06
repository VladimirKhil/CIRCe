using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace IRCWindow
{
    /// <summary>
    /// Интерфейс внутреннего хранилища данных Цирцеи
    /// </summary>
    internal abstract class StorageProvider
    {
        internal string RootPath { get; set; }

        internal abstract string RealPath(string name);

        internal abstract bool FileExists(string name);
        internal abstract Stream CreateFile(string name);
        internal abstract Stream OpenFile(string name, FileMode mode);
        internal abstract StreamWriter AppendText(string name, Encoding encoding);
        internal abstract void DeleteFile(string name);
        internal abstract void InsertFile(string sourceName, string innerName, bool overwrite);

        internal abstract void CreateDirectory(string name);
        internal abstract DirectoryInfo GetDirectoryInfo(string name);
    }
}
