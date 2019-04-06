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
    /// Диалог, возникающий при деинсталляции продукта
    /// </summary>
    public partial class UninstallDialog : Form
    {
        public bool DeleteLogs { get { return this.cbLogs.Checked; } }
        public bool DeleteAddons { get { return this.cbAddons.Checked; } }
        public bool DeleteMedia { get { return this.cbMedia.Checked; } }

        public UninstallDialog()
        {
            InitializeComponent();
        }

        private void UninstallDialog_Load(object sender, EventArgs e)
        {
            BringToFront();
            Activate();
        }
    }
}
