using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using IRCWindow.Properties;

namespace IRCWindow
{
    public partial class CustomizationDialog : Form
    {
        public CustomizationDialog()
        {
            InitializeComponent();
            this.propertyGrid1.SelectedObject = Program.ProgramOptions;
        }

        private void bDefault_Click(object sender, EventArgs e)
        {
            UISettings.Default.Reset();
            this.propertyGrid1.SelectedObject = Program.ProgramOptions;
        }
    }
}
