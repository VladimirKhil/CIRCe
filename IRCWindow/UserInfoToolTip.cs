using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using IRCConnection;
using IRCWindow.Properties;

namespace IRCWindow
{
    public partial class UserInfoToolTip : ToolTip
    {
        UserInfoPanel panel = new UserInfoPanel();

        public Size Size { get { return this.panel.Size; } }

        public UserInfoToolTip()
        {
            InitializeComponent();
        }

        public void UseObject(UserInfo info)
        {
            this.panel.UseObject(info);
        }

        public void UseObject()
        {
            this.panel.UseObject();
        }

        private void UserInfoToolTip_Draw(object sender, DrawToolTipEventArgs e)
        {
            if (e.ToolTipText == "wait")
            {
                e.Graphics.FillRectangle(Brushes.Lavender, e.Bounds);
                TextRenderer.DrawText(e.Graphics, Resources.WaitingForUserInfo, this.panel.Font, e.Bounds, Color.Black, TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
            }
            else
            {
                var b = new Bitmap(panel.Width, panel.Height);
                var grid = sender as ChannelUsersList;

                panel.DrawToBitmap(b, e.Bounds);
                e.Graphics.DrawImage(b, Point.Empty);
            }
            e.Graphics.DrawRectangle(Pens.Black, new Rectangle(0, 0, e.Bounds.Width - 1, e.Bounds.Height - 1));
        }

        private void UserInfoToolTip_Popup(object sender, PopupEventArgs e)
        {
            e.ToolTipSize = panel.Size;
        }
    }
}
