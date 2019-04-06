using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IRCWindow
{
    public partial class SettingsDialog : Form
    {
        public SettingsDialog()
        {
            InitializeComponent();
        }

        private void SettingsDialog_Load(object sender, EventArgs e)
        {
            treeView1.Nodes["nMessages"].Tag = pMessages;
            treeView1.Nodes["nColors"].Tag = pColors;
            treeView1.Nodes["nFonts"].Tag = pFonts;

            //tBChannelQuitMsg.DataBindings.Add("Text", MySettings.Options, "PartMessage");
            //tBServerQuitMsg.DataBindings.Add("Text", MySettings.Options, "QuitMessage");
            //tBChatFont.Text = MySettings.Options.ChatFont.Name;
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            (e.Node.Tag as Panel).BringToFront();
        }

        private void bConfigureChatFont_Click(object sender, EventArgs e)
        {
            FontDialog diag = new FontDialog();
            //diag.Font = MySettings.Options.ChatFont;
            //if (diag.ShowDialog() == DialogResult.OK)
            //{
            //    MySettings.Options.ChatFont = diag.Font;
            //    tBChatFont.Text = MySettings.Options.ChatFont.Name;
            //}
        }
    }
}
