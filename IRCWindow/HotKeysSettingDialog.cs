using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IRCProviders;
using IRCWindow.Properties;
using IRCWindow.Data;

namespace IRCWindow
{
    public partial class HotKeysSettingDialog : Form
    {
        public HotKeysSettingDialog()
        {
            InitializeComponent();

            foreach (var pair in UserOptions.Default.HotKeys)
            {
                dataGridView1.Rows.Add(pair.Key, pair.Value);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
                UserOptions.Default.HotKeys[(Keys)row.Cells[0].Value] = row.Cells[1].Value as string;
        }
    }
}
