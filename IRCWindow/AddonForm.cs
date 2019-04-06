using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IRCProviders;
using IRCWindow.Properties;
using CIRCe.Base;
using IRCWindow.ViewModel;

namespace IRCWindow
{
    public partial class AddonForm : Form
    {
        public AddonForm(IList<AddonSettings> list)
        {
            InitializeComponent();
            this.dataGridView1.AutoGenerateColumns = true;
            ((ISupportInitialize)this.bindingSource1).BeginInit();
            this.bindingSource1.DataSource = list;
            ((ISupportInitialize)this.bindingSource1).EndInit();
        }

        private void AddonForm_Load(object sender, EventArgs e)
        {
            this.dataGridView1.Columns["Name"].HeaderText = Resources.Addon;
            this.dataGridView1.Columns["Visibility"].HeaderText = Resources.MenuVisible;
            this.dataGridView1.Columns["StartMode"].Visible = false;

            var col = new DataGridViewComboBoxColumn();
            col.Name = "StartMode2";
            col.ValueType = typeof(AddonStartMode);
            col.DataSource = Enum.GetValues(typeof(AddonStartMode));
            col.DataPropertyName = "StartMode";
            col.HeaderText = Resources.StartMode;
            this.dataGridView1.Columns.Add(col);

            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                this.dataGridView1.Rows[i].Cells["Visibility"].ReadOnly = (AddonStartMode)this.dataGridView1.Rows[i].Cells["StartMode2"].Value == AddonStartMode.None;
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridView1.Columns[e.ColumnIndex].Name == "StartMode2" && e.RowIndex > -1)
            {
                if (this.dataGridView1.Rows[e.RowIndex].Cells["StartMode"].Value is AddonStartMode &&
                    (AddonStartMode)this.dataGridView1.Rows[e.RowIndex].Cells["StartMode"].Value != AddonStartMode.None)
                    this.dataGridView1.Rows[e.RowIndex].Cells["Visibility"].ReadOnly = false;
                else
                {
                    this.dataGridView1.Rows[e.RowIndex].Cells["Visibility"].ReadOnly = true;
                    this.dataGridView1.Rows[e.RowIndex].Cells["Visibility"].Value = false;
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridView1.Columns[e.ColumnIndex].Name == "StartMode2")
                this.dataGridView1.BeginEdit(true);
        }

        private void AddonForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Escape)
            {
                e.Handled = true;
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void AddonForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.dataGridView1.Focus();
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
    }
}
