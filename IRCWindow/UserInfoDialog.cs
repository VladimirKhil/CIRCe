using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IRCProviders;
using IRCConnection;

namespace IRCWindow
{
    public partial class UserInfoDialog : Form
    {
        public UserInfoDialog()
        {
            InitializeComponent();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var createParams = base.CreateParams;
                createParams.Style &= ~(int)Win32.WS_CAPTION;
                createParams.Style |= (int)Win32.WS_POPUP;
                return createParams;
            }
        }

        public void UseObject()
        {
            this.userInfoPanel1.UseObject();
        }

        public void UseObject(UserInfo userInfo)
        {
            this.userInfoPanel1.UseObject(userInfo);
        }
    }
}
