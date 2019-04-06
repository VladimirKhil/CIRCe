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
    public partial class LinksViewForm : IRCForm
    {
        public LinksViewForm(DataTable data)
        {
            InitializeComponent();

            foreach (DataRow row in data.Rows)
            {
                this.dataGridView1.Rows.Add(row.ItemArray);
            }
        }

        #region IVisual Members

        public void Deactivated()
        {
            
        }

        #endregion
    }
}
