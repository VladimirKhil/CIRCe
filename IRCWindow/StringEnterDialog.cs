using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IRCProviders;

namespace IRCWindow
{
    public partial class StringEnterDialog : Form
    {
        public string PrintedText
        {
            get { return textBox1.Text; }
        }

        public StringEnterDialog(string title)
        {
            InitializeComponent();

            this.Text = title;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
