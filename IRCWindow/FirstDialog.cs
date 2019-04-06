using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IRCWindow
{
    /// <summary>
    /// Диалог приветствия при первом входе в Цирцею
    /// </summary>
    public partial class FirstDialog : Form
    {
        internal string Nick { get { return this.textBox1.Text; } }

        public FirstDialog()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
