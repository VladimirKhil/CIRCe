using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using IRCConnection;
using IRCWindow.Properties;
using IRC.Client.Base;

namespace IRCWindow
{
    public partial class UserInfoPanel : UserControl
    {
        IRCConnection.UserInfo obj = null;

        public UserInfoPanel()
        {
            InitializeComponent();
        }
        
        public UserInfoPanel(IRCConnection.UserInfo userInfo)
        {
            InitializeComponent();

            UseObject(userInfo);
        }

        public void UseObject()
        {
            if (this.obj != null)
                UseObject(this.obj);
        }

        public void UseObject(IRCConnection.UserInfo userInfo)
        {
            this.obj = userInfo;
            this.label1.Text = userInfo.User.UserName + "@" + userInfo.User.Host;
            this.label3.Text = string.Format("{0}: {1}{2}{3}{4}",
                Resources.Nick,
                userInfo.Nick,
                userInfo.IsRegistered ? string.Format(" ({0})", Resources.IsRegistered) : string.Empty,
                userInfo.IsIRCOp ? string.Format(" ({0})", Resources.IRCOp) : string.Empty,
                userInfo.IsBot ? string.Format(" ({0})", Resources.Bot) : string.Empty);
            if (userInfo.Idle > 0)
                this.label2.Text = string.Format(Resources.IsIdleForSeconds, userInfo.Idle);
            else
                this.label2.Text = "";
            string name = Resources.RealName + ": " + userInfo.User.Name;
            if (this.mclName.Text != name)
                this.mclName.FormattedText = new FormattedText(name, 1, 99);
            this.label5.Text = userInfo.Server;
            this.label6.Text = userInfo.ServerInfo;
            this.label7.Text = userInfo.CodePage;

            this.label8.Text = Resources.Channels;
            this.label9.Text = string.Empty;

            foreach (KeyValuePair<string, char> pair in userInfo.Modes)
                this.label9.Text += (this.label9.Text.Length > 0 ? " " : "") + pair.Value + pair.Key;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.UseObject();
        }

        private void UserInfoPanel_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                timer1.Start();
            }
            else
            {
                timer1.Stop();
            }
        }
    }
}
