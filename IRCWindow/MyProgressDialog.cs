using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IRCWindow
{
    public partial class MyProgressDialog : Form
    {
        public int Value
        {
            get { return this.progressBar1.Value; }
            set { this.progressBar1.Value = value; }
        }

        public MyProgressDialog()
        {
            InitializeComponent();
        }
    }
}
