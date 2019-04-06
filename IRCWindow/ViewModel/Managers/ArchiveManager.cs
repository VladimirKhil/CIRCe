using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using IRCProviders;
using System.Windows.Forms;
using IRCWindow.Properties;
using System.IO.Packaging;

namespace IRCWindow.ViewModel
{
    /// <summary>
    /// Класс, отвечающий за работу с архивами
    /// </summary>
    internal static class ArchiveManager
    {
        /// <summary>
        /// Распаковать архив
        /// </summary>
        /// <param name="filePath">Расположение архива</param>
        internal static bool ExtractArchive(string filePath, bool standartUnpack = true)
        {
            var directory = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(Path.GetDirectoryName(filePath));

            var ext = Path.GetExtension(filePath);

            if (ext == ".exe")
            {
                var process = new Process();
                process.StartInfo.FileName = filePath;
                process.Start();
                process.WaitForExit();
                Directory.SetCurrentDirectory(directory);
                File.Delete(filePath);
            }
            else
            {
                var app = new StringBuilder(1024);
                var exten = Path.GetExtension(filePath);

                if (exten == ".zip")
                {
                    // Пробуем распаковать архив как OPC
                    if (ExctractOPC(filePath))
                        return true;

                    var shortFile = GetShortPath(filePath);

                    if (!FindApplication(shortFile, ".rar", ref app) && !FindApplication(shortFile, ".7z", ref app))
                    {
                        MessageBox.Show(Resources.CouldNotUnpack);
                        return false;
                    }
                }
                else
                {
                    var shortFile = GetShortPath(filePath);
                    var res = Win32.FindExecutable(shortFile, null, app);

                    if (res <= 32)
                    {
                        MessageBox.Show(Resources.CouldNotUnpack);
                        return false;
                    }
                }

                var appString = app.ToString();

                Action<string, string, string> unpack = standartUnpack ? new Action<string, string, string>(Unpack) : new Action<string, string, string>(Unpack2);

                if (appString.Contains("7z"))
                {
                    unpack(Path.Combine(Path.GetDirectoryName(appString), "7z.exe"), filePath, "x {0} -y -o{1}");
                }
                else if (appString.Contains("WinRAR"))
                {
                    unpack(Path.Combine(Path.GetDirectoryName(appString), "UnRAR.exe"), filePath, "x -y {0} {1}");
                }
                else if (exten == ".zip" || exten == ".rar" || exten == ".7z")
                {
                    MessageBox.Show(Resources.CouldNotUnpack);
                    Directory.SetCurrentDirectory(directory);
                    return false;
                }

                Directory.SetCurrentDirectory(directory);
            }

            return true;
        }

        internal static string GetShortPath(string filePath)
        {
            var size = Win32.GetShortPathName(filePath, null, 0);
            var shortFile = new StringBuilder(size);
            Win32.GetShortPathName(filePath, shortFile, size);
            return shortFile.ToString();
        }

        private static void CopyStream(Stream source, Stream target)
        {
            var buff = new byte[4096];
            int i = 0;

            while ((i = source.Read(buff, 0, buff.Length)) != 0)
                target.Write(buff, 0, i);
        }

        private static bool ExctractOPC(string filePath)
        {
            try
            {
                var folder = Path.GetDirectoryName(filePath);
                var subFolder = Path.GetFileNameWithoutExtension(filePath);
                var resultFolder = Path.Combine(folder, subFolder);
                Directory.CreateDirectory(resultFolder);

                using (var package = ZipPackage.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    var parts = package.GetParts().ToArray();
                    if (parts.Length == 0)
                        return false;
                    foreach (var part in parts)
                    {
                        var innerPath = Uri.UnescapeDataString(part.Uri.OriginalString.Substring(1));
                        var innerDir = Path.GetDirectoryName(innerPath);

                        if (!string.IsNullOrWhiteSpace(innerDir))
                        {
                            var innerFullPath = Path.Combine(resultFolder, innerDir);
                            Directory.CreateDirectory(innerFullPath);
                        }

                        var innerFilePath = Path.Combine(resultFolder, innerPath);
                        using (var stream = part.GetStream(FileMode.Open, FileAccess.Read))
                        {
                            using (var fileStream = File.Create(innerFilePath))
                            {
                                CopyStream(stream, fileStream);
                            }
                        }
                    }
                }

                File.Delete(filePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool FindApplication(string shortFileString, string ext, ref StringBuilder app)
        {
            var fileName = Path.ChangeExtension(shortFileString, ext);//shortFileString.Substring(0, shortFileString.Length - 4) + ext;
            int index = 0;
            while (File.Exists(fileName))
                fileName = Path.Combine(Path.GetDirectoryName(shortFileString), Path.ChangeExtension(Path.GetFileNameWithoutExtension(shortFileString) + index++, ext));
 
            File.Create(fileName).Close();

            var res = Win32.FindExecutable(fileName, null, app);

            File.Delete(fileName);

            return res > 32;
        }

        /// <summary>
        /// Распаковать архив (если в архиве несколько файлов, создаст дополнительную папку для их хранения)
        /// </summary>
        /// <param name="app">Приложение, которое будет распаковывать архив</param>
        /// <param name="file">Имя файла для распаковки</param>
        /// <param name="paramString">Параметры приложения с двумя подставляемыми полями: имя файла для распаковки и выходная директория</param>
        private static void Unpack(string app, string file, string paramString)
        {
            Process process = new Process();
            var folder = Path.GetDirectoryName(file);

            var invalid = Path.GetInvalidFileNameChars();
            var goodName = Path.GetFileNameWithoutExtension(file).Trim(invalid);
            var tmpDir = Path.Combine(folder, goodName);
            Directory.CreateDirectory(tmpDir);
            var newFile = Path.Combine(tmpDir, Path.GetFileName(file));
            File.Move(file, newFile);

            var outputDirShort = GetShortPath(tmpDir);
            var newFileShort = GetShortPath(newFile);

            process.StartInfo.FileName = app;
            process.StartInfo.Arguments = string.Format(paramString, newFileShort, outputDirShort);
            process.Start();
            process.WaitForExit();
            File.Delete(newFileShort.ToString());

            var info = new DirectoryInfo(tmpDir);
            var files = info.GetFiles();
            var dirs = info.GetDirectories();
            if (files.Length + dirs.Length == 1)
            {
                if (files.Length == 1)
                    File.Move(files[0].FullName, Path.Combine(folder, files[0].Name));

                if (dirs.Length == 1)
                {
                    var path = Path.Combine(folder, dirs[0].Name);
                    if (Directory.Exists(path))
                    {
                        MessageBox.Show(string.Format(Resources.InstallCopyBadTry, path, dirs[0].FullName));
                        return;
                    }
                    else
                        Directory.Move(dirs[0].FullName, path);
                }

                Directory.Delete(tmpDir);
            }
        }

        private static void Unpack2(string app, string file, string paramString)
        {
            var process = new Process();

            var invalid = Path.GetInvalidFileNameChars();
            var goodName = file.Trim(invalid);

            var outputDir = GetShortPath(Path.GetDirectoryName(file));
            var newFile = GetShortPath(goodName);

            process.StartInfo.FileName = app;
            process.StartInfo.Arguments = string.Format(paramString, newFile, outputDir);
            process.Start();
            process.WaitForExit();
            File.Delete(file);
        }
    }
}
