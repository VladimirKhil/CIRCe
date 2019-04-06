using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SIIRC
{
    public partial class PackageStoreForm : Form
    {
        public SIService.Package SelectedValue
        {
            get
            {
                var currentTab = this.tbControl.SelectedTab;
                var listBox = (ListBox)currentTab.Controls[0];
                return (SIService.Package)listBox.SelectedItem;
            }
        }

        public PackageStoreForm()
        {
            InitializeComponent();
        }

        public PackageStoreForm(Dictionary<SIService.PackageCategory, SIService.Package[]> data) : this()
        {
            foreach (var item in data)
            {
                var tabPage = new TabPage(item.Key.Name) { Tag = item.Key.ID };
                var listBox = new ListBox { Dock = DockStyle.Fill, DisplayMember = "Description" };
                listBox.MouseDoubleClick += lbPackages_MouseDoubleClick;
                tabPage.Controls.Add(listBox);
                foreach (var package in item.Value)
                {
                    listBox.Items.Add(package);
                }

                if (listBox.Items.Count > 0)
                    listBox.SelectedIndex = 0;
                this.tbControl.TabPages.Add(tabPage);
            }
        }

        private void lbPackages_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var listBox = (ListBox)sender;
            if (listBox.SelectedItem != null)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }
    }
}
