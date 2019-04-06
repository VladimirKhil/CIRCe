using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using IRCConnection;
using IRCProviders;
using IRCWindow.Properties;
using IRC.Client.Base;

namespace IRCWindow
{
    /// <summary>
    /// Список каналов
    /// </summary>
    public partial class ChannelViewForm : IRCForm
    {
        public event Action<string> NeedJoin;

        public ChannelViewForm(List<ChannelInfo> dataSource)
        {
            InitializeComponent();

            this.dataGridView1.Columns[2].CellTemplate = new MultiColorCell();

            foreach (var item in dataSource)
            {
                this.dataGridView1.Rows.Add(item.Name, item.UserNum, new FormattedText(item.Topic, 1, 99));
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (NeedJoin != null && e.RowIndex > -1)
                NeedJoin(this.dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
        }

        #region IVisual Members

        public void Deactivated()
        {
            
        }

        #endregion
    }
}
