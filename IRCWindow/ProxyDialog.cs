using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace IRCWindow
{
    public partial class ProxyDialog : Form
    {
        internal bool Useproxy { get { return this.cbUseProxy.Checked; } }

        internal Uri Address
        {
            get
            {
                return new Uri(string.Format("http://{0}:{1}", this.tbAddress.Text, this.nudPort.Value));
            }
        }
        internal NetworkCredential Credentials
        {
            get
            {
                return new NetworkCredential()
                {
                    Domain = this.tbDomain.Text,
                    UserName = this.tbLogin.Text,
                    Password = this.tbPassword.Text
                };
            }
        }

        public ProxyDialog(bool useProxy, Uri address, NetworkCredential credential)
        {
            InitializeComponent();

            this.cbUseProxy.Checked = useProxy;
            if (address != null)
            {
                this.tbAddress.Text = address.Host;
                this.nudPort.Value = address.Port;
            }

            if (credential != null)
            {
                this.tbDomain.Text = credential.Domain;
                this.tbLogin.Text = credential.UserName;
                this.tbPassword.Text = credential.Password;
            }

            this.tbAddress.DataBindings.Add("Enabled", this.cbUseProxy, "Checked");
            this.nudPort.DataBindings.Add("Enabled", this.cbUseProxy, "Checked");
            this.tbDomain.DataBindings.Add("Enabled", this.cbUseProxy, "Checked");
            this.tbLogin.DataBindings.Add("Enabled", this.cbUseProxy, "Checked");
            this.tbPassword.DataBindings.Add("Enabled", this.cbUseProxy, "Checked");
        }
    }
}
