using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IRCProviders;
using System.IO;

namespace IRCWindow
{
    internal partial class MultimediaForm : IRCForm
    {
        private MDIParent main = null;

        public MultimediaForm(MDIParent main)
        {
            InitializeComponent();

            this.main = main;

            var info = Program.DataStorage.GetDirectoryInfo("Media");
            var node = new TreeNode(info.Name);
            node.Tag = info;
            this.treeView1.Nodes.Add(node);
            Fill(node, info);
            this.treeView1.ExpandAll();

            var active = main.ActiveIRCWindow as MDIChildChannel;
            if (active != null)
            {
                this.comboBox1.Items.Add(active);
                this.comboBox1.SelectedItem = active;
            }
        }

        private void Fill(TreeNode treeNode, System.IO.DirectoryInfo info)
        {
            foreach (var dirInfo in info.GetDirectories())
            {
                var node = new TreeNode(dirInfo.Name);
                node.Tag = dirInfo;
                treeNode.Nodes.Add(node);
                Fill(node, dirInfo);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.listView1.Clear();

            foreach (var info in ((DirectoryInfo)e.Node.Tag).GetFiles())
            {
                this.listView1.Items.Add(info.Name);
            }
        }

        private string GetCurrentFile()
        {
            var node = treeView1.SelectedNode;
            if (node == null)
                return null;
            var dir = ((DirectoryInfo)node.Tag).FullName;
            var items = this.listView1.SelectedItems;
            if (items.Count == 0)
                return null;
            return Path.Combine(dir, items[0].Text);
        }

        private void bTest_Click(object sender, EventArgs e)
        {
            if (this.comboBox1.SelectedIndex == -1)
                return;

            var window = (MDIChildCommunication)this.comboBox1.SelectedItem;

            var items = this.listView1.SelectedItems;
            if (items.Count == 0)
                return;
            var file = items[0].Text;

            var command = Special.Color + "0play " + file;

            if (this.tbLoop.Checked)
                command += ":loop";
            else if (this.nudPos.Value > 0)
                command += ":" + this.nudPos.Value;

            window.ServerWindow.Send(window.WindowName, command, false);
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var file = GetCurrentFile();
            if (file == null)
                return;

            string result = this.main.Play(file, 0);
            if (result.Length > 0)
                MessageBox.Show(result);
        }

        private void bStop_Click(object sender, EventArgs e)
        {
            this.main.Play(string.Empty, 0);
            this.main.Play(string.Empty, 1);
        }

        private void MultimediaForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            this.comboBox1.Items.Clear();
            foreach (var server in this.main.GetServerWindows())
            {
                foreach (var channel in server.Channels)
                {
                    int pos = this.comboBox1.Items.Add(channel);
                }
            }
        }
    }
}
