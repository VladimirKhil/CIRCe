using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using IRCWindow.Properties;
using System.ComponentModel;
using System.IO;
using IRCWindow.Data;

namespace IRCWindow
{
    class AppDataEditor: UITypeEditor
    {
        private IWindowsFormsEditorService edSvc = null;

        // Стиль редактора - модальный диалог
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            try
            {
                if (context != null && context.Instance != null && provider != null)
                {
                    // получаем интерфейс сервиса
                    edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                    if (edSvc != null)
                    {
                        // Создаем форму для редактирования
                        using (var form = new DataFolderEditorDialog())
                        {
                            // вызываем модальный диалог
                            if (edSvc.ShowDialog(form) == DialogResult.OK && form.Change)
                            {
                                // получаем новое значение
                                string oldPath = Settings.Default.UseAppDataFolder ? Program.DefaultDataFolder() : Settings.Default.DataFolder;
                                string oldLocalPath = Settings.Default.UseAppDataFolder ? Program.DefaultLocalDataFolder() : Settings.Default.DataFolder;
                                Settings.Default.UseAppDataFolder = form.NewUse;
                                Settings.Default.DataFolder = form.Folder;
                                string newPath = Settings.Default.UseAppDataFolder ? Program.DefaultDataFolder() : Settings.Default.DataFolder;
                                string newLocalPath = Settings.Default.UseAppDataFolder ? Program.DefaultLocalDataFolder() : Settings.Default.DataFolder;

                                Program.DataStorage.RootPath = newPath;
                                Program.LocalDataStorage.RootPath = newLocalPath;

                                Program.DataStorage.CreateDirectory("AddOns");
                                Program.DataStorage.CreateDirectory("Logs");
                                Program.DataStorage.CreateDirectory("Media");

                                Program.LocalDataStorage.CreateDirectory("Cache");

                                if (form.NeedMove)
                                {
                                    Cursor.Current = Cursors.WaitCursor;
                                    Program.MoveAll(Path.Combine(oldPath, "AddOns"), Path.Combine(newPath, "AddOns"));
                                    Program.MoveAll(Path.Combine(oldPath, "Logs"), Path.Combine(newPath, "Logs"));
                                    Program.MoveAll(Path.Combine(oldPath, "Media"), Path.Combine(newPath, "Media"));
                                    Program.MoveAll(Path.Combine(oldLocalPath, "Cache"), Path.Combine(newLocalPath, "Cache"));

                                    Settings.Default.Save();
                                    UISettings.Default.Save();
                                    UserOptions.Default.Save();
                                    Cursor.Current = Cursors.Default;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            // возвращаем или старое или новое значение
            return value;
        }
    }
}
