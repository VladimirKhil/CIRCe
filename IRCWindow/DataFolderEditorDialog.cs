using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IRCWindow.Properties;

namespace IRCWindow
{
    public partial class DataFolderEditorDialog : Form
    {
        public bool NewUse { get { return this.radioButton1.Checked; } }
        public string Folder { get { return NewUse ? Program.DefaultDataFolder() : this.textBox1.Text; } }

        public bool Change { get { return Settings.Default.UseAppDataFolder != NewUse || !NewUse && Folder.Length > 0 && Settings.Default.DataFolder != Folder; } }
        public CheckState ChangeState { get { return Change ? CheckState.Checked : CheckState.Unchecked; } }

        public bool NeedMove { get { return this.cbMoveData.Checked; } }

        public DataFolderEditorDialog()
        {
            InitializeComponent();
        }

        private void DataEditorDialog_Load(object sender, EventArgs e)
        {
            this.radioButton2.Checked = !Settings.Default.UseAppDataFolder;
            this.textBox1.DataBindings.Add("Enabled", this.radioButton2, "Checked");
            this.bSelectDir.DataBindings.Add("Visible", this.radioButton2, "Checked");
        }

        private void bSelectDir_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                this.textBox1.Text = folderBrowserDialog1.SelectedPath;
        }
    }
}
