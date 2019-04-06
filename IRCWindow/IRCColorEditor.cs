using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Drawing;
using IRCWindow.Properties;

namespace IRCWindow
{
    public class IRCColorEditor: UITypeEditor
    {
        private IWindowsFormsEditorService editorService = null;
        private int selectedColor = -1;

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override bool IsDropDownResizable
        {
            get
            {
                return false;
            }
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context != null && context.Instance != null && provider != null)
            {
                editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                if (editorService != null)
                {
                    var colorPanel = new ColorPanel();
                    colorPanel.TextVisible = false;
                    colorPanel.Select += new EventHandler<ColorSelectedEventArgs>(colorPanel_Select);

                    editorService.DropDownControl(colorPanel);

                    value = this.selectedColor;
                }
            }
            
            return base.EditValue(context, provider, value);
        }

        void colorPanel_Select(object sender, ColorSelectedEventArgs e)
        {
            if (editorService != null)
            {
                this.selectedColor = e.ChoosenColor;
                editorService.CloseDropDown();
            }
        }

        public override bool GetPaintValueSupported(System.ComponentModel.ITypeDescriptorContext context)
        {
            return true;
        }

        public override void PaintValue(PaintValueEventArgs e)
        {
            base.PaintValue(e);
            
            var obj = (ProgramOptions)e.Context.Instance;

            using (var brush = new SolidBrush(Settings.Default.Colors[obj.PrintForeColor]))
                e.Graphics.FillRectangle(brush, e.Bounds);
        }
    }
}
