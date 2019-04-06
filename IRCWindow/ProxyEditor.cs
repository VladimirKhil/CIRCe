using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using IRCWindow.Properties;
using System.Windows.Forms;
using System.Net;
using System.ComponentModel;

namespace IRCWindow
{
    internal class ProxyEditor: UITypeEditor
    {
        private IWindowsFormsEditorService edSvc = null;

        // Стиль редактора - модальный диалог
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context != null && context.Instance != null && provider != null)
            {
                // получаем интерфейс сервиса
                edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                if (edSvc != null)
                {
                    using (var form = new ProxyDialog(Settings.Default.UseProxy, Settings.Default.Proxy, Settings.Default.ProxyCredentials))
                    {
                        // вызываем модальный диалог
                        if (edSvc.ShowDialog(form) == DialogResult.OK)
                        {
                            Settings.Default.UseProxy = form.Useproxy;
                            try
                            {
                                Settings.Default.Proxy = form.Address;
                                Settings.Default.ProxyCredentials = form.Credentials;

                                if (Settings.Default.UseProxy)
                                    WebRequest.DefaultWebProxy = new WebProxy(Settings.Default.Proxy) { Credentials = Settings.Default.ProxyCredentials };
                            }
                            catch (Exception exc)
                            {
                                MessageBox.Show(exc.Message);
                                Settings.Default.UseProxy = false;
                            }
                        }
                    }
                }
            }
            // возвращаем или старое или новое значение
            return value;
        }
    }
}
