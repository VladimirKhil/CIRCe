using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IRCProviders
{
    public partial class IRCForm : Form
    {
        private FormWindowState prevoiusState;
        //private string previousText = null;

        public event EventHandler Maximized;
        public event EventHandler Restored;
        public event EventHandler Minimized;

        /// <summary>
        /// Идентификатор окна (желательно, уникальный)
        /// </summary>
        public virtual string Id
        {
            get { return this.Text; }
        }

        //public string Caption
        //{
        //    get { return this.previousText ?? base.Text; }
        //}

        //public override string Text
        //{
        //    get
        //    {
        //        return base.Text;
        //    }
        //    set
        //    {
        //        if (this.previousText != null)
        //        {
        //            this.previousText = value;
        //            OnTextChanged(EventArgs.Empty);
        //        }
        //        else
        //            base.Text = value;                
        //    }
        //}

        public IRCForm()
        {
            InitializeComponent();

            this.TopLevel = false;
            this.prevoiusState = this.WindowState;
        }

        /// <summary>
        /// Возникает при деактивации окна
        /// </summary>
        public void DeactivateMe()
        {
            this.OnDeactivate(EventArgs.Empty);
        }

        /// <summary>
        /// Стандартная процедура обработки сообщений
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0021:
                    this.BringToFront();
                    this.OnActivated(EventArgs.Empty);
                    break;

                default:
                    break;
            }
            
            base.WndProc(ref m);
        }

        private void IRCForm_SizeChanged(object sender, EventArgs e)
        {
            if (this.prevoiusState != this.WindowState)
            {
                var previous = this.prevoiusState;
                this.prevoiusState = this.WindowState;
                
                switch (this.WindowState)
                {
                    case FormWindowState.Maximized:
                        {
                            //previousText = this.Text;
                            //base.Text = "";
                            //this.ControlBox = false;
                            var style = Win32.GetWindowLongFunc(this.Handle, Win32.GWL_STYLE);
                            Win32.SetWindowLongFunc(this.Handle, Win32.GWL_STYLE, style & ~Win32.WS_CAPTION);
                            //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                            //this.Left += 2 * SystemInformation.BorderSize.Width;
                            //this.Top += 2 * SystemInformation.BorderSize.Height;
                            //this.Width -= 2 * SystemInformation.BorderSize.Width;
                            //this.Height -= 2 * SystemInformation.BorderSize.Height;
                            if (Maximized != null)
                                Maximized(this, EventArgs.Empty);
                            break;
                        }

                    case FormWindowState.Minimized:
                        {
                            if (previous == FormWindowState.Maximized)
                            {
                                Restore();
                            }

                            if (Minimized != null)
                                Minimized(this, EventArgs.Empty);
                            break;
                        }

                    case FormWindowState.Normal:
                        {
                            if (previous == FormWindowState.Maximized)
                            {
                                Restore();
                            }

                            if (Restored != null)
                                Restored(this, EventArgs.Empty);
                            break;
                        }

                    default:
                        break;
                }
            }
        }

        private void Restore()
        {
            //this.SuspendLayout();

            var style = Win32.GetWindowLongFunc(this.Handle, Win32.GWL_STYLE);
            Win32.SetWindowLongFunc(this.Handle, Win32.GWL_STYLE, style | Win32.WS_CAPTION);
            //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;

            //this.PerformLayout();

            //base.Text = this.previousText;
            //this.previousText = null;
            //OnTextChanged(EventArgs.Empty);
            //this.ControlBox = true;
        }
    }
}
