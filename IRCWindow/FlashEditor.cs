using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.ComponentModel;
using IRCProviders;

namespace IRCWindow
{
    internal class FlashEditor: UITypeEditor
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
                    // Создаем форму для редактирования
                    var flashingParams = (FlashParams)((ProgramOptions)context.Instance).FlashingParams.Clone();
                    using (var form = new FlashingDialog(flashingParams))
                    {
                        // вызываем модальный диалог
                        if (edSvc.ShowDialog(form) == DialogResult.OK)
                            ((ProgramOptions)context.Instance).FlashingParams = flashingParams;
                    }
                }
            }
            // возвращаем или старое или новое значение
            return value;
        }
    }
}
