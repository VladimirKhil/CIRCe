using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIRCe.Base;
using IRC.Client.Base;
using System.Threading;

namespace IRCWindow.ViewModel
{
    internal class CIRCeItem : InfiniteMarshalByRefObject, ICIRCeItem
    {
        private IRCProviders.IRCForm form = null;
        private bool formClosed = false;

        internal IRCProviders.IRCForm Form
        {
            get { return this.form; }
        }

        void form_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            this.formClosed = true;
            this.form.FormClosed -= new System.Windows.Forms.FormClosedEventHandler(form_FormClosed);

            if (Closed != null)
                Closed();
        }

        public CIRCeItem(IRCProviders.IRCForm form)
        {
            this.form = form;
            this.form.FormClosed += new System.Windows.Forms.FormClosedEventHandler(form_FormClosed);
        }

        #region Члены ICIRCeItem

        public event Action Closed;

        public void Close()
        {
            if (this.form.InvokeRequired)
            {
                this.form.BeginInvoke(new Action(this.Close));
                return;
            }

            if (!this.formClosed)
                this.form.Close();
        }

        public void Activate()
        {
            if (this.form.InvokeRequired)
            {
                this.form.EndInvoke(this.form.BeginInvoke(new Action(this.Activate)));
                return;
            }
            
            this.form.Activate();
            this.form.BringToFront();
        }

        #endregion
    }
}
