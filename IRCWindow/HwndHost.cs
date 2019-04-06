using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IRCWindow
{
    /// <summary>
    /// Панель, позволяющая размешать произвольные окна Win32
    /// </summary>
    public sealed class HwndHost : Panel
    {
        private IntPtr child = IntPtr.Zero;

        public IntPtr Child
        {
            set
            {
                if (this.child != value)
                {
                    this.child = value;

                    IRCProviders.Win32.SetParent(this.child, this.Handle);
                    IRCProviders.Win32.MoveWindow(this.child, 0, 0, this.Width, this.Height, true);
                }
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            this.Invalidate();
            base.OnSizeChanged(e);
        }

        protected override void OnResize(EventArgs eventargs)
        {
            if (this.child != IntPtr.Zero)
            {
                IRCProviders.Win32.MoveWindow(this.child, 0, 0, this.Width, this.Height, true);
            }
            base.OnResize(eventargs);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            IRCProviders.Win32.SendMessage(this.child, IRCProviders.Win32.WM_PAINT, IntPtr.Zero, IntPtr.Zero);
        }
    }
}
