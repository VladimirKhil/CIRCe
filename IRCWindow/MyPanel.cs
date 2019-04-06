using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using IRCProviders;

namespace IRCWindow
{
    public partial class MyPanel : Panel
    {
        protected override void AdjustFormScrollbars(bool displayScrollbars)
        {            
            //base.AdjustFormScrollbars(displayScrollbars);
        }

        public MyPanel()
        {
            
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }

        public void UpdateSize()
        {
            this.SuspendLayout();
            foreach (var item in this.Controls)
            {
                var form = item as Form;
                if (form != null && form.WindowState == FormWindowState.Maximized)
                {
                    Win32.ShowWindow(form.Handle, Win32.SW_HIDE);
                    Win32.ShowWindow(form.Handle, Win32.SW_MAXIMIZE);
                }
            } this.PerformLayout();
        }
    }
}
